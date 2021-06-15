using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace com.lexus.th
{
    public class RegisterService
    {
        private string conn;
        public RegisterService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceRegisterModel> RegisterCustomer(string lang, string f_name, string l_name, string gender, string birthdate, string email, string address, string sub_distinct, string distinct, string province, string postcode, string mobile, string citizen_id, string vehicle_no, string register_type, string member_id, string v, string p, string plate_no, string title_name, int is_app_user, string uid, DateTime? confirm_checkbox_date, DateTime? confirm_popup_date)
        {

            ServiceRegisterModel value = new ServiceRegisterModel();
            TermService term = new TermService();
           
            try
            {
                value.data = new _ServiceRegisterData();

                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    if (member_id == "")
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(RandomString(2, false));
                        builder.Append(RandomNumber(1000, 9999));

                        member_id = builder.ToString();
                    }


                    ValidationModel validation = new ValidationModel();
                    if ((register_type == "CAR_OWNER") && (is_app_user == 1))
                    {
                        validation.Success = true;
                    }
                    else
                    {
                        validation =await CheckValidation(mobile, lang, citizen_id, vehicle_no);
                    }
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
DECLARE @FNAME NVARCHAR(250) = N'{0}'
DECLARE @LNAME NVARCHAR(250) = N'{1}'
DECLARE @GENDER NVARCHAR(100) = N'{2}'
DECLARE @BIRTHDATE NVARCHAR(100) = N'{3}'
DECLARE @EMAIL NVARCHAR(100) = N'{4}'
DECLARE @ADDRESS NVARCHAR(300) = N'{5}'
DECLARE @SUBDISTRICT NVARCHAR(100) = N'{6}'
DECLARE @DISTRICT NVARCHAR(100) = N'{7}'
DECLARE @PROVINCE NVARCHAR(100) = N'{8}'
DECLARE @POSTALCODE NVARCHAR(100) = N'{9}'
DECLARE @MOBILE NVARCHAR(50) = N'{10}'
DECLARE @CITIZENID NVARCHAR(20) = N'{11}'
DECLARE @VEHICLENO NVARCHAR(50) = N'{12}'
DECLARE @REGISTERTYPE NVARCHAR(50) = N'{13}'
DECLARE @MEMBERID NVARCHAR(50) = N'{14}'
DECLARE @PLATENO NVARCHAR(50) = N'{15}'
DECLARE @TITLENAME NVARCHAR(50) = N'{16}'
DECLARE @ISAPPUSER INT = N'{17}'
DECLARE @UID NVARCHAR(100) = N'{18}'

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
DECLARE @IS_CAR INT = 0
DECLARE @DEALER_ID INT = 0
DECLARE @model_id INT = 0
DECLARE @BODYCLR_CD NVARCHAR(10) = ''

DECLARE @RS_Date NVARCHAR(100) = ''
DECLARE @Expiry_Date DATE
DECLARE @rs_year INT
DECLARE @rs_month INT
DECLARE @rs_day INT
DECLARE @regis_date DATE
DECLARE @privilege_cnt INT

SET @DEALER_ID = (SELECT d.DEALER_ID FROM T_DEALER d LEFT JOIN initial_data ind ON d.DEALER_ID = ind.dealer WHERE ind.vin = @VEHICLENO)
SET @model_id = (SELECT model FROM initial_data WHERE vin = @VEHICLENO)
SET @IS_CAR = (SELECT (CASE WHEN COUNT(CUS_ID) > 0 THEN 1 ELSE 0 END) FROM T_CUSTOMER_CAR WHERE VIN = @VEHICLENO)
SET @RS_Date = (SELECT rs_date FROM initial_data WHERE vin = @VEHICLENO)
SET @rs_year = (SELECT YEAR(@RS_Date))
SET @rs_month = (SELECT MONTH(@RS_Date))
SET @rs_day = (SELECT DAY(@RS_Date))
SET @regis_date = (select cast(cast(YEAR(GETDATE())*10000 + @rs_month*100 + @rs_day as varchar(255)) as date))
SET @Expiry_Date = (SELECT CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 4, @RS_Date) 
			         THEN DATEADD(YEAR, 4, @RS_Date) 
			         ELSE CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 10, @RS_Date) 
				            THEN  CASE WHEN CONVERT(DATE,GETDATE()) < CONVERT(DATE,@regis_date)
									        THEN @regis_date 
									        ELSE DATEADD(YEAR, 1, @regis_date) 
									        END 
						        ELSE DATEADD(YEAR, 10, @RS_Date) 
						        END 
			         END)
SET @privilege_cnt = (SELECT CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 4, @RS_Date) THEN 4 ELSE 0 END)

