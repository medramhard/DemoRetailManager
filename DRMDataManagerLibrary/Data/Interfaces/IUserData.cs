using DRMDataManagerLibrary.Models;
using System.Threading.Tasks;

namespace DRMDataManagerLibrary.Data.Interfaces
{
    public interface IUserData
    {
        Task<UserModel> GetUser(string id);
    }
}