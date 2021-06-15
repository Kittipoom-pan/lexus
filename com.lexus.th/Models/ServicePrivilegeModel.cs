using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServicePrivilegeModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServicePrivilegeData data { get; set; }
    }
    public class _ServicePrivilegeData
    {
        public PrivilegeModel privileges { get; set; }
    }

    public class ServiceCheckPrivilegeRedeemModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceCheckPrivilegeRedeemData data { get; set; }
    }
    public class _ServiceCheckPrivilegeRedeemData
    {
        public string status { get; set; }
        public string expiry_ts { get; set; }
        public int customer_remain_in_period { get; set; }
        public int customer_usage_per_period { get; set; }
        public int remaining_count { get; set; }

    }
}