using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Trainingtype
{
    public decimal Trainingtypeid { get; set; }

    public string Trainingtypename { get; set; } = null!;

    public string? Isactive { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
