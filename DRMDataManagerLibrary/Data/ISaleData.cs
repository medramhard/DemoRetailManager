using DRMDataManagerLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public interface ISaleData
    {
        Task Add(SaleModel saleInfo, string cashierId);
        Task<List<SaleReportModel>> GetSaleReport();
    }
}