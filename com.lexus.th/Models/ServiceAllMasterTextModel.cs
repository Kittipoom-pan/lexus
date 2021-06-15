using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAllMasterTextModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceAllMasterTextModel data { get; set; }
    }
    public class _ServiceAllMasterTextModel
    {
        public List<MasterText> master_text { get; set; }
    }
    public class MasterText
    {
        public int id { get; set; }
        public string content { get; set; }
        public bool is_special_function { get; set; }
        public string function_name { get; set; }

        public void loadMasterText(DataRow dr)
        {
            id = int.Parse(dr["id"].ToString());
            content = dr["content"].ToString();
            is_special_function = bool.Parse(dr["is_special_function"].ToString());
            function_name = dr["function_name"].ToString();
        }
    }
}