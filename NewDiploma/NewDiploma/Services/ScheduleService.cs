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

        public List<Schedule> GetSchedule()
        {
            return _repository.GetSchedule();
        }

        public List<Student> GetStudents()
        {
            return _repository.GetStudents();   
        }

    }
}
