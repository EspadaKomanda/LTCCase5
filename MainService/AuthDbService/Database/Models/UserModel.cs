using System.ComponentModel.DataAnnotations;
namespace AuthDbService.Database.Models
{
    public class UserModel
    {
        [Key]
        public Guid uuid { get; set; }

        //[RegularExpression(@"/^[\w-\.]+@([\w-]+\.)+[\w-]{2,}$/g", ErrorMessage = "Invalid email format.")]
        [EmailAddress(ErrorMessage = "Неправильный адрес электронной почты.")]
        public string email { get; set; }

        [RegularExpression(@"/[a-zа-яё\-]{1,}/i",
            ErrorMessage = "Имя должно быть длиной не менее 1 зн. и содержать только буквы русского и латинского алфавитов или дефис.")]
        public string firstName { get; set; }

        [RegularExpression(@"/[a-zа-яё\-]{1,}/i", 
            ErrorMessage = "Фамилия должна быть длиной не менее 1 зн. и содержать только буквы русского и латинского алфавитов или дефис.")]
        public string lastName { get; set; }

        [RegularExpression(@"/[a-zа-яё\-]{0,}/i", 
            ErrorMessage = "Отчество должно содержать только буквы русского и латинского алфавитов или дефис.")]
        public string patronymic { get; set; }

        public string password { get; set; }

        [RegularExpression(@"/[a-zа-яё\d\-\. ]{1,}/i", ErrorMessage = "Роль должна быть длиной не менее 1 зн. и не может содержать специальные символы кроме пробела, точки и дефиса.")]
        public string role { get; set; }
    }

}
