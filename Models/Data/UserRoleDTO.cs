using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankApplication.Models.Data
{
    [Table("tblUserRoles")]
    public class UserRoleDTO
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual ClientsDTO ClientKey { get; set; }

        [ForeignKey("RoleId")]
        public virtual RoleDTO RoleClient { get; set; }
    }
}