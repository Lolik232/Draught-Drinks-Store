using DAL.Abstractions.Entities;
using DAL.EFCore.PostgreSQL;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;

namespace DAL.EFCore.BackgroundServices;
public class BackupService : BackgroundService
{
    private DockerClient _dockerClient;

    private Settings _settings;

    private ShopContext _context;

    public struct Settings
    {
        public string DockerContainerId { get; set; }
        public string PostresUser { get; set; }
        public string PostgresPassword { get; set; }
        public string PostgresDatabase { get; set; }
        public string YandexToken { get; set; }
        public string BackupPath { get; set; }
    }

    public BackupService(Settings settings)
    {
        _settings = settings;
        _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
    }

    private async Task LoadJsonAndXmlToYandexDisk()
    {

        var products = _context.Products.ToList();


        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>));

        using (FileStream fs = new FileStream("products.xml", FileMode.OpenOrCreate))
        {
            xmlSerializer.Serialize(fs, products);
        }


        using (FileStream fs = new FileStream("products.json", FileMode.OpenOrCreate))
        {
            await JsonSerializer.SerializeAsync<List<Product>>(fs, products);
        }

        IDiskApi diskApi = new DiskHttpApi(_settings.YandexToken);

        //Upload file from local
        diskApi.Files.UploadFileAsync(path: _settings.BackupPath + "/toFiles/products.xml",
                                           overwrite: true,
                                           localFile: "products.xml",
                                           cancellationToken: CancellationToken.None);

        diskApi.Files.UploadFileAsync(path: _settings.BackupPath + "/toFiles/products.json",
                                            overwrite: true,
                                            localFile: "products.json",
                                            cancellationToken: CancellationToken.None);

        File.Delete("products.json");
        File.Delete("products.xml");

        diskApi.Dispose();
    }

    private async Task Backup()
    {
        string username = _settings.PostresUser, password = _settings.PostgresPassword,
         database = _settings.PostgresDatabase; ;

        string backupFile = Path.Combine($"{_settings.PostgresDatabase}_{DateTime.Now:yyyy.MM.dd.HHmmss}.bak");

        var execConfig = new ContainerExecCreateParameters()
        {
            AttachStdout = true,
            AttachStderr = true,
            Cmd = new List<string> { "pg_dump", $"--file={backupFile}", $"--dbname=postgresql://{username}:{password}@localhost/{database}" }
        };

        var execId = await _dockerClient.Exec.ExecCreateContainerAsync(_settings.DockerContainerId, execConfig);

        await _dockerClient.Exec.StartContainerExecAsync(execId.ID);

        var checkCreatedDumpTask = Task.Run(async () =>
        {
            bool copy = false;
            while (!copy)
            {
                var inspectResult = await _dockerClient.Exec.InspectContainerExecAsync(execId.ID);
                if (inspectResult.Running)
                {
                    await Task.Delay(300);
                    continue;
                }

                copy = true;
                await Task.CompletedTask;
            }
        });

        await checkCreatedDumpTask;

        var result = await _dockerClient.Containers.GetArchiveFromContainerAsync(
            _settings.DockerContainerId,
            new GetArchiveFromContainerParameters { Path = backupFile },
            false);

        using (var fileStream = new FileStream($"{backupFile}", FileMode.Create))
        {
            await result.Stream.CopyToAsync(fileStream);
        }

        await result.Stream.DisposeAsync();

        IDiskApi diskApi = new DiskHttpApi(_settings.YandexToken);

        //Upload file from local
        await diskApi.Files.UploadFileAsync(path: _settings.BackupPath + backupFile,
                                            overwrite: true,
                                            localFile: backupFile,
                                            cancellationToken: CancellationToken.None);
        File.Delete(backupFile);

        diskApi.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Backup();
                await LoadJsonAndXmlToYandexDisk();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Backup error: " + ex.Message);
            }
            await Task.Delay(60 * 60 * 1000, stoppingToken);
        }
    }
}