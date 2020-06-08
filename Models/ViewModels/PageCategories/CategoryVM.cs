using BankApplication.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models.ViewModels.PageCategories
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя категории")]
        [DisplayName("Имя категории")]
        public string Name { get; set; }

        public int categoryCount { get; set; }

        public CategoryVM(CategoriesDTO row)
        {
            Id = row.Id;
            Name = row.Name;
        }

        public CategoryVM() { }
    }
}