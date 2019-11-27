using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using PharmacyApp;
using PharmacyApp.Constants;
using PharmacyApp.Models;
using PharmacyApp.Services;
using System;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(PharmacyApp.Startup))]
namespace PharmacyApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureContainer(app);
            ConfigureAuth(app);
            CreatePharmacyUserRoles();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

        }


        private void CreatePharmacyUserRoles()
        {
            var roleManager = ApplicationRoleManager.Instance;

            if (!roleManager.RoleExists(PharmacyUserRoles.SuperAdministrator))
                roleManager.Create(GetRole(PharmacyUserRoles.SuperAdministrator, "Creates Pharmacies and other super administrators."));

            if (!roleManager.RoleExists(PharmacyUserRoles.Administrator))
                roleManager.Create(GetRole(PharmacyUserRoles.Administrator, "Creates branches and users."));

            if (!roleManager.RoleExists(PharmacyUserRoles.ShopKeeper))
                roleManager.Create(GetRole(PharmacyUserRoles.ShopKeeper, "Manage the shops"));

            //if (!roleManager.RoleExists(PharmacyUserRoles.User))
            //    roleManager.Create(GetRole(PharmacyUserRoles.User, "Manage the shops"));

        }

        private void LogError(IdentityResult result)
        {
            if (!result.Succeeded)
                DependencyResolver.Current.GetService<ILoggingService>().Log(new Exception(string.Join("\r\n", result.Errors)));
        }


        private ApplicationRole GetRole(string name, string description) => new ApplicationRole { Name = name, Description = description };

    }


}
