using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceLoginOTPModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public _ServiceLoginOTPData data { get; set; }
    }
    public class _ServiceLoginOTPData
    {
        public string token { get; set; }
        public bool is_term { get; set; }
        public bool is_preference { get; set; }
        public bool is_update { get; set; }
        public bool is_first_login { get; set; }
        public string term_cond_url { get; set; }
    }
}