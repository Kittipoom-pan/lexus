using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceCarPurchasePlanModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTimeOffset ts { get; set; }
        public ServiceCarPurchasePlanData data { get; set; }
    }

    public class ServiceCarPurchasePlanData
    {
        public List<_ServiceCarPurchasePlanData> car_purchase_plan_datas { get; set; }
    }

    public class _ServiceCarPurchasePlanData
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}