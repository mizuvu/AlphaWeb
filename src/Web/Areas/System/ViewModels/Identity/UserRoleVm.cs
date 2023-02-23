namespace Web.Areas.System.ViewModels.Identity;

public class UserRoleVm
{
    public string RoleName { get; set; } = default!;
    public bool IsOwned { get; set; }
}