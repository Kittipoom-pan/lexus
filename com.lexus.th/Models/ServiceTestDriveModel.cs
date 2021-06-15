using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceTestDriveModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTimeOffset ts { get; set; }
        public TestDriveHistoryData data { get; set; }
    }

    public class TestDriveHistoryData
    {
        public List<_ServiceTestDriveHistoryDetailData> test_drive_datas { get; set; }
    }

    public class _ServiceTestDriveHistoryDetailData
    {
        public int id { get; set; }
        public string dealer_name { get; set; }
        public string dealer_mobile { get; set; }
        public int dealer_id { get; set; }
        public string contact_mobile { get; set; }
        public string  email { get; set; }
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
        public string test_drive_call_center { get; set; }
    }
}