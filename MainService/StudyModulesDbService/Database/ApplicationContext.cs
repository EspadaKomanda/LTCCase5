using StudyModulesDbService.Communicators;
using StudyModulesDbService.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace StudyModulesDbService.Database
{
    public class ApplicationContext : DbContext
    {
        private ConfigCommunicator _communicator = new ConfigCommunicator();
        public DbSet<StudyModuleModel> companyAbouts { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected async override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = await _communicator.GetDb("studymodulesdb");
            optionsBuilder.UseNpgsql($"Server = {options.Server}; Database ={options.Databasename}; Uid = {options.User}; Pwd = {options.Password};");
        }
    }
}