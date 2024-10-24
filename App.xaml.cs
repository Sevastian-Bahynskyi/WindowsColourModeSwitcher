using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms; // Make sure to reference System.Windows.Forms
using Microsoft.Win32;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace DarkModeSwitcher;

public partial class App : Application
{
    private NotifyIcon _notifyIcon;
    
    private void Log(string message)
    {
        string logFilePath = "application_log.txt"; // Adjust path if needed
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (Exception ex)
        {
            // In case the log writing fails, write to console (visible in Debug mode)
            Console.WriteLine($"Log Error: {ex.Message}");
        }
    }

    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        CreateSystemTrayIcon();
        Log("Application started.");
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }


    private void CreateSystemTrayIcon()
    {
        var themeColor = new ThemeColor();
        
        _notifyIcon = new NotifyIcon
        {
            Icon = new Icon("icons/moon.ico"),
            Visible = true,
            Text = "Windows Theme Switcher"
        };

        var contextMenu = new ContextMenuStrip();
        _notifyIcon.ContextMenuStrip = contextMenu;
        
        contextMenu.Renderer = new CustomToolStripRenderer(themeColor);
        
        contextMenu.Items.Add("Light theme", Image.FromFile("icons/sun.ico"),
            (sender, args) => SwitchToLightTheme(sender, args, themeColor));
        contextMenu.Items.Add("Dark theme", Image.FromFile("icons/full_moon.ico"),
            (sender, args) => SwitchToDarkTheme(sender, args, themeColor));
        
        
    }

    private void SwitchToDarkTheme(object? sender, EventArgs e, ThemeColor themeColor)
    {
        themeColor.SetDarkTheme();
    }

    private void SwitchToLightTheme(object? sender, EventArgs e, ThemeColor themeColor)
    {
        themeColor.SetLightTheme();
    }

    
}
