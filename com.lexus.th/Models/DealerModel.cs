using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class DealerModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string branch_name { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string office_hours { get; set; }
        public string office_hours2 { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public float distance { get; set; }

        //add in 2.2.0
        public bool is_pickup_service { get; set; }
        public string pin_path { get; set; }

        /*DEALER_ID
        DEALER_NAME
        DEALER_ADDRESS
        DEALER_MOBILE
        DEALER_OFFICE_HOURS
        PROVINCE_ID*/

    }
}