namespace Web.Areas.System.ViewModels.Identity;

public class RoleClaimVm
{
    public string ClaimType { get; set; } = default!;
    public string ClaimValue { get; set; } = default!;
    public bool IsOwned { get; set; }
}