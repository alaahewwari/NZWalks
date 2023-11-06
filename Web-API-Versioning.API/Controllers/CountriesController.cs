using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Versioning.API.Models.DTOs;

namespace Web_API_Versioning.API.Controllers
{
    [Route("api/v{varion:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CountriesController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetV1()
        {
            var countriesDomainModel = CountriesData.Get();
            //map domain model to dto
            var response = new List<CountryDTOV1>();
            foreach (var countryDomain in countriesDomainModel)
            {
                response.Add(new CountryDTOV1
                {
                    Id = countryDomain.Id,
                    Name = countryDomain.Name
                });
            }
            return Ok(response);
        }
        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetV2()
        {
            var countriesDomainModel = CountriesData.Get();
            //map domain model to dto
            var response = new List<CountryDTOV2>();
            foreach (var countryDomain in countriesDomainModel)
            {
                response.Add(new CountryDTOV2
                {
                    Id = countryDomain.Id,
                    CountryName = countryDomain.Name
                });
            }
            return Ok(response);
        }

    }
}
