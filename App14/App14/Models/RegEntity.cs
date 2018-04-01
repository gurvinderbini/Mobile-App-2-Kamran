using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace App14
{
    public class RegEntity
    {
        public RegEntity()
        {
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string school_name { get; set; }
        public string CompleteUrl { get; set; }
    }
}
