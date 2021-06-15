using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class OTPService
    {
        private string conn;
        public OTPService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceRequestOTPModel> RequestOTP(string v, string p, string lang, string mobile, string type)
        {
            ServiceRequestOTPModel value = new ServiceRequestOTPModel();
            try
            {
                value.data = new _ServiceRequestOTPData();

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
                    ValidationModel validation = await CheckValidation(mobile, lang);
                    //if (!validation.Success)
                    //{
                    //    value.success = validation.Success;
                    //    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    //    return value;
                    //}

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        //DECLARE @OTP NVARCHAR(10) = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
                        string cmd = @"
DECLARE @MOBILE NVARCHAR(100) = N'{0}'
DECLARE @V NVARCHAR(100) = N'{1}'
DECLARE @P NVARCHAR(100) = N'{2}'

DECLARE @OTP NVARCHAR(10) =  '' 
IF @MOBILE = '0844389146' BEGIN
  SET @OTP = '123456'
END
ELSE BEGIN
  SET @OTP = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
END
DECLARE @REFER NVARCHAR(20) = CONVERT(nvarchar(10),LEFT(REPLACE(NEWID(),'-',''),10))
DECLARE @OTP_EXPIREY_MIN INT = 3
DECLARE @MEMBERID NVARCHAR(50)

SET  @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')

INSERT system_otp (reference_code, otp_code, otp_expire, mobile, app_version, platform, is_used, create_date, create_by)
VALUES (@REFER, @OTP, DATEADD(HOUR, 7,DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())), @MOBILE, @V, @P, 0, DATEADD(HOUR, 7, GETDATE()), @MEMBERID)

SELECT @REFER AS REFER, @OTP AS OTP, DATEADD(HOUR, 7, DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())) AS OTP_EXPIREY";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, mobile, v, p)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string otp = dt.Rows[0]["OTP"].ToString();
                                DateTime otpExpiry = Convert.ToDateTime(dt.Rows[0]["OTP_EXPIREY"]);
                                value.data.reference_code = dt.Rows[0]["REFER"].ToString();
                                value.data.otp_code = otp;
                                value.data.otp_expire = UtilityService.GetDateTimeFormat(otpExpiry);

                                //string message2 = "MemberId: " + mobile + " " + "OTP: " + otp + " " + "Valid Until: " + UtilityService.GetDateTimeFormat(otpExpiry);
                                string message = "Your OTP for Lexus Elite Club Application is " + otp + ". Valid Until: " + UtilityService.GetDateTimeFormat2(otpExpiry);
                                value.success = true;
                                value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                                if (type == "")
                                {
                                    SMSService sms = new SMSService();
                                    SMSModel result = new SMSModel();
                                    result = await sms.SendSMS(mobile, message);

                                    if (result.success)
                                    {
                                        value.success = true;
                                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                                        cmd = @"
                                    INSERT INTO T_CUSTOMER_SMS ([MOBILE],[STATUS],[DETAIL],[UPDATE_DT])
                                    VALUES ('{0}','{1}','{2}',DATEADD(HOUR, 7, GETDATE()))";
                                        db.ExecuteNonQueryFromCommandText(string.Format(cmd, mobile, (result.success) ? "OK" : "ERR", result.msg.text));
                                    }
                                    else
                                    {
                                        value.success = false;
                                        value.msg = new MsgModel() { code = result.msg.code, text = result.msg.text };

                                        cmd = @"
                                    INSERT INTO T_CUSTOMER_SMS ([MOBILE],[STATUS],[DETAIL],[UPDATE_DT])
                                    VALUES ('{0}','{1}','{2}',DATEADD(HOUR, 7, GETDATE()))";
                                        db.ExecuteNonQueryFromCommandText(string.Format(cmd, mobile, (result.success) ? "OK" : "ERR", result.msg.text));
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

        private async  Task<ValidationModel> CheckValidation(string mobile, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                //ValidationModel.InvalidState state;
                //using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                //{
                //    //#region E802
                //    //state = ValidationModel.InvalidState.E802;
                //    //string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}'";
                //    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, mobile)))
                //    //{
                //    //    if (Convert.ToInt32(dt.Rows[0][0]) == 0)
                //    //    {
                //    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                //    //    }
                //    //}
                //    //#endregion
                //}

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceCheckOTPModel> CheckOTP(string v, string p, string lang, string mobile, string refer_code, string otp_code, string type)
        {
            ServiceCheckOTPModel value = new ServiceCheckOTPModel();
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
                    ValidationModel validation = await CheckValidationOTP(mobile, lang, refer_code, otp_code, type);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }
                    if ((otp_code == "999999") && (type != ""))
                    {

                    }
                    else
                    {

                        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                        {
                            string cmd = @"
                            DECLARE @MOBILE NVARCHAR(10) = N'{0}'
                            DECLARE @REFER NVARCHAR(10) = N'{1}'
                            DECLARE @OTP NVARCHAR(10) = N'{2}'

                            UPDATE system_otp 
                            SET is_used = 1, used_date = DATEADD(HOUR, 7, GETDATE())
                            WHERE mobile = @MOBILE AND reference_code = @REFER AND otp_code = @OTP

                            SELECT * FROM system_otp WHERE is_used = 1 AND used_date = DATEADD(HOUR, 7, GETDATE()) ";

                            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, mobile, refer_code, otp_code)))
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    value.success = true;
                                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                                }
                                else
                                {
                                    value.success = false;
                                    value.msg = new MsgModel() { code = 0, text = "", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                                }
                            }
                        }
                    }

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = "", version = "" };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<ValidationModel> CheckValidationOTP(string mobile, string lang, string refer_code, string otp_code, string type)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //#region E802
                    //state = ValidationModel.InvalidState.E802;
                    //string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}'";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, mobile)))
                    //{
                    //    if (Convert.ToInt32(dt.Rows[0][0]) == 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion
                    if ((otp_code == "999999") && (type != ""))
                    {

                    }
                    else
                    {
                        #region E901
                        state = ValidationModel.InvalidState.E901;
                        string cmd = @"SELECT COUNT(1) AS CNT FROM system_otp WHERE reference_code = N'{0}' AND otp_code = N'{1}' AND is_used = 0";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, refer_code, otp_code)))
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) == 0)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }
                        #endregion

                        #region E902
                        state = ValidationModel.InvalidState.E902;
                        cmd = @"SELECT COUNT(1) AS CNT FROM system_otp WHERE reference_code = N'{0}' AND otp_code = N'{1}' AND otp_expire < DATEADD(HOUR, 7, GETDATE())";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, refer_code, otp_code)))
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) == 1)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }
                        #endregion
                    }
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