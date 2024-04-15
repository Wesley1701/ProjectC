using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Emit;
using WebApplication1.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.VisualBasic;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public List<ProjectItem> GetProjects()
        {
            using (var context = new CompanyContext())
            {
                List<ProjectItem> displayProject = new List<ProjectItem>();
                var projects = from project in context.Project
                               select project;
                foreach (var x in projects)
                {
                    List<ServiceItem> items = new List<ServiceItem>();
                    using (var contextt = new CompanyContext())
                    {
                        var id = x.NumberID.Split(" ");
                        var services = contextt.Service.Where(s => s.ProjectID.Equals(id[0])).ToArray();
                        foreach (var sv in services)
                        {
                            List<HourTypeItem> hours = new List<HourTypeItem>();
                            using (var contexttt = new CompanyContext())
                            {
                                var hoursTypes = contexttt.HourType.Where(s => s.ServiceID.Equals(sv.ID)).ToArray();
                                foreach (var h in hoursTypes)
                                {
                                    hours.Add(new HourTypeItem()
                                    {
                                        Id = h.ID,
                                        Name = h.Name,
                                    });
                                }
                            }
                            items.Add(new ServiceItem
                            {
                                Name = sv.Name,
                                Id = sv.ID,
                                HourTypeItems = hours
                            });
                        }
                    }
                    displayProject.Add(new ProjectItem
                    {
                        Name = x.Name,
                        Id = x.NumberID,
                        ServiceItems = items
                    });
                }
                return displayProject;
            }
        }
        public IActionResult Index()
        {
            var displayProject = GetProjects();
            return View(displayProject);
        }
        [HttpPost]
        public IActionResult Index(LoginForm login)
        {
            var log = new Login();
            var bog = log.LoginFuncc(login.EmaiL, login.Psw);
            //var bog = log.LoginFuncc("dirk-jan@weareblis.com", "Hr2022$!S");
            System.Console.WriteLine(bog);
            if (bog == "0")
            {
                Login();
                return View("Login");
            }
            else
            {
                var displayProject = GetProjects();
                ViewBag.Message = bog;
                return View(displayProject);
            }
        }
        public IActionResult Account()
        {
            return View();
        }

        public IActionResult HourLog()
        {
            using (var context = new CompanyContext())
            {
                List<Hours> displayhours = new List<Hours>();
                var result =
                from hours in context.Hours
                orderby hours.ID
                select hours;
                foreach (var x in result)
                {
                    if (x.EndTime != "")
                    {
                        x.StartTime = DateTime.Parse(x.StartTime).ToString("HH:mm");
                        x.EndTime = DateTime.Parse(x.EndTime).ToString("HH:mm");
                        using (var contextt = new CompanyContext())
                        {
                            var r2 = contextt.Employee.Where(s => s.SSN.Equals(x.EmployeeID)).ToArray();
                            x.EmployeeID = r2[0].Name;
                        }
                        using (var contextt = new CompanyContext())
                        {
                            var r2 = contextt.Project.Where(s => s.NumberID.Equals(x.ProjectID)).ToArray();
                            x.ProjectID = r2[0].Name;
                        }
                        using (var contextt = new CompanyContext())
                        {
                            var r2 = contextt.Service.Where(s => s.ID.Equals(x.ServiceID)).ToArray();
                            x.ServiceID = r2[0].Name;
                        }
                        using (var contextt = new CompanyContext())
                        {
                            var r2 = contextt.HourType.Where(s => s.ID.Equals(x.HoursTypeID)).ToArray();
                            x.HoursTypeID = r2[0].Name;
                        }
                        displayhours.Add(x);
                    }
                }
                return View(displayhours);
            }
        }
        public IActionResult HourEdit()
        {
            using (var context = new CompanyContext())
            {
                List<ProjectItem> displayProject = new List<ProjectItem>();
                var projects = from project in context.Project
                               select project;
                foreach (var x in projects)
                {
                    List<ServiceItem> items = new List<ServiceItem>();
                    using (var contextt = new CompanyContext())
                    {
                        var id = x.NumberID.Split(" ");
                        var services = contextt.Service.Where(s => s.ProjectID.Equals(id[0])).ToArray();
                        foreach (var sv in services)
                        {
                            List<HourTypeItem> hours = new List<HourTypeItem>();
                            using (var contexttt = new CompanyContext())
                            {
                                var hoursTypes = contexttt.HourType.Where(s => s.ServiceID.Equals(sv.ID)).ToArray();
                                foreach (var h in hoursTypes)
                                {
                                    hours.Add(new HourTypeItem()
                                    {
                                        Id = h.ID,
                                        Name = h.Name,
                                    });
                                }
                            }
                            items.Add(new ServiceItem
                            {
                                Name = sv.Name,
                                Id = sv.ID,
                                HourTypeItems = hours
                            });
                        }
                    }
                    displayProject.Add(new ProjectItem
                    {
                        Name = x.Name,
                        Id = x.NumberID,
                        ServiceItems = items
                    });
                }
                return View(displayProject);
            }
        }
        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetStart(string startTime, string hourtypeID, string userID)
        {
            var projectID = "";
            var serID = "";
            using (var context = new CompanyContext())
            {
                var ht = context.HourType.Where(s => s.ID.Equals(hourtypeID)).ToArray();
                serID = ht[0].ServiceID;
                var s = context.Service.Where(s => s.ID.Equals(serID)).ToArray();
                var pID = s[0].ProjectID;
                var p = context.Project.Where(s => s.NumberID.Contains(pID)).ToArray();
                projectID = p[0].NumberID;
            }
            var hours = new InfoHours();
            hours.AddStartTime(DateTime.Parse(startTime), projectID, serID, hourtypeID, userID);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult SetEnd(string endTime, string projectID, string userID)
        {
            var hours = new InfoHours();
            hours.AddEndTime(DateTime.Parse(endTime), projectID, userID);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult GetStart(string UserID)
        {
            JsonResult RetVal = new JsonResult(new object());
            var start = "";
            try
            {
                using (var context = new CompanyContext())
                {
                    Hours[] hoursDetails = context.Hours.Where(s => s.EmployeeID == UserID).ToArray();
                    foreach (var hours in hoursDetails)
                    {
                        if (hours.EndTime == "")
                        {
                            var date = DateTime.Parse(hours.StartTime);
                            start = date.ToString("yyyy-MM-ddTHH:mm:ss");
                            start = start + "/"+ hours.HoursTypeID;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Response if there was an error
                RetVal = Json(new
                {
                    Status = "ERROR",
                    Message = ex.ToString(),
                    MyArray = new int[0]
                });
            }
            return Json(start);
        }

        [HttpPost]
        public IActionResult SendEdit(string EditID)
        {
            var start = "";
            using (var context = new CompanyContext())
            {
                Hours[] hour = context.Hours.Where(s => s.ID.ToString() == EditID).ToArray();
                start = hour[0].HoursTypeID;
                var date = DateTime.Parse(hour[0].StartTime);
                start = start + "/" + date.ToString("HH:mm");
                date = DateTime.Parse(hour[0].EndTime);
                start = start + "/" + date.ToString("HH:mm");
            }
            
            return Json(start);
        }
        [HttpPost]
        public IActionResult CheckEdit(string itemID, string projects,string services, string hourtypes, string sTime, string eTime)
        {
            if (hourtypes != null)
            {
                if (TimeOnly.Parse(sTime) < TimeOnly.Parse(eTime))
                {
                    using (var context = new CompanyContext())
                    {
                        Hours[] hour = context.Hours.Where(s => s.ID.ToString() == itemID).ToArray();
                        var eHour = hour[0];
                        var s = DateTime.Parse(eHour.StartTime);
                        sTime = s.ToString("dd/MM/yyyy") + " " + sTime + ":00";
                        eTime = s.ToString("dd/MM/yyyy") + " " + eTime + ":00";
                        eHour.StartTime = sTime;
                        eHour.EndTime = eTime;
                        var serID = "";
                        var projectID = "";
                        using (var contextt = new CompanyContext())
                        {
                            var ht = contextt.HourType.Where(s => s.ID.Equals(hourtypes)).ToArray();
                            serID = ht[0].ServiceID;
                            var sv = contextt.Service.Where(s => s.ID.Equals(serID)).ToArray();
                            var pID = sv[0].ProjectID;
                            var p = contextt.Project.Where(s => s.NumberID.Contains(pID)).ToArray();
                            projectID = p[0].NumberID;
                        }
                        eHour.ProjectID = projectID;
                        eHour.ServiceID = serID;
                        eHour.HoursTypeID = hourtypes;
                        context.SaveChanges();
                        HourLog();
                        return View("HourLog");
                    }
                }
                else
                {
                    ViewBag.Message = "Starttime has to be earlier then endtime. Please try again";
                    HourEdit();
                    return View("HourEdit");
                }
            }else
            {
                ViewBag.Message = "Please select a Project, Service and Hour Type";
                HourEdit();
                return View("HourEdit");
            }
            
            
        }

        [HttpPost]
        public IActionResult LoginFunc(string Email, string psw)
        {
            System.Console.WriteLine(Email + "bok");
            System.Console.WriteLine(psw + "dok");
            var log = new Login();
            var bog = log.LoginFuncc(Email, psw);
            System.Console.WriteLine(bog);
            return Json(bog);
        }
    }
}