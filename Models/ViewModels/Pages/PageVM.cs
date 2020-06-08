using BankApplication.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankApplication.Models.ViewModels.Pages
{
    public class PageVM
    {
        public int PageId { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Длина строки от 3-х до 200 символов")]
        [DisplayName("Имя страницы")]
        public string Title { get; set; }

        [DisplayName("Краткое описание")]
        public string Description { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 0)]
        [DisplayName("Тело страницы")]
        [AllowHtml]
        public string Body { get; set; }

        [Required(ErrorMessage = "Выберите категорию страницы")]
        [DisplayName("Категория страницы")]
        public int PageCategoryId { get; set; }

        public string PageCategory { get; set; }

        [DisplayName("Сайдбар")]
        public bool Sidebar { get; set; }

        public IEnumerable<SelectListItem> categoryList { get; set; }

        public PageVM(PagesDTO row)
        {
            PageId = row.PageId;
            Title = row.Title;
            Description = row.Description;
            Body = row.Body;
            PageCategoryId = row.PageCategoryId;
            PageCategory = row.PageCategory;
            Sidebar = row.SideBar;
        }

        public PageVM() { }
    }
}