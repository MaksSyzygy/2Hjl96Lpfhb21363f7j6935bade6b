using BankApplication.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace BankApplication.Models.ViewModels.Employees
{
    public class EmployeeVM
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина имени от 3-х до 20 символов")]
        [DisplayName("Имя")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина фамилии от 3-х до 20 символов")]
        [DisplayName("Фамилия")]
        public string EmployeeSurname { get; set; }

        [Required(ErrorMessage = "Укажите возраст")]
        [Range(18, 70, ErrorMessage = "Возраст сотрудника должен быть от 18 до 70 лет")]
        [DisplayName("Возраст")]
        public int EmployeeAge { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email адрес")]
        [DisplayName("Email адрес")]
        public string EmployeeEmail { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [RegularExpression(@"((\+38|8|\+3|\+ )[ ]?)?([(]?\d{3}[)]?[\- ]?)?(\d[ -]?){6,14}", ErrorMessage = "Некорректный номер телефона")]
        [DisplayName("Номер телефона")]
        public string EmployeePhone { get; set; }

        [Required(ErrorMessage = "Выберите логин")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин длиною от 3-х до 50 символов")]
        [DisplayName("Логин")]
        public string EmployeeLogin { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Пароль длиною от 3-х до 50 символов")]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string EmployeePassword { get; set; }

        [Required(ErrorMessage = "Введите подтверждение пароля")]
        [Compare("EmployeePassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [DisplayName("Подтвердите пароль")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Укажите должность")]
        [DisplayName("Должность")]
        public int EmployeePositionId { get; set; }

        [DisplayName("Должность")]
        public string EmployeePosition { get; set; }

        [Required(ErrorMessage = "Укажите заработную плату")]
        [DisplayName("Оклад")]
        public decimal EmployeeSalary { get; set; }

        [DisplayName("Кол-во клиентов")]
        public int ClientsCount { get; set; }

        public IEnumerable<SelectListItem> positionList { get; set; }

        public EmployeeVM(EmployeesDTO row)
        {
            EmployeeId = row.EmployeeId;
            EmployeeName = row.EmployeeName;
            EmployeeSurname = row.EmployeeSurname;
            EmployeeAge = row.EmployeeAge;
            EmployeeEmail = row.EmployeeEmail;
            EmployeePhone = row.EmployeePhone;
            EmployeeLogin = row.EmployeeLogin;
            EmployeePassword = row.EmployeePassword;
            EmployeePositionId = row.EmployeePositionId;
            EmployeePosition = row.EmployeePosition;
            EmployeeSalary = row.EmployeeSalary;
            ClientsCount = row.ClientsCount;
        }

        public EmployeeVM() { }

        public string Info()
        {
            return $"{EmployeeName} {EmployeeSurname} {EmployeePosition} {EmployeeLogin}";
        }
    }
}