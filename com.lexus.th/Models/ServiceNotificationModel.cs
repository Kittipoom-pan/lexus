using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceNotificationModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceNotificationData data { get; set; }
    }
    public class _ServiceNotificationData
    {
        public List<NotificationModel> notification { get; set; }
    }

    public class ServiceNotiModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
    }
}