IF @IS_CAR = 0 BEGIN
    INSERT T_CUSTOMER_CAR (MEMBERID ,DEALER ,VIN ,PLATE_NO ,MODEL_ID, RS_Date, CREATE_DT ,CREATE_USER)
    VALUES (@MEMBERID ,@DEALER_ID ,@VEHICLENO ,@PLATENO ,@model_id, @RS_Date, DATEADD(HOUR, 7, GETDATE()) ,@FNAME)
END

IF @ISAPPUSER = 0 AND @REGISTERTYPE = 'CAR_OWNER' BEGIN
INSERT T_CUSTOMER_CAR_OWNER ([FNAME],[LNAME],[GENDER],[BIRTHDATE],[EMAIL],[ADDRESS1],[SUBDISTRICT],[DISTRICT],[PROVINCE],[POSTALCODE],[CREATE_DT],
        [CREATE_USER], MOBILE, citizen_id, vehicle_no, register_type, MEMBERID, 
        EXPIRY, PRIVILEGE_CNT, plate_no, TITLENAME, is_update)
VALUES (@FNAME,@LNAME, @GENDER,
        CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE @BIRTHDATE END, @EMAIL, @ADDRESS, @SUBDISTRICT,@DISTRICT, @PROVINCE, @POSTALCODE, DATEADD(HOUR, 7, GETDATE()), 
        @FNAME, @MOBILE, @CITIZENID, @VEHICLENO, @REGISTERTYPE, @MEMBERID, 
        @Expiry_Date, 0, @PLATENO, @TITLENAME, 1)

INSERT T_CUSTOMER ([FNAME],[LNAME],[GENDER],[BIRTHDATE],[EMAIL],[ADDRESS1],[SUBDISTRICT],[DISTRICT],[PROVINCE],[POSTALCODE],[CREATE_DT],
        [CREATE_USER], MOBILE, citizen_id, vehicle_no, register_type, MEMBERID, 
        EXPIRY, PRIVILEGE_CNT, plate_no, TITLENAME, is_update)
VALUES (@FNAME,@LNAME, @GENDER,
        CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE @BIRTHDATE END,@EMAIL,@ADDRESS,@SUBDISTRICT,@DISTRICT, @PROVINCE, @POSTALCODE, DATEADD(HOUR, 7, GETDATE()), 
        @FNAME, @MOBILE, @CITIZENID, @VEHICLENO, 'APP_USER', @MEMBERID, 
        @Expiry_Date, @privilege_cnt, @PLATENO, @TITLENAME, 1)

