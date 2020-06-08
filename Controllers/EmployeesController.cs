using BankApplication.Models.Data;
using BankApplication.Models.ViewModels.Clients;
using BankApplication.Models.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankApplication.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("manager-details")]
        public ActionResult ManagerDetails()
        {
            string login = User.Identity.Name;

            EmployeeVM employeeVM;

            using(BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.FirstOrDefault(x => x.EmployeeLogin == login);

                employeeVM = new EmployeeVM(employeesDTO);
            }

            return View("ManagerDetails", employeeVM);
        }

        public ActionResult MyClientsPartial()
        {
            string login = User.Identity.Name;

            EmployeeVM employeeVM;
            List<ClientVM> clientsList = new List<ClientVM>();

            using(BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.FirstOrDefault(x => x.EmployeeLogin == login);

                employeeVM = new EmployeeVM(employeesDTO);

                clientsList = bankDB.Clients.ToArray().Where(x => x.EmployeeId == employeeVM.EmployeeId).Select(x => new ClientVM(x)).ToList();
            }

            return PartialView("_MyClientsPartial", clientsList);
        }

        [HttpGet]
        [ActionName("create-account-by-manager")]
        public ActionResult CreateEmployeeClient()
        {
            return View("CreateEmployeeClient");
        }

        [HttpPost]
        [ActionName("create-account-by-manager")]
        public ActionResult CreateEmployeeClient(ClientVM clientVM)
        {
            string login = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return View("CreateEmployeeClient", clientVM);
            }

            using(BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.FirstOrDefault(x => x.EmployeeLogin == login);

                if (bankDB.Clients.Any(x => x.ClientLogin == clientVM.ClientLogin))
                {
                    ModelState.AddModelError("loginExist", $"Логин {clientVM.ClientLogin} занят");

                    return View("CreateEmployeeClient", clientVM);
                }
                else if (bankDB.Clients.Any(x => x.ClientEmail == clientVM.ClientEmail))
                {
                    ModelState.AddModelError("emailExist", "Этот адрес занят");

                    return View("CreateEmployeeClient", clientVM);
                }
                else if (bankDB.Clients.Any(x => x.ClientPhone == clientVM.ClientPhone))
                {
                    ModelState.AddModelError("phoneExist", "Номер телефона уже используется");

                    return View("CreateEmployeeClient", clientVM);
                }

                int employeeId = employeesDTO.EmployeeId;
                string manager = $"{employeesDTO.EmployeeName} {employeesDTO.EmployeeSurname}";

                ClientsDTO clientsDTO = new ClientsDTO()
                {
                    ClientName = clientVM.ClientName,
                    ClientSurname = clientVM.ClientSurname,
                    ClientAge = clientVM.ClientAge,
                    ClientEmail = clientVM.ClientEmail,
                    ClientPhone = clientVM.ClientPhone,
                    Balance = clientVM.Balance,
                    Credit = clientVM.Credit,
                    Deposit = clientVM.Deposit,
                    ClientLogin = clientVM.ClientLogin,
                    ClientPassword = clientVM.ClientPassword,
                    EmployeeId = employeeId,
                    MyEmployee = manager
                };

                employeesDTO.ClientsCount += 1;

                bankDB.Clients.Add(clientsDTO);
                bankDB.SaveChanges();

                int id = clientsDTO.ClientId;
                int role = clientsDTO.Balance < 50000 ? 1 : clientsDTO.Balance >= 50000 ? 2 : clientsDTO.Balance > 200000 ? 3 : 3;

                UserRoleDTO userRole = new UserRoleDTO()
                {
                    UserId = id,
                    RoleId = role
                };

                bankDB.UserRoles.Add(userRole);
                bankDB.SaveChanges();
            }

            TempData["OK"] = "Аккаунт создан";

            return RedirectToAction("manager-details", "Employees");
        }

        public ActionResult Home()
        {
            return RedirectToAction("Index", "Pages");
        }
    }
}