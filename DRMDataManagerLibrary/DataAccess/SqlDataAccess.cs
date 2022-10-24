using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _isClosed = false;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }

        private string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }

        public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string cnnName)
        {
            using (IDbConnection connection = new SqlConnection(GetConnectionString(cnnName)))
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
            using (IDbConnection connection = new SqlConnection(GetConnectionString(cnnName)))
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
            using (IDbConnection connection = new SqlConnection(GetConnectionString(cnnName)))
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
            _connection = new SqlConnection(GetConnectionString(cnnName));
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
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Commit transaction failed in the Dispose method.");
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
