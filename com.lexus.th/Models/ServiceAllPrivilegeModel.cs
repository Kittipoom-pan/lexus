using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAllPrivilegeModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceAllPrivilegeData data { get; set; }
    }
    public class _ServiceAllPrivilegeData
    {
        public List<PrivilegeModel> privileges { get; set; }
    }
}