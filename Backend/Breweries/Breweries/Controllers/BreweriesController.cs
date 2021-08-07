using System.Collections.Generic;
using System.Threading.Tasks;
using Breweries.Interfaces;
using Breweries.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Breweries.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreweriesController : ControllerBase
    {
        private readonly ILogger<BreweriesController> _logger;
        private readonly IBreweriesRepository _breweriesRepository;

        public BreweriesController(ILogger<BreweriesController> logger, IBreweriesRepository breweriesRepository)
        {
            _logger = logger;
            _breweriesRepository = breweriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(bool cached = true)
        {
            IEnumerable<Brewery> breweries;
            if (cached)
            {
                breweries = await _breweriesRepository.GetAllBreweriesCached();
            }
            else
            {
                breweries = await _breweriesRepository.GetAllBreweries();
            }
            return Ok(breweries);
        }
    }
}
