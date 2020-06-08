using BankApplication.Models.Data;
using BankApplication.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankApplication.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public ActionResult Index(string page = "")
        {
            if (page == "")
            {
                page = "Home";
            }

            PageVM pageVM;
            PagesDTO pagesDTO;

            using (BankDB bankDB = new BankDB())
            {
                if (!bankDB.Pages.Any(x => x.Title.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }

            using (BankDB bankDB = new BankDB())
            {
                pagesDTO = bankDB.Pages.Where(x => x.Title == page).FirstOrDefault();
            }

            ViewBag.PageTitle = pagesDTO.Title;

            pageVM = new PageVM(pagesDTO);

            return View(pageVM);
        }
    }
}