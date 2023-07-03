using prjRGsystem.Context;
using prjRGsystem.Attributes;
using prjRGsystem.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace prjRGsystem.Filters
{
    public class UserLoginFilter : ActionFilterAttribute
    {
        private bool isIgnore = true;

        public UserLoginFilter()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            //action執行前
            var descriptor = (ControllerActionDescriptor)filterContext.ActionDescriptor;
            var attributes = descriptor.MethodInfo.CustomAttributes;
            if (attributes.Any(a => a.AttributeType == typeof(IgnoreUserLoginFilterAttribute))) isIgnore = false;

            UserContext? userContext = filterContext.HttpContext.Session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
            //當使用者是從area來的，並且這個area名稱是Admin時(也就是後台管理者未登入時)
            if (isIgnore && (userContext == null || userContext.user == null))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Home",
                    area = "Login"
                }));
            }
            else
            {
                await next();
            }
        }
    }
}
