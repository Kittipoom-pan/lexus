using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class CustomerModel
    {
        public int id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string disp_name { get; set; }
        public int privilege_cnt { get; set; }
        public string email { get; set; }
        public string tel_no { get; set; }
        public string expiry { get; set; }
        public string expiry_ts { get; set; }
        public string profile_image { get; set; }
        public string member_id { get; set; }
        public string title { get; set; }
        public string birthdate { get; set; }
        public string plate_no { get; set; }
        public string citizen_id { get; set; }
        public string vehicle_no { get; set; }
    }
}