using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class PreferenceModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool is_require_answer { get; set; }
        public List<PreferenceChoiceModel> preference_choice { get; set; }
    }
}