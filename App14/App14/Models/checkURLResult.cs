using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    public class checkURLResult
    {
        public bool status { get; set; }
        public getURL result { get; set; }
    }
    public class getURL
    {
        public bool status { get; set; }
        public string target { get; set; }
    }
}
