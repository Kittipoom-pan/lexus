using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class GeneralModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public GeneralDetailModel detail { get; set; }
    }

    public class GeneralDetailModel
    {
        public int id { get; set; }
        public string name { get; set; }
        List<_GeneralDetailData> _GeneralDetailDatas { get; set; }
    }

    public class _GeneralDetailData
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}