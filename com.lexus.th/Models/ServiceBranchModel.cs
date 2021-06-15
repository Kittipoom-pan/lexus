using com.lexus.th.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceBranchModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceBranchData data { get; set; }
    }
    public class _ServiceBranchData
    {
        public List<BranchModel> branches { get; set; }
    }
}