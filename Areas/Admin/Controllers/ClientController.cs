using BankApplication.Models.Data;
using BankApplication.Models.ViewModels.Clients;
using BankApplication.Models.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankApplication.Areas.Admin.Controllers
{
    public class ClientController : Controller
    {
        // GET: Admin/Client
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClientsQueryPartial(string sort, string filter)
        {
            List<ClientVM> clientsList = new List<ClientVM>();

            using (BankDB bankDB = new BankDB())
            {
                string SortQuery = string.IsNullOrEmpty(sort) ? "name_desc" : sort;

                switch (SortQuery)
                {
                    case "name_desc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderBy(x => x.ClientName).Select(x => new ClientVM(x)).ToList();
                        break;
                    case "AgeAsc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderBy(x => x.ClientAge).Select(x => new ClientVM(x)).ToList();
                        break;
                    case "AgeDesc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderByDescending(x => x.ClientAge).Select(x => new ClientVM(x)).ToList();
                        break;
                    case "BalanceAsc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderBy(x => x.Balance).Select(x => new ClientVM(x)).ToList();
                        break;
                    case "BalanceDesc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderByDescending(x => x.Balance).Select(x => new ClientVM(x)).ToList();
                        break;
                    case "CreditAsc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderBy(x => x.Credit).Select(x => new ClientVM(x)).ToList();
                        break;
                    case "CreditDesc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderByDescending(x => x.Credit).Select(x => new ClientVM(x)).ToList();
                        break;
                    case "DepositAsc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderBy(x => x.Deposit).Select(x => new ClientVM(x)).ToList();
                        break;
                    case "DepositDesc":
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderByDescending(x => x.Deposit).Select(x => new ClientVM(x)).ToList();
                        break;
                    default:
                        clientsList = bankDB.Clients.ToArray().Where(x => x.BanStatus == false).OrderBy(x => x.ClientName).Select(x => new ClientVM(x)).ToList();
                        break;
                }

                if (filter != null)
                {
                    clientsList = clientsList.Where(x => x.SearchInfo().ToLower().Contains(filter.ToLower())).ToList();
                }
            }

            return PartialView("_ClientsQueryPartial", clientsList);
        }

        [HttpGet]
        public ActionResult EditClient(int id)
        {
            ClientProfileVM clientVM;

            using(BankDB bankDB = new BankDB())
            {
                ClientsDTO clientsDTO = bankDB.Clients.Find(id);

                if(clientsDTO == null)
                {
                    return Content("Не доступно");
                }

                clientVM = new ClientProfileVM(clientsDTO);

                clientVM.employeeList = new SelectList(bankDB.Employees.ToList(), "EmployeeId", "EmployeeSurname");
            }

            return View(clientVM);
        }

        [HttpPost]
        public ActionResult EditClient(ClientProfileVM clientVM)
        {
            int id = clientVM.ClientId;

            using(BankDB bankDB = new BankDB())
            {
                clientVM.employeeList = new SelectList(bankDB.Employees.ToList(), "EmployeeId", "EmployeeSurname");
            }

            if (!ModelState.IsValid)
            {
                return View(clientVM);
            }

            using(BankDB bankDB = new BankDB())
            {
                ClientsDTO clientsDTO = bankDB.Clients.Find(id);

                if(clientsDTO.EmployeeId != clientVM.EmployeeId)
                {
                    EmployeesDTO oldEmployee = bankDB.Employees.Where(x => x.EmployeeId == clientsDTO.EmployeeId).First();
                    oldEmployee.ClientsCount -= 1;

                    clientsDTO.EmployeeId = clientVM.EmployeeId;
                    clientsDTO.Balance = clientVM.Balance;

                    EmployeesDTO newEmployee = bankDB.Employees.FirstOrDefault(x => x.EmployeeId == clientVM.EmployeeId);
                    newEmployee.ClientsCount += 1;
                    clientsDTO.MyEmployee = $"{newEmployee.EmployeeName} {newEmployee.EmployeeSurname}";

                    bankDB.SaveChanges();

                    TempData["OK"] = "Информация изменена";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult BlockClient(int idClient, string reason)
        {
            using(BankDB bankDB = new BankDB())
            {
                ClientsDTO clientsDTO = bankDB.Clients.FirstOrDefault(x=>x.ClientId == idClient);

                BlackListDTO blackListDTO = new BlackListDTO();

                blackListDTO.ClientId = clientsDTO.ClientId;
                blackListDTO.Login = clientsDTO.ClientLogin;
                blackListDTO.Name = clientsDTO.ClientName;
                blackListDTO.Surname = clientsDTO.ClientSurname;
                blackListDTO.Email = clientsDTO.ClientEmail;
                blackListDTO.Phone = clientsDTO.ClientPhone;
                blackListDTO.Reason = reason;
                clientsDTO.BanStatus = true;

                bankDB.BlackLists.Add(blackListDTO);
                bankDB.SaveChanges();
            }

            TempData["OK"] = "Клиент занесен в черный список";

            return RedirectToAction("Index");
        }

        public ActionResult BlackList()
        {
            List<BlackListVM> blackList = new List<BlackListVM>();

            using(BankDB bankDB = new BankDB())
            {
                blackList = bankDB.BlackLists.ToArray().OrderBy(x => x.Id).Select(x => new BlackListVM(x)).ToList();
            }

            return View(blackList);
        }

        public ActionResult Unblock(int id, int clientId)
        {
            using(BankDB bankDB = new BankDB())
            {
                BlackListDTO blackList = bankDB.BlackLists.Find(id);
                ClientsDTO clientsDTO = bankDB.Clients.Find(clientId);

                clientsDTO.BanStatus = false;

                bankDB.BlackLists.Remove(blackList);
                bankDB.SaveChanges();
            }

            TempData["OK"] = "Клиент восстановлен!";

            return RedirectToAction("Index");
        }
    }
}