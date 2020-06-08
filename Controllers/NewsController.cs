using BankApplication.Models.Data;
using BankApplication.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace BankApplication.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            List<PageVM> newsList = new List<PageVM>();

            using(BankDB bankDB = new BankDB())
            {
                newsList = bankDB.Pages.ToArray().Where(x => x.Title != "Home").Select(x => new PageVM(x)).ToList();
            }

            return View(newsList);
        }

        [ActionName("news-details")]
        public ActionResult NewsDetails(string name)
        {
            PagesDTO pagesDTO;
            PageVM pageVM;

            int id = 0;

            using(BankDB bankDB = new BankDB())
            {
                if (!bankDB.Pages.Any(x => x.Description.Equals(name)))
                {
                    return RedirectToAction("Index", "News");
                }

                pagesDTO = bankDB.Pages.Where(x => x.Description.Equals(name)).FirstOrDefault();

                id = pagesDTO.PageId;

                pageVM = new PageVM(pagesDTO);
            }

            return View("NewsDetails", pageVM);
        }

        [ActionName("news-category")]
        public ActionResult NewsCategory(string name)
        {
            List<PageVM> categoryList = new List<PageVM>();

            using(BankDB bankDB = new BankDB())
            {
                categoryList = bankDB.Pages.ToArray().Where(x => x.PageCategory.Equals(name)).Select(x => new PageVM(x)).ToList();
            }

            return View("NewsCategory", categoryList);
        }

        public ActionResult FindNews(string filter = null)
        {
            TextInfo text = CultureInfo.CurrentCulture.TextInfo;
            List<PageVM> newsList = new List<PageVM>();

            using(BankDB bankDB = new BankDB())
            {
                newsList = bankDB.Pages.ToArray().Where(x => x.Description != "home").Select(x => new PageVM(x)).ToList();
            }

            if (filter != null)
            {
                filter = text.ToTitleCase(filter);
            }

            return PartialView("_FindNews", filter == null ? newsList : newsList.Where(x => x.Title.Contains(filter)));
        }
    }
}