using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.ViewModels;
using System.Security.Claims;
using static Shipping_System.Constants.Permissions;

namespace Shipping_System.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //[Authorize(Permissions.Roles.View)]
        //public IActionResult Index(string word)
        //{
        //    List<IdentityRole> roles;
        //    if (string.IsNullOrEmpty(word))
        //    {
        //        roles = _roleManager.Roles.ToList().Where(r=>r.Name!= "Representative"&& r.Name != "Trader").ToList();
        //    }
        //    else
        //    {
        //        roles = _roleManager.Roles
        //            .Where(e => e.Name.ToLower().Contains(word.ToLower()))
        //            .Where(r => r.Name != "Representative" && r.Name != "Trader").ToList();
        //    }

        //    return View(roles);
        //}


        [Authorize(Permissions.Roles.View)]
        public IActionResult Index(string word)
        {
            List<IdentityRole> roles;
            if (string.IsNullOrEmpty(word))
            {
                roles = _roleManager.Roles.ToList().Where(r => r.Name != "Representative" && r.Name != "Trader").ToList();
            }
            else
            {
                roles = _roleManager.Roles
                    .Where(e => e.Name.ToLower().Contains(word.ToLower()))
                    .Where(r => r.Name != "Representative" && r.Name != "Trader").ToList();
            }

            // Move the SuperAdmin role to the first position in the list
            var superAdminRole = roles.SingleOrDefault(r => r.Name == "SuperAdmin");
            if (superAdminRole != null)
            {
                roles.Remove(superAdminRole);
                roles.Insert(0, superAdminRole);
            }

            return View(roles);
        }

        [Authorize(Permissions.Roles.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {            
            var roles = _roleManager.Roles.ToList().Where(r => r.Name != "Representative" && r.Name != "Trader").ToList();
            var superAdminRole = roles.SingleOrDefault(r => r.Name == "SuperAdmin");
            if (superAdminRole != null)
            {
                roles.Remove(superAdminRole);
                roles.Insert(0, superAdminRole);
            }


            if (!ModelState.IsValid)
                return View("Index", roles);


            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                ModelState.AddModelError("Name", "Role is exists!");
                    return View("Index", roles);

                }

            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Roles.Edit)]
        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            if (roleId == null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();

            var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            var allClaims = Permissions.GenerateAllPermissions();
            var allPermissions = allClaims.Select(p => new CheckBoxViewModel { DisplayValue = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(c => c == permission.DisplayValue))
                    permission.IsSelected = true;
            }

            var viewModel = new PermissionsFormViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleCalims = allPermissions
            };

            return View(viewModel);
        }
        [Authorize(Permissions.Roles.Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(PermissionsFormViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
                return NotFound();

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
                await _roleManager.RemoveClaimAsync(role, claim);

            var selectedClaims = model.RoleCalims.Where(c => c.IsSelected).ToList();

            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.DisplayValue));

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Roles.Edit)]
        public async Task<ActionResult> Edit(string id, string name)
        {
            if (id == null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            role.Name = name;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                throw new Exception("Error while deleting");

            return Ok(result);
        }
        [Authorize(Permissions.Roles.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new Exception("Error while deleting");

            return Ok(result);
        }
    }
}
