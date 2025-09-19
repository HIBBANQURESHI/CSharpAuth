using AuthWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthWebAPI.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }

    }
}
