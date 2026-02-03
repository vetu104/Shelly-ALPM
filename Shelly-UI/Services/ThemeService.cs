using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace Shelly_UI.Services;

public class ThemeService
{
    public void ApplyCustomAccent(Color accent)
    {
        var fluentTheme = Application.Current?.Styles.OfType<FluentTheme>().FirstOrDefault();
        if (fluentTheme != null)
        {
            if (fluentTheme.Palettes.TryGetValue(ThemeVariant.Dark, out var currentDark) &&
                currentDark is { } darkPalette)
            {
                darkPalette.Accent = accent;
                fluentTheme.Palettes[ThemeVariant.Dark] = darkPalette;
            }

            if (fluentTheme.Palettes.TryGetValue(ThemeVariant.Light, out var currentLight) &&
                currentLight is { } lightPalette)
            {
                lightPalette.Accent = accent;
                fluentTheme.Palettes[ThemeVariant.Light] = lightPalette;
            }
        }
    }
    
    public void ApplyLowChromeColor(Color accent)
    {
        var fluentTheme = Application.Current?.Styles.OfType<FluentTheme>().FirstOrDefault();
        if (fluentTheme != null)
        {
            if (fluentTheme.Palettes.TryGetValue(ThemeVariant.Dark, out var currentDark) &&
                currentDark is { } darkPalette)
            {
                darkPalette.ChromeLow = accent;
                fluentTheme.Palettes[ThemeVariant.Dark] = darkPalette;
            }

            if (fluentTheme.Palettes.TryGetValue(ThemeVariant.Light, out var currentLight) &&
                currentLight is { } lightPalette)
            {
                lightPalette.ChromeLow = accent;
                fluentTheme.Palettes[ThemeVariant.Light] = lightPalette;
            }
        }
    }
    
    public void ApplyAltHighColor(Color accent)
    {
        var fluentTheme = Application.Current?.Styles.OfType<FluentTheme>().FirstOrDefault();
        if (fluentTheme != null)
        {
            if (fluentTheme.Palettes.TryGetValue(ThemeVariant.Dark, out var currentDark) &&
                currentDark is { } darkPalette)
            {
                darkPalette.AltHigh = accent;
                fluentTheme.Palettes[ThemeVariant.Dark] = darkPalette;
            }

            if (fluentTheme.Palettes.TryGetValue(ThemeVariant.Light, out var currentLight) &&
                currentLight is { } lightPalette)
            {
                lightPalette.AltHigh = accent;
                fluentTheme.Palettes[ThemeVariant.Light] = lightPalette;
            }
        }
    }
    
    public void ApplySecondaryBackground(Color accent)
    {
        var fluentTheme = Application.Current?.Styles.OfType<FluentTheme>().FirstOrDefault();
        if (fluentTheme != null)
        {
            if (fluentTheme.Palettes.TryGetValue(ThemeVariant.Dark, out var currentDark) &&
                currentDark is { } darkPalette)
            {
                darkPalette.ChromeMedium = accent;
                fluentTheme.Palettes[ThemeVariant.Dark] = darkPalette;
            }

            if (fluentTheme.Palettes.TryGetValue(ThemeVariant.Light, out var currentLight) &&
                currentLight is { } lightPalette)
            {
                lightPalette.ChromeMedium = accent;
                fluentTheme.Palettes[ThemeVariant.Light] = lightPalette;
            }
        }
    }

    private void ApplyKdeTheme()
    {
        var configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config",
            "kdeglobals"
        );
    
        if (!File.Exists(configPath))
            return;
    
        var content = File.ReadAllText(configPath);
        var parser = new KdeThemeParser();
        parser.Parse(content);
    
       
    }
    
    private static Color ParseColor(string colorStr)
    {
        colorStr = colorStr.TrimStart('#');
        
        if (colorStr.Length == 6)
        {
            byte r = Convert.ToByte(colorStr.Substring(0, 2), 16);
            byte g = Convert.ToByte(colorStr.Substring(2, 2), 16);
            byte b = Convert.ToByte(colorStr.Substring(4, 2), 16);
            return Color.FromRgb(r, g, b);
        }
        
        return Color.FromRgb(0, 0, 0);
    }

    public void SetTheme(bool isDark)
    {
        if (Application.Current != null)
        {
            Application.Current.RequestedThemeVariant = isDark ? ThemeVariant.Dark : ThemeVariant.Light;
        }
    }
}

public class KdeThemeParser
{
    public Color WindowBackground { get; private set; }
    public Color WindowForeground { get; private set; }
    public Color ButtonBackground { get; private set; }
    public Color BaseBackground { get; private set; }
    public Color AlternateBase { get; private set; }
    public Color Highlight { get; private set; }
    public Color HighlightedText { get; private set; }
    public Color Link { get; private set; }
    public Color Text { get; private set; }
    public Color ButtonText { get; private set; }
    public Color DisabledText { get; private set; }
    
    public void Parse(string configContent)
    {
        var lines = configContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        Color[] paletteActive = null;
        Color[] paletteDisabled = null;
        
        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            
            if (trimmed.StartsWith("Palette\\active="))
            {
                var value = trimmed.Split(new[] { '=' }, 2)[1].Trim();
                paletteActive = value.Split(',').Select(s => ParseColor(s.Trim())).ToArray();
            }
            else if (trimmed.StartsWith("Palette\\disabled="))
            {
                var value = trimmed.Split(new[] { '=' }, 2)[1].Trim();
                paletteDisabled = value.Split(',').Select(s => ParseColor(s.Trim())).ToArray();
            }
        }
        
        // Extract specific colors from palette
        if (paletteActive != null && paletteActive.Length >= 22)
        {
            Text = paletteActive[6];              // Text color
            ButtonBackground = paletteActive[1];   // Button background
            ButtonText = paletteActive[8];         // Button text
            BaseBackground = paletteActive[9];     // Base/input background
            WindowBackground = paletteActive[10];  // Window background
            Highlight = paletteActive[12];         // Selection/highlight
            HighlightedText = paletteActive[13];   // Selected text
            Link = paletteActive[14];              // Link color
            AlternateBase = paletteActive[16];     // Alternate row color
        }
        
        if (paletteDisabled != null && paletteDisabled.Length >= 7)
        {
            DisabledText = paletteDisabled[6];     // Disabled text color
        }
        
        // Fallback to defaults if palette parsing failed
        if (paletteActive == null)
        {
            WindowBackground = Color.FromRgb(239, 240, 241);
            Text = Color.FromRgb(35, 38, 41);
            Highlight = Color.FromRgb(61, 174, 233);
        }
    }
    
    private static Color ParseColor(string colorStr)
    {
        colorStr = colorStr.TrimStart('#');
        
        if (colorStr.Length == 6)
        {
            byte r = Convert.ToByte(colorStr.Substring(0, 2), 16);
            byte g = Convert.ToByte(colorStr.Substring(2, 2), 16);
            byte b = Convert.ToByte(colorStr.Substring(4, 2), 16);
            return Color.FromRgb(r, g, b);
        }
        
        return Color.FromRgb(0, 0, 0);
    }
}