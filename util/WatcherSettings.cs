namespace TexasPrint.util
{
    class WatcherSettings : PathSettingsBase
    {
        public required string Chemin { get; set; }
        public string FullChemin => GetAbsolutePath(Chemin);
    }
}
