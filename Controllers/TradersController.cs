using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.Repository.BranchRepo;
using Shipping_System.Repository.CityRepo;
using Shipping_System.Repository.GovernorateRepo;
using Shipping_System.Repository.OrderRepo;
using Shipping_System.Repository.OrderStateRepo;
using Shipping_System.Repository.TraderRepo;
using Shipping_System.ViewModels;
using Shipping_System.ViewModels.TraderViewModel;
using System.Security.Claims;

namespace Shipping_System.Controllers
{
    public class TradersController : Controller
    {
        ITraderRepository _traderRepository;
        IGovernRepository _governRepository;
        ICityRepository _cityRepository;
        IBranchRepository _branchRepository;
        IOrderRepository _orderRepository;
        UserManager<ApplicationUser> _userManager;
        IOrderStateRepository _orderStateRepository;


        public TradersController
            (
            ITraderRepository traderRepository,
            IGovernRepository governRepository,
            ICityRepository cityRepository,
            IBranchRepository branchRepository,
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager,
            IOrderStateRepository orderStateRepository

            )
        {
            _traderRepository = traderRepository;
            _governRepository = governRepository;
            _cityRepository = cityRepository;
            _branchRepository = branchRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _orderStateRepository = orderStateRepository;
        }

        [Authorize(Permissions.Users.View)]
        public IActionResult Index()
        {
            var traders = _traderRepository.GetAll().ToList();
            return View(traders);
        }

        [Authorize(Permissions.Users.Create)]
        public async Task<IActionResult> Create()
        {
            var trader = new TraderAndUserViewModel
            {
                Governorates = _governRepository.GetAll(),
                Branchs = _branchRepository.GetAll(),
                Cities = _cityRepository.GetAll()
            };
            return View(trader);
        }

        [Authorize(Permissions.Users.Create)]
        [HttpPost]
        public async Task<IActionResult> Create(TraderAndUserViewModel model)
        {
            model.Governorates = _governRepository.GetAll();
            model.Branchs = _branchRepository.GetAll();
            model.Cities = _cityRepository.GetAll();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is already exist");
                return View(model);
            }

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                PhoneNumber = model.Phone,
                Address = model.Address,
                BranchId = model.BranchId
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("BranchId", error.Description);
                    return View(model);
                }
            }
            await _userManager.AddToRoleAsync(user, Roles.Trader.ToString());
            await _userManager.SetPhoneNumberAsync(user, model.Phone);

            var trader = new Trader
            {
                AppUserId = user.Id,
                GoverId = model.GoverId,
                BranchId = model.BranchId,
                CityId = model.CityId,
                SpecialPickupCost =model.SpecialPickupCost,
                StoreName = model.StoreName,
                TraderTaxForRejectedOrders = model.TraderTaxForRejectedOrders,
                IsDeleted = model.IsDeleted,
            };
            _traderRepository.Create(trader);
            _traderRepository.Save();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> Edit(string AppUserId)
        {
            if (AppUserId == null)
                return BadRequest();
            var trader = _traderRepository.GetById(AppUserId);
            if (trader == null)
                return NotFound();
            var user = await _userManager.FindByIdAsync(AppUserId);
            var model = new TraderAndUserViewModel
            {
                Email = user.Email,
                Name = user.Name,
                Phone = user.PhoneNumber,
                Address = user.Address,
                Password = "Dummy123+",
                BranchId = user.BranchId,
                GoverId = trader.GoverId,
                CityId = trader.CityId,
                SpecialPickupCost = trader.SpecialPickupCost,
                StoreName = trader.StoreName,
                TraderTaxForRejectedOrders = trader.TraderTaxForRejectedOrders,
                IsDeleted = trader.IsDeleted,
                Governorates = _governRepository.GetAll(),
                Branchs = _branchRepository.GetAll(),
                Cities = _cityRepository.GetAll()
            };
            return View(model);
        }

        [Authorize(Permissions.Users.Edit)]
        [HttpPost]
        public async Task<IActionResult> Edit(TraderAndUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.AppUserId);
            if (user == null)
                return NotFound();

            model.Governorates = _governRepository.GetAll();
            model.Branchs = _branchRepository.GetAll();
            model.Cities = _cityRepository.GetAll();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var checkUser = await _userManager.FindByEmailAsync(model.Email);
            if (checkUser != null && checkUser.Id != model.AppUserId)
            {
                ModelState.AddModelError("Email", "Email is already exist");
                return View(model);
            }
            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Address = model.Address;
            user.BranchId = model.BranchId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("BranchId", error.Description);
                    return View(model);
                }
            }
            var traderFDB = _traderRepository.GetById(model.AppUserId);

            traderFDB.GoverId = model.GoverId;
            traderFDB.BranchId = model.BranchId;
            traderFDB.CityId = model.CityId;
            traderFDB.SpecialPickupCost = model.SpecialPickupCost;
            traderFDB.StoreName = model.StoreName;
            traderFDB.TraderTaxForRejectedOrders = model.TraderTaxForRejectedOrders;
            
            _traderRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Users.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();
            var trader = _traderRepository.GetById(id);
            if (trader == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            user.IsDeleted = true;

            await _userManager.UpdateAsync(user);

            trader.IsDeleted = true;
            _traderRepository.Edit(trader);
            _traderRepository.Save();
            return Content("sucsses");
        }

        [Authorize(Permissions.Users.Delete)]
        public async Task<IActionResult> changeState(string id)
        {
            if (id == null)
                return BadRequest();
            var trader = _traderRepository.GetById(id);
            if (trader == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            user.IsDeleted = !user.IsDeleted;
            trader.IsDeleted = !trader.IsDeleted;
            _traderRepository.Save();
            return Ok();
        }
        [Authorize(Permissions.Traders.View)]
        public IActionResult Home()
        {

            //Get User Info
            var username = User.Identity.Name;
            var user = _userManager.FindByEmailAsync(username).Result;
            ViewBag.TraderName = user.Name;

            List<Order> orders = _orderRepository.GetAll().Where(o => o.TraderId == user.Id).ToList();

            List<int> OrderStatusNumbers = new List<int>();

            foreach (var orderStatus in _orderStateRepository.GetAll())
            {
                int count = 0;
                foreach (var order in orders)
                {
                    if(order.OrderStateId == orderStatus.Id)
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
