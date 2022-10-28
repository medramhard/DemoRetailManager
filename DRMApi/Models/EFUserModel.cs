using Microsoft.AspNetCore.Identity;

namespace DRMApi.Models
{
    public class EFUserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;

        public IdentityUser GetUser()
        {
            var user = new IdentityUser()
            {
                UserName = UserName,
                Email = Email,
                EmailConfirmed = EmailConfirmed
            };

            return user;
        }
    }
}
