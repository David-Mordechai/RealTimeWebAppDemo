using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;

namespace WebApi.Persistent
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
