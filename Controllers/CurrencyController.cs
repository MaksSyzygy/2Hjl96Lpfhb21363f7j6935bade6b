using BankApplication.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using BankApplication.Models.ViewModels.Currency;
using System.Data.Entity;

namespace BankApplication.Controllers
{
    public class CurrencyController : Controller
    {
        // GET: Currency
        public ActionResult Index()
        {
            List<CurrencyVM> currencyList = new List<CurrencyVM>();

            using (BankDB bankDB = new BankDB())
            {
                if (!bankDB.Currency.Any())
                {
                    NBU_data[] parsed = JsonConvert.DeserializeObject<NBU_data[]>(new WebClient
                    { Encoding = Encoding.UTF8 }.DownloadString(@"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json"));

                    CurrencyDTO currencyDTO = new CurrencyDTO();

                    foreach (var item in parsed)
                    {
                        currencyDTO.CurrencyName = item.txt;
                        currencyDTO.CurrencyCharCode = item.cc;
                        currencyDTO.CurrencyRate = item.rate;
                        currencyDTO.ExchangeDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        bankDB.Currency.Add(currencyDTO);
                        bankDB.SaveChanges();
                    }

                    currencyList = bankDB.Currency.ToArray().OrderBy(x => x.CurrencyName).Select(x => new CurrencyVM(x)).ToList();

                    return View(currencyList);
                }
                else
                {
                    var dateToday = DateTime.Now.ToShortDateString();
                    var exchangeDate = bankDB.Currency.Select(x => x.ExchangeDate).First().ToString("dd.MM.yyyy");

                    if (exchangeDate != dateToday)
                    {
                        NBU_data[] parsed = JsonConvert.DeserializeObject<NBU_data[]>(new WebClient
                        { Encoding = Encoding.UTF8 }.DownloadString(@"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json"));

                        foreach (var item in parsed)
                        {
                            CurrencyDTO currencyDTO = bankDB.Currency.FirstOrDefault(x => x.CurrencyCharCode == item.cc);

                            currencyDTO.CurrencyName = item.txt;
                            currencyDTO.CurrencyCharCode = item.cc;
                            currencyDTO.CurrencyRate = item.rate;
                            currencyDTO.ExchangeDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                            bankDB.SaveChanges();
                        }

                        currencyList = bankDB.Currency.ToArray().OrderBy(x => x.CurrencyName).Select(x => new CurrencyVM(x)).ToList();

                        return View(currencyList);
                    }
                    else
                    {
                        currencyList = bankDB.Currency.ToArray().OrderBy(x => x.CurrencyName).Select(x => new CurrencyVM(x)).ToList();

                        return View(currencyList);
                    }
                }
            }
        }

        private class NBU_data
        {
            [JsonProperty(PropertyName = "txt")]
            public string txt; // наименование валюты
            [JsonProperty(PropertyName = "rate")]
            public decimal rate; // курс валюты по отношению к гривне
            [JsonProperty(PropertyName = "cc")]
            public string cc; // международный код валюты
        }

        public ActionResult CurrencyCalc(string firstValute, string secondValute, decimal firstValuteCount = 1)
        {
            using(BankDB bankDB = new BankDB())
            {
                if(firstValute == null && secondValute == null)
                {
                    CurrencyDTO firstCurrencyDefault = bankDB.Currency.FirstOrDefault(x => x.CurrencyCharCode == "AUD");
                    CurrencyDTO secondCurrencyDefault = bankDB.Currency.FirstOrDefault(x => x.CurrencyCharCode == "AUD");

                    ViewBag.FisrtValuteName = firstCurrencyDefault.CurrencyName;
                    ViewBag.SecondValuteName = secondCurrencyDefault.CurrencyName;

                    ViewBag.Result = firstCurrencyDefault.CurrencyRate * firstValuteCount / secondCurrencyDefault.CurrencyRate;

                    return PartialView("_CurrencyCalc");
                }
                else
                {
                    CurrencyDTO firstCurrency = bankDB.Currency.FirstOrDefault(x => x.CurrencyCharCode == firstValute);
                    CurrencyDTO secondCurrency = bankDB.Currency.FirstOrDefault(x => x.CurrencyCharCode == secondValute);

                    ViewBag.FisrtValuteName = firstCurrency.CurrencyName;
                    ViewBag.SecondValuteName = secondCurrency.CurrencyName;

                    ViewBag.Result = firstCurrency.CurrencyRate * firstValuteCount / secondCurrency.CurrencyRate;

                    return PartialView("_CurrencyCalc");
                }
            }
        }
    }
}