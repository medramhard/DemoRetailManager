using DRMDesktopUILibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Api.Interfaces
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
        Task UpdateQuantity(ProductModel product);
    }
}