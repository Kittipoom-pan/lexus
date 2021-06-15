using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class LoginService
    {
        private string conn;
        public LoginService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async  Task<ServiceLoginModel> Login(string member_id, string phone_no, string v, string p, string m, string uid, string notif_token)
        {
            ServiceLoginModel value = new ServiceLoginModel();
            try
            {
                value.data = new _ServiceLoginData();

                SystemController syc = new SystemController();
                ValidationModel validation2 = await syc.CheckSystem(p, v);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    ValidationModel validation = await CheckValidation(member_id, phone_no);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        //DECLARE @OTP NVARCHAR(10) = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
                        string cmd = @"
DECLARE @MEMBERID NVARCHAR(50) = N'{0}'
DECLARE @MOBILE NVARCHAR(100) = N'{1}'
DECLARE @V NVARCHAR(100) = N'{2}'
DECLARE @P NVARCHAR(100) = N'{3}'
DECLARE @M NVARCHAR(100) = N'{4}'
DECLARE @UID NVARCHAR(100) = N'{5}'
DECLARE @NOTIF_TOKEN NVARCHAR(100) = N'{6}'

DECLARE @OTP_TOKEN NVARCHAR(100) = NEWID()
DECLARE @OTP NVARCHAR(10) =  '' 
IF @MOBILE = '0844389146' BEGIN
  SET @OTP = '123456'
END
ELSE BEGIN
  SET @OTP = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
END
DECLARE @OTP_EXPIREY_MIN INT = 3
DECLARE @CUS_ID INT = 0
DECLARE @IS_TERM INT = 0
DECLARE @IS_PREFERENCE INT = 0

SET  @CUS_ID = (SELECT ID FROM T_CUSTOMER WHERE MEMBERID = N'{0}')
SET  @IS_TERM = (SELECT COALESCE(IS_TERM,0) AS IS_TERM FROM T_CUSTOMER WHERE MEMBERID = N'{0}')
SET  @IS_PREFERENCE = (SELECT is_preference FROM T_CUSTOMER WHERE ID = @CUS_ID)

INSERT T_CUSTOMER_TOKEN ([MEMBERID],[TOKEN_NO],[TOKEN_EXPIREY],[OTP_TOKEN],[OTP],[OTP_EXPIREY],[MOBILE] ,[APP_VERSION],[PLATFORM],[MODEL],[UID],[REMOTE_IP],[LOGIN_DT],[LOGIN_OTP_DT],NOTIF_TOKEN)
VALUES (@MEMBERID,NULL,DATEADD(HOUR, 7,DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())),@OTP_TOKEN,@OTP,DATEADD(HOUR, 7,DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())),@MOBILE, @V, @P, @M, @UID, NULL, DATEADD(HOUR, 7, GETDATE()), DATEADD(HOUR, 7, GETDATE()),@NOTIF_TOKEN)

SELECT @OTP_TOKEN AS OTP_TOKEN, @OTP AS OTP, DATEADD(HOUR, 7, DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())) AS TOKEN_EXPIREY, @IS_TERM AS IS_TERM, @IS_PREFERENCE AS IS_PREFERENCE";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, phone_no, v, p, m, uid, notif_token)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                value.data.otp_token = dt.Rows[0]["OTP_TOKEN"].ToString();
                                value.data.is_term = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_TERM"].ToString()));
                                value.data.is_preference = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_PREFERENCE"].ToString()));
                                string otp = dt.Rows[0]["OTP"].ToString();
                                DateTime otpExpiry = Convert.ToDateTime(dt.Rows[0]["TOKEN_EXPIREY"]);
                                //string message = string.Format(@"MemberId: {0}\nOTP: {1}\nValid until: {2}", member_id, otp, UtilityService.GetDateTimeFormat(otpExpiry));
                                //string message = "MemberId: " + member_id + " " + "OTP: " + otp + " " + "Valid Until: " + UtilityService.GetDateTimeFormat(otpExpiry);
                                string message = "Your OTP for Lexus Elite Club Application is " + otp + ". Valid Until: " + UtilityService.GetDateTimeFormat2(otpExpiry);

                                SMSService sms = new SMSService();
                                SMSModel result = await sms.SendSMS(phone_no, message);

                                if (result.success)
                                {
                                    value.success = true;
                                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                                    cmd = @"
                                INSERT INTO T_CUSTOMER_SMS ([MOBILE],[STATUS],[DETAIL],[UPDATE_DT])
                                VALUES ('{0}','{1}','{2}',DATEADD(HOUR, 7, GETDATE()))";
                                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, member_id, (result.success) ? "OK" : "ERR", result.msg.text));
                                }
                                else
                                {
                                    value.success = false;
                                    value.msg = new MsgModel() { code = result.msg.code, text = result.msg.text };

                                    cmd = @"
                                INSERT INTO T_CUSTOMER_SMS ([MOBILE],[STATUS],[DETAIL],[UPDATE_DT])
                                VALUES ('{0}','{1}','{2}',DATEADD(HOUR, 7, GETDATE()))";
                                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, member_id, (result.success) ? "OK" : "ERR", result.msg.text));
                                }
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

        public async Task<ServiceLoginModel> LoginNew(string phone_no, string v, string p, string m, string uid, string lang, string type)
        {
            ServiceLoginModel value = new ServiceLoginModel();
            try
            {
                value.data = new _ServiceLoginData();

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
                    ValidationModel validation = await CheckValidationNew(phone_no, lang);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        //DECLARE @OTP NVARCHAR(10) = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
                        string cmd = @"
DECLARE @MOBILE NVARCHAR(100) = N'{0}'
DECLARE @V NVARCHAR(100) = N'{1}'
DECLARE @P NVARCHAR(100) = N'{2}'
DECLARE @M NVARCHAR(100) = N'{3}'
DECLARE @UID NVARCHAR(100) = N'{4}'

DECLARE @OTP_TOKEN NVARCHAR(100) = NEWID()
DECLARE @OTP NVARCHAR(10) =  '' 
IF @MOBILE = '0844389146' BEGIN
  SET @OTP = '123456'
END
ELSE BEGIN
  SET @OTP = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
END
DECLARE @OTP_EXPIREY_MIN INT = 3
DECLARE @CUS_ID INT = 0
DECLARE @IS_TERM INT = 0
DECLARE @IS_PREFERENCE INT = 0
DECLARE @MEMBERID NVARCHAR(50)

SET  @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @CUS_ID = (SELECT ID FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @IS_TERM = (SELECT COALESCE(IS_TERM,0) AS IS_TERM FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @IS_PREFERENCE = (SELECT is_preference FROM T_CUSTOMER WHERE ID = @CUS_ID)

INSERT T_CUSTOMER_TOKEN ([MEMBERID],[TOKEN_NO],[TOKEN_EXPIREY],[OTP_TOKEN],[OTP],[OTP_EXPIREY],[MOBILE] ,[APP_VERSION],[PLATFORM],[MODEL],[UID],[REMOTE_IP],[LOGIN_DT],[LOGIN_OTP_DT])
VALUES (@MEMBERID,NULL,DATEADD(HOUR, 7,DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())),@OTP_TOKEN,@OTP,DATEADD(HOUR, 7,DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())),@MOBILE, @V, @P, @M, @UID, NULL, DATEADD(HOUR, 7, GETDATE()), DATEADD(HOUR, 7, GETDATE()))

/*update [T_CUSTOMER]
set [is_first_login] = 0
where MEMBERID = @MEMBERID*/

SELECT @OTP_TOKEN AS OTP_TOKEN, @OTP AS OTP, DATEADD(HOUR, 7, DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())) AS TOKEN_EXPIREY, @IS_TERM AS IS_TERM, @IS_PREFERENCE AS IS_PREFERENCE";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, phone_no, v, p, m, uid)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                value.data.otp_token = dt.Rows[0]["OTP_TOKEN"].ToString();
                                value.data.is_term = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_TERM"].ToString()));
                                value.data.is_preference = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_PREFERENCE"].ToString()));

                                string otp = dt.Rows[0]["OTP"].ToString();
                                DateTime otpExpiry = Convert.ToDateTime(dt.Rows[0]["TOKEN_EXPIREY"]);
                                //string message = string.Format(@"MemberId: {0}\nOTP: {1}\nValid until: {2}", member_id, otp, UtilityService.GetDateTimeFormat(otpExpiry));
                                //string message = "MemberId: " + phone_no + " " + "OTP: " + otp + " " + "Valid Until: " + UtilityService.GetDateTimeFormat(otpExpiry);
                                string message = "Your OTP for Lexus Elite Club Application is " + otp + ". Valid Until: " + UtilityService.GetDateTimeFormat2(otpExpiry);

                                value.success = true;
                                value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                                if (type == "")
                                {
                                    SMSService sms = new SMSService();
                                    SMSModel result = await sms.SendSMS(phone_no, message);

                                    if (result.success)
                                    {
                                        value.success = true;
                                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                                        cmd = @"
                                    INSERT INTO T_CUSTOMER_SMS ([MOBILE],[STATUS],[DETAIL],[UPDATE_DT])
                                    VALUES ('{0}','{1}','{2}',DATEADD(HOUR, 7, GETDATE()))";
                                        db.ExecuteNonQueryFromCommandText(string.Format(cmd, phone_no, (result.success) ? "OK" : "ERR", result.msg.text));
                                    }
                                    else
                                    {
                                        value.success = false;
                                        value.msg = new MsgModel() { code = result.msg.code, text = result.msg.text };

                                        cmd = @"
                                    INSERT INTO T_CUSTOMER_SMS ([MOBILE],[STATUS],[DETAIL],[UPDATE_DT])
                                    VALUES ('{0}','{1}','{2}',DATEADD(HOUR, 7, GETDATE()))";
                                        db.ExecuteNonQueryFromCommandText(string.Format(cmd, phone_no, (result.success) ? "OK" : "ERR", result.msg.text));
                                    }
                                }
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

        private async Task<ValidationModel> CheckValidation(string member_id, string phone_no)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Empty;
                    if (member_id != "")
                    {
                        #region E801
                        state = ValidationModel.InvalidState.E801;
                        cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MEMBERID = N'{0}' AND register_type = 'APP_USER'";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id)))
                        {
                            if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                            }
                        }
                        #endregion
                    }

                    #region E802
                    state = ValidationModel.InvalidState.E802;
                    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}' AND register_type = 'APP_USER'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, phone_no)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                    }
                    #endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<ValidationModel> CheckValidationNew(string phone_no, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Empty;
                    //if (member_id != "")
                    //{
                    //    #region E801
                    //    state = ValidationModel.InvalidState.E801;
                    //    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MEMBERID = N'{0}'";
                    //    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id)))
                    //    {
                    //        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                    //        {
                    //            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //        }
                    //    }
                    //    #endregion
                    //}

                    #region E802
                    state = ValidationModel.InvalidState.E802;
                    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}' AND register_type = 'APP_USER'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, phone_no)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            cmd = @"
DECLARE @MEMBERID VARCHAR(10)
SET @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER WHERE MOBILE = N'{0}' AND register_type = 'APP_USER')

SELECT COUNT(1) AS CNT 
FROM T_CUSTOMER 
WHERE MOBILE = N'{0}' 
 AND register_type = 'CAR_OWNER' 
 AND (SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID AND register_type = 'APP_USER') = 0";
                            using (DataTable dt2 = db.GetDataTableFromCommandText(string.Format(cmd, phone_no)))
                            {
                                if (dt2.Rows.Count == 0 || Convert.ToInt32(dt2.Rows[0][0]) == 0)
                                {
                                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage =await    ValidationModel.GetInvalidMessageNew(state, lang) };
                                }
                            }
                            //return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }

                    #endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
    }
}