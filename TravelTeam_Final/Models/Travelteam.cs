using System;
using System.Collections.Generic;

namespace TravelTeam_Final.Models;

public partial class Travelteam
{
    public string TeamId { get; set; } = null!;

    public string TeamName { get; set; } = null!;

    public decimal TeamStatus { get; set; }

    public decimal Currentnumber { get; set; }

    public decimal Maxnumber { get; set; }

    public DateTime PostTime { get; set; }

    public DateTime TravelBeginTime { get; set; }

    public DateTime? TravelEndTime { get; set; }

    public string Destination { get; set; } = null!;

    public string Arrangement { get; set; } = null!;

    public virtual ICollection<Chatmessage> Chatmessages { get; set; } = new List<Chatmessage>();

    public virtual ICollection<Mediamessage> Mediamessages { get; set; } = new List<Mediamessage>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<Teamrelation> Teamrelations { get; set; } = new List<Teamrelation>();
}
