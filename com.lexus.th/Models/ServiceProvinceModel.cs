using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceProvinceModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceProvinceData data { get; set; }
    }
    public class _ServiceProvinceData
    {
        public List<ProvinceModel> province { get; set; }
    }
}