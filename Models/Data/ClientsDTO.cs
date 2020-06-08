using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankApplication.Models.Data
{
    [Table("tblClients")]
    public class ClientsDTO
    {
        [Key]
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public int ClientAge { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public decimal Balance { get; set; }
        public int EmployeeId { get; set; }
        public string MyEmployee { get; set; }
        public decimal Credit { get; set; }
        public decimal Deposit { get; set; }
        public string ClientLogin { get; set; }
        public string ClientPassword { get; set; }
        public bool BanStatus { get; set; } = false;

        [ForeignKey("EmployeeId")]
        public virtual EmployeesDTO Employee { get; set; }
    }
}