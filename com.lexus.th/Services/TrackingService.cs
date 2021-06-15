using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class TrackingService
    {
        private string conn;
        public TrackingService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task AddTracking(string v, string p, string token, string name, string device_id, string session_id, string language, string other, string treasure_data)
        {

            ServiceTrackingModel value = new ServiceTrackingModel();
            try
            {
                //SystemController syc = new SystemController();
                //ValidationModel validation2 = syc.CheckSystemNew(p, v, language);
                //if (!validation2.Success)
                //{
                //    value.success = validation2.Success;
                //    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                //    return value;
                //}
                //else
                //{
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //DECLARE @OTP NVARCHAR(10) = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
                    string cmd = @"
DECLARE @member_id NVARCHAR(50) = ''
IF '{0}' = '' BEGIN
 SET @member_id = 'Guest'
END
ELSE BEGIN
 SET @member_id = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{0}')
END

INSERT tracking (name, device_id, session_id, language, other, create_date, create_by, treasure_data)
VALUES ('{1}', '{2}', '{3}','{4}', '{5}', DATEADD(HOUR, 7, GETDATE()), @member_id, '{6}')";

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, token, name, device_id, session_id, language, other, treasure_data));

                }
                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}