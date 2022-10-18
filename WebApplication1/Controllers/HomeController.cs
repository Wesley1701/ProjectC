using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public IActionResult Index()
        {
            TimeInfo C = new TimeInfo();
            C.End = "";
            C.Start = "";
            return View(C);
        }
        public IActionResult Index(TimeInfo T)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(TimeInfo T)
        {
            return View();
        }
        public IActionResult Login()
        {
            TimeInfo C = new TimeInfo();
            C.End = "End time";
            C.Start = "Start time";
            return View(C);
        }
        [HttpPost]
        public IActionResult StartTimer()
        {
            TimeInfo T = new TimeInfo();
            T.Start = DateTime.Now.ToString("h:mm tt");
            T.End = "";
            return View("Index", T);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}