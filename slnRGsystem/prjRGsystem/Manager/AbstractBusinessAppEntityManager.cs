using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Models;

namespace prjRGsystem.Managers
{
    public abstract class AbstractBusinessAppEntityManager<T> : AbstractAppEntityManager<T> where T : AbstractBusinessAppEntity
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISession? session = null;
        public AbstractBusinessAppEntityManager(RGPropertyContext  _db, IHttpContextAccessor _httpContextAccessor) : base(_db)
        {
            httpContextAccessor = _httpContextAccessor;
            if (httpContextAccessor.HttpContext != null)
                session = httpContextAccessor.HttpContext.Session;

        }

        private protected UserContext GetUserLogin()
        {
            UserContext? userContext = null;
            if (session != null)
                userContext = session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
            return userContext ?? new();
        }

        public virtual IQueryable<T> GetEnterpriseEntitiesQ(bool isTracking = false)
        {
            long enterpriseId = 0;
            UserContext userContext = GetUserLogin();
            if (userContext != null && userContext.enterprise != null)
                enterpriseId = userContext.enterprise.id;
            return GetEntitiesQ(isTracking).Where(item => item.enterpriseId == enterpriseId && item.removed == false);
        }

        public virtual IQueryable<T> GetDepartmentEntitiesQ(bool isTracking = false)
        {
            long sysDepartmentsId = 0;
            UserContext userContext = GetUserLogin();
            if (userContext != null && userContext.department != null)
                sysDepartmentsId = userContext.department.id;

            return GetEnterpriseEntitiesQ(isTracking).Where(item => item.departmentId == sysDepartmentsId);
        }
    }
}
