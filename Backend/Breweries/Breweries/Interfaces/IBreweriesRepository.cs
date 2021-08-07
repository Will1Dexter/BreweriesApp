using Breweries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Breweries.Interfaces
{
    public interface IBreweriesRepository
    {
        Task<IEnumerable<Brewery>> GetAllBreweries();

        Task<IEnumerable<Brewery>> GetAllBreweriesCached();
    }
}
