using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    [Serializable]
    public class ResponseBaseModel
    {
        public ResponseBaseModel()
        {

        }
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }

    }
}