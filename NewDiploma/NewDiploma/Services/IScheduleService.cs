using NewDiploma.Models;

namespace NewDiploma.Services
{
    public interface IScheduleService
    {
        List<Student> GetStudents(string groupName);
        List<Schedule> GetSchedule(DateTime dateNow, Data.Identity.ApplicationIdentityUser user);
        List<Material> GetCourses();

        List<Present> GetPresents(int lesson, Data.Identity.ApplicationIdentityUser user);
        List<Group> GetGroups();
        List<StudentAttendance> GetStudentAttendances(string groupName, string courseName);
        void SetAttendance(int studentId, int lessonId, int attendance);
    }
}
