using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14
{

    public class DevicesResult
    {
        public bool status { get; set; }
        public DevicesResultData[] result { get; set; }
    }

    public class DevicesResultData
    {
        public string id { get; set; }
        public string devices_department { get; set; }
        public string devices_department_id { get; set; }
        public string name { get; set; }
        public string devices_make { get; set; }
        public string devices_model { get; set; }
        public string devices_purchased_on { get; set; }
        public string devices_purchased_from { get; set; }
        public string operating_system { get; set; }
        public string devices_ram { get; set; }
        public string devices_hdd { get; set; }
        public string devices_processor { get; set; }
        public string devices_os_activated { get; set; }
        public string devices_antivirus { get; set; }
        public string devices_temperature { get; set; }
        public string devices_device_age { get; set; }
        public string devices_hdd_status { get; set; }
        public string devices_linked_devices { get; set; }
        public string devices_linked_devices_id { get; set; }
        public string devices_update_date { get; set; }
        public string devices_created { get; set; }
    }

}
