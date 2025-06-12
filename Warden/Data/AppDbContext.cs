using Microsoft.EntityFrameworkCore;
using Warden.Data.Map;
using Warden.Models;

namespace Warden.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<UserModel> Users { get; set; }  

        public DbSet<ProductModel> Products { get; set; }

        public DbSet<StockMovementModel> stockMovement { get; set; }

        public DbSet<SaleModel> Sales { get; set; }
        public DbSet<SaleItemModel> SaleItems { get; set; }

        public DbSet<CashRegisterModel> CashRegisters { get; set; }
        public DbSet<CashMovementModel> CashMovements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactMap());

            

            base.OnModelCreating(modelBuilder);
        }
    }
}
