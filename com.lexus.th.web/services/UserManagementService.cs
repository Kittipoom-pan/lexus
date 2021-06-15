using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class UserManagementService
    {
        private string conn;
        public UserManagementService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetUsers(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT [ID]
      ,[USERNAME]
      ,CONVERT(NVARCHAR(50),DecryptByPassPhrase('Lexus', PASSWORD_ENC )) AS [PASSWORD]
      ,[EMAIL]
      ,[ROLES]
	  ,CASE [ROLES] 
		WHEN '1' THEN 'Super Admin'
		WHEN '2' THEN 'Admin'
		WHEN '3' THEN 'Dealer'
		WHEN '4' THEN 'Sales' END AS ROLE_NAME
      ,[STATUS]
      ,[DEALER]
      ,[SELLERID]
      ,[PASSWORD_EXPIREY]
      ,[CREATE_DT]
FROM   [T_USER]
WHERE  ISNULL([USERNAME], '') LIKE '%' + @Value + '%'
       OR ISNULL([EMAIL], '') LIKE '%' + @Value + '%'
       OR ISNULL([DEALER], '') LIKE '%' + @Value + '%'
       OR ISNULL([SELLERID], '') LIKE '%' + @Value + '%'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(searchValue)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetUsersById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
SELECT [ID]
      ,[USERNAME]
      ,CONVERT(NVARCHAR(50),DecryptByPassPhrase('Lexus', PASSWORD_ENC )) AS [PASSWORD]
      ,[EMAIL]
      ,[ROLES]
	  ,CASE [ROLES] 
		WHEN '1' THEN 'Super Admin'
		WHEN '2' THEN 'Admin'
		WHEN '3' THEN 'Dealer'
		WHEN '4' THEN 'Sales' END AS ROLE_NAME
      ,[STATUS]
      ,[DEALER]
      ,[SELLERID]
      ,[PASSWORD_EXPIREY]
      ,[CREATE_DT]
FROM   [T_USER]
WHERE  [ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id))))
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
        public void AddUser(string username, string password, string email, string role, string dealer, string sellerid)
        {
            try
            {
                string cmd = @"
DECLARE @USERNAME NVARCHAR(50) = N'{0}'
DECLARE @PASSWORD NVARCHAR(50) = N'{1}'
DECLARE @EMAIL NVARCHAR(100) = N'{2}'
DECLARE @ROLES NVARCHAR(1) = N'{3}'
DECLARE @DEALER NVARCHAR(150) = N'{4}'
DECLARE @SELLERID NVARCHAR(100) = N'{5}'

INSERT INTO T_USER ([USERNAME],[PASSWORD_ENC],[EMAIL],[ROLES],[STATUS],[DEALER],[SELLERID],[PASSWORD_EXPIREY],[CREATE_DT])
VALUES (@USERNAME,(SELECT EncryptByPassPhrase('Lexus', @PASSWORD)),@EMAIL,@ROLES,NULL,@DEALER,@SELLERID,NULL,GETDATE())";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(username),
                        WebUtility.GetSQLTextValue(password),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(role),
                        WebUtility.GetSQLTextValue(dealer),
                        WebUtility.GetSQLTextValue(sellerid)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateUser(string username, string password, string email, string role, string dealer, string sellerid, string id)
        {
            try
            {
                string cmd = @"
DECLARE @USERNAME NVARCHAR(50) = N'{0}'
DECLARE @PASSWORD NVARCHAR(50) = N'{1}'
DECLARE @EMAIL NVARCHAR(100) = N'{2}'
DECLARE @ROLES NVARCHAR(1) = N'{3}'
DECLARE @DEALER NVARCHAR(150) = N'{4}'
DECLARE @SELLERID NVARCHAR(100) = N'{5}'
DECLARE @ID NVARCHAR(10) = N'{6}'

UPDATE T_USER
SET USERNAME = CASE LEN(@USERNAME) WHEN 0 THEN NULL ELSE @USERNAME END,
	[PASSWORD_ENC] = CASE LEN(@PASSWORD) WHEN 0 THEN NULL ELSE (SELECT EncryptByPassPhrase('Lexus', @PASSWORD)) END,
	EMAIL = CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
	ROLES = CASE LEN(@ROLES) WHEN 0 THEN NULL ELSE @ROLES END,
	DEALER = CASE LEN(@DEALER) WHEN 0 THEN NULL ELSE @DEALER END,
	SELLERID = CASE LEN(@SELLERID) WHEN 0 THEN NULL ELSE @SELLERID END
WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(username),
                        WebUtility.GetSQLTextValue(password),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(role),
                        WebUtility.GetSQLTextValue(dealer),
                        WebUtility.GetSQLTextValue(sellerid),
                        WebUtility.GetSQLTextValue(id)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteUser(string id)
        {
            try
            {
                string cmd = @"
DECLARE @ID NVARCHAR(10) = N'{0}'
DELETE FROM T_USER WHERE ID = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}