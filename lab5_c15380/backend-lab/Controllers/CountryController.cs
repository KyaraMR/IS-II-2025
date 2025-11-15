using backend_lab_c15380.Models;
using backend_lab_c15380.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryService countryService;

        public CountryController()
        {
            countryService = new CountryService();
        }

        [HttpGet]
        public List<CountryModel> Get()
        {
            var countries = countryService.GetCountries();
            return countries;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateCountry(CountryModel country)
        {
            if (country == null)
            {
                return BadRequest();
            }

            var result = countryService.CreateCountry(country);
            if (string.IsNullOrEmpty(result))
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}