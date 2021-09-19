using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NodeBotServer.Services;

namespace NodeBotServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolanaController : ControllerBase
    {
        const string RestartCommand = "sudo systemctl restart solana";

        [HttpPost]
        public async Task<IActionResult> Restart()
        {
            var result = await Executer.Run(RestartCommand);

            return Ok(result);
        }
    }
}
