using System.Diagnostics;
using TexasPrint.util;
using PrinterSettings = TexasPrint.util.PrinterSettings;

namespace TexasPrint.feature;

class TFile()
{
    public static int Print(string fichier, SumatraSettings sumatraSettings, PrinterSettings printerSettings)
    {
        Process p = new();
        try
        {
            if (!string.IsNullOrEmpty(printerSettings.Name))
            {
                if (!TPrinter.IsExist(printerSettings))
                {
                    throw new ArgumentException($"L'imprimante demandée n'existe pas : '{printerSettings.Name}'");
                }         
            }

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

            if (p.WaitForExit(5000))
            {
                // Console.WriteLine($"{p.ExitCode}");
                if (p.ExitCode != 0)
                {
                    throw new Exception("Une erreur est survenue avec SumatraPDF.");
                }
            }

            return p.ExitCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            return -1;
        }
    }


    private static void DeleteWithRetry(string filePath)
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

    public static void PrintWithCleanup(string filePath, SumatraSettings sumatraSettings, PrinterSettings printerSettings)
    {
        // 1. Impression
        var exitCode = Print(filePath, sumatraSettings, printerSettings);

        if (exitCode != 0)
        {
            Console.WriteLine($"Impossible de supprimer : {filePath} suite à une erreur d'impression.");
            return;
        }

        // 2. On planifie la suppression dans le futur
        // On ne bloque pas le thread principal
        Task.Run(async () =>
        {
            // On attend généreusement
            // Cela couvre le temps de chargement Sumatra + Spooler + Impression réseau
            await Task.Delay(TimeSpan.FromMinutes(1));
            DeleteWithRetry(filePath);
        });
    }
}