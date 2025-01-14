using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Contactmessage
{
    public decimal Messageid { get; set; }

    public decimal? Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Useremail { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? Submissiondate { get; set; }

    public string? Status { get; set; }

    public virtual User? User { get; set; }
}
