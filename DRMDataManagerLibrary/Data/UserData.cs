using DRMDataManagerLibrary.DataAccess;
using DRMDataManagerLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<UserModel> GetUser(string id)
        {
            return (await _db.LoadData<UserModel, dynamic>("[dbo].[spUser_Get]", new { Id = id }, "DRMData")).First();
        }
    }
}
