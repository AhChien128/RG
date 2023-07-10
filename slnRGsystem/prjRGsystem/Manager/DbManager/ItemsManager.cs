using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Manager.DbManager
{
    public class ItemsManager : AbstractAppEntityManager<Items>
    {
        public ItemsManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db)
        {


        }
        public override IQueryable<Items> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.Items.Where(item => !item.removed);
            else
                return db.Items.AsNoTracking().Where(item => !item.removed);
        }
        public IQueryable<Items> ExcuteQuery(Criteria criteria)
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select * from " + Items.TABLE_NAME);

            sql.Append(" where  1=1 and removed = 0 ");
            if (!string.IsNullOrEmpty(criteria.name))
            {
                sql.Append(" and productName like @productName");
                parameters.Add(new SqlParameter("productName", "%" + criteria.name + "%"));
            }
            return db.Items.FromSqlRaw(sql.ToString(), parameters.ToArray());

        }

        public class Criteria
        {
            /// <summary>
            /// 頁數
            /// </summary>
            public int pageNumber { get; set; }
            /// <summary>
            /// 材料名稱
            /// </summary>
            public string? name { get; set; }
        }
    }
}
