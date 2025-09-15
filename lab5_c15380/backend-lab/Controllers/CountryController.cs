using Microsoft.AspNetCore.Mvc;
using backend_lab.Services;
using backend_lab.Models;

namespace backend_lab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly CountryService countryService;

        public CountryController(IConfiguration configuration)
        {
            countryService = new CountryService(configuration);
        }

        [HttpGet]
        public ActionResult<List<CountryModel>> Get()
        {
            return countryService.GetCountries();
        }

        [HttpPost]
        public ActionResult<bool> CreateCountry(CountryModel country)
        {
            if (country == null) return BadRequest();
            var result = countryService.CreateCountry(country);
            return string.IsNullOrEmpty(result) ? Ok(true) : BadRequest(result);
        }
    }
}
