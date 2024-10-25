using System.IO;
using System.Windows;
using Microsoft.Win32;
// Make sure to reference System.Windows.Forms
using Application = System.Windows.Application;

namespace DarkModeSwitcher;

public partial class App : Application
{
    private NotifyIcon _notifyIcon;
    private ViewModel _viewModel;


    private void AddToStartup()
    {
        using (RegistryKey key =
               Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
        {
            if (key != null)
            {
                string? appName = ResourceAssembly.GetName().Name;
                if (key.GetValue(appName) == null)
                    key.SetValue(appName, ResourceAssembly.Location);
            }
        }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        AddToStartup();
        base.OnStartup(e);
        _viewModel = new ViewModel();
        _viewModel.IconPathChanged += IconPathChanged;
        CreateSystemTrayIcon();
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }


    private void CreateSystemTrayIcon()
    {
        _notifyIcon = new NotifyIcon
        {
            Icon = new Icon(_viewModel.IconPath),
            Visible = true,
            Text = "Windows Theme Switcher"
        };

        var contextMenu = new ContextMenuStrip();
        _notifyIcon.ContextMenuStrip = contextMenu;

        contextMenu.Renderer = new CustomToolStripRenderer(_viewModel);

        contextMenu.Items.Add("Light theme", Image.FromFile(_viewModel.SUN_ICON_PATH),
            (_, _) => { _viewModel.SetLightTheme(); });

        contextMenu.Items.Add("Dark theme", Image.FromFile(_viewModel.MOON_ICON_PATH),
            (_, _) => { _viewModel.SetDarkTheme(); });
    }

    private void IconPathChanged(object? sender, EventArgs e)
    {
        _notifyIcon.Icon = new Icon(_viewModel.IconPath);
    }
}