using DRMDesktopUILibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
    }
}