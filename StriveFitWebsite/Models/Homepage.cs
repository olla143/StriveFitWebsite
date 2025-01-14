using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StriveFitWebsite.Models;

public partial class Homepage
{
    public decimal Pageid { get; set; }

    public string Sectionname { get; set; } = null!;

    public string? Imageurl { get; set; }

    public string? Headings { get; set; }

    public string? Descriptions { get; set; }

    public string? Details { get; set; }
    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }
}
