using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class Language
{
    public int LanguageId { get; set; }

    public string LanguageName { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<OfficialPost> OfficialPosts { get; set; } = new List<OfficialPost>();

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
