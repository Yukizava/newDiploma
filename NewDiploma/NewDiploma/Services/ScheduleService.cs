using NewDiploma.Models;
using NewDiploma.Repositories;

namespace NewDiploma.Services
{
    public class ScheduleService : IScheduleService
    {
        private IScheduleRepository _repository;

        public ScheduleService(IScheduleRepository repository) 
        { 
            _repository = repository;
        }

        public List<Schedule> GetSchedule(DateTime dateNow, Data.Identity.ApplicationIdentityUser user)
        {
            var dayOfWeek = (int)dateNow.DayOfWeek;
            var dateFrom = dateNow.AddDays(-dayOfWeek + (int)DayOfWeek.Monday);
            var dateTo = dateNow.AddDays(-dayOfWeek + 7);
            var userId = user.Id; 
            if (dayOfWeek == 0)
            {
                dateFrom = dateNow.AddDays(-7);
                dateTo = dateNow;
            } 

            var schedule = _repository.GetSchedule(dateFrom, dateTo, userId);

            foreach (var item in schedule)
            {
                item.DayOfWeek = (int)item.Date.DayOfWeek;
            }

            return schedule;
        }

        public List<Student> GetStudents(string groupName)
        {
            return _repository.GetStudents(groupName);   
        }

		public List<Material> GetCourses()
		{
			return _repository.GetCourses();
		}

        public List<Present> GetPresents(int lesson, Data.Identity.ApplicationIdentityUser user)
        {
            var date = DateTime.Now.Date;
            var userId = user.Id;

            var presents = _repository.GetPresents(lesson, date, userId);

            return presents;
        }

        public List<Group> GetGroups()
        {
            return _repository.GetGroups();
        }

        public List<StudentAttendanceModel> GetStudentAttendances(string groupName, string courseName)
        {
            var attendances = _repository.GetStudentAttendances(groupName, courseName);
            var passes = _repository.GetStudentPasses(groupName, courseName);
            foreach (var item in attendances)
            {
                var pass = passes.FirstOrDefault(x => x.Group == item.Group && x.CourseName == item.CourseName && x.FIO == item.FIO);
                if (pass != null)
                {
                    item.PassPercent = pass.PassPercent;
                    item.PassTotal = pass.PassTotal;
                    item.StudentPass = pass.StudentPass;
                }
            }

            return attendances;
        }

        public void SetAttendance(int studentId, int lessonId, int attendance)
        {
            _repository.SetAttendance(studentId, lessonId, attendance);
        }

        public void SetPass(int studentId, int lessonId, int pass)
        {
            _repository.SetPass(studentId, lessonId, pass);
        }
    }
}
