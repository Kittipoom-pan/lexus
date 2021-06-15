using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceHomeModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceHomeData data { get; set; }
    }
    public class _ServiceHomeData
    {
        public List<PrivilegeModel> privileges { get; set; }
        public List<EventModel> events { get; set; }
        public List<NewsModel> news { get; set; }
        public bool force_logout { get; set; }
        public string call_center { get; set; }
        public bool is_term { get; set; }
    }
}