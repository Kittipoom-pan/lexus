using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    [Serializable]
    public class QuestionRuleModel
    {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string PreviousQuestionId { get; set; }
        public string ChoiceId { get; set; }
        public int QuestionType { get; set; }
        public string QuestionTH { get; set; }
        public string QuestionEN { get; set; }
        public List<ChoiceModel> Choices { get; set; } = new List<ChoiceModel>();
        public string Status { get; set; }
    }
    [Serializable]
    public class QuestionRuleRPModel
    {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string PreviousQuestionId { get; set; }
        public int QuestionType { get; set; }
        public string QuestionTH { get; set; }
        public string QuestionEN { get; set; }
        public List<ChoiceModel> Choices { get; set; } = new List<ChoiceModel>();
        public List<ChoiceModel> QuestionRuleChoices { get; set; } = new List<ChoiceModel>();
        public string Status { get; set; }
    }
}