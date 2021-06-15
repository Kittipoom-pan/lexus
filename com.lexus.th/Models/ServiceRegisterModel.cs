using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceRegisterModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public _ServiceRegisterData data { get; set; }
    }

    public class _ServiceRegisterData
    {
        public string member_id { get; set; }
        public string otp_token { get; set; }
        public bool is_term { get; set; }
        public bool is_preference { get; set; }
        public bool is_first_login { get; set; }
    }

    public class ServiceCheckRegisterModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
    }
}