
using Microsoft.EntityFrameworkCore;

namespace RedisAPI.Models
{
    public class DataContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Igrica>? Igrice { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}