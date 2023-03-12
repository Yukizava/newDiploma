using NewDiploma.Models;
using NewDiploma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using NewDiploma.Data.Identity;

namespace NewDiploma.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScheduleService _service;

        private readonly UserManager<ApplicationIdentityUser> _usermanager;

        public HomeController(IScheduleService service, UserManager<ApplicationIdentityUser> userManagerService)
        {
            _service = service;
            _usermanager = userManagerService;
        }

        public IActionResult Index()
        {
            

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult Schedule([FromQuery]string date)
        {
            var dateNow = DateTime.Now;
            if (date != null)
            {
                dateNow = DateTime.Parse(date);
            }
            var user = _usermanager.GetUserAsync(User).Result;
            var schedule = _service.GetSchedule(dateNow, user);
            
            return View(schedule);
        }

        public IActionResult Presents([FromQuery]int lesson)
        {
            var user = _usermanager.GetUserAsync(User).Result; 
            var presents = _service.GetPresents(lesson, user);
            return View(presents);
        }

        public IActionResult Students([FromQuery] string groupName)
        {

            var students = _service.GetStudents(groupName);
            return View(students);
        }

        public IActionResult GroupsPage()
        {
            return View();
        }

        public IActionResult Materials()
        {
            var materials = _service.GetMaterials();
            return View(materials);
        }

        public IActionResult Notification()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}