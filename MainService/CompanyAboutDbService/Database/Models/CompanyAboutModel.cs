using System.ComponentModel.DataAnnotations;
namespace CompanyAboutDbService.Database.Models
{
    public class CompanyAboutModel
    {
        [Key]
        public Guid uuid { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string executives { get; set; }
    }

}
