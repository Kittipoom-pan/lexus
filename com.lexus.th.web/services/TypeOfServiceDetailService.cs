using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class TypeOfServiceDetailService
    {
        private string conn;
        public TypeOfServiceDetailService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetTypeOfServiceDetail(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
                DECLARE @Value NVARCHAR(255) = N'{0}'

            SELECT			D.[ID]
                			,D.[NAME_TH]  
                			,D.[NAME_EN]   
                			,D.[IS_ACTIVE]  
                            ,D.[TYPE_OF_SERVICE_ID]   
                            ,T.[NAME_EN]  AS TYPE_OF_SERVICE_NAME
                            ,D.[CREATED_DATE]	
                            ,D.[CREATED_USER]	

                FROM		[TYPE_OF_SERVICE_DETAIL] D
                LEFT JOIN [TYPE_OF_SERVICE] T ON T.ID = D.TYPE_OF_SERVICE_ID 
                WHERE		D.DELETED_FLAG IS NULL 
                            AND( ISNULL(D.[NAME_EN], '') LIKE '%' + @Value + '%'
                			OR ISNULL(D.[NAME_TH], '') LIKE '%' + @Value + '%')
                ORDER BY [ID]
";

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
        public DataRow GetTypeOfServiceDetailById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

            SELECT			[ID]
                			,[NAME_TH]   
                			,[NAME_EN]   
                			,[IS_ACTIVE]   
                            ,[TYPE_OF_SERVICE_ID]      
                            ,[CREATED_DATE]	
                            ,[CREATED_USER]	
FROM		[TYPE_OF_SERVICE_DETAIL]
WHERE		[ID] = @ID";

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
        public void AddTypeOfService(string desc_th, string desc_en, string active, string typeOfServiceId, string user)
        {
            try
            {
                string cmd = @"
DECLARE @NAME_EN  NVARCHAR(100) = N'{0}'
DECLARE @NAME_TH  NVARCHAR(100) = N'{1}'
DECLARE @IS_ACTIVE  TINYINT = N'{2}'
DECLARE @TYPE_OF_SERVICE_ID  INT = N'{3}'
DECLARE @USER  NVARCHAR(50) = N'{4}'

INSERT INTO [TYPE_OF_SERVICE_DETAIL] ([NAME_EN],[NAME_TH],[IS_ACTIVE],[TYPE_OF_SERVICE_ID],[CREATED_DATE],[CREATED_USER])
VALUES (
        CASE LEN(@NAME_TH) WHEN 0 THEN NULL ELSE @NAME_TH END,
        CASE LEN(@NAME_EN) WHEN 0 THEN NULL ELSE @NAME_EN END,	    
		CASE LEN(@IS_ACTIVE) WHEN 0 THEN NULL ELSE @IS_ACTIVE END,	
	    CASE LEN(@TYPE_OF_SERVICE_ID) WHEN 0 THEN NULL ELSE @TYPE_OF_SERVICE_ID END,	
		GETDATE(),
		@USER)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                          WebUtility.GetSQLTextValue(desc_th),
                        WebUtility.GetSQLTextValue(desc_en),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(typeOfServiceId),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTypeOfService(string descTH, string descEN, string isActive, string typeOfServiceId, string user, string id)
        {
            try
            {
                string cmd = @"
DECLARE @NAME_TH  NVARCHAR(100) = N'{0}'
DECLARE @NAME_EN  NVARCHAR(100) = N'{1}'
DECLARE @IS_ACTIVE  TINYINT = N'{2}'
DECLARE @TYPE_OF_SERVICE_ID  INT = N'{3}'
DECLARE @USER  NVARCHAR(50) = N'{4}'
DECLARE @ID INT = N'{5}'

UPDATE		[TYPE_OF_SERVICE_DETAIL]
SET			NAME_TH = CASE LEN(@NAME_TH) WHEN 0 THEN NULL ELSE @NAME_TH END,
			NAME_EN = CASE LEN(@NAME_EN) WHEN 0 THEN NULL ELSE @NAME_EN END,
			IS_ACTIVE = CASE LEN(@IS_ACTIVE) WHEN 0 THEN NULL ELSE @IS_ACTIVE END,
            TYPE_OF_SERVICE_ID = CASE LEN(@TYPE_OF_SERVICE_ID) WHEN 0 THEN NULL ELSE @TYPE_OF_SERVICE_ID END,
			UPDATED_DATE = GETDATE(),
			UPDATED_USER = @USER
WHERE		[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(descTH),
                        WebUtility.GetSQLTextValue(descEN),
                        WebUtility.GetSQLTextValue(isActive),
                          WebUtility.GetSQLTextValue(typeOfServiceId),
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
        public void DeleteTypeOfServiceDetail(string id, string user)
        {
            try
            {
                string cmd = @"
                                DECLARE @ID INT = N'{0}'
                                DECLARE @USER  NVARCHAR(50) = N'{1}'
                                
                                UPDATE	TYPE_OF_SERVICE_DETAIL
                                 SET	DELETED_FLAG = 'Y',
                                        DELETE_DATE = DATEADD(HOUR, 7, GETDATE()),
                                        DELETE_USER = @USER
                                WHERE	ID = @ID";
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

        internal object GetTypeOfServiceMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT ID, NAME_EN 
FROM TYPE_OF_SERVICE
WHERE DELETED_FLAG IS NULL";

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