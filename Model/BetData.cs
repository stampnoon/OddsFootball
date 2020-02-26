using System;

namespace FootballAPI.Models
{
    public class BetData
    {
        public string League { get; set; }
        public Event Event { get; set; }
        public Odds odds { get; set; }
    }
}