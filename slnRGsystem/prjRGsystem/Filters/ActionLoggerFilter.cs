using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using prjRGsystem.Attributes;
using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Filters
{
    public class ActionLoggerFilter : IAsyncActionFilter
    {
        private readonly SysLogAccessManager sysLogAccessManager;
        private readonly ILogger<ActionLoggerFilter> logger;
        public ActionLoggerFilter(ILogger<ActionLoggerFilter> _logger, SysLogAccessManager _sysAccessLoggerManager)
        {
            sysLogAccessManager = _sysAccessLoggerManager;
            logger = _logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //action執行前
            string controllerAttributesName = "";
            string actionAttributesName = "";
            string userId = "";
            UserContext? userContext = context.HttpContext.Session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
            //當使用者是從area來的，並且這個area名稱是Admin時(也就是後台管理者未登入時)
            if ((userContext != null && userContext.user != null))
                userId = (!string.IsNullOrEmpty(userContext.user.userId)) ? userContext.user.userId : "";

            ControllerBase controllerBase = ((ControllerBase)context.Controller);
            HttpContext httpContext = context.HttpContext;
            string ip = httpContext.Connection.RemoteIpAddress != null ? httpContext.Connection.RemoteIpAddress.ToString() : "";
            string host = httpContext.Request.Host.Value;
            string queryString = httpContext.Request.QueryString.Value ?? "";
            var controllerName = controllerBase.ControllerContext.ActionDescriptor.ControllerName;
            var actionName = controllerBase.ControllerContext.ActionDescriptor.ActionName;
            var areaName = context.ActionDescriptor.RouteValues["area"] ?? "";
            var controllerCustomAttributes = controllerBase.ControllerContext.ActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AppControllerAttribute), inherit: true);
            var actionAttributes = controllerBase.ControllerContext.ActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AppActionAttribute), inherit: true);
            if (controllerCustomAttributes != null && controllerCustomAttributes.Length > 0) controllerAttributesName = ((AppControllerAttribute)controllerCustomAttributes[0]).GetName();
            if (actionAttributes != null && actionAttributes.Length > 0) actionAttributesName = ((AppActionAttribute)actionAttributes[0]).GetName();
            //string? message = host + "/" + areaName + "/" + controllerName + "/" + actionName + queryString + " == " + controllerAttributesName + ":" + actionAttributesName + "使用者:" + userId + "來源IP:" + ip;
            logger.LogInformation("{host}/{areaName}/{controllerName}/{actionName}{queryString}=={controllerAttributesName}:{actionAttributesName}使用者:{userId}來源IP:{ip}", host, areaName, controllerName, actionName, queryString, controllerAttributesName, actionAttributesName, userId, ip);
            //logger.LogInformation(message);
            SysLogAccess sysAccessLogger = new SysLogAccess()
            {
                sysUser = userId,
                requestId = System.Diagnostics.Activity.Current != null ? System.Diagnostics.Activity.Current.Id : "",
                controllerName = controllerName,
                actionName = actionName,
                areaName = areaName,
                controllerDisplayName = controllerAttributesName,
                actionDisplayName = actionAttributesName,
                remoteIp = ip,
                queryString = queryString,
            };

            await sysLogAccessManager.SaveAsync(sysAccessLogger);
            await next();
            //action執行後
        }
    }
}
