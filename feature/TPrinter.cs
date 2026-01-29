using TexasPrint.util;

class TPrinter
{
    public static bool IsExist(PrinterSettings printerSettings)
    {
            // On parcourt les imprimantes installées sur Windows
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                // Comparaison insensible à la casse (majuscule/minuscule)
                if (string.Equals(printer, printerSettings.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        return false;
    }
}