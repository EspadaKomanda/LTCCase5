using System.ComponentModel.DataAnnotations;

namespace MainService.Pages
{
    public class CreationUserModel
    {
        [RegularExpression(@"/[a-zа-яё\d\-\. ]{1,}/i",
            ErrorMessage = "Должность должна быть длиной не менее 1 зн. и не может содержать специальные символы кроме пробела, точки и дефиса.")]
        public string position { get; set; }
        [EmailAddress(ErrorMessage = "Неправильный адрес электронной почты.")]
        public string email { get; set; }

        [Phone(ErrorMessage = "Введите правильный номер телефона.")]
        public string phone { get; set; }
        [RegularExpression(@"/[a-zа-яё\-]{1,}/i",
            ErrorMessage = "Имя должно быть длиной не менее 1 зн. и содержать только буквы русского и латинского алфавитов или дефис.")]
        public string name { get; set; }
        public string password { get; set; }
    }
}
