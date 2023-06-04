using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Models
{
    public class TeamInformation
    {
        public TeamData[] Response { get; set; }
    }
    public class TeamData
    {
        public Team Team { get; set; }
        public Venue Venue { get; set; }
    }
    public class Team
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public int Founded { get; set; }
        public string Logo { get; set; }
    }
    public class Venue
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }
        public string Image { get; set; }
    }
}
