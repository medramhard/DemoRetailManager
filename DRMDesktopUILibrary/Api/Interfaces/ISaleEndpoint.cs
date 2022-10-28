using DRMDesktopUILibrary.Models;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Api.Interfaces
{
    public interface ISaleEndpoint
    {
        Task Post(SaleModel sale);
    }
}