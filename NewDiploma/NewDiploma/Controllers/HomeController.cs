using NewDiploma.Models;
using NewDiploma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using NewDiploma.Data.Identity;
using System.Data;

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

        public IActionResult Schedule([FromQuery] string date)
        {
            var dateNow = DateTime.Now.Date;
            if (date != null)
            {
                dateNow = DateTime.Parse(date);
            }
            var user = _usermanager.GetUserAsync(User).Result;
            var schedule = _service.GetSchedule(dateNow, user);

            return View(schedule);
        }

        public IActionResult Presents([FromQuery] int lesson)
        {
            var dateNow = DateTime.Now.Date;
            var user = _usermanager.GetUserAsync(User).Result;
            var getLessons = _service.GetSchedule(dateNow, user);
            var presents = _service.GetPresents(lesson, user);
            var result = new { Presents = presents, Lessons = getLessons};
            return View(result);
        }

        public IActionResult Students([FromQuery] string groupName)
        {
            var groups = _service.GetGroups();
            var students = _service.GetStudents(groupName);
            var result = new StudentList { Groups = groups, Students = students };
            return View(result);
        }

        public IActionResult GroupsPage([FromQuery] string groupName, [FromQuery] string courseName)
        {
            var groups = _service.GetGroups();
            var courses = _service.GetCourses();
            var attendances = _service.GetStudentAttendances(groupName, courseName);
            var result = new GroupPageList { Groups = groups , Attendances = attendances, Courses = courses};
            return View(result);
        }

        public IActionResult Materials([FromQuery] string courseName)
        {
            var courses = _service.GetCourses();
            return View(courses);
        }

        public IActionResult Notification()
        {
            return View();
        }

        public IActionResult SetAttendance([FromQuery] int studentId, [FromQuery] int scheduleId, [FromQuery] int attendance, [FromQuery] int lessonId)
        {
            _service.SetAttendance(studentId, scheduleId, attendance);
            return RedirectToAction("Presents", new { lesson = lessonId });
        }

        public IActionResult SetPass([FromQuery] int studentId, [FromQuery] int scheduleId, [FromQuery] int pass, [FromQuery] int lessonId)
        {
            _service.SetPass(studentId, scheduleId, pass);
            return RedirectToAction("Presents", new { lesson = lessonId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}