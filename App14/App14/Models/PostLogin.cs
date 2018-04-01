using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14
{


    public class PostLogin
    {
        public bool status { get; set; }
        public string []result;
        public ResultClass[] resultdata { get; set; }
        public PostLogin()
        {

        }
    }

    public class ResultClass
    {
        public string session_string { get; set; }
        public string user_email { get; set; }
        public string user_id { get; set; }
        public string user_level { get; set; }
        public string user_status { get; set; }
        public string user_full_name { get; set; }
        public string message { get; set; }
        
    }


}
