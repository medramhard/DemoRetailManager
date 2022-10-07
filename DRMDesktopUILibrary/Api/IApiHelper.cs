using DRMDesktopUILibrary.Models;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Api
{
    public interface IApiHelper
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);
        Task GetUser(string token);
    }
}