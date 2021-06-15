using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class HistoryEventModel : ResponseBaseModel
    {
        public HistoryEventModel() { data = new List<EventHistoryDataModel>(); }
        public List<EventHistoryDataModel> data { get; set; }

    }

    public class EventHistoryDataModel
    {
        public int register_id { get; set; }
        public string title { get; set; }
        public List<string> images { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string description { get; set; }
        public string condition { get; set; }
        public string reg_period { get; set; }
        public string thankyou_message { get; set; }
        public string register_code { get; set; }
    }
}