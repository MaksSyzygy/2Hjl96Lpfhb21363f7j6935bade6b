using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BankApplication.Models.ViewModels.Login
{
    public class LoginPartialVM
    {
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Фамилия")]
        public string Surname { get; set; }
    }
}