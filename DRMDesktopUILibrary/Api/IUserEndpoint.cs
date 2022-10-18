using DRMDesktopUILibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
        Task<List<UserRoleModel>> GetAllRoles();
        Task AddUserToRole(string userId, string roleName);
        Task RemoveUserFromRole(string userId, string roleName);
    }
}