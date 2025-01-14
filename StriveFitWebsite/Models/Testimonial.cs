using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public decimal Memberid { get; set; }

    public string? Content { get; set; }

    public string? Status { get; set; }

    public DateTime? Submitteddate { get; set; }

    public decimal? Rating { get; set; }

    public virtual User Member { get; set; } = null!;
}
