using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BankApplication.Models.Data
{
    public class BankDB : DbContext
    {
        public DbSet<PagesDTO> Pages { get; set; }
        public DbSet<CategoriesDTO> Categories { get; set; }
        public DbSet<EmployeesDTO> Employees { get; set; }
        public DbSet<ClientsDTO> Clients { get; set; }
        public DbSet<RoleDTO> Roles { get; set; }
        public DbSet<UserRoleDTO> UserRoles { get; set; }
        public DbSet<EmployeeRoleDTO> EmployeeRoles { get; set; }
        public DbSet<ListEmployeeRoleDTO> ListEmployeeRoles { get; set; }
        public DbSet<BlackListDTO> BlackLists { get; set; }
        public DbSet<CurrencyDTO> Currency { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(18, 4));
        }
    }
}