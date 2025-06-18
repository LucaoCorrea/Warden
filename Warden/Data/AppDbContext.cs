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
        public DbSet<StockMovementModel> StockMovement { get; set; }
        public DbSet<SaleModel> Sales { get; set; }
        public DbSet<SaleItemModel> SaleItems { get; set; }
        public DbSet<CashRegisterModel> CashRegisters { get; set; }
        public DbSet<CashMovementModel> CashMovements { get; set; }
        public DbSet<LoyalCustomerModel> LoyalCustomers { get; set; }

        public DbSet<ChatModel> Chat { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }
        public DbSet<ReleaseNoteModel> ReleaseNotes { get; set; }
        public DbSet<UserReleaseViewModel> UserReleaseViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactMap());
            modelBuilder.ApplyConfiguration(new SaleMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
