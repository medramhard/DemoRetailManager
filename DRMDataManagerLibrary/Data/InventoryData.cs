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
    public class InventoryData
    {
        private readonly IConfiguration _config;

        public InventoryData(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<InventoryItemModel>> GetAll()
        {
            SqlDataAccess db = new SqlDataAccess(_config);

            return await db.LoadData<InventoryItemModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "DRMData");
        }

        public async Task Add(InventoryItemModel item)
        {
            SqlDataAccess db = new SqlDataAccess(_config);

            await db.SaveData("[dbo].[spInventory_Add]", item, "DRMData");
        }
    }
}
