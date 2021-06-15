using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class EventSpecialQuotaService
    {
        private string conn;
        public EventSpecialQuotaService()
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
SELECT [ID] AS [Event_ID]
      ,[TITLE] AS [Event_NAME]
FROM  [T_EVENTS]
WHERE DEL_FLAG IS NULL";

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
        public DataTable GetEventSpecialQuotaByEvent(string EventId)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
DECLARE @Event_ID INT = {0}
  SELECT SQ.Event_id, 
    SQ.member_id 
       ,SQ.[created_date]
      ,SQ.[updated_user]
      ,SQ.[deleted_flag]
      ,SQ.[delete_date]
      ,SQ.[delete_user],
    P.TITLE AS [Event_NAME] 
    FROM [T_EVENTS_BLACKLIST] SQ
    INNER JOIN	[T_EVENTS] P ON P.[ID] = SQ.[Event_id]
	 inner join  T_CUSTOMER c on  (SQ.member_id COLLATE Thai_CI_AI)  = c.[MEMBERID]
	 where SQ.Event_id =@Event_ID and SQ.deleted_flag IS NULL";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, EventId);
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetEventpecialQuotaByID(string EventId, string member_id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @Event_ID INT = N'{0}'
DECLARE @member_id NVARCHAR(50) = N'{1}'

SELECT SQ.Event_id, 
SQ.member_id,  
P.TITLE AS [Event_NAME] 
FROM [T_EVENTS_BLACKLIST] SQ
INNER JOIN	[T_EVENTS] P ON P.[ID] = SQ.[Event_id]
and P.ID = @Event_ID
and SQ.member_id = @member_id";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(EventId),
                        WebUtility.GetSQLTextValue(member_id));

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
        public void AddEventSpecialQuota(string Event_id, string member_id, string mobile, string user)
        {
            int insertCount = 0;
            try
            { 
                string cmd = @"
DECLARE @Event_id INT = N'{0}'
DECLARE @member_id NVARCHAR(50) = N'{1}'
DECLARE @mobile NVARCHAR(50) = N'{2}'
DECLARE @USER NVARCHAR(50) = N'{3}'

DECLARE @member_idset NVARCHAR(50) = null
DECLARE @mobileset NVARCHAR(50)= null

set @member_idset = (SELECT [MEMBERID]   
  FROM [dbo].[T_CUSTOMER]
  where [MEMBERID] =@member_id or [MOBILE] =@member_id)

set @mobileset = (SELECT [MOBILE]   
  FROM [dbo].[T_CUSTOMER]
  where [MEMBERID] =@member_id or [MOBILE] =@member_id)
IF (@member_idset IS NOT NULL AND @mobileset IS NOT NULL) 
    BEGIN
    INSERT INTO [T_EVENTS_BLACKLIST] ([Event_id], [member_id], [mobile ], [created_date],[updated_user]) 
    VALUES (@Event_id, @member_idset, @mobileset, DATEADD(HOUR, 7, GETDATE()), @USER)
    select CAST(SCOPE_IDENTITY() AS INT)
    END
else   BEGIN
       SELECT 0
       END
";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(Event_id),
                        WebUtility.GetSQLTextValue(member_id),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(user));

                    insertCount = db.ExecuteScalarFromCommandText<int>(cmd);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            switch (insertCount)
            {
                case -1:
                    throw new System.Exception("Member is already block.");
                case -2:
                    throw new System.Exception("Member is already block .");
                case -3:
                    throw new System.Exception("Member ID is not found.");
                case 0:
                    throw new System.Exception("Cannot insert please try again.");
                default:
                    break;
            }
        }

        public void DeleteEventSpecialQuota(string Event_id, string member_id, string user)
        {
            int updateCount = 0;
            try
            {
                string cmd = @"
DECLARE @Event_id INT = N'{0}'
DECLARE @member_id NVARCHAR(50) = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'

UPDATE  [T_EVENTS_BLACKLIST]
SET		[deleted_flag] = 1,
		[delete_date] = DATEADD(HOUR, 7, GETDATE()),
		[delete_user] = @USER
WHERE	Event_ID = @Event_ID and member_id = @member_id";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(Event_id),
                        WebUtility.GetSQLTextValue(member_id),
                        WebUtility.GetSQLTextValue(user));

                    updateCount = db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (updateCount == 0)
            {
                throw new System.Exception("Update quota not found Event or member.");
            }
        }

    }
}