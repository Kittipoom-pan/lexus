using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class LogService
    {
        private string userAgent = HttpContext.Current.Request.UserAgent.ToLower();
        private string conn;

        public LogService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public void InsertLogPush(string messagePush, string memberList, string androidList, string iosList)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @MESSAGEPUSH VARCHAR(300) = N'{0}'
DECLARE @MEMBERLIST VARCHAR(1000) = N'{1}'
DECLARE @ANDROIDLIST VARCHAR(1000) = N'{2}'
DECLARE @IOSLIST VARCHAR(1000) = N'{3}'

INSERT INTO log_push (message_push, member_id_list, ios_token_list, android_token_list, create_date)
VALUES (@MESSAGEPUSH, @MEMBERLIST, @ANDROIDLIST, @IOSLIST, DATEADD(HOUR, 7, GETDATE()))

SELECT id FROM log_push WHERE message_push = @MESSAGEPUSH AND create_date = DATEADD(HOUR, 7, GETDATE())";

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, messagePush, memberList, androidList, iosList));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertLogChange(string event_name, string before_data, string after_data, string user, string userAgent)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @EventName VARCHAR(50) = N'{0}'
DECLARE @BeforeData text = N'{1}'
DECLARE @AfterData text = N'{2}'
DECLARE @User VARCHAR(50) = N'{3}'
DECLARE @UserAgent text = N'{4}'

INSERT INTO log_change_data (event_name, before_data, after_data, device_info, create_date, create_by)
VALUES (@EventName, @BeforeData, @AfterData, @UserAgent, DATEADD(HOUR, 7, GETDATE()), @User)

SELECT id FROM log_push WHERE message_push = @MESSAGEPUSH AND create_date = DATEADD(HOUR, 7, GETDATE())";

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, event_name, before_data, after_data, user, userAgent));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertLogReceiveData(string service_name, string receive_data)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @ServiceName VARCHAR(50) = N'{0}'
DECLARE @ReceiveData VARCHAR(2000) = N'{1}'
DECLARE @UserAgent VARCHAR(2000) = N'{2}'

INSERT INTO log_receive_data (service_name, receive_data, device_info, create_date)
VALUES (@ServiceName, @ReceiveData, @UserAgent, DATEADD(HOUR, 7, GETDATE()))";

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, service_name, receive_data, userAgent));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}