using PharmacyApp.Constants;
using PharmacyApp.Helper;
using PharmacyApp.Models;
using PharmacyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Controllers
{
    [Authorize(Roles = PharmacyUserRoles.Administrator)]
    public class SecurityPolicyController : BaseController
    {

        public SecurityPolicyController()
        {

        }

        public SecurityPolicyController(ApplicationUserManager userManager)
            :base(userManager,context:new DAL.ApplicationDbContext())
        {

        }

        private SecurityPolicy GetPolicy()
        {
            return DataContext.Institutions.Find(CurrentUser.InstitutionId)
                ?.SecurityPolicy.ToObject<SecurityPolicy>() ?? new SecurityPolicy
                {
                    MaximumPasswordLength = 12,
                    MinimumPasswordLength = 6,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireSpecialCharacter = true,
                    RequireUppercase = true,
                    SpecialCharacters = "@$%&?*"
                };
        }

        [Route("policy", Name = SecurityPolicyControllerRoute.GetIndex)]
        public async Task<ActionResult>Index()
        {
            var policy = await Task.Run(() => GetPolicy());

            var viewModel = new SecurityPolicyViewModel
            {
                MaximumPasswordLength = policy.MaximumPasswordLength,
                MinimumPasswordLength = policy.MinimumPasswordLength,
                RequireDigit = policy.RequireDigit,
                RequireLowercase = policy.RequireLowercase,
                RequireSpecialCharacter = policy.RequireSpecialCharacter,
                RequireUppercase = policy.RequireUppercase,
                SpecialCharacters = policy.SpecialCharacters
            };
            return View(viewModel);
        }

        [Route("policy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SecurityPolicyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var policy = GetPolicy();

                policy.MaximumPasswordLength = viewModel.MaximumPasswordLength;
                policy.MinimumPasswordLength = viewModel.MinimumPasswordLength;
                policy.RequireDigit = viewModel.RequireDigit;
                policy.RequireLowercase = viewModel.RequireLowercase;
                policy.RequireSpecialCharacter = viewModel.RequireSpecialCharacter;
                policy.RequireUppercase = viewModel.RequireUppercase;
                policy.SpecialCharacters = viewModel.SpecialCharacters?.Trim();

                var institution = await DataContext.Institutions.FindAsync(CurrentUser.InstitutionId);
                if (institution != null)
                {
                    institution.SecurityPolicy = policy.ToXmlString();
                    await DataContext.SaveChangesAsync();
                    TempData[Alerts.Info] = "Security policy updated successfully";
                    return RedirectToAction(SecurityPolicyControllerAction.Index);
                }
            }

            return View(viewModel);

        }
    }
}