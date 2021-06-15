using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class PreferenceService
    {
        private string conn;
        public PreferenceService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetPreference()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"

SELECT id AS question_id
		,name_th AS description
		,is_active
        ,create_date
        ,create_by
        ,[order]
FROM	preference
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
        public DataRow GetPreferenceById(string preferenceId)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT id AS question_id
		,name_en
        ,name_th
		,is_active
        ,start_date
        ,end_date
        ,is_require_answer
        ,[order]
        ,max_selected
FROM	preference
WHERE id = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(preferenceId));

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
        public void AddPreference(string name_th, string name_en, string active, string require_answer, string user, string order, string max_selected)
        {
            try
            {
                string cmd = @"
DECLARE @NAMETH NVARCHAR(200) = N'{0}'
DECLARE @NAMEEN NVARCHAR(200) = N'{1}'
DECLARE @ACTIVE  INT = N'{2}'
DECLARE @REQUIRE  INT = N'{3}'
DECLARE @USER  NVARCHAR(50) = N'{4}'
DECLARE @ORDER  INT = N'{5}'
DECLARE @MAXSELECTED  INT = N'{6}'

INSERT INTO preference (name_th, name_en, is_active, is_require_answer, create_date, create_by, [order], max_selected)
VALUES (
    CASE LEN(@NAMETH) WHEN 0 THEN NULL ELSE @NAMETH END,
    CASE LEN(@NAMEEN) WHEN 0 THEN NULL ELSE @NAMEEN END,
    CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
    CASE LEN(@REQUIRE) WHEN 0 THEN NULL ELSE @REQUIRE END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER,
    CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
    CASE LEN(@MAXSELECTED) WHEN 0 THEN NULL ELSE @MAXSELECTED END)";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(name_th),
                        WebUtility.GetSQLTextValue(name_en),
                        WebUtility.GetSQLTextValue(active),
                        //WebUtility.GetSQLTextValue(start_date),
                        //WebUtility.GetSQLTextValue(end_date),
                        WebUtility.GetSQLTextValue(require_answer),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(max_selected));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePreference(string name_th, string name_en, string active, string require_answer, string user, string preferenceId, string order, string max_selected)
        {
            try
            {
                string cmd = @"
DECLARE @NAMETH NVARCHAR(200) = N'{0}'
DECLARE @NAMEEN NVARCHAR(200) = N'{1}'
DECLARE @ACTIVE  INT = N'{2}'
DECLARE @REQUIRE  INT = N'{3}'
DECLARE @USER  NVARCHAR(50) = N'{4}'
DECLARE @ID  INT = N'{5}'
DECLARE @ORDER  INT = N'{6}'
DECLARE @MAXSELECTED  INT = N'{7}'

UPDATE	preference
SET		name_th = CASE LEN(@NAMETH) WHEN 0 THEN NULL ELSE @NAMETH END,
        name_en = CASE LEN(@NAMEEN) WHEN 0 THEN NULL ELSE @NAMEEN END,
        is_active = CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
        is_require_answer = CASE LEN(@REQUIRE) WHEN 0 THEN NULL ELSE @REQUIRE END,
        update_date = DATEADD(HOUR, 7, GETDATE()),
        update_by = @USER,
        [order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
        max_selected = CASE LEN(@MAXSELECTED) WHEN 0 THEN NULL ELSE @MAXSELECTED END
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(name_th),
                        WebUtility.GetSQLTextValue(name_en),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(require_answer),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(preferenceId),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(max_selected));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePreference(string preferenceId, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'


UPDATE	preference
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(preferenceId),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAnswerByModelId(string preferenceId)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		id AS answer_id
            ,preference_id AS question_id
            ,name_th AS description
            ,name_en
            ,icon_image_url
            ,is_active
            ,start_date
            ,end_date
            ,[order]
            ,is_optional
            ,optional_text
            ,optional_header
            ,create_date
            ,create_by
            ,update_date
            ,update_by
            ,CASE is_optional WHEN 1 THEN 'Textbox' ELSE 'Checkbox' END AS type
            ,[order]
FROM		preference_choice
WHERE		DEL_FLAG IS NULL AND preference_id = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(preferenceId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetAnswerByModelIdAndChoice(string answerId)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		id AS answer_id
            ,preference_id AS question_id
            ,name_en
            ,name_th
            ,icon_image_url
            ,is_active
            ,start_date
            ,end_date
            ,[order]
            ,is_optional
            ,optional_text
            ,optional_header
            ,create_date
            ,create_by
            ,update_date
            ,update_by
            ,CASE is_optional WHEN 1 THEN 'Textbox' ELSE 'Checkbox' END AS type
            ,[order]
FROM		preference_choice
WHERE		id = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(answerId));

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
        public void AddAnswer(string preferenceId, string name_en, string name_th, string icon_image_url, string active,
            string optional, string optional_header, string optional_text, string user, string order)
        {
            try
            {
                string cmd = @"
DECLARE @PREFERENCEID  INT = N'{0}'
DECLARE @NAMEEN NVARCHAR(200) = N'{1}'
DECLARE @NAMETH NVARCHAR(200) = N'{2}'
DECLARE @IMAGE VARCHAR(250) = N'{3}'
DECLARE @ACTIVE  INT = N'{4}'
DECLARE @OPTIONAL  INT = N'{5}'
DECLARE @OPTIONALHEAD  NVARCHAR(150) = N'{6}'
DECLARE @OPTIONALTEXT  NVARCHAR(100) = N'{7}'
DECLARE @USER  NVARCHAR(50) = N'{8}'
DECLARE @ORDER  INT = N'{9}'

INSERT INTO preference_choice (preference_id, name_th, name_en, icon_image_url, is_active, is_optional, 
optional_text, optional_header, create_date, create_by, [order])
VALUES (
    CASE LEN(@PREFERENCEID) WHEN 0 THEN NULL ELSE @PREFERENCEID END,
    CASE LEN(@NAMETH) WHEN 0 THEN NULL ELSE @NAMETH END,
    CASE LEN(@NAMEEN) WHEN 0 THEN NULL ELSE @NAMEEN END,
    CASE LEN(@IMAGE) WHEN 0 THEN NULL ELSE @IMAGE END,
    CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
    CASE LEN(@OPTIONAL) WHEN 0 THEN NULL ELSE @OPTIONAL END,
    CASE LEN(@OPTIONALHEAD) WHEN 0 THEN NULL ELSE @OPTIONALHEAD END,
    CASE LEN(@OPTIONALTEXT) WHEN 0 THEN NULL ELSE @OPTIONALTEXT END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER,
    CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END)";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(preferenceId),
                        WebUtility.GetSQLTextValue(name_en),
                        WebUtility.GetSQLTextValue(name_th),
                        WebUtility.GetSQLTextValue(icon_image_url),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(optional),
                        WebUtility.GetSQLTextValue(optional_header),
                        WebUtility.GetSQLTextValue(optional_text),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(order));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateAnswer(string preferenceId, string name_en, string name_th, string icon_image_url, string active,
            string optional, string optional_header, string optional_text, string user, string answerid, string order)
        {
            try
            {
                string cmd = @"
DECLARE @PREFERENCEID  INT = N'{0}'
DECLARE @NAMEEN NVARCHAR(200) = N'{1}'
DECLARE @NAMETH NVARCHAR(200) = N'{2}'
DECLARE @IMAGE VARCHAR(250) = N'{3}'
DECLARE @ACTIVE  INT = N'{4}'
DECLARE @OPTIONAL  INT = N'{5}'
DECLARE @OPTIONALHEAD  NVARCHAR(150) = N'{6}'
DECLARE @OPTIONALTEXT  NVARCHAR(100) = N'{7}'
DECLARE @USER  NVARCHAR(50) = N'{8}'
DECLARE @ID  INT = N'{9}'
DECLARE @ORDER  INT = N'{10}'

UPDATE	preference_choice
SET		preference_id = CASE LEN(@PREFERENCEID) WHEN 0 THEN NULL ELSE @PREFERENCEID END,
        name_th = CASE LEN(@NAMETH) WHEN 0 THEN NULL ELSE @NAMETH END,
        name_en = CASE LEN(@NAMEEN) WHEN 0 THEN NULL ELSE @NAMEEN END,
        icon_image_url = CASE LEN(@IMAGE) WHEN 0 THEN NULL ELSE @IMAGE END,
        is_active = CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
        is_optional = CASE LEN(@OPTIONAL) WHEN 0 THEN NULL ELSE @OPTIONAL END,
        optional_text = CASE LEN(@OPTIONALTEXT) WHEN 0 THEN NULL ELSE @OPTIONALTEXT END,
        optional_header = CASE LEN(@OPTIONALHEAD) WHEN 0 THEN NULL ELSE @OPTIONALHEAD END,
        update_date = DATEADD(HOUR, 7, GETDATE()),
        update_by = @USER,
        [order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(preferenceId),
                        WebUtility.GetSQLTextValue(name_en),
                        WebUtility.GetSQLTextValue(name_th),
                        WebUtility.GetSQLTextValue(icon_image_url),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(optional),
                        WebUtility.GetSQLTextValue(optional_header),
                        WebUtility.GetSQLTextValue(optional_text),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(answerid),
                        WebUtility.GetSQLTextValue(order));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteAnswer(string answerId, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	preference_choice
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(answerId),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDlPreference()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT id, name_th 
FROM	preference
-- WHERE is_active = 1 AND DEL_FLAG IS NULL";

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