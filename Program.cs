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

            var printerSettings = config.GetSection("Printer").Get<PrinterSettings>();
            var monitoringSettings = config.GetSection("Monitoring").Get<MonitoringSettings>();
            var sumatraSettings = config.GetSection("Sumatra").Get<SumatraSettings>();
            var printSettings = config.GetSection("Print").Get<PrintSettings>();

            if (printerSettings == null) { return; }
            if (monitoringSettings == null) { return; }
            if (sumatraSettings == null) { return; }
            if (printSettings == null) { return; }

            Monitoring monitoring = new(monitoringSettings, sumatraSettings, printerSettings, printSettings);

            if (monitoring == null) { return; }

            monitoring.Start();
        }
    }

}
