using System;
using System.Collections.Generic;

namespace TravelTeam_Final.Models;

public partial class Mediamessage
{
    public string MediaUrl { get; set; } = null!;

    public string MuserId { get; set; } = null!;

    public string MteamId { get; set; } = null!;

    public decimal MediaSta { get; set; }

    public DateTime PostTime { get; set; }

    public virtual Travelteam Mteam { get; set; } = null!;

    public virtual User Muser { get; set; } = null!;
}
