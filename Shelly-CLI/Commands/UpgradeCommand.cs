using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using PackageManager.Alpm;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shelly_CLI.Commands;

public class UpgradeSettings : CommandSettings
{
    [CommandOption("--no-confirm")]
    [Description("Skip confirmation prompt")]
    public bool NoConfirm { get; set; }
}

public class UpgradeCommand : Command<UpgradeSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] UpgradeSettings settings)
    {
        AnsiConsole.MarkupLine("[yellow]Performing full system upgrade...[/]");

        if (!settings.NoConfirm)
        {
            if (!AnsiConsole.Confirm("Do you want to proceed with system upgrade?"))
            {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
                return 0;
            }
        }

        using var manager = new AlpmManager();

        manager.Progress += (sender, args) =>
        {
            AnsiConsole.MarkupLine($"[blue]{args.PackageName}[/]: {args.Percent}%");
        };

        manager.Question += (sender, args) =>
        {
            if (settings.NoConfirm)
            {
                // Machine-readable format for UI integration
                Console.Error.WriteLine($"[Shelly][ALPM_QUESTION]{args.QuestionText}");
                Console.Error.Flush();
                var input = Console.ReadLine();
                args.Response = input?.Trim().Equals("y", StringComparison.OrdinalIgnoreCase) == true ? 1 : 0;
            }
            else
            {
                var response = AnsiConsole.Confirm($"[yellow]{args.QuestionText}[/]", defaultValue: true);
                args.Response = response ? 1 : 0;
            }
        };

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Initializing and syncing ALPM...", ctx => { manager.IntializeWithSync(); });

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Upgrading system...", ctx => { manager.SyncSystemUpdate(); });

        AnsiConsole.MarkupLine("[green]System upgraded successfully![/]");
        return 0;
    }
}
