using PharmacyApp.Constants;
using System.Web;
using System.Web.Optimization;

namespace PharmacyApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Enable Optimizations
            // Set EnableOptimizations to false for debugging. For more information,
            // Web.config file system.web/compilation[debug=true]
            // OR
            // BundleTable.EnableOptimizations = true;

            // Enable CDN usage.
            // Note: that you can choose to remove the CDN if you are developing an intranet application.
            // Note: We are using Google's CDN where possible and then Microsoft if not available for better
            //       performance (Google is more likely to have been cached by the users browser).
            // Note: that protocol (http:) is omitted from the CDN URL on purpose to allow the browser to choose the protocol.
            bundles.UseCdn = true;
            Addcss(bundles);
            AddJavaScript(bundles);

        }


        private static void Addcss(BundleCollection bundles)
        {
            // Bootstrap - Twitter Bootstrap CSS (http://getbootstrap.com/).
            // Site - Your custom site CSS.
            // Note: No CDN support has been added here. Most likely you will want to customize your copy of bootstrap.
            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/animate.css",
                      "~/Content/style.css"));


            // Font Awesome - Icons using font (http://fortawesome.github.io/Font-Awesome/).
            bundles.Add(new StyleBundle(
                "~/Content/fa",
                ContentDeliveryNetwork.MaxCdn.FontAwesomeUrl)
                .Include("~/Content/fontawesome/site.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/Content/fonts/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform()));

            // jasnyBootstrap styles
            bundles.Add(new StyleBundle("~/plugins/jasnyBootstrapStyles").Include(
                      "~/Content/plugins/jasny/jasny-bootstrap.min.css"));

            // jQueryUI CSS
            bundles.Add(new ScriptBundle("~/jquery-ui/jqueryuiStyles").Include(
                        "~/Content/theme/base/jquery-ui.min.css"));


        }


        private static void AddJavaScript(BundleCollection bundles)
        {
            // jQuery - The JavaScript helper API (http://jquery.com/).
            Bundle jquerybundle = new Bundle("~/bundles/jquery", ContentDeliveryNetwork.Google.JQueryUrl)
                .Include("~/Scripts/jquery-{version}.js");
            bundles.Add(jquerybundle);


            // jQuery Validate - Client side JavaScript form validation (http://jqueryvalidation.org/).
            Bundle jqueryValidateBundle = new ScriptBundle("~/bundles/jqueryval", ContentDeliveryNetwork.Microsoft.JQueryValidateUrl)
                .Include("~/Scripts/jquery.validate*");
            bundles.Add(jqueryValidateBundle);


            // Microsoft jQuery Validate Unobtrusive - Validation using HTML data- attributes
            // http://stackoverflow.com/questions/11534910/what-is-jquery-unobtrusive-validation
            Bundle jqueryValidateUnobtrusiveBundle = new ScriptBundle("~/bundles/jqueryvalunobtrusive", ContentDeliveryNetwork.Microsoft.JQueryValidateUnobtrusiveUrl)
                .Include("~/Scripts/jquery.validate*");
            bundles.Add(jqueryValidateUnobtrusiveBundle);



            // Modernizr - Allows you to check if a particular API is available in the browser (http://modernizr.com).
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            // Note: The current version of Modernizr does not support Content Security Policy (CSP) (See FilterConfig).
            // See here for details: https://github.com/Modernizr/Modernizr/pull/1263 and
            // http://stackoverflow.com/questions/26532234/modernizr-causes-content-security-policy-csp-violation-errors
            Bundle modernizrBundle = new ScriptBundle("~/bundles/modernizr", ContentDeliveryNetwork.Microsoft.ModernizrUrl)
    .Include("~/Scripts/modernizr-*");
            bundles.Add(modernizrBundle);



            // Bootstrap - Twitter Bootstrap JavaScript (http://getbootstrap.com/).
            Bundle bootstrapBundle = new ScriptBundle("~/bundles/bootstrap", ContentDeliveryNetwork.Microsoft.BootstrapUrl)
    .Include("~/Scripts/bootstrap.js",
    "~/Scripts/respond.js");
            bundles.Add(bootstrapBundle);


            // Script bundle for the site. The fall-back scripts are for when a CDN fails, in this case we load a local
            // copy of the script instead.
            Bundle failoverCoreBundle = new ScriptBundle("~/bundles/site")
    .Include("~/Scripts/Fallback/styles.js")
    .Include("~/Scripts/Fallback/scripts.js")
    .Include("~/Scripts/site.js");
            bundles.Add(failoverCoreBundle);

            // Inspinia script
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Scripts/inspinia.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));

            // jQuery plugins
            bundles.Add(new ScriptBundle("~/plugins/metsiMenu").Include(
                      "~/Scripts/plugins/metisMenu/metisMenu.min.js"));

            bundles.Add(new ScriptBundle("~/plugins/pace").Include(
                      "~/Scripts/plugins/pace/pace.min.js"));

            // iCheck
            bundles.Add(new ScriptBundle("~/plugins/iCheck").Include(
                      "~/Scripts/plugins/iCheck/icheck.min.js"));

            // jasnyBootstrap 
            bundles.Add(new ScriptBundle("~/plugins/jasnyBootstrap").Include(
                      "~/Scripts/plugins/jasny/jasny-bootstrap.min.js"));


            // jQueryUI 
            bundles.Add(new StyleBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.min.js"));

        }

    }
}
