using BankApplication.Models.Data;
using BankApplication.Models.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using BankApplication.Models.ViewModels.Clients;

namespace BankApplication.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Admin/Employee
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult EmployeeQueryPartial(string sort, string filter)
        {
            List<EmployeeVM> employeeList = new List<EmployeeVM>();

            using (BankDB bankDB = new BankDB())
            {
                string SortQuery = string.IsNullOrEmpty(sort) ? "name_desc" : sort;

                switch (SortQuery)
                {
                    case "name_desc":
                        employeeList = bankDB.Employees.ToArray().OrderBy(x => x.EmployeeName).Select(x => new EmployeeVM(x)).ToList();
                        break;
                    case "AgeAsc":
                        employeeList = bankDB.Employees.ToArray().OrderBy(x => x.EmployeeAge).Select(x => new EmployeeVM(x)).ToList();
                        break;
                    case "AgeDesc":
                        employeeList = bankDB.Employees.ToArray().OrderByDescending(x => x.EmployeeAge).Select(x => new EmployeeVM(x)).ToList();
                        break;
                    case "SalaryAsc":
                        employeeList = bankDB.Employees.ToArray().OrderBy(x => x.EmployeeSalary).Select(x => new EmployeeVM(x)).ToList();
                        break;
                    case "SalaryDesc":
                        employeeList = bankDB.Employees.ToArray().OrderByDescending(x => x.EmployeeSalary).Select(x => new EmployeeVM(x)).ToList();
                        break;
                    case "Position":
                        employeeList = bankDB.Employees.ToArray().OrderBy(x => x.EmployeePosition).Select(x => new EmployeeVM(x)).ToList();
                        break;
                    case "ClientAsc":
                        employeeList = bankDB.Employees.ToArray().OrderBy(x => x.ClientsCount).Select(x => new EmployeeVM(x)).ToList();
                        break;
                    case "ClientDesc":
                        employeeList = bankDB.Employees.ToArray().OrderByDescending(x => x.ClientsCount).Select(x => new EmployeeVM(x)).ToList();
                        break;
                    default:
                        employeeList = bankDB.Employees.ToArray().OrderBy(x => x.EmployeeName).Select(x => new EmployeeVM(x)).ToList();
                        break;
                }

                if (filter != null)
                {
                    employeeList = employeeList.Where(x => x.Info().ToLower().Contains(filter.ToLower())).ToList();
                }
            }

            return PartialView("_EmployeeQueryPartial", employeeList);
        }

        //GET: Admin/Employee/CreateEmployee
        [HttpGet]
        public ActionResult CreateEmployee()
        {
            EmployeeVM employeeVM = new EmployeeVM();

            using(BankDB bankDB = new BankDB())
            {
                employeeVM.positionList = new SelectList(bankDB.EmployeeRoles.ToList(), "Id", "Name");
            }

            return View(employeeVM);
        }

        //POST: Admin/Employee/CreateEmployee
        [HttpPost]
        public ActionResult CreateEmployee(EmployeeVM model)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            if (!ModelState.IsValid)
            {
                using(BankDB bankDB = new BankDB())
                {
                    model.positionList = new SelectList(bankDB.EmployeeRoles.ToList(), "Id", "Name");

                    return View(model);
                }
            }

            using (BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = new EmployeesDTO();

                if (bankDB.Employees.Any(x => x.EmployeeLogin == model.EmployeeLogin))
                {
                    ModelState.AddModelError("loginExist", "Это имя занято, выберите другое");

                    return View(model);
                }

                employeesDTO.EmployeeName = textInfo.ToTitleCase(model.EmployeeName);
                employeesDTO.EmployeeSurname = textInfo.ToTitleCase(model.EmployeeSurname);
                employeesDTO.EmployeeAge = model.EmployeeAge;
                employeesDTO.EmployeeEmail = model.EmployeeEmail;
                employeesDTO.EmployeePhone = model.EmployeePhone;
                employeesDTO.EmployeeLogin = model.EmployeeLogin;
                employeesDTO.EmployeePassword = model.EmployeePassword;
                employeesDTO.EmployeePositionId = model.EmployeePositionId;

                EmployeeRoleDTO employeeRole = bankDB.EmployeeRoles.FirstOrDefault(x => x.Id == model.EmployeePositionId);

                employeesDTO.EmployeePosition = employeeRole.Name;
                employeesDTO.EmployeeSalary = model.EmployeeSalary;
                employeesDTO.ClientsCount = 0;

                bankDB.Employees.Add(employeesDTO);
                bankDB.SaveChanges();

                int id = employeesDTO.EmployeeId;
                int roleId = bankDB.EmployeeRoles.Where(x => x.Name == employeeRole.Name).Select(x => x.Id).First();

                ListEmployeeRoleDTO listRoleEmployee = new ListEmployeeRoleDTO()
                {
                    EmployeeId = id,
                    RoleEmployeeId = roleId
                };

                bankDB.ListEmployeeRoles.Add(listRoleEmployee);
                bankDB.SaveChanges();
            }

            TempData["OK"] = "Сотрудник добавлен";

            return RedirectToAction("Index");
        }

        //GET: Admin/Employee/EmployeeDetails/id
        [HttpGet]
        public ActionResult EmployeeDetails(int id)
        {
            EmployeeVM model;

            using (BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.Find(id);

                if (employeesDTO == null)
                {
                    return Content("Сотрудник не доступен");
                }

                model = new EmployeeVM(employeesDTO);
            }

            return View(model);
        }

        //GET: Admin/Employee/EditEmployee/id
        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            EmployeeProfileVM model;

            using (BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.Find(id);

                if (employeesDTO == null)
                {
                    return Content("Сотрудник не доступен");
                }

                model = new EmployeeProfileVM(employeesDTO);

                model.positionList = new SelectList(bankDB.EmployeeRoles.ToList(), "id", "Name");
            }

            return View(model);
        }

        //POST: Admin/Employee/EditEmployee
        [HttpPost]
        public ActionResult EditEmployee(EmployeeProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                using (BankDB bankDB = new BankDB())
                {
                    model.positionList = new SelectList(bankDB.EmployeeRoles.ToList(), "Id", "Name");

                    return View(model);
                }
            }

            if (!string.IsNullOrWhiteSpace(model.EmployeePassword))
            {
                if (!model.EmployeePassword.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("errorPass", "Пароли не совпадают");
                    model.EmployeePassword = "";
                    model.ConfirmPassword = "";

                    return View(model);
                }
            }

            using (BankDB bankDB = new BankDB())
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                int id = model.EmployeeId;
                string email = model.EmployeeEmail;
                string phone = model.EmployeePhone;
                string login = model.EmployeeLogin;

                EmployeesDTO employeesDTO = bankDB.Employees.Find(id);

                if (bankDB.Employees.Where(x => x.EmployeeId != id).Any(x => x.EmployeeEmail == email))
                {
                    ModelState.AddModelError("emailExist", $"Адрес {email} занят");

                    return View(model);
                }
                else if (bankDB.Employees.Where(x => x.EmployeeId != id).Any(x => x.EmployeePhone == phone))
                {
                    ModelState.AddModelError("phoneExist", $"Номер {phone} занят");

                    return View(model);
                }
                else if (bankDB.Employees.Where(x => x.EmployeeId != id).Any(x => x.EmployeeLogin == login))
                {
                    ModelState.AddModelError("loginExist", $"Логин {login} занят");

                    return View(model);
                }

                employeesDTO.EmployeeName = textInfo.ToTitleCase(model.EmployeeName);
                employeesDTO.EmployeeSurname = textInfo.ToTitleCase(model.EmployeeSurname);
                employeesDTO.EmployeeAge = model.EmployeeAge;
                employeesDTO.EmployeeEmail = email;
                employeesDTO.EmployeePhone = phone;
                employeesDTO.EmployeeLogin = model.EmployeeLogin;

                employeesDTO.EmployeePositionId = model.EmployeePositionId;
                employeesDTO.EmployeeSalary = model.EmployeeSalary;

                EmployeeRoleDTO employeeRole = bankDB.EmployeeRoles.FirstOrDefault(x => x.Id == model.EmployeePositionId);

                employeesDTO.EmployeePosition = employeeRole.Name;

                if (!string.IsNullOrWhiteSpace(model.EmployeePassword))
                {
                    employeesDTO.EmployeePassword = model.EmployeePassword;
                }

                bankDB.SaveChanges();
            }

            TempData["OK"] = "Профиль отредактирован";

            return RedirectToAction("Index");
        }

        //GET: Admin/Employee/DeleteEmployee/id
        [HttpGet]
        public ActionResult DeleteEmployee(int id)
        {
            using (BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.Find(id);

                bankDB.Employees.Remove(employeesDTO);

                bankDB.SaveChanges();
            }

            TempData["OK"] = "Клиент удален";

            return RedirectToAction("Index");
        }

        public ActionResult MyClients(int id)
        {
            EmployeeVM employeeVM = new EmployeeVM();
            List<ClientVM> clientsList = new List<ClientVM>();

            using(BankDB bankDB = new BankDB())
            {
                EmployeesDTO employeesDTO = bankDB.Employees.Find(id);

                employeeVM = new EmployeeVM(employeesDTO);

                clientsList = bankDB.Clients.ToArray().Where(x => x.EmployeeId == employeeVM.EmployeeId).Select(x => new ClientVM(x)).ToList();
            }

            return PartialView("_MyClients", clientsList);
        }
    }
}