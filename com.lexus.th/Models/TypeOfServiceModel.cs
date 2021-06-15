using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class TypeOfServiceModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<TypeOfServiceDetail> type_of_service_detail { get; set; }
    }

    public class TypeOfServiceDetail
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}