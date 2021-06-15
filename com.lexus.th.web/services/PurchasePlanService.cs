using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class PurchasePlanService
    {
        private string conn;
        public PurchasePlanService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetPurchasePlan(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
                DECLARE @Value NVARCHAR(255) = N'{0}'

            SELECT			[ID]
                			,[NAME_TH]   
                			,[NAME_EN]   
                			,[IS_ACTIVE]     
                            ,[CREATED_DATE]	
                            ,[CREATED_USER]	

                FROM		[PURCHASE_PLAN]
                WHERE		DELETED_FLAG IS NULL 
                            AND( ISNULL([NAME_EN], '') LIKE '%' + @Value + '%'
                			OR ISNULL([NAME_TH], '') LIKE '%' + @Value + '%')
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
        public DataRow GetPurchasePlanById(string id)
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
                            ,[CREATED_DATE]	
                            ,[CREATED_USER]	
FROM		[PURCHASE_PLAN]
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
        public void AddPurchasePlan(string desc_th, string desc_en, string active, string user)
        {
            try
            {
                string cmd = @"
DECLARE @NAME_EN  NVARCHAR(100) = N'{0}'
DECLARE @NAME_TH  NVARCHAR(100) = N'{1}'
DECLARE @IS_ACTIVE  TINYINT = N'{2}'
DECLARE @USER  NVARCHAR(50) = N'{3}'

INSERT INTO [PURCHASE_PLAN] ([NAME_EN],[NAME_TH],[IS_ACTIVE],[CREATED_DATE],[CREATED_USER])
VALUES (
        CASE LEN(@NAME_TH) WHEN 0 THEN NULL ELSE @NAME_TH END,
        CASE LEN(@NAME_EN) WHEN 0 THEN NULL ELSE @NAME_EN END,	    
		CASE LEN(@IS_ACTIVE) WHEN 0 THEN NULL ELSE @IS_ACTIVE END,	
		GETDATE(),
		@USER)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                          WebUtility.GetSQLTextValue(desc_th),
                        WebUtility.GetSQLTextValue(desc_en),                      
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePurchasePlan(string descTH, string descEN, string isActive, string user, string id)
        {
            try
            {
                string cmd = @"
DECLARE @NAME_TH  NVARCHAR(100) = N'{0}'
DECLARE @NAME_EN  NVARCHAR(100) = N'{1}'
DECLARE @IS_ACTIVE  TINYINT = N'{2}'
DECLARE @USER  NVARCHAR(50) = N'{3}'
DECLARE @ID INT = N'{4}'

UPDATE		[PURCHASE_PLAN]
SET			NAME_TH = CASE LEN(@NAME_TH) WHEN 0 THEN NULL ELSE @NAME_TH END,
			NAME_EN = CASE LEN(@NAME_EN) WHEN 0 THEN NULL ELSE @NAME_EN END,
			IS_ACTIVE = CASE LEN(@IS_ACTIVE) WHEN 0 THEN NULL ELSE @IS_ACTIVE END,
			UPDATED_DATE = GETDATE(),
			UPDATED_USER = @USER
WHERE		[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(descTH),
                        WebUtility.GetSQLTextValue(descEN),
                        WebUtility.GetSQLTextValue(isActive),
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
        public void DeletePurchasePlan(string id,string user)
        {
            try
            {

                string cmd = @"
                                DECLARE @ID INT = N'{0}'
                                DECLARE @USER  NVARCHAR(50) = N'{1}'
                                
                                UPDATE	PURCHASE_PLAN
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
    }
}