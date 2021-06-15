using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceEventRegisterModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public ServiceEventRegisterData data { get; set; }
    }

    public class ServiceEventRegisterData
    {
        public string thankyou_message { get; set; }
        public string event_type { get; set; }
        public string invitation_code { get; set; }
        public string name { get; set; }
        public string registered_date { get; set; }
        public List<string> images { get; set; }
    }
}