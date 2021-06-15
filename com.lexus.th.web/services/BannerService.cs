using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class BannerService
    {
        private string conn;
        public BannerService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetBanner()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
SELECT b.id,b.type,b.[order],e.TITLE,b.action_id
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN T_EVENTS e ON b.action_id = e.ID
WHERE		b.DEL_FLAG IS NULL AND b.type = 'Events'
UNION
SELECT b.id,b.type,b.[order],n.TITLE,b.action_id
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN T_NEWS n ON b.action_id = n.ID
WHERE		b.DEL_FLAG IS NULL AND b.type = 'News'
UNION
SELECT b.id,b.type,b.[order],p.TITLE,b.action_id
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN T_PRIVILEDGES p ON b.action_id = p.ID
WHERE		b.DEL_FLAG IS NULL AND b.type = 'Privilege'
UNION
SELECT b.id,b.type,b.[order],'',b.action_id
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b 
WHERE		b.DEL_FLAG IS NULL AND b.type = 'Main_Online_Booking' 
UNION
SELECT b.id,b.type,b.[order],bk.title_en COLLATE SQL_Latin1_General_CP1_CI_AS AS TITLE,b.action_id
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN booking bk ON b.action_id = bk.ID
WHERE		b.DEL_FLAG IS NULL AND b.type = 'Online_Booking_Repurchase' 
UNION
SELECT b.id,b.type,b.[order],bk.title_en COLLATE SQL_Latin1_General_CP1_CI_AS AS TITLE,b.action_id
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN booking bk ON b.action_id = bk.ID
WHERE		b.DEL_FLAG IS NULL AND b.type = 'Online_Booking_Referral' 
UNION
SELECT b.id,b.type,b.[order],bk.title_en COLLATE SQL_Latin1_General_CP1_CI_AS AS TITLE,b.action_id
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN booking bk ON b.action_id = bk.ID
WHERE		b.DEL_FLAG IS NULL AND b.type = 'Online_Booking_Guest' 
ORDER BY create_date DESC";

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

        public DataRow GetBannerById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		id
			,image_url
            ,type
            ,action_id
            ,[order]
            ,start_date
            ,end_date
            ,create_date
            ,create_by
            ,update_date
            ,update_by
FROM		banner
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

        public void AddBanner(string type, string action, string order, string img1, string start_date, string end_date, string user)
        {
            try
            {
                string cmd = @"
DECLARE @TYPE  NVARCHAR(100) = N'{0}'
DECLARE @ACTION  INT = N'{1}'
DECLARE @ORDER  INT = N'{2}'
DECLARE @IMAGES1  NVARCHAR(300) = N'{3}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{4}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{5}'
DECLARE @USER  NVARCHAR(50) = N'{6}'

INSERT INTO [banner] ([type],[action_id],[order],[image_url],[start_date],[end_date],[create_date],[create_by])
VALUES (CASE LEN(@TYPE) WHEN 0 THEN NULL ELSE @TYPE END,
		CASE LEN(@ACTION) WHEN 0 THEN NULL ELSE @ACTION END,
		CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
		CASE LEN(@IMAGES1) WHEN 0 THEN NULL ELSE @IMAGES1 END,
		CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END , 103) END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(type),
                        WebUtility.GetSQLTextValue(action),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(img1),
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
        public void UpdateBanner(string type, string action, string order, string img1, string start_date, string end_date, string user, string id)
        {
            try
            {
                string cmd = @"
DECLARE @TYPE  NVARCHAR(100) = N'{0}'
DECLARE @ACTION  INT = N'{1}'
DECLARE @ORDER  INT = N'{2}'
DECLARE @IMAGES1  NVARCHAR(300) = N'{3}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{4}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{5}'
DECLARE @USER  NVARCHAR(50) = N'{6}'
DECLARE @ID INT = N'{7}'

UPDATE		banner
SET			type = CASE LEN(@TYPE) WHEN 0 THEN NULL ELSE @TYPE END,
			action_id = CASE LEN(@ACTION) WHEN 0 THEN NULL ELSE @ACTION END,
			[order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
			image_url = CASE LEN(@IMAGES1) WHEN 0 THEN NULL ELSE @IMAGES1 END,
			start_date = CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
			end_date = CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END , 103) END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		id = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(type),
                        WebUtility.GetSQLTextValue(action),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(img1),
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
        public void DeleteBanner(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	banner
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	id = @ID";
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
WHERE DEL_FLAG IS NULL AND CONVERT(NVARCHAR(10), DISPLAY_END, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)
--AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }
                    if (action_type == "News")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_NEWS 
WHERE DEL_FLAG IS NULL AND is_active = 1 AND CONVERT(NVARCHAR(10), DISPLAY_END, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)
--AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }
                    if (action_type == "Privilege")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_PRIVILEDGES 
WHERE DEL_FLAG IS NULL AND CONVERT(NVARCHAR(10), DISPLAY_END, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)
--AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }

                    if (action_type == "Online_Booking_Repurchase")
                    {
                        cmd = @"
SELECT id, title_en AS TITLE  
FROM booking WHERE type = 1 AND deleted_flag IS NULL AND CONVERT(NVARCHAR(10), display_end, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)";
                    }

                    if (action_type == "Online_Booking_Referral")
                    {
                        cmd = @"
SELECT id, title_en AS TITLE 
FROM booking WHERE type = 2 AND deleted_flag IS NULL AND CONVERT(NVARCHAR(10), display_end, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)";
                    }

                    if (action_type == "Online_Booking_Guest")
                    {
                        cmd = @"
SELECT id, title_en AS TITLE 
FROM booking WHERE type = 3 AND deleted_flag IS NULL AND CONVERT(NVARCHAR(10), display_end, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)";
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

        public DataTable GetActionEdit(string action_type)
        {
            DataTable dt = new DataTable();
            if(action_type == "Main_Online_Booking")
            {
                return dt;
            }
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
--WHERE DEL_FLAG IS NULL AND CONVERT(NVARCHAR(10), DISPLAY_END, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)
--AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }
                    if (action_type == "News")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_NEWS 
--WHERE DEL_FLAG IS NULL AND is_active = 1 AND CONVERT(NVARCHAR(10), DISPLAY_END, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)
--AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }
                    if (action_type == "Privilege")
                    {
                        cmd = @"
SELECT ID, TITLE 
FROM T_PRIVILEDGES 
--WHERE DEL_FLAG IS NULL AND CONVERT(NVARCHAR(10), DISPLAY_END, 120) > CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120)
--AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";
                    }

                    if (action_type == "Online_Booking_Repurchase")
                    {
                        cmd = @"
SELECT id, title_en AS TITLE 
FROM booking WHERE type = 1 AND deleted_flag IS NULL";
                    }

                    if (action_type == "Online_Booking_Referral")
                    {
                        cmd = @"
SELECT id, title_en AS TITLE 
FROM booking WHERE type = 2 AND deleted_flag IS NULL";
                    }

                    if (action_type == "Online_Booking_Guest")
                    {
                        cmd = @"
SELECT id, title_en AS TITLE 
FROM booking WHERE type = 3 AND deleted_flag IS NULL";
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
    }
}