using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAllBannerModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceAllBannerData data { get; set; }
    }
    public class _ServiceAllBannerData
    {
        public List<BannerModel> banners { get; set; }
    }
    public class BannerModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string type { get; set; }
        public int action_id { get; set; }
        public int order { get; set; }
    }
}