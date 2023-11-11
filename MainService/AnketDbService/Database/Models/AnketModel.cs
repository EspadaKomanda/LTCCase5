using System.ComponentModel.DataAnnotations;
namespace AnketDbService.Database.Models
{
    public class AnketModel
    {
        [Key]
        public Guid id { get; set; }
        public Guid parentID { get; set; }
        public string text { get; set; }
        public string type { get; set; } 
        public string answerVariants { get; set; }
    }

}
