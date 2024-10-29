using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Drawing.Color;

namespace DarkModeSwitcher;

public class ViewModel
{
    private readonly Color LIGHT_COLOR = Color.White;
    private readonly Color DARK_COLOR = Color.LightSlateGray;
    public string MOON_ICON_PATH { get; private set; } = "icons/full_moon.ico" ;
    public string SUN_ICON_PATH { get; private set; } = "icons/sun.ico";
    public Color BackgroundColor { get; private set; }
    public Color SelectedItemColor { get; private set; }
    private string _iconPath;
    public string IconPath
    {
        get => _iconPath;
        private set
        {
            if (_iconPath != value)
            {
                _iconPath = value;
                OnIconPathChanged();
            }
        }
    }
    public event EventHandler IconPathChanged;
    
    
    public ViewModel()
    {
        SUN_ICON_PATH = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SUN_ICON_PATH);
        MOON_ICON_PATH = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MOON_ICON_PATH);
        
        BackgroundColor = CurrentThemeIsLight() ? LIGHT_COLOR : DARK_COLOR;
        IconPath = CurrentThemeIsLight()? SUN_ICON_PATH: MOON_ICON_PATH;
        SelectedItemColor = Color.GreenYellow;
    }
    protected virtual void OnIconPathChanged()
    {
        IconPathChanged?.Invoke(this, EventArgs.Empty);
    }

    
    
    public void SetDarkTheme()
    {
        SetTheme(false);
        BackgroundColor = DARK_COLOR;
        IconPath = MOON_ICON_PATH;
    }
    
    public void SetLightTheme()
    {
        SetTheme(true);
        BackgroundColor = LIGHT_COLOR;
        IconPath = SUN_ICON_PATH;
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