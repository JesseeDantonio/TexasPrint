namespace TexasPrint.util
{
    class SumatraSettings : PathSettingsBase
    {
        public required string CheminExe { get; set; }
        public string FullCheminExe => GetAbsolutePath(CheminExe);
    }
}
