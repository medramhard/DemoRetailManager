using DRMDataManagerLibrary.DataAccess;
using DRMDataManagerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public class SaleData
    {
        // NEEDS REFACTORING
        public async Task Add(SaleModel saleInfo, string cashierId)
        {
            ProductData products = new ProductData();
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            decimal taxRate = ConfigHelper.GetTaxRate();

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                try
                {
                    var product = await products.Get(item.ProductId);
                    detail.PurchasePrice = product.RetailPrice * detail.Quantity;

                    if (product.IsTaxable)
                    {
                        detail.Tax = detail.PurchasePrice * taxRate / 100;
                    }

                    details.Add(detail);
                }
                catch (Exception)
                {

                    throw new Exception($"The product Id of {item.ProductId} could not be found in the database.");
                }
            }

            SaleDBModel sale = new SaleDBModel()
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            using (SqlDataAccess db = new SqlDataAccess())
            {
                try
                {
                    db.StartTransaction("DRMData");
                    sale.Id = await db.SaveAndGetIdInTransaction("[dbo].[spSale_Add]",
                    new { sale.CashierId, sale.SaleDate, sale.SubTotal, sale.Tax, sale.Total });

                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        await db.SaveDataInTransaction("[dbo].[spSaleDetail_Add]", item);
                    }
                }
                catch
                {
                    db.RollBackTransaction();
                    throw;
                }
            }
        }
    }
}
