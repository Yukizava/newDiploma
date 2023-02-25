using NewDiploma.Models;

namespace NewDiploma.Services
{
    public interface IScheduleService
    {
        List<Student> GetStudents();
        List<Schedule> GetSchedule();
        List<Material> GetMaterials();
    }
}
