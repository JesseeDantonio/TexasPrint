namespace TexasPrint.util
{
    class MonitoringSettings : PathSettingsBase
    {
        public required string Path { get; set; }
        public string FullPath => GetAbsolutePath(Path);
    }
}
