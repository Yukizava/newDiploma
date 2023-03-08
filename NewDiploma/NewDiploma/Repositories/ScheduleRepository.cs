using NewDiploma.Models;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NewDiploma.Services;

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

        public List<Schedule> GetSchedule(DateTime dateFrom, DateTime dateTo, string userId)
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
                                                    JOIN [Lesson] ON [lesson_id] = [Lesson].id
													WHERE Schedule.[Date] >= @dateFrom
													AND Schedule.[Date] <= @dateTo 
													AND [User].[user_guid] = @userId",new {dateFrom = dateFrom, dateTo = dateTo, userId = userId}).ToList();

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
        public List<Present> GetPresents(int lesson, DateTime date, string userId)
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Present>(@"   SELECT Schedule.[date], Schedule.[lesson_id], [ScheduleStudent].[student_id],[User].[avatar_id] as 'Avatar', [User].[FIO] as 'FIO', [Group].name as 'Group', [User].[user_guid], [Schedule].teacher_id, Teacher_User.user_guid
                                                    FROM [Schedule]
                                                    JOIN [Group] ON [Schedule].group_id = [GROUP].id
                                                    JOIN [StudentGroup] ON [Group].id = [StudentGroup].[group_id]
                                                    JOIN [User] ON [StudentGroup].[user_id] = [User].[id]
                                                    LEFT JOIN [ScheduleStudent] ON [StudentGroup].[user_id] = [ScheduleStudent].[student_id] AND [ScheduleStudent].[schedule_id] = [Schedule].[id] 
													JOIN [User] AS Teacher_User ON [Schedule].[teacher_id] = [Teacher_User].id 
                                                    WHERE [Schedule].[date] = @date AND [Schedule].[lesson_id] = @lesson AND [Teacher_User].[user_guid] = @userId", new { lesson = lesson, date = date, userId = userId }).ToList();

                return result;
            }
        }
    }
}
