using System.Reflection;

[AttributeUsage(AttributeTargets.All)]
public class ColumnAttribute : System.Attribute
{
    private string displayName;

    public ColumnAttribute(string _displayName)
    {
        this.displayName = _displayName;
    }

    public static string GetDisplayName(object enm)
    {
        if (enm != null)
        {
            MemberInfo[] mi = enm.GetType().GetMember(enm.ToString() ?? "");
            if (mi != null && mi.Length > 0)
            {
                ColumnAttribute attr = Attribute.GetCustomAttribute(mi[0],
                    typeof(ColumnAttribute)) as ColumnAttribute ?? new ColumnAttribute("");
                if (attr != null)
                {
                    return attr.displayName;
                }
            }
        }
        return "";
    }

    public string GetDisplayName()
    {
        return this.displayName;
    }
}
