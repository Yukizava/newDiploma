namespace NewDiploma.Models
{
    public class Present
    {
        public int? AvatarId { get; set; }
        public int StudentId { get; set; }
        public string FIO { get; set; }
        public int? Attendance { get; set; }
        public int? Pass { get; set; }
        public string Group { get; set; }
        public string CourseName { get; set; }
        public int ScheduleId { get; set; }
    }
}
