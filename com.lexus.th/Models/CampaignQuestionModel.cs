using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class CampaignQuestionModel : ResponseBaseModel
    {
        public CampaignQuestionModel() { questions = new List<QuestionAnswer>(); }
        public List<QuestionAnswer> questions { get; set; }

    }
    public class QuestionAnswer
    {
        public int question_id { get; set; }
        public string question { get; set; }
        public string question_key { get; set; }
        public int seq { get; set; }
        public bool is_optional { get; set; }
        public int answer_type { get; set; }
        public List<Answer> answers { get; set; }
    }
    public class Answer
    {
        public int answer_id { get; set; }
        public string answer { get; set; }
        public int seq { get; set; }
    }
}