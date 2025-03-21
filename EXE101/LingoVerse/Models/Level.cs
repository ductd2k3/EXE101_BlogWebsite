﻿using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class Level
{
    public int LevelId { get; set; }

    public string LevelName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<OfficialPost> OfficialPosts { get; set; } = new List<OfficialPost>();

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
