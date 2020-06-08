using BankApplication.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankApplication.Models.ViewModels.Currency
{
    public class CurrencyVM
    {
        public int Id { get; set; }

        [DisplayName("Валюта")]
        public string CurrencyName { get; set; }
        public string CurrencyCharCode { get; set; }

        [DisplayName("Курс")]
        public decimal CurrencyRate { get; set; }

        [DisplayName("Дата")]
        public DateTime ExchangeDate { get; set; }

        public IEnumerable<SelectListItem> firstCurrency { get; set; }
        public IEnumerable<SelectListItem> secondCurrency { get; set; }

        public CurrencyVM(CurrencyDTO row)
        {
            Id = row.Id;
            CurrencyName = row.CurrencyName;
            CurrencyCharCode = row.CurrencyCharCode;
            CurrencyRate = row.CurrencyRate;
            ExchangeDate = row.ExchangeDate;
        }

        public CurrencyVM() { }
    }
}