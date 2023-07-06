using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.Repository.BranchRepo;
using Shipping_System.Repository.DiscountTypeRepo;
using Shipping_System.Repository.GovernorateRepo;
using Shipping_System.Repository.RepresentiveRepo;
using Shipping_System.ViewModels;

namespace Shipping_System.Controllers
{
    public class RepresentativesController : Controller
    {
        IRepresentativeRepository _representativeRepostiory;
        IGovernRepository _governRepository;
        IDiscountTypeRepository _discountTypeRepository;
        IBranchRepository _branchRepository;
        UserManager<ApplicationUser> _userManager;

        public RepresentativesController(
            IRepresentativeRepository representativeRepository,
            IGovernRepository governRepository,
            IDiscountTypeRepository discountTypeRepository,
            IBranchRepository branchRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _representativeRepostiory = representativeRepository;
            _governRepository = governRepository;
            _branchRepository = branchRepository;
            _userManager = userManager;
            _discountTypeRepository = discountTypeRepository;
        }
        [Authorize(Permissions.Representatives.View)]
        public IActionResult Index()
        {
            List<Representative> reps = _representativeRepostiory.GetAll();
            return View(reps);
        }
        //public IActionResult Details(int id)
        //{
        //    var rep = _representativeRepostiory.GetById(id);
        //    rep.Governorate = _governRepository.GetById(rep.GovernorateId);
        //    rep.Branch = _branchRepository.GetById(rep.BranchId);
        //    rep.DiscountType = _discountTypeRepository.GetById(rep.DiscountTypeId);

        //    return View(rep);
        //}

        [Authorize(Permissions.Representatives.Create)]
        public async Task<IActionResult> Create()
        {
            var repViewModel = new RepresentativeGovBranchPercentageViewModel
            {
                Governorates = _governRepository.GetAll(),
                Branchs = _branchRepository.GetAll(),
                DiscountTypes = _discountTypeRepository.GetAll()
            };
            return View(repViewModel);
        }

        [Authorize(Permissions.Representatives.Create)]
        [HttpPost]
        public async Task<IActionResult> Create(RepresentativeGovBranchPercentageViewModel repViewModel)
        {
            repViewModel.Governorates = _governRepository.GetAll();
            repViewModel.Branchs = _branchRepository.GetAll();
            repViewModel.DiscountTypes = _discountTypeRepository.GetAll();
            if (!ModelState.IsValid)
            {
                return View(repViewModel);
            }

            if (await _userManager.FindByEmailAsync(repViewModel.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is already exist");
                return View(repViewModel);
            }
            var user = new ApplicationUser
            {
                Email = repViewModel.Email,
                UserName = repViewModel.Email,
                Name = repViewModel.Name,
                PhoneNumber = repViewModel.Phone,
                Address = repViewModel.Address,
                BranchId = repViewModel.BranchId,
            };

            var result = await _userManager.CreateAsync(user, repViewModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("DiscountTypeId", error.Description);
                    return View(repViewModel);
                }
            }
            await _userManager.AddToRoleAsync(user, Roles.Representative.ToString());
            await _userManager.SetPhoneNumberAsync(user, repViewModel.Phone);

            var rep = new Representative
            {
                AppUserId = user.Id,
                CompanyPercentageOfOrder = repViewModel.CompanyPercentageOfOrder,
                GovernorateId = repViewModel.GovernorateId,
                BranchId = repViewModel.BranchId,
                DiscountTypeId = repViewModel.DiscountTypeId,
                IsDeleted = repViewModel.IsDeleted,
            };
            _representativeRepostiory.Add(rep);
            _representativeRepostiory.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Representatives.Edit)]
        public async Task<IActionResult> Edit(string AppUserId)
        {
            if (AppUserId == null)
                return BadRequest();
            var rep = _representativeRepostiory.GetById(AppUserId);
            if (rep == null)
                return NotFound();
            var user = await _userManager.FindByIdAsync(AppUserId);
            var repViewModel = new RepresentativeGovBranchPercentageViewModel
            {
                Name = user.Name,
                Address = user.Address,
                Email = user.Email,
                Password = "Dummy123+",
                Phone = user.PhoneNumber,
                BranchId = rep.BranchId,
                DiscountTypeId = rep.DiscountTypeId,
                GovernorateId = rep.GovernorateId,
                IsDeleted = rep.IsDeleted,
                CompanyPercentageOfOrder = rep.CompanyPercentageOfOrder,
                Governorates = _governRepository.GetAll(),
                Branchs = _branchRepository.GetAll(),
                DiscountTypes = _discountTypeRepository.GetAll()
            };
            return View(repViewModel);
        }

        [Authorize(Permissions.Representatives.Edit)]
        [HttpPost]
        public async Task<IActionResult> Edit(RepresentativeGovBranchPercentageViewModel repViewModel)
        {
            var user = await _userManager.FindByIdAsync(repViewModel.AppUserId);
            if (user == null)
                return NotFound();

            repViewModel.Governorates = _governRepository.GetAll();
            repViewModel.Branchs = _branchRepository.GetAll();
            repViewModel.DiscountTypes = _discountTypeRepository.GetAll();
            if (!ModelState.IsValid)
            {
                return View(repViewModel);
            }

            var checkUser = await _userManager.FindByEmailAsync(repViewModel.Email);
            if (checkUser != null && checkUser.Id != repViewModel.AppUserId)
            {
                ModelState.AddModelError("Email", "Email is already exist");
                return View(repViewModel);
            }
            user.Name = repViewModel.Name;
            user.Email = repViewModel.Email;
            user.UserName = repViewModel.Email;
            user.Address = repViewModel.Address;
            user.BranchId = repViewModel.BranchId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("DiscountTypeId", error.Description);
                    return View(repViewModel);
                }
            }
            var rep = new Representative
            {
                AppUserId = repViewModel.AppUserId,
                CompanyPercentageOfOrder = repViewModel.CompanyPercentageOfOrder,
                GovernorateId = repViewModel.GovernorateId,
                BranchId = repViewModel.BranchId,
                DiscountTypeId = repViewModel.DiscountTypeId,
                IsDeleted = repViewModel.IsDeleted,
            };
            _representativeRepostiory.Edit(rep);
            _representativeRepostiory.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Permissions.Representatives.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();
            var rep = _representativeRepostiory.GetById(id);
            if (rep == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            user.IsDeleted = true;

            await _userManager.UpdateAsync(user);

            _representativeRepostiory.Delete(id);
            _representativeRepostiory.Save();

            return Content("sucsses");
        }
        [Authorize(Permissions.Representatives.Delete)]
        public async Task<IActionResult> changeState(string id)
        {
            if (id == null)
                return BadRequest();
            var rep = _representativeRepostiory.GetById(id);
            if (rep == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            user.IsDeleted = !user.IsDeleted;
            rep.IsDeleted = !rep.IsDeleted;
            _representativeRepostiory.Save();
            return Ok();
        }


    }
}

