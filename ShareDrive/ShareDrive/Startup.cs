using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShareDrive.Data;
using ShareDrive.Models;
using ShareDrive.Services;
using ShareDrive.Services.Contracts;
using ShareDrive.Common;
using ShareDrive.Infrastructure;
using AutoMapper;
using ShareDrive.Infrastructure.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShareDrive.Services.Mappings;

namespace ShareDrive
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEmailSender, EmailSender>();
            
            services.AddScoped<ICarsService, CarsService>();
            services.AddScoped<IDrivesService, DrivesService>();
            services.AddScoped<IDriveHelperService, DriveHelperService>();
            services.AddScoped<IDriveCarsHelperService, DriveCarsHelperService>();
            services.AddScoped<ICitiesService, CitiesService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));

            services.AddMvc(optiops =>
            {
                optiops.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(CarAutoMapperProfile));
                cfg.AddProfile(typeof(DriveAutoMapperProfile));
                cfg.AddProfile(typeof(CityAutoMapperProfile));
                cfg.AddProfile(typeof(UserAutoMapperProfile));
                cfg.AddProfile(typeof(AdminDriveAutoMapperProfile));
                cfg.AddProfile(typeof(AdminCarAutoMapperProfile));
                cfg.AddProfile(typeof(AdminUserAutoMapperProfile));
                cfg.AddProfile(typeof(ServicesDriveAutoMapperProfile));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDatabaseMigration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "areas",
                     template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                   );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");


            });
        }
    }
}
