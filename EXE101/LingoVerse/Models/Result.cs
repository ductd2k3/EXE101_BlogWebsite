using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class Result
{
    public int ResultId { get; set; }

    public int PostId { get; set; }

    public int UserId { get; set; }

    public decimal Score { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual OfficialPost Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
