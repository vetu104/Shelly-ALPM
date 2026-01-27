using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PackageManager.Alpm;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shelly_CLI.Commands;

public class ListUpdatesCommand : Command
{
    public override int Execute([NotNull] CommandContext context)
    {
        using var manager = new AlpmManager();

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Initializing and syncing ALPM...", ctx => { manager.IntializeWithSync(); });

        var updates = manager.GetPackagesNeedingUpdate();

        if (updates.Count == 0)
        {
            AnsiConsole.MarkupLine("[green]All packages are up to date![/]");
            return 0;
        }

        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Current Version");
        table.AddColumn("New Version");
        table.AddColumn("Download Size");

        foreach (var pkg in updates.OrderBy(p => p.Name))
        {
            table.AddRow(
                pkg.Name,
                pkg.CurrentVersion,
                pkg.NewVersion,
                FormatSize(pkg.DownloadSize)
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"[yellow]{updates.Count} packages can be updated[/]");
        return 0;
    }

    private static string FormatSize(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB"];
        int order = 0;
        double size = bytes;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }

        return $"{size:0.##} {sizes[order]}";
    }
}
