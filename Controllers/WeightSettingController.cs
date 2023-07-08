using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.Repository.WeightSettingRepo;

namespace Shipping_System.Controllers
{
    public class WeightSettingController : Controller
    {

        private readonly IWeightSettingRepository _weightSettingRepository;

        public WeightSettingController(IWeightSettingRepository weightSettingRepository)
        {
            _weightSettingRepository = weightSettingRepository;
        }
        [Authorize(Permissions.WeightSetting.View)]

        public IActionResult Index()
        {
            var weightSettings = _weightSettingRepository.GetAll();
            return View(weightSettings);
        }

        [Authorize(Permissions.WeightSetting.Edit)]

        public IActionResult Edit(int id)
        {
            var weightSetting = _weightSettingRepository.GetById(id);
            if (weightSetting == null)
            {
                return NotFound();
            }
            return View(weightSetting);
        }

        [Authorize(Permissions.WeightSetting.Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,DefaultSize,PriceForEachExtraKilo")] WeightSetting weightSetting)
        {
            if (id != weightSetting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _weightSettingRepository.Update(weightSetting);
                    _weightSettingRepository.Save();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeightSettingExists(weightSetting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(weightSetting);
        }

        private bool WeightSettingExists(int id)
        {
            return _weightSettingRepository.GetById(id) != null;
        }
    }
}

