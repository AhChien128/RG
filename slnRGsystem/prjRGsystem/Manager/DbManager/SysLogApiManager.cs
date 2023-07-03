using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysLogApiManager : AbstractBusinessAppEntityManager<SysLogApi>
    {
        public SysLogApiManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }
        public override IQueryable<SysLogApi> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysLogApi.Where(item => !item.removed);
            else
                return db.SysLogApi.AsNoTracking().Where(item => !item.removed);
        }
        public IQueryable<SysLogApi> ExcuteQuery(Criteria criteria)
        {
            SysEnterprise? sysEnterprise = GetUserLogin().enterprise;
            long enterpriseId = sysEnterprise != null ? sysEnterprise.id : 0;
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select distinct * from " + SysLogApi.TABLE_NAME);
            sql.Append(" where removed = @removed ");
            parameters.Add(new SqlParameter("removed", false));
            sql.Append(" and enterpriseId = @enterpriseId ");
            parameters.Add(new SqlParameter("enterpriseId", enterpriseId));
            //呼叫URL
            if (!string.IsNullOrEmpty(criteria.url))
            {
                sql.Append(" and url = @url");
                parameters.Add(new SqlParameter("url", criteria.url));
            }
            //帳號
            if (!string.IsNullOrEmpty(criteria.createUser))
            {
                sql.Append(" and createUser = @createUser");
                parameters.Add(new SqlParameter("createUser", criteria.createUser));
            }
            return db.SysLogApi.FromSqlRaw(sql.ToString(), parameters.ToArray());
        }
        public class Criteria
        {
            /// <summary>
            /// 呼叫URL
            /// </summary>
            public string? url { set; get; }
            /// <summary>
            /// 帳號
            /// </summary>
            public string? createUser { set; get; }
            /// <summary>
            /// 頁數
            /// </summary>
            public int? pageNumber { get; set; }

        }
    }
}
