using BankApplication.Models.Data;
using BankApplication.Models.ViewModels.Clients;
using BankApplication.Models.ViewModels.Employees;
using BankApplication.Models.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankApplication.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            string login = User.Identity.Name;

            if (!string.IsNullOrEmpty(login))
            {
                return RedirectToAction("user-details");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("emptyField", "Введите данные");

                return View(loginVM);
            }

            bool isEmployee = false;
            bool isClient = false;

            using(BankDB bankDB = new BankDB())
            {
                if(bankDB.BlackLists.Any(x=>x.Login == loginVM.UserName))
                {
                    ModelState.AddModelError("ban", $"Ваш аккаунт заблокирован, причина - {bankDB.BlackLists.Select(x => x.Reason).First()}");

                    return View(loginVM);
                }
            }

            using(BankDB bankDB = new BankDB())
            {
                if(bankDB.Employees.Any(x=>x.EmployeeLogin == loginVM.UserName && x.EmployeePassword == loginVM.Password))
                {
                    isEmployee = true;
                }
                else if(bankDB.Clients.Any(x=>x.ClientLogin == loginVM.UserName && x.ClientPassword == loginVM.Password))
                {
                    isClient = true;
                }

                if (!isEmployee && !isClient)
                {
                    ModelState.AddModelError("entryError", "Ошибка логина или пароля");

                    return View(loginVM);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(loginVM.UserName, loginVM.RememberMy);

                    return Redirect(FormsAuthentication.GetRedirectUrl(loginVM.UserName, loginVM.RememberMy));
                }
            }
        }

        public ActionResult LoginPartial()
        {
            string login = User.Identity.Name;

            LoginPartialVM loginPartial;

            using (BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.FirstOrDefault(x => x.EmployeeLogin == login);
                ClientsDTO clientsDTO = bankDB.Clients.FirstOrDefault(x => x.ClientLogin == login);

                if(employeesDTO == null)
                {
                    loginPartial = new LoginPartialVM()
                    {
                        FirstName = clientsDTO.ClientName,
                        Surname = clientsDTO.ClientSurname
                    };
                }
                else
                {
                    loginPartial = new LoginPartialVM()
                    {
                        FirstName = employeesDTO.EmployeeName,
                        Surname = employeesDTO.EmployeeSurname
                    };
                }

                return PartialView("_LoginPartial", loginPartial);
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        [ActionName("user-details")]
        public ActionResult UserDetails()
        {
            string login = User.Identity.Name;

            using(BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.FirstOrDefault(x => x.EmployeeLogin == login);
                ClientsDTO clientsDTO = bankDB.Clients.FirstOrDefault(x => x.ClientLogin == login);

                if(employeesDTO == null)
                {
                    return RedirectToAction("ManagerDetails", "Employees");
                }
                else
                {
                    return RedirectToAction("ClientDetails", "Clients");
                }
            }
        }
    }
}