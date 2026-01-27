namespace TexasPrint.util
{
    class SumatraSettings : PathSettingsBase
    {
        public required string PathExe { get; set; }
        public string FullPathExe => GetAbsolutePath(PathExe);
    }
}
