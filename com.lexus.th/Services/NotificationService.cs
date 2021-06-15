using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
//using Quartz;
//using Quartz.Impl;

namespace com.lexus.th
{
    public class NotificationService
    {
        private string conn;

        public NotificationService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceNotificationModel> GetScreenData(string v, string p)
        {
            ServiceNotificationModel value = new ServiceNotificationModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceNotificationData();
                    value.data.notification = GetNotification(p, "");

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceNotificationModel> GetScreenDataNew(string v, string p, string lang, string token)
        {
            ServiceNotificationModel value = new ServiceNotificationModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceNotificationData();
                    value.data.notification = GetNotification(p, token);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private List<NotificationModel> GetNotification(string platform, string token)
        {
            List<NotificationModel> list = new List<NotificationModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
//                    string cmd = @"
//SELECT id, notify_title, notify_message, notify_type, reference_id, create_date, link_url
//FROM NOTIFICATION
//WHERE (device_type = 'All' OR device_type LIKE '%{0}%') AND MONTH(create_date) = MONTH(DATEADD(HOUR, 7, GETDATE()))
//order by create_date desc";
                    string cmd = @"
                           DECLARE @DEVICE NVARCHAR(200) = N'{0}'
                           DECLARE @TOKEN NVARCHAR(200) = N'{1}'
                           DECLARE @dateremaining INT = (SELECT [data_config] FROM system_config  where [name] = N'push_remaining_day')
                           
                            SELECT n.id, n.title AS notify_title, 
                           	n.message AS notify_message, 
														CASE 
														WHEN n.link_type = 'Notification_Center' AND n.is_service_reminder = 1 
														THEN 'Service_Reminder' 
														ELSE n.link_type END AS notify_type, 
														COALESCE(n.reference_id, 0) reference_id, 
                               COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) create_date, n.link_url
                           FROM notification2 n 
                           WHERE   CONVERT(NVARCHAR(20),  DATEADD(day,  -@dateremaining, (getdate())), 120) <=   CONVERT(NVARCHAR(20), n.create_date, 120)  and (n.destination = 'All' OR LOWER(n.sub_destination) = @DEVICE) 
                             AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date)  < DATEADD(HOUR, 7, GETDATE())
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) BETWEEN DATEADD(HOUR, 7, DATEADD(month, -6, GETDATE())) and DATEADD(HOUR, 7, GETDATE())
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) > (SELECT CREATE_DT 
                           											 FROM T_CUSTOMER tc 
                           													LEFT JOIN T_CUSTOMER_TOKEN tct ON tc.MEMBERID = tct.MEMBERID AND tct.TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE())
                           											 WHERE tct.TOKEN_NO = @TOKEN) 

                           UNION
                           SELECT n.id, n.title AS notify_title, 
                           	n.message AS notify_message, 
														CASE 
														WHEN n.link_type = 'Notification_Center' AND n.is_service_reminder = 1 
														THEN 'Service_Reminder' 
														ELSE n.link_type END AS notify_type,
														COALESCE(n.reference_id, 0) reference_id, 
                               COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) create_date, n.link_url
                           FROM notification2 n 
                           	LEFT JOIN T_CUSTOMER_TOKEN ct ON n.member_id = ct.MEMBERID
                           WHERE n.destination = 'Member' and CONVERT(NVARCHAR(20),  DATEADD(day,  -@dateremaining, (getdate())), 120) <=   CONVERT(NVARCHAR(20), n.create_date, 120) 
                               AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date)  < DATEADD(HOUR, 7, GETDATE())
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) BETWEEN DATEADD(HOUR, 7, DATEADD(month, -6, GETDATE())) and DATEADD(HOUR, 7, GETDATE())
                           	AND ct.TOKEN_NO = @TOKEN
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) > (SELECT CREATE_DT 
                           											 FROM T_CUSTOMER tc 
                           													LEFT JOIN T_CUSTOMER_TOKEN tct ON tc.MEMBERID = tct.MEMBERID AND tct.TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE())
                           											 WHERE tct.TOKEN_NO = @TOKEN)
                           UNION
                           SELECT n.id, n.title AS notify_title, 
                           	n.message AS notify_message, 	
														CASE 
														WHEN n.link_type = 'Notification_Center' AND n.is_service_reminder = 1 
														THEN 'Service_Reminder' 
														ELSE n.link_type END AS notify_type,
														COALESCE(n.reference_id, 0) reference_id, 
                               COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) create_date, n.link_url
                           FROM notification2 n 
                           	LEFT JOIN T_DEVICE ct ON n.device_id = ct.DEVICE_ID
                           WHERE n.destination = 'Device' and CONVERT(NVARCHAR(20),  DATEADD(day,  -@dateremaining, (getdate())), 120) <=   CONVERT(NVARCHAR(20), n.create_date, 120) 
                               AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date)  < DATEADD(HOUR, 7, GETDATE())
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) BETWEEN DATEADD(HOUR, 7, DATEADD(month, -6, GETDATE())) and DATEADD(HOUR, 7, GETDATE())
                           	AND ct.TOKEN_NO = @TOKEN
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) > (SELECT CREATE_DT 
                           											 FROM T_CUSTOMER tc 
                           													LEFT JOIN T_CUSTOMER_TOKEN tct ON tc.MEMBERID = tct.MEMBERID AND tct.TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE())
                           											 WHERE tct.TOKEN_NO = @TOKEN)
                           UNION



                           SELECT n.id, n.title AS notify_title, 
                           	n.message AS notify_message, 
														CASE 
														WHEN n.link_type = 'Notification_Center' AND n.is_service_reminder = 1 
														THEN 'Service_Reminder' 
														ELSE n.link_type END AS notify_type,
														COALESCE(n.reference_id, 0) reference_id, 
                               COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) create_date, n.link_url
                           FROM notification2 n 
                           	LEFT JOIN notification2_device nd ON n.id = nd.notification2_id
                           	LEFT JOIN T_CUSTOMER_TOKEN ct ON nd.member_id = ct.MEMBERID
                           WHERE n.sub_destination = 'Group Member ID' and CONVERT(NVARCHAR(20),  DATEADD(day,  -@dateremaining, (getdate())), 120) <=   CONVERT(NVARCHAR(20), n.create_date, 120) 
                               AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date)  < DATEADD(HOUR, 7, GETDATE())
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) BETWEEN DATEADD(HOUR, 7, DATEADD(month, -6, GETDATE())) and DATEADD(HOUR, 7, GETDATE())
                           	AND ct.TOKEN_NO = @TOKEN
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) > (SELECT CREATE_DT 
                           											 FROM T_CUSTOMER tc 
                           													LEFT JOIN T_CUSTOMER_TOKEN tct ON tc.MEMBERID = tct.MEMBERID AND tct.TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE())
                           											 WHERE tct.TOKEN_NO = @TOKEN)




                           UNION
                           SELECT n.id, n.title AS notify_title, 
                           	n.message AS notify_message, 	
														CASE 
														WHEN n.link_type = 'Notification_Center' AND n.is_service_reminder = 1 
														THEN 'Service_Reminder' 
														ELSE n.link_type END AS notify_type, 
														COALESCE(n.reference_id, 0) reference_id, 
                               COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) create_date, n.link_url
                            FROM notification2 n
                           	LEFT JOIN notification2_device nd ON n.id = nd.notification2_id
                           	LEFT JOIN T_DEVICE ct ON nd.device_id = ct.DEVICE_ID
                            WHERE n.sub_destination = 'Group Device' and CONVERT(NVARCHAR(20),  DATEADD(day,  -@dateremaining, (getdate())), 120) <=   CONVERT(NVARCHAR(20), n.create_date, 120) 
                            AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date)  < DATEADD(HOUR, 7, GETDATE())
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) BETWEEN DATEADD(HOUR, 7, DATEADD(month, -6, GETDATE())) and DATEADD(HOUR, 7, GETDATE())
                           	AND ct.TOKEN_NO = @TOKEN
                           	AND COALESCE(CAST(n.send_datetime AS DATETIME), n.create_date) > (SELECT CREATE_DT 
                            FROM T_CUSTOMER tc 
                           	LEFT JOIN T_CUSTOMER_TOKEN tct ON tc.MEMBERID = tct.MEMBERID AND tct.TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE())
                            WHERE tct.TOKEN_NO = @TOKEN)
                            ORDER BY create_date DESC, id DESC";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format( cmd, platform, token)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            NotificationModel noti = new NotificationModel();
                            noti.id = int.Parse(row["id"].ToString());
                            noti.notify_title = row["notify_title"].ToString();
                            noti.notify_message = row["notify_message"].ToString();
                            noti.notify_type = row["notify_type"].ToString();
                            noti.link_url = row["link_url"].ToString();
                            noti.reference_id = Convert.ToInt32(row["reference_id"].ToString());
                            noti.create_date = (row["create_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["create_date"]).ToString("yyyy-MM-dd HH:mm:ss");

                            list.Add(noti);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public async Task<ServiceNotificationModel> GetScreenData2(string v, string p)
        {
            ServiceNotificationModel value = new ServiceNotificationModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceNotificationData();
                    value.data.notification =await GetAllNotification();

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceNotificationModel> GetScreenDataAll(string v, string p, string lang)
        {
            ServiceNotificationModel value = new ServiceNotificationModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceNotificationData();
                    value.data.notification =await GetAllNotification();

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<List<NotificationModel>> GetAllNotification()
        {
            List<NotificationModel> list = new List<NotificationModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                   
                    string cmd = @"
DECLARE @dateremaining INT = (SELECT [data_config] FROM system_config  where [name] = N'push_remaining_day');
SELECT DISTINCT notify_title, notify_message, notify_type, reference_id, create_date ,DATEADD(day, -@dateremaining, (getdate()))
FROM NOTIFICATION  where  CONVERT(NVARCHAR(20),  DATEADD(day, -@dateremaining, (getdate())), 120) <=   CONVERT(NVARCHAR(20), create_date, 120)
order by create_date desc";
                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            NotificationModel noti = new NotificationModel();
                            noti.notify_title = row["notify_title"].ToString();
                            noti.notify_message = row["notify_message"].ToString();
                            noti.notify_type = row["notify_type"].ToString();
                            noti.reference_id = Convert.ToInt32(row["reference_id"].ToString());
                            noti.create_date = row["create_date"].ToString();                    
                            list.Add(noti);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public void CheckSendNotification()
        {
            //DataTable dt = new DataTable();
            try
            {
                string notiID, destination, sub_destination, message, title, reference_id, link_type, link_url, member_id, device_id = string.Empty;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    DataTable dt = new DataTable();
                    string cmd = @"
                        SELECT id, destination, sub_destination, message, title, reference_id, link_type,
                            link_url, member_id, device_id
                        FROM notification2
                        WHERE is_schedule = 1 AND CAST(send_datetime AS DATETIME) = DATEADD(HOUR, 7, GETDATE())";
                    using (dt = db.GetDataTableFromCommandText(cmd))
                    {
                        APIService api = new APIService();

                        DataRow drNoti = dt.Rows[0];

                        notiID = drNoti["id"].ToString();
                        destination = drNoti["destination"].ToString();
                        sub_destination = drNoti["sub_destination"].ToString();
                        message = drNoti["message"].ToString();
                        title = drNoti["title"].ToString();
                        reference_id = drNoti["reference_id"].ToString();
                        link_type = drNoti["link_type"].ToString();
                        link_url = drNoti["link_url"].ToString();
                        member_id = drNoti["member_id"].ToString();
                        device_id = drNoti["device_id"].ToString();

                        #region All
                        if (destination == "All")
                        {
                            string to = "/topics/iOS";
                            api.PostFirebase(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url);

                            string toandroid = "/topics/Android";
                            api.PostFirebaseAndroid(APIService.PushType.Topic, toandroid, message, title, reference_id, link_type, link_url);
                        }
                        #endregion

                        #region Member
                        else if (destination == "Member")
                        {
                            DataTable dt2;
                            if (sub_destination == "Group Member ID")
                            {
                                //get member_list
                                dt2 = api.GetMemberList(notiID);
                                if (dt2 != null && dt2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt2.Rows)
                                    {
                                        string device_token = dr["device_token"].ToString();
                                        string device_Type = dr["os_type"].ToString();
                                        api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
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
                                    api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                                }
                            }
                        }
                        #endregion

                        #region Device
                        else if (destination == "Device")
                        {
                            DataTable dt3;
                            if (sub_destination == "Group Device")
                            {
                                dt3 = api.GetMemberList(notiID);
                                if (dt3 != null && dt3.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt3.Rows)
                                    {
                                        string device_token = dr["device_token"].ToString();
                                        string device_Type = dr["os_type"].ToString();
                                        api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
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
                                    api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                                    //firebase = api.PostFirebase(APIService.PushType.Topic, to, tbMessage.Text, tbTitle.Text, ref_id, noti_type, link_url);
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
                                api.PostFirebaseAndroid(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url);
                            }
                            else
                            {
                                to = "/topics/iOS";
                                api.PostFirebase(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}