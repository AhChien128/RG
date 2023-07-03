using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using prjRGsystem.Attributes;
using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Models;

namespace prjRGsystem.Filters
{
    public class RolePrivilegeFilter : IAsyncActionFilter
    {

        public RolePrivilegeFilter()
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            UserContext? userContext = filterContext.HttpContext.Session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
            if (userContext != null && userContext.user != null && userContext.authorizedSysCodes != null && userContext.authorizedTaskBlackNames != null)
            {
                string areaName = filterContext.ActionDescriptor.RouteValues["area"] ?? "";
                if (!new List<string> { "Login" }.Contains(areaName))
                {
                    string controllerName = filterContext.ActionDescriptor.RouteValues["controller"] ?? "";
                    string actionName = filterContext.ActionDescriptor.RouteValues["action"] ?? "";
                    userContext.currentPageSysCodesTypes = userContext.authorizedSysCodes.TryGetValue($"{areaName}{controllerName}", out var sysCodesTypesValue) ? sysCodesTypesValue : null;
                    userContext.currentPageTaskBlackNames = userContext.authorizedTaskBlackNames.TryGetValue($"{areaName}{controllerName}", out var taskBlackNamesValue) ? taskBlackNamesValue : new();
                    filterContext.HttpContext.Session.SetObjectAsJson(UserContext.SESSION_NAME.ToString(), userContext);
                    ControllerBase controllerBase = ((ControllerBase)filterContext.Controller);

                    RolePrivilegeType[]? privilegeTypes = AppActionAttribute.GetPrivilegeTypes(controllerBase, actionName);
                    string appActionName = AppActionAttribute.GetName(controllerBase, actionName);

                    if (userContext.currentPageTaskBlackNames.Contains(appActionName))
                    {
                        string msg = $"您沒有權限可使用 {String.Join(",", appActionName)} 功能";
                        filterContext.Result = new ContentResult { Content = msg, StatusCode = 403 };
                    }
                    else
                    {
                        if (privilegeTypes == null || privilegeTypes.Length == 0)
                        {
                            await next();
                        }
                        else
                        {
                            bool isAllow = true;
                            bool isAdmin = userContext.currentPageSysCodesTypes != null && (userContext.currentPageSysCodesTypes.isAdmin ?? false);
                            if (isAdmin)
                            {
                                await next();
                            }
                            else
                            {
                                List<string> privilegeTexts = new();
                                foreach (RolePrivilegeType privilegeType in privilegeTypes)
                                {
                                    switch (privilegeType)
                                    {
                                        case RolePrivilegeType.ADD:
                                            isAllow = userContext.currentPageSysCodesTypes != null && (userContext.currentPageSysCodesTypes.isAdd ?? false);
                                            privilegeTexts.Add(ColumnAttribute.GetDisplayName(RolePrivilegeType.ADD));
                                            break;
                                        case RolePrivilegeType.EDIT:
                                            isAllow = userContext.currentPageSysCodesTypes != null && (userContext.currentPageSysCodesTypes.isEdit ?? false);
                                            privilegeTexts.Add(ColumnAttribute.GetDisplayName(RolePrivilegeType.EDIT));
                                            break;
                                        case RolePrivilegeType.VIEW:
                                            isAllow = userContext.currentPageSysCodesTypes != null && (userContext.currentPageSysCodesTypes.isView ?? false);
                                            privilegeTexts.Add(ColumnAttribute.GetDisplayName(RolePrivilegeType.VIEW));
                                            break;
                                        case RolePrivilegeType.REMOVED:
                                            isAllow = userContext.currentPageSysCodesTypes != null && (userContext.currentPageSysCodesTypes.isRemoved ?? false);
                                            privilegeTexts.Add(ColumnAttribute.GetDisplayName(RolePrivilegeType.REMOVED));
                                            break;
                                        case RolePrivilegeType.REPORT:
                                            isAllow = userContext.currentPageSysCodesTypes != null && (userContext.currentPageSysCodesTypes.isReport ?? false);
                                            privilegeTexts.Add(ColumnAttribute.GetDisplayName(RolePrivilegeType.REPORT));
                                            break;
                                        case RolePrivilegeType.AUDIT:
                                            isAllow = userContext.currentPageSysCodesTypes != null && (userContext.currentPageSysCodesTypes.isAudit ?? false);
                                            privilegeTexts.Add(ColumnAttribute.GetDisplayName(RolePrivilegeType.AUDIT));
                                            break;
                                    }
                                    if (isAllow)
                                    {
                                        await next();
                                        break;
                                    }
                                }
                                if (!isAllow)
                                {
                                    string msg = $"您沒有權限可使用 {String.Join(",", privilegeTexts)} 功能";
                                    filterContext.Result = new ContentResult { Content = msg, StatusCode = 403 };
                                }
                            }
                        }
                    }
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
