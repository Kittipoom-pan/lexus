using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceEventVinModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTime ts { get; set; }
        public _ServiceEventVinData data { get; set; }

    }

    public class _ServiceEventVinData
    {
        public List<CarModel> cars { get; set; }
    }
}