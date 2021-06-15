using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;
using System.Data.SqlClient;

namespace com.lexus.th.web
{
    public class LoginService
    {
        private string conn;
        public class UserInfo
        {
            public string UserName { get; set; }
            public string SellerId { get; set; }
            public string Role { get; set; }
            public string Dealer { get; set; }
        }

        public LoginService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public bool IsValidateLogin(string user, string password)
        {
            bool isValid = false;
            try
            {
                string cmd = @"
--DECLARE @USERNAME NVARCHAR(50) = N'{0}'
--DECLARE @PASSWORD NVARCHAR(50) = N'{1}'

SELECT COUNT(1) AS CNT FROM T_USER WHERE USERNAME = @USERNAME AND CONVERT(VARCHAR(50),DecryptByPassPhrase('Lexus', [PASSWORD_ENC])) = @PASSWORD";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(user),
                    //    WebUtility.GetSQLTextValue(password));
                    db.ParamterList.Add(new SqlParameter("@USERNAME", user));
                    db.ParamterList.Add(new SqlParameter("@PASSWORD", password));

                    if (db.ExecuteScalarFromCommandText<int>(cmd) > 0)
                    {
                        isValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isValid;
        }
        public UserInfo GetUserInfo(string user, string password)
        {
            UserInfo value = null;
            try
            {
                string cmd = @"
--DECLARE @USERNAME NVARCHAR(50) = N'{0}'
--DECLARE @PASSWORD NVARCHAR(50) = N'{1}'

SELECT  [USERNAME] AS [USERNAME], ISNULL([SELLERID], '') AS [SELLERID], ISNULL([ROLES], '') AS [ROLES], ISNULL([DEALER], '') AS [DEALER]
FROM    T_USER 
WHERE   [USERNAME] = @USERNAME 
        AND CONVERT(NVARCHAR(50),DecryptByPassPhrase('Lexus', PASSWORD_ENC )) COLLATE SQL_Latin1_General_CP1_CS_AS = @PASSWORD";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(user),
                    //    WebUtility.GetSQLTextValue(password));
                    db.ParamterList.Add(new SqlParameter("@USERNAME", user));
                    db.ParamterList.Add(new SqlParameter("@PASSWORD", password));


                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            value = new UserInfo();
                            value.UserName = dt.Rows[0]["USERNAME"].ToString();
                            value.SellerId = dt.Rows[0]["SELLERID"].ToString();
                            value.Role = dt.Rows[0]["ROLES"].ToString();
                            value.Dealer = dt.Rows[0]["DEALER"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
    }
}