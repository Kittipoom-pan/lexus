using AppLibrary.Database;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.Configuration;
using AppLibrary.Schedule;

namespace com.lexus.th
{
    public class ServiceReminderService
    {
        private string conn;
        public ServiceReminderService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async void LogServiceReminder(ServiceReminderHeader header, ServiceReminderBody body, ServiceReminderResponse response, ServiceReminderMessage noti)
        {
            try
            {
                await addLog(header, body, response, noti);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(ServiceReminderResponse, ServiceReminderMessage)> CreateResponse(ServiceReminderHeader header, ServiceReminderBody body)
        {
            ServiceReminderResponse value = new ServiceReminderResponse();
            ServiceReminderMessage noti = new ServiceReminderMessage();
            try
            {
                ServiceReminderMember data = await getDataForNoti(body);

                if (string.IsNullOrEmpty(data.Plate_no) || string.IsNullOrEmpty(data.MemberId) || string.IsNullOrEmpty(data.Message))
                {
                    var result = value.getRecordNotFound(body.nextservicefollowup.vin, body.nextservicefollowup.maintenanceItem);
                    return (result, noti);
                }

                string url = WebConfigurationManager.AppSettings["notificationURL"].ToString();

                noti.title_msg = $"เรียนเชิญนำรถเลขทะเบียน {data.Plate_no} เข้าเช็คระยะ";
                noti.detail_msg = $"เรียนเชิญ นำรถเลขทะเบียน {data.Plate_no} เข้ารับบริการเช็กระยะรอบ {data.Message} ภายในวันที่ {body.nextservicefollowup.GetDateText()} สามารถจองเข้ารับบริการได้ที่นี่ {url} ขออภัยหากท่านเข้ารับบริการแล้ว";
                noti.link_type = "Notification_Center";
                
                value.statusCode = "200";
                value.message = "Success";
                value.data = new ServiceReminderResponseData()
                {
                    vin = body.nextservicefollowup.vin,
                    MaintenanceItem = body.nextservicefollowup.maintenanceItem,
                    returnCode = "0000",
                    returnMessage = "Process completed successfully"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (value, noti);
        }

        private async Task addLog(ServiceReminderHeader header, ServiceReminderBody body, ServiceReminderResponse response, ServiceReminderMessage noti)
        {
            string headerData = JsonConvert.SerializeObject(header);
            string bodyData = JsonConvert.SerializeObject(body);
            string responseData = JsonConvert.SerializeObject(response);
            string user = header.fromSystem;
            string vin = body.nextservicefollowup.vin;
            string title = noti.title_msg;
            string message = noti.detail_msg;
            string link_type = noti.link_type;
            string schedule_date = noti.GetScheduleDate();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"INSERT INTO service_reminder_log (header, body, created_date, created_user, response,vin,title,message,link_type,schedule_date, is_sended)
                                   VALUES (N'{0}', N'{1}', DATEADD(HOUR, 7, GETDATE()), N'{2}', N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}', 0)";

                    using (DataTable dt = db.GetDataTableFromCommandText
                          (string.Format(cmd, headerData, bodyData, user, responseData, vin, title, message, link_type, schedule_date)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<ServiceReminderMember> getDataForNoti(ServiceReminderBody body)
        {
            ServiceReminderMember value = new ServiceReminderMember();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @VIN  NVARCHAR(100) = N'{0}'

SELECT MEMBERID,PLATE_NO FROM T_CUSTOMER_CAR WHERE VIN = @VIN
";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, body.nextservicefollowup.vin)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            value.MemberId = dt.Rows[0]["MEMBERID"] != DBNull.Value ? dt.Rows[0]["MEMBERID"].ToString() : "";
                            value.Plate_no = dt.Rows[0]["PLATE_NO"] != DBNull.Value ? dt.Rows[0]["PLATE_NO"].ToString() : "";
                        }
                        else
                        {
                            return value;
                        }
                    }
                }
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @MaintenanceItem NVARCHAR(100) = N'{0}'
DECLARE @MessageType  NVARCHAR(1) = N'{1}'

SELECT Message FROM service_reminder_message WHERE MaintenanceItem = @MaintenanceItem and MessageType = @MessageType
";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, body.nextservicefollowup.maintenanceItem, body.nextservicefollowup.messageType)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            value.Message = dt.Rows[0]["Message"] != DBNull.Value ? dt.Rows[0]["Message"].ToString() : "";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private string AddNotification(ServiceReminderMessage noti, string memberid)
        {
            string result = "";

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @destination  NVARCHAR(255) = N'{0}'
DECLARE @sub_destination  NVARCHAR(255) = N'{1}'
DECLARE @member_id  NVARCHAR(100) = N'{2}'
DECLARE @title  NVARCHAR(255) = N'{3}'
DECLARE @message NVARCHAR(max) = N'{4}'
DECLARE @is_link  INT = N'{5}'
DECLARE @link_type  NVARCHAR(255) = N'{6}'
DECLARE @is_schedule  INT = N'{7}'
DECLARE @schedule_date  NVARCHAR(100) = N'{8}'
DECLARE @schedule_time  NVARCHAR(100) = N'{9}'
DECLARE @USER  NVARCHAR(50) = N'{10}'
DECLARE @device_type  NVARCHAR(50) = N'{11}'
DECLARE @reference_id  INT = N'{12}'
DECLARE @scheduleDatetime  NVARCHAR(50) = N'{13}'
DECLARE @is_service_reminder  BIT = N'{14}'

INSERT INTO NOTIFICATION (notify_title, notify_message, device_type, notify_type, reference_id, create_date, is_service_reminder)
VALUES (CASE LEN(@title) WHEN 0 THEN NULL ELSE @title END,
		CASE LEN(@message) WHEN 0 THEN NULL ELSE @message END,
		CASE LEN(@device_type) WHEN 0 THEN NULL ELSE @device_type END,
		CASE LEN(@link_type) WHEN 0 THEN NULL ELSE @link_type END,
		CASE LEN(@reference_id) WHEN 0 THEN NULL ELSE @reference_id END,
		DATEADD(HOUR, 7, GETDATE()),
		CASE LEN(@is_service_reminder) WHEN 0 THEN NULL ELSE @is_service_reminder END)

INSERT INTO notification2 (destination, sub_destination, member_id, title, message, is_link, link_type, 
    is_schedule, schedule_date, schedule_time, create_date, create_by, reference_id, send_datetime, is_service_reminder, is_sended)
VALUES (CASE LEN(@destination) WHEN 0 THEN NULL ELSE @destination END,
		CASE LEN(@sub_destination) WHEN 0 THEN NULL ELSE @sub_destination END,
		CASE LEN(@member_id) WHEN 0 THEN NULL ELSE @member_id END,
		CASE LEN(@title) WHEN 0 THEN NULL ELSE @title END,
		CASE LEN(@message) WHEN 0 THEN NULL ELSE @message END,
		CASE LEN(@is_link) WHEN 0 THEN NULL ELSE @is_link END,
		CASE LEN(@link_type) WHEN 0 THEN NULL ELSE @link_type END,
		CASE LEN(@is_schedule) WHEN 0 THEN NULL ELSE @is_schedule END,
        CASE LEN(@schedule_date) WHEN 0 THEN NULL ELSE @schedule_date END,
        CASE LEN(@schedule_time) WHEN 0 THEN NULL ELSE @schedule_time END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER,
        CASE LEN(@reference_id) WHEN 0 THEN NULL ELSE @reference_id END,
        CASE LEN(@scheduleDatetime) WHEN 0 THEN NULL ELSE @scheduleDatetime END,
		CASE LEN(@is_service_reminder) WHEN 0 THEN NULL ELSE @is_service_reminder END,
        0)

select CONVERT(NVARCHAR, @@IDENTITY) AS [ID]";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, "Member", "Specific Member ID", memberid, noti.title_msg, noti.detail_msg, "0", "Notification_Center", "0"
                        , DBNull.Value, DBNull.Value, DBNull.Value, "Member", "0", DBNull.Value, "1")))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            result = Convert.ToString(dt.Rows[0]["ID"]);
                            //result = db.ExecuteScalarFromCommandText<string>(cmd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ServiceReminderDevice AddMemberList(string NotiID, DataTable MemberIDList, string MemberID)
        {
            ServiceReminderDevice notification2_device = new ServiceReminderDevice();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @notification2_id  NVARCHAR(50) = N'{0}'
DECLARE @MEMBERID  NVARCHAR(50) = N'{1}'

insert into notification2_device
(notification2_id, member_id, os_type, device_id, device_token)
select @notification2_id, t.MEMBERID, d.DEVICE_TYPE, d.DEVICE_ID, d.DEVICE_TOKEN  
from T_DEVICE d LEFT JOIN T_CUSTOMER_TOKEN t ON d.TOKEN_NO = t.TOKEN_NO
where t.TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE()) AND t.MEMBERID in (@MEMBERID) and d.TOKEN_NO <> '' and d.TOKEN_NO is not null order by d.CREATE_DATE desc

select distinct device_token, os_type from notification2_device where notification2_id = @notification2_id";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, NotiID, MemberID)))
                    {
                        string Members = "''";
                        if (MemberID == "")
                        {
                            foreach (DataRow dr in MemberIDList.Rows)
                            {
                                Members = "'" + dr[0].ToString() + "'," + Members;
                            }
                        }
                        else
                        {
                            Members = "'" + MemberID + "'";
                        }
                        if (dt.Rows.Count > 0)
                        {
                            notification2_device.device_token = dt.Rows[0]["device_token"] != DBNull.Value ? dt.Rows[0]["device_token"].ToString() : "";
                            notification2_device.device_type = dt.Rows[0]["os_type"] != DBNull.Value ? dt.Rows[0]["os_type"].ToString() : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return notification2_device;
        }

        public async void ServiceReminderSchedule()
        {
            string cmd = @"
                SELECT DISTINCT reminder.*, d.DEVICE_TOKEN, d.DEVICE_TYPE
                FROM ( 
	                SELECT DISTINCT 
	                noti.*, 
	                cr.MEMBERID,
	                (   select top(1) 
	                    CASE 
	                    WHEN ct.TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE()) THEN TOKEN_NO
	                    ELSE null
	                    END as TOKEN_NO
	                    from  T_CUSTOMER_TOKEN ct WHERE ct.MEMBERID = cr.MEMBERID order by ct.id DESC
                    ) as TOKEN_NO
                    FROM (
                        SELECT 
                        id, (vin COLLATE SQL_Latin1_General_CP1_CI_AS) as vin, title, message, link_type, is_sended
                        FROM [dbo].[service_reminder_log]
                        WHERE schedule_date is not null 
                        and (title is not null and title != '')
                        and (message is not null and message != '')
                        and (link_type is not null and link_type != '')
                        and schedule_date = CAST(GETDATE() AS DATE)
                        and (is_sended is null or is_sended = 0)
                    ) noti
                    LEFT JOIN T_CUSTOMER_CAR cr ON noti.vin = cr.VIN
                ) reminder
                LEFT JOIN T_DEVICE d ON d.TOKEN_NO = reminder.TOKEN_NO
            ";
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {
                        var service = new APIService();
                        foreach (DataRow dr in dt.Rows)
                        {
                            var id = dr["id"] != DBNull.Value ? dr["id"].ToString() : "";
                            var tokenID = dr["DEVICE_TOKEN"] != DBNull.Value ? dr["DEVICE_TOKEN"].ToString() : "";
                            var device_type = dr["device_type"] != DBNull.Value ? dr["device_type"].ToString() : "";
                            var messageText = dr["message"] != DBNull.Value ? dr["message"].ToString() : "";
                            var title_msg = dr["title"] != DBNull.Value ? dr["title"].ToString() : "";
                            var link_type = dr["link_type"] != DBNull.Value ? dr["link_type"].ToString() : "";
                            var memberID = dr["MEMBERID"] != DBNull.Value ? dr["MEMBERID"].ToString() : "";

                            if (!string.IsNullOrEmpty(messageText)
                               && !string.IsNullOrEmpty(title_msg)
                               && !string.IsNullOrEmpty(link_type)
                               && !string.IsNullOrEmpty(id))
                            {

                                ServiceReminderMessage noti = new ServiceReminderMessage()
                                {
                                    title_msg = title_msg,
                                    detail_msg = messageText,
                                    link_type = link_type
                                };
                                string NotiID = AddNotification(noti, memberID);

                                var device = AddMemberList(NotiID, null, memberID);

                                if(!string.IsNullOrEmpty(tokenID)
                                    && !string.IsNullOrEmpty(device_type))
                                {
                                    var firebase = service.SendPush(tokenID, messageText, "SendPush", 1, device_type, title_msg, "0", link_type, "", NotiID);
                                    service.UpdateNotificationSended(NotiID, firebase.Message, firebase.Success, true);
                                    if (firebase.Success)
                                    {
                                        ServiceReminderStampSended(id);
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void ServiceReminderStampSended(string id)
        {
            string cmd = @"
                UPDATE [dbo].[service_reminder_log]
                SET is_sended = 1
                WHERE id = {0}
            ";
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, id)))
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> GetServiceReminderSchedule(DateTime date)
        {
            var date_txt = date.ToString("yyyy-MM-dd HH:mm:ss");
            string cmd = @"
                SELECT *
                from [dbo].[service_reminder_schedule_log]
                WHERE schedule_date = CONVERT(DATETIME, N'{0}', 120)
            ";
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, date_txt)))
                    {
                        if (dt.Rows.Count == 0)
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public async void AddServiceReminderSchedule(DateTime date, string response)
        {
            var date_txt = date.ToString("yyyy-MM-dd HH:mm:ss");
            string cmd = @"
                INSERT INTO [dbo].[service_reminder_schedule_log]
                (schedule_date, schedule_response)
                VALUES (CONVERT(DATETIME, N'{0}', 120), N'{1}')
            ";
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, date_txt, response)))
                    {

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