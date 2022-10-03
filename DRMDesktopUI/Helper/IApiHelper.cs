using DRMDesktopUI.Models;
using System.Threading.Tasks;

namespace DRMDesktopUI.Helper
{
    public interface IApiHelper
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);
    }
}