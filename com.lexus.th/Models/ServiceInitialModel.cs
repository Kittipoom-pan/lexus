using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceInitialModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceInitialData data { get; set; }
    }
    public class _ServiceInitialData
    {
        public InitialModel initial { get; set; }
    }
}