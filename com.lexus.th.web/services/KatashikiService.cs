using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class KatashikiService
    {
        private string conn;
        public KatashikiService()
        {

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetKatashiki(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT		id
			,katashiki_code
			,model_id
			,is_active
            ,create_date
            ,create_by
            ,update_date
            ,update_by
FROM		katashiki_model
WHERE		DEL_FLAG IS NULL 
            AND (ISNULL(katashiki_code, '') LIKE '%' + @Value + '%'
			OR ISNULL(model_id, '') LIKE '%' + @Value + '%')";

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
        public DataTable GetKatashikiAndModel(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT		a.id
			,a.katashiki_code
			,a.model_id
			,a.is_active
            ,a.create_date
            ,a.create_by
            ,a.update_date
            ,a.update_by
            ,b.MODEL
            ,b.IMAGE
FROM		katashiki_model a
left join T_CAR_MODEL b
on a.model_id = b.MODEL_ID
WHERE	    a.DEL_FLAG IS NULL 
            AND (ISNULL(katashiki_code, '') LIKE '%' + @Value + '%'
			OR ISNULL(b.MODEL, '') LIKE '%' + @Value + '%')";

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
        public DataRow GetKatashikiById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		a.id
			,a.katashiki_code
			,a.model_id
			,a.is_active
            ,a.create_date
            ,a.create_by
            ,a.update_date
            ,a.update_by
            ,b.MODEL
            ,b.IMAGE
FROM		katashiki_model a
left join T_CAR_MODEL b on a.model_id = b.MODEL_ID
WHERE		a.id = @ID";

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
        public DataTable AddKatashikiModel(string katashiki_code, string model_id, string user, bool excel)
        {
            DataTable row = null;
            DateTime datenow = DateTime.Now;
            try
            {
                string cmd = @"
DECLARE @MODEL_ID int = (select top 1 model_id from T_CAR_MODEL where MODEL_ID = @c_model_id)

insert into katashiki_model
(katashiki_code, model_id, is_active, create_date, create_by)
select a.* from 
(select @katashiki_code katashiki_code, @c_model_id c_model_id, 1 is_active, @create_date create_date, @create_by create_by) a
join (select count(id) c_id from katashiki_model where katashiki_code = @katashiki_code) b
on b.c_id = 0 and @MODEL_ID is not null

IF @@ROWCOUNT = 0
BEGIN
	IF @MODEL_ID is null
	BEGIN
		select @katashiki_code katashiki_code, 'MODEL not found' log
	END ELSE
	BEGIN
		select @katashiki_code katashiki_code, 'Katashiki Code is duplicate' log
	END
END
ELSE
BEGIN
	select @katashiki_code katashiki_code, 'insert success' log
END";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ParamterList.Add(new SqlParameter("@katashiki_code", katashiki_code));
                    db.ParamterList.Add(new SqlParameter("@c_model_id", model_id));
                    db.ParamterList.Add(new SqlParameter("@create_date", datenow));
                    db.ParamterList.Add(new SqlParameter("@create_by", user));

                    row = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return row;
        }
        public void AddKatashikiModel(string katashiki_code, string model_id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @KATASHIKICODE  NVARCHAR(100) = N'{0}'
DECLARE @MODELID  INT = N'{1}'
DECLARE @USER  NVARCHAR(50) = N'{2}'

INSERT INTO katashiki_model (katashiki_code, model_id, create_date, create_by)
VALUES (CASE LEN(@KATASHIKICODE) WHEN 0 THEN NULL ELSE @KATASHIKICODE END,
		CASE LEN(@MODELID) WHEN 0 THEN NULL ELSE @MODELID END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(katashiki_code),
                        WebUtility.GetSQLTextValue(model_id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateKatashikiModel(string katashiki_code, string model_id, string user, string id)
        {
            try
            {
                string cmd = @"
DECLARE @KATASHIKICODE  NVARCHAR(100) = N'{0}'
DECLARE @MODELID  INT = N'{1}'
DECLARE @USER  NVARCHAR(50) = N'{2}'
DECLARE @ID INT = N'{3}'

UPDATE		katashiki_model
SET			katashiki_code = CASE LEN(@KATASHIKICODE) WHEN 0 THEN NULL ELSE @KATASHIKICODE END,
			model_id = CASE LEN(@MODELID) WHEN 0 THEN NULL ELSE @MODELID END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		id = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(katashiki_code),
                        WebUtility.GetSQLTextValue(model_id),
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
        public void DeleteKatashikiModel(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	katashiki_model
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

        public DataTable GetCarModels()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT MODEL_ID, MODEL 
FROM T_CAR_MODEL
WHERE DEL_FLAG IS NULL";

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public void InsertKatashikiCode(string katashiki_code, string model_id, string user)
        {

            try
            {
                string cmd = @"
DECLARE @KATASHIKI_CODE NVARCHAR(100) = N'{0}'
DECLARE @MODEL_ID INT = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'

INSERT INTO [katashiki_model] ([katashiki_code],[model_id],[create_date],[create_by])
VALUES (@KATASHIKI_CODE,@MODEL_ID,DATEADD(HOUR, 7, GETDATE()),@USER)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(katashiki_code),
                        WebUtility.GetSQLTextValue(model_id),
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