using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Models
{
    public class ModelPrediction
    {
        public string league_name { get; set; }
        public string match_date { get; set; }
        public string match_time { get; set; }
        public string match_hometeam_name { get; set; }
        public string match_hometeam_score { get; set; }
        public string match_awayteam_name { get; set; }
        public string match_awayteam_score { get; set; }
        public string match_stadium { get; set; }
    }
}
