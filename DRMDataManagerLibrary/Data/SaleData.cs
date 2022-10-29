using DRMDataManagerLibrary.Data.Interfaces;
using DRMDataManagerLibrary.DataAccess;
using DRMDataManagerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public class SaleData : ISaleData
    {
        private readonly IProductData _products;
        private readonly ISqlDataAccess _db;
        private const decimal _taxRate = 8.75m;

        public SaleData(IProductData products, ISqlDataAccess db)
        {
            _products = products;
            _db = db;
        }

        private async Task<List<SaleDetailDBModel>> GetDetails(SaleModel sale)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();

            foreach (var item in sale.SaleDetails)
            {
                var detail = new SaleDetailDBModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var product = await _products.Get(item.ProductId);

                // Update Quantity of product in stock
                product.QuantityInStock -= item.Quantity;
                await _products.Update(product);

                detail.PurchasePrice = product.RetailPrice * detail.Quantity;

                if (product.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * _taxRate / 100;
                }

                details.Add(detail);
            }

            return details;
        }

        private async Task PostSale(SaleDBModel sale, List<SaleDetailDBModel> details)
        {
            try
            {
                _db.StartTransaction("DRMData");

                sale.Id = await _db.SaveAndGetIdInTransaction("[dbo].[spSale_Add]",
                new { sale.CashierId, sale.SaleDate, sale.SubTotal, sale.Tax, sale.Total });

                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    await _db.SaveDataInTransaction("[dbo].[spSaleDetail_Add]", item);
                }

                _db.CommitTransaction();
            }
            catch
            {
                _db.RollBackTransaction();
                throw;
            }
        }

        public async Task Add(SaleModel saleInfo, string cashierId)
        {
            var details = await GetDetails(saleInfo);

            SaleDBModel sale = new SaleDBModel()
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            await PostSale(sale, details);
        }

        public async Task<List<SaleReportModel>> GetSaleReport()
        {
            return await _db.LoadData<SaleReportModel, dynamic>("[dbo].[spSale_GetSaleReport]", new { }, "DRMData");
        }
    }
}
