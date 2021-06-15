using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceDealerModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceDealerData data { get; set; }
    }
    public class _ServiceDealerData
    {
        public List<DealerModel> dealers { get; set; }
    }
}