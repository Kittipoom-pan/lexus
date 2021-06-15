using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppLibrary.Database;

namespace com.lexus.th
{
    public class TokenService
    {
        private static string conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        public class TokenServicResult
        {
            public int ResultCode { get; set; }
            public string ResultMsg { get; set; }
        }
        public static async Task<TokenServicResult> CheckTokenResult(string token, string lang)
        {
            TokenServicResult value = new TokenServicResult();
            try
            {
                MessageService ms = new MessageService();
                int message_code = 300705;

                if (string.IsNullOrEmpty(token))
                {
                    //Message : "PLease login."
                    message_code = 21023001;
                    value.ResultCode = 401;
                    value.ResultMsg =await ms.GetMessagetext(message_code, lang);
                    return value;
                }
                if (!IsValid(token))
                {
                    //Message : "Session expired, please re-activate again."
                    value.ResultCode = 101;
                    value.ResultMsg =await ms.GetMessagetext(message_code, lang);
                    return value;
                }
                if (IsExpire(token))
                {
                    //Message : "Session expired, please re-activate again."
                    value.ResultCode = 100;
                    value.ResultMsg =await ms.GetMessagetext(message_code, lang);
                    return value;
                }

                value.ResultCode = 1;
                value.ResultMsg = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        private static bool IsValid(string token)
        {
            bool value = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "SELECT ISNULL((SELECT 1 FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}'), 0) AS VALID";
                    value = Convert.ToBoolean(db.ExecuteScalarFromCommandText<int>(string.Format(cmd, token)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        private static bool IsExpire(string token)
        {
            bool value = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "SELECT ISNULL((SELECT 0 FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}' AND TOKEN_EXPIREY > DATEADD(HOUR, 7, GETDATE())), 1) AS VALID";
                    value = Convert.ToBoolean(db.ExecuteScalarFromCommandText<int>(string.Format(cmd, token)));
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