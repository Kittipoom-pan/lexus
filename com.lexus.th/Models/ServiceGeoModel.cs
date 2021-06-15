using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceGeoModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceGeoData data { get; set; }
    }
    public class _ServiceGeoData
    {
        public List<GeoModel> geos { get; set; }
    }
}