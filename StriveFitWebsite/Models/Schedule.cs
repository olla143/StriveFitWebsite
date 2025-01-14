using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Schedule
{ 
    public decimal Scheduleid { get; set; }

    public decimal Trainerid { get; set; }

    public DateTime Starttime { get; set; }

    public DateTime Endtime { get; set; }

    public string? Schedulestatus { get; set; }

    public decimal Trainingid { get; set; }

    public decimal? Capacity { get; set; }

    public string? Goal { get; set; }

    public TimeSpan? Lectuerstime { get; set; }

    public string? Exercisroutine { get; set; }

    public string? Classtype { get; set; }

    public virtual User Trainer { get; set; }

    public virtual Trainingtype Training { get; set; }

    public virtual ICollection<Workoutplan> Workoutplans { get; set; } = new List<Workoutplan>();
}
