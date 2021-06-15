using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceOnlineBookingModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceOnlineBookingData data { get; set; }
    }

    public class _ServiceOnlineBookingData
    {
        public List<OnlineBookingModel> bookings { get; set; }
    }

    public class ServiceOnlineBookingRegister
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceOnlineBookingRegisterData data { get; set; }
    }

    public class _ServiceOnlineBookingRegisterData
    {
        public string code { get; set; }
        public string thankyou_message { get; set; }
        public string booking_register_id { get; set; }
    }


    public class OnlineBookingDetailForExportExcel
    {
        public int booking_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string contact_number { get; set; }
        public string email { get; set; }
        public int preferred_model_id { get; set; }
        public int dealer_id { get; set; }

        public int type { get; set; }

        public bool need_to_test_drive { get; set; }
        public string remark { get; set; }

        //referral
        public string plate_number { get; set; }


        //car booking
        public int current_car_brand_id { get; set; }
        public int current_car_model_id { get; set; }
        public string referral_code { get; set; }
    }
}