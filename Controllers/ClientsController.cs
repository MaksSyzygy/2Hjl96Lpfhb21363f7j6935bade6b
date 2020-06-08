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
    public class ClientsController : Controller
    {
        // GET: Clients
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("create-account")]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        [HttpPost]
        [ActionName("create-account")]
        public ActionResult CreateAccount(ClientVM clientVM)
        {
            Random random = new Random();

            if (!ModelState.IsValid)
            {
                return View("CreateAccount", clientVM);
            }

            using (BankDB bankDB = new BankDB())
            {
                if (bankDB.Clients.Any(x => x.ClientLogin == clientVM.ClientLogin))
                {
                    ModelState.AddModelError("loginExist", $"Логин {clientVM.ClientLogin} занят");

                    return View("CreateAccount", clientVM);
                }
                else if (bankDB.Clients.Any(x => x.ClientEmail == clientVM.ClientEmail))
                {
                    ModelState.AddModelError("emailExist", "Этот адрес занят");

                    return View("CreateAccount", clientVM);
                }
                else if (bankDB.Clients.Any(x => x.ClientPhone == clientVM.ClientPhone))
                {
                    ModelState.AddModelError("phoneExist", "Номер телефона уже используется");

                    return View("CreateAccount", clientVM);
                }

                Dictionary<int, string> Employee = new Dictionary<int, string>();

                foreach(var item in bankDB.Employees.Where(x => x.EmployeePosition != "Администратор"))
                {
                    Employee.Add(item.EmployeeId, $"{item.EmployeeName} {item.EmployeeSurname}");
                }

                int employeeId = Employee.ElementAt(random.Next(0, Employee.Count)).Key;
                string manager = Employee[employeeId];

                ClientsDTO clientsDTO = new ClientsDTO()
                {
                    ClientName = clientVM.ClientName,
                    ClientSurname = clientVM.ClientSurname,
                    ClientAge = clientVM.ClientAge,
                    ClientEmail = clientVM.ClientEmail,
                    ClientPhone = clientVM.ClientPhone,
                    ClientLogin = clientVM.ClientLogin,
                    ClientPassword = clientVM.ClientPassword,
                    EmployeeId = employeeId,
                    MyEmployee = manager
                };

                EmployeesDTO employeesDTO = bankDB.Employees.Find(employeeId);
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

            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public ActionResult EditClientProfile()
        {
            string login = User.Identity.Name;

            ClientProfileVM clientProfile;

            using (BankDB bankDB = new BankDB())
            {
                ClientsDTO clientsDTO = bankDB.Clients.FirstOrDefault(x => x.ClientLogin == login);

                clientProfile = new ClientProfileVM(clientsDTO);
            }

            return View(clientProfile);
        }

        [HttpPost]
        public ActionResult EditClientProfile(ClientProfileVM clientProfile)
        {
            bool loginIsChanged = false;

            if (!ModelState.IsValid)
            {
                return View("EditClientProfile", clientProfile);
            }

            if (!string.IsNullOrEmpty(clientProfile.ClientPassword))
            {
                if (!clientProfile.ClientPassword.Equals(clientProfile.ConfirmPassword))
                {
                    ModelState.AddModelError("errorPass", "пароли не совпадают");
                    clientProfile.ConfirmPassword = "";
                    clientProfile.ConfirmPassword = "";

                    return View("EditClientProfile", clientProfile);
                }
            }

            using (BankDB bankDB = new BankDB())
            {
                string login = User.Identity.Name;

                if (login != clientProfile.ClientLogin)
                {
                    login = clientProfile.ClientLogin;
                    loginIsChanged = true;
                }

                if (bankDB.Clients.Where(x => x.ClientId != clientProfile.ClientId).Any(x => x.ClientLogin == login))
                {
                    ModelState.AddModelError("loginMatch", $"Логин {clientProfile.ClientLogin} занят");

                    return View("EditClientProfile", clientProfile);
                }
                else if (bankDB.Clients.Where(x => x.ClientId != clientProfile.ClientId).Any(x => x.ClientEmail == clientProfile.ClientEmail))
                {
                    ModelState.AddModelError("emailMatch", "Email занят");

                    return View("EditClientProfile", clientProfile);
                }
                else if (bankDB.Clients.Where(x => x.ClientId != clientProfile.ClientId).Any(x => x.ClientPhone == clientProfile.ClientPhone))
                {
                    ModelState.AddModelError("phoneMatch", "Телефон занят");

                    return View("EditClientProfile", clientProfile);
                }

                ClientsDTO clientsDTO = bankDB.Clients.Find(clientProfile.ClientId);

                clientsDTO.ClientName = clientProfile.ClientName;
                clientsDTO.ClientSurname = clientProfile.ClientSurname;
                clientsDTO.ClientAge = clientProfile.ClientAge;
                clientsDTO.ClientEmail = clientProfile.ClientEmail;
                clientsDTO.ClientPhone = clientProfile.ClientPhone;
                clientsDTO.ClientLogin = clientProfile.ClientLogin;

                if (!string.IsNullOrEmpty(clientProfile.ClientPassword))
                {
                    clientsDTO.ClientPassword = clientProfile.ClientPassword;
                }

                bankDB.SaveChanges();
            }

            TempData["OK"] = "Данные обновлены";

            if (!loginIsChanged)
            {
                return View("EditClientProfile", clientProfile);
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }

        [HttpGet]
        [ActionName("client-details")]
        public ActionResult ClientDetails()
        {
            string login = User.Identity.Name;

            ClientVM clientVM;

            using (BankDB bankDB = new BankDB())
            {
                ClientsDTO clientsDTO = bankDB.Clients.FirstOrDefault(x => x.ClientLogin == login);

                clientVM = new ClientVM(clientsDTO);
            }

            return View("ClientDetails", clientVM);
        }

        public ActionResult Home()
        {
            return RedirectToAction("Index", "Pages");
        }
    }
}