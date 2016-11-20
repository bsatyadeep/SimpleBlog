using NHibernate.Linq;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace SimpleBlog.Controllers
{
    [ChildActionOnly]
    public class LayoutController : Controller
    {
        public ActionResult Sidebar()
        {
            return View(new LayoutSidebar
            {
                IsLoggedIn = Auth.User != null,
                UserName = Auth.User != null ? Auth.User.UserName : "",
                IsAdmin = User.IsInRole("admin"),
                Tags = Database.Session.Query<Tag>().Select(tag => new
                {
                    tag.Id,
                    tag.Name,
                    tag.Slug,
                    PostCount = tag.Posts.Count
                }).Where(t => t.PostCount > 0).OrderByDescending(t => t.PostCount).Select(tag => new SidebarTag(tag.Id, tag.Name, tag.Slug, tag.PostCount)).ToList()
            });
        }
    }


}