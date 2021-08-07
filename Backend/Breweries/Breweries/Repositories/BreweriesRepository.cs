using Breweries.Interfaces;
using Breweries.Model;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IDistributedCache _distributedCache;

        private const string restBreweriesUrl = "https://api.openbrewerydb.org/breweries?per_page=50";
        private const string BreweriesKey = "Breweries";

        public BreweriesRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
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
            var breweriesObject = await _distributedCache.GetStringAsync(BreweriesKey);

            if (!string.IsNullOrWhiteSpace(breweriesObject))
            {
                return JsonConvert.DeserializeObject<List<Brewery>>(breweriesObject);
            }
            else
            {
                var breweries = await GetAllBreweries();

                var breweriesData = JsonConvert.SerializeObject(breweries);

                var memoryCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60), // Tempo de vida total (mesmo se estiver ativo constantemente)!
                    SlidingExpiration = TimeSpan.FromSeconds(30)                // Tempo para expirar por inatividade.
                };

                await _distributedCache.SetStringAsync(BreweriesKey, breweriesData, memoryCacheEntryOptions);

                return breweries;
            }
        }
    }
}
