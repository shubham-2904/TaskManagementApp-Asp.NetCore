using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Models.Domain;


namespace TaskManagerApp.Models
{
    public class TaskManagerDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost; Database=TaskManagerDB; Trusted_Connection=True; TrustServerCertificate=True");
        }

        public DbSet<TaskList> Lists { get; set; }
        public DbSet<Plan> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
