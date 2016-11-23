using System.Web.Optimization;

namespace SimpleBlog
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/admin/styles")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/Admin/site.css"));

            bundles.Add(new StyleBundle("~/styles")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/admin/scripts")
                .Include(string.Format("~/Scripts/jquery-{0}.js", "3.1.1"))
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/modernizr-*")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Areas/Admin/Scripts/Forms.js"));

            bundles.Add(new ScriptBundle("~/admin/post/scripts")
                .Include("~/Areas/Admin/Scripts/PostEditor.js"));

            bundles.Add(new ScriptBundle("~/scripts")
                .Include(string.Format("~/Scripts/jquery-{0}.js", "3.1.1"))
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/jquery.timeago.js")
                .Include("~/Scripts/modernizr-*")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/Frontend.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));


            //BundleTable.EnableOptimizations = true;
        }
    }
}