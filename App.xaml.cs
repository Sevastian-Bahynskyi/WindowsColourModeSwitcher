using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;
using DarkModeSwitcher;
using Microsoft.Win32.TaskScheduler;
// Make sure to reference System.Windows.Forms
using Application = System.Windows.Application;

namespace WindowsColourModeSwitcher;

public partial class App : Application
{
    private NotifyIcon _notifyIcon;
    private ViewModel _viewModel;
    
    
    private void AddToStartup()
    {
        string taskName = "WindowsThemeSwitcher";
        var processModule = Process.GetCurrentProcess().MainModule;
        if (processModule != null)
        {
            string exePath = processModule.FileName;
            
            Logger.LogStart();
            Logger.Log($"Starting AddToStartup method.");
            Logger.Log($"Task Name: {taskName}");
            Logger.Log($"Executable Path: {exePath}");

            try
            {
                using (TaskService ts = new TaskService())
                {
                    Logger.Log("Created TaskService instance.");

                    TaskDefinition td = ts.NewTask();
                    Logger.Log("Created new TaskDefinition.");

                    td.RegistrationInfo.Description = "Starts the Windows Theme Switcher application.";
                    td.Principal.UserId = WindowsIdentity.GetCurrent().Name; // Run as the current user
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                    td.Principal.LogonType = TaskLogonType.InteractiveToken;

                    Logger.Log($"Setting up principal: {td.Principal.UserId}, RunLevel: {td.Principal.RunLevel}, LogonType: {td.Principal.LogonType}");

                    // Create a trigger that starts the task at logon
                    td.Triggers.Add(new LogonTrigger());
                    Logger.Log("Added LogonTrigger.");

                    // Create the action to run the application
                    td.Actions.Add(new ExecAction(exePath, null, null));
                    Logger.Log("Added ExecAction to the task.");

                    // Register the task in the root folder
                    ts.RootFolder.RegisterTaskDefinition(taskName, td);
                    Logger.Log("Task registered successfully.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in AddToStartup: {ex.Message}", LOG_TYPE.ERROR);
            }
            finally
            {
                Logger.Log("AddToStartup method completed.");
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
        
        SessionEnding += OnSessionEndingHandler;
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

    private void OnSessionEndingHandler(object? sender, SessionEndingCancelEventArgs e)
    {
        Logger.LogEnd();
        _notifyIcon.Dispose();
    }
}