using Microsoft.AspNetCore.Mvc;
using Shipping_System.Models;
using Shipping_System.Repository.BranchRepo;
using Shipping_System.Repository.CityRepo;
using Shipping_System.Repository.DeliverTypeRepo;
using Shipping_System.Repository.GovernorateRepo;
using Shipping_System.Repository.OrderRepo;
using Shipping_System.Repository.OrderStateRepo;
using Shipping_System.Repository.OrderTypeRepo;
using Shipping_System.Repository.PaymentMethodRepo;
using Shipping_System.Repository.TraderRepo;
using Shipping_System.Repository.ProductRepo;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Shipping_System.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Shipping_System.Constants;
using Shipping_System.Repository.RepresentiveRepo;
using NuGet.Protocol;

namespace Shipping_System.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;
        IBranchRepository _branchRepository;
        IOrderTypeRepository _orderTypeRepository;
        IDeliverTypeRepository _deliverTypeRepository;
        IPaymentMethodRepository _paymentMethodRepository;
        IGovernRepository _ghostRepository;
        ICityRepository _cityRepository;
        IOrderStateRepository _orderStateRepository;
        IProductRepository _productRepository;
        ITraderRepository _traderRepository;
        UserManager<ApplicationUser> _userManager;
        IRepresentativeRepository  _representativeRepository;


        public OrderController(IOrderRepository orderRepository,
            IBranchRepository branchRepository,
            IOrderTypeRepository orderTypeRepository,
            IDeliverTypeRepository deliverTypeRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IGovernRepository governRepository,
            ICityRepository cityRepository,
            IOrderStateRepository orderStateRepository,
            IProductRepository productRepository,
            ITraderRepository traderRepository,
            UserManager<ApplicationUser> userManager,
            IRepresentativeRepository representativeRepository
            )
        {
            _orderRepository = orderRepository;   
            _branchRepository = branchRepository;
            _orderTypeRepository = orderTypeRepository;
            _deliverTypeRepository = deliverTypeRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _ghostRepository = governRepository;
            _cityRepository = cityRepository;
            _orderStateRepository = orderStateRepository;
            _productRepository = productRepository;
            _traderRepository = traderRepository;
            _userManager = userManager;
            _representativeRepository = representativeRepository;

        }
        [Authorize(Permissions.Orderes.View)]
        public IActionResult Index(string word)
        {
        List<Order> orders ;
            if (User.IsInRole("Trader"))
            {
                var username = User.Identity.Name;
                var user = _userManager.FindByEmailAsync(username).Result;
                ViewBag.TraderId = user.Id.ToString();

                orders = _orderRepository.GetAll();
                orders = orders.Where(o => o.TraderId == user.Id).ToList();
            }
            else
            {
                orders = _orderRepository.GetAll();
            }

            List<OrderReporttWithOrderByStatusDateViewModel> ordersViewModel = new List<OrderReporttWithOrderByStatusDateViewModel>();

            foreach (var item in orders)
            {
                City city = _cityRepository.GetById(item.ClientCityId);
                Governorate governorate = _ghostRepository.GetById(item.ClientGovernorateId);
                OrderState orderState = _orderStateRepository.GetById(item.OrderStateId);
                Trader trader = _traderRepository.GetById(item.TraderId);

                OrderReporttWithOrderByStatusDateViewModel ordersViewModelItem = new OrderReporttWithOrderByStatusDateViewModel();
                ordersViewModelItem.Id = item.Id;
                ordersViewModelItem.Date = item.creationDate;
                ordersViewModelItem.Client = item.ClientName;
                ordersViewModelItem.PhoneNumber = item.ClientPhone1;
                ordersViewModelItem.City = city.Name;
                ordersViewModelItem.Governorate = governorate.Name;
                ordersViewModelItem.ShippingPrice = item.ShippingPrice;
                ordersViewModelItem.Status = orderState.Name;
                ordersViewModelItem.Trader = trader.AppUser.Name;

                ordersViewModel.Add(ordersViewModelItem);
            }
            ViewData["OrderStates"] = _orderStateRepository.GetAll();

            return View(ordersViewModel);
        }
        [Authorize(Permissions.Orderes.View)]
        public IActionResult Details(int id)
        {

            Order order = _orderRepository.GetById(id);
            return View(order);
        }


        public IActionResult Invoice(int id)
        {
            var order = _orderRepository.GetById(id);

            var products = _productRepository.GetByOrderNo(order.OrderNo);

            ViewData["Products"] = products;
            return View(order);
        }

        [Authorize(Permissions.Orderes.Create)]
        public IActionResult Create()
        {
            ViewData["DeliverTypes"] = _deliverTypeRepository.GetAll();
            ViewData["OrderTypes"] = _orderTypeRepository.GetAll();
            ViewData["PaymentMethods"] = _paymentMethodRepository.GetAll();
            ViewData["Branches"] = _branchRepository.GetAll();
            ViewData["Governorates"] = _ghostRepository.GetAll();
            ViewBag.OrderNo = Guid.NewGuid().ToString();
            var username = User.Identity.Name;
            var user = _userManager.FindByEmailAsync(username).Result;
            ViewBag.TraderId = user.Id.ToString();
            return View() ;
        }


        [Authorize(Permissions.Orderes.Create)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            ViewData["DeliverTypes"] = _deliverTypeRepository.GetAll();
            ViewData["OrderTypes"] = _orderTypeRepository.GetAll();
            ViewData["PaymentMethods"] = _paymentMethodRepository.GetAll();
            ViewData["Branches"] = _branchRepository.GetAll();
            ViewData["Governorates"] = _ghostRepository.GetAll();

            if (ModelState.IsValid)
            {
                order.TotalWeight = ProductsWeight(order.OrderNo);
                order.ShippingPrice = _orderRepository.CalculateTotalPrice(order) /*+ ProductsCost(order.OrderNo)*/;
                order.OrderPrice = order.ShippingPrice + ProductsCost(order.OrderNo); 
                order.Products = _productRepository.GetByOrderNo(order.OrderNo);
                _orderRepository.Add(order);
                _orderRepository.Save();
                return RedirectToAction("Index");
            }
            return View(order);
        }
        public IActionResult getCitesByGovernrate(int govId) 
        {
            List<City> cities = _cityRepository.GetAllCitiesByGovId(govId);
            return Json(cities);
        }
        [Authorize(Permissions.Orderes.Edit)]
        public IActionResult Edit(int id)
        {
            ViewData["DeliverTypes"] = _deliverTypeRepository.GetAll();
            ViewData["OrderTypes"] = _orderTypeRepository.GetAll();
            ViewData["PaymentMethods"] = _paymentMethodRepository.GetAll();
            ViewData["Branches"] = _branchRepository.GetAll();
            ViewData["Governorates"] = _ghostRepository.GetAll();

            Order order = _orderRepository.GetById(id);

            ViewData["City"] = _cityRepository.GetAll().Where(c=>c.GoverId==order.ClientGovernorateId).ToList();
            order.Products = _productRepository.GetAll().Where(p => p.OrderNO == order.OrderNo).ToList();
            //City city = _cityRepository.GetById(order.ClientCityId);

            //ViewData["City"] = city.Name;
            return View(order);
        }

        [Authorize(Permissions.Orderes.Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order) 
        {
            if (ModelState.IsValid)
            {
                order.TotalWeight = ProductsWeight(order.OrderNo);
                order.ShippingPrice = _orderRepository.CalculateTotalPrice(order) /*+ ProductsCost(order.OrderNo)*/;
                order.OrderPrice = order.ShippingPrice + ProductsCost(order.OrderNo);
                order.Products = _productRepository.GetByOrderNo(order.OrderNo);
                _orderRepository.Edit(order);
                _orderRepository.Save();
                return RedirectToAction("Index");
            }

            ViewData["DeliverTypes"] =  _deliverTypeRepository.GetAll();
            ViewData["OrderTypes"] = _orderTypeRepository.GetAll();
            ViewData["PaymentMethods"] = _paymentMethodRepository.GetAll();
            ViewData["Branches"] = _branchRepository.GetAll();
            ViewData["Governorates"] = _ghostRepository.GetAll();

            return View(order);
        }
        [Authorize(Permissions.Orderes.Delete)]
        public IActionResult Delete(int id)
        {
            Order order = _orderRepository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            _orderRepository.Delete(id);
            _orderRepository.Save();
            return Ok();
        }
        public async Task<IActionResult> Status(int id)
        {
            Order order = _orderRepository.GetById(id);

            var representativesInSameCity = _representativeRepository.GetByBranchId(order.BranchId);

            List<RepresentativeGovBranchPercentageViewModel> viewmodels = new List<RepresentativeGovBranchPercentageViewModel>();

            foreach (var item in representativesInSameCity)
            {
                var user =await _userManager.FindByIdAsync(item.AppUserId);

                RepresentativeGovBranchPercentageViewModel viewmodel = new RepresentativeGovBranchPercentageViewModel()
                {
                    AppUserId = user.Id,
                    Name = user.Name,
                };
                viewmodels.Add(viewmodel);
            }


            ViewData["OrderStatus"] = _orderStateRepository.GetStatusForEmployee();
            ViewData["RepresentativesInSameCity"] = viewmodels;
            return View(order);
        }
        [Authorize(Permissions.Orderes.View)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Status(Order order)
        {
            ViewData["OrderStatus"] = _orderStateRepository.GetAll();
            Order orderFromDB = _orderRepository.GetById(order.Id);
            orderFromDB.OrderStateId = order.OrderStateId;
            _orderRepository.Save();

            return RedirectToAction("Index");
        }

        [Authorize(Permissions.Orderes.View)]
        public IActionResult LinkOrderToRepresentative(string orderId, string repId)
        {
            var order = _orderRepository.GetById(int.Parse(orderId));

            order.RepresentativeId = repId;

            _orderRepository.Save();

            return RedirectToAction("Index","Order");
        }


        [Authorize(Permissions.Orderes.View)]
        public IActionResult GetFilteredOrders(int orderState)
        {
            List<OrderReporttWithOrderByStatusDateViewModel> filteredOrdersViewModel = 
                new List<OrderReporttWithOrderByStatusDateViewModel>();
            List<Order> filteredOrders;
            if (User.IsInRole("Trader"))
            {

                var username = User.Identity.Name;  
                var user = _userManager.FindByNameAsync(username).Result;
                var TraderId = user.Id.ToString();

                filteredOrders = _orderRepository.GetAll();
                filteredOrders = filteredOrders.Where(o => o.TraderId == TraderId).ToList();
            }
            else
            {
                filteredOrders = _orderRepository.GetAll();
            }
            if (orderState == 0)
            {
            }
            else
            {
                filteredOrders = filteredOrders.Where(o => o.OrderStateId == orderState).ToList();
            }
            foreach (var item in filteredOrders)
            {
                City city = _cityRepository.GetById(item.ClientCityId);
                Governorate governorate = _ghostRepository.GetById(item.ClientGovernorateId);
                OrderState orderState_ = _orderStateRepository.GetById(item.OrderStateId);
                Trader trader = _traderRepository.GetById(item.TraderId);
                OrderReporttWithOrderByStatusDateViewModel ordersViewModelItem = 
                    new OrderReporttWithOrderByStatusDateViewModel();
                ordersViewModelItem.Id = item.Id;
                ordersViewModelItem.Date = item.creationDate;
                ordersViewModelItem.Client = item.ClientName;
                ordersViewModelItem.PhoneNumber = item.ClientPhone1;
                ordersViewModelItem.City = city.Name;
                ordersViewModelItem.Governorate = governorate.Name;
                ordersViewModelItem.ShippingPrice = item.ShippingPrice;
                ordersViewModelItem.Status = orderState_.Name;
                ordersViewModelItem.Trader = trader.AppUser.Name;

                filteredOrdersViewModel.Add(ordersViewModelItem);
            }



            return Json(filteredOrdersViewModel);
        }
        [Authorize(Permissions.Orderes.Create)]
        public IActionResult AddProduct(string name, int quantity, decimal weight, decimal price, string orderno)
        {
            var pro = new Product
            {
                OrderNO = orderno,
                Name = name,
                Quantity = quantity,
                Weight = weight,
                Price = price
            };
            _productRepository.Add(pro);
            _productRepository.Save();
            
            return Ok(pro.Id);
        }
        [Authorize(Permissions.Orderes.Create)]
        public IActionResult DeleteProduct(int id)
        {
            if (id == null)
                return BadRequest();
            var pro = _productRepository.GetById(id);
            if (pro == null)
                return NotFound();
            _productRepository.Delete(id);
            return Ok();
        }
        public decimal ProductsWeight(string orderNO)
        {
            var orderProducts = _productRepository.GetAll().Where(p => p.OrderNO == orderNO);
            decimal weight = 0;
            foreach (var product in orderProducts)
            {
                weight += product.Weight * product.Quantity;
            }
            return weight;

        }
        public decimal ProductsCost(string orderNO)
        {
            var orderProducts = _productRepository.GetAll().Where(p=>p.OrderNO==orderNO);
            decimal cost=0;
            foreach(var product in orderProducts)
            {
                cost += product.Price*product.Quantity;
            }
            return cost;
        }


        [Authorize(Permissions.OrderReports.View)]

        public IActionResult OrderReport(string startDate, string endDate, int statusId)
        {
            List<OrderReporttWithOrderByStatusDateViewModel> ordersViewModel = new List<OrderReporttWithOrderByStatusDateViewModel>();

            var orders = _orderRepository.GetAll().ToList();

            foreach (var item in orders)
            {
                OrderReporttWithOrderByStatusDateViewModel ordersViewModelItem = new OrderReporttWithOrderByStatusDateViewModel();
                ordersViewModelItem.SerialNumber = item.Id;
                ordersViewModelItem.StatusId = item.OrderStateId;
                ordersViewModelItem.Status = item.OrderState.Name;
                ordersViewModelItem.Trader = item.Trader.AppUser.Name;
                ordersViewModelItem.Client = item.ClientName;
                ordersViewModelItem.PhoneNumber = item.ClientPhone1;
                ordersViewModelItem.Governorate = item.ClientGovernorate.Name;
                ordersViewModelItem.City = item.ClientCity.Name;
                ordersViewModelItem.OrderPrice = item.OrderPrice;
                ordersViewModelItem.OrderPriceRecieved = item.OrderPriceRecieved;
                ordersViewModelItem.ShippingPrice = item.ShippingPrice;
                ordersViewModelItem.ShippingPriceRecived = item.ShippingPriceRecived;
                ordersViewModelItem.CompanyRate = item.RepresentativeId==null? 0 : (item.Representative.CompanyPercentageOfOrder * ordersViewModelItem.ShippingPrice) / 100M;
                ordersViewModelItem.Date = item.creationDate;
                ordersViewModel.Add(ordersViewModelItem);

            }

            if (statusId != 0)
            {
                ordersViewModel = ordersViewModel.Where(o=>o.StatusId == statusId).ToList();
            }

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate).AddDays(1);
                ordersViewModel = ordersViewModel.Where(o => o.Date >= start && o.Date < end).ToList();

            }

            ViewBag.status = _orderStateRepository.GetAll();

            return View(ordersViewModel);
        }



    

}
}
