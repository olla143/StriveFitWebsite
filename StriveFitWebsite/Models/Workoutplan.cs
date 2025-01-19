using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Workoutplan
{
    public decimal Workoutid { get; set; }

    public decimal Trainerid { get; set; }

    public decimal Memberid { get; set; }

    public decimal? Scheduleid { get; set; }

    public virtual User? Member { get; set; }

    public virtual Schedule? Schedule { get; set; }

    public virtual User? Trainer { get; set; }
}
