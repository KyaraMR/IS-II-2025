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

        public string CreateCountry(CountryModel country)
        {
            var result = string.Empty;
            try
            {
                var isCreated = countryRepository.CreateCountry(country);
                if (!isCreated)
                {
                    result = "Error al crear el país";
                }
            }
            catch (Exception)
            {
                result = "Error creando país";
            }

            return result;
        }

        public List<CountryModel> GetCountries()
        {
            return countryRepository.GetCountries();
        }
    }
}
