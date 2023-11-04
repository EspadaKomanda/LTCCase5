using AuthDbService.Database.Models;
using Microsoft.EntityFrameworkCore;
namespace AuthDbService.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserModel> users { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"");
        }
    }
}