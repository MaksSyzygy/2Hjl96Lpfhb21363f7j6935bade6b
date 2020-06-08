using BankApplication.Models.Data;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BankApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {
            if(User == null)
            {
                return;
            }

            string login = Context.User.Identity.Name;

            string[] roles = null;

            using(BankDB bankDB = new BankDB())
            {
                ClientsDTO clientsDTO = bankDB.Clients.FirstOrDefault(x => x.ClientLogin == login);
                EmployeesDTO employeesDTO = bankDB.Employees.FirstOrDefault(x => x.EmployeeLogin == login);

                if(clientsDTO != null)
                {
                    roles = bankDB.UserRoles.Where(x => x.UserId == clientsDTO.ClientId).Select(x => x.RoleClient.Name).ToArray();
                }
                else
                {
                    roles = bankDB.ListEmployeeRoles.Where(x => x.EmployeeId == employeesDTO.EmployeeId).Select(x => x.RoleEmployee.Name).ToArray();
                }
            }

            IIdentity clientIdentity = new GenericIdentity(login);
            IPrincipal newClientObject = new GenericPrincipal(clientIdentity, roles);

            Context.User = newClientObject;
        }
    }
}
