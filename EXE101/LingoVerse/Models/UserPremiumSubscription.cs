using System;
using System.Collections.Generic;

namespace LingoVerse.Models;

public partial class UserPremiumSubscription
{
    public int SubscriptionId { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual PremiumPlan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
