using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    class LoginData
    {
        public bool status { get; set; }
        public LoginDataResult[] result { get; set; }
    }
    public class LoginDataResult
    {
        public string session_string { get; set; }
        public string user_email { get; set; }
        public string user_id { get; set; }
        public string user_level { get; set; }
        public string user_status { get; set; }
        public string user_full_name { get; set; }
        public string message { get; set; }

        public string user_tenant_id { get; set; }

    }
}
