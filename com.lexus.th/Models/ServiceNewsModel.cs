using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceNewsModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceNewsData data { get; set; }
    }
    public class _ServiceNewsData
    {
        public NewsModel news { get; set; }
    }
}