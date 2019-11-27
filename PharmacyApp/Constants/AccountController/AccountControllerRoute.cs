using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.Constants
{
    public static class AccountControllerRoute
    {
        public const string GetLogin = ControllerName.Account + "GetLogin";
        public const string GetLogOff = ControllerName.Account + "GetLogOff";
        public const string GetChangePassword = ControllerName.Account + "GetChangePassword";
        public const string GetUsers = ControllerName.Account + "GetUsers";
        public const string GetDetails = ControllerName.Account + "GetDetails";
        public const string GetValidatePassword = ControllerName.Account + "GetValidatePassword";

    }
}