using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.Models
{
    public class NotificationSendModel
    {
        public string id { get; set; }
        public string destination { get; set; }
        public string sub_destination { get; set; }
        public string message { get; set; }
        public string title { get; set; }
        public string link_type { get; set; }
        public string link_url { get; set; }
        public string member_id { get; set; }
        public string device_id { get; set; }
        public string reference_id { get; set; }

    }
}