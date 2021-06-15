using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class EventModel
    {
        public int id { get; set; }
        public List<string> images { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string desc { get; set; }
        public string condition { get; set; }
        public string reg_period { get; set; }
        public string reg_period_start { get; set; }
        public string reg_period_end { get; set; }
        public bool is_expire { get; set; }
        public bool is_register { get; set; }
        public bool without_guest { get; set; }
        public bool car_owner_only { get; set; }
        public string text_message { get; set; }
        public int remaining_count { get; set; }
        public bool is_one_member_per_event { get; set; }

    }

    public class EventDetailControlButton
    {
        public bool is_register { get; set; }
        public string text_message { get; set; }
        public int remaining_count { get; set; }
    }


    public class EventDetailForCalculate
    {
        public DateTime register_start { get; set; }
        public DateTime register_end { get; set; }
        public DateTime config_period_start { get; set; }
        public DateTime config_period_end { get; set; }
        public bool allow_dupplicate_follow_number_of_vin { get; set; }
        public bool allow_dupplicate_register_event { get; set; }

    }

    public class EventCode
    {
        public string redeem_code { get; set; }
    }
}