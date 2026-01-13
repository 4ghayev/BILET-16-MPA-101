using BILET_16_MPA_101.Models;
using Microsoft.EntityFrameworkCore;

namespace BILET_16_MPA_101.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
