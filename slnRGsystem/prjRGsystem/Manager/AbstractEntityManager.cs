using prjRGsystem.Context;
using prjRGsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace prjRGsystem.Managers
{
    public abstract class AbstractEntityManager<T> where T : AbstractEntity
    {
        private protected readonly RGPropertyContext db;

        public abstract IQueryable<T> GetEntitiesQ(bool isTracking = false);

        public AbstractEntityManager(RGPropertyContext _db)
        {
            db = _db;
        }

        public virtual T? GetById(Int64 id, bool isTracking = false)
        {
            return GetByIds(new List<Int64>() { id }, isTracking).FirstOrDefault();
        }

        public virtual Task<T?> GetByIdAsync(Int64 id, bool isTracking = false)
        {
            return GetByIds(new List<Int64>() { id }, isTracking).FirstOrDefaultAsync();
        }

        public virtual IQueryable<T> GetByIds(List<Int64> ids, bool isTracking = false)
        {
            return GetEntitiesQ(isTracking).Where(item => ids.Contains(item.id));
        }

        public virtual void Save(T entity, bool isDetachAllEntities = false)
        {
            Save(new List<T>() { entity }, isDetachAllEntities);
        }

        public virtual void Save(List<T> entitys, bool isDetachAllEntities = false)
        {
            if (entitys != null && entitys.Count > 0)
            {
                foreach (T entity in entitys)
                {
                    if (entity.id == 0)
                    {
                        db.Entry(entity).State = EntityState.Added;
                    }
                    if (entity is INumberEntity)
                    {
                        if (((INumberEntity)entity).sysNumber == null)
                        { 
                            ((INumberEntity)entity).GenerateSysNumber(db);
                        }
                    }
                }
                db.SaveChanges();
                if (isDetachAllEntities)
                    db.DetachAllEntities();
            }
        }

        public async virtual Task<int> SaveAsync(T entity, bool isDetachAllEntities = false)
        {
            return await (SaveAsync(new List<T>() { entity }, isDetachAllEntities));
        }

        public async virtual Task<int> SaveAsync(IEnumerable<T> entitys, bool isDetachAllEntities = false)
        {
            if (entitys != null && entitys.Count() > 0)
            {
                foreach (T entity in entitys)
                {
                    if (entity.id == 0)
                    {
                        db.Entry(entity).State = EntityState.Added;
                    }
                    if (entity is INumberEntity)
                    {
                        if (((INumberEntity)entity).sysNumber == null)
                        {
                            ((INumberEntity)entity).GenerateSysNumber(db);
                        }
                    }
                }
                int saveInt = await db.SaveChangesAsync();
                if (isDetachAllEntities)
                    db.DetachAllEntities();
                return saveInt;
            }
            return 0;
        }

        public virtual void Delete(List<Int64> ids)
        {
            List<T> entities = GetByIds(ids).ToList();
            foreach (T entity in entities)
            {
                db.Entry(entity).State = EntityState.Deleted;
            }
            db.SaveChanges();
        }

        public Task<int> DeleteAsync(T obj)
        {
            return DeleteAsync(new List<Int64>() { obj.id });
        }

        public Task<int> DeleteAsync(Int64 id)
        {
            return DeleteAsync(new List<Int64>() { id });
        }

        public async virtual Task<int> DeleteAsync(List<Int64> ids)
        {
            List<T> entities = await GetByIds(ids).ToListAsync();

            foreach (T entity in entities)
            {
                db.Entry(entity).State = EntityState.Deleted;
            }
            return await db.SaveChangesAsync();
        }
    }
}
