using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class SystemController
    {
        private string conn;
        public SystemController()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ValidationModel> CheckSystem(string device_type, string app_version)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E903
                    state = ValidationModel.InvalidState.E903;
                    string cmd = @"SELECT description FROM SYSTEM_MAINTENANCE WHERE GETDATE() BETWEEN start_date AND end_date";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                    }
                    #endregion

                    #region E904
                    state = ValidationModel.InvalidState.E904;
                    cmd = @"SELECT COUNT(1) AS CNT, MAX(store_link) AS store_link, MAX(version) AS last_version FROM SYSTEM_VERSION WHERE platform ='{0}' AND version > '{1}' AND is_force = 1";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state), InvalidStoreLink = dt.Rows[0]["store_link"].ToString(), InvalidVersion = dt.Rows[0]["last_version"].ToString() };
                        }
                    }
                    #endregion

                    //#region E905
                    //state = ValidationModel.InvalidState.E905;
                    //cmd = @"SELECT COUNT(1) AS CNT, MAX(store_link) AS store_link, MAX(version) AS last_version FROM SYSTEM_VERSION WHERE platform = N'{0}'AND version > '{1}' AND is_force = 0";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                    //    {
                    //        return new ValidationModel { Success = true, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state), InvalidStoreLink = dt.Rows[0]["store_link"].ToString(), InvalidVersion = dt.Rows[0]["last_version"].ToString() };
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

        public async Task<ValidationModel> CheckSystemNew(string device_type, string app_version, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E903
                    state = ValidationModel.InvalidState.E903;
                    string cmd = @"SELECT description FROM SYSTEM_MAINTENANCE WHERE GETDATE() BETWEEN start_date AND end_date";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    #region E904
                    state = ValidationModel.InvalidState.E904;
                    cmd = @"SELECT COUNT(1) AS CNT, MAX(store_link) AS store_link, MAX(version) AS last_version 
FROM SYSTEM_VERSION WHERE platform ='{0}' AND version > '{1}' AND is_force = 1";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNewReplace(state, lang, device_type.ToLower()), InvalidStoreLink = dt.Rows[0]["store_link"].ToString(), InvalidVersion = dt.Rows[0]["last_version"].ToString() };
                        }
                    }
                    #endregion

//                    #region E905
//                    state = ValidationModel.InvalidState.E905;
//                    cmd = @"SELECT COUNT(1) AS CNT, MAX(store_link) AS store_link, MAX(version) AS last_version 
//FROM SYSTEM_VERSION WHERE platform = N'{0}'AND version > '{1}' AND is_force = 0";
//                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
//                    {
//                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
//                        {
//                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang), InvalidStoreLink = dt.Rows[0]["store_link"].ToString(), InvalidVersion = dt.Rows[0]["last_version"].ToString() };
//                        }
//                    }
//                    #endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ValidationModel> CheckSystemNewShould(string device_type, string app_version, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {

                    #region E905
                    state = ValidationModel.InvalidState.E905;
                    string cmd = @"SELECT COUNT(1) AS CNT, MAX(store_link) AS store_link, MAX(version) AS last_version 
FROM SYSTEM_VERSION WHERE platform = N'{0}'AND version > '{1}' AND is_force = 0";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang), InvalidStoreLink = dt.Rows[0]["store_link"].ToString(), InvalidVersion = dt.Rows[0]["last_version"].ToString() };
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