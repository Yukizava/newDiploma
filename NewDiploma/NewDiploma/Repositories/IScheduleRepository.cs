﻿using Microsoft.VisualBasic.FileIO;
using NewDiploma.Models;
using System.ComponentModel.DataAnnotations;

namespace NewDiploma.Repositories
{
    public interface IScheduleRepository
    {
        List<Schedule> GetSchedule(DateTime dateFrom, DateTime dateTo, string userId);
        List<Student> GetStudents(string groupName);
        List<Material> GetCourses();
        List<Present> GetPresents(int lesson, DateTime date, string userId);
        List<Group> GetGroups();
        List<StudentAttendanceModel> GetStudentAttendances(string groupName, string courseName);
        void SetAttendance(int studentId, int lessonId, int attendance);

        void SetPass(int studentId, int lessonId, int pass);
        List<StudentAttendanceModel> GetStudentPasses(string groupName, string courseName);
        void CreateFile(string fileName, string dataType, byte[] data, int fileType, string userId, long length);
    }
}