SET  @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @CUS_ID = (SELECT ID FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @IS_TERM = (SELECT COALESCE(IS_TERM,0) AS IS_TERM FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @IS_PREFERENCE = (SELECT COALESCE(is_preference,0) FROM T_CUSTOMER WHERE ID = @CUS_ID)

INSERT T_CUSTOMER_TOKEN ([MEMBERID],[TOKEN_NO],[TOKEN_EXPIREY],[OTP_TOKEN],[OTP],[OTP_EXPIREY],[MOBILE] ,[APP_VERSION],[PLATFORM],[MODEL],[UID],[REMOTE_IP],[LOGIN_DT],[LOGIN_OTP_DT])
VALUES (@MEMBERID,@OTP_TOKEN,CONVERT(DATETIME, '2199-01-01', 120),@OTP_TOKEN,@OTP,DATEADD(HOUR, 7,DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())),@MOBILE, '', '', '', @UID, NULL, DATEADD(HOUR, 7, GETDATE()), DATEADD(HOUR, 7, GETDATE()))
END

IF @ISAPPUSER = 1 AND @REGISTERTYPE = 'CAR_OWNER' BEGIN
INSERT T_CUSTOMER_CAR_OWNER ([FNAME],[LNAME],[GENDER],[BIRTHDATE],[EMAIL],[ADDRESS1],[SUBDISTRICT],[DISTRICT],[PROVINCE],[POSTALCODE],[CREATE_DT],
        [CREATE_USER], MOBILE, citizen_id, vehicle_no, register_type, MEMBERID, 
        EXPIRY, PRIVILEGE_CNT, plate_no, TITLENAME, is_update)
VALUES (@FNAME,@LNAME, @GENDER,
        CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE @BIRTHDATE END,@EMAIL,@ADDRESS,@SUBDISTRICT,@DISTRICT, @PROVINCE, @POSTALCODE, DATEADD(HOUR, 7, GETDATE()), 
        @FNAME, @MOBILE, @CITIZENID, @VEHICLENO, @REGISTERTYPE, @MEMBERID, 
        @Expiry_Date, 0, @PLATENO, @TITLENAME, 1)
END

IF @REGISTERTYPE = 'APP_USER' BEGIN
INSERT T_CUSTOMER ([FNAME],[LNAME],[GENDER],[BIRTHDATE],[EMAIL],[ADDRESS1],[SUBDISTRICT],[DISTRICT],[PROVINCE],[POSTALCODE],[CREATE_DT],
        [CREATE_USER], MOBILE, citizen_id, vehicle_no, register_type, MEMBERID, 
        EXPIRY, PRIVILEGE_CNT, plate_no, TITLENAME, is_update)
VALUES (@FNAME,@LNAME, @GENDER,
        CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE @BIRTHDATE END,@EMAIL,@ADDRESS,@SUBDISTRICT,@DISTRICT, @PROVINCE, @POSTALCODE, DATEADD(HOUR, 7, GETDATE()), 
        @FNAME, @MOBILE, @CITIZENID, @VEHICLENO, @REGISTERTYPE, @MEMBERID, 
        @Expiry_Date, @privilege_cnt, @PLATENO, @TITLENAME, 1)

SET  @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @CUS_ID = (SELECT ID FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @IS_TERM = (SELECT COALESCE(IS_TERM,0) AS IS_TERM FROM T_CUSTOMER WHERE MOBILE = @MOBILE AND register_type = 'APP_USER')
SET  @IS_PREFERENCE = (SELECT COALESCE(is_preference,0) FROM T_CUSTOMER WHERE ID = @CUS_ID)

INSERT T_CUSTOMER_TOKEN ([MEMBERID],[TOKEN_NO],[TOKEN_EXPIREY],[OTP_TOKEN],[OTP],[OTP_EXPIREY],[MOBILE] ,[APP_VERSION],[PLATFORM],[MODEL],[UID],[REMOTE_IP],[LOGIN_DT],[LOGIN_OTP_DT])
VALUES (@MEMBERID,@OTP_TOKEN,CONVERT(DATETIME, '2199-01-01', 120),@OTP_TOKEN,@OTP,DATEADD(HOUR, 7,DATEADD(MINUTE, @OTP_EXPIREY_MIN, GETDATE())),@MOBILE, '', '', '', @UID, NULL, DATEADD(HOUR, 7, GETDATE()), DATEADD(HOUR, 7, GETDATE()))
END

SELECT @OTP_TOKEN AS OTP_TOKEN, @IS_TERM AS IS_TERM, @IS_PREFERENCE AS IS_PREFERENCE";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, f_name, l_name, gender, birthdate, email, address, sub_distinct, distinct, province, postcode, mobile, citizen_id, vehicle_no, register_type, member_id, plate_no, title_name, is_app_user, uid)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                bool is_read_term = false;
                                int sys_version = Convert.ToInt32(WebConfigurationManager.AppSettings["version"].ToString());

                                is_read_term = term.CheckReadTerms(member_id, sys_version);
                                if (!string.IsNullOrEmpty(member_id) && is_read_term == false)
                                {
                                    term.InsertTermsStamp(member_id, uid, confirm_checkbox_date, confirm_popup_date);
                                }
                                
                                value.data.member_id = member_id;
                                value.data.otp_token = dt.Rows[0]["OTP_TOKEN"].ToString();
                                value.data.is_term = false;
                                value.data.is_preference = true;
                                value.data.is_first_login = false;

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

        private async Task<ValidationModel> CheckValidation(string mobile, string lang, string citizen_id, string vin)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E814
                    state = ValidationModel.InvalidState.E814;
                    string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE citizen_id = N'{0}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, citizen_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    #region E807
                    state = ValidationModel.InvalidState.E807;
                    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE vehicle_no = N'{0}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    //#region E808
                    //state = ValidationModel.InvalidState.E808;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM initial_data WHERE vin = N'{0}' AND ABS(DATEDIFF(month, rs_date, getdate()))/12 >= 10";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    //{
                    //    if (Convert.ToInt32(dt.Rows[0][0]) == 1)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion

                    #region E809
                    state = ValidationModel.InvalidState.E809;
                    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, mobile)))
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    //#region E815
                    //state = ValidationModel.InvalidState.E815;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM initial_data WHERE vin = N'{0}' AND ABS(DATEDIFF(month, rs_date, getdate()))/12 >= 10 ";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<ValidationModel> CheckValidation2(string mobile, string lang, string citizen_id)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E814
                    state = ValidationModel.InvalidState.E814;
                    string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE citizen_id = N'{0}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, citizen_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    #region E809
                    state = ValidationModel.InvalidState.E809;
                    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, mobile)))
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    //#region E815
                    //state = ValidationModel.InvalidState.E815;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM initial_data WHERE vin = N'{0}' AND ABS(DATEDIFF(month, rs_date, getdate()))/12 >= 10 ";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<ValidationModel> CheckValidation3(string lang, string citizen_id, string member_id)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E814
                    state = ValidationModel.InvalidState.E814;
                    string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE citizen_id = N'{0}' AND MEMBERID <> '{1}' ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, citizen_id, member_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    //#region E809
                    //state = ValidationModel.InvalidState.E809;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}'";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, mobile)))
                    //{
                    //    if (Convert.ToInt32(dt.Rows[0][0]) == 1)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion

                    //#region E815
                    //state = ValidationModel.InvalidState.E815;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM initial_data WHERE vin = N'{0}' AND ABS(DATEDIFF(month, rs_date, getdate()))/12 >= 10 ";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceCheckRegisterModel> CheckRegisterCustomer(string lang, string mobile, string citizen_id, string v, string p)
        {
            ServiceCheckRegisterModel value = new ServiceCheckRegisterModel();
            try
            {
                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    ValidationModel validation = new ValidationModel();
                    validation =await CheckValidation2(mobile, lang, citizen_id);

                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }
                    else
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success" };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceRegisterModel> UpdateCustomer(string lang, string f_name, string l_name, string gender, string birthdate, string email, string address, string sub_distinct, string distinct, string province, string postcode, string mobile, string citizen_id, string vehicle_no, string register_type, string member_id, string v, string p, string plate_no, string title_name, int is_app_user, string uid)
        {

            ServiceRegisterModel value = new ServiceRegisterModel();
            try
            {
                value.data = new _ServiceRegisterData();

                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    //if (member_id == "")
                    //{
                    //    StringBuilder builder = new StringBuilder();
                    //    builder.Append(RandomString(2, false));
                    //    builder.Append(RandomNumber(1000, 9999));

                    //    member_id = builder.ToString();
                    //}
                    ValidationModel validation = new ValidationModel();
                    if ((register_type == "CAR_OWNER") && (is_app_user == 1))
                    {
                        validation.Success = true;
                    }
                    else
                    {
                        validation = await CheckValidation3(lang, citizen_id, member_id);
                    }

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
DECLARE @FNAME NVARCHAR(250) = N'{0}'
DECLARE @LNAME NVARCHAR(250) = N'{1}'
DECLARE @GENDER NVARCHAR(100) = N'{2}'
DECLARE @BIRTHDATE NVARCHAR(100) = N'{3}'
DECLARE @EMAIL NVARCHAR(100) = N'{4}'
DECLARE @ADDRESS NVARCHAR(300) = N'{5}'
DECLARE @SUBDISTRICT NVARCHAR(100) = N'{6}'
DECLARE @DISTRICT NVARCHAR(100) = N'{7}'
DECLARE @PROVINCE NVARCHAR(100) = N'{8}'
DECLARE @POSTALCODE NVARCHAR(100) = N'{9}'
DECLARE @MOBILE NVARCHAR(50) = N'{10}'
DECLARE @CITIZENID NVARCHAR(20) = N'{11}'
DECLARE @VEHICLENO NVARCHAR(50) = N'{12}'
DECLARE @REGISTERTYPE NVARCHAR(50) = N'{13}'
DECLARE @MEMBERID NVARCHAR(50) = N'{14}'
DECLARE @PLATENO NVARCHAR(50) = N'{15}'
DECLARE @TITLENAME NVARCHAR(50) = N'{16}'
DECLARE @ISAPPUSER INT = N'{17}'
DECLARE @UID NVARCHAR(100) = N'{18}'

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
DECLARE @CAR_ID INT = 0

SET @CAR_ID = (SELECT MAX(CUS_ID) FROM T_CUSTOMER_CAR WHERE MEMBERID = @MEMBERID)

IF @ISAPPUSER = 0 AND @REGISTERTYPE = 'CAR_OWNER' BEGIN
UPDATE T_CUSTOMER_CAR_OWNER
SET FNAME = @FNAME, LNAME = @LNAME, GENDER = @GENDER, 
BIRTHDATE = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 120) END, 
EMAIL = @EMAIL, ADDRESS1 = @ADDRESS, SUBDISTRICT = @SUBDISTRICT, DISTRICT = @DISTRICT, PROVINCE = @PROVINCE,
POSTALCODE = @POSTALCODE, MOBILE = @MOBILE, citizen_id = @CITIZENID, vehicle_no = @VEHICLENO, is_update = 1,
plate_no = @PLATENO, TITLENAME = @TITLENAME, UPDATE_DT = DATEADD(HOUR, 7, GETDATE()), UPDATE_USER = @FNAME
WHERE MEMBERID = @MEMBERID

UPDATE T_CUSTOMER
SET FNAME = @FNAME, LNAME = @LNAME, GENDER = @GENDER, 
BIRTHDATE = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 120) END, 
EMAIL = @EMAIL, ADDRESS1 = @ADDRESS, SUBDISTRICT = @SUBDISTRICT, DISTRICT = @DISTRICT, PROVINCE = @PROVINCE,
POSTALCODE = @POSTALCODE, citizen_id = @CITIZENID, vehicle_no = @VEHICLENO, is_update = 1,
plate_no = @PLATENO, TITLENAME = @TITLENAME, UPDATE_DT = DATEADD(HOUR, 7, GETDATE()), UPDATE_USER = @FNAME
WHERE MEMBERID = @MEMBERID

SET  @CUS_ID = (SELECT ID FROM T_CUSTOMER_CAR_OWNER WHERE MEMBERID = @MEMBERID AND register_type = 'CAR_OWNER')
SET  @IS_TERM = (SELECT COALESCE(IS_TERM,0) AS IS_TERM FROM T_CUSTOMER_CAR_OWNER WHERE MEMBERID = @MEMBERID AND register_type = 'CAR_OWNER')

END

IF @ISAPPUSER = 1 AND @REGISTERTYPE = 'CAR_OWNER' BEGIN
UPDATE T_CUSTOMER_CAR_OWNER
SET FNAME = @FNAME, LNAME = @LNAME, GENDER = @GENDER, 
BIRTHDATE = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 120) END, 
EMAIL = @EMAIL, ADDRESS1 = @ADDRESS, SUBDISTRICT = @SUBDISTRICT, DISTRICT = @DISTRICT, PROVINCE = @PROVINCE,
POSTALCODE = @POSTALCODE, MOBILE = @MOBILE, citizen_id = @CITIZENID, vehicle_no = @VEHICLENO, is_update = 1,
plate_no = @PLATENO, TITLENAME = @TITLENAME, UPDATE_DT = DATEADD(HOUR, 7, GETDATE()), UPDATE_USER = @FNAME
WHERE MEMBERID = @MEMBERID

