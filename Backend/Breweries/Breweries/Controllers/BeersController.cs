using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Breweries.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BeersController : ControllerBase
    {
        private readonly ILogger<BeersController> _logger;

        public BeersController(ILogger<BeersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(300);
            return Ok(new { Mensagem = "Toma uma cerveja!" });
        }
    }
}
