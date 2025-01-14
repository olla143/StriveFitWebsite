using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StriveFitWebsite.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Isactive { get; set; }

    public decimal? Balance { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }

    public virtual ICollection<Contactmessage> Contactmessages { get; set; } = new List<Contactmessage>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();

    public virtual ICollection<Workoutplan> WorkoutplanMembers { get; set; } = new List<Workoutplan>();

    public virtual ICollection<Workoutplan> WorkoutplanTrainers { get; set; } = new List<Workoutplan>();
}
