using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SimpleBlog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("users")]
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View(new UsersIndex
            {
                Users = Database.Session.Query<User>().ToList()
            });
        }

        public ActionResult NewUser()
        {
            return View(new UsersNew
            {
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckBox
                {
                    Id = role.Id,
                    IsChecked = false,
                    Name = role.Name
                }).ToList()
            });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewUser(UsersNew form)
        {
            var user = new User();
            SyncRoles(form.Roles, user.Roles);


            var users = Database.Session.Query<User>().ToList();
            if (users.Any(i => i.UserName == form.Username))
            {
                ModelState.AddModelError(form.Username, "Username must be unique");
            }
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            user.Email = form.Email;
            user.UserName = form.Username;

            //var user = new User
            //{
            //    UserName = form.Username,
            //    Email = form.Email
            //};
            user.Setpassword(form.Password);
            Database.Session.Save(user);
            return RedirectToAction("index");
        }
        public ActionResult EditUser(int id)
        {
            //var users = Database.Session.Query<User>().ToList();
            //var user = users.FirstOrDefault(i => i.Id == id);
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(new UsersEdid
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckBox
                {
                    Id = role.Id,
                    IsChecked = user.Roles.Contains(role),
                    Name = role.Name
                }).ToList()
            });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditUser(int id, UsersEdid form)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            SyncRoles(form.Roles, user.Roles);
            var users = Database.Session.Query<User>().ToList();
            if (users.Any(i => i.UserName == form.Username && i.Id != id))
            {
                ModelState.AddModelError(form.Username, "Username already used");
            }
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            user.UserName = form.Username;
            user.Email = form.Email;
            Database.Session.Update(user, user.Id);
            return RedirectToAction("index");
        }

        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(new UsersResetPassword
            {
                Username = user.UserName
            });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id, UsersResetPassword form)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            form.Username = user.UserName;

            if (!ModelState.IsValid)
            {
                return View(form);
            }
            user.Setpassword(form.Password);
            Database.Session.Update(user, user.Id);
            return RedirectToAction("index");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            Database.Session.Delete(user);
            return RedirectToAction("index");
        }

        private void SyncRoles(IList<RoleCheckBox> checkBoxs, IList<Role> roles)
        {
            var selectedRoles = new List<Role>();
            var dbroles = Database.Session.Query<Role>().ToList();
            foreach (var role in dbroles)
            {
                var checkBox = checkBoxs.Single(i => i.Id == role.Id);
                checkBox.Name = role.Name;
                if (checkBox.IsChecked)
                {
                    selectedRoles.Add(role);
                }
            }
            foreach (Role roleToAdd in selectedRoles.Where(r => !roles.Contains(r)))
            {
                roles.Add(roleToAdd);
            }
            foreach (Role roleToRemove in roles.Where(r => !selectedRoles.Contains(r)).ToList())
            {
                roles.Remove(roleToRemove);
            }
        }
    }
}