using AppLibrary.Database;
using com.lexus.th.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th.Services
{
    public class GetNotificationCountService
    {
        private string conn;
        public GetNotificationCountService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<NotificationCountModel> GetNotification(string token,string v, string p, string lang)
        {
            NotificationCountModel value = new NotificationCountModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new NotificationCount();
                    value.data.noti_count = await GetNotificationCount(token);
                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetNotificationCount");
                throw ex;
            }
            return value;
        }

        public async Task<int> GetNotificationCount(string token)
        {
            int count = 0;

            try
            {
                    
                string cmd = @"DECLARE @memberid nvarchar(50) = (SELECT memberid FROM T_CUSTOMER_TOKEN where TOKEN_NO = '{0}')
                               DECLARE @login_now DATETIME = (SELECT LOGIN_DT FROM T_CUSTOMER_TOKEN where TOKEN_NO = '{0}')
                               DECLARE @last_login DATETIME = (SELECT TOP 1 LOGIN_DT FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO != '{0}' AND MEMBERID = @memberid ORDER BY id DESC)

                               if @login_now is null
                               begin
                                    SELECT COUNT(*) AS CNT
                                    FROM notification2 n
                                    WHERE n.member_id = @memberid AND is_service_reminder = 1 AND is_sended = 0
                               end
                               else
                               begin
                                    SELECT COUNT(*) AS CNT
                                    FROM notification2 n
                                    WHERE n.member_id = @memberid AND is_service_reminder = 1 AND is_sended = 0
                                    AND create_date BETWEEN @last_login AND @login_now
                               end";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, token);

                    var dt = db.GetDataTableFromCommandText(cmd);

                    foreach (DataRow row in dt.Rows)
                    {
                        count = row["CNT"] != DBNull.Value ? Convert.ToInt32(row["CNT"]) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return count;
         }
    }
}