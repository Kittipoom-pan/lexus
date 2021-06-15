using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceTypeOfServiceModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTimeOffset ts { get; set; }
        public ServiceTypeOfServiceData data { get; set; }
    }
    public class ServiceTypeOfServiceData
    {
        public List<_ServiceTypeOfServiceData> type_of_service_data { get; set; }
    }

    public class _ServiceTypeOfServiceData
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<_ServiceTypeOfServiceDetailData> type_of_service_detail { get; set; }
    }

    public class _ServiceTypeOfServiceDetailData
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}