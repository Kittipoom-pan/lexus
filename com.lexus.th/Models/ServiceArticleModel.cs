using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceArticleModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceArticleData data { get; set; }
    }
    public class _ServiceArticleData
    {
        public List<ArticleModel> article { get; set; }
    }

    public class ServiceArticleDetailModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceArticleDetailData data { get; set; }
    }
    public class _ServiceArticleDetailData
    {
        public ArticleModel article { get; set; }
    }
}