using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.Models
{
    public class BranchModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public bool is_pickup_service { get; set; }
    }
}