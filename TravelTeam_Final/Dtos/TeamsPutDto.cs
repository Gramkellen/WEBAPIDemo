using TravelTeam_Final.Models;

namespace TravelTeam_Final.Dtos
{
    public class TeamsPutDto
    {
        public string TeamName { get; set; } = null!;

        public decimal TeamStatus { get; set; }

        public decimal Maxnumber { get; set; }

        public DateTime TravelBeginTime { get; set; }

        public DateTime? TravelEndTime { get; set; }

        public string Destination { get; set; } = null!;

        public string Arrangement { get; set; } = null!;
        //用字符串列表传递标签
        public List<string>? TagList { get; set; }

        public List<string>? MediaList { get; set; }

    }
}
