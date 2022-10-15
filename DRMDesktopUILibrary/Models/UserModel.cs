using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRMDesktopUILibrary.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public List<UserRoleModel> Roles { get; set; } = new List<UserRoleModel>();

        public string DisplayRoles
        {
            get
            {
                return string.Join(", ", Roles.Select(x => x.Name));
            }
        }
    }
}
