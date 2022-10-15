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
    public class SqlDataAccess : IDisposable
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _isClosed = false;

        public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string cnnName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnName].ConnectionString))
            {
                return (await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public async Task<List<T>> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            return (await _connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction)).ToList();
        }

        public async Task SaveData<T>(string storedProcedure, T parameters, string cnnName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnName].ConnectionString))
            {
                await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            await _connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public async Task<int> SaveAndGetId<T>(string storedProcedure, T parameters, string cnnName)
        {
            using (IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnName].ConnectionString))
            {
                return await connection.QuerySingleAsync<int>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public async Task<int> SaveAndGetIdInTransaction<T>(string storedProcedure, T parameters)
        {
            return await _connection.QuerySingleAsync<int>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public void StartTransaction(string cnnName)
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnName].ConnectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            _isClosed = false;
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            _isClosed = true;
        }

        public void RollBackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            _isClosed = true;
        }

        public void Dispose()
        {
            if (_isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    // TODO: make a proper exception handling, log this issue
                    throw;
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
