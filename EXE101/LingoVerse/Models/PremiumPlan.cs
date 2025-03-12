using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class PremiumPlan
{
    public int PlanId { get; set; }

    public string PlanName { get; set; } = null!;

    public string? Description { get; set; }

    public int DurationDays { get; set; }

    public decimal Price { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<UserPremiumSubscription> UserPremiumSubscriptions { get; set; } = new List<UserPremiumSubscription>();
}
