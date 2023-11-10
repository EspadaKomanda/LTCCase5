using StudyModulesDbService.Communicators;
using StudyModulesDbService.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace StudyModulesDbService.Database
{
    public class ApplicationContext : DbContext
    {
        private ConfigCommunicator _communicator = new ConfigCommunicator();
        public DbSet<StudyModuleModel> studyModules { get; set; }
        public DbSet<StudySubmoduleModel> studySubmodules { get; set; }
        public DbSet<AttachmentModel> attachments { get; set; }
        public DbSet<TestModel> tests { get; set; }
        public DbSet<DeadlineModel> deadlines { get; set; }
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