using System.Web.Mvc;

namespace SimpleBlog.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}