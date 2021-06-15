using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class CampaignDetailResponseModel : ResponseBaseModel
    {
        public CampaignDetailResponseModel() { data = new CampaignDetail(); }
        public CampaignDetail data { get; set; }

    }
    public class CampaignDetail
    {
        public int id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string condition { get; set; }
        public string reg_period { get; set; }
        public bool allow_reg { get; set; }
        public bool is_expired { get; set; }
        public bool is_registered { get; set; }
        public bool is_full { get; set; }
        public bool is_required_plate_no { get; set; }
        public bool question_answer_more { get; set; }
        public List<PreferredModel> preferred_model { get; set; }
        //public string reg_end { get; set; }
        public List<string> images { get; set; }
        public List<ReferralPlateModel> plate_numbers { get; set; }
    }
    public class PreferredModel
    {
        public string id { get; set; } 
        public string name { get; set; }
    }
}