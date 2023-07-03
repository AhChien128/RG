using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;
using System.Net;

namespace prjRGsystem.Services.DbServices
{
    public class SysLogFileDownloadService : SysLogFileDownloadManager
    {
        private readonly ISession? session;
        public SysLogFileDownloadService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
        }

        public void SaveSysLogFileDownload(HttpRequest Request, string tableName, string? fileName, string? displayName)
        {
            IPAddress? ipAddress = Request.HttpContext.Connection.RemoteIpAddress;//獲取客戶端IP位置
            string remoteIpAddress = ipAddress is null ? "" : ipAddress.ToString();
            string accessUserId = "";
            if (session != null)
            {
                UserContext? userContext = session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
                if (userContext != null && userContext.user != null && userContext.user.userId != null)
                    accessUserId = userContext.user.userId;
            }
            SysLogFileDownload sysLogFileDownload = new();
            sysLogFileDownload.remoteIp = remoteIpAddress;
            sysLogFileDownload.accessUserId = accessUserId;
            sysLogFileDownload.tableName = tableName;
            sysLogFileDownload.fileName = fileName;
            sysLogFileDownload.displayName = displayName;
            Save(sysLogFileDownload);
        }

    }
}
