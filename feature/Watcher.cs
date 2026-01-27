
// using System;
using System.Diagnostics;
using TexasPrint.util;
// using System.IO;
// using System.Threading;
namespace TexasPrint.feature
{
    class Watcher
    {
        private PrinterSettings printerSettings;
        private WatcherSettings watcherSettings;
        private SumatraSettings sumatraSettings;

        public Watcher(PrinterSettings printerSettings, WatcherSettings watcherSettings, SumatraSettings sumatraSettings)
        {
            this.printerSettings = printerSettings;
            this.watcherSettings = watcherSettings;
            this.sumatraSettings = sumatraSettings;
        }

        public void Start()
        {
            if (!Directory.Exists(watcherSettings.Chemin)) Directory.CreateDirectory(watcherSettings.Chemin);

            FileSystemWatcher SysWatcher = new()
            {
                Path = watcherSettings.Chemin,

                // On surveille uniquement les fichiers PDF
                Filter = "*.pdf"
            };

            // On déclenche l'événement quand un fichier est créé ou copié
            SysWatcher.Created += OnFileCreated;

            SysWatcher.EnableRaisingEvents = true;

            Console.WriteLine($"Surveillance active sur : {watcherSettings.Chemin}");
            Console.WriteLine("Appuyez sur 'Enter' pour quitter.");
            Console.ReadLine();
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Nouveau fichier détecté : {e.Name}");

            // IMPORTANT : Quand l'événement se déclenche, le fichier est peut-être encore en cours de copie.
            // Il faut attendre qu'il soit libéré.
            if (WaitForFile(e.FullPath))
            {
                PrintFile(e.FullPath);
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

        private void PrintFile(string fichier)
        {
            try
            {
                Process p = new();
                p.StartInfo.FileName = sumatraSettings.CheminExe;

                // Arguments pour impression silencieuse via SumatraPDF
                // -print-to "Nom" ou -print-to-default
                string args = $"-silent -print-to \"{printerSettings.Name}\" \"{fichier}\"";
                if (string.IsNullOrEmpty(printerSettings.Name)) args = $"-silent -print-to-default \"{fichier}\"";

                p.StartInfo.Arguments = args;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.Start();

                Console.WriteLine($" -> Commande d'impression envoyée pour {Path.GetFileName(fichier)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }
}

