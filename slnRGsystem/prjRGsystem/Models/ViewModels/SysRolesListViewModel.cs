using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Models.ViewModels
{
    public class SysRolesListViewModel
    {
        public Int64 sysRolesId { get; set; }
        public SysRoles? sysRole { get; set; }
        public List<SysUsersRoles>? sysUsersRoles { get; set; }
        public List<SysRolesTasks>? sysRolesTasks { get; set; }
        public string? roleName { get; set; }
        public string? sysUserIds { get; set; }
        public string? sysTasksString { get; set; }
        [NotMapped]
        public List<string>? permissions { get; set; }//權限
        public List<SysTasksBlack>? sysTasksBlack { get; set; }

        public List<SysRolesTasksBlack>? sysRolesTasksBlack { get; set; }
        [NotMapped]
        Dictionary<long, List<SysTasksBlack>>? dicSysTasksBlack { get; set; }
        public List<string>? hasBlackCheck { get; set; }
        
    }
}
