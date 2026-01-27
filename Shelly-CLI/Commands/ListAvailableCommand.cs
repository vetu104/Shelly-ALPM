using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PackageManager.Alpm;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shelly_CLI.Commands;

public class ListAvailableCommand : Command
{
    public override int Execute([NotNull] CommandContext context)
    {
        using var manager = new AlpmManager();

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Initializing and syncing ALPM...", ctx => { manager.IntializeWithSync(); });

        var packages = manager.GetAvailablePackages();

        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Version");
        table.AddColumn("Repository");
        table.AddColumn("Description");

        foreach (var pkg in packages.OrderBy(p => p.Name).Take(100))
        {
            table.AddRow(
                pkg.Name,
                pkg.Version,
                pkg.Repository,
                pkg.Description.EscapeMarkup().Truncate(50)
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"[blue]Showing first 100 of {packages.Count} available packages[/]");
        return 0;
    }
}
