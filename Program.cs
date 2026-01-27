// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using TexasPrint.feature;
using TexasPrint.util;


namespace TexasPrint
{
    class Program
    {

        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            var printerSettings = config.GetSection("Imprimante").Get<PrinterSettings>();
            var watcherSettings = config.GetSection("Surveillance").Get<WatcherSettings>();
            var sumatraSettings = config.GetSection("Sumatra").Get<SumatraSettings>();

            if (printerSettings == null){return;}

            if (watcherSettings == null){return;}

            if (sumatraSettings == null){return;}

            Watcher watcher = new(printerSettings, watcherSettings, sumatraSettings);

            if (watcher == null) { return; }

            watcher.Start();
        }
    }

}
