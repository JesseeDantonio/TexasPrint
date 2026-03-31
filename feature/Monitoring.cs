using TexasPrint.util;
namespace TexasPrint.feature
{
    class Monitoring(MonitoringSettings monitoringSettings, SumatraSettings sumatraSettings, PrinterSettings printerSettings, PrintSettings printSettings)
    {
        public void Start()
        {
            if (!Directory.Exists(monitoringSettings.FullPath)) Directory.CreateDirectory(monitoringSettings.FullPath);
            string pathCombin = Path.Combine(monitoringSettings.FullPath, "Failed");
            Directory.CreateDirectory(pathCombin);
            if (!Directory.Exists(pathCombin)) Directory.CreateDirectory(pathCombin);
            
            pathCombin = Path.Combine(monitoringSettings.FullPath, "Success");
            Directory.CreateDirectory(pathCombin);
            if (!Directory.Exists(pathCombin)) Directory.CreateDirectory(pathCombin);

            FileSystemWatcher SysWatcher = new()
            {
                Path = monitoringSettings.FullPath,

                // On surveille uniquement les fichiers PDF
                Filter = "*.pdf"
            };

            // On déclenche l'événement quand un fichier est créé ou copié
            SysWatcher.Created += OnFileCreated;

            SysWatcher.EnableRaisingEvents = true;
            Console.WriteLine($"Surveillance active sur : {monitoringSettings.FullPath}");
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Nouveau fichier détecté : {e.Name}");

            // IMPORTANT : Quand l'événement se déclenche, le fichier est peut-être encore en cours de copie.
            // Il faut attendre qu'il soit libéré.
            if (WaitForFile(e.FullPath))
            {
                if (printSettings.DeleteFile)
                {
                    TFile.PrintWithCleanup(e.FullPath, sumatraSettings, printerSettings, monitoringSettings);
                    return;
                }

                TFile.Print(e.FullPath, sumatraSettings, printerSettings, monitoringSettings);
            }
        }

        private static bool WaitForFile(string fullPath)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using FileStream stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.None);
                    return true; // Le fichier est prêt
                }
                catch (IOException)
                {
                    // Le fichier est verrouillé, on attend un peu
                    Thread.Sleep(500);
                }
            }
            return false;
        }
    }
}

