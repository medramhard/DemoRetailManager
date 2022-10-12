using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.DataAccess
{
    internal class SqlDataAccess
    {
        public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string cnnName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnName].ConnectionString))
            {
                return (await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task SaveData<T>(string storedProcedure, T parameters, string cnnName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnName].ConnectionString))
            {
                await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> SaveEntry<T>(string storedProcedure, T parameters, string cnnName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnName].ConnectionString))
            {
                int identity = await connection.QuerySingleAsync<int>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return identity;
            }

        }
    }
}
