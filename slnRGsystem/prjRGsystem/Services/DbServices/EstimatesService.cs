using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Models.DbModels;
using System.Text;

namespace prjRGsystem.Services.DbServices
{
    public class EstimatesService : EstimatesManager
    {
        public EstimatesService(RGPropertyContext _db) : base(_db)
        {
        }
        public IQueryable<Estimates> ExcuteQuery(Criteria criteria)
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            sql.Append(" SELECT * FROM " + Estimates.TABLE_NAME);

            sql.Append(" WHERE  1=1 AND removed = 0 ");
            if (!string.IsNullOrEmpty(criteria.keyWord))
            {
                sql.Append(" AND carOwner like @keyWord OR carLicense like @keyWord OR engineNum like @keyWord OR carCategory like @keyWord ");
                parameters.Add(new SqlParameter("@keyWord", "%" + criteria.keyWord + "%"));
            }
            return db.Estimates.FromSqlRaw(sql.ToString(), parameters.ToArray());

        }

    }
}
