using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class OfficialPost
{
    public int PostId { get; set; }

    public int CategoryId { get; set; }

    public int AuthorId { get; set; }

    public int LanguageId { get; set; }

    public int LevelId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Status { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public bool IsDeleted { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual Level Level { get; set; } = null!;

    public virtual ICollection<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; } = new List<MultipleChoiceQuestion>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ICollection<WritingQuestion> WritingQuestions { get; set; } = new List<WritingQuestion>();
}
