using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServicePreferredModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTimeOffset ts { get; set; }
        public ServicePreferredData data { get; set; }
    }


    public class ServicePreferredData
    {
        public List<_ServicePreferredData> preferred_model_datas { get; set; }
    }

    public class _ServicePreferredData
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}