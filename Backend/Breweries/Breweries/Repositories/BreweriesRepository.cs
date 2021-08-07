using Breweries.Interfaces;
using Breweries.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Breweries.Repositories
{
    public class BreweriesRepository : IBreweriesRepository
    {
        private const string restBreweriesUrl = "https://api.openbrewerydb.org/breweries?per_page=50";

        public BreweriesRepository()
        {

        }

        public async Task<IEnumerable<Brewery>> GetAllBreweries()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(restBreweriesUrl);

                var responseData = await response.Content.ReadAsStringAsync();

                var breweries = JsonConvert.DeserializeObject<List<Brewery>>(responseData);

                return breweries.OrderBy(x => Guid.NewGuid()).Take(10).ToList();
            }
        }

        public async Task<IEnumerable<Brewery>> GetAllBreweriesCached()
        {
            throw new NotImplementedException();
        }
    }
}
