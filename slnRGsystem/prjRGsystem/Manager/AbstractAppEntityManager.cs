using prjRGsystem.Context;
using prjRGsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace prjRGsystem.Managers
{
    public abstract class AbstractAppEntityManager<T> : AbstractEntityManager<T> where T : AbstractAppEntity
    {
        public AbstractAppEntityManager(RGPropertyContext _db) : base(_db)
        {

        }

        public virtual int Removed(T t)
        {
            return Removed(new List<Int64>() { t.id });
        }

        public virtual int Removed(List<T> ts)
        {
            return Removed(ts.Select(m => m.id).ToList());
        }

        public virtual int Removed(Int64 id, bool isDetachAllEntities = false)
        {
            return Removed(new List<Int64>() { id }, isDetachAllEntities);
        }

        public virtual int Removed(List<Int64> ids, bool isDetachAllEntities = false)
        {

            List<T> entities = GetByIds(ids, true).ToList();
            foreach (T entity in entities)
            {
                if (entity != null && entity.id > 0)
                {
                    entity.removed = true;
                }
            }
            int i = db.SaveChanges();
            if (isDetachAllEntities)
                db.DetachAllEntities();
            return i;
        }

        public virtual async Task<int> RemovedAsync(Int64 id, bool isDetachAllEntities = false)
        {
            return await RemovedAsync(new List<Int64>() { id }, isDetachAllEntities);
        }

        public virtual async Task<int> RemovedAsync(List<Int64> ids, bool isDetachAllEntities = false)
        {

            List<T> entities = await GetByIds(ids, true).ToListAsync();
            foreach (T entity in entities)
            {
                if (entity != null && entity.id > 0)
                {
                    entity.removed = true;
                }
            }
            int i = await db.SaveChangesAsync();
            if (isDetachAllEntities)
                db.DetachAllEntities();
            return i;
        }
    }
}
