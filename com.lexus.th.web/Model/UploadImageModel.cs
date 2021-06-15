using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    [Serializable]
    public class UploadImageModel
    {
        public string ID { get; set; }
        public string ParrentID { get; set; }
        public string Type { get; set; }
        public string Page { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string OriginalFileName { get; set; }
        public string Status { get; set; }

    }
}