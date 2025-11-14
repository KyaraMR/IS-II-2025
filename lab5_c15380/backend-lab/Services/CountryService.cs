using backend_lab_c15380.Models;
using backend_lab_c15380.Repositories;
namespace backend_lab_c15380.Services
{
    public class CountryService
    {
        private readonly CountryRepository countryRepository;
        public CountryService()
        {
            countryRepository = new CountryRepository();
        }
        public List<CountryModel> GetCountries()
        {
            // Add any missing business logic when it is neccesary
        return countryRepository.GetCountries();
        }
    }
}