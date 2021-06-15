using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceRedeemModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceRedeemData data { get; set; }
    }
    public class _ServiceRedeemData
    {
        public int id { get; set; }
        public string redeem_code { get; set; }
        public string expiry_ts { get; set; }
        public string status { get; set; }
        public int iredeem_display_type { get; set; }
        public string redeem_display_type { get; set; }
        public string redeem_display_image { get; set; }
        public string redeem_display_html { get; set; }
        public int redeem_display_height { get; set; }
        public int redeem_display_width { get; set; }
        public string redeem_datetime { get; set; }
    }
}