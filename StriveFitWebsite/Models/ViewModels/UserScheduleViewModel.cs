namespace StriveFitWebsite.Models.ViewModels
{
    public class UserScheduleViewModel
    {
        public List<ScheduleViewModel> AvailableSchedules { get; set; } = new();
        public List<ScheduleViewModel> EnrolledSchedules { get; set; } = new();
    }
}
