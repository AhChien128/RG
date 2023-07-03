using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;
using System.Text;
using static prjRGsystem.Services.DbServices.SysRolesService;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysRolesManager : AbstractBusinessAppEntityManager<SysRoles>
    {
        public SysRolesManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysRoles> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysRoles.Where(item => !item.removed);
            else
                return db.SysRoles.AsNoTracking().Where(item => !item.removed);
        }

        public IQueryable<SysRoles> ExcuteQuery(Criteria criteria)
        {
            SysEnterprise? sysEnterprise = GetUserLogin().enterprise;
            SysDepartment? sysDepartment = GetUserLogin().department;
            long enterpriseId = sysEnterprise != null ? sysEnterprise.id : 0;
            long departmentId = sysDepartment != null ? sysDepartment.id : 0;

            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select distinct sr.* from " + SysRoles.TABLE_NAME + " as sr ");
            sql.Append(" left join " + SysUsersRoles.TABLE_NAME + " as sur on sr.id = sur.sysRolesId and sur.removed = 0 ");
            sql.Append(" left join " + SysUser.TABLE_NAME + " as su on su.id = sur.sysUsersId and su.removed = 0 ");
            sql.Append(" where sr.removed = 0 ");

            if (!string.IsNullOrEmpty(criteria.roleName))
            {
                sql.Append(" and sr.roleName like @roleName");
                parameters.Add(new SqlParameter("roleName", "%" + criteria.roleName + "%"));
            }
            if (!string.IsNullOrEmpty(criteria.userName))
            {
                sql.Append(" and su.userName like @userName");
                parameters.Add(new SqlParameter("userName", "%" + criteria.userName + "%"));
            }
            if (enterpriseId > 0)
            {
                sql.Append(" and sr.enterpriseId = @enterpriseId ");
                parameters.Add(new SqlParameter("enterpriseId", enterpriseId));
            }
            //if (departmentId > 0)
            //{
            //    sql.Append(" and sr.departmentId = @departmentId ");
            //    parameters.Add(new SqlParameter("departmentId", departmentId));
            //}
            return db.SysRoles.FromSqlRaw(sql.ToString(), parameters.ToArray());
        }

        public List<IdAndPermissions> HandlePermissions(List<string> sysTasksString)
        {
            List<IdAndPermissions> idAndPermissions = new List<IdAndPermissions>();
            foreach (string perTask in sysTasksString)
            {
                if (!string.IsNullOrEmpty(perTask))
                {
                    IdAndPermissions newData = new IdAndPermissions();
                    string[] perTaskElements = perTask.Split("_").ToArray();
                    newData.id = Convert.ToInt64(perTaskElements[0]);
                    newData.permission = perTaskElements[1];
                    idAndPermissions.Add(newData);
                }
            }
            idAndPermissions = idAndPermissions.OrderBy(m => m.id).ToList();
            return idAndPermissions;
        }

        public class Criteria
        {
            /// <summary>
            /// 身份名稱
            /// </summary>
            public string? roleName { set; get; }
            /// <summary>
            /// 系統代碼
            /// </summary>
            public string? sysCode { set; get; }
            /// <summary>
            /// 使用者帳號
            /// </summary>
            public string? userName { set; get; }
            /// <summary>
            /// 查詢頁數
            /// </summary>
            public int? pageNumber { get; set; }
            /// <summary>
            /// 單頁最大筆數
            /// </summary>
            public int? pageSize { get; set; }
        }
    }
}
