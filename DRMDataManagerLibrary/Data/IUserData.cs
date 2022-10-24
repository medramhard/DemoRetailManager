using DRMDataManagerLibrary.Models;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data
{
    public interface IUserData
    {
        Task<UserModel> GetUser(string id);
    }
}