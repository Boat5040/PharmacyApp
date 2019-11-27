using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.Constants
{
    public static class InstitutionControllerRoute
    {
        public const string GetIndex = ControllerName.Institution + "GetIndex";
        public const string GetCreate = ControllerName.Institution + "GetCreate";
        public const string GetEdit = ControllerName.Institution + "GetEdit";
        public const string GetDelete = ControllerName.Institution + "GetDelete";
        public const string GetVerifyInstitutionName = ControllerName.Institution + "GetVerifyInstitutionName";
        public const string GetVerifyInstitustionTitle = ControllerName.Institution + "GetVerifyInstitustionTitle";
    }
}