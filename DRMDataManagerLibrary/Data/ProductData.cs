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
    public class ProductData
    {
        private readonly IConfiguration _config;

        public ProductData()
        {

        }

        public ProductData(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<ProductModel>> GetAll()
        {
            SqlDataAccess db = new SqlDataAccess(_config);

            return await db.LoadData<ProductModel, dynamic>("[dbo].[spProduct_GetAll]", new { }, "DRMData");
        }

        public async Task<ProductModel> Get(int id)
        {
            SqlDataAccess db = new SqlDataAccess(_config);

            return (await db.LoadData<ProductModel, dynamic>("[dbo].[spProduct_Get]", new { Id = id}, "DRMData")).FirstOrDefault();
        }
    }
}
