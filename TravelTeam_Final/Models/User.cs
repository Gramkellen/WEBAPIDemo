using System;
using System.Collections.Generic;

namespace TravelTeam_Final.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? WechatNumber { get; set; }

    public decimal? Gender { get; set; }

    public string? Location { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Email { get; set; }

    public string HeadImageUrl { get; set; } = null!;

    public decimal VipGrade { get; set; }

    public virtual ICollection<Chatmessage> Chatmessages { get; set; } = new List<Chatmessage>();

    public virtual ICollection<Mediamessage> Mediamessages { get; set; } = new List<Mediamessage>();

    public virtual ICollection<Teamrelation> Teamrelations { get; set; } = new List<Teamrelation>();
}
