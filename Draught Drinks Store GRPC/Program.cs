using Draught_Drinks_Store_GRPC.Services.v1;


//y0_AgAAAABk2XcVAAsDqQAAAAD1RNOOt5Jsm5bvT7i4zkM3tB7d2Z - OWdE

using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Timers;
using System.IO.Pipes;
using YandexDisk.Client.Http;
using YandexDisk.Client;
using YandexDisk.Client.Clients;

public class DatabaseBackupService
{
    private System.Timers.Timer _timer;
    private DockerClient _dockerClient;

    public DatabaseBackupService()
    {
        _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
    }

    public void Start()
    {
        _timer = new(60 * 60 * 1000); // 1 hour
        _timer.Elapsed += async (sender, e) => await BackupDatabaseToYandexDisk("9923bae4dd9f619321ab86e9c14aeb9040bbf262180da37e5dbf641b8ac9e972", "postgres", "secret", "shop", "y0_AgAAAABk2XcVAAsDqQAAAAD1RNOOt5Jsm5bvT7i4zkM3tB7d2Z-OWdE", "backupPath");
        _timer.Start();
    }

    public void Stop()
    {
        _timer?.Stop();
        _timer?.Dispose();
    }

    public async Task BackupDatabaseToYandexDisk(string containerId, string username, string password, string database, string yandexToken, string backupPath)
    {
        string backupFile = Path.Combine($"{database}_{DateTime.Now:yyyy.MM.dd.HHmmss}.bak");

        // �������� ��������� ����� ���� ������
        var execConfig = new ContainerExecCreateParameters()
        {
            AttachStdout = true,
            AttachStderr = true,

            Cmd = new List<string> { "pg_dump", $"--file={backupFile}", $"--dbname=postgresql://{username}:{password}@localhost/{database}" }
        };

        var execId = await _dockerClient.Exec.ExecCreateContainerAsync(containerId, execConfig);

        await _dockerClient.Exec.StartContainerExecAsync(execId.ID);

        bool copy = false;
        while (!copy) {
           var inspectResult =  await _dockerClient.Exec.InspectContainerExecAsync(execId.ID);
            if (inspectResult.Running) {
                await Task.Delay(300);
                continue;
            }

            var result = await _dockerClient.Containers.GetArchiveFromContainerAsync(containerId, new GetArchiveFromContainerParameters { Path = backupFile }, false);

            using (var fileStream = new FileStream($"{backupFile}", FileMode.Create))
            {
                await result.Stream.CopyToAsync(fileStream);
            }

            await result.Stream.DisposeAsync();
            copy = true;
        }


        string oauthToken = "y0_AgAAAABk2XcVAAsEKQAAAAD1TjOIXG_QaHMBRsGbhjc9Qiqz5dxQGEM";
        IDiskApi diskApi = new DiskHttpApi(oauthToken);

        //Upload file from local
        await diskApi.Files.UploadFileAsync(path: "backups/" + backupFile,
                                            overwrite: true,
                                            localFile: backupFile,
                                            cancellationToken: CancellationToken.None);
        File.Delete(backupFile);

        diskApi.Dispose();
    }
}


internal class Program
{
    private async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services.AddGrpc();

        builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin();
        }));

        var backupSvc = new DatabaseBackupService();
        await backupSvc.BackupDatabaseToYandexDisk("9923bae4dd9f619321ab86e9c14aeb9040bbf262180da37e5dbf641b8ac9e972", "postgres", "secret", "shop", "y0_AgAAAABk2XcVAAsDqQAAAAD1RNOOt5Jsm5bvT7i4zkM3tB7d2Z-OWdE", "backupPath");


        var app = builder.Build();
        app.UseCors();
        app.MapGrpcService<CategoryService>().EnableGrpcWeb().RequireCors("AllowAll");
        app.MapGrpcService<ProductService>().EnableGrpcWeb().RequireCors("AllowAll");

        // Configure the HTTP request pipeline.
        //app.MapGrpcService<GreeterService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}