using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping_System.Constants;
using Shipping_System.Models;
using Shipping_System.Repository.BranchRepo;

namespace Shipping_System.Controllers
{
    public class BranchesController : Controller
    {
        IBranchRepository _branchRepository;

        public BranchesController(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        [Authorize(Permissions.Branches.View)]
        public IActionResult Index()
        {
            List<Branch> branches;
           branches=_branchRepository.GetAll();
            return View(branches);

        }
        [Authorize(Permissions.Branches.Create)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Permissions.Branches.Create)]
        [HttpPost]
        public IActionResult Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                _branchRepository.Add(branch);
                _branchRepository.Save();
                return RedirectToAction("Index");
            }

            return View(branch);
        }

        [Authorize(Permissions.Branches.Edit)]
        public IActionResult Edit(int id)
        {
            Branch branch = _branchRepository.GetById(id);
            return View(branch);
        }

        [Authorize(Permissions.Branches.Edit)]
        [HttpPost]
        public IActionResult Edit(Branch branch)
        {

            if (ModelState.IsValid)
            {
                _branchRepository.Edit(branch);
                _branchRepository.Save();
                return RedirectToAction("Index");
            }
            return View(branch);

        }

        [Authorize(Permissions.Branches.Delete)]
        public IActionResult changeState(int id)
        {
            Branch branch = _branchRepository.GetById(id);
            if (branch == null)
            {
                return NotFound();
            }
            else
            {
                branch.IsDeleted = !branch.IsDeleted;
                _branchRepository.Save();
                return Ok();
            }
        }
    }
}
