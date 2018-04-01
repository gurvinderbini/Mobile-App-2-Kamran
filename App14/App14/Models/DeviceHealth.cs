using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    public class HealthResult
    {
        public bool status { get; set; }
        public HealthResultData[] result { get; set; }
    }

    public class HealthResultData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string operating_system { get; set; }
        public string ram { get; set; }
        public string cpu_usage { get; set; }
        public string processor { get; set; }
        public string hdd { get; set; }
        public string os_activated { get; set; }
        public string os_activated_id { get; set; }
        public string antivirus { get; set; }
        public string temperature { get; set; }
        public string device_age { get; set; }
        public string hdd_status { get; set; }
        public string linked_devices { get; set; }
        public string linked_devices_id { get; set; }
        public string update_date { get; set; }
        public string created { get; set; }
    }
    class DeviceHealth
    {
    }
}
