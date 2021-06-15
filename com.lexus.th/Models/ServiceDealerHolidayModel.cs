using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceDealerHolidayModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTime ts { get; set; }
        public ServiceDealerHolidayData data { get; set; }
    }

    public class ServiceDealerHolidayData
    {
        public List<string> dealer_holidays { get; set; }
        public int dealer_duration_min { get; set; }
        public int dealer_duration_max { get; set; }
    }


    public class ServiceDealerWorkingTimeModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTime ts { get; set; }
        public ServiceDealerWorkingTimeData data { get; set; }
    }

    public class ServiceDealerWorkingTimeData
    {
        public List<ServiceDealerWorkingTimeData_> times { get; set; }
    }
    public class ServiceDealerWorkingTimeData_ 
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}