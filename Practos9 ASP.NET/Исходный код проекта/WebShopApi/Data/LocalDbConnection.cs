using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;

namespace WebShopApi.Data;

internal static class LocalDbConnection
{
    public static async Task<string> ResolveAsync(string connectionString)
    {
        const string localDbPrefix = @"(localdb)\";
        var builder = new SqlConnectionStringBuilder(connectionString);
        if (!OperatingSystem.IsWindows() ||
            RuntimeInformation.ProcessArchitecture != Architecture.Arm64 ||
            !builder.DataSource.StartsWith(localDbPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return connectionString;
        }

        // Windows ARM64 cannot load the x64 LocalDB discovery library in-process.
        var instance = builder.DataSource[localDbPrefix.Length..];
        await RunLocalDbAsync("start", instance);
        var instanceInfo = await RunLocalDbAsync("info", instance);

        const string pipePrefix = @"np:\\.\pipe\";
        var pipeStart = instanceInfo.IndexOf(pipePrefix, StringComparison.OrdinalIgnoreCase);
        if (pipeStart < 0)
            throw new InvalidOperationException($"LocalDB did not report a named pipe for '{instance}'.");

        builder.DataSource = instanceInfo[pipeStart..].Split('\r', '\n')[0].Trim();
        return builder.ConnectionString;
    }

    private static async Task<string> RunLocalDbAsync(string command, string instance)
    {
        var startInfo = new ProcessStartInfo("sqllocaldb.exe")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        startInfo.ArgumentList.Add(command);
        startInfo.ArgumentList.Add(instance);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Could not start SQL Server LocalDB utility.");
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        try
        {
            await process.WaitForExitAsync(timeout.Token);
        }
        catch (OperationCanceledException) when (timeout.IsCancellationRequested)
        {
            if (!process.HasExited)
                process.Kill();

            throw new TimeoutException($"LocalDB '{command}' timed out for '{instance}'.");
        }

        if (process.ExitCode != 0)
            throw new InvalidOperationException($"LocalDB '{command}' failed for '{instance}': {(await output).Trim()} {(await error).Trim()}");

        return await output;
    }
}
