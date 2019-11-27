using Boilerplate.Web.Mvc.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PharmacyApp.Constants;
using PharmacyApp.DAL;
using PharmacyApp.Models;
using System.Web;
using System.Web.Mvc;
using System;

namespace PharmacyApp.Controllers
{
    [NoCache(Duration = 0)]
    [NoLowercaseQueryString]

    public class BaseController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;


        public BaseController()
        {
            DataContext = new ApplicationDbContext();
        }

        public BaseController(ApplicationUserManager userManager = null,ApplicationSignInManager signInManager = null,ApplicationRoleManager roleManager = null,ApplicationDbContext context = null)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            DataContext = context;
        }

        public ApplicationDbContext DataContext { get; set; }

        public ApplicationUser CurrentUser => UserManager.FindById(User.Identity.GetUserId<int>());

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }

            private set
            {
                _roleManager = value;
            }
        }

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(HomeControllerAction.Index,ControllerName.Home);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DataContext != null)
                    DataContext.Dispose();

                if (UserManager != null)
                    UserManager.Dispose();

                if (SignInManager != null)
                    SignInManager.Dispose();

                if (RoleManager != null)
                    RoleManager.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}