using System;
using System.Collections.Generic;

namespace StriveFitWebsite.Models;

public partial class Role
{
    public decimal Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();
}
