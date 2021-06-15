using AppLibrary.Database;
using System;
using System.Runtime.Serialization;

namespace com.lexus.th
{

    [DataContract]
    public class TreasureDataAddEventServiceAppointmentRequest
    {
        [DataMember]
        public string member_id { get; set; }

        [DataMember]
        public string user_type { get; set; }

        [DataMember]
        public string own_mobile { get; set; }

        [DataMember]
        public string own_email { get; set; }

        [DataMember]
        public string contact_mobile { get; set; }

        [DataMember]
        public string contact_email { get; set; }

        [DataMember]
        public string plate_no { get; set; }

        [DataMember]
        public string car_model { get; set; }

        [DataMember]
        public string service_type { get; set; }

        [DataMember]
        public string dealer { get; set; }

        [DataMember]
        public string service_date { get; set; }

        [DataMember]
        public string service_time { get; set; }

        [DataMember]
        public string remark { get; set; }

        [DataMember]
        public string status { get; set; }
    }
}