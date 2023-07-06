using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.Repository.CityRepo;
using Shipping_System.Repository.GovernorateRepo;

namespace Shipping_System.Controllers
{
    public class CityController : Controller
    {
        ICityRepository _cityRepository;
        IGovernRepository _governRepository;

        public CityController(ICityRepository cityRepository, IGovernRepository governRepository)
        {
            _cityRepository = cityRepository;
            _governRepository = governRepository;
        }

        [Authorize(Permissions.City.View)]
        public IActionResult Index()
        {
            List<City> cities;
            cities = _cityRepository.GetAll();

            return View(cities);
        }

        [Authorize(Permissions.City.Create)]
        public IActionResult Create(int id)
        {
            ViewData["GovList"] = _governRepository.GetAll();
            return View();
        }

        [Authorize(Permissions.City.Create)]
        [HttpPost]
        public IActionResult Create(City city)
        {
            if (ModelState.IsValid)
            {
                _cityRepository.Add(city);
                _cityRepository.Save();
                return RedirectToAction("Index");
            }
            ViewData["GovList"] = _governRepository.GetAll();
            return View(city);
        }

        [Authorize(Permissions.City.Edit)]
        public IActionResult Edit(int id)
        {
            City city = _cityRepository.GetById(id);
            ViewData["GovList"] = _governRepository.GetAll();
            return View(city);
        }

        [Authorize(Permissions.City.Edit)]
        [HttpPost]
        public IActionResult Edit(City city)
        {
            if (ModelState.IsValid)
            {
                _cityRepository.Edit(city);
                _cityRepository.Save();
                return RedirectToAction("Index");
            }
            ViewData["GovList"] = _governRepository.GetAll();
            return View(city);
        }
        //changeState City
        [Authorize(Permissions.City.Delete)]
        public IActionResult changeState(int id)
        {
            City city = _cityRepository.GetById(id);
            if (city == null)
            {
                return NotFound();
            }
            else
            {
                city.IsDeleted = !city.IsDeleted;
                _cityRepository.Save();
                return Ok();
            }
        }
    }
}
