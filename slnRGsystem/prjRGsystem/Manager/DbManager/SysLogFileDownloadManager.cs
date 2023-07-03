using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysLogFileDownloadManager : AbstractBusinessAppEntityManager<SysLogFileDownload>
    {
        public SysLogFileDownloadManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysLogFileDownload> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysLogFileDownload.Where(item => !item.removed);
            else
                return db.SysLogFileDownload.AsNoTracking().Where(item => !item.removed);
        }

        public IQueryable<SysLogFileDownload> ExcuteQuery(Criteria criteria)
        {
            SysEnterprise? sysEnterprise = GetUserLogin().enterprise;
            SysDepartment? sysDepartment = GetUserLogin().department;
            long enterpriseId = sysEnterprise != null ? sysEnterprise.id : 0;
            long departmentId = sysDepartment != null ? sysDepartment.id : 0;

            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select distinct * from " + SysLogFileDownload.TABLE_NAME);
            sql.Append(" where removed = 0 ");

            if (!string.IsNullOrEmpty(criteria.accessUserId))
            {
                sql.Append(" and accessUserId like @accessUserId");
                parameters.Add(new SqlParameter("accessUserId", "%" + criteria.accessUserId + "%"));
            }
            if (!string.IsNullOrEmpty(criteria.remoteIp))
            {
                sql.Append(" and remoteIp like @remoteIp");
                parameters.Add(new SqlParameter("remoteIp", "%" + criteria.remoteIp + "%"));
            }
            if (!string.IsNullOrEmpty(criteria.tableName))
            {
                sql.Append(" and tableName like @tableName");
                parameters.Add(new SqlParameter("tableName", "%" + criteria.tableName + "%"));
            }
            if (!string.IsNullOrEmpty(criteria.fileName))
            {
                sql.Append(" and fileName like @fileName");
                parameters.Add(new SqlParameter("fileName", "%" + criteria.fileName + "%"));
            }

            return db.SysLogFileDownload.FromSqlRaw(sql.ToString(), parameters.ToArray());
        }

        public class Criteria
        {
            public string? accessUserId { set; get; }
            public string? remoteIp { get; set; }
            public string? tableName { get; set; }
            public string? fileName { get; set; }
            public int? pageNumber { get; set; }
            public int? pageSize { get; set; }
        }
    }
}
