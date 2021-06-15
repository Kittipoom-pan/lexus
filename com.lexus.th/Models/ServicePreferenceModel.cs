using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServicePreferenceModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServicePreferenceData data { get; set; }
    }
    public class _ServicePreferenceData
    {
        public List<PreferenceModel> preference { get; set; }
    }
}