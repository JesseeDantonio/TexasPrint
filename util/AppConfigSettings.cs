namespace TexasPrint.util
{
    class AppConfigSettings
    {
        public required PrinterSettings Printer { get; set; }
        public required PrintSettings Print { get; set; }
        public required MonitoringSettings Monitoring { get; set; }
        public required SumatraSettings Sumatra { get; set; }
    }

}
