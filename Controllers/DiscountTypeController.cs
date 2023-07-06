using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping_System.Models;
using Shipping_System.Repository.DiscountTypeRepo;

namespace Shipping_System.Controllers
{
    //[Authorize(Roles = "Employee, Admin")]
    public class DiscountTypeController : Controller
    {
        IDiscountTypeRepository _discountTypeRepository;
        public DiscountTypeController(IDiscountTypeRepository discountTypeRepository)
        {
            _discountTypeRepository = discountTypeRepository;

        }
        public IActionResult Index()
        {
            List<DiscountType> discountTypes;
            discountTypes = _discountTypeRepository.GetAll();
            return View(discountTypes);
        }
        public IActionResult Details(int id)
        {
            var city = _discountTypeRepository.GetById(id);
            return View(city);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DiscountType discountType)
        {
            if (ModelState.IsValid)
            {
                _discountTypeRepository.Add(discountType);
                _discountTypeRepository.Save();
                return RedirectToAction("Index");
            }
            return View(discountType);
        }
        public IActionResult Edit(int id)
        {
            DiscountType discount = _discountTypeRepository.GetById(id);
            return View(discount);
        }
        [HttpPost]
        public IActionResult Edit(DiscountType discount)
        {
            if (ModelState.IsValid)
            {
                _discountTypeRepository.Edit(discount);
                _discountTypeRepository.Save();
                return RedirectToAction("Index");
            }
            return View(discount);
        }
        public IActionResult Delete(int id)
        {
            _discountTypeRepository.Delete(id);
            _discountTypeRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
