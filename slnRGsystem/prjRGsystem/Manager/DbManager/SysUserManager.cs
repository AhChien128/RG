using prjRGsystem.Context;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysUserManager : AbstractBusinessAppEntityManager<SysUser>
    {
        public SysUserManager(RGPropertyContext _db,
            IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysUser> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysUser.Where(item => !item.removed);
            else
                return db.SysUser.AsNoTracking().Where(item => !item.removed);
        }

        /// <summary>
        /// 綜合查詢
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IQueryable<SysUser> ExcuteQuery(Criteria criteria)
        {
            SysEnterprise? sysEnterprise = GetUserLogin().enterprise;
            //SysDepartment? sysDepartment = GetUserLogin().department;
            long enterpriseId = sysEnterprise is null ? 0 : sysEnterprise.id;
            //long departmentId = sysDepartment is null ? 0 : sysDepartment.id;
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select s.* from ").Append(SysUser.TABLE_NAME).Append(" as s ");
            sql.Append(" where s.removed = @removed ");
            parameters.Add(new SqlParameter("removed", false));
            sql.Append(" and s.enterpriseId = @enterpriseId ");
            parameters.Add(new SqlParameter("enterpriseId", enterpriseId));
            //sql.Append(" and s.departmentId = @departmentId ");
            //parameters.Add(new SqlParameter("departmentId", departmentId));
            if (!String.IsNullOrEmpty(criteria.userName))
            {
                sql.Append(" and s.userName like @userName ");
                parameters.Add(new SqlParameter("userName", "%" + criteria.userName.Trim() + "%"));
            }
            if (!String.IsNullOrEmpty(criteria.userId))
            {
                sql.Append(" and s.userId like @userId ");
                parameters.Add(new SqlParameter("userId", "%" + criteria.userId.Trim() + "%"));
            }
            if (!String.IsNullOrEmpty(criteria.email))
            {
                sql.Append(" and s.email like @email ");
                parameters.Add(new SqlParameter("email", "%" + criteria.email.Trim() + "%"));
            }
            if (SearchType.ENABLED.Equals(criteria.searchType))
            {
                sql.Append(" and s.enabled = 1 ");
            }
            if (SearchType.DISABLED.Equals(criteria.searchType))
            {
                sql.Append(" and s.enabled = 0 ");
            }
            if (criteria.userDepartmentId != 0)
            {
                sql.Append(" and s.userDepartmentId = @userDepartmentId ");
                parameters.Add(new SqlParameter("userDepartmentId", criteria.userDepartmentId));
            }
            return db.SysUser.FromSqlRaw(sql.ToString(), parameters.ToArray());
        }


        public async Task<List<SysUser>> GetAllAsync()
        {
            return await db.SysUser.AsNoTracking().ToListAsync();
        }

        public IQueryable<SysUser> GetBySysRolesIds(List<Int64> sysRolesIds, bool isTracking = false)
        {
            var re = from su in db.SysUser
                     join sur in db.SysUsersRoles on su.id equals sur.sysUsersId
                     where sysRolesIds.Contains(sur.sysRolesId)
                     select su;
            if (!isTracking)
                re = re.AsNoTracking();
            return re;
        }


        public IQueryable<SysUser> GetByUserIds(List<string> userIds, bool isTracking = false)
        {
            return GetEntitiesQ(isTracking).Where(m => m.userId != null && userIds.Contains(m.userId));
        }

        public IQueryable<SysUser> GetByNames(List<string> sysUserNames, bool isTracking = false)
        {
            return GetEntitiesQ(isTracking).Where(m => m.userName != null && sysUserNames.Contains(m.userName));
        }
        public IQueryable<SysUser> GetByUserDepartmentId(long departmentId, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(n => n.userDepartmentId == departmentId);
        }


        public IQueryable<SysUser> GetByUserDepartmentIds(List<long> departmentIds, bool isTracking = false)
        {
            return GetEntitiesQ(isTracking).Where(m => departmentIds.Contains(m.userDepartmentId));
        }

        public class Criteria
        {
            public string? userId { get; set; }
            public string? userName { get; set; }
            public string? email { get; set; }
            public SearchType? searchType { get; set; }
            public Int64 userDepartmentId { get; set; }
            public string? title { get; set; }
            public string? pwd { get; set; }
            public bool? isEnabled { get; set; }
            public int? pageNumber { get; set; }
            public string? batch { get; set; }
        }

    }
}
