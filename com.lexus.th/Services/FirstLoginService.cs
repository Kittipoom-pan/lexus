using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace com.lexus.th
{
    public class FirstLoginService
    {
        private string conn;
        public FirstLoginService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceFirstLoginModel> StampFirstLogin(string v, string p, string token, string lang)
        {

            ServiceFirstLoginModel value = new ServiceFirstLoginModel();
            try
            {
                SystemController syc = new SystemController();
                ValidationModel validation2 = await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        //DECLARE @OTP NVARCHAR(10) = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
                        string cmd = @"
DECLARE @token NVARCHAR(250) = N'{0}'
DECLARE @platform NVARCHAR(250) = N'{1}'
DECLARE @title NVARCHAR(250) = N'{2}'
DECLARE @lang NVARCHAR(250) = N'{3}'
DECLARE @member_id NVARCHAR(50) = ''
DECLARE @message NVARCHAR(400) = ''
SET @member_id = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @token)
SET @message = (SELECT content FROM system_message WHERE code = '10006003' AND lang = @lang AND platform = @platform)

INSERT INTO notification2 (destination, member_id, device_id, title, message, is_link, link_type,
    is_schedule, create_date, create_by)
VALUES ('Member',
		CASE LEN(@member_id) WHEN 0 THEN NULL ELSE @member_id END,
		CASE LEN(@token) WHEN 0 THEN NULL ELSE @token END,
		@title,
		@message,
		0,
		'none',
		0,
		DATEADD(HOUR, 7, GETDATE()),
		@member_id)

update [T_CUSTOMER]
set [is_first_login]=1
where MEMBERID = @member_id

select @@ROWCOUNT";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, p, WebConfigurationManager.AppSettings["TitleFirstLogin"], lang)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                NotificationService srv = new NotificationService();
                                //string NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, 0, "none", "", 0, "", "", user, "All", reference_id, ScheduleDate.ToString());
                                value.success = true;
                                value.msg = new MsgModel() { code = 0, text = "Success" };
                            }
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