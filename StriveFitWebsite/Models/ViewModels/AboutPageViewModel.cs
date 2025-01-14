namespace StriveFitWebsite.Models.ViewModels
{
    public class AboutPageViewModel
    {
        public List<Testimonial>? Testimonials { get; set; }
        public List<Aboutuspage>? AboutusPage { get; set; }
        public List<TrainerViewModel>? Trainers { get; set; }
    }

    public class TrainerViewModel
    {
        public string? Name { get; set; }
        public string? ImagePath { get; set; }
    }
}
