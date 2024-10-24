using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Win32;
using Application = System.Windows.Application;

namespace DarkModeSwitcher;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private NotifyIcon _notifyIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        CreateSystemTrayIcon();
    }
    
    private bool CurrentThemeIsLight()
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
        {
            if (key != null)
            {
                // Read the AppsUseLightTheme value
                object appsUseLightTheme = key.GetValue("AppsUseLightTheme");
                if (appsUseLightTheme != null && appsUseLightTheme is int)
                {
                    return (int)appsUseLightTheme == 1; // 1 indicates light mode
                }
            }
        }
        return false; // Default to dark mode if not found
    }

    private void CreateSystemTrayIcon()
    {
        _notifyIcon = new NotifyIcon
        {
            Icon = new Icon("icons/moon.ico"), // Add an .ico file in your project resources
            Visible = true,
            Text = "Windows Theme Switcher"
        };

        // Add context menu for tray icon
        var contextMenu = new ContextMenuStrip();
        _notifyIcon.ContextMenuStrip = contextMenu;
        contextMenu.BackColor = CurrentThemeIsLight() ? Color.White: Color.LightSlateGray;
        
        _notifyIcon.ContextMenuStrip.Items.Add("Light theme", Image.FromFile("icons/sun.ico"),
            (sender, args) => SwitchToLightTheme(sender, args, contextMenu));
        _notifyIcon.ContextMenuStrip.Items.Add("Dark theme", Image.FromFile("icons/full_moon.ico"),
            (sender, args) => SwitchToDarkTheme(sender, args, contextMenu));
    }

    private void SwitchToDarkTheme(object? sender, EventArgs e, ContextMenuStrip contextMenuStrip)
    {
        contextMenuStrip.BackColor = Color.LightSlateGray;
        SetTheme(false);
    }

    private void SwitchToLightTheme(object? sender, EventArgs e, ContextMenuStrip contextMenuStrip)
    {
        contextMenuStrip.BackColor = Color.White;
        SetTheme(true);
    }

    private void SetTheme(bool isLight)
    {
        using (RegistryKey key =
               Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true))
        {
            if (key != null)
            {
                key.SetValue("AppsUseLightTheme", isLight ? 1 : 0, RegistryValueKind.DWord);
                key.SetValue("SystemUsesLightTheme", isLight ? 1 : 0, RegistryValueKind.DWord);
            }
        }
    }
}