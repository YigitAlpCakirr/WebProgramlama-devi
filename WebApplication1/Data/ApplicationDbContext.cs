using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;  // User modelini kullanmak için doğru namespace'i ekleyin

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }  // DbSet<User> kullan
    }
}



