using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ArticleModel
    {
        public int id { get; set; }
        public string topic { get; set; }
        public string topic_date { get; set; }
        public string topic_url { get; set; }
        public List<string> images { get; set; }
    }
}