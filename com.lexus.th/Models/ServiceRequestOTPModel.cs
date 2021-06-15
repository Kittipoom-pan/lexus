using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceRequestOTPModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public _ServiceRequestOTPData data { get; set; }
    }
    public class _ServiceRequestOTPData
    {
        public string reference_code { get; set; }
        public string otp_code { get; set; }
        public string otp_expire { get; set; }
    }

    public class ServiceCheckOTPModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
    }
}