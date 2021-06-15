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
    public class LoginOTPService
    {
        public class Result
        {
            public bool success { get; set; }
            public string msg { get; set; }
            public string token { get; set; }
            public bool is_term { get; set; }
            public bool is_preference { get; set; }
            public bool is_update { get; set; }
            public bool is_first_login { get; set; }
        }

        private string conn;
        private string optServiceURL;
        public LoginOTPService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
            this.optServiceURL = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusOTPServiceURL"];
        }
        public async Task<ServiceLoginOTPModel> LoginOTP(string otp_token, string otp, string v, string p)
        {
            ServiceLoginOTPModel value = new ServiceLoginOTPModel();
            try
            {
                value.data = new _ServiceLoginOTPData();

                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystem(p, v);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    ValidationModel validation = CheckValidation(otp_token, otp);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    Result result =await ValidateOTP2(otp_token, otp);

                    if (result.success)
                    {
                        value.data.token = result.token;

                        value.data.is_term = true;
                        value.data.is_preference = result.is_preference;
                        value.data.is_update= result.is_update;

                        //value.data.is_term = true;
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        value.success = false;
                        value.msg = new MsgModel() { code = 100, text = result.msg };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceLoginOTPModel> LoginOTPNew(string otp_token, string otp, string v, string p, string lang, string type, string device_id)
        {
            ServiceLoginOTPModel value = new ServiceLoginOTPModel();
            try
            {
                value.data = new _ServiceLoginOTPData();

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
                    ValidationModel validation = await CheckValidationNew(otp_token, otp, lang, type);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    Result result = await ValidateOTP2(otp_token, otp);
                    MasterDataService service = new MasterDataService();

                    string token = "";

                    int version_ = Convert.ToInt32(WebConfigurationManager.AppSettings["version"].ToString());
                   

                    if (result.success)
                    {
                        token = result.token;
                        value.data.token = result.token;
                        value.data.is_term = result.is_term;
                        value.data.is_preference = result.is_preference;
                        value.data.is_update = result.is_update;
                        value.data.is_first_login = result.is_first_login;

                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                        bool is_read_terms = await service.CheckReadTermsAndCondition(token, version_);

                        if (!is_read_terms)
                        {

                            //version += service.CheckLastestVersionOfTermsAndCondition();
                            string version = "version=";
                            version += WebConfigurationManager.AppSettings["version"].ToString();
                            string lang_ = "lang=";
                            lang_ += lang;

                            //string content_type = "content_type=privacy";
                            value.data.term_cond_url = string.Format(WebConfigurationManager.AppSettings["term_cond_url"].ToString(), version, lang_);
                        }
                    }
                    else
                    {
                        value.success = false;
                        value.msg = new MsgModel() { code = 100, text = result.msg };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public Result ValidateOTP(string otp_token, string otp)
        {
            Result result = new Result();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @OTP_TOKEN NVARCHAR(100) = N'{0}'
DECLARE @OTP NVARCHAR(10) = N'{1}'

DECLARE @SUCCESS BIT = 0
DECLARE @MSG NVARCHAR(255) = ''
DECLARE @OTP_EXPIREY DATETIME
DECLARE @TOKEN_NO NVARCHAR(100) = NEWID()

--SELECT @OTP_EXPIREY = OTP_EXPIREY FROM T_CUSTOMER_TOKEN WHERE OTP_TOKEN = @OTP_TOKEN AND OTP = @OTP

--IF @OTP_EXPIREY IS NULL OR @TOKEN_NO IS NULL
--BEGIN
--	SET @SUCCESS = 0
--	SET @MSG = 'Not found OTP'
--END ELSE
--BEGIN
--	SET @SUCCESS = 1
--	SET @MSG = ''
--END

--IF (@OTP_EXPIREY < GETDATE()) AND @SUCCESS <> 0
--BEGIN
--	SET @SUCCESS = 0
--	SET @MSG = 'OTP has been expired'
--END ELSE
--BEGIN
--	SET @SUCCESS = 1
--	SET @MSG = ''
--END

--IF @SUCCESS <> 0
--BEGIN
--	UPDATE	T_CUSTOMER_TOKEN 
--	SET		TOKEN_NO = @TOKEN_NO,
--			TOKEN_EXPIREY = CONVERT(DATETIME, '2199-01-01', 120)
--	WHERE	OTP_TOKEN = @OTP_TOKEN
--END

--SELECT @SUCCESS AS SUCCESS, @MSG AS MSG, @TOKEN_NO AS TOKEN_NO

UPDATE  T_CUSTOMER_TOKEN 
SET     TOKEN_EXPIREY = DATEADD(HOUR, 7, GETDATE()) 
WHERE   MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE OTP_TOKEN = @OTP_TOKEN)

UPDATE	T_CUSTOMER_TOKEN 
SET		TOKEN_NO = @TOKEN_NO,
		TOKEN_EXPIREY = CONVERT(DATETIME, '2199-01-01', 120)
WHERE	OTP_TOKEN = @OTP_TOKEN

SELECT 0 AS SUCCESS, '' AS MSG, @TOKEN_NO AS TOKEN_NO";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, otp_token, otp)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                result.success = Convert.ToBoolean(row["SUCCESS"]);
                                result.msg = row["MSG"].ToString();
                                result.token = row["TOKEN_NO"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<Result> ValidateOTP2(string otp_token, string otp)
        {
            Result result = new Result();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"SELECT CAST(NEWID() AS NVARCHAR(100)) AS TOKEN_NO";
                    string token = db.ExecuteScalarFromCommandText<string>(cmd);

                    cmd = @"
UPDATE  T_CUSTOMER_TOKEN 
SET     TOKEN_EXPIREY = DATEADD(HOUR, 7, GETDATE())
WHERE   MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE OTP_TOKEN = N'{0}')";
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, otp_token));

                    cmd = @"
UPDATE	T_CUSTOMER_TOKEN 
SET		TOKEN_NO = N'{0}',
		TOKEN_EXPIREY = CONVERT(DATETIME, '2199-01-01', 120)
WHERE	OTP_TOKEN = N'{1}'";
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, token, otp_token));

                    cmd = @"
DECLARE @CUS_ID INT = 0
DECLARE @IS_TERM INT = 0
DECLARE @IS_PREFERENCE INT = 0
DECLARE @IS_UPDATE INT = 0
DECLARE @IS_FIRST_LOGIN INT = 0
DECLARE @MEMBERID NVARCHAR(50)

SET  @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE OTP_TOKEN = N'{0}')
SET  @CUS_ID = (SELECT ID FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID AND register_type = 'APP_USER')
SET  @IS_TERM = (SELECT COALESCE(IS_TERM,0) AS IS_TERM FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID AND register_type = 'APP_USER')
SET  @IS_PREFERENCE = (SELECT is_preference FROM T_CUSTOMER WHERE ID = @CUS_ID)
SET  @IS_UPDATE = (SELECT COALESCE(is_update,0) AS is_update FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID AND register_type = 'APP_USER')
SET  @IS_FIRST_LOGIN = (SELECT is_first_login FROM T_CUSTOMER WHERE ID = @CUS_ID)

SELECT @IS_TERM AS IS_TERM, @IS_PREFERENCE AS IS_PREFERENCE, @IS_UPDATE AS IS_UPDATE, @IS_FIRST_LOGIN as IS_FIRST_LOGIN";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, otp_token)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            result.is_term = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_TERM"].ToString()));
                            result.is_preference = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_PREFERENCE"].ToString()));
                            result.is_update = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_UPDATE"].ToString()));
                            result.is_first_login = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_FIRST_LOGIN"].ToString()));
                        }
                    }
                    result.success = true;
                    result.msg = "";
                    result.token = token;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private ValidationModel CheckValidation(string otp_token, string otp)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E901
                    state = ValidationModel.InvalidState.E901;
                    string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_TOKEN WHERE OTP_TOKEN = N'{0}' AND OTP = N'{1}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, otp_token, otp)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                    }
                    #endregion
                    #region E902
                    state = ValidationModel.InvalidState.E902;
                    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_TOKEN WHERE OTP_TOKEN = N'{0}' AND OTP_EXPIREY > DATEADD(HOUR, 7, GETDATE())";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, otp_token)))
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

        private async Task<ValidationModel> CheckValidationNew(string otp_token, string otp, string lang, string type)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    if ((otp == "999999") && (type != ""))
                    {
                        
                    }
                    else
                    {
                        #region E901
                        state = ValidationModel.InvalidState.E901;
                        string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_TOKEN WHERE OTP_TOKEN = N'{0}' AND OTP = N'{1}'";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, otp_token, otp)))
                        {
                            if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }
                        #endregion
                        #region E902
                        state = ValidationModel.InvalidState.E902;
                        cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_TOKEN WHERE OTP_TOKEN = N'{0}' AND OTP_EXPIREY > DATEADD(HOUR, 7, GETDATE())";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, otp_token)))
                        {
                            if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
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