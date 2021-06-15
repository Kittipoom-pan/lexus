using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceTestDriveCreateModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public DateTimeOffset ts { get; set; }
        public TestDriveCreateData data { get; set; }
    }

    public class TestDriveCreateData
    {
        public int id { get; set; }
    }

   
}