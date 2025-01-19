using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Userlogin
{
    public decimal Loginid { get; set; }

    public string Username { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public decimal Roleid { get; set; }

    public decimal Userid { get; set; }

    public DateTime? Lastlogin { get; set; }

    public string? Isactive { get; set; }

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}
