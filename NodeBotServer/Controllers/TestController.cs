using Microsoft.AspNetCore.Mvc;

namespace NodeBotServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Test()
        {
            return Ok("{'response': 'ok'}");
        }
    }
}