using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    public class Dashboard
    {
        public bool status { get; set; }
        public DashboardResultData[] result { get; set; }
    }
    public class DashboardResultData
    {
        public string online { get; set; }
        public string devices { get; set; }
        public string tickets { get; set; }
        public string warnings { get; set; }
        public string todo { get; set; }
        public string LCSFI { get; set; }
    }
}
