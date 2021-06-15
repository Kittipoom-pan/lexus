using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class RedeemService
    {
        private string conn;
        public RedeemService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        //public ServiceRedeemModel RedeemItem(string token, int privilege_id, string v, string p)
        //{
        //    ServiceRedeemModel value = new ServiceRedeemModel();
        //    try
        //    {
        //        DateTime ts = DateTime.Now;
        //        value.ts = UtilityService.GetDateTimeFormat(ts);

        //        SystemController syc = new SystemController();
        //        ValidationModel validation2 = syc.CheckSystem(p, v);
        //        if (!validation2.Success)
        //        {
        //            value.success = validation2.Success;
        //            value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
        //            return value;
        //        }
        //        else
        //        {
        //            ValidationModel validation = CheckValidation(token, privilege_id);
        //            if (!validation.Success)
        //            {
        //                value.success = validation.Success;
        //                value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
        //                return value;
        //            }

        //            //if (!IsCheckPrivilegeCount(token))
        //            //{
        //            //    return value;
        //            //}
        //            //if (!IsCheckPrivilege(privilege_id))
        //            //{
        //            //    return value;
        //            //}

        //            value.data = Redeem(token, privilege_id);

        //            value.success = true;
        //            value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return value;
        //}

        //public ServiceRedeemModel RedeemItemNew(string token, int privilege_id, string v, string p, string lang)
        //{
        //    ServiceRedeemModel value = new ServiceRedeemModel();
        //    try
        //    {
        //        DateTime ts = DateTime.Now;
        //        value.ts = UtilityService.GetDateTimeFormat(ts);

        //        SystemController syc = new SystemController();
        //        ValidationModel validation2 = syc.CheckSystemNew(p, v, lang);
        //        if (!validation2.Success)
        //        {
        //            value.success = validation2.Success;
        //            value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
        //            return value;
        //        }
        //        else
        //        {
        //            //ValidationModel validation = CheckValidationNew(token, privilege_id, lang);
        //            //if (!validation.Success)
        //            //{
        //            //    value.success = validation.Success;
        //            //    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
        //            //    return value;
        //            //}

        //            //if (!IsCheckPrivilegeCount(token))
        //            //{
        //            //    return value;
        //            //}
        //            //if (!IsCheckPrivilege(privilege_id))
        //            //{
        //            //    return value;
        //            //}

        //            PrivilegeService srv = new PrivilegeService();
        //            ServiceCheckPrivilegeRedeemModel CheckRedeem = new ServiceCheckPrivilegeRedeemModel();
        //            CheckRedeem = srv.CheckPrivilegeRedeem(v, p, lang, token, privilege_id.ToString());
        //            if (CheckRedeem == null || CheckRedeem.data == null || CheckRedeem.data.status == null)
        //            {
        //                value.msg = new MsgModel() { code = 0, text = "Redeem not success", store_link = "", version = "" };
        //            }
        //            else if (CheckRedeem.data.status == "FULLY_REDEEMED")
        //            {
        //                value.msg = new MsgModel() { code = 0, text = "Privilege is fully redeem", store_link = "", version = "" };
        //            }
        //            else if (CheckRedeem.data.status == "REDEEMED")
        //            {
        //                value.msg = new MsgModel() { code = 0, text = "Privilege has redeemed", store_link = "", version = "" };
        //            }
        //            else if (CheckRedeem.data.status == "REDEEM")
        //            {
        //                value.data = Redeem(token, privilege_id);
        //                value.success = true;
        //                value.msg = new MsgModel() { code = 0, text = "Success", store_link = "", version = "" };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return value;
        //}

        public async Task<ServiceRedeemModel> RedeemItemOld(string token, int privilege_id, string v, string p, string lang)
        {
            ServiceRedeemModel value = new ServiceRedeemModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation2 = await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                    return value;
                }
                else
                {
                    bool isUseSpecialQuota;
                    ValidationModel validation =  CheckValidationOld(token, privilege_id, lang, out isUseSpecialQuota);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    //if (!IsCheckPrivilegeCount(token))
                    //{
                    //    value.success = validation.Success;
                    //    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    //    return value;
                    //}
                    //if (!IsCheckPrivilege(privilege_id))
                    //{
                    //    value.success = validation.Success;
                    //    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    //    return value;
                    //}
                    ServicePrivilegeModel priv = new ServicePrivilegeModel();
                    PrivilegeService privSvr = new PrivilegeService();
                    priv.data = new _ServicePrivilegeData();
                    priv.data.privileges = await privSvr.GetPrivilege(token, privilege_id.ToString(), lang);

                    if (priv.data.privileges.message_code != 0)
                    {
                        MessageService ms = new MessageService();
                        value.success = false;
                        value.msg = new MsgModel() { code = 0, text =await ms.GetMessagetext(priv.data.privileges.message_code, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    int priod_quota = 0;
                    priod_quota = priv.data.privileges.period_quota;

                    if (priv.data.privileges.redeem_status == "COMING_SOON"
                        || priv.data.privileges.redeem_status == "PRIVILEGE_EXPIRED"
                        || priv.data.privileges.redeem_status == "FULLY_REDEEMED")
                    {
                        value.data.status = priv.data.privileges.redeem_status;
                    }
                    else
                    {
                        value.data =await Redeem(privilege_id.ToString(), token,
                            priv.data.privileges.privilege_period.period_start.ToString("yyyy-MM-dd HH:mm:ss"),
                            priv.data.privileges.privilege_period.period_end.ToString("yyyy-MM-dd HH:mm:ss"),
                            priod_quota.ToString(),
                            priv.data.privileges.customer_remain_in_period.ToString());
                    }

                    value.success = value.data.status == "REDEEMED";
                    MessageService MS = new MessageService();
                    if (value.data.status == "REDEEMED")
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    else if (value.data.status == "CANNOT_USES")
                        value.msg = new MsgModel() { code = 0, text =await MS.GetMessagetext(300501, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    else if (value.data.status == "BLOCK")
                        value.msg = new MsgModel() { code = 0, text =await MS.GetMessagetext(300509, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    else
                        value.msg = new MsgModel() { code = 0, text =await MS.GetMessagetext(300503, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private bool IsCheckPrivilegeCount(string token)
        {
            bool flag = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT ISNULL(PRIVILEGE_CNT, 0) AS PRIVILEGE_CNT
FROM T_CUSTOMER C
INNER JOIN T_CUSTOMER_TOKEN T ON C.MEMBERID = T.MEMBERID
WHERE T.TOKEN_NO = N'{0}'";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dt.Rows[0]["PRIVILEGE_CNT"]) > 0)
                            {
                                flag = true;
                            }
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
        private bool IsCheckPrivilege(int privilege_id)
        {
            bool flag = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT COUNT(1) AS CNT_PRIVILEDGES
FROM  T_PRIVILEDGES P
INNER JOIN T_PRIVILEDGES_CODE PC ON PC.PRIVILEGE_ID = P.ID
WHERE CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), [PERIOD_START], 120) AND CONVERT(NVARCHAR(20), [PERIOD_END], 120) 
AND P.ID = {0}";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dt.Rows[0]["CNT_PRIVILEDGES"]) > 0)
                            {
                                flag = true;
                            }
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
        private async Task<_ServiceRedeemData> Redeem(string privilege_id, string token, string period_start, string period_end, string period_quota, string customer_remain_in_period)
        {
            _ServiceRedeemData value = new _ServiceRedeemData();
            PrivilegeService priv = new PrivilegeService();
            int SpecialQuota = await priv.GetSpecialQuota(token, privilege_id);
            try
            {
                if (SpecialQuota == -1)
                {
                    value.status = "BLOCK";
                    value.expiry_ts = "";
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
                            DECLARE @temp TABLE (redeem_no INT, redeem_code NVARCHAR(50))
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

                                        INSERT INTO T_CUSTOMER_REDEEM (CUS_ID, MEMBERID, PRIVILEGE_ID, [NO], REDEEM_CODE, REDEEM_DT, EXPIRY_DT, SHOP_NM, is_special_quota)
                                        SELECT C.ID, C.MEMBERID, @privilege_id, te.redeem_no, te.redeem_code, DATEADD(HOUR, 7, GETDATE()), 
                                            CONVERT(DATETIME, @EXPIRY_DATE, 103), (select title from T_PRIVILEDGES where id = @privilege_id), iif(@SpecialQuota>0,1,0)
                                        FROM T_CUSTOMER_TOKEN T 
                                        INNER JOIN T_CUSTOMER C ON C.MEMBERID = T.MEMBERID
		                                join @temp te on te.redeem_no is not null
                                        WHERE T.TOKEN_NO = @TOKEN

                                        set @insert_record = SCOPE_IDENTITY()

                                        IF @DISPLAY_TYPE = 2 and @insert_record is not null BEGIN --Luxury Privilege
                                            UPDATE T_CUSTOMER SET PRIVILEGE_CNT = PRIVILEGE_CNT - 1 WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN T WHERE T.TOKEN_NO = @TOKEN)
                                        END

                                        IF @SpecialQuota > 0 and @insert_record is not null
                                        BEGIN
                                            update T_PRIVILEGE_SPECIAL_QUOTA
                                            set special_usage = special_usage + 1
                                            where member_id = (select top 1 MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @TOKEN)
                                            and privilege_id = @privilege_id
                                            and enabled = 1
                                        END

                                        if @remaining_period_quota = 1
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

                                        SELECT TOP 1 CR.id, CR.[REDEEM_CODE], CR.[EXPIRY_DT], 'REDEEMED' status, redeem_display_type, 
                                            redeem_display_height, redeem_display_width, redeem_display_image, redeem_display_html, CR.REDEEM_DT
                                        FROM (SELECT redeem_display_type, redeem_display_height, redeem_display_width, redeem_display_image, redeem_display_html
                                            FROM T_PRIVILEDGES WHERE ID = @privilege_id) b, T_CUSTOMER_REDEEM CR
                                        INNER JOIN T_CUSTOMER_TOKEN T ON CR.MEMBERID = T.MEMBERID
                                        join @temp te on te.redeem_no is not null
                                        WHERE TOKEN_NO = @TOKEN
                                        AND CR.PRIVILEGE_ID = @privilege_id
                                        ORDER BY CR.id DESC
                                    END
	                                ELSE -- remaining_period_quota = 0 and remaining_code_total = 0
	                                    SELECT 'FULLY_REDEEMED' AS status, null EXPIRY_DT, 0 remaining_period_quota 
                                END
                            END";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id, token, period_start, period_end, period_quota, SpecialQuota, customer_remain_in_period)))
                        {
                            if (dt == null || dt.Rows.Count == 0)
                            {

                            }
                            else
                            {
                                DataRow row = dt.Rows[0];
                                if (row["status"].ToString() != "REDEEMED")
                                {
                                    value.status = row["status"].ToString();
                                }
                                else
                                {
                                    value.id = Convert.ToInt32(row["id"]);
                                    value.redeem_code = row["REDEEM_CODE"].ToString();
                                    value.expiry_ts = (row["EXPIRY_DT"] == DBNull.Value || row["status"].ToString() == "REDEEMED") ?
                                        UtilityService.GetDateTimeFormat(DateTime.Now.AddYears(100)) : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY_DT"]));
                                    value.status = row["status"].ToString();
                                    value.iredeem_display_type = (int)row["redeem_display_type"];
                                    value.redeem_datetime = (row["REDEEM_DT"] == DBNull.Value) ? "" : Convert.ToDateTime(row["REDEEM_DT"]).ToString("dd/MM/yyyy HH:mm");

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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private ValidationModel CheckValidation(string token, int privilege_id)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    #region E501
                    state = ValidationModel.InvalidState.E501;
                    cmd = @"SELECT PRIVILEGE_CNT FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
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
                    #region E503
                    state = ValidationModel.InvalidState.E503;
                    cmd = @"SELECT COUNT(1) AS CNT FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = {0} AND STATUS = 'Y' AND DEL_FLAG IS NULL";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                    }
                    #endregion
                    #region E504
                    state = ValidationModel.InvalidState.E504;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_REDEEM WHERE PRIVILEGE_ID = {0} AND MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{1}') AND YEAR(REDEEM_DT) = YEAR(GETDATE()) AND MONTH(REDEEM_DT) = MONTH(GETDATE())";
                    cmd = @"SELECT COUNT(1) AS CNT 
                            FROM T_PRIVILEDGES 
                            WHERE ID = {0} AND REED_TIME <=
                            (SELECT COUNT(r.id) AS CNT
                            FROM T_CUSTOMER_REDEEM r
                            WHERE r.PRIVILEGE_ID = {0} 
                            AND r.MEMBERID = (
	                            SELECT MEMBERID 
	                            FROM T_CUSTOMER_TOKEN 
	                            WHERE TOKEN_NO = N'{1}'))";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id, token)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
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

        private ValidationModel CheckValidationOld(string token, int privilege_id, string lang, out bool isUseSpecialQuota)
        {
            ValidationModel value = new ValidationModel();
            isUseSpecialQuota = false;
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E501
                    state = ValidationModel.InvalidState.E501;
                    string cmd = @"DECLARE @DISPLAY_TYPE INT = (SELECT display_type FROM T_PRIVILEDGES WHERE ID = {1})
                    IF @DISPLAY_TYPE = 2 BEGIN
                    SELECT PRIVILEGE_CNT FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')
                    END
                    ELSE BEGIN
                    SELECT 1 AS PRIVILEGE_CNT
                    END";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, privilege_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage =  ValidationModel.GetInvalidMessageNew(state, lang).Result };
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
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang).Result };
                            }
                        }
                    }
                    #endregion

                    #region E503
                    //state = ValidationModel.InvalidState.E503;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = {0} AND STATUS = 'Y' AND DEL_FLAG IS NULL";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    #endregion

                    #region E503, E504
                    //cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_REDEEM WHERE PRIVILEGE_ID = {0} AND MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{1}') AND YEAR(REDEEM_DT) = YEAR(GETDATE()) AND MONTH(REDEEM_DT) = MONTH(GETDATE())";

                    PrivilegeService privService = new PrivilegeService();
                    int SpecialQuota = privService.GetSpecialQuota(token, privilege_id.ToString()).Result;
                    if (SpecialQuota == -1)
                    {
                        MessageService MS = new MessageService();
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = MS.GetMessagetext(300509, lang).Result };
                    }

                    ServiceCheckPrivilegeRedeemModel check = privService.CheckPrivilegeRedeem(null, null, lang, token, privilege_id.ToString()).Result;

                    if (check.data.status == "FULLY_REDEEMED")
                    {
                        state = ValidationModel.InvalidState.E503;
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang).Result };
                    }
                    else if (check.data.status == "REDEEMED")
                    {
                        state = ValidationModel.InvalidState.E504;
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang).Result };
                    }
                    else
                    {
                        isUseSpecialQuota = SpecialQuota > 0;
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

        private ValidationModel CheckValidationNew(string token, int privilege_id, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //#region E501
                    //state = ValidationModel.InvalidState.E501;
                    //string cmd = @"SELECT PRIVILEGE_CNT FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion

                    //#region E502
                    //state = ValidationModel.InvalidState.E502;
                    //cmd = @"SELECT CASE WHEN ISNULL(EXPIRY, '1800-01-01') < GETDATE() THEN 1 ELSE 0 END AS IS_EXPIRE FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //    else
                    //    {
                    //        if (Convert.ToInt32(dt.Rows[0][0]) == 1)
                    //        {
                    //            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //        }
                    //    }
                    //}
                    //#endregion

                    //#region E503
                    //state = ValidationModel.InvalidState.E503;
                    //cmd = @"SELECT COUNT(1) AS CNT 
                    //        FROM T_PRIVILEDGES_CODE pc
                    //            LEFT JOIN T_CUSTOMER_REDEEM cr ON pc.REDEEM_CODE = cr.REDEEM_CODE
                    //         LEFT JOIN T_CUSTOMER_TOKEN ct ON cr.MEMBERID = ct.MEMBERID
                    //        WHERE pc.PRIVILEGE_ID = {0} AND (pc.STATUS = 'Y' OR pc.STATUS = 'T')
                    //            AND ct.TOKEN_NO = N'{1}'";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id, token)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                    //    }
                    //}
                    //#endregion

                    //#region E504
                    //state = ValidationModel.InvalidState.E504;
                    ////cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_REDEEM WHERE PRIVILEGE_ID = {0} AND MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{1}') AND YEAR(REDEEM_DT) = YEAR(GETDATE()) AND MONTH(REDEEM_DT) = MONTH(GETDATE())";
                    //cmd = @"SELECT COUNT(1) AS CNT 
                    //        FROM T_PRIVILEDGES 
                    //        WHERE ID = {0} AND REED_TIME <=
                    //        (SELECT COUNT(r.id) AS CNT
                    //        FROM T_CUSTOMER_REDEEM r LEFT JOIN T_PRIVILEDGES_CODE pc ON r.REDEEM_CODE = pc.REDEEM_CODE
                    //        WHERE r.PRIVILEGE_ID = {0} AND pc.STATUS = 'N'
                    //        AND r.MEMBERID = (
                    //         SELECT MEMBERID 
                    //         FROM T_CUSTOMER_TOKEN 
                    //         WHERE TOKEN_NO = N'{1}'))";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilege_id, token)))
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

        private ValidationModel CheckSystem(string device_type, string app_version, string token)
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
                    cmd = @"SELECT COUNT(1) AS CNT FROM SYSTEM_VERSION WHERE platform ='{0}' AND version > '{1}' AND is_force = 1";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                    }
                    #endregion

                    #region E905
                    state = ValidationModel.InvalidState.E905;
                    cmd = @"SELECT COUNT(1) AS CNT, MAX(store_link) AS store_link, MAX(version) AS last_version FROM SYSTEM_VERSION WHERE platform = N'{0}' AND version > '{1}' AND is_force = 0";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = true, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state), InvalidStoreLink = dt.Rows[0]["store_link"].ToString(), InvalidVersion = dt.Rows[0]["last_version"].ToString() };
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

        private async Task<ValidationModel> CheckSystemNew(string device_type, string app_version, string token, string lang)
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
                    cmd = @"SELECT COUNT(1) AS CNT FROM SYSTEM_VERSION WHERE platform ='{0}' AND version > '{1}' AND is_force = 1";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    #region E905
                    state = ValidationModel.InvalidState.E905;
                    cmd = @"SELECT COUNT(1) AS CNT, MAX(store_link) AS store_link, MAX(version) AS last_version FROM SYSTEM_VERSION WHERE platform = N'{0}' AND version > '{1}' AND is_force = 0";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_type, app_version)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                        {
                            return new ValidationModel { Success = true, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang), InvalidStoreLink = dt.Rows[0]["store_link"].ToString(), InvalidVersion = dt.Rows[0]["last_version"].ToString() };
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