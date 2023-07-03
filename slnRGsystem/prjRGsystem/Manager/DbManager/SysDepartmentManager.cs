using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysDepartmentManager : AbstractAppEntityManager<SysDepartment>
    {
        private readonly ISession? session;
        public SysDepartmentManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
        }

        public override IQueryable<SysDepartment> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysDepartment.Where(item => !item.removed);
            else
                return db.SysDepartment.AsNoTracking().Where(item => !item.removed);
        }
        public IQueryable<SysDepartment> GetEnterpriseEntitiesQ(bool isTracking = false)
        {
            UserContext? userContext = GetUserLogin();
            SysEnterprise? sysEnterprise = (userContext != null) ? userContext.enterprise : null;
            long enterpriseId = sysEnterprise != null ? sysEnterprise.id : 0;
            return GetEntitiesQ(isTracking).Where(n => n.enterpriseId == enterpriseId);
        }
        public async Task<List<SysDepartment>> GetAllAsync()
        {
            return await db.SysDepartment.AsNoTracking().ToListAsync();
        }
        public IQueryable<SysDepartment> GetByDepartmentId(long Id, bool isTracking = false)
        {
            return GetEntitiesQ(isTracking).Where(m => m.id == Id);
        }
        private protected UserContext? GetUserLogin()
        {
            if (session is not null)
                return session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
            else
                return new UserContext();
        }
        public IQueryable<SysDepartment> GetByDepartmentNames(List<string> SysDepartmentNames, bool isTracking = false)
        {
            return GetEntitiesQ(isTracking).Where(m => SysDepartmentNames.Contains(m.departmentName ?? ""));
        }

        public IQueryable<SysDepartment> GetByBusinessTypes(List<BusinessType> businessTypes, bool isTracking = false)
        {
            List<string> businessTypeTexts = businessTypes.Select(item => item.ToString()).ToList();
            return GetEntitiesQ(isTracking).Where(m => businessTypeTexts.Contains(m.businessType ?? ""));
        }

        /// <summary>
        /// "會內管理人員"、"會內主管"、"系統管理者"可選擇其他單位，其餘只可選擇自己所屬的單位。
        /// </summary>
        /// <param name="isTracking"></param>
        /// <returns></returns>
        public IQueryable<SysDepartment> GetByUserSysRole(bool isTracking = false)
        {
            UserContext? userContext = GetUserLogin();
            List<SysRoles> sysRoles = userContext != null && userContext.sysRoles != null ? userContext.sysRoles.Where(m => m.roleName != null && (m.roleName.Contains("系統管理員") || m.roleName.Contains("會內管理員") || m.roleName.Contains("會內主管"))).ToList() : new List<SysRoles>();
            if (sysRoles.Count > 0)
            {
                return GetEnterpriseEntitiesQ(isTracking);
            }
            else
            {
                SysUser? sysUser = userContext != null ? userContext.user : null;
                long userDepartmentId = sysUser != null ? sysUser.userDepartmentId : 0;
                return GetByDepartmentId(userDepartmentId);
            }
        }

        /// <summary>
        /// 取得使用者所屬單位
        /// </summary>
        /// <param name="isTracking"></param>
        /// <returns></returns>
        public IQueryable<SysDepartment> GetByUserDepartment(bool isTracking = false)
        {
            UserContext? userContext = GetUserLogin();
            long userDepartmentId = userContext != null && userContext.user != null ? userContext.user.userDepartmentId : 0;
            return GetByDepartmentId(userDepartmentId, isTracking);
        }

        /// <summary>
        /// 綜合查詢方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IQueryable<SysDepartment> ExcuteQuery(Criteria criteria)
        {
            UserContext? userContext = GetUserLogin();
            SysEnterprise? sysEnterprise = userContext != null ? userContext.enterprise : null;
            long enterpriseId = sysEnterprise != null ? sysEnterprise.id : 0;
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select * from ").Append(SysDepartment.TABLE_NAME);
            sql.Append(" where removed = @removed");
            sql.Append(" and enterpriseId = @enterpriseId");
            parameters.Add(new SqlParameter("removed", false));
            parameters.Add(new SqlParameter("enterpriseId", enterpriseId));
            //部門名稱
            if (!String.IsNullOrEmpty(criteria.departmentName))
            {
                sql.Append(" and departmentName like @departmentName ");
                parameters.Add(new SqlParameter("departmentName", "%" + criteria.departmentName.Trim() + "%"));
            }
            if (!String.IsNullOrEmpty(criteria.id.ToString()))
            {
                sql.Append(" and id =  @id ");
                parameters.Add(new SqlParameter("id", criteria.id));
            }
            if (criteria.purchaseRequestStartDateStart != null)
            {
                sql.Append(" and purchaseRequestStartDate >= @purchaseRequestStartDateStart ");
                parameters.Add(new SqlParameter("purchaseRequestStartDateStart", Convert.ToDateTime(criteria.purchaseRequestStartDateStart).ToString("yyyy-MM-dd")));
            }
            if (criteria.purchaseRequestStartDateEnd != null)
            {
                sql.Append(" and purchaseRequestStartDate <= @purchaseRequestStartDateEnd ");
                parameters.Add(new SqlParameter("purchaseRequestStartDateEnd", Convert.ToDateTime(criteria.purchaseRequestStartDateEnd).ToString("yyyy-MM-dd")));
            }
            if (criteria.purchaseRequestEndDateStart != null)
            {
                sql.Append(" and purchaseRequestEndDate >= @purchaseRequestEndDateStart ");
                parameters.Add(new SqlParameter("purchaseRequestEndDateStart", Convert.ToDateTime(criteria.purchaseRequestEndDateStart).ToString("yyyy-MM-dd")));
            }
            if (criteria.purchaseRequestEndDateEnd != null)
            {
                sql.Append(" and purchaseRequestEndDate <= @purchaseRequestEndDateEnd ");
                parameters.Add(new SqlParameter("purchaseRequestEndDateEnd", Convert.ToDateTime(criteria.purchaseRequestEndDateEnd).ToString("yyyy-MM-dd")));
            }
            if (criteria.businessTypes != null && criteria.businessTypes.Count > 0)
            {
                List<string> paramHolders = new();
                for (int i = 0, length = criteria.businessTypes.Count; i < length; i++)
                {
                    paramHolders.Add($"@businessType_{i}");
                    parameters.Add(new SqlParameter($"businessType_{i}", criteria.businessTypes[i].ToString()));
                }
                sql.Append($" and businessType in ({String.Join(",", paramHolders)}) ");

            }
            return db.SysDepartment.FromSqlRaw(sql.ToString(), parameters.ToArray());
        }

        public IQueryable<SysDepartment> GetByBusinessType(BusinessType businessType, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(n => businessType.ToString() == n.businessType);
        }

        public class Criteria
        {
            /// <summary>
            /// 部門id
            /// </summary>
            public long? id { get; set; }

            /// <summary>
            /// 部門名稱
            /// </summary>
            public string? departmentName { get; set; }
            /// <summary>
            /// 上一層的部門序號
            /// </summary>
            public long? parentDepartmentId { get; set; }
            /// <summary>
            /// 採購申請日(起)
            /// </summary>
            public DateTime? purchaseRequestStartDateStart { get; set; }

            public DateTime? purchaseRequestStartDateEnd { get; set; }
            /// <summary>
            /// 採購申請日(迄)
            /// </summary>
            public DateTime? purchaseRequestEndDateStart { get; set; }
            public DateTime? purchaseRequestEndDateEnd { get; set; }
            /// <summary>
            /// 業務類別
            /// </summary>
            public List<BusinessType>? businessTypes { get; set; }
            /// <summary>
            /// 頁數
            /// </summary>
            public int? pageNumber { get; set; }
        }

    }
}
