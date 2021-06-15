using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAppointmentModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTimeOffset ts { get; set; }
        public AppointmentHistoryData data { get; set; }
    }
    public class AppointmentHistoryData
    {
        public List<_ServiceAppointmentHistoryDetailData> service_appointment_datas { get; set; }
    }
    public class _ServiceAppointmentData
    {
        //public List<AppointmentModel> appointments { get; set; }
        public int id { get; set; }
        public string name { get; set; }

    }

    public  class _ServiceAppointmentHistoryDetailData
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
        public string appointment_call_center { get; set; }

        public bool is_pickup_service { get; set; }
        public string location_detail { get; set; }
        public string pickup_address { get; set; }
        public string pickup_date { get; set; }
        public string pickup_time { get; set; }

    }

   // public class _ServiceAppointmentDetailData
    //{
    //    public int id { get; set; }
    //    public string dealer_name { get; set; }
    //    public string dealer_mobile { get; set; }
    //    public int dealer_id { get; set; }
    //    public string contact_mobile { get; set; }
    //    public string type { get; set; }
    //    public string status { get; set; }
    //    public string remark { get; set; }
    //    public CarModel car { get; set; }
    //    public List<TypeOfServiceModel> type_of_service { get; set; }
    //    public string f_name { get; set; }
    //    public string l_name { get; set; }
    //    public string appt_date { get; set; }
    //    public string appt_time { get; set; }
    //    public string date { get; set; }
    //}

  

   
}