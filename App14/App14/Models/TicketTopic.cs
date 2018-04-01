using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    class TicketTopic
    {
        public bool status { get; set; }
        public Result[] result { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string topic_title { get; set; }
    }
    
}
