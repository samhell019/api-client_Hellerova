using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_client_Hellerova.Models
{
    class Films
    {
        public int filmid { get; set; }
        public string name { get; set; }
        public int? zanrid { get; set; }
    }

    class Zanrs
    {
        public int zanrid { get; set; }
        public string name { get; set; }
    }

}
