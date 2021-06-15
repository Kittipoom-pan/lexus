using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAddPreferenceModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
    }

    public class Preference
    {
        public int preference_id { get; set; }
        public List<PreferenceChoice> preference_choice { get; set; }
    }
    public class PreferenceChoice
    {
        public int choice_id { get; set; }
        public string choice_optinal_text { get; set; }
    }
}