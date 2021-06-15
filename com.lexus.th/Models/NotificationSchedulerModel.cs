using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace com.lexus.th.Models
{
    public class NotificationSchedulerModel
    {
            public string id { get; set; }
            public string schedule_date { get; set; }

        public DateTime scheduleDate
        {
            get
            {
                DateTime date = DateTime.ParseExact(schedule_date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                return date;
            }
        }
    }
}