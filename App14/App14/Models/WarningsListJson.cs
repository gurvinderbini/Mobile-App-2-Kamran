using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14
{

    public class WarningsResult
    {
        public bool status { get; set; }
        public WarningsResultData[] result { get; set; }
    }

    public class WarningsResultData
    {
        public string id { get; set; }
        public string warnings_count { get; set; }
        public string warnings_device_id { get; set; }
        public string device_name { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public Warnings_Warnings warnings_warnings { get; set; }
        public string warnings_has_warning { get; set; }
        public string warnings_created { get; set; }
    }

    public class Warnings_Warnings
    {
        public OS os { get; set; }
        public Antivirus antivirus { get; set; }
        public Temperature temperature { get; set; }
        public Hdd_Status hdd_status { get; set; }
        public CPU cpu { get; set; }
        public RAM ram { get; set; }
        public Name name { get; set; }
        public Date date { get; set; }
    }
    public class Name
    {
        public string data { get; set; }
        public bool status { get; set; }
        public string warning { get; set; }
    }
    public class Date
    {
        public string data { get; set; }
        public bool status { get; set; }
        public string warning { get; set; }
    }

    public class OS
    {
        public string data { get; set; }
        public bool status { get; set; }
        public string warning { get; set; }
    }

    public class CPU
    {
        public string data { get; set; }
        public bool status { get; set; }
        public string warning { get; set; }
    }

    public class RAM
    {
        public string data { get; set; }
        public bool status { get; set; }
        public string warning { get; set; }
    }

    public class Antivirus
    {
        public string data { get; set; }
        public bool status { get; set; }
        public string warning { get; set; }
    }

    public class Temperature
    {
        public string data { get; set; }
        public bool status { get; set; }
        public string warning { get; set; }
    }

    public class Hdd_Status
    {
        public string data { get; set; }
        public bool status { get; set; }
        public string warning { get; set; }
    }

}
