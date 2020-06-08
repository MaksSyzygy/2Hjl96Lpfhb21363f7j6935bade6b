using BankApplication.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BankApplication.Models.ViewModels.Clients
{
    public class BlackListVM
    {
        [DisplayName("#")]
        public int Id { get; set; }
        public int ClientId { get; set; }
        
        [DisplayName("Имя")]
        public string Name { get; set; }

        [DisplayName("Фамилия")]
        public string Surname { get; set; }

        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Телефон")]
        public string Phone { get; set; }

        [DisplayName("Причина блокировки")]
        public string Reason { get; set; }

        public BlackListVM(BlackListDTO row)
        {
            Id = row.Id;
            ClientId = row.ClientId;
            Name = row.Name;
            Surname = row.Surname;
            Login = row.Login;
            Email = row.Email;
            Phone = row.Phone;
            Reason = row.Reason;
        }

        public BlackListVM() { }
    }
}