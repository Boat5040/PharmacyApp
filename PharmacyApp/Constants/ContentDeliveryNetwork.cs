using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.Constants
{
    public static class ContentDeliveryNetwork
    {
        public  static class Google
        {
            public const string Domain = "ajax.googleapis.com";
            public const string Domain2 = "fonts.googleapis.com";
            public const string Domain3 = "www.google-analytics.com";
            public const string Domain4 = "maps.googleapis.com";
            public const string JQueryUrl = "//ajax.googleapis.com/ajax/libs/jquery/2.2.3/jquery.min.js";
            public const string MapUrl = "https://maps.googleapis.com/maps/api/js";
        }


        public static class MaxCdn
        {
            public const string Domain = "maxcdn.bootstrapcdn.com";
            public const string FontAwesomeUrl = "//maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css";

        }

        public static class GStatic
        {
            public const string Domain = "fonts.gstatic.com";
        }


        public static class Microsoft
        {
            public const string Domain = "ajax.aspnetcdn.com";
            public const string JQueryValidateUrl = "//ajax.aspnetcdn.com/ajax/jquery.validate/1.15.0/jquery.validate.min.js";
            public const string JQueryValidateUnobtrusiveUrl = "//ajax.aspnetcdn.com/ajax/mvc/5.2.3/jquery.validate.unobtrusive.min.js";
            public const string ModernizrUrl = "//ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.8.3.js";
            public const string BootstrapUrl = "//ajax.aspnetcdn.com/ajax/bootstrap/3.3.6/bootstrap.min.js";

        }

    }


}
