using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankApplication.Models.Data
{
    [Table("tblListEmployeeRole")]
    public class ListEmployeeRoleDTO
    {
        [Key, Column(Order = 0)]
        public int EmployeeId { get; set; }

        [Key, Column(Order = 1)]
        public int RoleEmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual EmployeesDTO EmployeeKey { get; set; }

        [ForeignKey("RoleEmployeeId")]
        public virtual EmployeeRoleDTO RoleEmployee { get; set; }
    }
}