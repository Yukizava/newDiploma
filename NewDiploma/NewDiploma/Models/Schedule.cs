namespace NewDiploma.Models
{
    public class Schedule
    {
        public string Course { get; set; }

        public string Group { get; set; }

        public TimeSpan Time { get; set; }

        public string Teacher { get; set; }

        public int Type { get; set; }
    }
}
