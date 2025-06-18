using Warden.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Warden.Data.Map
{
    public class SaleMap : IEntityTypeConfiguration<SaleModel>
    {
        public void Configure(EntityTypeBuilder<SaleModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SaleDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CashbackUsed)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.HasOne(x => x.LoyalCustomer)
                .WithMany()
                .HasForeignKey(x => x.LoyalCustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);
        }
    }
}
