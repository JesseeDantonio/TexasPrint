using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using TexasPrint.util;

namespace TexasPrint.feature;

public class PrintWorker : BackgroundService
{
    static readonly Monitoring[] monitorings = [];
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettingstest.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        foreach (IConfigurationSection section in config.GetChildren())
        {
            var appConfig = section.Get<AppConfigSettings>();

            if (appConfig != null)
            {
                Monitoring monitoring = new(appConfig.Monitoring, appConfig.Sumatra, appConfig.Printer, appConfig.Print);

                if (monitoring != null)
                {
                    monitoring.Start();
                    monitorings.Append(monitoring);
                }
            }
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken); // Pause pour ne pas surcharger le CPU
        }
    }
}