using DRMDesktopUILibrary.Models;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Api
{
    public interface ISaleEndpoint
    {
        Task Post(SaleModel sale);
    }
}