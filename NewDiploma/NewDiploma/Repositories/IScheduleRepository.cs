using NewDiploma.Models;

namespace NewDiploma.Repositories
{
    public interface IScheduleRepository
    {
        List<Schedule> GetSchedule();
        List<Student> GetStudents();
        
    }
}
