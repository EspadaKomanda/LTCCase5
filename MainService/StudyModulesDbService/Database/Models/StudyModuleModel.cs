using System.ComponentModel.DataAnnotations;
namespace StudyModulesDbService.Database.Models
{
    public class StudyModuleModel
    {
        [Key]
        public Guid uuid { get; set; }

        public string name { get; set; }

        public string asignee { get; set; }
    }

}