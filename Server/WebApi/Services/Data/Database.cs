using Microsoft.EntityFrameworkCore;
using WebApi.Services.Data.Entities;

namespace WebApi.Services
{
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
