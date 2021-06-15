using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceLoginModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public _ServiceLoginData data { get; set; }
    }
    public class _ServiceLoginData
    {
        public string otp_token { get; set; }
        public bool is_term { get; set; }
        public bool is_preference { get; set; }
    }
}