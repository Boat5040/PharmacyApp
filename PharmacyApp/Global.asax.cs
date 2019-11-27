using Boilerplate.Web.Mvc;
using jQuery.DataTables.Mvc;
using NWebsec.Csp;
using PharmacyApp.Constants;
using PharmacyApp.Controllers;
using PharmacyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PharmacyApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Ensure that the X-AspNetMvc-Version HTTP header is not 
            //MvcHandler.DisableMvcResponseHeader = true;

            ConfigureViewEngines();
            ConfigureAntiForgeryTokens();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Lets MVC know that anytime there is a JQueryDataTablesModel as a parameter in an action to use the
            // JQueryDataTablesModelBinder when binding the model.
            ModelBinders.Binders.Add(typeof(JQueryDataTablesModel), new JQueryDataTablesModelBinder());

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string[] headers = { "Server", "X-AspNet-Version", "X-Powered-By", "X-AspNetMvc-Version" };
            if (Response != null && Response.Headers != null)
            {
                foreach (string header in headers)
                {
                    if (Response.Headers[header] != null)
                    {
                        Response.Headers.Remove(header);
                    }
                }
            }
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            Response.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("Server");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            DependencyResolver.Current.GetService<ILoggingService>().Log(lastError);

            if (lastError.Message.Contains("modernizr") || lastError.Message.Contains("skin-config.html")
                || lastError.Message.Contains("header-profile.png")) return;

            int statusCode =500;

            if (lastError is HttpException)
                statusCode = (lastError as HttpException).GetHttpCode();

            string routeName = string.Empty;
            ErrorController errorController = new ErrorController();

            switch (statusCode)
            {
                case 400:
                    routeName = ErrorControllerRoute.GetBadRequest;
                    break;
                case 401:
                    routeName = ErrorControllerRoute.GetUnauthorized;
                    break;
                case 403:
                    routeName = ErrorControllerRoute.GetForbidden;
                    break;
                case 404:
                    routeName = ErrorControllerRoute.GetNotFound;
                    break;
                case 405:
                    routeName = ErrorControllerRoute.GetMethodNotAllowed;
                    break;
                case 500:
                    routeName = ErrorControllerRoute.GetInternalServerError;
                    break;
                default:
                    routeName = ErrorControllerRoute.GetInternalServerError;
                    break;
            }

            //RouteData routeData = new RouteData();
            //routeData.Values.Add("controller", ControllerName.Error);
            //routeData.Values.Add("action", routeName);

            //IController controller = errorController;
            //controller.Execute(new RequestContext(new HttpContextWrapper(this.Context), routeData));

            Server.ClearError();

            Response.TrySkipIisCustomErrors = true;
            Response.RedirectToRoute(routeName);
            Response.End();
        }


        /// <summary>
        /// Handles the Content Security Policy (CSP) violation errors. For more information see FilterConfig.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CspViolationReportEventArgs"/> instance containing the event data.</param>
        protected void NWebsecHttpHeaderSecurityModule_CspViolationReported(object sender, CspViolationReportEventArgs e)
        {
            // Log the Content Security Policy (CSP) violation.
            CspViolationReport violationReport = e.ViolationReport;
            CspReportDetails reportDetails = violationReport.Details;
            string violationReportString = string.Format(
                "UserAgent:<{0}>\r\nBlockedUri:<{1}>\r\nColumnNumber:<{2}>\r\nDocumentUri:<{3}>\r\nEffectiveDirective:<{4}>\r\nLineNumber:<{5}>\r\nOriginalPolicy:<{6}>\r\nReferrer:<{7}>\r\nScriptSample:<{8}>\r\nSourceFile:<{9}>\r\nStatusCode:<{10}>\r\nViolatedDirective:<{11}>",
                violationReport.UserAgent,
                reportDetails.BlockedUri,
                reportDetails.ColumnNumber,
                reportDetails.DocumentUri,
                reportDetails.EffectiveDirective,
                reportDetails.LineNumber,
                reportDetails.OriginalPolicy,
                reportDetails.Referrer,
                reportDetails.ScriptSample,
                reportDetails.SourceFile,
                reportDetails.StatusCode,
                reportDetails.ViolatedDirective);
            CspViolationException exception = new CspViolationException(violationReportString);
           // DependencyResolver.Current.GetService<ILoggingService>().Log(exception);
        }

        /// <summary>
        /// Configures the view engines. By default, Asp.Net MVC includes the Web Forms (WebFormsViewEngine) and 
        /// Razor (RazorViewEngine) view engines that supports both C# (.cshtml) and VB (.vbhtml). You can remove view 
        /// engines you are not using here for better performance and include a custom Razor view engine that only 
        /// supports C#.
        /// </summary>
        private static void ConfigureViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CSharpRazorViewEngine());
        }

        /// <summary>
        /// Configures the anti-forgery tokens. See 
        /// http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages
        /// </summary>
        private static void ConfigureAntiForgeryTokens()
        {
            // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "f". This adds a little security 
            // through obscurity and also saves sending a few characters over the wire. Sadly there is no way to change 
            // the form input name which is hard coded in the @Html.AntiForgeryToken helper and the 
            // ValidationAntiforgeryTokenAttribute to  __RequestVerificationToken.
            // <input name="__RequestVerificationToken" type="hidden" value="..." />
            AntiForgeryConfig.CookieName = "f";

            // If you have enabled SSL. Uncomment this line to ensure that the Anti-Forgery 
            // cookie requires SSL to be sent across the wire. 
            // AntiForgeryConfig.RequireSsl = true;
        }
    }
}
