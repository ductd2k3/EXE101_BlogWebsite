using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class WritingQuestion
{
    public int QuestionId { get; set; }

    public int PostId { get; set; }

    public string QuestionText { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual OfficialPost Post { get; set; } = null!;
}
