using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class RedeemNewService
    {
        private string conn;
        private static string[] warningTargetMobileNo = ConfigurationManager.AppSettings["WARNING_TARGET_MOBILE"].Split(';');

        public RedeemNewService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ServiceNewRedeemModel> RedeemItem(string token, int privilege_id, string v, string p)
        {
            ServiceNewRedeemModel value = new ServiceNewRedeemModel();
            int customer_id = 0;
            try
            {
                DateTime ts = DateTime.Now;

                SystemController syc = new SystemController();
                ValidationModel validation2 = await syc.CheckSystem(p, v);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                    return value;
                }
                else
                {
                    ValidationModel validation = CheckValidation(token, privilege_id, out customer_id);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    //if (!IsCheckPrivilegeCount(token))
                    //{
                    //    return value;
                    //}
                    //if (!IsCheckPrivilege(privilege_id))
                    //{
                    //    return value;
                    //}

                    value = Redeem(customer_id, privilege_id, ts);
                    value.ts = UtilityService.GetDateTimeFormat(ts);

                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async  Task<ServiceNewRedeemModel> RedeemItemNew(string token, int privilege_id, string v, string p, string lang)
        {
            ServiceNewRedeemModel value = new ServiceNewRedeemModel();
            int customer_id = 0;
            try
            {
                DateTime ts = DateTime.Now;

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
                    ValidationModel validation = CheckValidationNew(token, privilege_id, out customer_id, lang);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    //if (!IsCheckPrivilegeCount(token))
                    //{
                    //    return value;
                    //}
                    //if (!IsCheckPrivilege(privilege_id))
                    //{
                    //    return value;
                    //}

                    value = Redeem(customer_id, privilege_id, ts);
                    value.ts = UtilityService.GetDateTimeFormat(ts);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private ValidationModel CheckValidation(string token, int privilege_id, out int customerID)
        {
            ValidationModel value = new ValidationModel();
            customerID = 0;
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E501
                    state = ValidationModel.InvalidState.E501;
                    string cmd = @"SELECT ID, PRIVILEGE_CNT FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][1]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                        else
                        {
                            customerID = Convert.ToInt32(dt.Rows[0][0]);
                        }
                    }
                    #endregion

                    #region E502
                    state = ValidationModel.InvalidState.E502;
                    cmd = @"SELECT CASE WHEN ISNULL(EXPIRY, '1800-01-01') < GETDATE() THEN 1 ELSE 0 END AS IS_EXPIRE FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) == 1)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                            }
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

        private ValidationModel CheckValidationNew(string token, int privilege_id, out int customerID, string lang)
        {
            ValidationModel value = new ValidationModel();
            customerID = 0;
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E501
                    state = ValidationModel.InvalidState.E501;
                    string cmd = @"SELECT ID, PRIVILEGE_CNT FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][1]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage =  ValidationModel.GetInvalidMessageNew(state, lang).Result };
                        }
                        else
                        {
                            customerID = Convert.ToInt32(dt.Rows[0][0]);
                        }
                    }
                    #endregion

                    #region E502
                    state = ValidationModel.InvalidState.E502;
                    cmd = @"SELECT CASE WHEN ISNULL(EXPIRY, '1800-01-01') < GETDATE() THEN 1 ELSE 0 END AS IS_EXPIRE FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang).Result };
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[0][0]) == 1)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage =  ValidationModel.GetInvalidMessageNew(state, lang).Result };
                            }
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

        private ServiceNewRedeemModel Redeem(int customer_id, int privilege_id, DateTime ts)
        {
            ServiceNewRedeemModel value = new ServiceNewRedeemModel();
            try
            {
                string specialMsg = "";

                string shopName;
                int privId;
                string successMsg;
                int oQuota, oUsage, pQuota, pUsage,
                    pNo, pCount, pStart, pType, pQuotaPeriod, pLevel, pCumulative;
                int result = 1;
                DateTime startDate, endDate;

                bool isManualCode = false;
                bool isNotify = true;

                List<int> oQuotaWarning = new List<int>();
                List<int> pQuotaWarning = new List<int>();
                {
                    #region Query Privilege with common condition check
                    List<SqlParameter> paramList = new List<SqlParameter>();
                    DataTable dt = null;
                    string sql = "";
                    sql += "select a.ID,a.TITLE AS shop_name,b.overall_quota,b.overall_usage,b.period_quota,b.period_usage,b.period_running_round, " +
                            "b.period_count,b.period_start AS p_start,b.period_type,b.customer_usage_per_period,b.privilege_level,a.PERIOD_START,a.PERIOD_END, " +
                            "b.use_pre_define_code,b.sms_success_msg,b.quota_cumulative,b.has_notify_sms,b.special_reply_message, " +
                            "b.warning_qty,b.warning_qty2,b.warning_qty3,b.warning_qty4,b.period_warning_qty,b.period_warning_qty2,b.period_warning_qty3,b.period_warning_qty4 " +
                            "from T_PRIVILEDGES a join privilege_quota b on a.ID = b.privilege_id " +
                            "where a.ID = @privilege_id";

                    paramList.Add(new SqlParameter("@privilege_id", privilege_id));

                    try
                    {
                        dt = ExecuteParameterizedQuery(sql, paramList);

                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                
                            }
                            else
                            {
                                value.success = false;
                                value.msg = new MsgModel() { code = 506, text = "Privilege not found" };
                                LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.WRONG_CODE, privilege_id);
                                return value;
                            }
                        }
                        else
                        {
                            value.success = false;
                            value.msg = new MsgModel() { code = 506, text = "Privilege not found" };
                            LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.WRONG_CODE, privilege_id);
                            return value;
                        }
                    }
                    catch (Exception ex)
                    {
                        value.success = false;
                        value.msg = new MsgModel() { code = 506, text = "Privilege not found" };
                        LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.WRONG_CODE, privilege_id);
                        return value;
                    }
                    #endregion

                    #region save variable
                    privId = GetNullableInt(dt.Rows[0], "ID");
                    oQuota = GetNullableInt(dt.Rows[0], "overall_quota");
                    oUsage = GetNullableInt(dt.Rows[0], "overall_usage");
                    pQuota = GetNullableInt(dt.Rows[0], "period_quota");
                    pUsage = GetNullableInt(dt.Rows[0], "period_usage");
                    pNo = GetNullableInt(dt.Rows[0], "period_running_round");
                    pCount = GetNullableInt(dt.Rows[0], "period_count");
                    pStart = GetNullableInt(dt.Rows[0], "p_start");
                    pType = GetNullableInt(dt.Rows[0], "period_type");
                    pQuotaPeriod = GetNullableInt(dt.Rows[0], "customer_usage_per_period");
                    pLevel = GetNullableInt(dt.Rows[0], "privilege_level");
                    shopName = GetNullableString(dt.Rows[0], "shop_name");
                    startDate = GetNullableDateTime(dt.Rows[0], "PERIOD_START").Value;
                    endDate = GetNullableDateTime(dt.Rows[0], "PERIOD_END").Value;
                    isManualCode = GetNullableBoolean(dt.Rows[0], "use_pre_define_code");

                    successMsg = GetNullableString(dt.Rows[0], "sms_success_msg");
                    pCumulative = GetNullableInt(dt.Rows[0], "quota_cumulative");
                    isNotify = GetNullableBoolean(dt.Rows[0], "has_notify_sms");
                    specialMsg = GetNullableString(dt.Rows[0], "special_reply_message");

                    oQuotaWarning.Add(GetNullableInt(dt.Rows[0], "warning_qty"));
                    oQuotaWarning.Add(GetNullableInt(dt.Rows[0], "warning_qty2"));
                    oQuotaWarning.Add(GetNullableInt(dt.Rows[0], "warning_qty3"));
                    oQuotaWarning.Add(GetNullableInt(dt.Rows[0], "warning_qty4"));

                    pQuotaWarning.Add(GetNullableInt(dt.Rows[0], "period_warning_qty"));
                    pQuotaWarning.Add(GetNullableInt(dt.Rows[0], "period_warning_qty2"));
                    pQuotaWarning.Add(GetNullableInt(dt.Rows[0], "period_warning_qty3"));
                    pQuotaWarning.Add(GetNullableInt(dt.Rows[0], "period_warning_qty4"));
                    #endregion
                }

                #region Common condition check
                if (ts.CompareTo(endDate) > 0)    // Privilege has alread expired
                {
                    value.success = false;
                    value.msg = new MsgModel() { code = 505, text = "Privilege is expired" };
                    return value;
                }
                else if (ts.CompareTo(startDate) <= 0)    // Privilege has alread expired
                {
                    value.success = false;
                    value.msg = new MsgModel() { code = 506, text = "Privilege not found" };
                    LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.WRONG_CODE, privilege_id);
                    return value;
                }
                else if (oQuota != -1 && oUsage >= oQuota)
                {
                    //Item is out of stock
                    value.success = false;
                    value.msg = new MsgModel() { code = 503, text = "Item is out of stock" };
                    LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.EXCEED_QUOTA, privilege_id);
                    return value;
                }
                else if (((pType != 0 && pType != 5) && pUsage >= pQuota) &&
                        !NeedResetPeriodUsage(privId, (PrivilegeType)pType, ts, pStart, 1, pLevel))
                {
                    //result = ReturnMSGCountReach((PrivilegeType)pType);
                    value.success = false;
                    value.msg = new MsgModel() { code = 503, text = "Item is out of stock" };
                    LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.EXCEED_QUOTA, privilege_id);
                    return value;
                }
                else if ((pType == 10) && //&& pUsage2 >= pQuota2) &&
                    !NeedResetPeriodUsage(privId, (PrivilegeType)pType, ts, pStart, 2, pLevel))
                {
                    value.success = false;
                    value.msg = new MsgModel() { code = 503, text = "Item is out of stock" };
                    LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.EXCEED_QUOTA, privilege_id);
                    return value;
                }
                #endregion

                #region Check period usage
                if (pType != 0)
                {
                    int previousUsage;

                    {
                        previousUsage = CountPrivilegeRedemption(customer_id,
                                                                     privId,
                                                                     ts,
                                                                     (PrivilegeType)pType,                                                                    
                                                                     pStart);
                    }

                    {
                        if (previousUsage >= pQuotaPeriod)
                        {
                            //You've already redeemed this item
                            value.success = false;
                            value.msg = new MsgModel() { code = 504, text = "You've already redeemed this item" };
                            LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.PRIVILEGE_USED, privilege_id);
                            return value;
                        }
                    }
                }
                #endregion

                // Proceed to retrieve Discount Code
                #region Get redeem code
                if (isManualCode)
                {
                    #region Manual code
                    //List<get_manual_code_Result> ListCode = context.get_manual_code(privId).ToList();
                    DataTable dt = null;
                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@privilege_id", privId));
                    paramList.Add(new SqlParameter("@redeemed_date", ts));
                    string sqlCmd = " call get_manual_code (@privilege_id, @redeemed_date) ";
                    dt = ExecuteParameterizedQuery(sqlCmd, paramList);
                    if (dt.Rows.Count > 0)
                    {
                        string code = dt.Rows[0][1].ToString();

                        //successMsg = successMsg == "" ? ConfigurationManager.AppSettings["MESSAGE_ACCEPT_PRIVILEGE_WORD"].ToString() : successMsg;
                        //result = GenerateReplyMessage(successMsg, shopName, code, ts.ToString("dd'/'MM'/'yy"), true, specialMsg, privId);

                        if (pType != 0 && pType != 5)    // Period type
                        {
                            ResetPeriodUsageIfNeccesary(privId, pType, ts, pStart, pLevel);
                        }

                        string checkExceed = UpdateQuotaUsed(privId);
                        if (checkExceed == "EXCEED_QUOTA_OVERALL" || checkExceed == "EXCEED_QUOTA_PERIOD")
                        {
                            value.success = false;
                            value.msg = new MsgModel() { code = 503, text = "Item is out of stock" };
                            LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.EXCEED_QUOTA, privilege_id);
                            return value;
                        }

                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = successMsg };
                        LogResult(customer_id, code, isManualCode, privId.ToString(), value.msg.text, result, privId);

                        value.data = new _ServiceNewRedeemData();
                        value.data.id = privId;
                        value.data.redeem_code = code;
                    }
                    else
                    {
                        value.success = false;
                        value.msg = new MsgModel() { code = 503, text = "Item is out of stock" };
                        LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.EXCEED_QUOTA, privilege_id);
                        return value;
                    }
                    #endregion
                }
                else
                {
                    #region generate random code
                    int count = 0;
                    string redeemCode;
                    do
                    {
                        count++;
                        redeemCode = GenerateRandomNumberString();

                    } while (CheckCodeExistsInPeriod(redeemCode) && count < 12000);

                    if (count >= 12000)
                    {
                        value.success = false;
                        value.msg = new MsgModel() { code = 506, text = "Privilege not found" };
                        LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.WRONG_CODE, privilege_id);
                        return value;
                    }
                    else
                    {
                        isManualCode = false;
                        //successMsg = successMsg == "" ? ConfigurationManager.AppSettings["MESSAGE_ACCEPT_PRIVILEGE_WORD"].ToString() : successMsg;
                        //result = GenerateReplyMessage(successMsg, shopName, redeemCode, ts.ToString("dd'/'MM'/'yy"), true, specialMsg, privId);

                        if (pType != 0 && pType != 5)    // Period type
                        {
                            ResetPeriodUsageIfNeccesary(privId, pType, ts, pStart, pLevel);
                            
                        }

                        string checkExceed = UpdateQuotaUsed(privId);
                        if (checkExceed == "EXCEED_QUOTA_OVERALL" || checkExceed == "EXCEED_QUOTA_PERIOD")
                        {
                            value.success = false;
                            value.msg = new MsgModel() { code = 503, text = "Item is out of stock" };
                            LogResult(customer_id, "", isManualCode, privilege_id.ToString(), value.msg.text, (int)ResultCase.EXCEED_QUOTA, privilege_id);
                            return value;
                        }

                        

                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = successMsg };
                        LogResult(customer_id, redeemCode, isManualCode, privId.ToString(), value.msg.text, result, privId);

                        value.data = new _ServiceNewRedeemData();
                        value.data.id = privId;
                        value.data.redeem_code = redeemCode;

                        
                    }
                    #endregion
                }
                #endregion

                #region notifySMS
                //Warning period
                pQuotaWarning.Sort();
                foreach (int WarningPercent in pQuotaWarning.Distinct().ToList())
                {
                    CheckWarningQTY(pUsage, WarningPercent, shopName, privId.ToString(), pQuota, true);
                }

                //Warning Overall
                oQuotaWarning.Sort();
                foreach (int WarningPercent in oQuotaWarning.Distinct().ToList())
                {
                    CheckWarningQTY(oUsage, WarningPercent, shopName, privId.ToString(), oQuota, false);
                }
                #endregion

                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public static bool ExecuteParameterizedNonQuery(string sqlCommand, List<SqlParameter> parameters)
        {
            bool result = false;
            string connectionString = ConfigurationManager.AppSettings["LexusDBConn"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlCommand, connection);

                foreach (SqlParameter parameter in parameters)
                    command.Parameters.Add(parameter);

                try
                {
                    connection.Open();
                    result = command.ExecuteNonQuery() > 0;
                }
                catch //(Exception ex)
                {
                    result = false;
                }
            }

            return result;
        }

        public static DataTable ExecuteParameterizedQuery(string sqlCommand, List<SqlParameter> parameters)
        {
            DataTable result = null;
            string connectionString = ConfigurationManager.AppSettings["LexusDBConn"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sqlCommand;

                if (parameters != null)
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = new DataTable();
                        if (reader.HasRows)
                        {
                            result.Load(reader);
                        }
                    }
                }
                catch //(Exception ex)
                {
                    result = null;
                }
                finally
                {
                    connection.Dispose();
                    connection.Close();
                }
            }

            return result;
        }

        public static DateTime? GetNullableDateTime(DataRow dr, string fieldName)
        {
            try
            {
                if (!dr.IsNull(dr.Table.Columns[fieldName].Ordinal))
                    return Convert.ToDateTime(dr[fieldName]);
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetNullableString(DataRow dr, string fieldName)
        {
            try
            {
                if (!dr.IsNull(dr.Table.Columns[fieldName].Ordinal))
                    return dr[fieldName].ToString();
                else
                    return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static double GetNullableDouble(DataRow dr, string fieldName)
        {
            try
            {
                if (!dr.IsNull(dr.Table.Columns[fieldName].Ordinal))
                    return Convert.ToDouble(dr[fieldName]);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool GetNullableBoolean(DataRow dr, string fieldName)
        {
            try
            {
                if (!dr.IsNull(dr.Table.Columns[fieldName].Ordinal))
                    return dr[fieldName].ToString() == "True";
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int GetNullableInt(DataRow dr, string fieldName)
        {
            try
            {
                if (!dr.IsNull(dr.Table.Columns[fieldName].Ordinal))
                    return Convert.ToInt32(dr[fieldName].ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int WeekOfYear(DateTime dt, DayOfWeek dayOfWeek)
        {
            CalendarWeekRule weekRule = CalendarWeekRule.FirstFourDayWeek;
            DayOfWeek firstWeekDay = dayOfWeek;
            Calendar calendar = CultureInfo.GetCultureInfo("th-TH").Calendar;

            int currentWeek = calendar.GetWeekOfYear(dt, weekRule, firstWeekDay);
            return currentWeek;
        }

        public enum PrivilegeType
        {
            UNLIMIT = 0,
            DAILY = 1,
            WEEKLY = 2,
            MONTHLY = 3,
            YEARLY = 4,
            ONCE_PER_CAMPAIGN = 5,
            ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH = 6,
            MONTHLY_ACCUM = 7,
            ONCE_PER_CAMPAIGN_ACCUM = 8,
            DAILY_LIMIT_BY_MONTH = 9,
            //MONTHLY_LIMIT_DAILY_AND_MONTH = 10,
            ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY = 11
        }

        public enum ResultCase
        {
            SUCCESS = 1,
            NOT_REGISTER = -1,
            WRONG_CODE = -2,
            PRIVILEGE_EXPIRE = -3,
            MEMBER_EXPIRE = -4,
            EXCEED_QUOTA = -5,
            ERROR = -6,
            PRIVILEGE_USED = -7,
            WRONG_MEMBER_LEVEL = -8,
            REQUEST_NOT_ALLOW = -9,
            NON_TOYOTA_NOT_ALLOW = -10,
            UNKNOWN = 0
        }

        private static bool NeedResetPeriodUsage(int pId, PrivilegeType pType, DateTime ts, int pStart, int PeriodNo, int pLevel)
        {
            bool needReset = false;

            if (pType == PrivilegeType.DAILY || pType == PrivilegeType.ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY)// || (pType == PrivilegeType.MONTHLY_LIMIT_DAILY_AND_MONTH && PeriodNo == 1))
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select * from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 and DATEADD(dd, 0, DATEDIFF(dd, 0, sms_date)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @sms_date)) ";
                paramList.Add(new SqlParameter("@privilege_id", pId));
                paramList.Add(new SqlParameter("@sms_date", ts));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    needReset = dt.Rows.Count == 0;
                }
                catch (Exception ex)
                {

                }
            }
            else if (pType == PrivilegeType.WEEKLY)
            {
                int currentDayOfWeek = (int)ts.DayOfWeek;
                int dateDiff = currentDayOfWeek - pStart;
                dateDiff += dateDiff < 0 ? 7 : 0;

                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select * from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 and DATEDIFF(DAY, DATEADD(dd, 0, DATEDIFF(dd, 0, sms_date)), DATEADD(dd, 0, DATEDIFF(dd, 0, @sms_date))) <= @dateDiff ";
                paramList.Add(new SqlParameter("@privilege_id", pId));
                paramList.Add(new SqlParameter("@sms_date", ts));
                paramList.Add(new SqlParameter("@dateDiff", dateDiff));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    needReset = dt.Rows.Count == 0;
                }
                catch (Exception ex)
                {

                }
            }
            else if (pType == PrivilegeType.MONTHLY || pType == PrivilegeType.MONTHLY_ACCUM ||
                     pType == PrivilegeType.ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH || pType == PrivilegeType.ONCE_PER_CAMPAIGN_ACCUM ||
                     pType == PrivilegeType.DAILY_LIMIT_BY_MONTH)// || (pType == PrivilegeType.MONTHLY_LIMIT_DAILY_AND_MONTH && PeriodNo == 2))
            {
                DateTime fromDate, toDate;

                if (ts.Day < pStart)
                {
                    DateTime previousPeriod = ts.AddMonths(-1);
                    fromDate = new DateTime(previousPeriod.Year, previousPeriod.Month, pStart);
                    toDate = fromDate.AddMonths(1);//.AddDays(-1);
                }
                else
                {
                    DateTime nextPeriod = ts.AddMonths(1);

                    toDate = new DateTime(nextPeriod.Year, nextPeriod.Month, pStart);
                    //toDate = toDate.AddDays(-1);
                    fromDate = toDate.AddMonths(-1);//.AddDays(1);
                }
                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select * from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 and sms_date >= @fromDate and sms_date < @toDate";
                paramList.Add(new SqlParameter("@privilege_id", pId));
                paramList.Add(new SqlParameter("@fromDate", fromDate));
                paramList.Add(new SqlParameter("@toDate", toDate));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    needReset = dt.Rows.Count == 0;
                }
                catch (Exception ex)
                {

                }
            }
            else if (pType == PrivilegeType.YEARLY)
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select * from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 and year(sms_date) = @inputYear";
                paramList.Add(new SqlParameter("@privilege_id", pId));
                paramList.Add(new SqlParameter("@inputYear", ts.Year));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    needReset = dt.Rows.Count == 0;
                }
                catch (Exception ex)
                {

                }
            }


            return needReset;
        }

        private static int CountPrivilegeRedemption(int customerId, int pId, DateTime ts, PrivilegeType mode, int pStart)
        {
            int result = 0;


            if (mode == PrivilegeType.DAILY || mode == PrivilegeType.DAILY_LIMIT_BY_MONTH)
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select count(id) from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 and DATEADD(dd, 0, DATEDIFF(dd, 0, sms_date)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @sms_date)) " +
                    "and customer_id = @customer_id";
                paramList.Add(new SqlParameter("@customer_id", customerId));
                paramList.Add(new SqlParameter("@privilege_id", pId));
                paramList.Add(new SqlParameter("@sms_date", ts));
                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    if (dt.Rows.Count > 0)
                    {
                        result = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }

                }
                catch (Exception ex)
                {

                }
            }
            else if (mode == PrivilegeType.WEEKLY)
            {
                int currentWeek = WeekOfYear(ts, (DayOfWeek)pStart);
                int currentDayOfWeek = (int)ts.DayOfWeek;
                int dateDiff = currentDayOfWeek - pStart;
                dateDiff += dateDiff < 0 ? 7 : 0;

                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select count(id) from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 and DATEDIFF(DAY, DATEADD(dd, 0, DATEDIFF(dd, 0, sms_date)), DATEADD(dd, 0, DATEDIFF(dd, 0, @sms_date))) <= @dateDiff " +
                    "and customer_id = @customer_id";
                paramList.Add(new SqlParameter("@customer_id", customerId));
                paramList.Add(new SqlParameter("@privilege_id", pId));
                paramList.Add(new SqlParameter("@sms_date", ts));
                paramList.Add(new SqlParameter("@dateDiff", dateDiff));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    if (dt.Rows.Count > 0)
                    {
                        result = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                }
                catch (Exception ex)
                {

                }

            }
            else if (mode == PrivilegeType.MONTHLY || mode == PrivilegeType.MONTHLY_ACCUM)// || mode == PrivilegeType.MONTHLY_LIMIT_DAILY_AND_MONTH)
            {
                DateTime fromDate, toDate = new DateTime(); ;

                if (ts.Day < pStart)
                {
                    DateTime previousPeriod = ts.AddMonths(-1);
                    fromDate = new DateTime(previousPeriod.Year, previousPeriod.Month, pStart);
                    toDate = fromDate.AddMonths(1);//.AddDays(-1);
                }
                else
                {
                    DateTime nextPeriod = ts.AddMonths(1);
                    toDate = new DateTime(nextPeriod.Year, nextPeriod.Month, pStart);
                    //toDate = toDate.AddDays(-1);
                    fromDate = toDate.AddMonths(-1);//.AddDays(1);
                }

                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select count(id) from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 and sms_date >= @fromDate and sms_date < @toDate " +
                    "and customer_id = @customer_id";
                paramList.Add(new SqlParameter("@customer_id", customerId));
                paramList.Add(new SqlParameter("@privilege_id", pId));
                paramList.Add(new SqlParameter("@fromDate", fromDate));
                paramList.Add(new SqlParameter("@toDate", toDate));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    if (dt.Rows.Count > 0)
                    {
                        result = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else if (mode == PrivilegeType.YEARLY)
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select count(id) from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 and year(sms_date) = @inputYear " +
                    "and customer_id = @customer_id";
                paramList.Add(new SqlParameter("@customer_id", customerId));
                paramList.Add(new SqlParameter("@privilege_id", pId));
                paramList.Add(new SqlParameter("@inputYear", ts.Year));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    if (dt.Rows.Count > 0)
                    {
                        result = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else if (mode == PrivilegeType.ONCE_PER_CAMPAIGN || mode == PrivilegeType.ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH ||
                     mode == PrivilegeType.ONCE_PER_CAMPAIGN_ACCUM || mode == PrivilegeType.ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY)
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select count(id) from sms_log " +
                    "where privilege_id = @privilege_id and result = 1 " +
                    "and customer_id = @customer_id";
                paramList.Add(new SqlParameter("@customer_id", customerId));
                paramList.Add(new SqlParameter("@privilege_id", pId));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);

                    if (dt.Rows.Count > 0)
                    {
                        result = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return result;
        }

        private static string GenerateRandomNumberString()
        {
            Random rnd = new Random();
            string code = rnd.Next(10000).ToString("0000");
            char[] ary = code.ToCharArray();
            int i1 = int.Parse(ary[0].ToString());
            int i2 = int.Parse(ary[1].ToString());
            int i3 = int.Parse(ary[2].ToString());
            int i4 = int.Parse(ary[3].ToString());
            int sum = i1 + i2 + i3 + i4;

            int checkSum = 9 - sum % 10;

            return code + checkSum;
        }

        private static bool CheckCodeExistsInPeriod(string code)
        {
            DateTime dateNow = DateTime.Now;
            //if (ParseBoolean(ConfigurationManager.AppSettings["ENABLE_AZURE_TIMEZONE"].ToString()))
            dateNow = dateNow.AddHours(7);

            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt = null;
            string sql = "";
            sql += "select * from sms_log " +
                "where discount_code = @code and DATEADD(dd, 0, DATEDIFF(dd, 0, sms_date)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @dateNow)) ";
            paramList.Add(new SqlParameter("@code", code));
            paramList.Add(new SqlParameter("@dateNow", dateNow));

            try
            {
                dt = ExecuteParameterizedQuery(sql, paramList);

                return dt.Rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool ParseBoolean(string param)
        {
            bool result = false;

            if (!Boolean.TryParse(param, out result))
            {
                result = false;
            }

            return result;
        }

        public static string GenerateReplyMessage(string rawMessage, string shopName, string code, string date, bool needReplace, string specialMsg, int priv_id)
        {
            if (specialMsg != null && specialMsg != "")
            {
                string result = specialMsg.Replace("{1}", shopName).Replace("{2}", code).Replace("{3}", date);
                return result;
            }

            if (needReplace)
            {
                string prShop = ConfigurationManager.AppSettings["MESSAGE_REPLACE_SHOP_PREFIX"].ToString();
                string poShop = ConfigurationManager.AppSettings["MESSAGE_REPLACE_SHOP_POSTFIX"].ToString();
                string patternShop = ConfigurationManager.AppSettings["MESSAGE_REPLACE_SHOP_PATTERN"].ToString();
                string wordShop = patternShop.Replace("{0}", prShop).Replace("{1}", shopName).Replace("{2}", poShop);
                string wordCode = "";

                string prDate = ConfigurationManager.AppSettings["MESSAGE_REPLACE_DATE_PREFIX"].ToString();
                string poDate = ConfigurationManager.AppSettings["MESSAGE_REPLACE_DATE_POSTFIX"].ToString();
                string patternDate = ConfigurationManager.AppSettings["MESSAGE_REPLACE_DATE_PATTERN"].ToString();
                string wordDate = patternDate.Replace("{0}", prDate).Replace("{1}", date).Replace("{2}", poDate);

                string patternWord = ConfigurationManager.AppSettings["MESSAGE_ACCEPT_PRIVILEGE_PATTERN"].ToString();
                patternWord = patternWord.Replace("{0}", rawMessage).Replace("{1}", wordShop).Replace("{2}", wordCode).Replace("{3}", "");

                return patternWord;
            }
            else
            {
                return rawMessage;
            }
        }

        private static bool ResetPeriodUsageIfNeccesary(int pId, int pType, DateTime ts, int pStart, int pLevel)
        {
            PrivilegeType type = (PrivilegeType)pType;
            
            if (NeedResetPeriodUsage(pId, type, ts, pStart, 1, pLevel))
            {
                bool needAccu = type == PrivilegeType.ONCE_PER_CAMPAIGN_ACCUM || type == PrivilegeType.MONTHLY_ACCUM;

                List<SqlParameter> paramList = new List<SqlParameter>();
                DataTable dt = null;
                string sql = "";
                sql += "select * from privilege_quota " +
                    "where privilege_id = @privilege_id ";
                paramList.Add(new SqlParameter("@privilege_id", pId));

                try
                {
                    dt = ExecuteParameterizedQuery(sql, paramList);
                    int period_quota = 0;

                    if (dt.Rows.Count > 0)
                    {
                        if (needAccu && GetNullableInt(dt.Rows[0], "period_running_round") != 0)
                        {
                            int accu = GetNullableInt(dt.Rows[0], "period_quota") - GetNullableInt(dt.Rows[0], "period_usage");
                            period_quota = GetNullableInt(dt.Rows[0], "quota_cumulative") + accu;
                            //InsertLog(page, Constant.CATEGORY_SMS, "Accumulate quota " + accu + " to " + period_quota, Constant.PROJECT_TMT_PRIVILEGE);
                        }
                        else
                        {
                            period_quota = GetNullableInt(dt.Rows[0], "period_quota");
                        }

                        List<SqlParameter> paramList2 = new List<SqlParameter>();
                        string sql2 = "";
                        sql2 += "update privilege_quota set period_usage = 0, period_running_round = period_running_round + 1, period_quota = @period_quota where privilege_id = @privilege_id ";
                        paramList2.Add(new SqlParameter("@privilege_id", pId));
                        paramList2.Add(new SqlParameter("@period_quota", period_quota));

                        return ExecuteParameterizedNonQuery(sql2, paramList2);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            LogResult(1, "Test", false, "", "2", (int)ResultCase.EXCEED_QUOTA, 1);
        }

        private static string UpdateQuotaUsed(int pId)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt = null;
            string sql = "";
            sql += "select * from privilege_quota " +
                "where privilege_id = @privilege_id ";
            paramList.Add(new SqlParameter("@privilege_id", pId));

            try
            {
                dt = ExecuteParameterizedQuery(sql, paramList);

                if (dt.Rows.Count > 0)
                {
                    if (GetNullableInt(dt.Rows[0], "overall_usage") >= GetNullableInt(dt.Rows[0], "overall_quota"))
                    {
                        return "EXCEED_QUOTA_OVERALL";
                    }
                    else if (GetNullableInt(dt.Rows[0], "period_usage") >= GetNullableInt(dt.Rows[0], "period_quota"))
                    {
                        return "EXCEED_QUOTA_PERIOD";
                    }
                    else
                    {
                        List<SqlParameter> paramList2 = new List<SqlParameter>();
                        string sql2 = "";
                        sql2 += "update privilege_quota set overall_usage = overall_usage + 1, period_usage = period_usage + 1 " +
                            "where privilege_id = @privilege_id ";
                        paramList2.Add(new SqlParameter("@privilege_id", pId));

                        ExecuteParameterizedNonQuery(sql2, paramList2);

                        return "";
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public static void CheckWarningQTY(int Usage, int WarningPercent, string shopName, string privID, int Quota, bool isPeriod)
        {
            if (WarningPercent > 0)
            {
                double WarningQty = 0;
                WarningQty = (Quota * WarningPercent) / 100;
                WarningQty = Math.Ceiling(WarningQty);
                if ((Usage + 1) == (int)WarningQty)
                {
                    string message = ConfigurationManager.AppSettings["MESSAGE_WARNING_QUOTA"].ToString();
                    if (isPeriod)
                        message = string.Format(message, shopName, privID, "รอบนี้ใช้แล้ว", (Usage + 1).ToString(), Quota.ToString());
                    else
                        message = string.Format(message, shopName, privID, "รวมทั้งสิ้นใช้แล้ว", (Usage + 1).ToString(), Quota.ToString());

                    foreach (string mobile in warningTargetMobileNo)
                        sendSMS(message, mobile);

                    //Utility.UpdateNotifyWarningSMS(privId);
                    //Utility.InsertLog(page, Constant.CATEGORY_SMS, "Send SMS Warning|" + message, Constant.PROJECT_TMT_PRIVILEGE);
                }
            }
        }

        public static void sendSMS(string message, string mobileNo)
        {
            string url = "http://smartcomm2.net/sc2lc/SendMessage";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string postData = "ACCOUNT=" + ConfigurationManager.AppSettings["ACCOUNT_SMS"].ToString() +
                "&PASSWORD=" + ConfigurationManager.AppSettings["PASSWORD_SMS"].ToString();
            postData += "&MOBILE=" + mobileNo + "&MESSAGE=" + message;

            byte[] sourceByte = Encoding.UTF8.GetBytes(postData);
            byte[] bytes = Encoding.Convert(
                Encoding.GetEncoding("UTF-8"),
                Encoding.GetEncoding("TIS-620"),
                sourceByte);

            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            var result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();
        }

        public static int LogResult(int customerId, string discountCode, bool isManualCode, string privilegeCode, string reply, int result, int shopId)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            DataTable dt = null;
            string sql = "";
            sql += "insert into sms_log " +
                    "(customer_id, reply_msg, result, " +
                    "privilege_code, discount_code, is_manual_code, sms_date, " +
                    "privilege_id) " +
                    "VALUES(@customer_id, @reply_msg, @result, " +
                    "@privilege_code, @discount_code, @is_manual_code, DATEADD(HOUR, 7, GETDATE()), " +
                    "@shop_id); " +
                    "SELECT COUNT(1) FROM sms_log WHERE customer_id = @customer_id AND discount_code = @discount_code; ";

            paramList.Add(new SqlParameter("@customer_id", customerId));
            paramList.Add(new SqlParameter("@discount_code", discountCode));
            paramList.Add(new SqlParameter("@is_manual_code", isManualCode));
            paramList.Add(new SqlParameter("@privilege_code", privilegeCode));
            paramList.Add(new SqlParameter("@reply_msg", reply));
            paramList.Add(new SqlParameter("@result", result));
            paramList.Add(new SqlParameter("@shop_id", shopId));
            //paramList.Add(new SqlParameter("@sms_date", inputDate));
            //paramList.Add(new SqlParameter("@sms_msg", input.msg));
            //paramList.Add(new SqlParameter("@sms_src", src));
            //paramList.Add(new SqlParameter("@sms_src_detail", src_detail));

            try
            {
                dt = ExecuteParameterizedQuery(sql, paramList);
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}