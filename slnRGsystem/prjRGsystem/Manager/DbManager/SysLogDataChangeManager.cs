using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysLogDataChangeManager : AbstractBusinessAppEntityManager<SysLogDataChange>
    {
        public SysLogDataChangeManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }
        public override IQueryable<SysLogDataChange> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysLogDataChange.Where(item => !item.removed);
            else
                return db.SysLogDataChange.AsNoTracking().Where(item => !item.removed);
        }
        public IQueryable<SysLogDataChange> ExcuteQuery(Criteria criteria)
        {
            SysEnterprise? sysEnterprise = GetUserLogin().enterprise;
            long enterpriseId = sysEnterprise != null ? sysEnterprise.id : 0;
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select * from " + SysLogDataChange.TABLE_NAME);
            sql.Append(" where removed = @removed ");
            parameters.Add(new SqlParameter("removed", false));
            sql.Append(" and enterpriseId = @enterpriseId ");
            parameters.Add(new SqlParameter("enterpriseId", enterpriseId));
            //帳號
            if (!string.IsNullOrEmpty(criteria.sysUser))
            {
                sql.Append(" and sysUser = @sysUser");
                parameters.Add(new SqlParameter("sysUser", criteria.sysUser));
            }
            //資料表名稱
            if (!string.IsNullOrEmpty(criteria.tableName))
            {
                sql.Append(" and tableName = @tableName");
                parameters.Add(new SqlParameter("tableName", criteria.tableName));
            }
            //修改欄位
            if (!string.IsNullOrEmpty(criteria.keyValues))
            {
                sql.Append(" and keyValues = @keyValues");
                parameters.Add(new SqlParameter("keyValues", criteria.keyValues));
            }
            return db.SysLogDataChange.FromSqlRaw(sql.ToString(), parameters.ToArray());
        }
        public class Criteria
        {
            /// <summary>
            /// 帳號
            /// </summary>
            public string? sysUser { set; get; }
            /// <summary>
            /// 資料表名稱
            /// </summary>
            public string? tableName { set; get; }
            /// <summary>
            /// 修改欄位
            /// </summary>
            public string? keyValues { set; get; }
            /// <summary>
            /// 頁數
            /// </summary>
            public int? pageNumber { get; set; }

        }
    }
}
