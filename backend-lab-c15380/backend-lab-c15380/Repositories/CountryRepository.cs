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

        public List<CountryModel> GetCountries()
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Country";
            return connection.Query<CountryModel>(query).ToList();
        }
    }
}
