using NewDiploma.Models;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace NewDiploma.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IConfiguration _config;
        public ScheduleRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DiplomaContext"));
            }
        }
        public List<Student> GetStudents()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Student>(@"   SELECT FIO, [GROUP].name as 'GroupName', [GROUP].start_year as 'StartYear'
                                                    FROM StudentGroup 
                                                    JOIN [User] ON [user_id] = [USER].id 
                                                    JOIN [Group] ON [group_id] = [GROUP].id").ToList();

                return result;
            }
        }

        public List<Schedule> GetSchedule()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Schedule>(@"  SELECT [Course].[name] as 'Course', 
                                                           [GROUP].[name] as 'Group',
                                                           [User].FIO as 'Teacher',
                                                           [Lesson].time as 'Time',
                                                           [Lesson].id as 'LessonNumber',
                                                           Schedule.[type] as 'Type',
                                                           Schedule.[Date] as 'Date',
                                                           Schedule.Room as 'Room'
                                                    FROM Schedule
                                                    JOIN [Group] ON [group_id] = [GROUP].id
                                                    JOIN [Course] ON [course_id] = [Course].id
                                                    JOIN [User] ON [teacher_id] = [User].id
                                                    JOIN [Lesson] ON [lesson_id] = [Lesson].id").ToList();

                return result;
            }
        }
        public List<Material> GetMaterials()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Material>(@"  SELECT Course.[name] as 'CourseName'
                                                    FROM Course").ToList();

                return result;
            }
        }
        public List<Present> GetPresents()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Present>(@"   SELECT [User].[FIO] as 'FIO',
                                                    [User].[avatar_id] as 'Avatar'
                                                    FROM [User]").ToList();

                return result;
            }
        }
    }
}
