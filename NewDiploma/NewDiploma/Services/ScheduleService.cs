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

        public List<Student> GetStudents()
        {
            return _repository.GetStudents();   
        }

		public List<Material> GetMaterials()
		{
			return _repository.GetMaterials();
		}

        public List<Present> GetPresents(int lesson)
        {
            var date = DateTime.Now.Date;
            return _repository.GetPresents(lesson, date);
        }

    }
}
