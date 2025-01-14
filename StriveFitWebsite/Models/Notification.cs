using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Notification
{
    public decimal Notificationid { get; set; }

    public decimal Userid { get; set; }

    public string Message { get; set; } = null!;

    public string? Isread { get; set; }

    public DateTime? Createddate { get; set; }

    public virtual User User { get; set; } = null!;
}
