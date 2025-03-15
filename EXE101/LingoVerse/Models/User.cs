using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CommunityPost> CommunityPosts { get; set; } = new List<CommunityPost>();

    public virtual ICollection<OfficialPost> OfficialPosts { get; set; } = new List<OfficialPost>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();

    public virtual ICollection<UserPremiumSubscription> UserPremiumSubscriptions { get; set; } = new List<UserPremiumSubscription>();
}
