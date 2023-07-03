
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Context
{
    public class UserContext
    {
        public const string SESSION_NAME = "UserContext";

        public SysUser? user;
        public SysEnterprise? enterprise;
        public SysDepartment? department;
        public Dictionary<string, SysRolesTasks>? authorizedSysCodes;  //areaName+controllerName,  SysRolesTasks
        public Dictionary<string, List<string>>? authorizedTaskBlackNames; // areaName+controllerName, List<SysTasksBlack.actionName>
        public List<SysRoles>? sysRoles;

        public LinkedList<SysTasks>? authorizedSysTasks { get; set; }//已授權可用功能
        public SysRolesTasks? currentPageSysCodesTypes { get; set; }//當前功能細項
        public List<string>? currentPageTaskBlackNames { get; set; }//當前功能黑名單動作名稱清單

        public string GetSessionName()
        {
            return SESSION_NAME;
        }
    }
}
