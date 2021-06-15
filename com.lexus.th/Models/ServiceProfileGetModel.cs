using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceProfileGetModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceProfileGetData data { get; set; }
    }
    public class _ServiceProfileGetData
    {
        public int id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public int privilege_cnt { get; set; }
        public string email { get; set; }
        public string tel_no { get; set; }
        public string expiry { get; set; }
        public List<CarModel> cars { get; set; }
        public string disp_name { get; set; }
        public string expiry_ts { get; set; }
        public string profile_image { get; set; }
        public string member_id { get; set; }
        public string title { get; set; }
        public string birthdate { get; set; }
    }
    public class ServiceProfileNewGetModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceProfileNewGetData data { get; set; }
    }
    public class _ServiceProfileNewGetData
    {
        public ProfileGetData app_user { get; set; }
        public ProfileGetData car_owner { get; set; }
    }
    public class ProfileGetData
    {
        public int id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public int privilege_cnt { get; set; }
        public string email { get; set; }
        public string tel_no { get; set; }
        public string expiry { get; set; }
        public List<CarModel> cars { get; set; }
        public string disp_name { get; set; }
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