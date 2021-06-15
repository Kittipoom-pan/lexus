using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class AppointmentModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string PlateNo { get; set; }
        public string DealerId { get; set; }
        public string ConfirmDate { get; set; }
        public string ConfirmTime { get; set; }
        public string IsCancel { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; }
        public string CallCenterRemark { get; set; }
        public string StatusId { get; set; }
        public string VehicleNo { get; set; }

        public string PickupLatitude { get; set; }
        public string PickupLongitude { get; set; }
        public string PickupAddress { get; set; }
        public string PickupLocationDetail { get; set; }
        public string PickupDate { get; set; }
        public string PickupTimeId { get; set; }

    }
}