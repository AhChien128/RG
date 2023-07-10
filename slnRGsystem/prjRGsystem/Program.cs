using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Filters;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Services;
using prjRGsystem.Services.DbServices;

namespace prjRGsystem
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ActionLoggerFilter>();
                options.Filters.Add<HostMappingFilter>();
                options.Filters.Add<UserLoginFilter>();
                options.Filters.Add<RolePrivilegeFilter>();
            });
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(720);
            });
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDbContext<RGPropertyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RGPropertyContext")));

            builder.Services.AddScoped<SysDepartmentManager>();
            builder.Services.AddScoped<SysEnterpriseManager>();
            builder.Services.AddScoped<SysRolesManager>();
            builder.Services.AddScoped<SysRolesTasksManager>();
            builder.Services.AddScoped<SysTasksManager>();
            builder.Services.AddScoped<SysTasksBlackManager>();
            builder.Services.AddScoped<SysRolesTasksBlackManager>();
            builder.Services.AddScoped<SysUserManager>();
            builder.Services.AddScoped<SysUsersRolesManager>();
            builder.Services.AddScoped<SysLogAccessManager>();
            builder.Services.AddScoped<SysLogApiManager>();
            builder.Services.AddScoped<SysLogDataChangeManager>();


            builder.Services.AddScoped<SysDepartmentService>();
            builder.Services.AddScoped<SysEnterpriseService>();
            builder.Services.AddScoped<SysRolesService>();
            builder.Services.AddScoped<SysRolesTasksService>();
            builder.Services.AddScoped<SysTasksService>();
            builder.Services.AddScoped<SysTasksBlackService>();
            builder.Services.AddScoped<SysRolesTasksBlackService>();
            builder.Services.AddScoped<SysUserService>();
            builder.Services.AddScoped<SysUsersRolesService>();

            builder.Services.AddScoped<SysLogAccessService>();
            builder.Services.AddScoped<SysLogLoginService>();
            builder.Services.AddScoped<SysLogApiService>();
            builder.Services.AddScoped<SysLogDataChangeService>();

            builder.Services.AddScoped<SysLogFileDownloadService>();

            builder.Services.AddScoped<SysUserAskForLeaveManager>();
            builder.Services.AddScoped<SysUserAskForLeaveService>();

            builder.Services.AddScoped<SysUserSubstituteManager>();
            builder.Services.AddScoped<SysUserSubstituteService>();
            builder.Services.AddScoped<RolePrivilegeService>();

            builder.Services.AddScoped<MemberService>();
            builder.Services.AddScoped<MemberManager>();
            builder.Services.AddScoped<MemberCarsManager>();
            builder.Services.AddScoped<MemberCarsService>();    
            
            builder.Services.AddScoped<ItemsManager>();
            builder.Services.AddScoped<ItemsService>();
            builder.Services.AddScoped<FixItemsManager>();
            builder.Services.AddScoped<FixItemsService>();
            builder.Services.AddScoped<ItemOrderManager>();
            builder.Services.AddScoped<ItemOrderService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Login/errorPage/ErrorPage404";
                    await next();
                }
                else if (context.Response.StatusCode == 500)
                {
                    context.Request.Path = "/Login/errorPage/ErrorPage500";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();
            app.MapControllerRoute(
                name: "areaRoute",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}