namespace Shipping_System.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>();

            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
                allPermissions.AddRange(GeneratePermissionsList(module.ToString()));

            return allPermissions;
        }

        public static class Branches
        {
            public const string View = "Permissions.Branches.View";
            public const string Create = "Permissions.Branches.Create";
            public const string Edit = "Permissions.Branches.Edit";
            public const string Delete = "Permissions.Branches.Delete";
        }
        public static class WeightSetting
        {
            public const string View = "Permissions.WeightSetting.View";
            public const string Create = "Permissions.WeightSetting.Create";
            public const string Edit = "Permissions.WeightSetting.Edit";
            public const string Delete = "Permissions.WeightSetting.Delete";
        }
        
        public static class Orderes
        {
            public const string View = "Permissions.Orderes.View";
            public const string Create = "Permissions.Orderes.Create";
            public const string Edit = "Permissions.Orderes.Edit";
            public const string Delete = "Permissions.Orderes.Delete";
        }
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
        }
        public static class Representatives
        {
            public const string View = "Permissions.Representative.View";
            public const string Create = "Permissions.Representative.Create";
            public const string Edit = "Permissions.Representative.Edit";
            public const string Delete = "Permissions.Representative.Delete";
        }
        public static class Traders
        {
            public const string View = "Permissions.Trader.View";
            public const string Create = "Permissions.Trader.Create";
            public const string Edit = "Permissions.Trader.Edit";
            public const string Delete = "Permissions.Trader.Delete";
        }

        public static class City
        {
            public const string View = "Permissions.City.View";
            public const string Create = "Permissions.City.Create";
            public const string Edit = "Permissions.City.Edit";
            public const string Delete = "Permissions.City.Delete";
        }

        public static class Governorate
        {
            public const string View = "Permissions.Governorate.View";
            public const string Create = "Permissions.Governorate.Create";
            public const string Edit = "Permissions.Governorate.Edit";
            public const string Delete = "Permissions.Governorate.Delete";
        }
        public static class OrderReports
        {
            public const string View = "Permissions.OrderReports.View";
            public const string Create = "Permissions.OrderReports.Create";
            public const string Edit = "Permissions.OrderReports.Edit";
            public const string Delete = "Permissions.OrderReports.Delete";
        }
    }
}
