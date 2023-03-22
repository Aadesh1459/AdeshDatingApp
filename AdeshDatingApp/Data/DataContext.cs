using AdeshDatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdeshDatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
    }
}