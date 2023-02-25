using NewDiploma.Models;
using NewDiploma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NewDiploma.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScheduleService _service;

        public HomeController(IScheduleService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var model = _service.GetStudents();
            
            return View(model);
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

        public IActionResult Schedule()
        {
            var schedule = _service.GetSchedule();
            return View(schedule);
        }

        public IActionResult Presents()
        {
            return View();
        }

        public IActionResult Students()
        {
            var students = _service.GetStudents();
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