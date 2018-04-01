using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14.Models
{
    class TimeSheet
    {
        public bool status { get; set; }
        public TimeSheetResultData[] result { get; set; }
    }
    public class TimeSheetResultData
    {
        public string id;
        public string status;
    }

}
