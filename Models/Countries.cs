using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Models
{
    public class Countries
    {
        public List<Response_> response { get; set; }
    }
    public class Response_
    {
        public string name { get; set; }
        public string flag { get; set; }
    }
}
