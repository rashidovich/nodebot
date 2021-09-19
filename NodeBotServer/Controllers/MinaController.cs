using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NodeBotServer.Services;
using Serilog;

namespace NodeBotServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinaController : ControllerBase
    {
        const string RestartContainer = "docker restart {name}";
        const string MinaContainer = "mina";

        [HttpPost]
        public async Task<IActionResult> Restart(string containerName)
        {
            Log.Information($"Restarting {containerName ?? MinaContainer}");

            var result = await Executer.Run(string.Format(RestartContainer, containerName ?? MinaContainer));

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var status = "docker exec -it mina mina client status";
            var result = await Executer.Run(status);

            return Ok(result);
        }        
    }
}
