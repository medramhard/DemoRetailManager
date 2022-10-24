using DRMDataManagerLibrary.Data.Interfaces;
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
    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAccess _db;

        public InventoryData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<InventoryItemModel>> GetAll()
        {
            return await _db.LoadData<InventoryItemModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "DRMData");
        }

        public async Task Add(InventoryItemModel item)
        {
            await _db.SaveData("[dbo].[spInventory_Add]", item, "DRMData");
        }
    }
}