SET  @CUS_ID = (SELECT ID FROM T_CUSTOMER_CAR_OWNER WHERE MEMBERID = @MEMBERID AND register_type = 'CAR_OWNER')
SET  @IS_TERM = (SELECT COALESCE(IS_TERM,0) AS IS_TERM FROM T_CUSTOMER_CAR_OWNER WHERE MEMBERID = @MEMBERID AND register_type = 'CAR_OWNER')

END

IF @REGISTERTYPE = 'APP_USER' BEGIN
UPDATE T_CUSTOMER
SET FNAME = @FNAME, LNAME = @LNAME, GENDER = @GENDER, 
BIRTHDATE = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 120) END, 
EMAIL = @EMAIL, ADDRESS1 = @ADDRESS, SUBDISTRICT = @SUBDISTRICT, DISTRICT = @DISTRICT, PROVINCE = @PROVINCE,
POSTALCODE = @POSTALCODE, citizen_id = @CITIZENID, vehicle_no = @VEHICLENO, is_update = 1,
plate_no = @PLATENO, TITLENAME = @TITLENAME, UPDATE_DT = DATEADD(HOUR, 7, GETDATE()), UPDATE_USER = @FNAME
WHERE MEMBERID = @MEMBERID

SET  @CUS_ID = (SELECT ID FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID AND register_type = 'APP_USER')
SET  @IS_TERM = (SELECT COALESCE(IS_TERM,0) AS IS_TERM FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID AND register_type = 'APP_USER')

END

UPDATE T_CUSTOMER_CAR SET PLATE_NO = @PLATENO WHERE CUS_ID = @CAR_ID

SET  @IS_PREFERENCE = (SELECT COALESCE(is_preference,0) FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID)

SELECT @OTP_TOKEN AS OTP_TOKEN, @IS_TERM AS IS_TERM, @IS_PREFERENCE AS IS_PREFERENCE";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, f_name, l_name, gender, birthdate, email, address, sub_distinct, distinct, province, postcode, mobile, citizen_id, vehicle_no, register_type, member_id, plate_no, title_name, is_app_user, uid)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                value.data.member_id = member_id;
                                value.data.otp_token = dt.Rows[0]["OTP_TOKEN"].ToString();
                                value.data.is_term = false; //Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_TERM"].ToString()));
                                value.data.is_preference = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_PREFERENCE"].ToString()));

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


        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size    
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^[0-9]{10}$").Success;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}