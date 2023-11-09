using System.ComponentModel.DataAnnotations;
namespace StudyModulesDbService.Database.Models
{
    public class AttachmentModel
    {
        [Key]
        public Guid uuid { get; set; }

        public Guid parentId { get; set; }

        public string url { get; set; }
    }

}