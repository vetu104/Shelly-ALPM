using Spectre.Console;

namespace Shelly_CLI;

public static class PacmanKeyRunner
{
    public static int Run(string args)
    {
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "pacman-key",
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
