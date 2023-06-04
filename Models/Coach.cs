using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Models
{
    public class Birth
    {
        public string date { get; set; }
        public string place { get; set; }
        public string country { get; set; }
    }

    public class Response
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
        public Birth birth { get; set; }
        public string nationality { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string photo { get; set; }
    }

    public class Coach
    {
        public List<Response> response { get; set; }
    }
}
