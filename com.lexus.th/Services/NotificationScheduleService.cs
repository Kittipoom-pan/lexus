using AppLibrary.Database;
using com.lexus.th.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th.Services
{
    public class NotificationScheduleService
    {
        private string conn;
        enum Scheduler_status
        {
            in_queue, sending, sended
        }
        public NotificationScheduleService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<bool> GetNotificationSchedule()
        {
            try
            {
                APIService api = new APIService();
                List<NotificationSchedulerModel> schedulers = await GetNotiSchedule();
                if (schedulers.Count == 0)
                {
                    return true;
                }
                foreach (NotificationSchedulerModel scheduler in schedulers)
                {
                    UpdateNotificationScheduler(scheduler.id, Scheduler_status.sending);
                    var notifications = await GetNotificationSchedule(scheduler.id);

                    foreach (NotificationSendModel notification in notifications)
                    {
                        var result = await SendPush(notification);
                        api.UpdateNotificationSended(notification.id, result.Message, result.Success, true);
                    }
                    UpdateNotificationScheduler(scheduler.id, Scheduler_status.sended);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        private async Task<List<NotificationSchedulerModel>> GetNotiSchedule()
        {
            List<NotificationSchedulerModel> schedulers = new List<NotificationSchedulerModel>();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                                SELECT id, convert(varchar, schedule_date, 120) AS schedule_date
                                FROM [dbo].[notification_schedule] 
                                WHERE status = 'in_queue'
                                ANd schedule_date < DATEADD(HOUR, 7, GETDATE());";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            NotificationSchedulerModel scheduler = new NotificationSchedulerModel()
                            {
                                id = dr["id"] != DBNull.Value ? dr["id"].ToString() : "",
                                schedule_date = dr["schedule_date"] != DBNull.Value ? dr["schedule_date"].ToString() : ""

                            };
                            schedulers.Add(scheduler);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return schedulers;
        }

        private async Task<List<NotificationSendModel>> GetNotificationSchedule(string scheduler_id)
        {
            List<NotificationSendModel> notifications = new List<NotificationSendModel>();

            try
            {
                string cmd = @"
                DECLARE @scheduler_id INT = {0}

                SELECT id,destination, sub_destination, message, title, reference_id, link_type, link_url, member_id, device_id
                FROM notification2
                WHERE notification_schedule_id = @scheduler_id AND is_sended = '0'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, scheduler_id);

                    var dt = db.GetDataTableFromCommandText(cmd);

                    foreach (DataRow dr in dt.Rows)
                    {
                        NotificationSendModel scheduler = new NotificationSendModel()
                        {
                            id = dr["id"] != DBNull.Value ? dr["id"].ToString() : "",
                            destination = dr["destination"] != DBNull.Value ? dr["destination"].ToString() : "",
                            sub_destination = dr["sub_destination"] != DBNull.Value ? dr["sub_destination"].ToString() : "",
                            message = dr["message"] != DBNull.Value ? dr["message"].ToString() : "",
                            title = dr["title"] != DBNull.Value ? dr["title"].ToString() : "",
                            reference_id = dr["reference_id"] != DBNull.Value ? dr["reference_id"].ToString() : "",
                            link_type = dr["link_type"] != DBNull.Value ? dr["link_type"].ToString() : "",
                            link_url = dr["link_url"] != DBNull.Value ? dr["link_url"].ToString() : "",
                            member_id = dr["member_id"] != DBNull.Value ? dr["member_id"].ToString() : "",
                            device_id = dr["device_id"] != DBNull.Value ? dr["device_id"].ToString() : ""
                        };
                        notifications.Add(scheduler);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notifications;
        }

        private async void UpdateNotificationScheduler(string id, Scheduler_status status)
        {
            try
            {
                string cmd = @"
                                DECLARE @status NVARCHAR(50) = N'{0}'
                                DECLARE @id INT = N'{1}'

                                UPDATE notification_schedule
                                SET status = @status
                                WHERE id = @id";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, status, id);

                    var dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<ServiceModel> SendPush(NotificationSendModel model)
        {
            ServiceModel firebase = new ServiceModel();
            APIService api = new APIService();
            string destination, sub_destination, message, title, reference_id, link_type, link_url, member_id, device_id = string.Empty;

            string notiID = model.id;
            destination = model.destination;
            sub_destination = model.sub_destination;
            message = model.message;
            title = model.title;
            reference_id = model.reference_id;
            link_type = model.link_type;
            link_url = model.link_url;
            member_id = model.member_id;
            device_id = model.device_id;

            #region All
            if (destination == "All")
            {
                string toios = "/topics/iOS";
                var responseIOS = api.PostFirebase(APIService.PushType.Topic, toios, message, title, reference_id, link_type, link_url);

                string toandroid = "/topics/Android";
                var responseAndroid = api.PostFirebaseAndroid(APIService.PushType.Topic, toandroid, message, title, reference_id, link_type, link_url);

                firebase.Success = responseIOS.Success && responseAndroid.Success;
                firebase.Message = responseIOS.Message + responseAndroid.Message;
            }
            #endregion
            #region Member
            else if (destination == "Member")
            {
                DataTable dt;
                if (sub_destination == "Group Member ID")
                {
                    //get member_list
                    dt = api.GetMemberList(notiID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string device_token = dr["device_token"].ToString();
                            string device_Type = dr["os_type"].ToString();
                            firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                        }
                    }
                }
                else
                {
                    DataTable dtMember = api.GetDeviceToken(member_id);
                    if (dtMember.Rows.Count > 0)
                    {
                        DataRow drMember = dtMember.Rows[0];
                        string device_token = drMember["DEVICE_TOKEN"].ToString();
                        string device_Type = drMember["DEVICE_TYPE"].ToString();
                        firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                    }
                }
            }
            #endregion

            #region Device
            else if (destination == "Device")
            {
                DataTable dt;
                if (sub_destination == "Group Device")
                {
                    dt = api.GetMemberList(notiID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string device_token = dr["device_token"].ToString();
                            string device_Type = dr["os_type"].ToString();
                            firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                        }
                    }
                }
                else
                {
                    DataTable dtDevice = api.GetDeviceTokenByDevice(device_id);
                    if (dtDevice.Rows.Count > 0)
                    {
                        DataRow drDevice = dtDevice.Rows[0];
                        string device_token = drDevice["DEVICE_TOKEN"].ToString();
                        string device_Type = drDevice["DEVICE_TYPE"].ToString();
                        firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                    }
                }
            }
            #endregion

            #region Mobile OS
            else if (destination == "Mobile OS")
            {
                string to = "";
                if (sub_destination == "Android")
                {
                    to = "/topics/Android";
                    firebase = api.PostFirebaseAndroid(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url);
                }
                else
                {
                    to = "/topics/iOS";
                    firebase = api.PostFirebase(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url);
                }
            }
            #endregion

            return firebase;
        }
    }
}