using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceMessageMobileModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public _ServiceMessageMobileData data { get; set; }
    }
    public class _ServiceMessageMobileData
    {
        public List<MessageMobileModel> message { get; set; }
    }
}