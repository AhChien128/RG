using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;
using RGContext.Context;
using static prjRGsystem.Services.DbServices.SysRolesService;

namespace prjRGsystem.Services.DbServices
{
    public class SysRolesTasksBlackService : SysRolesTasksBlackManager
    {
        private readonly SysRolesTasksManager sysRolesTasksManager;
        private readonly SysTasksBlackManager sysTasksBlackManager;
        private readonly SysRolesTasksBlackManager sysRolesTasksBlackManager;
        private readonly SysRolesManager sysRolesManager;
        public SysRolesTasksBlackService(SysRolesTasksManager _sysRolesTasksManager, SysTasksBlackManager _sysTasksBlackManager, SysRolesTasksBlackManager _sysRolesTasksBlackManager, SysRolesManager _sysRolesManager, RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {
            sysRolesTasksManager = _sysRolesTasksManager;
            sysTasksBlackManager = _sysTasksBlackManager;
            sysRolesTasksBlackManager = _sysRolesTasksBlackManager;
            sysRolesManager = _sysRolesManager;
        }
        public async Task SaveNewBlackTasks(Int64 sysRolesId, List<string> sysTasksString)
        {
            List<IdAndPermissions> idAndPermissions = sysRolesManager.HandlePermissions(sysTasksString);
            List<SysRolesTasksBlack> sysRolesTasksBlackList = new List<SysRolesTasksBlack>();

            if (idAndPermissions.Count > 0)
            {
                try
                {
                    foreach (IdAndPermissions data in idAndPermissions)
                    {
                        SysRolesTasksBlack sysRolesTasksBlack = new SysRolesTasksBlack();

                        switch (data.permission)
                        {
                            case "Add":
                            case "View":
                            case "Edit":
                            case "Delete":
                            case "Audit":
                            case "Report":
                            case "Admin":
                                break;
                            default:
                                sysRolesTasksBlack.sysRolesTasksId = sysRolesTasksManager.GetBySysRolesId(sysRolesId).Where(w => w.sysTasksId == data.id).Select(m => m.id).FirstOrDefault();
                                sysRolesTasksBlack.sysTasksBlackId = sysTasksBlackManager.GetEnterpriseEntitiesQ().Where(w => (w.actionName ?? "").Contains(data.permission ?? "")).Select(m => m.id).FirstOrDefault();
                                sysRolesTasksBlackList.Add(sysRolesTasksBlack);
                                break;
                        }
                    }
                }
                catch (Exception ex) { throw ex; }

            }
            await sysRolesTasksBlackManager.SaveAsync(sysRolesTasksBlackList);

        }

    }
}
