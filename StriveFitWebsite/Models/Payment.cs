using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Payment
{
    public decimal Paymentid { get; set; }

    public decimal Subscriptionid { get; set; }

    public decimal Amount { get; set; }

    public DateTime? Paymentdate { get; set; }

    public string Paymentmethod { get; set; } = null!;

    public string? Paymentstatus { get; set; }

    public virtual Subscription? Subscription { get; set; }
}
