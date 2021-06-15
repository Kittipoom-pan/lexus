using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceEventModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceEventData data { get; set; }
    }
    public class _ServiceEventData
    {
        public EventModel events { get; set; }
    }

}