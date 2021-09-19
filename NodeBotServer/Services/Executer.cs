using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NodeBotServer.Services
{
    public class Executer
    {
        internal static Task<string> Run(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                return Task.FromResult("");

            var escapedArgs = cmd.Replace("\"", "\\\"");

            Log.Information($"Executing: '{escapedArgs}'");

            var source = new TaskCompletionSource<string>();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Exited += (sender, args) =>
            {
                Log.Warning(process.StandardError.ReadToEnd());

                var output = process.StandardOutput.ReadToEnd();
                Log.Information(output);

                if (process.ExitCode == 0)
                {
                    source.SetResult(output);
                }
                else
                {
                    source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
                }

                process.Dispose();
            };

            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                Log.Error(e, "Command {} failed", cmd);
                source.SetException(e);
            }

            return source.Task;
        }
    }
}
