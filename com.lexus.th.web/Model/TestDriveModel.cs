using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class TestDriveModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string PreferredModelId { get; set; }
        public string DealerId { get; set; }
        public string PurchasePlanId { get; set; }
        public string ConfirmDate { get; set; }
        public string ConfirmTime { get; set; }
        public string IsCancel { get; set; }
        public string CancelDate { get; set; }
        public string CancelReason { get; set; }
        public string CallCenterRemark { get; set; }
        public string StatusId { get; set; }
    }
}