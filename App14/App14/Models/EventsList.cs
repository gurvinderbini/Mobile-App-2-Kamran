using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14
{
    public class EventsList
    {
        public EventsList()
        {
        }
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string startDate { get; set; }
        public string strtDate { get; set; }
        public string strtMonth { get; set; }
        public string startTime { get; set; }
        public string endDate { get; set; }
        public string endTime { get; set; }
        public string overallTime { get; set; }
        public string email { get; set; }
        public string notificationTime { get; set; }
    }
}
