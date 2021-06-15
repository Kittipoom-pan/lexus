using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class HistoryModel
    {
        public int id { get; set; }
        public string title { get; set; }
        //public string desc { get; set; }
        public bool is_expire { get; set; }
        public string image { get; set; }
        public string expiry_ts { get; set; }
        public string redeem_code { get; set; }
        public bool is_verified { get; set; }
        //public string redeem_id { get; set; }
        public string redeem_date { get; set; }
        public string redeem_datetime { get; set; }

        public int iredeem_display_type { get; set; }
        public string redeem_display_type { get; set; }
        public string redeem_display_image { get; set; }
        public string redeem_display_html { get; set; }
        public int redeem_display_height { get; set; }
        public int redeem_display_width { get; set; }
    }
}