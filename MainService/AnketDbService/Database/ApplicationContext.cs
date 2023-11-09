using AnketDbService.Communicators;
using AnketDbService.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnketDbService.Database
{
    public class ApplicationContext : DbContext
    {
        private ConfigCommunicator _communicator = new ConfigCommunicator();
        public DbSet<AnketModel> ankets { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = _communicator.GetDb("anketDb").Result;
            Console.WriteLine(options.Databasename);

            optionsBuilder.UseNpgsql($"Server = {options.Server}; Database ={options.Databasename}; Uid = {options.User}; Pwd = {options.Password};");
        }
    }
}