using DRMDataManagerLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data.Interfaces
{
    public interface IProductData
    {
        Task<ProductModel> Get(int id);
        Task<List<ProductModel>> GetAll();
    }
}