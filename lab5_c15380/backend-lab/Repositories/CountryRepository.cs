using backend_lab.Models;
using Dapper;
using System.Data.SqlClient;

namespace backend_lab.Repositories
{
    public class CountryRepository
    {
        private readonly string _connectionString;

        public CountryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CountryContext");
        }

        public bool CreateCountry(CountryModel country)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO [dbo].[Country] ([Name],[Language],[Continent])  
                        VALUES(@Name, @Language, @Continent)";
            var affectedRows = connection.Execute(query, new
            {
                Name = country.Name,
                Language = country.Language,
                Continent = country.Continent
            });
            return affectedRows >= 1;
        }

        public List<CountryModel> GetCountries()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Country";
            return connection.Query<CountryModel>(query).ToList();
        }
    }
}
