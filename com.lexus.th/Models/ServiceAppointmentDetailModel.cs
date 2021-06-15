using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAppointmentDetailModel
    {

        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTimeOffset ts { get; set; }
        public AppointmentDetailData data { get; set; }
    }
    public class AppointmentDetailData
    {
        public _ServiceAppointmentDetailData service_appointment_data { get; set; }
    }

    public class _ServiceAppointmentDetailData
    {
        public int id { get; set; }
        public string dealer_name { get; set; }
        public string dealer_mobile { get; set; }
        public int dealer_id { get; set; }
        public string contact_mobile { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public CarModel car { get; set; }
        public List<TypeOfServiceModel> type_of_service { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string appt_date { get; set; }
        public string appt_time { get; set; }
        public string date { get; set; }

        public bool is_pickup_service { get; set; }
        public string location_detail { get; set; }
        public string pickup_address { get; set; }
        public string pickup_date { get; set; }
        public string pickup_time { get; set; }
    }



    public class AppointmentDetailDataForSendEmail
    {
        public AppointmentDetailDataForSendEmail_CarOwner car_owner_data { get; set; }
        public AppointmentDetailDataForSendEmail_Appointment appointment_data { get; set; }
    }

    public class AppointmentDetailDataForExportExcel
    {
        public string code { get; set; }
        public string dealer_name { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string contact_mobile { get; set; }
        public string plate_number { get; set; }
        public string car_model { get; set; }
        public string type_of_service { get; set; }
        public string remark { get; set; }
        public string appt_date { get; set; }
        public string appt_time { get; set; }
        public string branch_name { get; set; }

        public bool is_pickup_service { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string location_detail { get; set; }
        public string pickup_date { get; set; }
        public string pickup_time_id { get; set; }
        public string pickup_address { get; set; }
    }


    public class AppointmentDetailDataForSendEmail_CarOwner
    {
        public int service_appointment_type { get; set; }
        public int id { get; set; }
        public string code { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string contact_mobile { get; set; }
        public string vin { get; set; }
        public string plate_number { get; set; }
        public string car_model { get; set; }
        public string type_of_service { get; set; }
        public string dealer_name { get; set; }
        public string remark { get; set; }
        public string created_date { get; set; }
        public string appt_date { get; set; }
        public string appt_time { get; set; }
        public string call_center_email { get; set; }
        public string tcap_call_center_email { get; set; }

    }

    public class AppointmentDetailDataForSendEmail_Appointment
    {
        public int service_appointment_type { get; set; }
        public int id { get; set; }
        public string code { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string contact_mobile { get; set; }
        public string vin { get; set; }
        public string plate_number { get; set; }
        public string car_model { get; set; }
        public string type_of_service { get; set; }
        public string dealer_name { get; set; }
        public string branch_name { get; set; }

        public string remark { get; set; }
        public string created_date { get; set; }
        public string appt_date { get; set; }
        public string appt_time { get; set; }
        public string call_center_email { get; set; }
        public string tcap_call_center_email { get; set; }

        public bool is_pickup_service { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string location_detail { get; set; }
        public string pickup_date { get; set; }
        public string pickup_time_id { get; set; }
        public string pickup_address { get; set; }
    }
}