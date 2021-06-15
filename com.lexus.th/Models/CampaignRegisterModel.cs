using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class CampaignRegisterModel
    {
        public int booking_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string contact_number { get; set; }
        public string email { get; set; }
        public int preferred_model_id { get; set; }
        public string preferred_model_name { get; set; }
        public int dealer_id { get; set; }

        public string type { get; set; }

        public bool need_to_test_drive { get; set; }
        public string remark { get; set; }
        public string booking_code { get; set; }
        //referral
        public string plate_number { get; set; }
        public string car_model { get; set; }
        public string referral_name { get; set; }
        public string referral_surname { get; set; }
        public string referral_contact_number { get; set; }
        public string referral_email { get; set; }
        public string current_car_brand { get; set; }
        public string current_car_model { get; set; }


        //car booking
        public int current_car_brand_id { get; set; }
        public int current_car_model_id { get; set; }
        public string referral_code { get; set; }
        public string device_id { get; set; }
        public List<RegisterQuestion> question_answers { get; set; }
        public string confirm_checkbox_date { get; set; }
        public string confirm_popup_date { get; set; }
    }
    public class RegisterQuestion
    {
        public int question_id { get; set; }
        public int answer_id { get; set; }
        public string answer_text { get; set; }
    }
}