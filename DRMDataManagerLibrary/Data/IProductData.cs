using DRMDataManagerLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public interface IProductData
    {
        Task<ProductModel> Get(int id);
        Task<List<ProductModel>> GetAll();
    }
}