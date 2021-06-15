using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class PrivilegeService
    {
        private string conn;

        public PrivilegeService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServicePrivilegeModel> GetScreenData(string v, string p, string token, string privilege_id)
        {
            ServicePrivilegeModel value = new ServicePrivilegeModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServicePrivilegeData();
                    value.data.privileges =await GetPrivilege(token, privilege_id, "EN");

                    ValidationModel.InvalidState state;
                    if (value.data.privileges.id != 0)
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        state = ValidationModel.InvalidState.E506;
                        value.success = false;
                        value.msg = new MsgModel() { code = ValidationModel.GetInvalidCode(state), text = ValidationModel.GetInvalidMessage(state), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServicePrivilegeModel> GetScreenDataNew(string v, string p, string token, string privilege_id, string lang)
        {
            ServicePrivilegeModel value = new ServicePrivilegeModel();
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
                    value.data = new _ServicePrivilegeData();
                    value.data.privileges =await GetPrivilege(token, privilege_id, lang);

                    ValidationModel.InvalidState state;
                    if (value.data.privileges.id != 0)
                    {
                        if (value.data.privileges.redeem_status == "BLOCK")
                        {
                            value.data.privileges.redeem_status = "REDEEM";
                            value.data.privileges.message_code = 0;
                        }
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        state = ValidationModel.InvalidState.E506;
                        value.success = false;
                        value.msg = new MsgModel() { code = ValidationModel.GetInvalidCode(state), text = await ValidationModel.GetInvalidMessageNew(state, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<PrivilegeModel> GetPrivilege(string token, string privilege_id, string lang)
        {
            PrivilegeModel privilege = new PrivilegeModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{1}'

SELECT 
ID,
CASE  
  WHEN @LANG = 'EN' THEN TITLE 
  ELSE TITLE END AS TITLE,
CASE  
  WHEN @LANG = 'EN' THEN [DESC] 
  ELSE desc_th END AS [DESC],
CASE  
  WHEN @LANG = 'EN' THEN RED_CONDITION 
  ELSE red_condition_th END AS RED_CONDITION,
CASE  
  WHEN @LANG = 'EN' THEN RED_LOCATION 
  ELSE red_location_th END AS RED_LOCATION,
CASE  
  WHEN @LANG = 'EN' THEN thk_message 
  ELSE thk_message_th END AS thk_message,
DATEADD(HOUR, 7, GETDATE()) AS PERIOD,
PERIOD_START,
PERIOD_END,
IMAGE,
IMAGE_1,
RED_PERIOD,
RED_EXPIRY,
PRIVILEDGE_TYPE,
IMAGE_GALLERY_COUNT,
IMAGE_GALLERY_NO,
display_type,
CASE WHEN (SELECT COUNT(1) AS CNT FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = P.ID AND STATUS = 'Y' AND DEL_FLAG IS NULL) = 0 THEN 1 ELSE 0 END AS IS_OUT_OF_STOCK,
(SELECT COUNT(1) AS CNT FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = P.ID AND STATUS = 'Y' AND DEL_FLAG IS NULL) AS all_stock,
is_show_stock,
period_type,
period_start_in_week,
period_start_in_month,
customer_usage_per_period,
overall_quota,
overall_usage,
period_quota,
period_usage,
is_show_your_remaining,
customer_usage_with_car_total,
COALESCE((SELECT top 1 PRIVILEGE_CNT FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{2}')), 0) PRIVILEGE_CNT,
CASE WHEN CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) < CONVERT(NVARCHAR(20), PERIOD_START, 120) THEN 'COMING_SOON' 
     WHEN CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) > CONVERT(NVARCHAR(20), PERIOD_END, 120) THEN 'PRIVILEGE_EXPIRED' 
     WHEN CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) < CONVERT(NVARCHAR(20), fully_redeem_to, 120) THEN 
    iif((select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
    where PRIVILEGE_ID = p.id
    and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))
    and REDEEM_CODE is null
    and MEMBERID in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = '{2}')) > 0, 'REDEEM', 'FULLY_REDEEMED')
ELSE 'REDEEM' END AS status
FROM T_PRIVILEDGES AS P
WHERE DEL_FLAG IS NULL AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), DISPLAY_START, 120) AND CONVERT(NVARCHAR(20), DISPLAY_END, 120)
AND P.ID = {0}
ORDER BY P.[order]";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id, lang, token)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            privilege = new PrivilegeModel();
                            privilege.id = Convert.ToInt32(row["ID"]);
                            privilege.title = row["TITLE"].ToString();
                            privilege.desc = row["DESC"].ToString();
                            privilege.period = row["RED_PERIOD"].ToString();
                            privilege.period_m = (row["PERIOD"] == DBNull.Value) ? "" : Convert.ToDateTime(row["PERIOD"]).ToString("MMMM").ToUpper();
                            privilege.period_start = (row["PERIOD_START"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["PERIOD_START"]));
                            privilege.period_end = (row["PERIOD_END"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["PERIOD_END"]));
                            privilege.is_out_of_stock = Convert.ToBoolean(Convert.ToInt32(row["IS_OUT_OF_STOCK"].ToString()));
                            privilege.all_stock = int.Parse(row["all_stock"].ToString());
                            privilege.is_show_stock = Convert.ToBoolean(Convert.ToInt32(row["is_show_stock"].ToString()));

                            int image_gallery_count = int.Parse(row["IMAGE_GALLERY_COUNT"].ToString());
                            int image_gallery_no = int.Parse(row["IMAGE_GALLERY_NO"].ToString());
                            privilege.images = new List<string>();

                            privilege.image = row["IMAGE"].ToString();
                            privilege.image_1 = row["IMAGE_1"].ToString();
                            privilege.privilege_type = int.Parse(row["PRIVILEDGE_TYPE"].ToString());
                            privilege.display_type = int.Parse(row["display_type"].ToString());
                            privilege.thk_message = "";// row["thk_message"].ToString();

                            privilege.redeem = new RedeemModel()
                            {
                                condition = row["RED_CONDITION"].ToString(),
                                period = row["RED_PERIOD"].ToString(),
                                location = row["RED_LOCATION"].ToString()
                                //expiry = row["RED_EXPIRY"].ToString()
                            };

                            //get special quota
                            //if special quota = -1 block quota in privilege
                            int special_quota = 0;
                            special_quota =await GetSpecialQuota(token, privilege_id);

                            privilege.period_type = int.Parse(row["period_type"].ToString());
                            privilege.period_start_in_week = int.Parse(row["period_start_in_week"].ToString());
                            privilege.period_start_in_month = int.Parse(row["period_start_in_month"].ToString());
                            privilege.customer_usage_per_period = int.Parse(row["customer_usage_per_period"].ToString());
                            privilege.redeem_status = row["status"].ToString();

                            //privilege.overall_quota = int.Parse(row["overall_quota"].ToString());
                            //privilege.overall_usage = int.Parse(row["overall_quota"].ToString()) - privilege.all_stock;//int.Parse(row["overall_usage"].ToString());
                            if ((PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.UNLIMIT ||
                                (PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.ONCE_PER_CAMPAIGN)
                                privilege.period_quota = int.Parse(row["all_stock"].ToString());
                            else
                                privilege.period_quota = int.Parse(row["period_quota"].ToString());
                            PrivilegePeriodModel pPeriod = new PrivilegePeriodModel(privilege.period_type, 
                                privilege.period_start_in_week, privilege.period_start_in_month, 
                                Convert.ToDateTime(privilege.period_start), Convert.ToDateTime(privilege.period_end));
                            //get all_stock in period DAILY, WEEKLY, MONTHLY, ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH, DAILY_LIMIT_BY_MONTH, ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY
                            //if ((PrivilegePeriodModel.PeriodType)privilege.period_type != PrivilegePeriodModel.PeriodType.UNLIMIT &&
                            //    (PrivilegePeriodModel.PeriodType)privilege.period_type != PrivilegePeriodModel.PeriodType.ONCE_PER_CAMPAIGN)
                            int period_quota = 0;
                            {
                                int period_usage = 0;
                                int reserve_usage = 0;
                                //qry period_usage with pPeriod
                                string cmd2 = @"
select * from (
select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
where PRIVILEGE_ID = {0}
and ((REDEEM_CODE is not null and REDEEM_DT BETWEEN '{1}' and '{2}') 
  or (REDEEM_CODE is null and MEMBERID not in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = N'{3}') 
        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))))
) a,
(
select COUNT(1) AS CNT2  from T_CUSTOMER_REDEEM
where PRIVILEGE_ID = {0}
and REDEEM_CODE is null and MEMBERID not in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = N'{3}')
and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))) b ";
                                using (DataTable dt2 = db.GetDataTableFromCommandText(string.Format(cmd2, privilege_id, pPeriod.period_start.ToString("yyyy-MM-dd HH:mm:ss"), pPeriod.period_end.ToString("yyyy-MM-dd HH:mm:ss"), token)))
                                {
                                    period_usage = int.Parse(dt2.Rows[0][0].ToString());
                                    reserve_usage = int.Parse(dt2.Rows[0][1].ToString());
                                }

                                //if (privilege.all_stock >= privilege.period_quota)
                                //    privilege.all_stock = privilege.period_quota;
                                //else
                                    period_quota = (privilege.all_stock < privilege.period_quota - period_usage ? privilege.all_stock : privilege.period_quota - period_usage) - reserve_usage;
                            }

                            if (period_quota == 0)
                            {
                                if ((PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.DAILY &&
                                    (PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY)
                                {
                                    privilege.ribbon_message = "Fully Redeemed Daily";
                                }
                                else if ((PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.WEEKLY)
                                {
                                    privilege.ribbon_message = "Fully Redeemed Weekly";
                                }
                                else if ((PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.MONTHLY &&
                                    (PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH &&
                                    (PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.DAILY_LIMIT_BY_MONTH)
                                {
                                    privilege.ribbon_message = "Fully Redeemed Monthly";
                                }
                                else
                                {
                                    privilege.ribbon_message = "Fully Redeemed";
                                }
                            }

                            {

                                if (special_quota == -1)
                                {
                                    //privilege.customer_remain_in_period = 0;
                                    //privilege.customer_usage_in_period = 0;
                                    privilege.customer_special_quota = -1;
                                    privilege.redeem_status = "BLOCK";
                                    privilege.message_code = 300509;
                                }
                                //else
                                {
                                    int car_total = 0;
                                    //qry period_usage with pPeriod
                                    string cmd3 = @"
SELECT COALESCE(SUM(iif(DATEADD(HOUR, 7, GETDATE()) < DATEADD(YEAR, 4, RS_Date), 1, 0)), 0) AS CNT_Elite
, COALESCE(SUM(iif(DATEADD(HOUR, 7, GETDATE()) < DATEADD(YEAR, 10, RS_Date), 1, 0)), 0) AS CNT_Lifestyle
FROM [dbo].[T_CUSTOMER_CAR] WHERE RS_Date is not null and DEL_FLAG is null and MEMBERID in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = '{0}')";
                                    using (DataTable dt2 = db.GetDataTableFromCommandText(string.Format(cmd3, token)))
                                    {
                                        car_total = privilege.display_type == 3 ? int.Parse(dt2.Rows[0][1].ToString()) : int.Parse(dt2.Rows[0][0].ToString());
                                    }

                                    if (car_total <= 0)
                                    {
                                        privilege.customer_usage_per_period = 0;
                                        privilege.message_code = privilege.display_type == 3 ? 20014002 : 20014001;
                                    }
                                    else
                                    if (int.Parse(row["customer_usage_with_car_total"].ToString()) == 1)
                                    {
                                        privilege.customer_usage_per_period = car_total;
                                    }

                                    if (privilege.display_type == 2)
                                    {
                                        if (int.Parse(row["PRIVILEGE_CNT"].ToString()) < privilege.customer_usage_per_period)
                                        {
                                            privilege.customer_usage_per_period = int.Parse(row["PRIVILEGE_CNT"].ToString());
                                        }
                                        privilege.privilege_cnt = int.Parse(row["PRIVILEGE_CNT"].ToString());
                                    }
                                    else
                                    {
                                        privilege.privilege_cnt = -1;
                                    }

                                    {
                                        int cust_usage = 0;
                                        //qry period_usage with pPeriod
                                        string cmd2 = @"
                           DECLARE @code_use_in_period INT
                           DECLARE @privilege_id INT = {0}
                           DECLARE @period_start NVARCHAR(30) = N'{1}'
                           DECLARE @period_end NVARCHAR(30) = N'{2}'
                           DECLARE @token NVARCHAR(100) = N'{3}'
                           DECLARE @repeat_redeem_when_verify_expire INT
                           -- DECLARE @code_reserve_in_period INT

                           SET @code_use_in_period = (
                                                        select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                        where PRIVILEGE_ID = @privilege_id and is_special_quota = 0
                                                        and REDEEM_DT BETWEEN @period_start and @period_end
                                                        and REDEEM_CODE is not null
                                                        and MEMBERID in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @token)
                                                        )

                           SET @repeat_redeem_when_verify_expire = (
                                                        select COALESCE(repeat_redeem_when_verify_expire, 0) from T_PRIVILEDGES
                                                        where id = @privilege_id
                                                        )

                           IF @repeat_redeem_when_verify_expire = 0 BEGIN
                               SET @code_use_in_period = @code_use_in_period + (
                                                            select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                            where PRIVILEGE_ID = @privilege_id
                                                            and REDEEM_DT BETWEEN @period_start and @period_end
                                                            and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) <> DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))
                                                            and REDEEM_CODE is null
                                                            and MEMBERID in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @token)
                                                            )
                           END

                           /*SET @code_reserve_in_period = (
                                                        select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                        where PRIVILEGE_ID = {0}
                                                        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))
                                                        and REDEEM_CODE is null
                                                        and MEMBERID in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = '{3}')
                                                        )*/
                            
                           select @code_use_in_period-- + @code_reserve_in_period";
                                        using (DataTable dt2 = db.GetDataTableFromCommandText(string.Format(cmd2, privilege_id,
                                            pPeriod.period_start.ToString("yyyy-MM-dd HH:mm:ss"), pPeriod.period_end.ToString("yyyy-MM-dd HH:mm:ss"), token)))
                                        {
                                            cust_usage = int.Parse(dt2.Rows[0][0].ToString());
                                        }

                                        privilege.customer_remain_in_period = privilege.customer_usage_per_period + special_quota - cust_usage;
                                        privilege.customer_usage_in_period = cust_usage;
                                        privilege.customer_special_quota = special_quota;
                                    }
                                }
                            }

                            privilege.privilege_period = pPeriod;
                            //list.Add(priviledge);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return privilege;
        }

        public async Task<int> GetSpecialQuota(string CustomerToken, string PrivilegeID)
        {
            int quota = 0;
            int special_quota = 0;
            int special_usage = 0;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @TOKEN NVARCHAR(50) = N'{0}'
                           DECLARE @PrivilegeID INT = {1}

                           SELECT special_quota, special_usage
                           FROM T_PRIVILEGE_SPECIAL_QUOTA c
                           LEFT JOIN T_CUSTOMER_TOKEN t ON c.member_id = t.MEMBERID
                           WHERE c.privilege_id = @PrivilegeID AND t.TOKEN_NO = @TOKEN and c.enabled = 1
                           ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, CustomerToken, PrivilegeID)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            special_quota = (int)row["special_quota"];
                            special_usage = (int)row["special_usage"];
                            if (special_quota == -1)
                                quota = -1;
                            else
                                quota = special_quota - special_usage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return quota;
        }

        public async Task<ServiceAllPrivilegeModel> GetAllPrivilege(string v, string p, string lang, string token)
        {
            ServiceAllPrivilegeModel value = new ServiceAllPrivilegeModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

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
                    value.data = new _ServiceAllPrivilegeData();
                    value.data.privileges =await GetAllDisplayPrivilege(lang, token);

                    ValidationModel.InvalidState state;
                    if (value.data.privileges.Count > 0)
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        state = ValidationModel.InvalidState.E506;
                        value.success = false;
                        value.msg = new MsgModel() { code =  ValidationModel.GetInvalidCode(state), text = await ValidationModel.GetInvalidMessageNew(state, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<List<PrivilegeModel>> GetAllDisplayPrivilege(string lang, string token)
        {
            List<PrivilegeModel> list = new List<PrivilegeModel>();
            PrivilegeModel privilege = new PrivilegeModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{0}'

SELECT 
ID,
CASE  
  WHEN @LANG = 'EN' THEN TITLE 
  ELSE TITLE END AS TITLE,
CASE  
  WHEN @LANG = 'EN' THEN [DESC] 
  ELSE desc_th END AS [DESC],
CASE  
  WHEN @LANG = 'EN' THEN RED_CONDITION 
  ELSE red_condition_th END AS RED_CONDITION,
CASE  
  WHEN @LANG = 'EN' THEN RED_LOCATION 
  ELSE red_location_th END AS RED_LOCATION,
CASE  
  WHEN @LANG = 'EN' THEN thk_message 
  ELSE thk_message_th END AS thk_message,
DATEADD(HOUR, 7, GETDATE()) AS PERIOD,
PERIOD_START,
PERIOD_END,
IMAGE,
IMAGE_1,
RED_PERIOD,
RED_EXPIRY,
PRIVILEDGE_TYPE,
display_type,
IMAGE_GALLERY_COUNT,
IMAGE_GALLERY_NO,
CASE WHEN (SELECT COUNT(1) AS CNT FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = P.ID AND STATUS = 'Y' AND DEL_FLAG IS NULL) = 0 THEN 1 ELSE 0 END AS IS_OUT_OF_STOCK,
(SELECT COUNT(1) AS CNT FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = P.ID AND STATUS = 'Y' AND DEL_FLAG IS NULL) AS all_stock,
is_show_stock,
period_type,
period_start_in_week,
period_start_in_month,
customer_usage_per_period,
overall_quota,
overall_usage,
period_quota,
period_usage,
is_show_your_remaining,
customer_usage_with_car_total,
COALESCE((SELECT top 1 PRIVILEGE_CNT FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{1}')), 0) PRIVILEGE_CNT,
CASE WHEN CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) < CONVERT(NVARCHAR(20), PERIOD_START, 120) THEN 'COMING_SOON' 
     WHEN CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) > CONVERT(NVARCHAR(20), PERIOD_END, 120) THEN 'PRIVILEGE_EXPIRED' 
     WHEN CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) < CONVERT(NVARCHAR(20), fully_redeem_to, 120) 
        and (select max(expiry_dt) from T_CUSTOMER_REDEEM
        where PRIVILEGE_ID = p.id
        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))
        and REDEEM_CODE is null
        and MEMBERID in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = '{1}')) is null THEN 'FULLY_REDEEMED'
     WHEN CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) < CONVERT(NVARCHAR(20), fully_redeem_to, 120) THEN 
        iif((select max(expiry_dt) from T_CUSTOMER_REDEEM
        where PRIVILEGE_ID = p.id
        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))
        and REDEEM_CODE is null
        and MEMBERID in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = '{1}')) > DATEADD(HOUR, 7, GETDATE()), 'VERIFY', 'REDEEM_EXPIRED')
