using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        void CommitTransaction();
        void Dispose();
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string cnnName);
        Task<List<T>> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
        void RollBackTransaction();
        Task<int> SaveAndGetId<T>(string storedProcedure, T parameters, string cnnName);
        Task<int> SaveAndGetIdInTransaction<T>(string storedProcedure, T parameters);
        Task SaveData<T>(string storedProcedure, T parameters, string cnnName);
        Task SaveDataInTransaction<T>(string storedProcedure, T parameters);
        void StartTransaction(string cnnName);
    }
}