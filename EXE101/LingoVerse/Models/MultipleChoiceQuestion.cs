using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class MultipleChoiceQuestion
{
    public int QuestionId { get; set; }

    public int PostId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string OptionA { get; set; } = null!;

    public string OptionB { get; set; } = null!;

    public string? OptionC { get; set; }

    public string? OptionD { get; set; }

    public string CorrectAnswer { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual OfficialPost Post { get; set; } = null!;
}
