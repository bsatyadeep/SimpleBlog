using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;
using SimpleBlog.Infrastructure;
using SimpleBlog.Infrastructure.Extensions;
using SimpleBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SimpleBlog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("posts")]
    public class PostsController : Controller
    {
        private const int PostsPerPage = 5;
        public ActionResult Index(int page = 1)
        {
            var baseQuery = Database.Session.Query<Post>().OrderByDescending(f => f.CreateedAt);
            var totalPostCount = baseQuery.Count();

            var postIds = baseQuery
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .Select(p => p.Id)
                .ToArray();

            var currentPostPage = baseQuery
                .Where(i => postIds.Contains(i.Id))
                .FetchMany(f => f.Tags)
                .Fetch(p => p.User)
                .ToList();

            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(currentPostPage, totalPostCount, page, PostsPerPage)
            });
        }

        public ActionResult NewPost()
        {
            return View("Form", new PostsForm
            {
                IsNew = true,
                Tags = Database.Session.Query<Tag>().Select(t => new TagCheckBox
                {
                    Id = t.Id,
                    IsChecked = false,
                    Name = t.Name
                }).ToList()
            });
        }

        public ActionResult EditPost(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View("Form", new PostsForm
            {
                IsNew = false,
                PostId = post.Id,
                Content = post.Content,
                Slug = post.Slug,
                Title = post.Title,
                Tags = Database.Session.Query<Tag>().Select(t => new TagCheckBox
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsChecked = post.Tags.Contains(t)
                }).ToList()
            });
        }
        //[HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Form(PostsForm form)
        {
            form.IsNew = form.PostId == null;
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var selectedTags = ReconsileTags(form.Tags).ToList();

            Post post;
            if (form.IsNew)
            {
                post = new Post
                {
                    CreateedAt = DateTime.UtcNow,
                    User = Auth.User
                };
                foreach (Tag tag in selectedTags)
                {
                    post.Tags.Add(tag);
                }
            }
            else
            {
                post = Database.Session.Load<Post>(form.PostId);
                if (post == null)
                {
                    return HttpNotFound();
                }
                post.UpdatedAt = DateTime.UtcNow;

                foreach (Tag tagToAdd in selectedTags.Where(t => !post.Tags.Contains(t)))
                {
                    post.Tags.Add(tagToAdd);
                }
                foreach (Tag tagToRemove in post.Tags.Where(t => !selectedTags.Contains(t)).ToList())
                {
                    post.Tags.Remove(tagToRemove);
                }
            }
            post.Title = form.Title;
            post.Slug = form.Slug;
            post.Content = form.Content;

            Database.Session.SaveOrUpdate(post);
            return RedirectToAction("index");
        }

        private IEnumerable<Tag> ReconsileTags(IList<TagCheckBox> tags)
        {
            foreach (TagCheckBox tagCheckBox in tags.Where(t => t.IsChecked))
            {
                if (tagCheckBox.Id != null)
                {
                    yield return Database.Session.Load<Tag>(tagCheckBox.Id);
                    continue;
                }
                var existingTag = Database.Session.Query<Tag>().FirstOrDefault(t => t.Name == tagCheckBox.Name);
                if (existingTag != null)
                {
                    yield return existingTag;
                    continue;
                }
                var newTag = new Tag
                {
                    Name = tagCheckBox.Name,
                    Slug = tagCheckBox.Name.Slugify()
                };
                Database.Session.Save(newTag);
                yield return newTag;
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult TrashPost(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.DeletedAt = DateTime.UtcNow;

            Database.Session.Update(post);
            return RedirectToAction("index");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            Database.Session.Delete(post);
            return RedirectToAction("index");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RestorPost(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.DeletedAt = null;

            Database.Session.Update(post);
            return RedirectToAction("index");
        }
    }
}