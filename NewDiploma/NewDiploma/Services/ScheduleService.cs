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
            var schedule = _repository.GetSchedule();

            foreach (var item in schedule)
            {
                item.DayOfWeek = (int)item.Date.DayOfWeek;
            }

            return schedule;
        }

        public List<Student> GetStudents()
        {
            return _repository.GetStudents();   
        }

    }
}
