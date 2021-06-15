using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class InitialService
    {
        private string conn;
        public InitialService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ServiceInitialModel> CheckDataInitial(string v, string p, string lang, string vin, string citizen_id)
        {
            ServiceInitialModel value = new ServiceInitialModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                    return value;
                }
                else
                {
                    ValidationModel validation =await CheckValidation(lang,  vin, citizen_id);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    value.data = new _ServiceInitialData();
                    value.data.initial =await GetInitialData(vin);

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

        private async Task<ValidationModel> CheckValidation(string lang, string vin, string citizen_id)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Empty;
                    //#region E810
                    //state = ValidationModel.InvalidState.E810;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}'";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, mobile)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion

                    #region E816
                    state = ValidationModel.InvalidState.E816;
                    cmd = @"SELECT COUNT(1) AS CNT FROM initial_data WHERE vin = N'{0}' AND citizen_id = N'{1}' ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin, citizen_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    #region E811
                    state = ValidationModel.InvalidState.E811;
                    cmd = @"SELECT COUNT(1) AS CNT FROM initial_data WHERE vin = N'{0}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    #region E812
                    state = ValidationModel.InvalidState.E812;
                    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE vehicle_no = N'{0}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    #region E813
                    state = ValidationModel.InvalidState.E813;
                    cmd = @"SELECT COUNT(1) AS CNT FROM initial_data WHERE citizen_id = N'{0}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, citizen_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion


                    #region E805
                    state = ValidationModel.InvalidState.E805;
                    cmd = @"SELECT  COUNT(1) AS CNT FROM t_customer_car WHERE del_flag is null AND vin = N'{0}' ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    {
                        bool checkUsed = dt.Rows.Count == 1 && Convert.ToInt32(dt.Rows[0][0]) > 0;
                        if (checkUsed)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    //#region E814
                    //state = ValidationModel.InvalidState.E814; 
                    //cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE citizen_id = N'{0}'";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, citizen_id)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
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

        private async Task<InitialModel> GetInitialData(string vin)
        {
            InitialModel value = new InitialModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
SELECT id, firstname, lastname, gender, birthday, citizen_id,
    email, dealer, vin, plate_no, model, color, rs_date
FROM initial_data 
WHERE vin = N'{0}'", vin);

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            value.id = Convert.ToInt32(row["id"]);
                            value.firstname = row["firstname"].ToString();
                            value.lastname = row["lastname"].ToString();
                            value.gender = row["gender"].ToString();
                            value.birthday = row["birthday"].ToString();
                            value.citizen_id = row["citizen_id"].ToString();
                            value.email = row["email"].ToString();
                            value.dealer = row["dealer"].ToString();
                            value.vin = row["vin"].ToString();
                            value.plate_no = row["plate_no"].ToString();
                            value.model = row["model"].ToString();
                            value.color = row["color"].ToString();
                            value.rs_date = row["rs_date"].ToString();
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

        public async Task<ServiceInitialModel> GetInitial(string v, string p, string lang, string vin)
        {
            ServiceInitialModel value = new ServiceInitialModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                    return value;
                }
                else
                {
                    ValidationModel validation =await CheckValidation(lang, vin, "");
                    //if (!validation.Success)
                    //{
                    //    value.success = validation.Success;
                    //    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    //    return value;
                    //}

                    value.data = new _ServiceInitialData();
                    value.data.initial =await GetInitialData(vin);

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
    }
}