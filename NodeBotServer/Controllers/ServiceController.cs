using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NodeBotServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecast : ControllerBase
    {
        private readonly ILogger<WeatherForecast> _logger;

        public WeatherForecast(ILogger<WeatherForecast> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Execute(string command)
        {
            var result = await Executer(command);
            return Ok(result);
        }

        private Task<string> Executer(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
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
                _logger.LogWarning(process.StandardError.ReadToEnd());

                var output = process.StandardOutput.ReadToEnd();
                _logger.LogInformation(output);

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
                _logger.LogError(e, "Command {} failed", cmd);
                source.SetException(e);
            }

            return source.Task;
        }
    }
}
