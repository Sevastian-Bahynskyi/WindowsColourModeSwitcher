# Windows Colour Mode Switcher

## Overview

Windows Colour Mode Switcher is a simple application that allows users to switch between light and dark themes on Windows.  
The application updates the system theme and provides a system tray icon for easy access.
> _NOTE:_ The application doesn't magically change the theme of all applications.  
> However, a lot of applications support the sync with system theme which means they will change their theme automatically when the Windows theme mode is changed.
> This way you can achieve a system-wide theme change with a single click.

## Features

- Switch between light and dark themes
- System tray icon with context menu
- Automatic icon update based on the selected theme

## Installation

1. Download a zip file from the [Releases](https://github.com/Sevastian-Bahynskyi/WindowsColourModeSwitcher/releases) page.
2. Unzip the downloaded file.
3. Put the unzipped folder somewhere on your PC.

## Usage

1. Run the `WindowsColourModeSwitcher.exe` file.
2. The application will add itself to system startup.
3. Right-click the system tray icon to open the context menu.
4. Select "Light theme" or "Dark theme" to switch themes.

(While running an executable, you might get a window with a warning, just click `More info` then `Run anyway`.  
I need to buy a certificate to sign the application, but of course I am not going to do it for open-source project.)

When everything is done correctly, the application should start automatically with Windows and add itself to the system tray.

## Contributing

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -am 'Add new feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Create a new Pull Request.

## Issues

### Known issues

- If change of theme was triggered outside the application, the icon in the system tray will not update.
  - (didn't figure out how to add a listener for a Windows theme mode registry key)
- It is very likely that the application will not work on Windows versions older than Windows 10.
    - (didn't test it on older versions)

If you find any issues, please report them on the [Issues](https://github.com/Sevastian-Bahynskyi/WindowsColourModeSwitcher/issues) page.
If you have any suggestions or improvements you can contact me by email: `s.bahynskyi@gmail.com`

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.