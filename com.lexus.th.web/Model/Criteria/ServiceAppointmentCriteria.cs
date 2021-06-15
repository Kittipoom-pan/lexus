using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class ServiceAppointmentCriteria
    {
   
        public string DealerId { get; set; }
        public string TypeOfServiceId { get; set; }
        public string AppointmentCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string Status { get; set; }
        public string RegisDateFrom { get; set; }
        public string RegisDateTo { get; set; }
        public string AppointmentDateFrom { get; set; }
        public string AppointmentDateTo { get; set; }
        public string IsPickupService { get; set; }

    }
}