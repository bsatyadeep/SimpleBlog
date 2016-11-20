using NHibernate.Linq;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SimpleBlog.Controllers
{
    public class PostsController : Controller
    {
        private const int PostPerPage = 5;
        public ActionResult Index(int page = 1)
        {
            var baseQuery = Database.Session.Query<Post>()
                .Where(t => t.DeletedAt == null)
                .OrderByDescending(t => t.CreateedAt).ToList();
            var totalPostCount = baseQuery.Count;
            var postIds = baseQuery.Skip((page - 1) * (PostPerPage)).Take(PostPerPage).Select(t => t.Id).ToList();
            var posts = baseQuery.Where(t => postIds.Contains(t.Id)).ToList();
            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(posts, totalPostCount, page, PostPerPage)
            });
        }

        public ActionResult Tag(string idandslug, int page = 1)
        {
            var parts = SeparateIdAndSlug(idandslug);
            if (parts == null)
            {
                return HttpNotFound();
            }
            var tag = Database.Session.Load<Tag>(parts.Item1);
            if (tag == null)
            {
                return HttpNotFound();
            }
            if (!tag.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToRoutePermanent("Tag", new { id = parts.Item1, slug = tag.Slug });
            }
            var totalostCount = tag.Posts.Count;
            var postids = tag.Posts
                .OrderByDescending(t => t.CreateedAt)
                .Skip((page - 1) * PostPerPage).Take(PostPerPage)
                .Where(t => t.DeletedAt == null)
                .Select(t => t.Id).ToList();
            var posts = Database.Session.Query<Post>()
                .OrderByDescending(b => b.CreateedAt)
                .Where(t => postids.Contains(t.Id)).ToList();
            return View(new PostTag
            {
                Tag = tag,
                Posts = new PagedData<Post>(posts, totalostCount, page, PostPerPage)
            });

        }
        public ActionResult Show(string idandslug)
        {
            var parts = SeparateIdAndSlug(idandslug);
            if (parts == null)
            {
                return HttpNotFound();
            }
            var post = Database.Session.Load<Post>(parts.Item1);
            if (post == null || post.IsDeleted)
            {
                return HttpNotFound();
            }
            if (!post.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToRoutePermanent("Post", new { id = parts.Item1, slug = post.Slug });
            }
            return View(new PostsShow
            {
                Post = post
            });
        }

        private Tuple<int, string> SeparateIdAndSlug(string idandslug)
        {
            var matches = Regex.Match(idandslug, @"^(\d+)\-(.*)?$");
            if (!matches.Success)
            {
                return null;
            }
            var id = int.Parse(matches.Result("$1"));
            var slug = matches.Result("$2");
            return Tuple.Create(id, slug);

        }
    }
}