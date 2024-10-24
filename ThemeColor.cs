using Microsoft.Win32;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace DarkModeSwitcher;

public class ThemeColor
{
    public Color backgroundColor { get; private set; }
    public Color selectedItemColor { get; private set; }
    private readonly Color LIGHT_COLOR = Color.White;
    private Color DARK_COLOR = Color.LightSlateGray;

    public ThemeColor()
    {
        backgroundColor = CurrentThemeIsLight() ? LIGHT_COLOR : DARK_COLOR;
        selectedItemColor = Color.GreenYellow;
    }
    
    public void SetDarkTheme()
    {
        SetTheme(false);
        backgroundColor = DARK_COLOR;
    }
    
    public void SetLightTheme()
    {
        SetTheme(true);
        backgroundColor = LIGHT_COLOR;
    }
    
    private void SetTheme(bool isLight)
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true))
        {
            if (key != null)
            {
                key.SetValue("AppsUseLightTheme", isLight ? 1 : 0, RegistryValueKind.DWord);
                key.SetValue("SystemUsesLightTheme", isLight ? 1 : 0, RegistryValueKind.DWord);
            }
        }
        
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Accent", true))
        {
            if (key != null)
            {
                // Set AccentColorMenu to 0 (or you can customize the value)
                key.SetValue("AccentColorMenu", isLight? 1: 0, RegistryValueKind.DWord);
            }
        }
    }

    private bool CurrentThemeIsLight()
    {
        using (RegistryKey key =
               Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
        {
            if (key != null)
            {
                object appsUseLightTheme = key.GetValue("AppsUseLightTheme");
                if (appsUseLightTheme != null && appsUseLightTheme is int)
                {
                    return (int)appsUseLightTheme == 1; // 1 indicates light mode
                }
            }
        }

        return false;
    }
}