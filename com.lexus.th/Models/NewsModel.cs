using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class NewsModel
    {
        public int id { get; set; }
        public List<string> images { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string date_mmm { get; set; }
        public string date_dd { get; set; }
        public string desc { get; set; }
    }
}