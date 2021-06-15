using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class BookingModel
    {
        public string Id { get; set; }
        public string TitleEN { get; set; }
        public string TitleTH { get; set; }
        public string DescEN { get; set; }
        public string DescTH { get; set; }
        public string ConditionEN { get; set; }
        public string ConditionTH { get; set; }
        public string RegPeriodEN { get; set; }
        public string RegPeriodTH { get; set; }
        public string RegStart { get; set; }
        public string RegEnd { get; set; }
        public string DisplayStart { get; set; }
        public string DisplayEnd { get; set; }
        public string ThankyouMessageEN { get; set; }
        public string ThankyouMessageTH { get; set; }
        public string CodeMessageEN { get; set; }
        public string CodeMessageTH { get; set; }
        public string IsActive { get; set; }      
        public string User { get; set; }

        //Additional Referral
        public string IsRequirePlateNo { get; set; }
        public string PreferredModelIds { get; set; }
        public string FollowingBy { get; set; }
        public string MaxUsed { get; set; }
    }
}