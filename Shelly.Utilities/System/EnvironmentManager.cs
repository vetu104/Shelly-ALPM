using Shelly.Utilities.Extensions;
using Shelly.Utilities.System.Enums;

namespace Shelly.Utilities.System;

public static class EnvironmentManager
{
    private const string DesktopEnvironmentVariable = "XDG_CURRENT_DESKTOP";

    public static string CreateWindowManagerVars()
    {
        switch (GetDesktopEnvironment())
        {
            case SupportedDesktopEnvironments.KDE:
            case SupportedDesktopEnvironments.GNOME:
            case SupportedDesktopEnvironments.XFCE:
            case SupportedDesktopEnvironments.Cinnamon:
            case SupportedDesktopEnvironments.MATE:
            case SupportedDesktopEnvironments.LXQt:
            case SupportedDesktopEnvironments.LXDE:
            case SupportedDesktopEnvironments.Budgie:
            case SupportedDesktopEnvironments.Pantheon:
            case SupportedDesktopEnvironments.COSMIC:
                return "";
            case SupportedDesktopEnvironments.Hyprland:
            case SupportedDesktopEnvironments.Sway:
            case SupportedDesktopEnvironments.Niri:
            case SupportedDesktopEnvironments.i3:
            case SupportedDesktopEnvironments.Unknown:
                return CreateWMLaunchVars();
            default:
                // Should never happen. If this does; get a drink and pray.
                throw new ArgumentOutOfRangeException();
        }
    }

    private static string CreateWMLaunchVars()
    {
        List<string> convertedVars = [];
        var envVars = EnumExtensions.ToNameList<WindowManagerEnvVariables>();
        convertedVars.AddRange(from envVar in envVars
            let value = Environment.GetEnvironmentVariable(envVar)
            where !string.IsNullOrEmpty(value)
            select $"{envVar}={value}");

        return convertedVars.Count > 0 ? $" {string.Join(" ", convertedVars)} " : "";
    }

    public static SupportedDesktopEnvironments GetDesktopEnvironment() =>
        Enum.TryParse<SupportedDesktopEnvironments>(Environment.GetEnvironmentVariable(DesktopEnvironmentVariable),
            true, out var result)
            ? result
            : SupportedDesktopEnvironments
                .Unknown;
}