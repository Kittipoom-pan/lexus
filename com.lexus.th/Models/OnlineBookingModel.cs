using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class OnlineBookingModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string condition { get; set; }
        public string reg_period { get; set; }
        public bool allow_reg { get; set; }
        public bool is_registered { get; set; }
        public bool is_full { get; set; }
        //public string reg_start { get; set; }
        //public string reg_end { get; set; }
        public List<string> images { get; set; }
    }

    public class OnlineBookingExportExcelModel
    {
        public string title { get; set; }
        public string dealer_name { get; set; }
        public string car_model { get; set; }
        public string test_drive { get; set; }
        public string booking_code { get; set; }
        public string call_center_email { get; set; }
        public string tcap_call_center_email { get; set; }
        public List<MeassgeSms> message { get; set; }
        public class MeassgeSms
        {
            public string msg { get; set; }
            public string contact_number { get; set; }
        }
       
        public List<QuestionAnswers> question_answer { get; set; }
        public class QuestionAnswers
        {
            public string questions { get; set; }
            public string answers { get; set; }
        }
        public CampaignRegisterModel campaignRegisterModel { get; set; }
    }
}