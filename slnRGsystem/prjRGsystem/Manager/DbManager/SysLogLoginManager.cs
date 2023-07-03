using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysLogLoginManager : AbstractBusinessAppEntityManager<SysLogLogin>
    {
        public SysLogLoginManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysLogLogin> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysLogLogin.Where(item => !item.removed);
            else
                return db.SysLogLogin.AsNoTracking().Where(item => !item.removed);
        }


        public IQueryable<SysLogLogin> ExcuteQuery(Criteria criteria)
        {
            SysEnterprise? sysEnterprise = GetUserLogin().enterprise;
            //SysDepartment? sysDepartment = GetUserLogin().department;
            long enterpriseId = sysEnterprise != null ? sysEnterprise.id : 0;
            //long departmentId = sysDepartment != null ? sysDepartment.id : 0;

            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select distinct * from " + SysLogLogin.TABLE_NAME);
            sql.Append(" where removed = 0 ");

            if (!string.IsNullOrEmpty(criteria.beginTime))
            {
                DateTime time;
                if (DateTime.TryParse(criteria.beginTime, out time))
                {
                    sql.Append(" and createDate >= @beginTime");
                    parameters.Add(new SqlParameter("beginTime", time));
                }
            }
            if (!string.IsNullOrEmpty(criteria.endTime))
            {
                DateTime time;
                if (DateTime.TryParse(criteria.endTime, out time))
                {
                    time = time.AddDays(1);
                    sql.Append(" and createDate < @endTime");
                    parameters.Add(new SqlParameter("endTime", time));
                }
            }
            if (!string.IsNullOrEmpty(criteria.userId))
            {
                sql.Append(" and accessUserId = @accessUserId");
                parameters.Add(new SqlParameter("accessUserId", criteria.userId));
            }
            if (!string.IsNullOrEmpty(criteria.loginStatus))
            {
                sql.Append(" and loginStatus = @loginStatus");
                parameters.Add(new SqlParameter("loginStatus", criteria.loginStatus));
            }
            if (!string.IsNullOrEmpty(criteria.loginIp))
            {
                sql.Append(" and loginIp like @loginIp");
                parameters.Add(new SqlParameter("loginIp", "%" + criteria.loginIp + "%"));
            }
            if (!string.IsNullOrEmpty(criteria.remark))
            {
                sql.Append(" and remark like @remark");
                parameters.Add(new SqlParameter("remark", "%" + criteria.remark + "%"));
            }
            if (enterpriseId > 0)
            {
                sql.Append(" and enterpriseId = @enterpriseId ");
                parameters.Add(new SqlParameter("enterpriseId", enterpriseId));
            }
            //if (departmentId > 0)
            //{
            //    sql.Append(" and departmentId = @departmentId ");
            //    parameters.Add(new SqlParameter("departmentId", departmentId));
            //}
            return db.SysLogLogin.FromSqlRaw(sql.ToString(), parameters.ToArray());
        }


        public class Criteria
        {
            public string? beginTime { set; get; }
            public string? endTime { set; get; }
            public string? userId { set; get; }
            public string? loginIp { get; set; }
            public string? loginStatus { get; set; }
            public string? remark { get; set; }
            public int? pageNumber { get; set; }
            public int? pageSize { get; set; }
        }

    }
}
