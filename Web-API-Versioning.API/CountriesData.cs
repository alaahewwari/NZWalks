using System.Reflection.Metadata.Ecma335;
using Web_API_Versioning.API.Models.Domain;

namespace Web_API_Versioning.API
{
    public class CountriesData
    {
        //list of ten random countries
        public static List<Country> Get()
        {
            var countries = new List<Country>
            {
                new Country { Id = 1, Name = "New Zealand" },
                new Country { Id = 2, Name = "Australia" },
                new Country { Id = 3, Name = "United Kingdom" },
                new Country { Id = 4, Name = "United States" },
                new Country { Id = 5, Name = "Canada" },
                new Country { Id = 6, Name = "China" },
                new Country { Id = 7, Name = "Japan" },
                new Country { Id = 8, Name = "France" },
                new Country { Id = 9, Name = "Germany" },
                new Country { Id = 10, Name = "Italy" }
            };
            return countries.Select(c => new Country
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}
