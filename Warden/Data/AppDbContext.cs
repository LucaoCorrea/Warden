using Microsoft.EntityFrameworkCore;
using Warden.Models;

namespace Warden.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ContactModel> Contacts { get; set; }
    }
}
