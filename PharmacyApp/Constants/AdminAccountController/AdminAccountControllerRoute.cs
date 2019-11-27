using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.Constants
{
    public static class AdminAccountControllerRoute
    {
        public const string GetIndex = ControllerName.AdminAccount + "GetIndex";
        public const string GetCreate = ControllerName.AdminAccount + "GetCreate";
        public const string GetVerifyUserName = ControllerName.AdminAccount + "GetVerifyUserName";
        public const string GetEdit = ControllerName.AdminAccount + "GetEdit";
        public const string GetDelete = ControllerName.AdminAccount + "GetDelete";
        public const string GetReset = ControllerName.AdminAccount + "GetReset";
    }
}