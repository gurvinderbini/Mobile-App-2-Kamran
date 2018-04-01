using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14
{

        public class TicketResult
        {
            public bool status { get; set; }
            public TicketResultData[] result { get; set; }
        }

        public class TicketResultData
        {
            public string tickets_id { get; set; }
            public string tickets_type { get; set; }
            public string tickets_type_id { get; set; }
            public string tickets_notify_user { get; set; }
            public string tickets_notify_user_id { get; set; }
            public string tickets_full_name { get; set; }
            public string tickets_email_address { get; set; }
            public string tickets_device_name { get; set; }
            public string tickets_device_id { get; set; }
            public string tickets_device_id_id { get; set; }
            public string tickets_summary { get; set; }
            public string tickets_detail { get; set; }
            public string tickets_source { get; set; }
            public string tickets_source_id { get; set; }
            public string tickets_topic { get; set; }
            public string tickets_topic_id { get; set; }
            public string tickets_sla_plan { get; set; }
            public string tickets_sla_plan_id { get; set; }
            public string tickets_due_date { get; set; }
            public string tickets_responses { get; set; }
            public string tickets_internal_notes { get; set; }
            public string tickets_status { get; set; }
            public string tickets_status_id { get; set; }
            public string tickets_tasks { get; set; }
            public string tickets_expenses { get; set; }
            public string tickets_timesheet { get; set; }
            public string tickets_created { get; set; }
            public string tickets_assigned_to { get; set; }
            public string tickets_assigned_to_id { get; set; }
        }

}
