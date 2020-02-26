using System;

namespace FootballAPI.Models
{
    public class Event
    {
        public string home { get; set; }
        public string away { get; set; }
        public DateTime start_time { get; set; }
    }
}