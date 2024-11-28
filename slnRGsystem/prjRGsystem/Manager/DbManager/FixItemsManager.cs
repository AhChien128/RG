using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Manager.DbManager
{
    public class FixItemsManager : AbstractAppEntityManager<FixItems>
    {
        public FixItemsManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db)
        {


        }
        public override IQueryable<FixItems> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.FixItems.Where(item => !item.removed);
            else
                return db.FixItems.AsNoTracking().Where(item => !item.removed);
        }

        public IQueryable<FixItems> ExcuteQuery(string timeType = "today")
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" select " + FixItems.TABLE_NAME + ". * from " + FixItems.TABLE_NAME);
            sql.Append(" LEFT JOIN " + MemberCars.TABLE_NAME + " on fixItems.memberCarID = memberCars.ID ");
            sql.Append(" where 1=1 and FixItems.removed = 0 and MemberCars.removed=0  ");
            if (timeType == "today")
            {
                sql.Append(" and CONVERT(date, fixItems.createDate) = CONVERT(date, GETDATE())");
            }
            if (timeType == "toMonth")
            {
                sql.Append(" AND YEAR(FixItems.createDate) = YEAR(GETDATE()) ");
                sql.Append(" AND MONTH(FixItems.createDate) = MONTH(GETDATE()) ");
            }
            if (timeType == "toYear")
            {
                sql.Append(" AND YEAR(FixItems.createDate) = YEAR(GETDATE()) ");
            }
            return db.FixItems.FromSqlRaw(sql.ToString(), parameters.ToArray());

        }

    }
}
