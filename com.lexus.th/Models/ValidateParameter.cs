using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ValidateParameter
    {
        public int id { get; set; }
        public int service_appointment_type { get; set; }
        public int is_car_owner { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string mobile { get; set; }
        public string plate_no { get; set; }
        public string vehicle_no { get; set; }
        public int dealer_id { get; set; }
        public string remark { get; set; }
        public string appt_date { get; set; }
        public int appt_time_id { get; set; }
        public List<int> type_of_service_ids { get; set; }
        public int default_status { get; set; }

        //Test_drive
        public string device_id { get; set; }
        public string email { get; set; }
        public int preferred_model_id { get; set; } 
        public int purchase_plan_id { get; set; }
        



        public string model_name { get; set; }       
        public string type_of_service_name { get; set; }      
        public string dealer_name { get; set; }
        public string dealer_mobile { get; set; }       
        public string appointmentdate { get; set; }
        public string appointmenttime { get; set; }
        public string footer_remark_description { get; set; }
        public DateTime created_date { get; set; }
        public string created_user { get; set; }
        public DateTime updated_date { get; set; }
        public string updated_user { get; set; }
        public string deleted_flag { get; set; }
        public DateTime deleted_date { get; set; }
        public string deleted_user { get; set; }   
        public int status_id { get; set; }
        public string status_name { get; set; }
        public string member_id { get; set; }
        public string date_display { get; set; }

        public bool is_pickup_service { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string location_detail { get; set; }
        public string pickup_date { get; set; }
        public int pickup_time_id { get; set; }
        public string pickup_address { get; set; }
    }



    public class Validate_Booking
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
    }
}