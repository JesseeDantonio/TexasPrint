using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace TexasPrint.feature;
public class TrayIconService : ApplicationContext
{
    private readonly NotifyIcon _trayIcon;
    private readonly IHostApplicationLifetime _appLifetime;

    public TrayIconService(IHostApplicationLifetime appLifetime)
    {
        _appLifetime = appLifetime;

        // Création du menu du clic droit
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Redémarrer", null, OnRestartClicked);
        contextMenu.Items.Add("Arrêter", null, OnExitClicked);

        // Configuration de l'icône
        _trayIcon = new NotifyIcon()
        {
            // IMPORTANT : Mettre le chemin vers un vrai fichier .ico
            // ou utilise SystemIcons.Application pour tester
            Icon = SystemIcons.Application, 
            ContextMenuStrip = contextMenu,
            Visible = true,
            Text = "TexasPrint - Service en cours"
        };
    }

    private void OnRestartClicked(object? sender, EventArgs e)
    {
        // Pour redémarrer, on lance une nouvelle instance du programme...
        var exePath = Process.GetCurrentProcess().MainModule?.FileName;
        if (exePath != null)
        {
            Process.Start(exePath);
        }
        // ... et on ferme celle-ci
        ExitApplication();
    }

    private void OnExitClicked(object? sender, EventArgs e)
    {
        ExitApplication();
    }

    private void ExitApplication()
    {
        // Cache l'icône avant de quitter
        _trayIcon.Visible = false;
        _trayIcon.Dispose();

        // Demande au BackgroundService (le Host) de s'arrêter proprement
        // (C'est ce qui va mettre le stoppingToken.IsCancellationRequested à true !)
        _appLifetime.StopApplication();
        
        // Arrête la boucle graphique
        Application.Exit();
    }
}