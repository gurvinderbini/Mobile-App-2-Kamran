using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    public class rooms
    {
        public bool status { get; set; }
        public RoomsResultData[] result { get; set; }
    }
    public class RoomsResultData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string rooms_person { get; set; }
        public string rooms_person_id { get; set; }
        public string rooms_location { get; set; }
        public string rooms_created { get; set; }
    }
}
