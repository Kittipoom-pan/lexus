using AppLibrary.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace com.lexus.th.web
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

        public DataTable GetNotification(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'
DECLARE @push_notification_day_config nvarchar(3) = (select data_config from system_config where name = 'push_remaining_day' AND is_active = 1)
DECLARE @push_notification_date_from datetime =(select dateadd(day, convert(int,@Push_notification_day_config *-1), getdate()))


SELECT		id
            ,title
            ,destination
            ,sub_destination
            ,schedule_date + ' ' + schedule_time AS send_date
            ,status
            ,member_id
            ,device_id
            ,create_date
            ,create_by
			,DEL_FLAG
FROM		notification2
WHERE		DEL_FLAG IS NULL 
            AND (ISNULL(title, '') LIKE '%' + @Value + '%'
			    OR ISNULL(destination, '') LIKE '%' + @Value + '%'
                OR ISNULL(sub_destination, '') LIKE '%' + @Value + '%'
                OR ISNULL(status, '') LIKE '%' + @Value + '%'
			    OR CONVERT(NVARCHAR(10), schedule_date, 103) LIKE '%' + @Value + '%')
            AND create_date BETWEEN @push_notification_date_from AND DATEADD(HOUR, 7, GETDATE())
ORDER BY create_date DESC";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(searchValue));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetNotificationById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		id
            ,title
            ,destination
            ,message
            ,COALESCE(sub_destination,'none') sub_destination
            ,is_link
            ,link_type
            ,link_url
            ,is_schedule
            ,schedule_date
            ,schedule_time
            ,status
            ,member_id
            ,device_id
            ,path_file
            ,create_date
            ,create_by
FROM		notification2
WHERE		id = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            row = r;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        public string AddNotification(string destination, string sub_destination, string member_id, string device_id, string title, string message,
            string is_link, string link_type, string link_url, string is_schedule, string schedule_date, string schedule_time, string user, 
            string device_type, string reference_id, string schedule_datetime,int is_sended, string noti_schedule_id = null)
        {
            //DateTime scheduleDatetime = DateTime.ParseExact(schedule_datetime, "yyyy-MM-dd HH:mm:ss", null);
            string result = "";
            try
            {
                string cmd = @"
DECLARE @destination  NVARCHAR(255) = N'{0}'
DECLARE @sub_destination  NVARCHAR(255) = N'{1}'
DECLARE @member_id  NVARCHAR(100) = N'{2}'
DECLARE @device_id  NVARCHAR(100) = N'{3}'
DECLARE @title  NVARCHAR(255) = N'{4}'
DECLARE @message NVARCHAR(max) = N'{5}'
DECLARE @is_link  INT = N'{6}'
DECLARE @link_type  NVARCHAR(255) = N'{7}'
DECLARE @link_url  NVARCHAR(255) = N'{8}'
DECLARE @is_schedule  INT = N'{9}'
DECLARE @schedule_date  NVARCHAR(100) = N'{10}'
DECLARE @schedule_time  NVARCHAR(100) = N'{11}'
DECLARE @USER  NVARCHAR(50) = N'{12}'
DECLARE @device_type  NVARCHAR(50) = N'{13}'
DECLARE @reference_id  INT = N'{14}'
DECLARE @scheduleDatetime  NVARCHAR(50) = N'{15}'
DECLARE @is_sended BIT = N'{16}'
DECLARE @notification_schedule_id  NVARCHAR(10) = N'{17}'

INSERT INTO NOTIFICATION (notify_title, notify_message, device_type, notify_type, reference_id, 
    link_url, create_date)
VALUES (CASE LEN(@title) WHEN 0 THEN NULL ELSE @title END,
		CASE LEN(@message) WHEN 0 THEN NULL ELSE @message END,
		CASE LEN(@device_type) WHEN 0 THEN NULL ELSE @device_type END,
		CASE LEN(@link_type) WHEN 0 THEN NULL ELSE @link_type END,
		CASE LEN(@reference_id) WHEN 0 THEN NULL ELSE @reference_id END,
		CASE LEN(@link_url) WHEN 0 THEN NULL ELSE @link_url END,
		DATEADD(HOUR, 7, GETDATE()))

INSERT INTO notification2 (destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, 
    is_schedule, schedule_date, schedule_time, create_date, create_by, reference_id, send_datetime,is_sended,notification_schedule_id)
VALUES (CASE LEN(@destination) WHEN 0 THEN NULL ELSE @destination END,
		CASE LEN(@sub_destination) WHEN 0 THEN NULL ELSE @sub_destination END,
		CASE LEN(@member_id) WHEN 0 THEN NULL ELSE @member_id END,
		CASE LEN(@device_id) WHEN 0 THEN NULL ELSE @device_id END,
		CASE LEN(@title) WHEN 0 THEN NULL ELSE @title END,
		CASE LEN(@message) WHEN 0 THEN NULL ELSE @message END,
		CASE LEN(@is_link) WHEN 0 THEN NULL ELSE @is_link END,
		CASE LEN(@link_type) WHEN 0 THEN NULL ELSE @link_type END,
		CASE LEN(@link_url) WHEN 0 THEN NULL ELSE @link_url END,
		CASE LEN(@is_schedule) WHEN 0 THEN NULL ELSE @is_schedule END,
        CASE LEN(@schedule_date) WHEN 0 THEN NULL ELSE @schedule_date END,
        CASE LEN(@schedule_time) WHEN 0 THEN NULL ELSE @schedule_time END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER,
        CASE LEN(@reference_id) WHEN 0 THEN NULL ELSE @reference_id END,
        CASE LEN(@scheduleDatetime) WHEN 0 THEN NULL ELSE @scheduleDatetime END,
        @is_sended,
        @notification_schedule_id)

select CONVERT(NVARCHAR, @@IDENTITY) AS [ID]";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    if (noti_schedule_id == null)
                    {
                        cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(destination),
                        WebUtility.GetSQLTextValue(sub_destination),
                        WebUtility.GetSQLTextValue(member_id),
                        WebUtility.GetSQLTextValue(device_id),
                        WebUtility.GetSQLTextValue(title.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(message.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(is_link),
                        WebUtility.GetSQLTextValue(link_type),
                        WebUtility.GetSQLTextValue(link_url),
                        WebUtility.GetSQLTextValue(is_schedule),
                        WebUtility.GetSQLTextValue(schedule_date),
                        WebUtility.GetSQLTextValue(schedule_time),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(device_type),
                        WebUtility.GetSQLTextValue(reference_id),
                        WebUtility.GetSQLTextValue(schedule_datetime),
                        WebUtility.GetSQLTextValue(is_sended.ToString()),
                        DBNull.Value);
                    }
                    else {
                        cmd = string.Format(cmd,
                            WebUtility.GetSQLTextValue(destination),
                            WebUtility.GetSQLTextValue(sub_destination),
                            WebUtility.GetSQLTextValue(member_id),
                            WebUtility.GetSQLTextValue(device_id),
                            WebUtility.GetSQLTextValue(title.Replace("'", "’")),
                            WebUtility.GetSQLTextValue(message.Replace("'", "’")),
                            WebUtility.GetSQLTextValue(is_link),
                            WebUtility.GetSQLTextValue(link_type),
                            WebUtility.GetSQLTextValue(link_url),
                            WebUtility.GetSQLTextValue(is_schedule),
                            WebUtility.GetSQLTextValue(schedule_date),
                            WebUtility.GetSQLTextValue(schedule_time),
                            WebUtility.GetSQLTextValue(user),
                            WebUtility.GetSQLTextValue(device_type),
                            WebUtility.GetSQLTextValue(reference_id),
                            WebUtility.GetSQLTextValue(schedule_datetime),
                            WebUtility.GetSQLTextValue(is_sended.ToString()),
                            WebUtility.GetSQLTextValue(noti_schedule_id));
                    }
                    //dr = db.GetDataTableFromCommandText(cmd).AsEnumerable().FirstOrDefault();
                    result = db.ExecuteScalarFromCommandText<string>(cmd);
                    //result = dr[0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public string AddNotificationSchedule(DateTime schedule_date, string status, string user)
        {
            //DateTime date = DateTime.ParseExact(schedule_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string result = "";
            try
            {
                string cmd = @"
                            DECLARE @schedule_date DATETIME = N'{0}'
                            DECLARE @status  NVARCHAR(100) = N'{1}'
                            DECLARE @user  NVARCHAR(100) = N'{2}'

                            INSERT INTO notification_schedule (schedule_date, status ,created_by, created_date)
                            VALUES (CASE LEN(@schedule_date) WHEN 0 THEN NULL ELSE @schedule_date END,
		                            CASE LEN(@status) WHEN 0 THEN NULL ELSE @status END,
                                    @USER,
	                                DATEADD(HOUR, 7, GETDATE()))

                            select CONVERT(NVARCHAR, @@IDENTITY) AS [ID]";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(schedule_date.ToString()),
                        WebUtility.GetSQLTextValue(status),
                        WebUtility.GetSQLTextValue(user));

                    //dr = db.GetDataTableFromCommandText(cmd).AsEnumerable().FirstOrDefault();
                    result = db.ExecuteScalarFromCommandText<string>(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public DataTable AddMemberList(string NotiID, DataTable MemberIDList, string MemberID)
        {
            DataTable notification2_device;
            try
            {
                string cmd = @"
insert into notification2_device
(notification2_id, member_id, os_type, device_id, device_token)
select distinct @notification2_id, t.MEMBERID, d.DEVICE_TYPE, d.DEVICE_ID, d.DEVICE_TOKEN 
from T_DEVICE d LEFT JOIN T_CUSTOMER_TOKEN t ON d.TOKEN_NO = t.TOKEN_NO
where t.TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE()) AND t.MEMBERID in ({0}) and d.TOKEN_NO <> '' and d.TOKEN_NO is not null

select distinct device_token, os_type from notification2_device where notification2_id = @notification2_id";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
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

                    cmd = string.Format(cmd,Members);

                    db.ParamterList.Add(new SqlParameter("@notification2_id", NotiID));

                    notification2_device = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return notification2_device;
        }

        public DataTable AddDeviceList(string NotiID, DataTable DeviceIDList, string DeviceID)
        {
            DataTable notification2_device;
            try
            {
                string cmd = @"
insert into notification2_device
(notification2_id, member_id, os_type, device_id, device_token)
select distinct @notification2_id, t.MEMBERID, d.DEVICE_TYPE, d.DEVICE_ID, d.DEVICE_TOKEN 
from T_DEVICE d LEFT JOIN T_CUSTOMER_TOKEN t ON d.TOKEN_NO = t.TOKEN_NO
where d.DEVICE_ID in ({0})

select distinct device_token, os_type from notification2_device where notification2_id = @notification2_id";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string Device = "''";
                    if (DeviceID == "")
                    {
                        foreach (DataRow dr in DeviceIDList.Rows)
                        {
                            Device = "'" + dr[0].ToString() + "'," + Device;
                        }
                    }
                    else
                    {
                        Device = "'" + DeviceID + "'";
                    }

                    cmd = string.Format(cmd,Device);

                    db.ParamterList.Add(new SqlParameter("@notification2_id", NotiID));

                    notification2_device = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return notification2_device;
        }

        public void UpdateNotification(string destination, string sub_destination, string member_id, string device_id, string title, string message,
            string is_link, string link_type, string link_url, string is_schedule, string schedule_date, string schedule_time, string user, string id,
            string device_type, string reference_id)
        {
            try
            {
                string cmd = @"
DECLARE @destination  NVARCHAR(255) = N'{0}'
DECLARE @sub_destination  NVARCHAR(255) = N'{1}'
DECLARE @member_id  NVARCHAR(100) = N'{2}'
DECLARE @device_id  NVARCHAR(100) = N'{3}'
DECLARE @title  NVARCHAR(255) = N'{4}'
DECLARE @message  NVARCHAR(255) = N'{5}'
DECLARE @is_link  INT = N'{6}'
DECLARE @link_type  NVARCHAR(255) = N'{7}'
DECLARE @link_url  NVARCHAR(255) = N'{8}'
DECLARE @is_schedule  INT = N'{9}'
DECLARE @schedule_date  NVARCHAR(100) = N'{10}'
DECLARE @schedule_time  INT = N'{11}'
DECLARE @USER  NVARCHAR(50) = N'{12}'
DECLARE @ID INT = N'{13}'
DECLARE @device_type  NVARCHAR(50) = N'{14}'
DECLARE @reference_id  NVARCHAR(50) = N'{15}'


UPDATE		notification2
SET			destination = CASE LEN(@destination) WHEN 0 THEN NULL ELSE @destination END,
			sub_destination = CASE LEN(@sub_destination) WHEN 0 THEN NULL ELSE @sub_destination END,
			member_id = CASE LEN(@member_id) WHEN 0 THEN NULL ELSE @member_id END,
			device_id = CASE LEN(@device_id) WHEN 0 THEN NULL ELSE @device_id END,
			title = CASE LEN(@IMAGES2) WHEN 0 THEN NULL ELSE @title END,
			message = CASE LEN(@IMAGES3) WHEN 0 THEN NULL ELSE @message END,
			is_link = CASE LEN(@IMAGES4) WHEN 0 THEN NULL ELSE @is_link END,
			link_type = CASE LEN(@IMAGES5) WHEN 0 THEN NULL ELSE @link_type END,
			link_url = CASE LEN(@link_url) WHEN 0 THEN NULL ELSE @link_url END,
			is_schedule = CASE LEN(@is_schedule) WHEN 0 THEN NULL ELSE @is_schedule END,
            schedule_date = CASE LEN(@schedule_date) WHEN 0 THEN NULL ELSE @schedule_date END,
            schedule_time = CASE LEN(@schedule_time) WHEN 0 THEN NULL ELSE @schedule_time END,
			UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
			UPDATE_USER = @USER,
            reference_id = CASE LEN(@reference_id) WHEN 0 THEN NULL ELSE @reference_id END
WHERE	id = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(destination),
                        WebUtility.GetSQLTextValue(sub_destination),
                        WebUtility.GetSQLTextValue(member_id),
                        WebUtility.GetSQLTextValue(device_id),
                        WebUtility.GetSQLTextValue(title.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(message.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(is_link),
                        WebUtility.GetSQLTextValue(link_type),
                        WebUtility.GetSQLTextValue(link_url),
                        WebUtility.GetSQLTextValue(is_schedule),
                        WebUtility.GetSQLTextValue(schedule_date),
                        WebUtility.GetSQLTextValue(schedule_time),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(device_type),
                        WebUtility.GetSQLTextValue(reference_id));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteNotification(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER NVARCHAR(50) = N'{1}'

UPDATE notification2 
SET DEL_FLAG = 'Y', 
    DEL_DT = DATEADD(HOUR, 7, GETDATE()),
    DEL_USER = @USER
WHERE ID = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAction(string action_type)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    if (action_type == "Privilege")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_PRIVILEDGES 
WHERE DEL_FLAG IS NULL AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }
                    if (action_type == "Event")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_EVENTS 
WHERE DEL_FLAG IS NULL AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }
                    if (action_type == "News")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_NEWS 
WHERE DEL_FLAG IS NULL AND is_active = 1 
AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }
                    if (action_type == "Article")
                    {
                        cmd = @"
SELECT ID, topic_th AS TITLE 
FROM article 
WHERE DEL_FLAG IS NULL AND is_active = 1 
    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), display_start_date, 120) AND CONVERT(NVARCHAR(10), display_end_date, 120)";
                    }

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public void UpdateNotificationSended(string noti_id, string firebase_response, bool success, bool is_sended)
        {
            try
            {
                string cmd = @"
                DECLARE @firebase_response NVARCHAR(MAX) = N'{0}'
                DECLARE @success BIT = N'{1}'
                DECLARE @is_sended BIT = N'{2}'
                DECLARE @id INT = N'{3}'

                UPDATE notification2 
                SET [firebase_response] = @firebase_response, 
                    [firebase_success] = @success,
                    [is_sended] = @is_sended
                WHERE ID = @id";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(firebase_response),
                        success,
                        is_sended,
                        noti_id);

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}