using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Globalization;
using Quartz;
using AppLibrary.Database;
using Quartz.Impl;

namespace com.lexus.th.web.pages
{
    public partial class zzzTest : System.Web.UI.Page
    {
        private string conn;
        private enum ModalType { Success, Error, Warning }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //BindGridData();
                DateTime ScheduleDate = new DateTime();
                DateTime DateNow = DateTime.Now;

                DataTable dt = new DataTable();
                dt = GetAllNotificationNotSend();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string NotiID = row["id"].ToString();
                        string schedule_datetime = row["send_datetime"].ToString();
                        ScheduleDate = Convert.ToDateTime(schedule_datetime);
                        //ScheduleDate = DateTime.ParseExact(schedule_datetime, "dd/MM/yyyy HH:mm", null);

                        //ScheduleDate = DateTime.SpecifyKind(ScheduleDate, DateTimeKind.Utc);

                        ISchedulerFactory schedFact = new StdSchedulerFactory();
                        // get a scheduler
                        IScheduler sched = schedFact.GetScheduler();
                        sched.Start();
                        // define the job and tie it to our HelloJob class
                        IJobDetail job = JobBuilder.Create<NotificationJob>()
                            .WithIdentity("All", NotiID)
                            .Build();
                        var difSecound = Convert.ToInt32(ScheduleDate.Subtract(DateNow).TotalSeconds);
                        ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                            .WithIdentity("trigger1", NotiID)
                            .StartAt(DateBuilder.FutureDate(difSecound, IntervalUnit.Second)) // some Date 
                            .ForJob("All", NotiID) // identify job with name, group strings
                            .Build();
                        sched.ScheduleJob(job, trigger);
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private DataTable GetAllNotificationNotSend()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        SELECT id, CAST(send_datetime AS DATETIME) send_datetime
                        FROM notification2
                        WHERE is_schedule = 1 AND CAST(send_datetime AS DATETIME) > DATEADD(HOUR, 7, GETDATE())
                        order by CAST(send_datetime AS DATETIME)";
                    using (dt = db.GetDataTableFromCommandText(cmd))
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}