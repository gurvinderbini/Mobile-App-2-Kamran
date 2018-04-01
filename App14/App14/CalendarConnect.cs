using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14
{
    public interface CalendarConnect
    {
        string fromAndroidNative(string name);
        string AddEvent(string title, DateTime startDate, string shour, string smin, DateTime endDate, string ehour, string emin);
        List<string> CalendarList();
    }
}
