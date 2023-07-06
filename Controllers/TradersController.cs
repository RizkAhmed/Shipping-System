using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.Repository.BranchRepo;
using Shipping_System.Repository.CityRepo;
using Shipping_System.Repository.GovernorateRepo;
using Shipping_System.Repository.OrderRepo;
using Shipping_System.Repository.TraderRepo;
using Shipping_System.ViewModels;
using Shipping_System.ViewModels.TraderViewModel;

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


        public TradersController
            (
            ITraderRepository traderRepository,
            IGovernRepository governRepository,
            ICityRepository cityRepository,
            IBranchRepository branchRepository,
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager

            )
        {
            _traderRepository = traderRepository;
            _governRepository = governRepository;
            _cityRepository = cityRepository;
            _branchRepository = branchRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        [Authorize(Permissions.Traders.View)]
        public IActionResult Index()
        {
            var traders = _traderRepository.GetAll().ToList();
            return View(traders);
        }

        [Authorize(Permissions.Traders.Create)]
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

        [Authorize(Permissions.Traders.Create)]
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
        [Authorize(Permissions.Traders.Edit)]
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

        [Authorize(Permissions.Traders.Edit)]
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
            var trader = new Trader
            {
                AppUserId = user.Id,
                GoverId = model.GoverId,
                BranchId = model.BranchId,
                CityId = model.CityId,
                SpecialPickupCost = model.SpecialPickupCost,
                StoreName = model.StoreName,
                TraderTaxForRejectedOrders = model.TraderTaxForRejectedOrders,
                IsDeleted = model.IsDeleted,
            };
            _traderRepository.Edit(trader);
            _traderRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Representatives.Delete)]
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

        [Authorize(Permissions.Traders.Delete)]
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
        public IActionResult Home()
        {
            //Claim nameClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            //var TraderId = int.Parse(nameClaim.Value);

            List<Order> orders = _orderRepository.GetAll().Where(o => o.TraderId == "07f7857d-6375-477f-a595-123054c38133").ToList();

            OrderStatusViewModel orderStatusViewModel = new OrderStatusViewModel();

            orderStatusViewModel.NewCount = orders.Where(O => O.OrderStateId == 1).Count();
            orderStatusViewModel.pendingCount = orders.Where(O => O.OrderStateId == 2).Count();
            orderStatusViewModel.The_order_has_been_deliveredCount = orders.Where(O => O.OrderStateId == 3).Count();
            orderStatusViewModel.sent_delivered_handedCount = orders.Where(O => O.OrderStateId == 4).Count();
            orderStatusViewModel.Can_not_reachCount = orders.Where(O => O.OrderStateId == 5).Count();
            orderStatusViewModel.postponedCount = orders.Where(O => O.OrderStateId == 6).Count();
            orderStatusViewModel.Partially_deliveredCount = orders.Where(O => O.OrderStateId == 7).Count();
            orderStatusViewModel.Canceled_by_ClientCount = orders.Where(O => O.OrderStateId == 8).Count();
            orderStatusViewModel.Refused_with_paymentCount = orders.Where(O => O.OrderStateId == 9).Count();
            orderStatusViewModel.Refused_with_part_paymentCount = orders.Where(O => O.OrderStateId == 10).Count();
            orderStatusViewModel.Rejected_and_not_paidCount = orders.Where(O => O.OrderStateId == 11).Count();

            return View(orderStatusViewModel);


        }
    }
}
