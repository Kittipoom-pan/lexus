using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class PreferenceChoiceModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon_image_url { get; set; }
        public bool is_optional { get; set; }
        public string optional_text { get; set; }
        public string optional_header { get; set; }

    }
}