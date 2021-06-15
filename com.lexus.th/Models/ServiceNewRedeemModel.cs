using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceNewRedeemModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceNewRedeemData data { get; set; }
    }
    public class _ServiceNewRedeemData
    {
        public int id { get; set; }
        public string redeem_code { get; set; }
        public string expiry_ts { get; set; }
    }
}