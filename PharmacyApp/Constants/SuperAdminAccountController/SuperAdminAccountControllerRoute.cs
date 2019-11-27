using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.Constants
{
    public static class SuperAdminAccountControllerRoute
    {
        public const string GetIndex =AreaNames.Admin + ControllerName.SuperAdminAccount + "GetIndex";
        public const string GetCreate = ControllerName.SuperAdminAccount + "GetCreate";
        public const string GetVerifyUserName = ControllerName.SuperAdminAccount + "GetVerifyUserName";
        public const string GetEdit = ControllerName.SuperAdminAccount + "GetEdit";
        public const string GetDelete = ControllerName.SuperAdminAccount + "GetDelete";
        public const string GetDetails = ControllerName.SuperAdminAccount + "GetDetails";
        public const string GetReset = ControllerName.SuperAdminAccount + "GetReset";
        public const string GetValidatePassword = ControllerName.SuperAdminAccount + "GetValidatePassword";
    }
}