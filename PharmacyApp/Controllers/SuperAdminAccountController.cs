using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;
using PharmacyApp.Constants;
using PharmacyApp.DataTableCollections;
using PharmacyApp.Helper;
using PharmacyApp.Models;
using PharmacyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Controllers
{
    [Authorize(Roles = PharmacyUserRoles.SuperAdministrator)]
    public class SuperAdminAccountController : BaseController
    {
        public SuperAdminAccountController() { }
        public SuperAdminAccountController(ApplicationUserManager userManager,ApplicationRoleManager roleManager)
            :base(userManager,roleManager:roleManager)
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

        // GET: SuperAdminAccount
        [Route("super-users", Name = SuperAdminAccountControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View(new List<SuperAdminViewModel>());
        }

        [HttpPost]
        [Route("super-users")]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var user = new SuperAdminCollection(DataContext, User.Identity.GetUserId<int>())
                .GetEntityData(startIndex: viewModel.iDisplayStart, pageSize: viewModel.iDisplayLength,
                sortedColumns: viewModel.GetSortedColumns(), totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount, searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<SuperAdminViewModel>(items: user, totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount, sEcho: viewModel.sEcho));

        }

        [Route("new-super-user", Name = SuperAdminAccountControllerRoute.GetCreate)]
        public ActionResult Create()
        {
            ViewBag.Info = SecurityPolicyFactory.GenerateValidationMessage(GetPolicy());
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new-super-user")]
        public async Task<ActionResult> Create(NewSuperAdminViewModel viewModel)
        {
            if (ModelState.IsValid && SecurityPolicyFactory.EnforcePolicy(viewModel.Password,GetPolicy()))
            {
                byte[] uploadedFile = new byte[viewModel.File.InputStream.Length];
                viewModel.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                var user = new ApplicationUser
                {
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    UserName = viewModel.UserName,
                    UserImage = uploadedFile,
                };

                var result = await UserManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    result = null;
                    result = await UserManager.AddToRoleAsync(user.Id, PharmacyUserRoles.SuperAdministrator);

                    if (result.Succeeded)
                        return RedirectToAction(SuperAdminAccountControllerAction.Index);
                    else
                        AddErrors(result);
                }
                else
                    AddErrors(result);
            }
            ViewBag.Info = SecurityPolicyFactory.GenerateValidationMessage(GetPolicy());
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("verify-super-username", Name = SuperAdminAccountControllerRoute.GetVerifyUserName)]
        public async Task<ActionResult> UserNameExists(string userName)
        {
            return Json(await UserManager.FindByNameAsync(userName) == null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("check-super-user-password", Name = SuperAdminAccountControllerRoute.GetValidatePassword)]
        public ActionResult ValidatePassword(string password)
        {
            return Json(SecurityPolicyFactory.EnforcePolicy(password, GetPolicy()), JsonRequestBehavior.AllowGet);
        }


        [Route("super-user-profile", Name = SuperAdminAccountControllerRoute.GetDetails)]
        public async Task<ActionResult> Details(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            return View(new SuperAdminViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = user.Status.ToString(),
                UserId = user.Id,
                UserName = user.UserName,
                Institution = user.Institution == null ? "PharmacyApp" : user.Institution.Name
            });

        }

        [Route("update-super-user", Name = SuperAdminAccountControllerRoute.GetEdit)]
        public async Task<ActionResult> Edit(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            return View(new EditSuperAdminViewModel
            {
                FirstName = user.FirstName,
                LastName =user.LastName,
                UserName = user.UserName,
                IsDisabled = user.Status == UserStatus.Disabled,
                UserId = user.Id
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update-super-user")]
        public async Task<ActionResult> Edit(EditSuperAdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(viewModel.UserId);

                if (user == null)
                    return new HttpNotFoundResult();
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Status = viewModel.IsDisabled ? UserStatus.Disabled : UserStatus.Active;
                user.ModifiedBy = User.Identity.GetUserId<int>();
                user.ModifiedDate = DateTime.Now;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction(SuperAdminAccountControllerAction.Index);
                else
                    AddErrors(result);
            }

            return View(viewModel);
        }

        [Route("delete-super-user", Name = SuperAdminAccountControllerRoute.GetDelete)]
        public async Task<ActionResult> Delete(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            return View(new SuperAdminViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.Id,
                Status = user.Status.ToString(),
                UserName = user.UserName,
                Institution = user.Institution == null ? "PharmacyApp" : user.Institution.Name
            });
        }

        [HttpPost]
        [Route("delete-super-user")]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            user.Status = UserStatus.Deleted;
            user.ModifiedBy = User.Identity.GetUserId<int>();
            user.ModifiedDate = DateTime.Now;

            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction(SuperAdminAccountControllerAction.Index);
            else
            {
                // show error on next response
                TempData[Alerts.Error] = result.Errors.Count() == 1 ? result.Errors.First() : string.Join(",", result.Errors);
                return RedirectToAction(SuperAdminAccountControllerAction.Delete, new { userId = userId });
            }
        }

        [Route("reset-super-user", Name = SuperAdminAccountControllerRoute.GetReset)]
        public async Task<ActionResult> Reset(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            return View(new ResetSuperAdminViewModel
            {
                UserId = user.Id
            });
        }

        [HttpPost]
        [Route("reset-super-user")]
        public async Task<ActionResult> Reset(ResetSuperAdminViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(viewModel.UserId);

                if (user == null)
                    return new HttpNotFoundResult();

                string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);


                var result = await UserManager.ResetPasswordAsync(user.Id, token, viewModel.NewPassword);

                if (result.Succeeded)
                {
                    user.LastPasswordChangedDate = DateTime.Now;

                    var passwordHistories = user.PasswordHistory?.ToObject<PasswordHistories>();

                    if (passwordHistories == null)
                    {
                        passwordHistories = new PasswordHistories();
                    }

                    passwordHistories.Add(new PasswordHistory
                    {
                        ChangeDate = user.LastPasswordChangedDate.Value,
                        ChangedBy = User.Identity.Name,
                        ChangeType = PasswordChangeType.SuperAdminReset,
                        RemoteIPAddress = this.GetRemoteServerIPAddress()
                    });

                    user.PasswordHistory = passwordHistories.ToXmlString();
                    user.Status = UserStatus.Active;
                    user.ModifiedBy = User.Identity.GetUserId<int>();
                    user.ModifiedDate = DateTime.Now;
                    await UserManager.UpdateAsync(user);

                    return RedirectToAction(SuperAdminAccountControllerAction.Index);
                }

                else
                    AddErrors(result);
            }

            return View(viewModel);
        }



    }
}