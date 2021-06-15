using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceYearOfPurchaseModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTime ts { get; set; }
        public _YearOfPurchaseModel data { get; set; }
    }

    public class _YearOfPurchaseModel
    {
        public List<YearOfPurchaseModel> year_of_purchases { get; set; }
    }

    public class YearOfPurchaseModel
    {
        public int id { get; set; }
        public string value { get; set; }
    }

}