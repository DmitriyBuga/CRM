using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CRM.App_Start
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js").Include(
                        "~/Scripts/angular-ui/ui-bootstrap.js").Include(
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.js")
                        );
            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                "~/Scripts/underscore.js"));
            bundles.Add(new ScriptBundle("~/bundles/CRMScripts").Include(
                        "~/Angular/module.js", "~/Angular/Controller.js", "~/Angular/TicketsTable.js", "~/Angular/Directives.js", "~/Angular/Filter.js", "~/Angular/Service.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js", "~/Scripts/bootstrap-modalmanager.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css").Include(
                      "~/Bootstrap_Styles/css/styles.css").Include(
                      "~/Content/bootstrap-theme.min.css"));

        }
    }
}