using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models.ViewModels.Login
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Введите логин")]
        [DisplayName("Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Оставаться в системе")]
        public bool RememberMy { get; set; }
    }
}