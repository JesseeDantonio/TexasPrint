using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using TexasPrint.util;

namespace TexasPrint.feature;

public class PrintWorker : BackgroundService
{
    static readonly List<Monitoring> monitorings = [];
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettingstest.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        TLog.Write("Service démarré");

        foreach (IConfigurationSection section in config.GetChildren())
        {
            var appConfig = section.Get<AppConfigSettings>();

            if (appConfig != null)
            {
                Monitoring monitoring = new(appConfig.Monitoring, appConfig.Sumatra, appConfig.Printer, appConfig.Print);

                if (monitoring != null)
                {
                    monitoring.Start();
                    monitorings.Add(monitoring);
                }
            }
        }

        TLog.Write($"Nombre de surveillances actives : {monitorings.Count}");

        if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
        {
            TLog.Write("Suppression des anciens logs...");
            string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

            foreach (string file in Directory.GetFiles(logDirectory))
            {
                try
                {
                    TFile.DeleteWithRetry(file);
                    TLog.Write($" -> Ancien log supprimé : {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    TLog.Write($"Impossible de supprimer le log : {ex.Message}");
                }
            }
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken); // Pause pour ne pas surcharger le CPU
        }
    }
}