using Microsoft.AspNetCore.Identity;
using Shipping_System.Constants;

namespace Shipping_System.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManger)
        {
            await roleManger.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManger.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManger.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
            await roleManger.CreateAsync(new IdentityRole(Roles.Trader.ToString()));
            await roleManger.CreateAsync(new IdentityRole(Roles.Representative.ToString()));

            await roleManger.SeedClaimsForRepresentative();
            await roleManger.SeedClaimsForTrader();
        }
        private static async Task SeedClaimsForRepresentative(this RoleManager<IdentityRole> roleManager)
        {
            var representativeRole = await roleManager.FindByNameAsync(Roles.Representative.ToString());

            await roleManager.AddPermissionClaims(representativeRole, Modules.Representative.ToString());
            
        }
        private static async Task SeedClaimsForTrader(this RoleManager<IdentityRole> roleManager)
        {
            var representativeRole = await roleManager.FindByNameAsync(Roles.Trader.ToString());

            await roleManager.AddPermissionClaims(representativeRole, Modules.Trader.ToString());

        }
    }
}
