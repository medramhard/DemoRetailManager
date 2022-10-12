using DRMDataManagerLibrary.DataAccess;
using DRMDataManagerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public class ProductData
    {
        private readonly SqlDataAccess _db;

        public ProductData()
        {
            _db = new SqlDataAccess();
        }

        public async Task<List<ProductModel>> GetAll()
        {
            return await _db.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, "DRMData");
        }

        public async Task<ProductModel> Get(int id)
        {
            return (await _db.LoadData<ProductModel, dynamic>("[dbo].[spProduct_Get]", new { Id = id}, "DRMData")).FirstOrDefault();
        }
    }
}
