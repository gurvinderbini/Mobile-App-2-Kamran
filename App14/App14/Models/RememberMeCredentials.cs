using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    public class RememberMeCredentials
    {
        public RememberMeCredentials()
        {
        }
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string url_name { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public int sign_out { get; set; }
    }
}
