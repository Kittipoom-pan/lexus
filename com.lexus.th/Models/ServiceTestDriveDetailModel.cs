using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceTestDriveDetailModel
    {

        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTimeOffset ts { get; set; }
        public TestDriveDetailData data { get; set; }
    }

    public class TestDriveDetailData
    {
        public _ServiceTestDriveDetailData test_drive_data { get; set; }
    }

    public class _ServiceTestDriveDetailData
    {
        public int id { get; set; }
        public string dealer_name { get; set; }
        public string dealer_mobile { get; set; }
        public int dealer_id { get; set; }
        public string contact_mobile { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public string car_model { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string booking_date { get; set; }
        public string booking_time { get; set; }
        public string date { get; set; }
        public string purchase_model { get; set; }
        public string purchase_plan { get; set; }
        public string remark { get; set; }
    }

    public class TestDriveDetailForSendEmail
    {
        public int id { get; set; }
        public string code { get; set; }
        public string create_date { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string contact_number { get; set; }
        public string email { get; set; }
        public string preferred_model { get; set; }
        public string branch_name { get; set; }

        public string dealer { get; set; }
        public string car_purchase_plan { get; set; }
        public string call_center_email { get; set; }
        public string tcap_call_center_email { get; set; }
        public string remark { get; set; }
    }

    public class TestDriveDetailDataForExportExcel
    {
        public string code { get; set; }
        public string dealer_name { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string contact_mobile { get; set; }
        public string email { get; set; }
        public string preferred_model { get; set; }
        public string remark { get; set; }
        public string purchase_plan { get; set; }
        public string branch_name { get; set; }
    }
}
