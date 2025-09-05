using backend_lab.Models;
using backend_lab.Repositories;

namespace backend_lab.Services
{
    public class CountryService
    {
        private readonly CountryRepository countryRepository;

        public CountryService(IConfiguration configuration)
        {
            countryRepository = new CountryRepository(configuration);
        }

        public List<CountryModel> GetCountries()
        {
            return countryRepository.GetCountries();
        }
    }
}
