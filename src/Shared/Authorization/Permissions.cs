using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Authorization;

public class Permissions
{
    [DisplayName("Management")]
    public static class Managers
    {
        public const string View = $"Managers.View";
        public const string Create = $"Managers.Create";
        public const string Update = $"Managers.Update";

        [Display(Name = "Push notifications", Description = "use this to push notifications")]
        public const string Push = $"Managers.Push";
    }

    [DisplayName("Users")]
    [Description("Users Management")]
    public static class Users
    {
        private const string _user = nameof(Users);

        [Display(Name = "View users list")]
        public const string View = $"{_user}.{nameof(View)}";
        public const string Create = $"{_user}.{nameof(Create)}";
        public const string Update = $"{_user}.{nameof(Update)}";
        public const string Delete = $"{_user}.{nameof(Delete)}";
    }

    [Description("Roles Management")]
    public static class Roles
    {
        private const string _role = nameof(Roles);

        [Display(Description = "use this to view roles list")]
        public const string View = $"{_role}.{nameof(View)}";
        public const string Create = $"{_role}.{nameof(Create)}";
        public const string Update = $"{_role}.{nameof(Update)}";
        public const string Delete = $"{_role}.{nameof(Delete)}";
    }

    public static class Orders
    {
        public const string View = $"{nameof(Orders)}.{nameof(View)}";
        public const string Create = $"{nameof(Orders)}.{nameof(Create)}";
        public const string Update = $"{nameof(Orders)}.{nameof(Update)}";
        public const string Delete = $"{nameof(Orders)}.{nameof(Delete)}";

        [Display(Description = "use this to create SRO")]
        public const string Return = $"{nameof(Orders)}.{nameof(Return)}";

        [Display(Description = "use this to manual post SRO")]
        public const string PostReturn = $"{nameof(Orders)}.{nameof(PostReturn)}";
    }

    public static class MarketPlaces
    {
        public const string View = $"{nameof(MarketPlaces)}.{nameof(View)}";

        [Display(Description = "use this for sync MKP orders")]
        public const string SyncOrder = $"{nameof(MarketPlaces)}.{nameof(SyncOrder)}";

        [Display(Description = "use this for shipout MKP orders")]
        public const string ShipOrder = $"{nameof(MarketPlaces)}.{nameof(ShipOrder)}";

        public const string Stock = $"{nameof(MarketPlaces)}.{nameof(Stock)}";
        public const string UploadStock = $"{nameof(MarketPlaces)}.{nameof(UploadStock)}";
    }

    public static class GrabMart
    {
        public const string View = $"{nameof(GrabMart)}.{nameof(View)}";

        public const string Api = $"{nameof(GrabMart)}.{nameof(Api)}";

        public const string Admin = $"{nameof(GrabMart)}.{nameof(Admin)}";

        public const string MasterData = $"{nameof(GrabMart)}.{nameof(MasterData)}";

        public const string Order = $"{nameof(GrabMart)}.{nameof(Order)}";
    }

    public static class Marketing
    {
        public const string View = $"{nameof(Marketing)}.{nameof(View)}";
        public const string Coupon = $"{nameof(Marketing)}.{nameof(Coupon)}";
    }
}
