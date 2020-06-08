using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankApplication.Models.Data
{
    [Table("tblEmployees")]
    public class EmployeesDTO
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public int EmployeeAge { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeLogin { get; set; }
        public string EmployeePassword { get; set; }
        public int EmployeePositionId { get; set; }
        public string EmployeePosition { get; set; }
        public decimal EmployeeSalary { get; set; }
        public int ClientsCount { get; set; }
    }
}