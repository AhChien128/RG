using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using prjRGsystem.Extensions;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using System.Reflection;

namespace prjRGsystem.Context
{
    public class RGPropertyContext : DbContext
    {
        private readonly ISession? session = null;

        public RGPropertyContext(DbContextOptions<RGPropertyContext> options, IHttpContextAccessor _httpContextAccessor) : base(options)
        {
            if (_httpContextAccessor.HttpContext != null)
                session = _httpContextAccessor.HttpContext.Session;
        }
        protected virtual UserContext GetUserLogin()
        {
            UserContext? userContext = null;
            if (session != null)
                userContext = session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
            return userContext ?? new UserContext();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UserContext userContext = GetUserLogin();
            AutoSetAbstractAppEntity(this.ChangeTracker.Entries(), userContext);
            AutoSetAbstractBusinessAppEntity(this.ChangeTracker.Entries(), userContext);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override int SaveChanges()
        {
            UserContext userContext = GetUserLogin();
            AutoSetAbstractAppEntity(this.ChangeTracker.Entries(), userContext);
            AutoSetAbstractBusinessAppEntity(this.ChangeTracker.Entries(), userContext);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UserContext userContext = GetUserLogin();
            this.ChangeTracker.DetectChanges();
            var sysDataChangeLoggerEntrys = OnBeforeSaveChanges(this.ChangeTracker.Entries(), userContext);
            AutoSetAbstractAppEntity(this.ChangeTracker.Entries(), userContext);
            AutoSetAbstractBusinessAppEntity(this.ChangeTracker.Entries(), userContext);
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await OnAfterSaveChanges(sysDataChangeLoggerEntrys, acceptAllChangesOnSuccess, cancellationToken);
            return result;
        }

        private List<SysLogDataChangeEntry> OnBeforeSaveChanges(IEnumerable<EntityEntry> entries, UserContext userContext)
        {
            var sysLogDataChangeEntryEntries = new List<SysLogDataChangeEntry>();
            foreach (var entry in entries)
            {
                if (typeof(ILoggerEntity).GetTypeInfo().IsAssignableFrom(entry.Entity.GetType().Ge‌​tTypeInfo()) ||
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                    continue;

                var sysLogDataChangeEntry = new SysLogDataChangeEntry(entry)
                {
                    requestId = (System.Diagnostics.Activity.Current != null && System.Diagnostics.Activity.Current.Id != null) ? System.Diagnostics.Activity.Current.Id : "",
                    sysUser = (userContext != null && userContext.user != null) ? userContext.user.userId : "" //存帳號
                };
                string? tableName = "";

                if (entry != null && entry.Metadata != null && entry.Metadata.GetTableName() != null)
                    tableName = entry.Metadata.GetTableName();


                sysLogDataChangeEntry.tableName = (!string.IsNullOrEmpty(tableName)) ? tableName : "";
                sysLogDataChangeEntryEntries.Add(sysLogDataChangeEntry);
                if (entry != null)
                {
                    foreach (var property in entry.Properties)
                    {
                        if (property.IsTemporary)
                        {//資料庫取出的資料
                            sysLogDataChangeEntry.TemporaryProperties.Add(property);
                            continue;
                        }

                        string propertyName = property.Metadata.Name;
                        object propertyCurrentValue = property.CurrentValue ?? new object();
                        object propertyOriginalValue = property.OriginalValue ?? new object();

                        if (property.Metadata.IsPrimaryKey())
                        {
                            sysLogDataChangeEntry.KeyValues[propertyName] = propertyCurrentValue;
                            continue;
                        }

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                sysLogDataChangeEntry.NewValues[propertyName] = propertyCurrentValue;
                                break;

                            case EntityState.Deleted:
                                sysLogDataChangeEntry.OldValues[propertyName] = propertyOriginalValue;
                                break;

                            case EntityState.Modified:
                                if (property.IsModified)
                                {//是否修改
                                    sysLogDataChangeEntry.OldValues[propertyName] = propertyOriginalValue;//原始資料
                                    sysLogDataChangeEntry.NewValues[propertyName] = propertyCurrentValue;//修改資料
                                }
                                break;
                        }
                    }

                }
            }

            foreach (var sysLogDataChangeEntry in sysLogDataChangeEntryEntries.Where(_ => !_.HasTemporaryProperties))
            {
                SysLogDataChange.Add(sysLogDataChangeEntry.ToSysLogDataChange());
            }

            return sysLogDataChangeEntryEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<SysLogDataChangeEntry> sysLogDataChangeEntries, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (sysLogDataChangeEntries == null || sysLogDataChangeEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var sysLogDataChangeEntry in sysLogDataChangeEntries)
            {
                foreach (var prop in sysLogDataChangeEntry.TemporaryProperties)
                {
                    object? propCurrentValue = prop.CurrentValue ?? new object();
                    if (prop.Metadata.IsPrimaryKey())
                        sysLogDataChangeEntry.KeyValues[prop.Metadata.Name] = propCurrentValue;
                    else
                        sysLogDataChangeEntry.NewValues[prop.Metadata.Name] = propCurrentValue;
                }
                SysLogDataChange.Add(sysLogDataChangeEntry.ToSysLogDataChange());
            }

            return SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AutoSetAbstractAppEntity(IEnumerable<EntityEntry> entries, UserContext userContext)
        {
            var entities = entries.Where(e => e.State == EntityState.Added
                                         || e.State == EntityState.Modified)
                                        .Select(e => e.Entity).OfType<AbstractAppEntity>();

            string userId = "";
            if (userContext != null && userContext.user != null && userContext.user.userId != null) userId = userContext.user.userId;
            else userId = "admin";
            foreach (var entitie in entities)
            {
                if (entitie.id > 0) this.Entry(entitie).State = EntityState.Modified;
                if (entitie.createDate == null) entitie.createDate = DateTime.Now;
                if (string.IsNullOrEmpty(entitie.createUser)) entitie.createUser = userId;
                entitie.modifyDate = DateTime.Now;
                entitie.modifyUser = userId;
            }
        }

        private void AutoSetAbstractBusinessAppEntity(IEnumerable<EntityEntry> entries, UserContext userContext)
        {
            var abstractBusinessAppEntitys = entries
                                         .Where(e => e.State == EntityState.Added
                                                  || e.State == EntityState.Modified)
                                         .Select(e => e.Entity).OfType<AbstractBusinessAppEntity>();
            long enterpriseId = 0;
            if (userContext != null && userContext.enterprise != null) enterpriseId = userContext.enterprise.id;

            foreach (var abstractBusinessAppEntity in abstractBusinessAppEntitys)
            {
                if (!abstractBusinessAppEntity.nonSaveAutowiredEnterpriseId)
                    abstractBusinessAppEntity.enterpriseId = enterpriseId;

                if (!abstractBusinessAppEntity.nonSaveAutowiredDepartmentId
                    && userContext != null && userContext.user != null && userContext.user.userDepartmentId > 0)
                    abstractBusinessAppEntity.departmentId = userContext.user.userDepartmentId;
            }
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted ||
                            e.State == EntityState.Unchanged)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public DbSet<AbstractEntity> Set(string name)
        {
            // you may need to fill in the namespace of your context
            return base.Set<AbstractEntity>(name);
        }

        public DbSet<SysDepartment> SysDepartment => Set<SysDepartment>();
        public DbSet<SysEnterprise> SysEnterprise => Set<SysEnterprise>();
        public DbSet<SysLogAccess> SysLogAccess => Set<SysLogAccess>();
        public DbSet<SysLogApi> SysLogApi => Set<SysLogApi>();
        public DbSet<SysLogDataChange> SysLogDataChange => Set<SysLogDataChange>();
        public DbSet<SysLogFileDownload> SysLogFileDownload => Set<SysLogFileDownload>();
        public DbSet<SysLogLogin> SysLogLogin => Set<SysLogLogin>();
        public DbSet<SysRoles> SysRoles => Set<SysRoles>();
        public DbSet<SysRolesTasks> SysRolesTasks => Set<SysRolesTasks>();
        public DbSet<SysTasks> SysTasks => Set<SysTasks>();
        public DbSet<SysUser> SysUser => Set<SysUser>();
        public DbSet<SysUsersRoles> SysUsersRoles => Set<SysUsersRoles>();
        public DbSet<SysTasksBlack> SysTasksBlack => Set<SysTasksBlack>();
        public DbSet<SysRolesTasksBlack> SysRolesTasksBlack => Set<SysRolesTasksBlack>();
        public DbSet<SysUserAskForLeave> SysUserAskForLeave => Set<SysUserAskForLeave>();
        public DbSet<SysUserSubstitute> SysUserSubstitute => Set<SysUserSubstitute>();
        public DbSet<Member> Member => Set<Member>();
        public DbSet<MemberCars> MemberCars => Set<MemberCars>();
        public DbSet<Items> Items => Set<Items>();
        public DbSet<FixItems> FixItems => Set<FixItems>();
        public DbSet<ItemOrder> ItemOrder => Set<ItemOrder>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(19, 4);
        }
    }
}