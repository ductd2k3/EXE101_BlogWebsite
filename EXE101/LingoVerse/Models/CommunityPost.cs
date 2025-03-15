using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class CommunityPost
{
    public int PostId { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Status { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public bool IsDeleted { get; set; }

    public virtual User Author { get; set; } = null!;
}
