using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class HistoryOnlineBookingModel : ResponseBaseModel
    {
        public HistoryOnlineBookingModel() { data = new List<HistoryOnlineModel>(); }
        public List<HistoryOnlineModel> data { get; set; }

    }

    public class HistoryOnlineModel
    {
        public int booking_register_id { get; set; }
        public int booking_id { get; set; }
        public string booking_code { get; set; }
        public string title { get; set; }
        public string preferred_model_name { get; set; }
        public string dealer_name { get; set; }
        public string type { get; set; }
        public string plate_number { get; set; }
        public string referral_name { get; set; }

        public string register_date { get; set; }
        public string code_message { get; set; }
        public string call_center { get; set; }
    }
}