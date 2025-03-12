using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<OfficialPost> OfficialPosts { get; set; } = new List<OfficialPost>();

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
