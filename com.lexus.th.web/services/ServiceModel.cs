using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class ServiceModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class ServiceRedeemModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceRedeemData data { get; set; }
    }
    public class _ServiceRedeemData
    {
        public int id { get; set; }
        public string redeem_code { get; set; }
        public string expiry_ts { get; set; }
    }
    public class MsgModel
    {
        public int code { get; set; }
        public string text { get; set; }
    }
    public class FirebasePostModel
    {
        public string to { get; set; }
        //public bool content_available { get; set; }
        public _FirebasePostModel notification { get; set; }
        public _FirebaseData data { get; set; }
    }
    public class _FirebasePostModel
    {
        public string body { get; set; }
        public string title { get; set; }
        public string sound { get; set; }
        public bool content_available { get; set; }
        public int badge { get; set; }
    }
    public class _FirebaseData
    {
        public int reference_id { get; set; }
        public string notify_type { get; set; }
        public string link_url { get; set; }
        public string notify_message { get; set; }
        public string notification_id { get; set; }
    }

    public class FirebasePostAndroidModel
    {
        public string to { get; set; }
        //public bool content_available { get; set; }
        public _FirebaseAndroidData data { get; set; }
    }

    public class _FirebaseAndroidData
    {
        public int reference_id { get; set; }
        public string notify_type { get; set; }
        public string notify_title { get; set; }
        public string link_url { get; set; }
        public string notify_message { get; set; }
        public string notification_id { get; set; }
    }
}