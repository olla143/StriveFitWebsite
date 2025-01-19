using System.ComponentModel.DataAnnotations;

namespace StriveFitWebsite.Models.ViewModels
{
    public class TestimonialsViewModel
    {
        public decimal Testimonialid { get; set; }

        [Required]
        public string Username { get; set; } 

        public string? Content { get; set; }

        public string? Status { get; set; }

        public DateTime? Submitteddate { get; set; }

        public decimal? Rating { get; set; }
    }
}
