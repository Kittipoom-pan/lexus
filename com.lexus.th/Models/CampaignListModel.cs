using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    [Serializable]
    public class CampaignListModel : ResponseBaseModel
    {
        public CampaignListModel() { data = new CampaignList(); }
        public CampaignList data { get; set; }

    }
    public class CampaignList
    {
        public List<CampaignHeader> campaigns { get; set; }
    }
    public class CampaignHeader
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<string> images { get; set; }
    }
}