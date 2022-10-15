using DRMDataManagerLibrary.DataAccess;
using DRMDataManagerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public class InventoryData
    {
        public async Task<List<InventoryItemModel>> GetAll()
        {
            SqlDataAccess db = new SqlDataAccess();

            return await db.LoadData<InventoryItemModel, dynamic>("[dbo].[spInventory_GetAll]", new { }, "DRMData");
        }

        public async Task Add(InventoryItemModel item)
        {
            SqlDataAccess db = new SqlDataAccess();

            await db.SaveData("[dbo].[spInventory_Add]", item, "DRMData");
        }
    }
}
