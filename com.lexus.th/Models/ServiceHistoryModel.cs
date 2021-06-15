using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceHistoryModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceHistoryData data { get; set; }
    }
    public class _ServiceHistoryData
    {
        public List<HistoryModel> items { get; set; }
    }
}