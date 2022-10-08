using DRMDesktopUILibrary.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Api
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUserModel> Authenticate(string username, string password);
        Task GetUser(string token);
    }
}