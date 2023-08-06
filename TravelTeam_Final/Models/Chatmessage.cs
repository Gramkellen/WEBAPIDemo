using System;
using System.Collections.Generic;

namespace TravelTeam_Final.Models;

public partial class Chatmessage
{
    public DateTime PostTime { get; set; }

    public string UserId { get; set; } = null!;

    public string TeamId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public virtual Travelteam Team { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
