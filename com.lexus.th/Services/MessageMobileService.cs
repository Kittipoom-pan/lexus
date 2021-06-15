using AppLibrary.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class MessageMobileService
    {
        private string conn;

        public MessageMobileService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceMessageMobileModel> GetMessageMobile(string device_type, string lang)
        {
            ServiceMessageMobileModel value = new ServiceMessageMobileModel();
            try
            {
                value.data = new _ServiceMessageMobileData();

                value.data.message = GetAllMessageMobile(device_type, lang);

                value.success = true;
                value.msg = new MsgModel() { code = 0, text = "Success" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private List<MessageMobileModel> GetAllMessageMobile(string device_type, string lang)
        {
            List<MessageMobileModel> list = new List<MessageMobileModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT code, content
FROM system_message
WHERE platform = N'{0}' AND lang = N'{1}'
ORDER BY code ASC";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            MessageMobileModel message = new MessageMobileModel();
                            message.code = row["code"].ToString();
                            message.content = row["content"].ToString();

                            list.Add(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}