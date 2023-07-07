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
        public IActionResult Home(/*int branchId*/)
        {
            var allOrders = (from O in _orderRepository.GetAll()
                             join OS in _orderStateRepository.GetAll()
                             on O.OrderStateId equals OS.Id
                             /*where O.BranchId == branchId*/
                             select new
                             {
                                 OrderStateName = OS.Name
                             }).ToList();

            OrderStatusViewModel orderStatusViewModel = new OrderStatusViewModel();

            orderStatusViewModel.NewCount = allOrders.Where(O => O.OrderStateName == "New").Count();
            orderStatusViewModel.pendingCount = allOrders.Where(O => O.OrderStateName == "Waiting").Count();
            orderStatusViewModel.The_order_has_been_deliveredCount = allOrders.Where(O => O.OrderStateName == "Delivered to the representative").Count();
            orderStatusViewModel.sent_delivered_handedCount = allOrders.Where(O => O.OrderStateName == "Delivered to the client").Count();
            orderStatusViewModel.Can_not_reachCount = allOrders.Where(O => O.OrderStateName == "Cannot reach").Count();
            orderStatusViewModel.postponedCount = allOrders.Where(O => O.OrderStateName == "Postponed").Count();
            orderStatusViewModel.Partially_deliveredCount = allOrders.Where(O => O.OrderStateName == "Partially delivered").Count();
            orderStatusViewModel.Canceled_by_ClientCount = allOrders.Where(O => O.OrderStateName == "Canceled by the client").Count();
            orderStatusViewModel.Refused_with_paymentCount = allOrders.Where(O => O.OrderStateName == "Declined but Paid").Count();
            orderStatusViewModel.Refused_with_part_paymentCount = allOrders.Where(O => O.OrderStateName == "Declined but Partially Paid").Count();
            orderStatusViewModel.Rejected_and_not_paidCount = allOrders.Where(O => O.OrderStateName == "Declined without Payment").Count();

            return View(orderStatusViewModel);

        }
    }
}
