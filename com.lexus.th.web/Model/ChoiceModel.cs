using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    [Serializable]
    public class ChoiceModel
    {
        public int QuestionRuleId { get; set; }
        public string ChoiceId { get; set; }
        public string ChoiceTH { get; set; }
        public string ChoiceEN { get; set; }
        public bool IsChecked { get; set; }
        public string Status { get; set; }
    }
}