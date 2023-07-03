using prjRGsystem.Context;

namespace prjRGsystem.Models
{
    public abstract class AbstractEntity
    {
        public virtual Int64 id { get; set; }

        //public string DecryptText(string text)
        //{
        //    return EncryptUtil.DecryptAES(text);
        //}
        public virtual void GenerateSysNumber(RGPropertyContext db)
        {
            /*
            Type entityType = this.GetType();
            string entityTypeName = entityType.Name;
            FieldInfo? fieldInfo = entityType.GetField("TABLE_NAME");
            if (fieldInfo != null)
            {
                string tableName = fieldInfo.GetValue(null).ToString() ?? "";

                StringBuilder sql = new();
                List<SqlParameter> parameters = new();
                sql.Append($" select max(sysNumber) from {tableName} ");
                var data = db.SysUser.FromSqlRaw(sql.ToString(), parameters.ToArray()).ToList();
                Console.WriteLine(data);
                / *
                var query = db.Set(entityTypeName);
                if (query != null)
                {
                    string sysNumber = query
                    .Select(item => item != null ? ((INumberEntity)item).sysNumber : "")
                    .OrderByDescending(item => item)
                    .First();
                    Console.WriteLine(sysNumber);
                }
                * /
            }
            */
        }

    }
}
