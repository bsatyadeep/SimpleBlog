using System.Web.Mvc;

namespace SimpleBlog.Infrastructure
{

    public class TransactionFilter : IActionFilter
    {
        //when finished
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
                Database.Session.Transaction.Commit();
            else
                Database.Session.Transaction.Rollback();
        }
        //when begins
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Database.Session.BeginTransaction();
        }
    }
}