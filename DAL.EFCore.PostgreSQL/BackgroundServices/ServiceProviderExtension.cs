using DAL.EFCore.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static void AddBackgroundBackup(this IServiceCollection services, BackupService.Settings settings)
    {
        services.AddHostedService(provider => new BackupService(settings));
    }
}