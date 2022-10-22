namespace DRMApi.Models;

public class ApplicationUserModel
{
    public string Id { get; set; }
    public string EmailAddress { get; set; }
    public List<ApplicationUserRoleModel> Roles { get; set; } = new List<ApplicationUserRoleModel>();
}
