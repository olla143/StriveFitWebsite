using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Subscription
{
    public decimal Subscriptionid { get; set; }

    public decimal Userid { get; set; }

    public decimal Planid { get; set; }

    public string? Renewalstatus { get; set; }

    public string? Paymentstatus { get; set; }

    public DateTime Startdate { get; set; }

    public DateTime Enddate { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Membershipplan? Plan { get; set; }

    public virtual User? User { get; set; }
}
