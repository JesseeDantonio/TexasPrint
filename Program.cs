// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using TexasPrint.feature;
using TexasPrint.util;


namespace TexasPrint
{
    class Program
    {
        static readonly Monitoring[] monitorings = [];

        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            foreach (IConfigurationSection section in config.GetChildren())
            {
                var appConfig = section.Get<AppConfigSettings>();

                if (appConfig != null)
                {
                    Console.WriteLine($"{section.Key} initialisée");
                    Console.WriteLine($"{appConfig.Monitoring.FullPath}");
                    Monitoring monitoring = new(appConfig.Monitoring, appConfig.Sumatra, appConfig.Printer, appConfig.Print);

                    if (monitoring != null)
                    {
                        monitoring.Start();
                        monitorings.Append(monitoring);
                    }
                }
            }

            Console.WriteLine("Appuyez sur 'Enter' pour quitter.");
            Console.ReadLine();
        }
    }

}
