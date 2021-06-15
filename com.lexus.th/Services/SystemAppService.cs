using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class SystemAppService
    {
        private string conn;
        public SystemAppService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceSystemAppModel> CheckSystemApp(string device_type, string app_version)
        {
            ServiceSystemAppModel value = new ServiceSystemAppModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(device_type, app_version);

                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    //value.msg.store_link
                    return value;
                }
                else
                {
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

        public async Task<ServiceSystemAppModel> CheckSystemAppNew(string device_type, string app_version, string lang)
        {
            ServiceSystemAppModel value = new ServiceSystemAppModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystemNewShould(device_type, app_version, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    //value.msg.store_link
                    return value;
                }
                else
                {
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

        //private ValidationModel CheckSystem(string device_type, string app_version)
        //{
        //    ValidationModel value = new ValidationModel();
        //    try
        //    {
        //        ValidationModel.InvalidState state;
        //        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //        {
        //            #region E903
        //            state = ValidationModel.InvalidState.E903;
        //            string cmd = @"SELECT description FROM SYSTEM_MAINTENANCE WHERE GETDATE() BETWEEN start_date AND end_date";
        //            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
        //            {
        //                if (dt.Rows.Count > 0)
        //                {
        //                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
        //                }
        //            }
        //            #endregion
        //            #region E904
        //            state = ValidationModel.InvalidState.E904;
        //            cmd = @"SELECT COUNT(1) AS CNT FROM SYSTEM_VERSION WHERE platform ='{0}' AND version > '{1}' AND is_force = 1";
        //            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
        //            {
        //                if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
        //                {
        //                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
        //                }
        //            }
        //            #endregion
        //            #region E905
        //            state = ValidationModel.InvalidState.E905;
        //            cmd = @"SELECT COUNT(1) AS CNT FROM SYSTEM_VERSION WHERE platform ='{0}' AND version > '{1}' AND is_force = 0";
        //            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
        //            {
        //                if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
        //                {
        //                    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
        //                }
        //            }
        //            #endregion
        //        }

        //        value.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return value;
        //}
    }
}