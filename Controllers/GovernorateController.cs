using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping_System.Models;
using Shipping_System.Repository.CityRepo;
using Shipping_System.Repository.GovernorateRepo;

namespace Shipping_System.Controllers
{
    [Authorize(Roles = "Employee, Admin")]
    public class GovernorateController : Controller
    {
        IGovernRepository _governRepository;
        ICityRepository _cityRepository;
        public GovernorateController(IGovernRepository governRepository, ICityRepository cityRepository)
        {
            _governRepository = governRepository;
            _cityRepository = cityRepository;

        }
        public IActionResult Index(string word)
        {
            List<Governorate> governorates;
            if (string.IsNullOrEmpty(word))
            {
                governorates = _governRepository.GetAll();
            }
            else
            {
                governorates = _governRepository.GetAll().Where(
                                e => e.Name.ToLower().Contains(word.ToLower())).ToList();
            }
            return View(governorates);
        }
        public IActionResult Details(int id)
        {
            var cites = _cityRepository.GetAllCitiesByGovId(id);
            ViewData["GovName"] = _governRepository.GetById(id).Name;
            return View(cites);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Governorate governorate)
        {
            if (ModelState.IsValid)
            {

                _governRepository.Add(governorate);
                _governRepository.Save();
                return RedirectToAction("Index");
            }
            return View(governorate);
        }
        public IActionResult Edit(int id)
        {
            Governorate governorate = _governRepository.GetById(id);
            return View(governorate);
        }
        [HttpPost]
        public IActionResult Edit(Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                _governRepository.Edit(governorate);
                _governRepository.Save();
                return RedirectToAction("Index");
            }
            return View(governorate);
        }

        //changeState governorate
        public IActionResult changeState(int id)
        {
            Governorate governorate = _governRepository.GetById(id);
            if (governorate == null)
            {
                return NotFound();
            }
            else
            {
                governorate.IsDeleted = !governorate.IsDeleted;
                _governRepository.Save();
                return Ok();
            }
        }
    }
}
