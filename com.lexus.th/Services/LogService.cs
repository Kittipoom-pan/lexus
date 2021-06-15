using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class LogService
    {
        private string userAgent = HttpContext.Current.Request.UserAgent.ToLower();
        private string conn;
        public LogService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task InsertLogReceiveData(string service_name, string receive_data)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
INSERT INTO log_receive_data (service_name, receive_data, device_info, create_date)
VALUES ('{0}', '{1}', '{2}', DATEADD(HOUR, 7, GETDATE()))";

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, service_name, receive_data, userAgent));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}