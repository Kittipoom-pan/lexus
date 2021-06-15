using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    
    public class ValidationToken
    {
        private string conn;
        public ValidationToken()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public ValidationModel CheckValidation(string otp_token)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {

                    #region E902
                    state = ValidationModel.InvalidState.E902;
                    string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}' AND OTP_EXPIREY > DATEADD(HOUR, 7, GETDATE())";
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

        public async Task<ValidationModel> CheckValidationNew(string otp_token, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {

                    #region E902
                    state = ValidationModel.InvalidState.E902;
                    string cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}' AND OTP_EXPIREY > DATEADD(HOUR, 7, GETDATE())";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, otp_token)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
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