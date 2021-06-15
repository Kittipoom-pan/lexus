using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class MessageService
    {
        private string conn;

        public MessageService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<string> GetMessagetext(int message_code, string lang)
        {
            string message_text = string.Empty;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT  content
FROM system_message
WHERE --platform = 'SERVICE' AND 
code = {0} AND lang = N'{1}'
ORDER BY code ASC";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, message_code, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            message_text = row["content"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message_text;
        }

        public string GetMessagetextReplace(int message_code, string lang, string platform)
        {
            string message_text = string.Empty;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @version VARCHAR(10)
SET @version = (SELECT version FROM SYSTEM_VERSION WHERE LOWER(platform) = N'{2}')

SELECT REPLACE(content, '[0]', @version) content
FROM system_message
WHERE platform = 'SERVICE' AND code = {0} AND lang = N'{1}'
ORDER BY code ASC";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, message_code, lang, platform.ToLower())))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            message_text = row["content"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message_text;
        }
    }
}