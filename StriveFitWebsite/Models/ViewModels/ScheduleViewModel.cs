namespace StriveFitWebsite.Models.ViewModels
{
    public class ScheduleViewModel
    {
        public decimal ScheduleId { get; set; }
        public string TrainerName { get; set; }       
        public DateTime Starttime { get; set; }
        public DateTime Endtime { get; set; }
        public string Classtype { get; set; }

        public string Exercisroutine { get; set; }
        
    }
}
