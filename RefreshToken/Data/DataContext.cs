using Microsoft.EntityFrameworkCore;
using RefreshToken.Model;

namespace RefreshToken.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RolesDetail> RolesDetail { get; set; }
    }
}
