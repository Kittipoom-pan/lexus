using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class SMSModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
    }
    public class SMSReportModel
    {
        public string STATUS { get; set; }
        public string DETAIL { get; set; }  
    }
}