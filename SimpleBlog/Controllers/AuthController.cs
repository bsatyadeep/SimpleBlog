using NHibernate.Linq;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace SimpleBlog.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            return View(new AuthLogin());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(AuthLogin formLogin, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(formLogin);

            var users = Database.Session.Query<User>().ToList();
            var user = users.FirstOrDefault(u => u.UserName == formLogin.UserName);
            if (user == null)
                Models.User.FakeHash();

            if (user == null || !user.CheckPassword(formLogin.Password))
            {
                ModelState.AddModelError(formLogin.UserName, "Username or Password is incorrect");
                return View(formLogin);
            }
            if (!ModelState.IsValid)
                return View(formLogin);

            FormsAuthentication.SetAuthCookie(user?.UserName ?? formLogin.UserName, true);

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToRoute("Home");


        }
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Home");
        }
    }
}