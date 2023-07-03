using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Models.ViewModels;

namespace prjRGsystem.Services.DbServices
{
    public class SysTasksService : SysTasksManager
    {
        public SysTasksService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public async Task<List<SysTasks>> GetUserMenuAsync(List<SysRolesTasks> sysRolesTasksList)
        {
            List<SysTasks> userMenu;
            List<long> sysTasksIds = sysRolesTasksList.Select(m => m.sysTasksId).ToList();
            userMenu = await GetByIds(sysTasksIds, false).OrderBy(m => m.taskSort).ToListAsync();
            return userMenu;
        }

        public LinkedList<SysTasks> GetMenus(TreeNode<SysTasks>? treeNode)
        {
            LinkedList<SysTasks> menus = new LinkedList<SysTasks>();
            if (treeNode != null && treeNode.Children != null && treeNode.Children.Count > 0)
            {
                foreach (TreeNode<SysTasks> childTask in treeNode.Children)
                {
                    SysTasks sysTasks = childTask.Data;
                    if (childTask.Children.Count > 0)
                    {
                        sysTasks.childSysTask = GetMenus(childTask);
                    }
                    menus.AddLast(sysTasks);
                }
            }
            return menus;
        }
    }
}
