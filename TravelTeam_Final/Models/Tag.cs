using System;
using System.Collections.Generic;

namespace TravelTeam_Final.Models;

public partial class Tag
{
    public string TagName { get; set; } = null!;

    public string TeamId { get; set; } = null!;

    public virtual Travelteam Team { get; set; } = null!;
}
