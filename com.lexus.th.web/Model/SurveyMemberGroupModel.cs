using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    [Serializable]
    public class SurveyMemberGroupModel
    {
        public string Id { get; set; }
        public string Survey_Id { get; set; }
        public string MemberId { get; set; }
        //public string DeviceId { get; set; }
     
    }
  
}