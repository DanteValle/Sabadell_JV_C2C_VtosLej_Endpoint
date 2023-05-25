using Microsoft.Data.SqlClient;
using Sabadell_JV_C2C_VtosLej_Endpoint.Logic.Interface;
using System.Data;

namespace Sabadell_JV_C2C_VtosLej_Endpoint.DataAcces
{
    public class DataBaseConnectionFactory : IDataBaseConnectionFactory
    {
        private readonly string _connectionString;
        public DataBaseConnectionFactory(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("conexiondb_local");
        }

        public IDbConnection GetDbConnection() {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
