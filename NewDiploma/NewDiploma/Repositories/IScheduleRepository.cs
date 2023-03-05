using NewDiploma.Models;

namespace NewDiploma.Repositories
{
    public interface IScheduleRepository
    {
        List<Schedule> GetSchedule(DateTime dateFrom, DateTime dateTo, string userId);
        List<Student> GetStudents();
        List<Material> GetMaterials();
        List<Present> GetPresents();
        
    }
}
