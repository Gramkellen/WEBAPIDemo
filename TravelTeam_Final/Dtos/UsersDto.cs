namespace TravelTeam_Final.Dtos
{
    public class UsersDto
    {
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
    }
}
