using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class CarModelService
    {
        private string conn;
        public CarModelService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetModels(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT  [MODEL_ID]
		,[MODEL]
		,[IMAGE]
        ,[IS_TEST_DRIVE]
FROM	[T_CAR_MODEL]
WHERE	DEL_FLAG IS NULL AND [MODEL] LIKE '%' + @Value + '%'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(searchValue)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetModelById(string modelId)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @MODEL_ID NVARCHAR(20) = N'{0}'

SELECT  [MODEL_ID]
		,[MODEL]
		,[IMAGE]
        ,[IS_TEST_DRIVE]
FROM	[T_CAR_MODEL]
WHERE	[MODEL_ID] = @MODEL_ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(modelId));

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
        public void AddModel(string model, string image, string isTestDrive, string user)
        {
            try
            {
                string cmd = @"
DECLARE @MODEL NVARCHAR(255) = N'{0}'
DECLARE @IMAGE NVARCHAR(255) = N'{1}'
DECLARE @IS_TEST_DRIVE BIT = N'{2}'

INSERT INTO [T_CAR_MODEL](MODEL, IMAGE,IS_TEST_DRIVE)
VALUES (@MODEL, @IMAGE,@IS_TEST_DRIVE)";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(model),
                        WebUtility.GetSQLTextValue(image),
                        WebUtility.GetSQLTextValue(isTestDrive));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateModel(string model, string image, string isTestDrive, string user, string modelId)
        {
            try
            {
                string cmd = @"
DECLARE @MODEL NVARCHAR(255) = N'{0}'
DECLARE @IMAGE NVARCHAR(255) = N'{1}'
DECLARE @MODEL_ID NVARCHAR(20) = N'{2}'
DECLARE @IS_TEST_DRIVE BIT = N'{3}'

UPDATE	[T_CAR_MODEL]
SET		[MODEL] = @MODEL,
		[IMAGE] = @IMAGE,
        [IS_TEST_DRIVE] = @IS_TEST_DRIVE
WHERE	[MODEL_ID] = @MODEL_ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(model),
                        WebUtility.GetSQLTextValue(image),
                        WebUtility.GetSQLTextValue(modelId),
                        WebUtility.GetSQLTextValue(isTestDrive));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteModel(string modelId, string user)
        {
            try
            {
                string cmd = @"
DECLARE @MODEL_ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	T_CAR_MODEL
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	MODEL_ID = @MODEL_ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(modelId),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetColorByModelId(string modelId)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @MODEL_ID NVARCHAR(20) = N'{0}'

SELECT		MD.[MODEL_ID],
			MD.[MODEL],
			CL.[BODYCLR_CD],
			CL.[BODYCLR_NAME]
FROM		[T_CAR_MODEL] MD
INNER JOIN	[T_CAR_MODEL_CLR] CL ON CL.MODEL_ID = MD.MODEL_ID
WHERE		MD.[MODEL_ID] = @MODEL_ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(modelId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetColorByModelIdAndCode(string modelId, string colorCode)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @MODEL_ID NVARCHAR(20) = N'{0}'
DECLARE @BODYCLR_CD NVARCHAR(10) = N'{1}'

SELECT		MD.[MODEL_ID],
			MD.[MODEL],
			CL.[BODYCLR_CD],
			CL.[BODYCLR_NAME]
FROM		[T_CAR_MODEL] MD
INNER JOIN	[T_CAR_MODEL_CLR] CL ON CL.MODEL_ID = MD.MODEL_ID
WHERE		MD.[MODEL_ID] = @MODEL_ID AND CL.[BODYCLR_CD] = @BODYCLR_CD";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(modelId),
                        WebUtility.GetSQLTextValue(colorCode));

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
        public void AddColor(string modelId, string colorCode, string colorName)
        {
            try
            {
                string cmd = @"
DECLARE @MODEL_ID NVARCHAR(20) = N'{0}'
DECLARE @BODYCLR_CD NVARCHAR(10) = N'{1}'
DECLARE @BODYCLR_NAME NVARCHAR(150) = N'{2}'

INSERT INTO [T_CAR_MODEL_CLR]
VALUES (@MODEL_ID, @BODYCLR_CD, @BODYCLR_NAME)";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(modelId),
                        WebUtility.GetSQLTextValue(colorCode),
                        WebUtility.GetSQLTextValue(colorName));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateColor(string modelId, string colorCodeOld, string colorCode, string colorName)
        {
            try
            {
                string cmd = @"
DECLARE @MODEL_ID NVARCHAR(20) = N'{0}'
DECLARE @BODYCLR_CD NVARCHAR(10) = N'{1}'
DECLARE @BODYCLR_CD_OLD NVARCHAR(10) = N'{2}'
DECLARE @BODYCLR_NAME NVARCHAR(150) = N'{3}'

UPDATE	[T_CAR_MODEL_CLR]
SET		BODYCLR_CD = @BODYCLR_CD,
		BODYCLR_NAME = @BODYCLR_NAME
WHERE	MODEL_ID = @MODEL_ID AND BODYCLR_CD = @BODYCLR_CD_OLD";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(modelId),
                        WebUtility.GetSQLTextValue(colorCode),
                        WebUtility.GetSQLTextValue(colorCodeOld),
                        WebUtility.GetSQLTextValue(colorName));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteColor(string modelId, string colorCode)
        {
            try
            {
                string cmd = @"
DECLARE @MODEL_ID NVARCHAR(20) = N'{0}'
DECLARE @BODYCLR_CD NVARCHAR(10) = N'{1}'

DELETE FROM [T_CAR_MODEL_CLR] WHERE	MODEL_ID = @MODEL_ID AND BODYCLR_CD = @BODYCLR_CD";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(modelId),
                        WebUtility.GetSQLTextValue(colorCode));

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