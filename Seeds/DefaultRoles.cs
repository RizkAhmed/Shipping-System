using Microsoft.AspNetCore.Identity;
using Shipping_System.Constants;
using Shipping_System.Models;
using System.Security.Claims;

namespace Shipping_System.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManger)
        {
            await roleManger.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManger.CreateAsync(new IdentityRole(Roles.Trader.ToString()));
            await roleManger.CreateAsync(new IdentityRole(Roles.Representative.ToString()));

            await roleManger.SeedClaimsForRepresentative();
            await roleManger.SeedClaimsForTrader();
        }
        private static async Task SeedClaimsForRepresentative(this RoleManager<IdentityRole> roleManager)
        {
            var representativeRole = await roleManager.FindByNameAsync(Roles.Representative.ToString());

            await roleManager.AddClaimAsync(representativeRole, new Claim("Permission", $"Permissions.Representative.View"));

        }
        private static async Task SeedClaimsForTrader(this RoleManager<IdentityRole> roleManager)
        {
            var traderRole = await roleManager.FindByNameAsync(Roles.Trader.ToString());
            await roleManager.AddClaimAsync(traderRole, new Claim("Permission", $"Permissions.Trader.View"));
            await roleManager.AddPermissionClaims(traderRole, Modules.Orderes.ToString());

        }
    }
}
