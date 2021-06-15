using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceEventQuestionModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceEventQuestionData data { get; set; }
    }

    public class _ServiceEventQuestionData
    {
        public List<_EventQuestionData> car_owner { get; set; }
        public List<_EventQuestionData> representative { get; set; }
        public List<_EventQuestionData> follower_car_owner { get; set; }
        public List<_EventQuestionData> follower_representative { get; set; }
    }


    public class _EventQuestionData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string answer_type { get; set; }
        public List<Choices> choices { get; set; }
    }

    public class Choices
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool is_optional { get; set; }
    }

    public class EventAnswer
    {
        public int question_id { get; set; }
        public int answer_id { get; set; }
        public string text { get; set; }
    }
}