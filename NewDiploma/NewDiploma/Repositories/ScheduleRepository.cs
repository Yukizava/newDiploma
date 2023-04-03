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
        public List<Student> GetStudents(string groupName)
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Student>(@"   SELECT FIO, [GROUP].[name] as 'GroupName', [GROUP].start_year as 'StartYear'
                                                    FROM StudentGroup 
                                                    JOIN [User] ON [user_id] = [USER].id 
                                                    JOIN [Group] ON [group_id] = [GROUP].id
													WHERE [Group].[name] = @groupName", new { groupName = groupName }).ToList();

                return result;
            }
        }

        public List<Group> GetGroups()
        {
            using (IDbConnection db = Connection)
            {
                var result = db.Query<Group>(@"SELECT [GROUP].[name] as 'GroupName', [GROUP].start_year as 'StartYear', [GROUP].id as 'Id'
                                                    FROM [Group] ").ToList();

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
													AND [User].[user_guid] = @userId", new { dateFrom = dateFrom, dateTo = dateTo, userId = userId }).ToList();

                return result;
            }
        }
        public List<Material> GetCourses()
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
                var result = db.Query<Present>(@"   SELECT [StudentGroup].user_id as 'StudentId', Schedule.[date], Schedule.[id] as 'ScheduleId', Course.[name] as 'CourseName', [ScheduleStudent].[student_id], [ScheduleStudent].[attendance], [ScheduleStudent].[pass],[User].[avatar_id] as 'Avatar', [User].[FIO] as 'FIO', [Group].name as 'Group', [User].[user_guid], [Schedule].teacher_id, Teacher_User.user_guid
                                                    FROM [Schedule]
                                                    JOIN [Course] ON [course_id] = [Course].id
                                                    JOIN [Group] ON [Schedule].group_id = [GROUP].id
                                                    JOIN [StudentGroup] ON [Group].id = [StudentGroup].[group_id]
                                                    JOIN [User] ON [StudentGroup].[user_id] = [User].[id]
                                                    LEFT JOIN [ScheduleStudent] ON [StudentGroup].[user_id] = [ScheduleStudent].[student_id] AND [ScheduleStudent].[schedule_id] = [Schedule].[id] 
													JOIN [User] AS Teacher_User ON [Schedule].[teacher_id] = [Teacher_User].id 
                                                    WHERE [Schedule].[date] = @date AND [Schedule].[lesson_id] = @lesson AND [Teacher_User].[user_guid] = @userId", new { lesson = lesson, date = date, userId = userId }).ToList();

                return result;
            }
        }

        public List<StudentAttendanceModel> GetStudentAttendances(string groupName, string courseName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                groupName = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(courseName))
            {
                courseName = string.Empty;
            }
            using (IDbConnection db = Connection)
            {
                var result = db.Query<StudentAttendanceModel>(@" SELECT CourseName, FIO, LessonType, StudentAttendance, AttendanceTotal, CAST(StudentAttendance as DECIMAL)/CAST(AttendanceTotal as DECIMAL) * 100 as AttendancePercent
                                                            FROM 
                                                            (SELECT Course.[name] as 'CourseName', [User].[FIO] as 'FIO', [Schedule].type as 'LessonType', COUNT(*) as StudentAttendance, 
	                                                            (
	                                                            SELECT COUNT(*)
	                                                            FROM [Schedule] as Schedule2
	                                                            JOIN [Course] as Course2 ON [Schedule2].[course_id] = [Course2].id
	                                                            WHERE [Schedule2].[type] = [Schedule].[type] AND [Course2].[name] = [Course].[name] AND [Schedule2].group_id = [Schedule].group_id
	                                                            ) as AttendanceTotal
                                                            FROM [Schedule]
                                                            JOIN [Course] ON [Schedule].[course_id] = [Course].id
                                                            JOIN [Group] ON [Schedule].group_id = [GROUP].id
                                                            JOIN [StudentGroup] ON [Group].id = [StudentGroup].[group_id]
                                                            JOIN [User] ON [StudentGroup].[user_id] = [User].[id]
                                                            LEFT JOIN [ScheduleStudent] ON [StudentGroup].[user_id] = [ScheduleStudent].[student_id] AND [ScheduleStudent].[schedule_id] = [Schedule].[id]		
                                                            WHERE [ScheduleStudent].attendance IN (1,2) AND [Group].name LIKE '%'+@groupName+'%' AND [Course].name LIKE '%'+@courseName+'%'
                                                            GROUP BY [Course].[name],[User].[FIO], [Schedule].[type], [Schedule].[group_id]
                                                            ) as Result", new { groupName = groupName, courseName = courseName }).ToList();

                return result;
            }
        }

        public void SetAttendance(int studentId, int scheduleId, int attendance)
        {
            using (IDbConnection db = Connection)
            {
                db.Query(@"IF NOT EXISTS(SELECT * FROM ScheduleStudent WHERE schedule_id = @scheduleId AND student_id = @studentId)
	                                BEGIN
		                                INSERT INTO ScheduleStudent(schedule_id,student_id,attendance,mark,pass)
		                                VALUES (@scheduleId,@studentId,@attendance,NULL,NULL)
	                                END
                                ELSE
	                                BEGIN
		                                UPDATE ScheduleStudent 
		                                SET attendance = @attendance
		                                WHERE schedule_id = @scheduleId AND student_id = @studentId
	                                END", new { scheduleId = scheduleId, studentId = studentId, attendance = attendance});
            };
        }

        public void SetPass(int studentId, int scheduleId, int pass)
        {
            using (IDbConnection db = Connection)
            {
                db.Query(@"IF NOT EXISTS(SELECT * FROM ScheduleStudent WHERE schedule_id = @scheduleId AND student_id = @studentId)
	                                BEGIN
		                                INSERT INTO ScheduleStudent(schedule_id,student_id,attendance,mark,pass)
		                                VALUES (@scheduleId,@studentId,NULL,NULL,@pass)
	                                END
                                ELSE
	                                BEGIN
		                                UPDATE ScheduleStudent 
		                                SET pass = @pass
		                                WHERE schedule_id = @scheduleId AND student_id = @studentId
	                                END", new { scheduleId = scheduleId, studentId = studentId, pass = pass });
            };
        }
    }
}
