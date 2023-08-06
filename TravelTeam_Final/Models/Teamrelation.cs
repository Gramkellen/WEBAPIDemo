using System;
using System.Collections.Generic;

namespace TravelTeam_Final.Models;

public partial class Teamrelation
{
    public string TeamId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public decimal UserStatus { get; set; }

    public virtual Travelteam Team { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
