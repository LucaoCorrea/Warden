using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Warden.Data;
using Warden.Enums;
using Warden.Helper;
using Warden.Models;
using Warden.Repositories;
using Warden.Repository;
using Warden.Services;

namespace Warden
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Database")));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStockMovementRepository, StockMovementRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<StockMovementService>();
            services.AddScoped<ProductService>();

            services.AddScoped<ISessionHelper, Session>();
            services.AddScoped<IEmail, Email>();

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                CreateDefaultUser(context);
            }
        }

        private void CreateDefaultUser(AppDbContext context)
        {
            if (!context.Users.Any(u => u.Login == "admin"))
            {
                var user = new UserModel
                {
                    Name = "adm",
                    Login = "admin",
                    Email = "admin@warden.com",
                    Profile = ProfileEnum.Admin,
                    Password = "123",
                    CreatedAt = DateTime.Now
                };

                user.SetPasswordHash();

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
