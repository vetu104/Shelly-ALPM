using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using PackageManager.Flatpak;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Shelly_CLI;

public class Flatpak
{
// Helper class for running pacman-key commands
    public static class FlatpakRunner
    {
        public static int Run(string args)
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "flatpak",
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null) AnsiConsole.WriteLine(e.Data);
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null) AnsiConsole.MarkupLine($"[red]{Markup.Escape(e.Data)}[/]");
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return process.ExitCode;
        }
    }

    public class FlatpakSetting : CommandSettings
    {
        [CommandArgument(0, "<packages>")]
        [Description("Package name to operate on")]
        public string Packages { get; set; } = string.Empty;
    }

    public class FlatpakSearchCommand : Command<FlatpakSetting>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] FlatpakSetting settings)
        {
            AnsiConsole.MarkupLine("[yellow]Searching flatpak...[/]");
            var result = FlatpakRunner.Run("search" + settings.Packages.Aggregate("", (a, b) => a + " " + b));

            return result;
        }
    }

    public class FlatpakInstallCommand : Command<FlatpakSetting>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] FlatpakSetting settings)
        {
            AnsiConsole.MarkupLine("[yellow]Installing flatpak app...[/]");
            //var result = FlatpakManager.InstallApp("Flathub", settings.Packages);

            AnsiConsole.MarkupLine("[yellow]Installed: " + "");
            
            return 0;
        }
    }

    public class FlatpakRemoveCommand : Command<FlatpakSetting>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] FlatpakSetting settings)
        {
            AnsiConsole.MarkupLine("[yellow]Removing flatpak app...[/]");
            var result = FlatpakRunner.Run("remove" + settings.Packages.Aggregate("", (a, b) => a + " " + b));

            return result;
        }
    }

    public class FlatpakUpdateCommand : Command<FlatpakSetting>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] FlatpakSetting settings)
        {
            AnsiConsole.MarkupLine("[yellow]Updating flatpak app...[/]");
            var result = FlatpakRunner.Run("update" + settings.Packages.Aggregate("", (a, b) => a + " " + b));

            return result;
        }
    }

    public class FlatpakListCommand : Command
    {
        public override int Execute([NotNull] CommandContext context)
        {
            var manager = new FlatpakManager();

            var packages = manager.SearchInstalled();

            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn("Id");
            table.AddColumn("Version");
            table.AddColumn("Arch");
            table.AddColumn("Branch");
            table.AddColumn("Summary");

            foreach (var pkg in packages.OrderBy(p => p.Id))
            {
                table.AddRow(
                    pkg.Name,
                    pkg.Id,
                    pkg.Version,
                    pkg.Arch,
                    pkg.Version,
                    pkg.Summary.EscapeMarkup().Truncate(50)
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"[blue]Total: packages[/]");
            return 0;
        }
    }

    public class FlatpakRunCommand : Command<FlatpakSetting>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] FlatpakSetting settings)
        {
            AnsiConsole.MarkupLine("[yellow]Running selected flatpak app...[/]");
            var result = new FlatpakManager().LaunchApp(settings.Packages);

            if (result)
            {
                AnsiConsole.MarkupLine("[green]App launched successfully[/]");
                return 0;
            }

            AnsiConsole.MarkupLine("[red]Failed to launch app[/]");
            return 1;
        }
    }

    public class FlatpakRunningCommand : Command
    {
        public override int Execute([NotNull] CommandContext context)
        {
            AnsiConsole.MarkupLine("[yellow]Currently running flatpack instances on machine...[/]");
            var result = new FlatpakManager().GetRunningInstances();
            
            if (result.Count > 0)
            {
                var table = new Table();
                table.AddColumn("Id");
                table.AddColumn("Pid");

                foreach (var pkg in result.OrderBy(pkg => pkg.Pid))
                {
                    table.AddRow(
                        pkg.AppId,
                        pkg.Pid.ToString() 
                    );
                }
                AnsiConsole.Write(table);
                return 0;
            }

            AnsiConsole.MarkupLine("[green]No instances running[/]");
            return 0;
        }
    }

    public class FlatpakKillCommand : Command<FlatpakSetting>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] FlatpakSetting settings)
        {
            AnsiConsole.MarkupLine("[yellow]Killing selected flatpak app...[/]");
            var result = new FlatpakManager().KillApp(settings.Packages);
            
            AnsiConsole.MarkupLine("[red]" + result + "[/]");
            
          

            return  0;
        }
    }
}