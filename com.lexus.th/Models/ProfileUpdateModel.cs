using MultipartDataMediaFormatter.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.Models
{
    public class ProfileUpdateModel
    {
        //public HttpPostedFile image { get; set; }
        public string image { get; set; }
        public string token { get; set; }
        public string lang { get; set; }
        public string name { get; set; }
    }
}