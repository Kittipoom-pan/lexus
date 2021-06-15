using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.Models
{
    public class NotificationCountModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public NotificationCount data { get; set; }
    }
    
    public class NotificationCount
    {
        public int noti_count { get; set; }
    }
}