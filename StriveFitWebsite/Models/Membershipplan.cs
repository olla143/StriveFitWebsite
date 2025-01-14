using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Membershipplan
{
    public decimal Planid { get; set; }

    public string Planname { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Durationmonths { get; set; }

    public string? Details { get; set; }
    public List<string> Features
    {
        get
        {
            return Planname switch
            {
                "Normal" => new List<string>
                {
                    "Unlimited access to the gym",
                    "1 class per week",
                    "FREE drinking package",
                    "1 Free personal training",
                    "1 Month"
                },
                "Professional" => new List<string>
                {
                    "Unlimited access to the gym",
                    "2 classes per week",
                    "FREE drinking package",
                    "2 Free personal training",
                    "3 Months"
                },
                "Advanced" => new List<string>
                {
                    "Unlimited access to the gym",
                    "6 classes per week",
                    "FREE drinking package",
                    "5 Free personal training",
                    "6 Months"
                },
                _ => new List<string>()
            };
        }
    }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
