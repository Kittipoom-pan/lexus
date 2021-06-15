using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceDistrictModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceDistrictData data { get; set; }
    }
    public class _ServiceDistrictData
    {
        public List<DistrictModel> district { get; set; }
    }
}