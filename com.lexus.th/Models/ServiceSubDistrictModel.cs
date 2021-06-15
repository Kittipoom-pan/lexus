using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceSubDistrictModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceSubDistrictData data { get; set; }
    }
    public class _ServiceSubDistrictData
    {
        public List<SubDistrictModel> sub_district { get; set; }
    }
}