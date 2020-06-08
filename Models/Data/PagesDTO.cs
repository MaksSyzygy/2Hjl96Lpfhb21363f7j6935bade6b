using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankApplication.Models.Data
{
    [Table("tblPages")]
    public class PagesDTO
    {
        [Key]
        public int PageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public int PageCategoryId { get; set; }
        public string PageCategory { get; set; }
        public bool SideBar { get; set; }

        [ForeignKey("PageCategoryId")]
        public virtual CategoriesDTO Category { get; set; }
    }
}