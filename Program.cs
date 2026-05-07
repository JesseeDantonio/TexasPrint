// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TexasPrint.feature;

namespace TexasPrint
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices(services =>
            {
                // Enregistre le service d'arrière-plan
                services.AddHostedService<PrintWorker>();
            });

            var host = builder.Build();

            // 1. On démarre le service d'arrière-plan (sans bloquer le reste du code)
            await host.StartAsync();

            // 2. On prépare l'interface graphique pour l'icône
            ApplicationConfiguration.Initialize();

            // 3. On récupère le gestionnaire de cycle de vie pour pouvoir arrêter le service depuis l'icône
            var appLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

            // 4. On lance l'icône dans la zone de notification (ceci bloque jusqu'à ce qu'on clique sur "Arrêter")
            Application.Run(new TrayIconService(appLifetime));

            // 5. Quand l'icône est fermée, on s'assure que le service s'arrête proprement
            await host.StopAsync();
        }
    }

}
