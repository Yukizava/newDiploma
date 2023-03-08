using NewDiploma.Models;

namespace NewDiploma.Services
{
    public interface IScheduleService
    {
        List<Student> GetStudents();
        List<Schedule> GetSchedule(DateTime dateNow, Data.Identity.ApplicationIdentityUser user);
        List<Material> GetMaterials();

        List<Present> GetPresents(int lesson, Data.Identity.ApplicationIdentityUser user);
    }
}
