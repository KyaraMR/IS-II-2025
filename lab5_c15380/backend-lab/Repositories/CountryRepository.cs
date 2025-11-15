using backend_lab_c15380.Models;
using Dapper;
using System.Data.SqlClient;

namespace backend_lab_c15380.Repositories
{
    public class CountryRepository
    {
        private readonly string _connectionString;

        public CountryRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("CountryContext");
        }

        public List<CountryModel> GetCountries()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM dbo.Country";
            return connection.Query<CountryModel>(query).ToList();
        }

        public bool CreateCountry(CountryModel country)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO [dbo].[Country] ([Name], [Language], [Continent])
                         VALUES (@Name, @Language, @Continent)";
            var affectedRows = connection.Execute(query, new
            {
                Name = country.Name,
                Language = country.Language,
                Continent = country.Continent
            });

            return affectedRows >= 1;
        }
    }
}