ELSE 'REDEEM' END AS status
FROM T_PRIVILEDGES AS P
WHERE DEL_FLAG IS NULL AND P.display_type > 0 
    AND DATEADD(HOUR, 7, GETDATE()) BETWEEN DISPLAY_START AND DISPLAY_END
ORDER BY P.[order] ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, token)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string privilege_id = row["ID"].ToString();
                            privilege = new PrivilegeModel();
                            privilege.id = Convert.ToInt32(row["ID"]);
                            privilege.title = row["TITLE"].ToString();
                            privilege.desc = row["DESC"].ToString();
                            privilege.period = row["RED_PERIOD"].ToString();
                            privilege.period_m = (row["PERIOD"] == DBNull.Value) ? "" : Convert.ToDateTime(row["PERIOD"]).ToString("MMMM").ToUpper();
                            privilege.period_start = (row["PERIOD_START"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["PERIOD_START"]));
                            privilege.period_end = (row["PERIOD_END"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["PERIOD_END"]));
                            privilege.is_out_of_stock = Convert.ToBoolean(Convert.ToInt32(row["IS_OUT_OF_STOCK"].ToString()));
                            privilege.all_stock = int.Parse(row["all_stock"].ToString());
                            privilege.is_show_stock = Convert.ToBoolean(Convert.ToInt32(row["is_show_stock"].ToString()));

                            int image_gallery_count = int.Parse(row["IMAGE_GALLERY_COUNT"].ToString());
                            int image_gallery_no = int.Parse(row["IMAGE_GALLERY_NO"].ToString());
                            privilege.images = new List<string>();

                            privilege.image = row["IMAGE"].ToString();
                            privilege.image_1 = row["IMAGE_1"].ToString();
                            privilege.privilege_type = int.Parse(row["PRIVILEDGE_TYPE"].ToString());
                            privilege.display_type = int.Parse(row["display_type"].ToString());
                            privilege.thk_message = "";// row["thk_message"].ToString();

                            privilege.redeem = new RedeemModel()
                            {
                                condition = row["RED_CONDITION"].ToString(),
                                period = row["RED_PERIOD"].ToString(),
                                location = row["RED_LOCATION"].ToString()
                                //expiry = row["RED_EXPIRY"].ToString()
                            };

                            //get special quota
                            //if special quota = -1 block quota in privilege
                            int special_quota = 0;
                            special_quota =await GetSpecialQuota(token, privilege_id);

                            privilege.period_type = int.Parse(row["period_type"].ToString());
                            privilege.period_start_in_week = int.Parse(row["period_start_in_week"].ToString());
                            privilege.period_start_in_month = int.Parse(row["period_start_in_month"].ToString());
                            privilege.customer_usage_per_period = int.Parse(row["customer_usage_per_period"].ToString());
                            privilege.redeem_status = row["status"].ToString();

                            //privilege.overall_quota = int.Parse(row["overall_quota"].ToString());
                            //privilege.overall_usage = int.Parse(row["overall_quota"].ToString()) - privilege.all_stock;//int.Parse(row["overall_usage"].ToString());
                            if ((PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.UNLIMIT ||
                                (PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.ONCE_PER_CAMPAIGN)
                                privilege.period_quota = int.Parse(row["all_stock"].ToString());
                            else
                                privilege.period_quota = int.Parse(row["period_quota"].ToString());
                            PrivilegePeriodModel pPeriod = new PrivilegePeriodModel(privilege.period_type, privilege.period_start_in_week, privilege.period_start_in_month, Convert.ToDateTime(privilege.period_start), Convert.ToDateTime(privilege.period_end));
                            //get all_stock in period DAILY, WEEKLY, MONTHLY, ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH, DAILY_LIMIT_BY_MONTH, ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY
                            //if ((PrivilegePeriodModel.PeriodType)privilege.period_type != PrivilegePeriodModel.PeriodType.UNLIMIT &&
                            //    (PrivilegePeriodModel.PeriodType)privilege.period_type != PrivilegePeriodModel.PeriodType.ONCE_PER_CAMPAIGN)
                            int period_quota = 0;
//                            {
//                                int period_usage = 0;
//                                int reserve_usage = 0;
//                                //qry period_usage with pPeriod
//                                string cmd2 = @"
//select * from (
//select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
//where PRIVILEGE_ID = {0}
//and ((REDEEM_CODE is not null and REDEEM_DT BETWEEN '{1}' and '{2}') 
//  or (REDEEM_CODE is null and MEMBERID not in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = N'{3}') 
//        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))))
//) a,
//(
//select COUNT(1) AS CNT2  from T_CUSTOMER_REDEEM
//where PRIVILEGE_ID = {0}
//and REDEEM_CODE is null and MEMBERID not in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = N'{3}')
//and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(HOUR, 7, GETDATE())))) b ";
//                                using (DataTable dt2 = db.GetDataTableFromCommandText(string.Format(cmd2, privilege_id, pPeriod.period_start.ToString("yyyy-MM-dd HH:mm:ss"), pPeriod.period_end.ToString("yyyy-MM-dd HH:mm:ss"), token)))
//                                {
//                                    period_usage = int.Parse(dt2.Rows[0][0].ToString());
//                                    reserve_usage = int.Parse(dt2.Rows[0][1].ToString());
//                                }

//                                //if (privilege.all_stock >= privilege.period_quota)
//                                //    privilege.all_stock = privilege.period_quota;
//                                //else
//                                period_quota = (privilege.all_stock < privilege.period_quota - period_usage ? privilege.all_stock : privilege.period_quota - period_usage) - reserve_usage;
//                            }

                            if (period_quota == 0)
                            {
                                if ((PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.DAILY &&
                                    (PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.ONCE_PER_CAMPAIGN_LIMIT_BY_DAILY)
                                {
                                    privilege.ribbon_message = "Fully Redeemed Daily";
                                }
                                else if ((PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.WEEKLY)
                                {
                                    privilege.ribbon_message = "Fully Redeemed Weekly";
                                }
                                else if ((PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.MONTHLY &&
                                    (PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.ONCE_PER_CAMPAIGN_LIMIT_BY_MONTH &&
                                    (PrivilegePeriodModel.PeriodType)privilege.period_type == PrivilegePeriodModel.PeriodType.DAILY_LIMIT_BY_MONTH)
                                {
                                    privilege.ribbon_message = "Fully Redeemed Monthly";
                                }
                                else
                                {
                                    privilege.ribbon_message = "Fully Redeemed";
                                }
                            }

                            privilege.privilege_period = pPeriod;
                            list.Add(privilege);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public async Task<ServiceCheckPrivilegeRedeemModel> CheckPrivilegeRedeem(string v, string p, string lang, string token, string privilege_id)
        {
            ServiceCheckPrivilegeRedeemModel value = new ServiceCheckPrivilegeRedeemModel();
            ServicePrivilegeModel priv = new ServicePrivilegeModel();
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
                    priv.data = new _ServicePrivilegeData();
                    priv.data.privileges = await GetPrivilege(token, privilege_id, lang);
                    value.data = new _ServiceCheckPrivilegeRedeemData();

                    int priod_quota = 0;
                    priod_quota = priv.data.privileges.period_quota;

                    if (priv.data.privileges.redeem_status == "COMING_SOON"
                        || priv.data.privileges.redeem_status == "PRIVILEGE_EXPIRED"
                        || priv.data.privileges.redeem_status == "FULLY_REDEEMED"
                        || priv.data.privileges.redeem_status == "BLOCK")
                    {
                        value.data.status = priv.data.privileges.redeem_status;
                        //value.data.customer_usage_per_period = 0;
                        //value.data.customer_remain_in_period = 0;
                        value.data.remaining_count = 0;
                    }
                    else
                    {
                        value.data = CheckPrivilegeRedeemCode(privilege_id, token,
                            priv.data.privileges.privilege_period.period_start.ToString("yyyy-MM-dd HH:mm:ss"),
                            priv.data.privileges.privilege_period.period_end.ToString("yyyy-MM-dd HH:mm:ss"),
                            priod_quota.ToString(),
                            priv.data.privileges.customer_remain_in_period.ToString());
                        
                        //value.data.remaining_count = priod_quota;
                        value.data.customer_usage_per_period = priv.data.privileges.customer_usage_per_period;
                        value.data.customer_remain_in_period = priv.data.privileges.customer_remain_in_period;
                    }

                    ValidationModel.InvalidState state;
                    if (priv.data.privileges.message_code == 0)
                    {
                        if (priv.data.privileges.privilege_cnt != -1)
                        {
                            value.data.customer_remain_in_period = priv.data.privileges.privilege_cnt;
                        }
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        MessageService ms = new MessageService();
                        value.success = false;
                        value.msg = new MsgModel() { code = priv.data.privileges.message_code, text = await ms.GetMessagetext(priv.data.privileges.message_code, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private _ServiceCheckPrivilegeRedeemData CheckPrivilegeRedeemCode(string privilege_id, string token, string period_start, string period_end, string period_quota, string customer_remain_in_period)
        {
            _ServiceCheckPrivilegeRedeemData result = new _ServiceCheckPrivilegeRedeemData();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @TOKEN NVARCHAR(50) = N'{1}'
                           DECLARE @PRIVILEGE_USED INT
                           DECLARE @remaining_code_total INT
                           DECLARE @status NVARCHAR(50)=''
                           DECLARE @period_start DATETIME= '{2}'
                           DECLARE @period_end DATETIME= '{3}'
                           DECLARE @period_quota INT = {4}
                           DECLARE @remaining_period_quota INT
                           DECLARE @code_reserve_in_period INT
                           DECLARE @code_used_in_period INT
                           DECLARE @reserve_date DATETIME= DATEADD(HOUR, 7, GETDATE())
                           DECLARE @customer_remain_in_period INT = {5}
                           DECLARE @reserve_id int = 0
                           DECLARE @PRIVILEGE_ID int = {0}
                           DECLARE @TOKEN_Expire DATETIME

                           SET @reserve_id = ( 
                                                    SELECT top 1 c.ID
                                                    FROM T_CUSTOMER_REDEEM c
                                                    LEFT JOIN T_CUSTOMER_TOKEN t ON c.MEMBERID = t.MEMBERID
                                                    WHERE c.PRIVILEGE_ID = @PRIVILEGE_ID AND t.TOKEN_NO = @TOKEN
                                                    and REDEEM_CODE is null
                                                    and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @reserve_date))
                                                )

                           SET @remaining_code_total = (
                                                        SELECT COUNT(1) AS CNT 
                                                        FROM T_PRIVILEDGES_CODE 
                                                        WHERE PRIVILEGE_ID = @PRIVILEGE_ID 
                                                        AND STATUS = 'Y' AND DEL_FLAG IS NULL
                                                     )

                            SET @code_reserve_in_period = (
                                                        select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                        where PRIVILEGE_ID = @PRIVILEGE_ID
                                                        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @reserve_date))
                                                        and REDEEM_CODE is null and MEMBERID not in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @TOKEN)
                                                        )

                            SET @code_used_in_period = (
                                                        select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                        where PRIVILEGE_ID = @PRIVILEGE_ID
                                                        and REDEEM_DT BETWEEN @period_start and @period_end
                                                        and REDEEM_CODE is not null 
                                                        )

                           SET @period_quota = (
                                                        SELECT period_quota
                                                        FROM T_PRIVILEDGES 
                                                        WHERE ID = @PRIVILEGE_ID
                                                     )

                           SET @remaining_period_quota = ( iif(@remaining_code_total >= @period_quota - @code_used_in_period, @period_quota - @code_reserve_in_period - @code_used_in_period, @remaining_code_total - @code_reserve_in_period))

                           SET @PRIVILEGE_USED = (
                                                    SELECT COUNT(c.ID) 
                           				            FROM T_CUSTOMER_REDEEM c
                           				            LEFT JOIN T_CUSTOMER_TOKEN t ON c.MEMBERID = t.MEMBERID
                           				            WHERE c.PRIVILEGE_ID = @PRIVILEGE_ID AND t.TOKEN_NO = @TOKEN
                                                    and REDEEM_DT BETWEEN @period_start and @period_end
                                                 )

                           SET @TOKEN_Expire = (
                                                    SELECT top 1 TOKEN_EXPIREY
                           				            FROM T_CUSTOMER_TOKEN
                           				            WHERE TOKEN_NO = @TOKEN
                                                    order by [TOKEN_EXPIREY] DESC
                                                 )
                            BEGIN
                                IF @TOKEN_Expire is null or @TOKEN_Expire < DATEADD(hour, 7, GETDATE())
                                begin
                                    SELECT 'REDEEM' AS status, null EXPIRY_DT, iif(@remaining_period_quota < 0, 0, @remaining_period_quota) remaining_period_quota 
                                end
                                IF @reserve_id is not null and @reserve_id > 0 --user repeat request
                                begin
                                    SELECT CASE WHEN EXPIRY_DT >= DATEADD(HOUR, 7, GETDATE()) THEN 'VERIFY' ELSE 'REDEEM_EXPIRED' END AS status, 
                                        EXPIRY_DT, iif(@remaining_period_quota < 0, 1, @remaining_period_quota) remaining_period_quota
                                    FROM T_CUSTOMER_REDEEM where ID = @reserve_id
                                end
                                else IF @customer_remain_in_period <= 0 and @PRIVILEGE_USED > 0
                                begin
                                    SELECT CASE WHEN EXPIRY_DT >= DATEADD(HOUR, 7, GETDATE()) or REDEEM_CODE is not null THEN 'REDEEMED' ELSE 'REDEEM_EXPIRED' END AS status, 
                                        EXPIRY_DT, iif(@remaining_period_quota < 0, 1, @remaining_period_quota) remaining_period_quota
                                    FROM T_CUSTOMER_REDEEM where ID = (SELECT  max(c.ID)
                                                    FROM T_CUSTOMER_REDEEM c
                                                    LEFT JOIN T_CUSTOMER_TOKEN t ON c.MEMBERID = t.MEMBERID
                                                    WHERE c.PRIVILEGE_ID = @PRIVILEGE_ID AND t.TOKEN_NO = @TOKEN)
                                    -- SELECT 'REDEEMED' AS status, null EXPIRY_DT, iif(@remaining_period_quota < 0, 0, @remaining_period_quota) remaining_period_quota
                                end
                                else IF @customer_remain_in_period <= 0 and @PRIVILEGE_USED <= 0
                                begin
                                    SELECT 'REDEEM' AS status, null EXPIRY_DT, iif(@remaining_period_quota < 0, 0, @remaining_period_quota) remaining_period_quota 
                                end
	                    	    ELSE IF @remaining_period_quota > 0             --new or old member that not use this privilege.
	                    	        SELECT 'REDEEM' AS status, null EXPIRY_DT, @remaining_period_quota remaining_period_quota
	                    	    ELSE --IF @remaining_period_quota = 0 or @remaining_code_total = 0
	                    	    	SELECT 'FULLY_REDEEMED' AS status, null EXPIRY_DT, 0 remaining_period_quota
                                -- remaining_period_quota = 0 and remaining_code_total > 0 Fully period
	                        END";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id, token, period_start, period_end, period_quota, customer_remain_in_period)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            result.status = row["status"].ToString();
                            result.expiry_ts = (row["EXPIRY_DT"] == DBNull.Value || row["status"].ToString() == "REDEEMED") ?
                                UtilityService.GetDateTimeFormat(DateTime.Now.AddYears(100)) : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY_DT"]));
                            result.remaining_count = (int)row["remaining_period_quota"];
                        }
                        else
                        {
                            result.status = "FULLY_REDEEMED";
                            result.expiry_ts = "";
                            result.remaining_count = 0;
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

        public async Task<ServiceCheckPrivilegeRedeemModel> ReserveQuota(string v, string p, string lang, string token, string privilege_id)
        {
            ServiceCheckPrivilegeRedeemModel value = new ServiceCheckPrivilegeRedeemModel();
            ServicePrivilegeModel priv = new ServicePrivilegeModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

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
                    priv.data = new _ServicePrivilegeData();
                    priv.data.privileges =await GetPrivilege(token, privilege_id, lang);
                    value.data = new _ServiceCheckPrivilegeRedeemData();

                    if (priv.data.privileges.message_code != 0)
                    {
                        MessageService ms = new MessageService();
                        value.success = false;
                        value.msg = new MsgModel() { code = 0, text = await ms.GetMessagetext(priv.data.privileges.message_code, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    int priod_quota = 0;
                    priod_quota = priv.data.privileges.period_quota;

                    if (priv.data.privileges.redeem_status == "COMING_SOON"
                        || priv.data.privileges.redeem_status == "PRIVILEGE_EXPIRED"
                        || priv.data.privileges.redeem_status == "FULLY_REDEEMED")
                    {
                        value.data.status = priv.data.privileges.redeem_status;
                        value.data.customer_usage_per_period = 0;
                        value.data.customer_remain_in_period = 0;
                        value.data.remaining_count = 0;
                    }
                    else
                    {
                        value.data = await CheckAndReserveQuota(privilege_id, token,
                            priv.data.privileges.privilege_period.period_start.ToString("yyyy-MM-dd HH:mm:ss"),
                            priv.data.privileges.privilege_period.period_end.ToString("yyyy-MM-dd HH:mm:ss"),
                            priod_quota.ToString(),
                            priv.data.privileges.customer_remain_in_period.ToString());

                        //value.data.remaining_count = priod_quota;
                        value.data.customer_usage_per_period = priv.data.privileges.customer_usage_per_period;
                        value.data.customer_remain_in_period = priv.data.privileges.customer_remain_in_period;
                    }


                    ValidationModel.InvalidState state;
                    value.success = value.data.status == "VERIFY" || value.data.status == "PROCESS";
                    MessageService MS = new MessageService();
                    if (value.data.status == "REDEEMED")
                        value.msg = new MsgModel() { code = 0, text = await MS.GetMessagetext(300504, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    else if (value.data.status == "FULLY_REDEEMED")
                        value.msg = new MsgModel() { code = 0, text =await MS.GetMessagetext(300503, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    else if (value.data.status == "CANNOT_USES")
                        value.msg = new MsgModel() { code = 0, text = await MS.GetMessagetext(300501, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    else if (value.data.status == "BLOCK")
                        value.msg = new MsgModel() { code = 0, text =await  MS.GetMessagetext(300509, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    else
                    {
                        if (priv.data.privileges.privilege_cnt != -1)
                        {
                            value.data.customer_remain_in_period = priv.data.privileges.privilege_cnt;
                        }
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    } 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<_ServiceCheckPrivilegeRedeemData> CheckAndReserveQuota(string privilege_id, string token, string period_start, string period_end, string period_quota, string customer_remain_in_period)
        {
            _ServiceCheckPrivilegeRedeemData result = new _ServiceCheckPrivilegeRedeemData();
            int SpecialQuota = await GetSpecialQuota(token, privilege_id);
            try
            {
                if (SpecialQuota == -1)
                {
                    result.status = "BLOCK";
                    result.expiry_ts = "";
                }
                else
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = @"
                            DECLARE @privilege_id INT = {0}
                            DECLARE @TOKEN NVARCHAR(50) = N'{1}'
                            DECLARE @PRIVILEGE_USED INT
                            DECLARE @remaining_code_total INT
                            DECLARE @code_reserve_in_period INT
                            DECLARE @code_used_in_period INT
                            DECLARE @status NVARCHAR(50)=''
                            DECLARE @period_start DATETIME= '{2}'
                            DECLARE @period_end DATETIME= '{3}'
                            DECLARE @period_quota INT = {4}
                            DECLARE @reserve_date DATETIME= DATEADD(HOUR, 7, GETDATE())
                            DECLARE @remaining_period_quota INT
                            DECLARE @EXPIRY_TYPE INT = (SELECT red_expiry_type FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID)
                            DECLARE @EXPIRY_DATE DATETIME
                            DECLARE @reserve_id INT = 0
                            DECLARE @redeemed_id INT = 0
                            DECLARE @SpecialQuota INT = {5}
                            DECLARE @customer_remain_in_period INT = {6}
                            DECLARE @PRIVILEGE_TYPE INT = (SELECT PRIVILEDGE_TYPE FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID)
                            DECLARE @DISPLAY_TYPE INT = (SELECT display_type FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID)
                            DECLARE @insert_record int

                            SET @reserve_id = ( 
                                                    SELECT top 1 c.ID
                                                    FROM T_CUSTOMER_REDEEM c
                                                    LEFT JOIN T_CUSTOMER_TOKEN t ON c.MEMBERID = t.MEMBERID
                                                    WHERE c.PRIVILEGE_ID = @privilege_id AND t.TOKEN_NO = @TOKEN
                                                    and REDEEM_CODE is null
                                                    and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @reserve_date))
                                                )

                            SET @redeemed_id = ( 
                                                    SELECT top 1 c.ID
                                                    FROM T_CUSTOMER_REDEEM c
                                                    LEFT JOIN T_CUSTOMER_TOKEN t ON c.MEMBERID = t.MEMBERID
                                                    WHERE c.PRIVILEGE_ID = @privilege_id AND t.TOKEN_NO = @TOKEN
                                                    and REDEEM_CODE is not null
                                                    and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @reserve_date))
                                                )

                            SET @remaining_code_total = (
                                                        SELECT COUNT(1) AS CNT 
                                                        FROM T_PRIVILEDGES_CODE 
                                                        WHERE PRIVILEGE_ID = @privilege_id
                                                        AND STATUS = 'Y' AND DEL_FLAG IS NULL
                                                        )

                            SET @code_reserve_in_period = (
                                                        select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                        where PRIVILEGE_ID = @privilege_id
                                                        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @reserve_date))
                                                        and REDEEM_CODE is null and MEMBERID not in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @TOKEN)
                                                        )

                            SET @code_used_in_period = (
                                                        select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                        where PRIVILEGE_ID = @PRIVILEGE_ID
                                                        and REDEEM_DT BETWEEN @period_start and @period_end
                                                        and REDEEM_CODE is not null 
                                                        )

                           SET @period_quota = (
                                                        SELECT period_quota
                                                        FROM T_PRIVILEDGES 
                                                        WHERE ID = @PRIVILEGE_ID
                                                     )

                           SET @remaining_period_quota = ( iif(@remaining_code_total >= @period_quota - @code_used_in_period, @period_quota - @code_reserve_in_period - @code_used_in_period, @remaining_code_total - @code_reserve_in_period))

                            SET @PRIVILEGE_USED = (
                                                    SELECT COUNT(c.ID) 
                                                    FROM T_CUSTOMER_REDEEM c
                                                    LEFT JOIN T_CUSTOMER_TOKEN t ON c.MEMBERID = t.MEMBERID
                                                    WHERE c.PRIVILEGE_ID = @privilege_id AND t.TOKEN_NO = @TOKEN
                                                    and REDEEM_DT BETWEEN @period_start and @period_end
                                                    )

                            BEGIN
	                            --print @status
                                IF @reserve_id is not null and @reserve_id > 0 --user repeat request
                                begin
                                    SELECT CASE WHEN EXPIRY_DT >= DATEADD(HOUR, 7, GETDATE()) THEN 'VERIFY' ELSE 'REDEEM_EXPIRED' END AS status, 
                                        EXPIRY_DT, iif(@remaining_period_quota < 0, 1, @remaining_period_quota) remaining_period_quota
                                    FROM T_CUSTOMER_REDEEM where ID = @reserve_id
                                end
                                else IF @customer_remain_in_period <= 0 and @PRIVILEGE_USED > 0
                                begin
                                    SELECT 'REDEEMED' AS status, null EXPIRY_DT, iif(@remaining_period_quota < 0, 0, @remaining_period_quota) remaining_period_quota 
                                end
                                else IF @customer_remain_in_period <= 0 and @PRIVILEGE_USED <= 0
                                begin
                                    SELECT 'CANNOT_USES' AS status, null EXPIRY_DT, iif(@remaining_period_quota < 0, 0, @remaining_period_quota) remaining_period_quota 
                                end
	                            ELSE 
                                BEGIN
                                    IF @EXPIRY_TYPE = 1 BEGIN
                                        SET @EXPIRY_DATE = DATEADD(HOUR, 7, DATEADD(MINUTE, ISNULL((SELECT RED_EXPIRY FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID), 0), GETDATE()))
                                    END
                                    ELSE BEGIN
                                        SET @EXPIRY_DATE = (SELECT convert(datetime, CONVERT(VARCHAR(10), CONVERT(DATE, GETDATE(), 102), 120) + ' ' + (SELECT RED_EXPIRY_TIME FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID), 120))
                                        SET @EXPIRY_DATE = (CASE WHEN CONVERT(datetime, @EXPIRY_DATE, 103) < DATEADD(HOUR, 7, GETDATE()) THEN DATEADD(DAY, 1, CONVERT(datetime, @EXPIRY_DATE, 103)) ELSE CONVERT(datetime, @EXPIRY_DATE, 103) END)
                                    END

                                    IF @PRIVILEGE_TYPE = 3 BEGIN --Redeem Code
                                        SET @EXPIRY_DATE = DATEADD(HOUR, 7, DATEADD(YEAR, 100, GETDATE()))
                                    END

                                    SET @remaining_code_total = (
                                                                SELECT COUNT(1) AS CNT 
                                                                FROM T_PRIVILEDGES_CODE 
                                                                WHERE PRIVILEGE_ID = @privilege_id
                                                                AND STATUS = 'Y' AND DEL_FLAG IS NULL
                                                                )

                                    SET @code_used_in_period = (
                                                                select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                                where PRIVILEGE_ID = @PRIVILEGE_ID
                                                                and REDEEM_DT BETWEEN @period_start and @period_end
                                                                and REDEEM_CODE is not null 
                                                                )

                                    SET @code_reserve_in_period = (
                                                                select COUNT(1) AS CNT  from T_CUSTOMER_REDEEM
                                                                where PRIVILEGE_ID = @privilege_id
                                                                and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @reserve_date))
                                                                and REDEEM_CODE is null and MEMBERID not in (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @TOKEN)
                                                                )

                                    SET @remaining_period_quota = ( iif(@remaining_code_total >= @period_quota - @code_used_in_period, 
                                        @period_quota - @code_reserve_in_period - @code_used_in_period, 
                                        @remaining_code_total - @code_reserve_in_period))

                                    IF @remaining_period_quota > 0             --new or old member that not use this privilege.
                                    BEGIN
                                        INSERT INTO T_CUSTOMER_REDEEM (CUS_ID, MEMBERID, PRIVILEGE_ID, [NO], REDEEM_CODE, REDEEM_DT, EXPIRY_DT, SHOP_NM, is_special_quota)
                                        SELECT C.ID, C.MEMBERID, @privilege_id, null, null, DATEADD(HOUR, 7, GETDATE()), CONVERT(DATETIME, @EXPIRY_DATE, 103), (select title from T_PRIVILEDGES where id = @privilege_id), iif(@SpecialQuota>0,1,0)
                                        FROM T_CUSTOMER_TOKEN T
                                        INNER JOIN T_CUSTOMER C ON C.MEMBERID = T.MEMBERID
                                        WHERE T.TOKEN_NO = @TOKEN

                                        set @insert_record = (select top 1 id from T_CUSTOMER_REDEEM
                                            where PRIVILEGE_ID = @privilege_id and EXPIRY_DT = CONVERT(DATETIME, @EXPIRY_DATE, 103)
                                            and MEMBERID = (select top 1 MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @TOKEN))


                                        IF @SpecialQuota > 0 and @insert_record is not null
                                        BEGIN
                                            update T_PRIVILEGE_SPECIAL_QUOTA
                                            set special_usage = special_usage + 1
                                            where member_id = (select top 1 MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @TOKEN)
                                            and privilege_id = @privilege_id
                                            and enabled = 1
                                        END
                                        IF @DISPLAY_TYPE = 2 and @insert_record is not null BEGIN --Luxury Privilege
                                            UPDATE T_CUSTOMER SET PRIVILEGE_CNT = PRIVILEGE_CNT - 1 WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN T WHERE T.TOKEN_NO = @TOKEN)
                                        END

                                        if @remaining_period_quota = 1
                                        begin
                                            update T_PRIVILEDGES
                                            set fully_redeem_to = CONVERT(NVARCHAR(10), @reserve_date, 120) + ' 23:59:59'
                                            where id = @privilege_id
                                        end

	                                    SELECT 'VERIFY' AS status, @EXPIRY_DATE EXPIRY_DT, @remaining_period_quota remaining_period_quota
                                    END
	                                ELSE -- remaining_period_quota = 0 and remaining_code_total = 0
	                                    SELECT 'FULLY_REDEEMED' AS status, null EXPIRY_DT, 0 remaining_period_quota 
                                END
                            END
                               ";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id, token, period_start, period_end, period_quota, SpecialQuota, customer_remain_in_period)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];

                                result.status = row["status"].ToString();
                                result.expiry_ts = (row["EXPIRY_DT"] == DBNull.Value) ?
                                    UtilityService.GetDateTimeFormat(DateTime.Now.AddYears(100)) : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY_DT"]));
                                result.remaining_count = (int)row["remaining_period_quota"];
                            }
                            else
                            {
                                result.status = "FULLY_REDEEMED";
                                result.expiry_ts = "";
                                result.remaining_count = 0;
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
        
        public async  Task<ServiceRedeemModel> VerifyCode(string v, string p, string lang, string token, string privilege_id, string verify_code)
        {
            ServiceRedeemModel value = new ServiceRedeemModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

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
                    ServicePrivilegeModel priv = new ServicePrivilegeModel();
                    priv.data = new _ServicePrivilegeData();
                    priv.data.privileges = await GetPrivilege(token, privilege_id, lang);
                    value.data = new _ServiceRedeemData();
                    value.data = await VerifyAndStampCode(privilege_id, token, verify_code, priv.data.privileges.period_end);

                    ValidationModel.InvalidState state;
                    value.success = value.data.status == "REDEEMED";
                    MessageService MS = new MessageService();
                    if (value.data.status == "FULLY_REDEEMED")
                        value.msg = new MsgModel() { code = 0, text = await MS.GetMessagetext(300503, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    if (value.data.status == "INCORRECT")
                        value.msg = new MsgModel() { code = 0, text = await MS.GetMessagetext(300601, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    else
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<_ServiceRedeemData> VerifyAndStampCode(string privilege_id, string token, string verify_code, string period_end)
        {
            _ServiceRedeemData value = new _ServiceRedeemData();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @PRIVILEGE_ID INT = {0}
DECLARE @TOKEN NVARCHAR(100) = N'{1}'
DECLARE @NO INT
DECLARE @REDEEM_CODE NVARCHAR(50)
DECLARE @reserve_id INT
DECLARE @reserve_date DATETIME= DATEADD(HOUR, 7, GETDATE())
DECLARE @verify_privilege INT
DECLARE @VERIFY_CODE NVARCHAR(100) = N'{2}'
DECLARE @redeem_display_type INT
DECLARE @DISPLAY_TYPE INT = (SELECT display_type FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID)
DECLARE @temp TABLE (redeem_no INT, redeem_code NVARCHAR(50))
DECLARE @reserve_count INT
DECLARE @period_end datetime = '{3}'
DECLARE @shop_name nvarchar(150)

SET @redeem_display_type = (SELECT redeem_display_type FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID)
SET @verify_privilege = (SELECT PRIVILEGE_ID FROM T_PRIVILEGE_VERIFY WHERE VERIFY_CODE = @VERIFY_CODE and PRIVILEGE_ID = @PRIVILEGE_ID AND DEL_FLAG IS NULL)
SET @shop_name = (SELECT SHOP_NM FROM T_PRIVILEGE_VERIFY WHERE VERIFY_CODE = @VERIFY_CODE and PRIVILEGE_ID = @PRIVILEGE_ID AND DEL_FLAG IS NULL)

SET @reserve_id = ( 
                        SELECT top 1 c.ID
                        FROM T_CUSTOMER_REDEEM c
                        LEFT JOIN T_CUSTOMER_TOKEN t ON c.MEMBERID = t.MEMBERID
                        WHERE c.PRIVILEGE_ID = @privilege_id AND t.TOKEN_NO = @TOKEN
                        and REDEEM_CODE is null
                        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @reserve_date))
                    )

IF @verify_privilege is null or @verify_privilege <> @PRIVILEGE_ID
    select 0 id, '' REDEEM_CODE, null EXPIRY_DT, 'INCORRECT' status, @redeem_display_type redeem_display_type, null REDEEM_DT, null VERIFY_DT
ELSE IF @reserve_id is not null and @reserve_id > 0 BEGIN
    Update mc
	Set mc.STATUS = 'N'
	OUTPUT INSERTED.[NO], INSERTED.REDEEM_CODE into @temp
	From
	(
			Select Top 1 [NO], REDEEM_CODE, status
			From T_PRIVILEDGES_CODE
			Where privilege_id = @privilege_id
			and status = 'Y' AND DEL_FLAG IS NULL
			Order By [NO]
	) mc

    set @REDEEM_CODE = (select redeem_code from @temp)
    set @NO = (select redeem_no from @temp)

    IF @REDEEM_CODE is not null and @REDEEM_CODE <> '' BEGIN

        set @reserve_count = ( select count(REDEEM_DT) from T_CUSTOMER_REDEEM
                        where PRIVILEGE_ID = @privilege_id
                        and REDEEM_CODE is null
                        and DATEADD(dd, 0, DATEDIFF(dd, 0, REDEEM_DT)) = DATEADD(dd, 0, DATEDIFF(dd, 0, @reserve_date)))

        update T_CUSTOMER_REDEEM
        set [NO] = @NO
        , REDEEM_CODE = @REDEEM_CODE
        , VERIFY_DT = @reserve_date
        , SHOP_NM = @shop_name
        where id = @reserve_id

        if @reserve_count = (Select count(*) From T_PRIVILEDGES_CODE Where privilege_id = @privilege_id and status = 'Y' AND DEL_FLAG IS NULL) 
        begin
            update T_PRIVILEDGES
            set fully_redeem_to = @period_end
            where id = @privilege_id
        end

        if (Select count(*) From T_PRIVILEDGES_CODE Where privilege_id = @privilege_id and status = 'Y' AND DEL_FLAG IS NULL) = 0
        begin
            update T_PRIVILEDGES
            set fully_redeem_to = period_end
            where id = @privilege_id
        end

        select *, 'REDEEMED' status
        from T_CUSTOMER_REDEEM a, 
        (SELECT redeem_display_type, redeem_display_height, redeem_display_width, redeem_display_image, redeem_display_html
            FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID) b
        where id = @reserve_id
    END
    ELSE BEGIN
    -- not enough REDEEM_CODE
        select 0 id, '' REDEEM_CODE, null EXPIRY_DT, 'FULLY_REDEEMED' status, @redeem_display_type redeem_display_type, null REDEEM_DT, null VERIFY_DT
    END
END
ELSE BEGIN
-- reserve record not found
    select 0 id, '' REDEEM_CODE, null EXPIRY_DT, 'FULLY_REDEEMED' status, @redeem_display_type redeem_display_type, null REDEEM_DT, null VERIFY_DT
END
";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id, token, verify_code, period_end)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            value.id = Convert.ToInt32(row["id"]);
                            value.redeem_code = row["REDEEM_CODE"].ToString();
                            value.expiry_ts = (row["EXPIRY_DT"] == DBNull.Value || row["status"].ToString() == "REDEEMED") ?
                                UtilityService.GetDateTimeFormat(DateTime.Now.AddYears(100)) : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY_DT"]));
                            value.status = row["status"].ToString();
                            value.iredeem_display_type = (int)row["redeem_display_type"];
                            value.redeem_datetime = (row["VERIFY_DT"] == DBNull.Value) ? 
                                ((row["REDEEM_DT"] == DBNull.Value) ? "" : Convert.ToDateTime(row["REDEEM_DT"]).ToString("dd/MM/yyyy HH:mm")) :
                                Convert.ToDateTime(row["VERIFY_DT"]).ToString("dd/MM/yyyy HH:mm");

                            switch (value.iredeem_display_type)
                            {
                                case 1:
                                    value.redeem_display_type = "text";
                                    break;
                                case 2:
                                    value.redeem_display_type = "qr_code";
                                    break;
                                case 3:
                                    value.redeem_display_type = "barcode";
                                    break;
                                case 4:
                                    value.redeem_display_type = "html";
                                    break;
                                default:
                                    value.redeem_display_type = "text";
                                    break;
                            }

                            if ((int)row["redeem_display_type"] == 4 && value.status == "REDEEMED") //html and barcode
                            {
                                value.redeem_display_height = (int)row["redeem_display_height"];
                                value.redeem_display_width = (int)row["redeem_display_width"];
                                value.redeem_display_image = row["redeem_display_image"].ToString();
                                string redeem_html = row["redeem_display_html"].ToString();
                                string URLBARCODE = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusURL"]
                                    + "/Page/GenerateBarcodeImage.aspx?d=" + value.redeem_code
                                    + "&h=200&w=600&bc=ffffff&fc=000000&t=Code 128&il=false&if=jpeg&align=c";
                                string URLQRCODE = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusURL"]
                                    + "/Page/GenerateQRCodeImage.aspx?d=" + value.redeem_code;
                                string URLREDEEMBACKGROUND = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusURL"]
                                    + "/images/" + value.redeem_display_image;
                                value.redeem_display_html = redeem_html
                                    .Replace("URLBARCODE", URLBARCODE)
                                    .Replace("URLQRCODE", URLQRCODE)
                                    .Replace("REDEEMEDCODE", value.redeem_code)
                                    .Replace("URLREDEEMBACKGROUND", URLREDEEMBACKGROUND);
                            }
                            //value.expiry_min = Convert.ToInt16(row["EXPIRY_DT"]);
                            //value.expiry_ts = UtilityService.GetDateTimeFormat(DateTime.Now);
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

    }
}