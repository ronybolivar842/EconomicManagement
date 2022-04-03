using EconomicManagementAPP.Models;
using EconomicManagementAPP.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class AccountTypesController : Controller
    {
        private readonly IRepositorieAccountTypes repositorieAccountTypes;
        private readonly IUserServices userServices;

        public AccountTypesController(IRepositorieAccountTypes repositorieAccountTypes,
                                      IUserServices userServices)
        {
            this.repositorieAccountTypes = repositorieAccountTypes;
            this.userServices = userServices;
        }

       
        public async Task<IActionResult> Index()
        {
            
            var userId = userServices.GetUserId();
            var accountTypes = await repositorieAccountTypes.GetAccounts(userId);
            return View(accountTypes);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountTypes accountTypes)
        {
            if (!ModelState.IsValid)
            {
                return View(accountTypes);
            }

            accountTypes.UserId = userServices.GetUserId();

            
            var accountTypeExist =
               await repositorieAccountTypes.Exist(accountTypes.Name, accountTypes.UserId);

            if (accountTypeExist)
            {
                
                ModelState.AddModelError(nameof(accountTypes.Name),
                    $"The account {accountTypes.Name} already exist.");

                return View(accountTypes);
            }
            await repositorieAccountTypes.Create(accountTypes);
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryAccountType(string Name)
        {
            var UserId = userServices.GetUserId();
            var accountTypeExist = await repositorieAccountTypes.Exist(Name, UserId);

            if (accountTypeExist)
            {
                
                return Json($"The account {Name} already exist");                                                                
            }

            return Json(true);
        }

        
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var userId = userServices.GetUserId();

            var accountType = await repositorieAccountTypes.GetAccountById(id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }
        [HttpPost]
        public async Task<ActionResult> Modify(AccountTypes accountTypes)
        {
            var userId = userServices.GetUserId();
            var accountType = await repositorieAccountTypes.GetAccountById(accountTypes.Id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccountTypes.Modify(accountTypes);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = userServices.GetUserId();
            var account = await repositorieAccountTypes.GetAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = userServices.GetUserId();
            var account = await repositorieAccountTypes.GetAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var isUsed = await repositorieAccountTypes.AccountTypeIsUsed(id);

            if (isUsed)
            {
                return RedirectToAction("Error", "Home");
            }

            await repositorieAccountTypes.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
