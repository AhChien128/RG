using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Filters
{
    public class HostMappingFilter : ActionFilterAttribute
    {
        private readonly RGPropertyContext db;

        public HostMappingFilter(RGPropertyContext _db)
        {
            db = _db;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            string forgedDomainName = filterContext.HttpContext.Request.Host.Host;  //拿到domainName
            string ip = "";
            if (filterContext.HttpContext != null &&
                filterContext.HttpContext.Connection != null &&
                filterContext.HttpContext.Connection.RemoteIpAddress != null)
                ip = filterContext.HttpContext.Connection.RemoteIpAddress.ToString();
            SysEnterprise? sysEnterprise = null;

            //進入網站頁面時判斷是否為localhost，是的話抓取isDeBug為True的網站
            if (forgedDomainName == "localhost")
                sysEnterprise = await db.SysEnterprise.AsNoTracking().Where(e => e.isDeBug == true).FirstOrDefaultAsync();

            sysEnterprise ??= await db.SysEnterprise.AsNoTracking().Where(e => e.domainName == forgedDomainName).FirstOrDefaultAsync();

            if (sysEnterprise != null && sysEnterprise.id > 0)
            {
                UserContext? userContext = null;
                if (filterContext.HttpContext != null && filterContext.HttpContext.Session != null)
                {
                    userContext = filterContext.HttpContext.Session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
                    userContext ??= new UserContext();
                    userContext.enterprise = sysEnterprise;
                    filterContext.HttpContext.Session.SetObjectAsJson(UserContext.SESSION_NAME.ToString(), userContext);
                }
            }
            await base.OnActionExecutionAsync(filterContext, next);
        }
    }
}
