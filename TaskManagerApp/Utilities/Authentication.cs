using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class Authentication : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {

        if (filterContext.HttpContext.Session.GetString("UserName") == null)
        {
            filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary {
                                { "Controller", "Task" },
                                { "Action", "Login" }
                        });
        }

    }
}