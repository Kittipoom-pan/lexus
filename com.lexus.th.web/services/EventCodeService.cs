using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class EventCodeService
    {
        private string conn;

        public EventCodeService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetEvents()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
                SELECT [id] AS [EVENT_ID]
                      ,[event_type] AS [EVENT_TYPE]
                      ,[TITLE] 
                FROM  [T_EVENTS]
                WHERE DEL_FLAG IS NULL AND event_type = 'invitation'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetEventsCodeById(string eventId)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @EVENT_ID INT = N'{0}'

SELECT		EC.[EVENT_ID]
            ,EC.[ID]
			,EC.[REDEEM_CODE]
            ,EC.[STATUS]
			,EC.[CREATE_DT]
			,EC.[CREATE_USER]
			,EC.[UPDATE_DT]
			,EC.[UPDATE_USER]
			,E.TITLE AS [TITLE]
FROM		[T_EVENTS] E
INNER JOIN	[T_EVENTS_CODE] EC ON E.[ID] = EC.[EVENT_ID]
WHERE EC.EVENT_ID = @EVENT_ID AND EC.DEL_FLAG IS NULL
order by EC.[ID]";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(eventId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataRow GetEventCodeById(string eventId, string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @EVENT_ID INT = N'{0}'
DECLARE @ID INT = N'{1}'

SELECT		EC.[ID]
			,EC.[EVENT_ID]
			,EC.[REDEEM_CODE]
            ,EC.[STATUS]
			,EC.[CREATE_DT]
			,EC.[CREATE_USER]
			,EC.[UPDATE_DT]
			,EC.[UPDATE_USER]
			,E.TITLE
FROM		[T_EVENTS] E
INNER JOIN	[T_EVENTS_CODE] EC ON E.[ID] = EC.[EVENT_ID]
WHERE EC.EVENT_ID = @EVENT_ID AND EC.[ID] = @ID AND EC.DEL_FLAG IS NULL";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(eventId),
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
        public void AddEventCode(string eventId, string redeemCode, string user, string status)
        {
            try
            {
                string cmd = @"
DECLARE @EVENT_ID INT = N'{0}'
DECLARE @REDEEM_CODE NVARCHAR(50) = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'
DECLARE @STATUS NVARCHAR(10) = N'{3}'

DECLARE @ID INT = ISNULL((SELECT MAX([ID]) FROM [T_EVENTS_CODE] WHERE EVENT_ID = @EVENT_ID), 0) + 1
INSERT INTO [T_EVENTS_CODE] ([EVENT_ID],[ID],[REDEEM_CODE],[STATUS],[CREATE_DT],[CREATE_USER])
VALUES (@EVENT_ID, @ID, @REDEEM_CODE, @STATUS, DATEADD(HOUR, 7, GETDATE()), @USER)";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(eventId),
                        WebUtility.GetSQLTextValue(redeemCode.Trim()),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(status));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEventCode(string eventId, string redeemCode, string user, string id, string status)
        {
            try
            {
                string cmd = @"
DECLARE @EVENT_ID INT = N'{0}'
DECLARE @REDEEM_CODE NVARCHAR(50) = N'{1}'
DECLARE @ID INT = N'{2}'
DECLARE @USER NVARCHAR(50) = N'{3}'
DECLARE @STATUS NVARCHAR(10) = N'{4}'

UPDATE  [T_EVENTS_CODE]
SET		[REDEEM_CODE] = @REDEEM_CODE,
        [STATUS] = @STATUS,
		[UPDATE_DT] = DATEADD(HOUR, 7, GETDATE()),
		[UPDATE_USER] = @USER
WHERE	EVENT_ID = @EVENT_ID AND [ID] = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(eventId),
                        WebUtility.GetSQLTextValue(redeemCode),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(status)
                        );

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEventCode(string eventId, string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @EVENT_ID INT = N'{0}'
DECLARE @ID INT = N'{1}'
DECLARE @USER  NVARCHAR(50) = N'{2}'

UPDATE	T_EVENTS_CODE
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	EVENT_ID = @EVENT_ID AND [ID] = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(eventId),
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
        public bool IsDupplicateRedeem(string eventId, string redeemCode)
        {
            bool isDupp = true;
            try
            {
                string cmd = @"
DECLARE @REDEEM_CODE NVARCHAR(50) = N'{1}'

SELECT COUNT(1) FROM T_EVENTS_CODE WHERE REDEEM_CODE = @REDEEM_CODE";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(eventId),
                        WebUtility.GetSQLTextValue(redeemCode.Trim()));

                    int cnt = db.ExecuteScalarFromCommandText<int>(cmd);
                    if (cnt == 0)
                    {
                        isDupp = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDupp;
        }
        public void UploadEventCode(string eventId, List<string> redeemCode, string user)
        {
            try
            {
                string cmd = @"
DECLARE @EVENT_ID INT = N'{0}'
DECLARE @REDEEM_CODE NVARCHAR(50) = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'

DECLARE @ID INT = ISNULL((SELECT MAX([ID]) FROM [T_EVENTS_CODE] WHERE EVENT_ID = @EVENT_ID), 0) + 1
INSERT INTO [T_EVENTS_CODE] ([EVENT_ID],[ID],[REDEEM_CODE],[STATUS],[CREATE_DT],[CREATE_USER])
VALUES (@EVENT_ID,@ID,@REDEEM_CODE,'NotUsed',DATEADD(HOUR, 7, GETDATE()),@USER)";

                List<string> cmdList = new List<string>();
                foreach (string code in redeemCode)
                {
                    cmdList.Add(string.Format(cmd,
                        WebUtility.GetSQLTextValue(eventId),
                        WebUtility.GetSQLTextValue(code),
                        WebUtility.GetSQLTextValue(user)));
                }

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(cmdList);
                }

                cmdList = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}