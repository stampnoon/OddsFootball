namespace FootballAPI.Models
{
    public class Bet365
    {
        public Odds odds { get; set; }
        public bool bookmaker { get; set; }
        public bool exchange { get; set; }
        public string name { get; set; }

    }
}