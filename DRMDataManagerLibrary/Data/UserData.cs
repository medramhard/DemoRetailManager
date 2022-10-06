using DRMDataManagerLibrary.DataAccess;
using DRMDataManagerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public class UserData
    {
        private readonly SqlDataAccess _db;

        public UserData()
        {
            _db = new SqlDataAccess();
        }

        public async Task<UserModel> GetUser(string id)
        {
            return (await _db.LoadData<UserModel, dynamic>("[dbo].[spUser_Get]", new { Id = id }, "DRMData")).First();
        }
    }
}
