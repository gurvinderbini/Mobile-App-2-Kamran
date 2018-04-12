using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace App14.Models
{
    public class NotificationBO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Screen { get; set; }
        public string Body { get; set; }
        public string Sound { get; set; }
        public string ContentAvailable { get; set; }
    }
}
