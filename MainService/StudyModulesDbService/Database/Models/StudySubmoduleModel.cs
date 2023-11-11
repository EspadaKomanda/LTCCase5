using System.ComponentModel.DataAnnotations;
namespace StudyModulesDbService.Database.Models
{
    public class StudySubmoduleModel
    {
        [Key]
        public Guid uuid { get; set; }

        public Guid parentId { get; set; }

        public string name { get; set; }

        public string text { get; set; }
    }

}