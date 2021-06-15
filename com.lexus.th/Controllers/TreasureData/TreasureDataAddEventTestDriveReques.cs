using AppLibrary.Database;
using System;
using System.Runtime.Serialization;

namespace com.lexus.th
{

    [DataContract]
    public class TreasureDataAddEventTestDriveReques
    {
        [DataMember]
        public string member_id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string surname { get; set; }

        [DataMember]
        public string contact_no { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string interested_model { get; set; }

        [DataMember]
        public string dealer { get; set; }

        [DataMember]
        public string buying_plan { get; set; }

        [DataMember]
        public string remark { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public string datetime { get; set; }
    }
}