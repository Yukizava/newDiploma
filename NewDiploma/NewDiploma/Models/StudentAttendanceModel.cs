namespace NewDiploma.Models
{
    public class StudentAttendanceModel
    {
        public string FIO { get; set; }
        public int StudentAttendance { get; set; }
        public double AttendancePercent { get; set; }
        public int AttendanceTotal { get; set; }
        public int StudentPass { get; set; }
        public double PassPercent { get; set; }
        public int PassTotal { get; set; }
        public string Group { get; set; }
        public string CourseName { get; set; }
        public LessonType LessonType { get; set; }
    }
}
