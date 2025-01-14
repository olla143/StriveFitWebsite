namespace StriveFitWebsite.Models
{
    public class ScheduleForm
    {
        public DateTime Starttime { get; set; }

        public DateTime Endtime { get; set; }
        public decimal? Capacity { get; set; }

        public string? Goal { get; set; }

        public TimeSpan? Lectuerstime { get; set; }

        public string? Exercisroutine { get; set; }

        public string? Classtype { get; set; }
        public decimal Trainingid { get; set; }
    }
}
