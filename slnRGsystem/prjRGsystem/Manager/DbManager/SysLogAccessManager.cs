using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;
using System.Text;


namespace prjRGsystem.Managers.DbManagers
{
    public class SysLogAccessManager : AbstractBusinessAppEntityManager<SysLogAccess>
    {

        public SysLogAccessManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysLogAccess> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysLogAccess.Where(item => !item.removed);
            else
                return db.SysLogAccess.AsNoTracking().Where(item => !item.removed);
        }
        /// <summary>
        /// 綜合查詢方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IQueryable<SysLogAccess> ExcuteQuery(Criteria criteria)
        {
            SysEnterprise? sysEnterprise = GetUserLogin().enterprise;
            long enterpriseId = sysEnterprise is null ? 0 : sysEnterprise.id;
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select * from ").Append(SysLogAccess.TABLE_NAME);
            sql.Append(" where removed = @removed ");
            parameters.Add(new SqlParameter("removed", false));
            sql.Append(" and enterpriseId = @enterpriseId ");
            parameters.Add(new SqlParameter("enterpriseId", enterpriseId));
            //帳號
            if (!String.IsNullOrEmpty(criteria.sysUser))
            {
                sql.Append(" and sysUser = @sysUser ");
                parameters.Add(new SqlParameter("sysUser", criteria.sysUser.Trim()));
            }
            //來源IP
            if (!String.IsNullOrEmpty(criteria.remoteIp))
            {
                sql.Append(" and remoteIp =  @remoteIp ");
                parameters.Add(new SqlParameter("remoteIp", criteria.remoteIp.Trim()));
            }
            //控制器顯示名稱
            if (!String.IsNullOrEmpty(criteria.controllerDisplayName))
            {
                sql.Append(" and controllerDisplayName =  @controllerDisplayName ");
                parameters.Add(new SqlParameter("controllerDisplayName", criteria.controllerDisplayName.Trim()));
            }
            //方法顯示名稱
            if (!String.IsNullOrEmpty(criteria.actionDisplayName))
            {
                sql.Append(" and actionDisplayName = @actionDisplayName ");
                parameters.Add(new SqlParameter("actionDisplayName", criteria.actionDisplayName.Trim()));
            }

            return db.SysLogAccess.FromSqlRaw(sql.ToString(), parameters.ToArray());

        }

        public class Criteria
        {
            /// <summary>
            /// 帳號
            /// </summary>
            public string? sysUser { get; set; }
            /// <summary>
            /// 來源IP
            /// </summary>
            public string? remoteIp { get; set; }
            /// <summary>
            /// 控制器顯示名稱
            /// </summary>
            public string? controllerDisplayName { get; set; }
            /// <summary>
            /// 方法顯示名稱
            /// </summary>
            public string? actionDisplayName { get; set; }
            /// <summary>
            /// 頁數
            /// </summary>
            public int? pageNumber { get; set; }
        }
    }
}
