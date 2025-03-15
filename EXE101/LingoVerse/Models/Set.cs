using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class Set
{
    public int SetId { get; set; }

    public int UserId { get; set; }

    public int LanguageId { get; set; }

    public int CategoryId { get; set; }

    public int LevelId { get; set; }

    public string SetName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int WordCount { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual Level Level { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Word> Words { get; set; } = new List<Word>();
}
