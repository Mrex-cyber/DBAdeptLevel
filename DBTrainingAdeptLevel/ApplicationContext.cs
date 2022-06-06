using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBTrainingAdeptLevel
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;


        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<MenuItem> MenuItems { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DBAdeptLevel.db");            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().OwnsOne(u => u.UserProfile, p => { p.OwnsOne(n => n.Name); p.OwnsOne(a => a.Age); });
            builder.Entity<Company>();
            builder.Entity<Position>();
            builder.Entity<Country>();
            builder.Entity<City>();

            builder.Entity<Student>();
            builder.Entity<Course>();

            builder.Entity<MenuItem>();
        }
    }
}
