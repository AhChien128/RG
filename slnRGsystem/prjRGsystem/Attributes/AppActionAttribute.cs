using prjRGsystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace prjRGsystem.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class AppActionAttribute : System.Attribute
    {
        private string Name;
        private RolePrivilegeType[]? PrivilegeTypes;

        public AppActionAttribute(string _name, RolePrivilegeType[] _privilegeTypes)
        {
            this.Name = _name;
            this.PrivilegeTypes = _privilegeTypes;
        }

        public AppActionAttribute(string _name, RolePrivilegeType _privilegeType)
        {
            this.Name = _name;
            this.PrivilegeTypes = new[] { _privilegeType };
        }

        public AppActionAttribute(string _name)
        {
            this.Name = _name;
        }

        public static RolePrivilegeType[]? GetPrivilegeTypes(ControllerBase controller, string actionName)
        {
            if (controller != null && actionName != null)
            {
                Type type = controller.GetType();
                MethodInfo? mi = controller.GetType().GetMethod(actionName);
                if (mi != null)
                {
                    AppActionAttribute attr = Attribute.GetCustomAttribute(mi,
                    typeof(AppActionAttribute)) as AppActionAttribute ?? new AppActionAttribute("");
                    if (attr != null)
                    {
                        return attr.PrivilegeTypes;
                    }
                }
            }
            return null;
        }

        public static string GetName(ControllerBase controller, string actionName)
        {
            if (controller != null && actionName != null)
            {
                Type type = controller.GetType();
                MethodInfo? mi = controller.GetType().GetMethod(actionName);
                if (mi != null)
                {
                    AppActionAttribute attr = Attribute.GetCustomAttribute(mi,
                    typeof(AppActionAttribute)) as AppActionAttribute ?? new AppActionAttribute("");
                    if (attr != null)
                    {
                        return attr.Name;
                    }
                }
            }
            return "";

        }

        public string GetName()
        {
            return this.Name;
        }

        public RolePrivilegeType[]? GetPrivilegeTypes()
        {
            return this.PrivilegeTypes;
        }
    }
}
