using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class NotificationModel
    {
        public int id { get; set; }
        public string notify_title { get; set; }
        public string notify_message { get; set; }
        public string notify_type { get; set; }
        public string link_url { get; set; }
        public int reference_id { get; set; }
        public string create_date { get; set; }
        //public int push_remaining_day { get; set; }
    }
}