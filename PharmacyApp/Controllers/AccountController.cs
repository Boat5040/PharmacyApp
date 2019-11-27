using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PharmacyApp.Constants;
using PharmacyApp.Helper;
using PharmacyApp.ViewModels;
using PharmacyApp.Models;

namespace PharmacyApp.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController()
        {

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
            : base(userManager, signInManager, roleManager)
        {

        }

        private void CreateDefaultUser()
        {
            string Password = "P@$$w0rd01";
            if (UserManager.FindByName("Super") == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Super",
                    FirstName = "Kwasi",
                    LastName = "Boateng",
                    Email = "Boat5040@yahoo.com",
                    CreatedBy = 0,
                    CreatedDate = DateTime.Now
                };
                var result = UserManager.Create(user, Password);
                UserManager.AddToRoles(user.Id, PharmacyUserRoles.SuperAdministrator);
            }
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


        //
        // GET: /Account/Login
        [AllowAnonymous]
        [Route("", Name = AccountControllerRoute.GetLogin)]
        public ActionResult Login(string returnUrl)
        {
            CreateDefaultUser();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("")]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null || user.Status == UserStatus.Deleted)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
            if (user == null || user.Status == UserStatus.Disabled)
            {
                ModelState.AddModelError("", "Your account has been disabled.");
                return View(model);
            }
            if (user == null || user.Status == UserStatus.Expired)
            {
                ModelState.AddModelError("", "Your password is expired. Contact administrator");
                return View(model);
            }


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    if (user.ForceChangePassword && !user.HasPasswordChange)
                    {
                        TempData[Alerts.Warn] = $"{user.FirstName + " " + user.LastName}, please change your password";
                    }
                    else
                    {
                        TempData[Alerts.Success] = $"Welcome {user.FirstName + " " + user.LastName}";
                    }

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "You've been locked out. Wait 5 minutes.");
                    return View(model);
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [Authorize]
        [Route("profile", Name = AccountControllerRoute.GetDetails)]
        public async Task<ActionResult> Details()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            string roles = string.Empty;

            foreach (var role in user.Roles)
            {
                roles += RoleManager.FindById(role.RoleId).Name + ",";
            }

            roles = roles.Substring(0, roles.Length - 2);

            return View(new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Institution = user.Institution == null ? "PharmacyApp" : user.Institution.Name,
                Roles = roles,
                Status = user.Status,
                UserName = user.UserName
            });
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("logout", Name = AccountControllerRoute.GetLogOff)]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction(AccountControllerAction.Login,ControllerName.Account);
        }

        [Authorize]
        [Route("change-password", Name = AccountControllerRoute.GetChangePassword)]
        public ActionResult ChangePassword()
        {
            ViewBag.Info = SecurityPolicyFactory.GenerateValidationMessage(GetPolicy());
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid && SecurityPolicyFactory.EnforcePolicy(viewModel.NewPassword, GetPolicy()))
            {
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), viewModel.OldPassword, viewModel.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                    if (user != null)
                    {
                        user.HasPasswordChange = true;
                        user.LastPasswordChangedDate = DateTime.Now;

                        var PasswordHistories = user.PasswordHistory?.ToObject<PasswordHistories>();
                        if (PasswordHistories == null)
                        {
                            PasswordHistories = new PasswordHistories();
                        }

                        PasswordHistories.Add(new PasswordHistory
                        {
                            ChangeDate = user.LastPasswordChangedDate.Value,
                            ChangedBy = User.Identity.Name,
                            ChangeType = PasswordChangeType.SelfReset,
                            RemoteIPAddress = this.GetRemoteServerIpAddress()
                        });

                        user.PasswordHistory = PasswordHistories.ToXmlString();

                        user.Status = UserStatus.Active;
                        user.ModifiedBy = User.Identity.GetUserId<int>();
                        user.ModifiedDate = DateTime.Now;
                        await UserManager.UpdateAsync(user);
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        TempData[Alerts.Success] = "Password changed successfully";
                    }

                    return RedirectToAction(HomeControllerAction.Index, ControllerName.Home);
                }
                AddErrors(result);
            }
            ViewBag.Info = SecurityPolicyFactory.GenerateValidationMessage(GetPolicy());
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("check-admin-password", Name = AccountControllerRoute.GetValidatePassword)]
        public ActionResult ValidatePassword(string newPassword)
        {
            return Json(SecurityPolicyFactory.EnforcePolicy(newPassword, GetPolicy()), JsonRequestBehavior.AllowGet);
        }


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}