using BankApplication.Models.Data;
using BankApplication.Models.ViewModels.PageCategories;
using BankApplication.Models.ViewModels.Pages;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankApplication.Areas.Admin.Controllers
{
    public class PageController : Controller
    {
        // GET: Admin/Page
        public ActionResult Index()
        {
            List<PageVM> pageList = new List<PageVM>();

            using (BankDB bankDB = new BankDB())
            {
                pageList = bankDB.Pages.ToArray().OrderBy(x => x.Title).Select(x => new PageVM(x)).ToList();

            }

            return View(pageList);
        }

        //GET: Admin/CreatePage
        [HttpGet]
        public ActionResult CreatePage()
        {
            PageVM pageVM = new PageVM();

            using(BankDB bankDB = new BankDB())
            {
                pageVM.categoryList = new SelectList(bankDB.Categories.ToList(), "Id", "Name");
            }

            return View(pageVM);
        }

        //POST: Admin/CreatePage
        [HttpPost]
        public ActionResult CreatePage(PageVM pageVM)
        {
            if (!ModelState.IsValid)
            {
                using (BankDB bankDB = new BankDB())
                {
                    pageVM.categoryList = new SelectList(bankDB.Categories.ToList(), "Id", "Name");

                    return View(pageVM);
                }
            }

            using (BankDB bankDB = new BankDB())
            {
                string description = null;

                PagesDTO pagesDTO = new PagesDTO();

                pagesDTO.Title = pageVM.Title;

                if (string.IsNullOrWhiteSpace(pageVM.Description))
                {
                    description = pageVM.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    description = pageVM.Description.Replace(" ", "-").ToLower();
                }

                if (bankDB.Pages.Any(x => x.Title == pageVM.Title))
                {
                    ModelState.AddModelError("titleExist", "Это имя страницы занято");

                    return View(pageVM);
                }
                else if(bankDB.Pages.Any(x => x.Description == pageVM.Description))
                {
                    ModelState.AddModelError("descrExist", "Этот адрес страницы занят");

                    return View(pageVM);
                }

                pagesDTO.Description = description;
                pagesDTO.Body = pageVM.Body;
                pagesDTO.SideBar = pageVM.Sidebar;
                pagesDTO.PageCategoryId = pageVM.PageCategoryId;

                CategoriesDTO categories = bankDB.Categories.FirstOrDefault(x => x.Id == pageVM.PageCategoryId);

                pagesDTO.PageCategory = categories.Name;

                bankDB.Pages.Add(pagesDTO);
                bankDB.SaveChanges();
            }

            TempData["OK"] = "Страница создана";

            return RedirectToAction("Index");
        }

        //GET: Admin/Page/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageVM pageVM = new PageVM();

            using (BankDB bankDB = new BankDB())
            {
                PagesDTO pagesDTO = bankDB.Pages.Find(id);

                if (pagesDTO == null)
                {
                    return Content("Страница не доступна");
                }

                pageVM = new PageVM(pagesDTO);

                pageVM.categoryList = new SelectList(bankDB.Categories.ToList(), "Id", "Name");
            }

            return View(pageVM);
        }

        //POST: Admin/Page/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM pageVM)
        {
            int id = pageVM.PageId;

            using(BankDB bankDB = new BankDB())
            {
                pageVM.categoryList = new SelectList(bankDB.Categories.ToList(), "Id", "Name");
            }

            if (!ModelState.IsValid)
            {
                return View(pageVM);
            }

            using (BankDB bankDB = new BankDB())
            {
                string description = null;

                PagesDTO pagesDTO = bankDB.Pages.Find(id);

                pagesDTO.Title = pageVM.Title;

                if (string.IsNullOrWhiteSpace(pageVM.Description))
                {
                    description = pageVM.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    description = pageVM.Description.Replace(" ", "-").ToLower();
                }

                if (bankDB.Pages.Where(x => x.PageId != pageVM.PageId).Any(x => x.Title == pageVM.Title))
                {
                    ModelState.AddModelError("titleError", "Это имя страницы занято");

                    return View(pageVM);
                }
                else if (bankDB.Pages.Where(x => x.PageId != pageVM.PageId).Any(x => x.Description == description))
                {
                    ModelState.AddModelError("descrError", "Этот адрес страницы занят");

                    return View(pageVM);
                }

                pagesDTO.Description = description;
                pagesDTO.Body = pageVM.Body;
                pagesDTO.PageCategoryId = pageVM.PageCategoryId;
                pagesDTO.SideBar = pageVM.Sidebar;

                CategoriesDTO categories = bankDB.Categories.FirstOrDefault(x => x.Id == pageVM.PageCategoryId);
                pagesDTO.PageCategory = categories.Name;

                bankDB.SaveChanges();
            }

            TempData["OK"] = "Страница изменена";

            return RedirectToAction("Index");
        }

        //GET: Admin/PageDetails/id
        [HttpGet]
        public ActionResult PageDetails(int id)
        {
            PageVM pageVM = new PageVM();

            using (BankDB bankDB = new BankDB())
            {
                PagesDTO pagesDTO = bankDB.Pages.Find(id);

                if (pagesDTO == null)
                {
                    return Content("Эта страница не доступна");
                }

                pageVM = new PageVM(pagesDTO);
            }

            return View(pageVM);
        }

        //GET: Admin/DeletePage/id
        [HttpGet]
        public ActionResult DeletePage(int id)
        {
            using (BankDB bankDB = new BankDB())
            {
                PagesDTO pagesDTO = bankDB.Pages.Find(id);

                bankDB.Pages.Remove(pagesDTO);

                bankDB.SaveChanges();
            }

            TempData["OK"] = "Страница удалена";

            return RedirectToAction("Index");
        }

        public ActionResult PageCategories()
        {
            List<CategoryVM> categoryList = new List<CategoryVM>();

            using(BankDB bankDB = new BankDB())
            {
                categoryList = bankDB.Categories.ToArray().OrderBy(x => x.Name).Select(x => new CategoryVM(x)).ToList();
            }

            return View(categoryList);
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(CategoryVM categoryVM)
        {
            TextInfo text = CultureInfo.CurrentCulture.TextInfo;

            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }

            using(BankDB bankDB = new BankDB())
            {
                if(bankDB.Categories.Any(x=>x.Name == categoryVM.Name))
                {
                    ModelState.AddModelError("catNameExist", "Эта категория уже создана");

                    return View(categoryVM);
                }

                CategoriesDTO categoriesDTO = new CategoriesDTO();

                categoriesDTO.Name = text.ToTitleCase(categoryVM.Name);

                bankDB.Categories.Add(categoriesDTO);
                bankDB.SaveChanges();
            }

            TempData["OK"] = "Категория создана";

            return View(categoryVM);
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            CategoryVM categoryVM = new CategoryVM();

            using(BankDB bankDB = new BankDB())
            {
                CategoriesDTO categoriesDTO = bankDB.Categories.Find(id);

                if(categoriesDTO == null)
                {
                    return Content("Категория не доступна");
                }

                categoryVM = new CategoryVM(categoriesDTO);
            }

            return View(categoryVM);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryVM categoryVM)
        {
            TextInfo text = CultureInfo.CurrentCulture.TextInfo;

            int id = categoryVM.Id;

            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }

            using(BankDB bankDB = new BankDB())
            {
                CategoriesDTO categoriesDTO = bankDB.Categories.Find(id);

                if(bankDB.Categories.Where(x => x.Id != categoryVM.Id).Any(x => x.Name == categoryVM.Name))
                {
                    ModelState.AddModelError("nameExist", "Категория уже создана");

                    return View(categoryVM);
                }

                categoriesDTO.Name = text.ToTitleCase(categoryVM.Name);
                bankDB.SaveChanges();
            }

            TempData["OK"] = "Категория изменена";

            return RedirectToAction("PageCategories");
        }

        public ActionResult DeleteCategory(int id)
        {
            using(BankDB bankDB = new BankDB())
            {
                CategoriesDTO categoriesDTO = bankDB.Categories.Find(id);

                bankDB.Categories.Remove(categoriesDTO);

                bankDB.SaveChanges();
            }

            TempData["OK"] = "Категория удалена";

            return RedirectToAction("PageCategories");
        }
    }
}