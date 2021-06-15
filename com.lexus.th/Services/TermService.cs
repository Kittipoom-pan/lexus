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
    public class TermService
    {
        private string conn;

        public TermService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceAllBannerModel> UpdateReadTermCondition(string token, string v, string p, string lang)
        {
            ServiceAllBannerModel value = new ServiceAllBannerModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    ValidationModel validation2 = CheckValidationNew(token, lang);
                    //if (!validation2.Success)
                    //{
                    //    value.success = validation2.Success;
                    //    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                    //    return value;
                    //}

                    bool success = UpdateReadTerm(token);
                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private ValidationModel CheckValidationNew(string token, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E704
                    //state = ValidationModel.InvalidState.E704;
                    //string cmd = @"SELECT CASE WHEN ISNULL(EXPIRY, '1800-01-01') < GETDATE() THEN 1 ELSE 0 END AS IS_EXPIRE FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
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

        private bool UpdateReadTerm(string token)
        {
            bool flag = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
UPDATE T_CUSTOMER SET IS_TERM = 1
WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')

SELECT IS_TERM
FROM T_CUSTOMER
WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            flag = Convert.ToBoolean(int.Parse(dt.Rows[0]["IS_TERM"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }


        public async Task<ServiceAllBannerModel> UpdateReadTerms(string token, string v, string p, string lang, string device_id, string read_from)
        {
            ServiceAllBannerModel value = new ServiceAllBannerModel();
            value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);
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
                    MasterDataService service = new MasterDataService();
                    //string sys_terms_version = service.CheckLastestVersionOfTermsAndCondition();
                    string sys_terms_version = WebConfigurationManager.AppSettings["version"].ToString();
                    int accept_sys_terms_version = !string.IsNullOrEmpty(sys_terms_version) ? Convert.ToInt32(sys_terms_version) : 0;

                    value = await UpdateReadTermsAndConditionPolicy(token, device_id, accept_sys_terms_version, read_from);
                    value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);
                    value.msg = new MsgModel() { code = 200, text = "Success" };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "UpdateReadTerms");
                throw ex;
            }

            return value;
        }

        public async Task<ServiceAllBannerModel> UpdateReadTermsAndConditionPolicy(string token, string device_id, int sys_terms_version, string read_from)
        {
            ServiceAllBannerModel value = new ServiceAllBannerModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"   
        DECLARE @member_id NVARCHAR(50);
        DECLARE @device_id NVARCHAR(200) = N'{1}';
        DECLARE @sys_version int = N'{2}';
		DECLARE @read_from NVARCHAR(20) = N'{3}';
        

        SET @member_id = (SELECT top 1 MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')
	   
			BEGIN
				INSERT INTO [dbo].[terms_and_condition_stamp_read] (member_id, device_id, confirm_popup, confirm_popup_date, version, read_from) 
				OUTPUT INSERTED.id 
				VALUES (@member_id, @device_id, 1, dateadd(hour, 7,getdate()), @sys_version, @read_from)
			END
";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, device_id, sys_terms_version, read_from)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            value.success = dt.Rows[0]["id"].ToString() == "" ? false : true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "UpdateReadTermsAndConditionPolicy");
            }
            return value;
        }

        public void InsertTermsStamp(string member_id, string device_id, DateTime? confirm_checkbox_date, DateTime? confirm_popup_date)
        {

            //bool is_read_terms = CheckReadTerms(member_id, sys_version);
            int sys_version = Convert.ToInt32(WebConfigurationManager.AppSettings["version"].ToString());
            string memberId = !string.IsNullOrEmpty(member_id) ? member_id : "";
            string deviceId = !string.IsNullOrEmpty(device_id) ? device_id : "";


        
            try {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
            DECLARE @member_id NVARCHAR(50) = N'{0}';
            DECLARE @device_id NVARCHAR(200) = N'{1}'; 
            DECLARE @sys_version int = N'{2}';
            DECLARE @confirm_checkbox_date datetime = N'{3}';
            DECLARE @confirm_popup_date datetime = N'{4}';     

            DECLARE @confirm_checkbox bit;      
            DECLARE @confirm_popup bit; 

         

             IF @confirm_checkbox_date IS NULL OR @confirm_checkbox_date = ''
                BEGIN
                    SET @confirm_checkbox = 0;
                    SET @confirm_checkbox_date = null;
                END
            ELSE
                BEGIN
                    SET @confirm_checkbox = 1;
                END
        
            
            IF @confirm_popup_date IS NULL OR @confirm_popup_date = ''
                BEGIN
                    SET @confirm_popup = 0;
                    SET @confirm_popup_date = null;
                END
            ELSE
                BEGIN
                    SET @confirm_popup = 1;
                END

                
            BEGIN
            	INSERT INTO [dbo].[terms_and_condition_stamp_read] 
                       (member_id, device_id, confirm_checkbox, confirm_checkbox_date, [version], confirm_popup, confirm_popup_date, read_from) 
                   OUTPUT Inserted.id
            	VALUES
                       (@member_id, @device_id, @confirm_checkbox, @confirm_checkbox_date, @sys_version, @confirm_popup, @confirm_popup_date, 'register')
            END
            ";
            
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, memberId, deviceId, sys_version, confirm_checkbox_date, confirm_popup_date)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            int id = 0;
                            id = int.Parse(dt.Rows[0]["id"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "InsertCheckBoxDate_ForTermsPolicy");
                throw ex;
            }
        }


        public bool CheckReadTerms(string member_id, int sys_version)
        {
            bool is_read_terms = false;

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"   
                DECLARE @member_id NVARCHAR(50) = N'{0}';
                DECLARE @sys_version int = N'{1}';

                SELECT TOP 1 COUNT(*) AS count FROM terms_and_condition_stamp_read WHERE member_id = @member_id AND version = @sys_version";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, sys_version)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            is_read_terms = Convert.ToInt32(dt.Rows[0]["count"]) > 0 ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return is_read_terms;
        }


    }
}