using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankApplication.Models.Data
{
    [Table("tblBlackList")]
    public class BlackListDTO
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Reason { get; set; }
    }
}