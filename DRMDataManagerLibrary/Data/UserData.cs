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
    public class UserData
    {
        private readonly IConfiguration _config;

        public UserData()
        {

        }

        public UserData(IConfiguration config)
        {
            _config = config;
        }

        public async Task<UserModel> GetUser(string id)
        {
            SqlDataAccess db = new SqlDataAccess(_config);

            return (await db.LoadData<UserModel, dynamic>("[dbo].[spUser_Get]", new { Id = id }, "DRMData")).First();
        }
    }
}
