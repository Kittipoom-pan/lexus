using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAuthorizeDealerModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTime ts { get; set; }
        public ServiceAuthorizeDealerData data { get; set; }
    }


    public class ServiceAuthorizeDealerData
    {
        public List<AuthorizeServiceModel> dealer_groups { get; set; }
    }


    public class AuthorizeServiceModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<GeoDealerModel> geos { get; set; }
    }

    public class GeoDealerModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<DealerModel> dealers { get; set; }
    }  
}