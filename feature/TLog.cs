namespace TexasPrint.feature;

public class TLog
{
    private static readonly object _lockObj = new object();
    public static void Write(string message)
    {
        string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs.txt");
        string logEntry = $"{DateTime.Now} - {message}";

        try
        {
            // Le 'lock' met en file d'attente les autres appels à cette méthode.
            // Cela empêche deux threads d'écrire en même temps dans le même fichier.
            lock (_lockObj)
            {
                // StreamWriter avec append: true s'occupe de créer le fichier s'il manque.
                // L'utilisation de 'using' garantit que le fichier est bien fermé et libéré après l'écriture.
                using StreamWriter sw = new StreamWriter(logFilePath, append: true);
                sw.WriteLine(logEntry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'écriture dans le fichier de log: {ex.Message}");
        }
    }
}