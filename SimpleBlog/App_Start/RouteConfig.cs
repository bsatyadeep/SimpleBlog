﻿using SimpleBlog.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var namespaces = new[] { typeof(PostsController).Namespace };
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("TagsForRealThisTime", "tag/{idandslug}", new { controller = "Posts", action = "Tag" },
                namespaces);
            routes.MapRoute("Tag", "tag/{id}-{slug}", new { controller = "Posts", action = "Tag" }, namespaces);

            routes.MapRoute("PostsForRealThisTime", "post/{idandslug}", new { controller = "Posts", action = "Show" },
                namespaces);
            routes.MapRoute("Post", "post/{id}-{slug}", new { controller = "Posts", action = "Show" }, namespaces);

            routes.MapRoute("Login", "login", new { controller = "Auth", action = "Login" }, namespaces);
            routes.MapRoute("Logout", "logout", new { controller = "Auth", action = "Logout" }, namespaces);
            routes.MapRoute("Home", "", new { controller = "Posts", action = "Index" }, namespaces);

            routes.MapRoute("Sidebar", "", new { controller = "Layout", action = "Sidebar" }, namespaces);

            routes.MapRoute("Error500", "error/500", new { controller = "Error", action = "NotFound" }, namespaces);
            routes.MapRoute("Error404", "error/404", new { controller = "Error", action = "NotFound" }, namespaces);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Posts", action = "Index", id = UrlParameter.Optional},namespaces:namespaces
            //    );
            //routes.MapRoute(
            //    name: "Login",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Auth", action = "Login", id = UrlParameter.Optional },
            //    namespaces: namespaces
            //    );
        }
    }
}
