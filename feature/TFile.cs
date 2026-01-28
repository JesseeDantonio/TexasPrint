using System.Diagnostics;
using TexasPrint.util;

namespace TexasPrint.feature;

class TFile()
{

    public static void PrintFile(string fichier, SumatraSettings sumatraSettings, PrinterSettings printerSettings)
    {
        try
        {
            Process p = new();
            p.StartInfo.FileName = sumatraSettings.FullPathExe;

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


    private static void DeleteFileWithRetry(string filePath)
    {
        int attempts = 0;
        while (attempts < 5)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($" -> Fichier supprimé : {filePath}");
                }
                return;
            }
            catch (IOException)
            {
                // Le fichier est probablement encore verrouillé par le système
                attempts++;
                Thread.Sleep(1000); // Attendre 1 seconde avant de réessayer
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Impossible de supprimer : {ex.Message}");
                return;
            }
        }
    }

    public static void PrintFileWithCleanup(string fichier, SumatraSettings sumatraSettings, PrinterSettings printerSettings)
    {
        // 1. Impression
        PrintFile(fichier, sumatraSettings, printerSettings);

        // 2. On planifie la suppression dans le futur
        // On ne bloque pas le thread principal
        Task.Run(async () =>
        {
            // On attend généreusement
            // Cela couvre le temps de chargement Sumatra + Spooler + Impression réseau
            await Task.Delay(TimeSpan.FromMinutes(1));

            DeleteFileWithRetry(fichier);
        });
    }
}