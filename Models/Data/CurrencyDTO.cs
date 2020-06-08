using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankApplication.Models.Data
{
    [Table("tblCurrency")]
    public class CurrencyDTO
    {
        [Key]
        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCharCode { get; set; }

        public decimal CurrencyRate { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}