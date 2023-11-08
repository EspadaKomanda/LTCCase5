using AuthDbService.Communicators;
using AuthDbService.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthDbService.Database
{
    public class ApplicationContext : DbContext
    {
        private ConfigCommunicator _communicator = new ConfigCommunicator();
        public DbSet<UserModel> users { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = _communicator.GetDb("authdb").Result;
            Console.WriteLine(options.Databasename);

            optionsBuilder.UseNpgsql($"Server = {options.Server}; Database ={options.Databasename}; Uid = {options.User}; Pwd = {options.Password};");
        }
    }
}