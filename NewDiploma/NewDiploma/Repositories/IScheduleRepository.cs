using NewDiploma.Models;

namespace NewDiploma.Repositories
{
    public interface IScheduleRepository
    {
        List<Schedule> GetSchedule(DateTime dateFrom, DateTime dateTo, string userId);
        List<Student> GetStudents(string groupName);
        List<Material> GetCourses();
        List<Present> GetPresents(int lesson, DateTime date, string userId);
        List<Group> GetGroups();
        List<StudentAttendance> GetStudentAttendances(string groupName, string courseName);
    }
}
