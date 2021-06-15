using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class ArticleService
    {
        private string conn;
        public ArticleService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetArticle(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT		[id]
			,[topic_th]
			,[is_active]
			,[display_start_date]
			,[display_end_date]
			,[create_date]
			,[create_by]
			,[update_date]
			,[update_by]
            ,[order]
FROM		[article]
WHERE		DEL_FLAG IS NULL
            AND (ISNULL([topic_th], '') LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [display_start_date], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [display_end_date], 103) LIKE '%' + @Value + '%')";

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

        public DataRow GetArticleById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[id]
			,[topic_th]
            ,[topic_date]
            ,[topic_url]
			,[is_active]
            ,[images1]
            ,[images2]
            ,[images3]
            ,[images4]
            ,[images5]
			,[display_start_date]
			,[display_end_date]
			,[create_date]
			,[create_by]
			,[update_date]
			,[update_by]
            ,[order]
FROM		[article]
WHERE		[id] = @ID";

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

        public void AddArticle(string title, string date, string url, string img1, string img2, string img3, string img4, string img5, string dispStart, string dispEnd, string user, string active, string order)
        {
            try
            {
                string cmd = @"
DECLARE @TITLE  NVARCHAR(250) = N'{0}'
DECLARE @DATE  NVARCHAR(20) = N'{1}'
DECLARE @URL  NVARCHAR(MAX) = N'{2}'
DECLARE @IMAGES1  NVARCHAR(150) = N'{3}'
DECLARE @IMAGES2  NVARCHAR(150) = N'{4}'
DECLARE @IMAGES3  NVARCHAR(150) = N'{5}'
DECLARE @IMAGES4  NVARCHAR(150) = N'{6}'
DECLARE @IMAGES5  NVARCHAR(150) = N'{7}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{8}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{9}'
DECLARE @USER  NVARCHAR(50) = N'{10}'
DECLARE @ACTIVE  INT = N'{11}'
DECLARE @ORDER  INT = N'{12}'

INSERT INTO [article] ([topic_th],[topic_date],[topic_url],[images1],[images2],[images3],[images4],[images5],[display_start_date],[display_end_date],
[create_date],[create_by],is_active,[order])
VALUES (CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		CASE LEN(@DATE) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DATE, 103) END,
		CASE LEN(@URL) WHEN 0 THEN NULL ELSE @URL END,
		CASE LEN(@IMAGES1) WHEN 0 THEN NULL ELSE @IMAGES1 END,
		CASE LEN(@IMAGES2) WHEN 0 THEN NULL ELSE @IMAGES2 END,
		CASE LEN(@IMAGES3) WHEN 0 THEN NULL ELSE @IMAGES3 END,
		CASE LEN(@IMAGES4) WHEN 0 THEN NULL ELSE @IMAGES4 END,
		CASE LEN(@IMAGES5) WHEN 0 THEN NULL ELSE @IMAGES5 END,
		CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END + ' 23:59:59', 103) END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER,
        CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
        CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(date),
                        WebUtility.GetSQLTextValue(url),
                        WebUtility.GetSQLTextValue(img1),
                        WebUtility.GetSQLTextValue(img2),
                        WebUtility.GetSQLTextValue(img3),
                        WebUtility.GetSQLTextValue(img4),
                        WebUtility.GetSQLTextValue(img5),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(order));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateArticle(string title, string date, string url, string img1, string img2, string img3, string img4, string img5, string dispStart, string dispEnd, string user, string id, string active, string order)
        {
            try
            {
                string cmd = @"
DECLARE @TITLE  NVARCHAR(250) = N'{0}'
DECLARE @DATE  NVARCHAR(20) = N'{1}'
DECLARE @URL  NVARCHAR(MAX) = N'{2}'
DECLARE @IMAGES1  NVARCHAR(150) = N'{3}'
DECLARE @IMAGES2  NVARCHAR(150) = N'{4}'
DECLARE @IMAGES3  NVARCHAR(150) = N'{5}'
DECLARE @IMAGES4  NVARCHAR(150) = N'{6}'
DECLARE @IMAGES5  NVARCHAR(150) = N'{7}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{8}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{9}'
DECLARE @USER  NVARCHAR(50) = N'{10}'
DECLARE @ID INT = N'{11}'
DECLARE @ACTIVE  INT = N'{12}'
DECLARE @ORDER  INT = N'{13}'

UPDATE		[article]
SET			topic_th = CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
			[topic_date] = CASE LEN(@DATE) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DATE, 103) END,
			[topic_url] = CASE LEN(@URL) WHEN 0 THEN NULL ELSE @URL END,
			images1 = CASE LEN(@IMAGES1) WHEN 0 THEN NULL ELSE @IMAGES1 END,
			images2 = CASE LEN(@IMAGES2) WHEN 0 THEN NULL ELSE @IMAGES2 END,
			images3 = CASE LEN(@IMAGES3) WHEN 0 THEN NULL ELSE @IMAGES3 END,
			images4 = CASE LEN(@IMAGES4) WHEN 0 THEN NULL ELSE @IMAGES4 END,
			images5 = CASE LEN(@IMAGES5) WHEN 0 THEN NULL ELSE @IMAGES5 END,
			display_start_date = CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
			display_end_date = CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END + ' 23:59:59', 103) END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER,
            [is_active] = CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
            [order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END
WHERE		[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(date),
                        WebUtility.GetSQLTextValue(url),
                        WebUtility.GetSQLTextValue(img1),
                        WebUtility.GetSQLTextValue(img2),
                        WebUtility.GetSQLTextValue(img3),
                        WebUtility.GetSQLTextValue(img4),
                        WebUtility.GetSQLTextValue(img5),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(order));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteArticle(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	article
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
    }
}