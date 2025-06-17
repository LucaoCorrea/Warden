using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
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
            QuestPDF.Settings.License = LicenseType.Community;
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
            services.AddScoped<ISaleRepository, SaleRepository>();

            services.AddScoped<StockMovementService>();
            services.AddScoped<ProductService>();
            services.AddScoped<SaleService>();

            services.AddScoped<ICashRegisterRepository, CashRegisterRepository>();
            services.AddScoped<CashRegisterService>();

            services.AddScoped<IReleaseNoteRepository, ReleaseNoteRepository>();
            services.AddScoped<IUserReleaseViewRepository, UserReleaseViewRepository>();


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
                CreateDefaultContact(context);
                CreateDefaultProduct(context);
                CreateDefaultReleaseNote(context);
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

        private void CreateDefaultContact(AppDbContext context)
        {
            if (!context.Contacts.Any(c => c.Email == "lucas@warden.com"))
            {
                var contato = new ContactModel
                {
                    Name = "Lucas Leonardi",
                    Email = "lucas@warden.com",
                    Phone = "(11) 91234-5678",
                    UserId = context.Users.FirstOrDefault(u => u.Login == "admin")?.Id
                };

                context.Contacts.Add(contato);
                context.SaveChanges();
            }
        }

        private void CreateDefaultProduct(AppDbContext context)
        {
            if (!context.Products.Any(p => p.SKU == "REF-COCA-1L"))
            {
                var produto = new ProductModel
                {
                    Name = "Coca Cola 1L",
                    SKU = "REF-COCA-1L",
                    Description = "Refrigerante Coca Cola de 1 Litro.",
                    Category = "Bebidas",
                    Stock = 50,
                    CostPrice = 11,
                    SalePrice = 13.90m,
                    ImageUrl = "https://www.extramercado.com.br/img/uploads/1/844/19796844.jpg",
                    Unit = "un",
                    CreatedAt = DateTime.Now
                };

                context.Products.Add(produto);
                context.SaveChanges();
            }
        }

        private void CreateDefaultReleaseNote(AppDbContext context)
        {
            if (!context.ReleaseNotes.Any(r => r.Version == "BETA - 1.2.2.25"))
            {
                var release = new ReleaseNoteModel
                {
                    Title = "Nota de Atualização",
                    Version = "BETA - 1.2.2.25",
                    Description = @"
- Novo sistema de vendas com integração ao estoque
- Módulo de Caixa completo com abertura, fechamento e histórico
- Sistema Warden Lovers de fidelização com cashback (em desenvolvimento)
- Exportação de relatórios para Excel
- Geração de notas fiscais simuladas em PDF
",
                    ReleaseDate = DateTime.Now
                };

                context.ReleaseNotes.Add(release);
                context.SaveChanges();
            }
        }
    }
}
