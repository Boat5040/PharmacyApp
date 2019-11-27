using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;
using PharmacyApp.Constants;
using PharmacyApp.DAL;
using PharmacyApp.DataTableCollections;
using PharmacyApp.Helper;
using PharmacyApp.Models;
using PharmacyApp.Utility;
using PharmacyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Controllers
{
    [Authorize(Roles = PharmacyUserRoles.SuperAdministrator)]

    public class AdminAccountController : BaseController
    {
        public AdminAccountController() { }

        public AdminAccountController(ApplicationUserManager userManager,ApplicationSignInManager signInManager,ApplicationRoleManager roleManager)
            :base(userManager,signInManager,roleManager,context:new ApplicationDbContext())
        {

        }
        [Route("admins", Name = AdminAccountControllerRoute.GetIndex)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("admins")]
        public ActionResult Index(JQueryDataTablesModel viewModel)
        {
            int totalRecordCount;
            int searchRecordCount;

            var user = new AdminCollection(DataContext, User.Identity.GetUserId<int>())
                .GetEntityData(startIndex: viewModel.iDisplayStart, pageSize: viewModel.iDisplayLength,
                sortedColumns: viewModel.GetSortedColumns(), totalRecordCount: out totalRecordCount,
                searchRecordCount: out searchRecordCount, searchString: viewModel.sSearch);

            return Json(new JQueryDataTablesResponse<AdminAccountViewModel>(items: user, totalRecords: totalRecordCount,
                totalDisplayRecords: searchRecordCount, sEcho: viewModel.sEcho));

        }

        [Route("new-admin", Name = AdminAccountControllerRoute.GetCreate)]
        public async Task<ActionResult> Create()
        {
            ViewBag.Institutions = new SelectList(await Task.Run(() => DataContext.Institutions.ToList()), "InstitutionId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new-admin")]
        public async Task<ActionResult> Create(NewAdminAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] uploadedFile = new byte[viewModel.File.InputStream.Length];
                viewModel.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                var user = new ApplicationUser
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,                   
                    UserName = viewModel.UserName,
                    UserImage = uploadedFile,
                    Email = viewModel.Email,
                    InstitutionId = viewModel.InstitutionId.Value,
                    ForceChangePassword = true,
                    CreatedBy = User.Identity.GetUserId<int>(),
                    CreatedDate = DateTime.Now
                    

                };

                string generatedPassword = new PasswordGenerator().GenerateSimple();
                var result = await UserManager.CreateAsync(user, generatedPassword);

                if (result.Succeeded)
                {
                    await SendPasswordMailAsync(user, generatedPassword);

                    result = null;
                    result = await UserManager.AddToRoleAsync(user.Id, PharmacyUserRoles.Administrator);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(AdminAccountControllerAction.Index);
                    }
                    else
                        AddErrors(result);
                }
                else
                    AddErrors(result);
            }

            ViewBag.Institutions = new SelectList(DataContext.Institutions.ToList(), "InstitutionId", "Name", viewModel.InstitutionId.Value);
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("verify-admin-username", Name = AdminAccountControllerRoute.GetVerifyUserName)]
        public async Task<ActionResult> UserNameExists(string userName)
        {
            return Json(await UserManager.FindByNameAsync(userName) == null);
        }

        [Route("update-admin", Name = AdminAccountControllerRoute.GetEdit)]
        public async Task<ActionResult> Edit(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            return View(new EditAdminAccountViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsDisabled = user.Status == UserStatus.Disabled,
                UserId = user.Id,
                UserName = user.UserName,
                Institution = user.Institution.Name,
                Email = user.Email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update-admin")]
        public async Task<ActionResult> Edit(EditAdminAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(viewModel.UserId);

                if (user == null)
                    return new HttpNotFoundResult();

                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Email = viewModel.Email.Trim();
                user.Status = viewModel.IsDisabled ? UserStatus.Disabled : UserStatus.Active;
                user.ModifiedBy = User.Identity.GetUserId<int>();
                user.ModifiedDate = DateTime.Now;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction(AdminAccountControllerAction.Index);
                else
                    AddErrors(result);
            }

            return View(viewModel);
        }


        [Route("delete-admin", Name = AdminAccountControllerRoute.GetDelete)]
        public async Task<ActionResult> Delete(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            return View(new AdminAccountViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = user.Status.ToString(),
                UserId = user.Id,
                Institution = user.Institution.Name,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        [HttpPost]
        [ActionName("Delete")]
        [Route("delete-admin")]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction(AdminAccountControllerAction.Index);
            else
            {
                TempData[Alerts.Error] = result.Errors.Count() == 1 ? result.Errors.First() : string.Join(",", result.Errors);
                return RedirectToAction(AdminAccountControllerAction.Delete, new { userId = userId });
            }
        }

        [Route("reset-admin", Name = AdminAccountControllerRoute.GetReset)]
        public async Task<ActionResult> Reset(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            return View(new AdminAccountViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = user.Status.ToString(),
                UserId = user.Id,
                Institution = user.Institution.Name,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        [HttpPost]
        [ActionName("Reset")]
        [Route("reset-admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetConfirmed(int? userId = null)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(userId.Value);

            if (user == null)
                return new HttpNotFoundResult();

            string generatedPassword = new PasswordGenerator().GenerateSimple();
            var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, token, generatedPassword);
            if (result.Succeeded)
            {
                await SendPasswordMailAsync(user, generatedPassword);

                result = null;
                user.ForceChangePassword = true;
                user.HasPasswordChange = false;
                user.Status = UserStatus.Active;

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

                user.ModifiedBy = User.Identity.GetUserId<int>();
                user.ModifiedDate = DateTime.Now;
                result = await UserManager.UpdateAsync(user);
            }

            if (result.Succeeded)
                return RedirectToAction(AdminAccountControllerAction.Index);
            else
            {
                TempData[Alerts.Error] = result.Errors.Count() == 1 ? result.Errors.First() : string.Join(",", result.Errors);
                return RedirectToAction(AdminAccountControllerAction.Reset, new { userId = userId });
            }
        }

        private Task SendPasswordMailAsync(ApplicationUser user, string generatedPassword)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var smtp = new SmtpClient())
                    {
                        var mail = new MailMessage();
                        mail.To.Add(user.Email);
                        mail.Subject = "Pharmacy App";
                        mail.IsBodyHtml = true;
                        mail.Body = $"Hello {user.FirstName+""+user.LastName},<br /> Your password: <b>{generatedPassword}</b>.<br />Best regards.";
                        smtp.Send(mail);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    Trace.TraceError(ex.ToString());
                }
            });
        }


    }
}