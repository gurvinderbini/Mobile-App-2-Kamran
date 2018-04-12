using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace App14.Models
{
    public class DeviceTokenBO
    {
        [PrimaryKey, AutoIncrement]

        public int Id { get; set; }
        public string DeviceToken { get; set; }

    }
}
