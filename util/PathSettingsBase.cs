namespace TexasPrint.util
{
    public abstract class PathSettingsBase
    {
        // Méthode utilitaire protégée pour convertir un chemin
        protected static string GetAbsolutePath(string rawPath)
        {
            if (string.IsNullOrWhiteSpace(rawPath))
                return string.Empty;

            // Path.GetFullPath gère automatiquement :
            // - Les chemins absolus (C:\...) -> restent tels quels
            // - Les chemins relatifs (.\dossier) -> deviennent C:\...\bin\Debug\dossier
            return Path.GetFullPath(rawPath);
        }
    }
}
