using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}