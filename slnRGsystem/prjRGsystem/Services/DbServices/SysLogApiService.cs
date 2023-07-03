using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Services.DbServices
{
    public class SysLogApiService : SysLogApiManager
    {
        public SysLogApiService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public void SaveApiLog(string? searchValue, string response, HttpContext httpContext)
        {
            SysLogApi sysLogApi = new();
            sysLogApi.url = GetCompleteUrl(httpContext);
            sysLogApi.port = httpContext.Request.Host.Port.ToString();
            sysLogApi.queryData = searchValue;
            sysLogApi.httpMethodType = httpContext.Request.Method;
            sysLogApi.response = response;
            Save(sysLogApi);
        }

        private string GetCompleteUrl(HttpContext httpContext)
        {
            return new StringBuilder()
                 .Append(httpContext.Request.Scheme)
                 .Append("://")
                 .Append(httpContext.Request.Host)
                 .Append(httpContext.Request.PathBase)
                 .Append(httpContext.Request.Path)
                 .Append(httpContext.Request.QueryString)
                 .ToString();
        }
    }
}
