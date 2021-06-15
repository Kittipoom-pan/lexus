using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class LogoutService
    {
        private string conn;
        public LogoutService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ServiceLogoutModel> Logout(string token, string v, string p)
        {
            ServiceLogoutModel value = new ServiceLogoutModel();
            try
            {
                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = @"
UPDATE T_CUSTOMER_TOKEN
SET TOKEN_EXPIREY = GETDATE()
WHERE TOKEN_NO = N'{0}'";

                        db.ExecuteNonQueryFromCommandText(string.Format(cmd, token));
                    }

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceLogoutModel> LogoutNew(string token, string v, string p, string lang)
        {
            ServiceLogoutModel value = new ServiceLogoutModel();
            try
            {
                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = @"
UPDATE T_CUSTOMER_TOKEN
SET TOKEN_EXPIREY = GETDATE()
WHERE TOKEN_NO = N'{0}'";

                        db.ExecuteNonQueryFromCommandText(string.Format(cmd, token));
                    }

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
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