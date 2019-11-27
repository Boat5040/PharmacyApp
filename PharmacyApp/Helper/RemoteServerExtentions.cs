using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.Helper
{
    public static class RemoteServerExtentions
    {
        public static string GetRemoteServerIpAddress(this ControllerBase controller)
        {
            string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                string[] ipRange = ipAddress.Split(',');
                ipAddress = ipRange[0];
            }
            else
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            }

            return ipAddress.Trim();
        }
    }
}