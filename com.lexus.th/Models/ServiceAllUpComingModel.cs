using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAllUpComingModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceAllUpComingData data { get; set; }
    }
    public class _ServiceAllUpComingData
    {
        public List<UpcomingModel> Upcoming { get; set; }
        public List<NewsModel> news { get; set; }
    }
    public class UpcomingModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<string> images { get; set; }
        public string type { get; set; }
        public int action_id { get; set; }
        public int order { get; set; }
        public string create_date { get; set; }
    }
    //public class UpcomingNewsModel
    //{
    //    public int id { get; set; }
    //    public string title_name { get; set; }
    //    public List<string> image { get; set; }
    //    public string type { get; set; }
    //    public int action_id { get; set; }
    //    public int order { get; set; }
    //    public string start_date { get; set; }
    //    public string end_date { get; set; }
    //    public int image_gallery_count { get; set; }
    //}
}