using jQuery.DataTables.Mvc;
using Microsoft.AspNet.Identity;
using PharmacyApp.Areas.Administration.DataTableCollections;
using PharmacyApp.Areas.Administration.Models;
using PharmacyApp.Constants;
using PharmacyApp.Controllers;
using PharmacyApp.DAL;
using PharmacyApp.Helper;
using PharmacyApp.Models;
using PharmacyApp.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Areas.Administration.Controllers
{
    [Authorize(Roles = PharmacyUserRoles.Administrator)]

    public class InstitutionUserController : BaseController
    {
        // GET: Administration/InstitutionUser


            public InstitutionUserController()
            {

            }

            public InstitutionUserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
                ApplicationRoleManager roleManager) : base(userManager, signInManager, roleManager, context: new ApplicationDbContext())
            {

            }

            private string GetRoles(ApplicationUser user)
            {
                string roles = string.Empty;

                foreach (var role in user.Roles)
                {
                    roles += RoleManager.FindById(role.RoleId).Name + ",";
                }
                return roles = roles.Substring(0, roles.Length - 2);
            }

            [Route("intitution-users", Name = InstitutionUserControllerRoute.GetIndex)]
            public ActionResult Index()
            {
                return View();
            }

            [HttpPost]
            [Route("intitution-users")]
            public ActionResult Index(JQueryDataTablesModel viewModel)
            {
                int totalRecordCount;
                int searchRecordCount;

                var users = new InstitutionUserCollection(DataContext, User.Identity.GetUserId<int>())
                    .GetEntityData(startIndex: viewModel.iDisplayStart,
                    pageSize: viewModel.iDisplayLength, sortedColumns: viewModel.GetSortedColumns(),
                    totalRecordCount: out totalRecordCount, searchRecordCount: out searchRecordCount,
                    searchString: viewModel.sSearch);

                return Json(new JQueryDataTablesResponse<InstitutionUserViewModel>(items: users,
                    totalRecords: totalRecordCount,
                    totalDisplayRecords: searchRecordCount,
                    sEcho: viewModel.sEcho));
            }

            [Route("new-institution-user", Name = InstitutionUserControllerRoute.GetCreate)]
            public async Task<ActionResult> Create()
            {
                ViewBag.Roles = new MultiSelectList(await Task.Run(() => RoleManager.Roles.Where(r =>
                  !r.Name.Equals(PharmacyUserRoles.SuperAdministrator))), "Id", "Name");

                var user = DataContext.Users.Find(User.Identity.GetUserId<int>());
                ViewBag.Branches = new SelectList(/*await Task.Run(() => */DataContext.Branches.Where(s => s.InstitutionId == (int)user.InstitutionId), "BranchId", "Name");

                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("new-institution-user")]
            public async Task<ActionResult> Create(NewInstitutionUserViewModel viewModel)
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
                        Email = viewModel.Email,
                        InstitutionId = (await UserManager.FindByIdAsync(User.Identity.GetUserId<int>())).InstitutionId.Value,
                        BranchId = viewModel.BranchId.Value,
                        ForceChangePassword = true,
                        UserImage = uploadedFile,
                        CreatedBy = User.Identity.GetUserId<int>(),
                        CreatedDate = DateTime.Now

                    };

                    string generatedPassword = new PasswordGenerator().GenerateSimple();
                    var result = await UserManager.CreateAsync(user, generatedPassword);

                    if (result.Succeeded)
                    {
                        await SendPasswordMailAsync(user, generatedPassword);

                        result = null;

                        List<string> roles = new List<string>();
                        foreach (int roleId in viewModel.RoleIds)
                            roles.Add((await RoleManager.FindByIdAsync(roleId)).Name);

                        result = await UserManager.AddToRolesAsync(user.Id, roles.ToArray());

                        if (result.Succeeded)
                        {
                            return RedirectToAction(InstitutionControllerAction.Index);
                        }

                        else
                            AddErrors(result);
                    }
                    else
                        AddErrors(result);
                }
                ModelState.AddModelError("", "Please select at least one role");
                ViewBag.Roles = new MultiSelectList(await Task.Run(() => RoleManager.Roles.Where(r =>
                  !r.Name.Equals(PharmacyUserRoles.SuperAdministrator))), "Id", "Name", viewModel.RoleIds);
                return View(viewModel);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("verify-institution-user-username", Name = InstitutionUserControllerRoute.GetVerifyUserName)]
            public async Task<ActionResult> UserNameExists(string userName)
            {
                return Json(await UserManager.FindByNameAsync(userName) == null);
            }

            [Route("update-institution-user", Name = InstitutionUserControllerRoute.GetEdit)]
            public async Task<ActionResult> Edit(int? userId = null)
            {
                if (userId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var user = await UserManager.FindByIdAsync(userId.Value);

                if (user == null)
                    return new HttpNotFoundResult();

                if (user.InstitutionId != CurrentUser.InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                ViewBag.Roles = new MultiSelectList(await Task.Run(() => RoleManager.Roles.Where(r =>
               !r.Name.Equals(PharmacyUserRoles.SuperAdministrator))), "Id", "Name", user.Roles.Select(r => r.RoleId));

                return View(new EditInstitutionUserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsDisabled = user.Status == UserStatus.Disabled,
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleIds = user.Roles.Select(r => r.RoleId).ToList()
                });
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [Route("update-institution-user")]
            public async Task<ActionResult> Edit(EditInstitutionUserViewModel viewModel)
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByIdAsync(viewModel.UserId);

                    if (user == null)
                        return new HttpNotFoundResult();

                    if (user.InstitutionId != (await UserManager.FindByIdAsync(User.Identity.GetUserId<int>())).InstitutionId)
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                    user.FirstName = viewModel.FirstName;
                    user.LastName = viewModel.LastName;
                    user.Email = viewModel.Email;
                    user.Status = viewModel.IsDisabled ? UserStatus.Disabled : UserStatus.Active;
                    user.ModifiedBy = User.Identity.GetUserId<int>();
                    user.ModifiedDate = DateTime.Now;

                    var result = await UserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        List<string> roles = new List<string>();
                        foreach (int roleId in viewModel.RoleIds)
                            roles.Add((await RoleManager.FindByIdAsync(roleId)).Name);

                        await UserManager.RemoveFromRolesAsync(user.Id, user.Roles.Select(r => RoleManager.FindById(r.RoleId).Name).ToArray());
                        await UserManager.AddToRolesAsync(user.Id, roles.ToArray());

                        return RedirectToAction(InstitutionUserControllerAction.Index);
                    }
                    else
                        AddErrors(result);
                }

                ModelState.AddModelError("", "Please select at least one role");
                ViewBag.Roles = new MultiSelectList(await Task.Run(() => RoleManager.Roles.Where(r =>
               !r.Name.Equals(PharmacyUserRoles.SuperAdministrator))), "Id", "Name", viewModel.RoleIds);

                return View(viewModel);
            }


            [Route("delete-institution-user", Name = InstitutionUserControllerRoute.GetDelete)]
            public async Task<ActionResult> Delete(int? userId = null)
            {
                if (userId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var user = await UserManager.FindByIdAsync(userId.Value);

                if (user == null)
                    return new HttpNotFoundResult();

                if (user.InstitutionId != (await UserManager.FindByIdAsync(User.Identity.GetUserId<int>())).InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                return View(new InstitutionUserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Status = user.Status.ToString(),
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = GetRoles(user)
                });
            }

            [HttpPost]
            [ActionName("Delete")]
            [Route("delete-institution-user")]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> DeleteConfirmed(int? userId = null)
            {
                if (userId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var user = await UserManager.FindByIdAsync(userId.Value);

                if (user == null)
                    return new HttpNotFoundResult();

                if (user.InstitutionId != (await UserManager.FindByIdAsync(User.Identity.GetUserId<int>())).InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                user.Status = UserStatus.Deleted;
                user.ModifiedBy = User.Identity.GetUserId<int>();
                user.ModifiedDate = DateTime.Now;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction(InstitutionUserControllerAction.Index);
                else
                {
                    TempData[Alerts.Error] = result.Errors.Count() == 1 ? result.Errors.First() : string.Join(",", result.Errors);
                    return RedirectToAction(InstitutionUserControllerAction.Delete, new { userId = userId });
                }
            }

            [Route("reset-institution-user", Name = InstitutionUserControllerRoute.GetReset)]
            public async Task<ActionResult> Reset(int? userId = null)
            {
                if (userId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var user = await UserManager.FindByIdAsync(userId.Value);

                if (user == null)
                    return new HttpNotFoundResult();

                if (user.InstitutionId != (await UserManager.FindByIdAsync(User.Identity.GetUserId<int>())).InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                return View(new InstitutionUserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Status = user.Status.ToString(),
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = GetRoles(user)
                });
            }

            [HttpPost]
            [ActionName("Reset")]
            [Route("reset-institution-user")]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> ResetConfirmed(int? userId = null)
            {
                if (userId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var user = await UserManager.FindByIdAsync(userId.Value);

                if (user == null)
                    return new HttpNotFoundResult();

                if (user.InstitutionId != (await UserManager.FindByIdAsync(User.Identity.GetUserId<int>())).InstitutionId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                string generatedPassword = new PasswordGenerator().GenerateSimple();
                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = await UserManager.ResetPasswordAsync(user.Id, token, generatedPassword);
                if (result.Succeeded)
                {
                    await SendPasswordMailAsync(user, generatedPassword);

                    result = null;
                    user.LastPasswordChangedDate = DateTime.Now;
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
                        ChangeType = PasswordChangeType.AdminReset,
                        RemoteIPAddress = this.GetRemoteServerIPAddress()
                    });

                    user.PasswordHistory = passwordHistories.ToXmlString();

                    user.ModifiedBy = User.Identity.GetUserId<int>();
                    user.ModifiedDate = DateTime.Now;
                    await UserManager.UpdateAsync(user);
                    return RedirectToAction(InstitutionUserControllerAction.Index);
                }
                TempData[Alerts.Error] = result.Errors.Count() == 1 ? result.Errors.First() : string.Join(",", result.Errors);
                return RedirectToAction(InstitutionUserControllerAction.Reset, new { userId = userId });
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
                            mail.Subject = "Pharmacy App ";
                            mail.IsBodyHtml = true;
                            mail.Body = $"Hello {user.FirstName + " " + user.LastName},<br /> Your password: <b>{generatedPassword}</b>.<br />Best regards.";
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
