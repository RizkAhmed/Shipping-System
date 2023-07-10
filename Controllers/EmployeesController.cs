using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.Repository.BranchRepo;
using Shipping_System.Repository.OrderRepo;
using Shipping_System.Repository.OrderStateRepo;
using Shipping_System.ViewModels;

namespace Shipping_System.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBranchRepository _branchRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderStateRepository _orderStateRepository;

        public EmployeesController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IOrderRepository orderRepository,
            IOrderStateRepository orderStateRepository,
            IBranchRepository branchRepository
            )

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _branchRepository = branchRepository;
            _orderRepository = orderRepository;
            _orderStateRepository = orderStateRepository;
        }

        [Authorize(Permissions.Users.View)]
        public async Task<IActionResult> Index()
        {
            var usersFromDb = await _userManager.Users.Include(u => u.Branch).ToListAsync();
            var users = usersFromDb
                .Where(u=>!_userManager.IsInRoleAsync(u, "Representative").Result && !_userManager.IsInRoleAsync(u, "Trader").Result)
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    creationDate = user.creationDate,
                    Branch = user.Branch,
                    Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                    IsDeleted = user.IsDeleted
                })
                .ToList();
            return View(users);
        }
        [Authorize(Permissions.Users.Create)]
        public IActionResult Create()
        {
            var roles = _roleManager.Roles
                .Where(r => r.Name != "Representative" && r.Name != "Trader")
                .Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            var user = new UserFormViewModel
            {
                Roles = roles,
                Branches = _branchRepository.GetAll()
            };
            return View(user);
        }
        [Authorize(Permissions.Users.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
            var roles = _roleManager.Roles
                .Where(r => r.Name != "Representative" && r.Name != "Trader")
                .Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            model.Roles = roles;
            model.Branches = _branchRepository.GetAll();
            if (!ModelState.IsValid)
                return View(model);
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is already exist");
                return View(model);
            }
            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                BranchId = model.BranchId,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Role", error.Description);
                    return View(model);
                }
            }
            await _userManager.AddToRoleAsync(user, model.RoleName);
            await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return NotFound();

            var roles = _roleManager.Roles
                .Where(r => r.Name != "Representative" && r.Name != "Trader")
                .Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            var model = new UpdateUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                BranchId = user.BranchId,
                RoleName = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                Roles = roles,
                Branches = _branchRepository.GetAll()
            };
            return View(model);
        }

        [Authorize(Permissions.Users.Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateUserViewModel model)
        {
            var roles = _roleManager.Roles
                .Where(r => r.Name != "Representative" && r.Name != "Trader")
                .Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
            model.Roles = roles;
            model.Branches = _branchRepository.GetAll();

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);
            var checkUser = await _userManager.FindByEmailAsync(model.Email);
            if (checkUser != null && checkUser.Id != model.Id)
            {
                ModelState.AddModelError("Email", "Email is already exist");
                return View(model);
            }
            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Address = model.Address;
            user.BranchId = model.BranchId;

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, model.RoleName);
            await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Role", error.Description);
                    return View(model);
                }
            }


            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Users.Delete)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.IsDeleted = true;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Error while deleting");

            return Ok(result);
        }

        [Authorize(Permissions.Users.Delete)]
        public async Task<IActionResult> changeState(string id)
        {
            if (id == null)
                return BadRequest();
      

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            user.IsDeleted = !user.IsDeleted;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Content("Not Okey");
            }
            return Ok();
        }
        [Authorize(Permissions.Users.View)]
        public IActionResult Home()
        {

            //Get User Info
            var username = User.Identity.Name;
            var user = _userManager.FindByEmailAsync(username).Result;
            ViewBag.EmployeeName = user.Name;

            List<Order> orders = _orderRepository.GetAll();

            List<int> OrderStatusNumbers = new List<int>();

            foreach (var orderStatus in _orderStateRepository.GetAll())
            {
                int count = 0;
                foreach (var order in orders)
                {
                    if (order.OrderStateId == orderStatus.Id)
                    {
                        count++;
                    }
                }
                OrderStatusNumbers.Add(count);

            }


            ViewBag.OrderStatus = _orderStateRepository.GetAll();
            ViewBag.OrderStatusNumbers = OrderStatusNumbers;


            return View();

        }
    }
}
