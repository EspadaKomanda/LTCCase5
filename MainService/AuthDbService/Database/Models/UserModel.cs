using System.ComponentModel.DataAnnotations;
namespace AuthDbService.Database.Models
{
    public class UserModel
    {
        [Key]
        public Guid uuid { get; set; }

        [EmailAddress(ErrorMessage = "Неправильный адрес электронной почты.")]
        public string email { get; set; }

        [Phone(ErrorMessage = "Введите правильный номер телефона.")]
        public string phone { get; set; }

        [RegularExpression(@"/[a-z_\d]{5,32}/i",
            ErrorMessage = "Введите правильное имя пользователя Telegram.")]
        public string telegram { get; set; }

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

        [RegularExpression(@"/[a-zа-яё\d\-\. ]{1,}/i",
            ErrorMessage = "Должность должна быть длиной не менее 1 зн. и не может содержать специальные символы кроме пробела, точки и дефиса.")]
        public string position { get; set; }

        [RegularExpression(@"/[a-zа-яё\d\-\. ]{1,}/i",
            ErrorMessage = "Роль должна быть длиной не менее 1 зн. и не может содержать специальные символы кроме пробела, точки и дефиса.")]
        public string role { get; set; }

        [RegularExpression(@"/.{0,70}/",
            ErrorMessage = "Отчество должно содержать только буквы русского и латинского алфавитов или дефис.")]
        public string about { get; set; }

        public string avatar { get; set; }
    }

}
