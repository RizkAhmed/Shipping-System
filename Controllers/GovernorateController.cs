using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.Repository.CityRepo;
using Shipping_System.Repository.GovernorateRepo;

namespace Shipping_System.Controllers
{
    public class GovernorateController : Controller
    {
        IGovernRepository _governRepository;
        ICityRepository _cityRepository;
        public GovernorateController(IGovernRepository governRepository, ICityRepository cityRepository)
        {
            _governRepository = governRepository;
            _cityRepository = cityRepository;

        }

        [Authorize(Permissions.Governorate.View)]
        public IActionResult Index(string word)
        {
            List<Governorate> governorates;
            governorates=_governRepository.GetAll();
            return View(governorates);
        }

        [Authorize(Permissions.Governorate.View)]
        public IActionResult Details(int id)
        {
            var cites = _cityRepository.GetAllCitiesByGovId(id);
            ViewData["GovName"] = _governRepository.GetById(id).Name;
            return View(cites);
        }

        [Authorize(Permissions.Governorate.Create)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Permissions.Governorate.Create)]
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

        [Authorize(Permissions.Governorate.Edit)]
        public IActionResult Edit(int id)
        {
            Governorate governorate = _governRepository.GetById(id);
            return View(governorate);
        }

        [Authorize(Permissions.Governorate.Edit)]
        [HttpPost]
        public IActionResult Edit(Governorate governorate)
        {
            var govFDB =_governRepository.GetById(governorate.Id);
            govFDB.Name= governorate.Name;
            if (ModelState.IsValid)
            {
                _governRepository.Edit(govFDB);
                _governRepository.Save();
                return RedirectToAction("Index");
            }
            return View(governorate);
        }

        //changeState governorate
        [Authorize(Permissions.Governorate.Delete)]
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
