using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class UpComingService
    {
        private string conn;
        public UpComingService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetUpComing(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = "";
                if (searchValue == "UpEvents")
                {
                    cmd = @"
SELECT		u.id
			,u.type
            ,u.action_id
            ,u.[order]
            ,u.is_active
            ,u.start_date
            ,u.end_date
            ,u.create_date
            ,u.create_by
            ,u.update_date
            ,u.update_by
            ,e.title
FROM		up_coming u LEFT JOIN T_EVENTS e ON u.action_id = e.ID
WHERE		u.DEL_FLAG IS NULL AND u.type = 'Events'
UNION
SELECT		u.id
			,u.type
            ,u.action_id
            ,u.[order]
            ,u.is_active
            ,u.start_date
            ,u.end_date
            ,u.create_date
            ,u.create_by
            ,u.update_date
            ,u.update_by
            ,n.title
FROM		up_coming u LEFT JOIN T_NEWS n ON u.action_id = n.ID
WHERE		u.DEL_FLAG IS NULL AND u.type = 'News'";
                }
                else
                {
                    cmd = @"
SELECT		u.id
			,u.type
            ,u.action_id
            ,u.[order]
            ,u.is_active
            ,u.start_date
            ,u.end_date
            ,u.create_date
            ,u.create_by
            ,u.update_date
            ,u.update_by
            ,n.title
FROM		new_control u LEFT JOIN T_NEWS n ON u.action_id = n.ID
WHERE		u.DEL_FLAG IS NULL AND u.type = 'News'";
                }

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(searchValue));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataRow GetUpComingById(string id, string typeList)
        {
            DataRow row = null;
            try
            {
                string cmd = "";
                if (typeList == "UpEvents")
                {
                    cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		id, 'UpEvents' AS list
			,type
            ,action_id
            ,[order]
            ,start_date
            ,end_date
FROM		up_coming
WHERE		id = @ID AND DEL_FLAG IS NULL";
                }
                else
                {
                    cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		id, 'News' AS list
			,type
            ,action_id
            ,[order]
            ,start_date
            ,end_date
FROM		new_control
WHERE		id = @ID AND DEL_FLAG IS NULL";
                }

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

        public void AddUpcoming(string type, string action, string order, string start_date, string end_date, string user, string list)
        {
            try
            {
                string cmd = "";
                if (list == "UpEvents")
                {
                    cmd = @"
DECLARE @TYPE  NVARCHAR(100) = N'{0}'
DECLARE @ACTION  INT = N'{1}'
DECLARE @ORDER  INT = N'{2}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{3}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{4}'
DECLARE @USER  NVARCHAR(50) = N'{5}'

INSERT INTO [up_coming] ([type],[action_id],[order],[start_date],[end_date],[create_date],[create_by])
VALUES (CASE LEN(@TYPE) WHEN 0 THEN NULL ELSE @TYPE END,
		CASE LEN(@ACTION) WHEN 0 THEN NULL ELSE @ACTION END,
		CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
		CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER)";
                }
                else
                {
                    cmd = @"
DECLARE @TYPE  NVARCHAR(100) = N'{0}'
DECLARE @ACTION  INT = N'{1}'
DECLARE @ORDER  INT = N'{2}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{3}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{4}'
DECLARE @USER  NVARCHAR(50) = N'{5}'

INSERT INTO [new_control] ([type],[action_id],[order],[start_date],[end_date],[create_date],[create_by])
VALUES (CASE LEN(@TYPE) WHEN 0 THEN NULL ELSE @TYPE END,
		CASE LEN(@ACTION) WHEN 0 THEN NULL ELSE @ACTION END,
		CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
		CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER)";
                }

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(type),
                        WebUtility.GetSQLTextValue(action),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(start_date),
                        WebUtility.GetSQLTextValue(end_date),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateUpcoming(string type, string action, string order, string start_date, string end_date, string user, string id, string list)
        {
            try
            {
                string cmd = "";
                if (list == "UpEvents")
                {
                    cmd = @"
DECLARE @TYPE  VARCHAR(100) = N'{0}'
DECLARE @ACTION  INT = N'{1}'
DECLARE @ORDER  INT = N'{2}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{3}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{4}'
DECLARE @USER  NVARCHAR(50) = N'{5}'
DECLARE @ID INT = N'{6}'

UPDATE		up_coming
SET			type = CASE LEN(@TYPE) WHEN 0 THEN NULL ELSE @TYPE END,
			action_id = CASE LEN(@ACTION) WHEN 0 THEN NULL ELSE @ACTION END,
			[order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
			start_date = CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
			end_date = CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		id = @ID";
                }
                else
                {
                    cmd = @"
DECLARE @TYPE  VARCHAR(100) = N'{0}'
DECLARE @ACTION  INT = N'{1}'
DECLARE @ORDER  INT = N'{2}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{3}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{4}'
DECLARE @USER  NVARCHAR(50) = N'{5}'
DECLARE @ID INT = N'{6}'

UPDATE		new_control
SET			type = CASE LEN(@TYPE) WHEN 0 THEN NULL ELSE @TYPE END,
			action_id = CASE LEN(@ACTION) WHEN 0 THEN NULL ELSE @ACTION END,
			[order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
			start_date = CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
			end_date = CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		id = @ID";
                }

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(type),
                        WebUtility.GetSQLTextValue(action),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(start_date),
                        WebUtility.GetSQLTextValue(end_date),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteUpcoming(string id, string user, string list)
        {
            try
            {
                string cmd = "";
                if (list == "UpEvents")
                {
                    cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	up_coming
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	id = @ID";
                }
                else
                {
                    cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	new_control
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	id = @ID";
                }
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
                    if (action_type == "Events")
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

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetActionEdit(string action_type, int id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    if (action_type == "Events")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_EVENTS 
WHERE DEL_FLAG IS NULL AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)
    OR ID = {0}";
                    }
                    if (action_type == "News")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_NEWS 
WHERE DEL_FLAG IS NULL AND is_active = 1 
    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)
    OR ID = {0}";
                    }

                    cmd = string.Format(cmd, id);

                    //db.ParamterList.Add(new SqlParameter("@notification2_id", NotiID));

                    dt = db.GetDataTableFromCommandText(cmd);

                    //dt = db.GetDataTableFromCommandText(cmd, id);

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