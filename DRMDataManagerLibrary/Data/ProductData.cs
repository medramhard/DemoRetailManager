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
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _db;
        private const string _dbName = "DRMData";

        public ProductData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<ProductModel>> GetAll()
        {
            return await _db.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, _dbName);
        }

        public async Task<ProductModel> Get(int id)
        {
            return (await _db.LoadData<ProductModel, dynamic>("[dbo].[spProduct_Get]", new { Id = id }, _dbName)).FirstOrDefault();
        }

        public async Task Update(ProductModel product)
        {
            await _db.SaveData("[dbo].[spProduct_UpdateQuantity]", product, _dbName);
        }
    }
}
