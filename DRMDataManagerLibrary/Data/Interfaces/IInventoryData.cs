using DRMDataManagerLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data.Interfaces
{
    public interface IInventoryData
    {
        Task Add(InventoryItemModel item);
        Task<List<InventoryItemModel>> GetAll();
    }
}