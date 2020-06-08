using BankApplication.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace BankApplication.Models.ViewModels.Clients
{
    public class ClientVM
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Укажите Ваше имя")]
        [DisplayName("Имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Минимальная длина имени 3 символа, максимальная 50")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Укажите Вашу фамилию")]
        [DisplayName("Фамилия")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Минимальная длина имени 3 символа, максимальная 50")]
        public string ClientSurname { get; set; }

        [Required(ErrorMessage = "Укажите свой возраст")]
        [Range(18, 90, ErrorMessage = "Возраст в пределах от 18 до 90 лет")]
        [DisplayName("Возраст")]
        public int ClientAge { get; set; }

        [Required(ErrorMessage = "Введите Ваш Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email адрес")]
        [DisplayName("Email адрес")]
        public string ClientEmail { get; set; }

        [Required(ErrorMessage = "Введите свой номер телефона")]
        [RegularExpression(@"((\+38|8|\+3|\+ )[ ]?)?([(]?\d{3}[)]?[\- ]?)?(\d[ -]?){6,14}", ErrorMessage = "Некорректный номер телефона")]
        [DisplayName("Номер телефона")]
        public string ClientPhone { get; set; }

        [DisplayName("Баланс на счету")]
        public decimal Balance { get; set; } = 0m;

        public int EmployeeId { get; set; }

        [DisplayName("Ваш менеджер")]
        public string MyEmployee { get; set; }

        [DisplayName("Кредитная задолженность")]
        public decimal Credit { get; set; } = 0m;

        [DisplayName("Сумма на депозите")]
        public decimal Deposit { get; set; } = 0m;

        [Required(ErrorMessage = "Укажите Ваш Логин")]
        [DisplayName("Логин")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Минимальная длина Логина 3 символа, максимальная 50")]
        public string ClientLogin { get; set; }

        [Required(ErrorMessage = "Укажите Ваш пароль")]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Минимальная длина пароля 3 символа, максимальная 50")]
        public string ClientPassword { get; set; }

        [Required(ErrorMessage = "Введите подтверждение пароля")]
        [Compare("ClientPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [DisplayName("Подтверждение пароля")]
        public string ConfirmPassword { get; set; }

        public bool BanStatus { get; set; }

        public IEnumerable<SelectListItem> employeeList { get; set; }

        public ClientVM(ClientsDTO row)
        {
            ClientId = row.ClientId;
            ClientName = row.ClientName;
            ClientSurname = row.ClientSurname;
            ClientAge = row.ClientAge;
            ClientEmail = row.ClientEmail;
            ClientPhone = row.ClientPhone;
            Balance = row.Balance;
            EmployeeId = row.EmployeeId;
            MyEmployee = row.MyEmployee;
            Credit = row.Credit;
            Deposit = row.Deposit;
            ClientLogin = row.ClientLogin;
            ClientPassword = row.ClientPassword;
            BanStatus = row.BanStatus;
        }

        public ClientVM() { }

        public string SearchInfo()
        {
            return $"{ClientName} {ClientSurname} {ClientAge} {ClientEmail} {ClientPhone} {ClientLogin}";
        }
    }
}