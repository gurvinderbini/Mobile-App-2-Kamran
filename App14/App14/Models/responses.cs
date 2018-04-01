using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    class responses
    {
        public bool status { get; set; }
        public ResponseResult[] result { get; set; }
    }
    public class ResponseResult
    {
        public string id { get; set; }
        public string ticket_id { get; set; }
        public string user_id { get; set; }
        public string full_name { get; set; }
        public string response_content { get; set; }
        public string response_files { get; set; }
        public string created_by { get; set; }
        public string created { get; set; }
        public string modified_by { get; set; }
        public string modified { get; set; }
        public string assigned_to { get; set; }
        public string ticket_responsesno { get; set; }
        public string tenant_id { get; set; }
    }
}
