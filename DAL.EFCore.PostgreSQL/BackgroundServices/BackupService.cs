using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Hosting;

public class BackupService : BackgroundService
{
    private DockerClient _dockerClient;

    private Settings _settings;
    public struct Settings
    {
        public string DockerContainerId { get; set; }
        public string PostresUser { get; set; }
        public string PostgresPassword { get; set; }
        public string PostgresDatabase { get; set; }
        public string YandexToken { get; set; }
        public string BackupPath { get; set; }


        "9923bae4dd9f619321ab86e9c14aeb9040bbf262180da37e5dbf641b8ac9e972",
         "postgres",
          "secret",
           "shop",
            "y0_AgAAAABk2XcVAAsDqQAAAAD1RNOOt5Jsm5bvT7i4zkM3tB7d2Z-OWdE",
             "backupPath"
        
    }

    public BackupService(Settings settings)
    {
        _settings = settings;
        _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
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

        var checkCreatedDump = Task.Run(async () =>
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
                return;
            }

        });

        await checkCreatedDump;

        var result = await _dockerClient.Containers.GetArchiveFromContainerAsync(
            _settings.DockerContainerId,
            new GetArchiveFromContainerParameters { Path = backupFile }, false);

        using (var fileStream = new FileStream($"{backupFile}", FileMode.Create))
        {
            await result.Stream.CopyToAsync(fileStream);
        }

        await result.Stream.DisposeAsync();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Backup();
            await Task.Delay(60 * 60 * 1000, stoppingToken);
        }
    }
}