using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
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
        public DataTable GetPrivileges(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'
SELECT		[ID]
			,[TITLE]
			,[DESC]
			,[IMAGE]
            ,[IMAGE_1]
			,[PERIOD]
			,[PERIOD_START]
			,[PERIOD_END]
			,[RED_CONDITION]
			,[RED_PERIOD]
			,[RED_LOCATION]
			,[RED_EXPIRY]
			,[DISPLAY_START]
			,[DISPLAY_END]
			,[CREATE_DT]
			,[CREATE_USER]
			,[UPDATE_DT]
			,[UPDATE_USER]
            ,[PRIVILEDGE_TYPE]
            ,[display_type]
            ,CASE display_type WHEN 1 THEN 'Elite Privilege' WHEN 2 THEN 'Luxury Privilege' WHEN 3 THEN 'Lifestyle Plivilege' END AS display_type_name
            ,[REED_TIME]
            ,CASE PRIVILEDGE_TYPE WHEN 1 THEN 'Shop Verify' WHEN 3 THEN 'Redeem Code' END AS PRIVILEDGE_TYPE_NAME
            ,desc_th
            ,red_condition_th
            ,red_location_th
            ,thk_message
            ,thk_message_th
            ,red_expiry_type
            ,is_show_remaining
            ,[order]
FROM		[T_PRIVILEDGES]
WHERE		DEL_FLAG IS NULL
            AND (ISNULL([TITLE], '') LIKE '%' + @Value + '%'
			OR ISNULL([DESC], '') LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [PERIOD_START], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [PERIOD_END], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DISPLAY_START], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DISPLAY_END], 103) LIKE '%' + @Value + '%'
			OR ISNULL([RED_CONDITION], '') LIKE '%' + @Value + '%'
			OR ISNULL([RED_LOCATION], '') LIKE '%' + @Value + '%')";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(searchValue)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetPrivilegeById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
SELECT		[ID]
			,[TITLE]
			,[DESC]
			,[IMAGE]
            ,[IMAGE_1]
			,[PERIOD]
			,[PERIOD_START]
			,[PERIOD_END]
			,[RED_CONDITION]
			,[RED_PERIOD]
			,[RED_LOCATION]
			,[RED_EXPIRY]
			,[DISPLAY_START]
			,[DISPLAY_END]
			,[CREATE_DT]
			,[CREATE_USER]
			,[UPDATE_DT]
			,[UPDATE_USER]
            ,[PRIVILEDGE_TYPE]
            ,[display_type]
            ,CASE display_type WHEN 1 THEN 'Elite Privilege' WHEN 2 THEN 'Luxury Privilege' WHEN 3 THEN 'Lifestyle Plivilege' END AS display_type_name
            ,[REED_TIME]
            ,CASE PRIVILEDGE_TYPE WHEN 1 THEN 'Shop Verify' WHEN 2 THEN 'Redeem Code' END AS PRIVILEDGE_TYPE_NAME
            ,desc_th
            ,red_condition_th
            ,red_location_th
            ,thk_message
            ,thk_message_th
            ,red_expiry_type
            ,is_show_remaining
            ,[order]
            ,RED_EXPIRY_TIME
            ,redeem_display_type
            ,redeem_display_image
            ,period_type
            ,customer_usage_per_period
            ,period_quota
            ,period_start_in_week
            ,period_start_in_month
            ,return_redeem_when_verify_expire
            ,customer_usage_with_car_total
            ,redeem_display_html
            ,redeem_display_height
            ,redeem_display_width
            ,repeat_redeem_when_verify_expire
FROM		[T_PRIVILEDGES]
WHERE		[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id))))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            row = r;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        public void AddPrivilege(string title, string desc, string period, string periodStart, string periodEnd, string image, string redCondition,
            string redPeriod, string redLocation, string redExpiry, string dispStart, string dispEnd, string privilegeType, string user,
            string maxRedCount, string desc_th, string redCondition_th, string redLocation_th, string thk_message, string thk_message_th,
            string red_expiry_type, string is_show_remaining, string order, string image_1, string display_type, string redeem_display_image,
            string redeem_display_type, string period_type, string customer_usage_per_period, string period_quota, string period_start_in_week,
            string period_start_in_month, string return_redeem_when_verify_expire, string customer_usage_with_car_total, String redeem_display_html, 
            string redeem_display_height, string redeem_display_width, string repeat_redeem_when_verify_expire)
        {
            String sql = "";
            try
            {
                String cmd = @"
DECLARE @TITLE NVARCHAR(250) = N'{0}'
DECLARE @DESC NVARCHAR(MAX) = N'{1}'
DECLARE @PERIOD NVARCHAR(50) = N'{2}'
DECLARE @PERIOD_START NVARCHAR(20) = N'{3}'
DECLARE @PERIOD_END NVARCHAR(20) = N'{4}'
DECLARE @IMAGE NVARCHAR(400) = N'{5}'
DECLARE @RED_CONDITION NVARCHAR(MAX) = N'{6}'
DECLARE @RED_PERIOD NVARCHAR(50) = N'{7}'
DECLARE @RED_LOACATION NVARCHAR(MAX) = N'{8}'
DECLARE @RED_EXPIRY NVARCHAR(20) = N'{9}'
DECLARE @DISPLAY_START NVARCHAR(20) = N'{10}'
DECLARE @DISPLAY_END NVARCHAR(20) = N'{11}'
DECLARE @PRIVILEDGE_TYPE NVARCHAR(20) = N'{12}'
DECLARE @USER NVARCHAR(50) = N'{13}'
DECLARE @MAX_RED_CNT NVARCHAR(20) = N'{14}'
DECLARE @DESC_TH NVARCHAR(MAX) = N'{15}'
DECLARE @RED_CONDITION_TH NVARCHAR(MAX) = N'{16}'
DECLARE @RED_LOACATION_TH NVARCHAR(MAX) = N'{17}'
DECLARE @THK_MESSAGE NVARCHAR(MAX) = N'{18}'
DECLARE @THK_MESSAGE_TH NVARCHAR(MAX) = N'{19}'
DECLARE @RED_EXPIRY_TYPE INT = N'{20}'
DECLARE @IS_SHOW_REMAINING NVARCHAR(20) = N'{21}'
DECLARE @ORDER INT = N'{22}'
DECLARE @IMAGE_1 NVARCHAR(400) = N'{23}'
DECLARE @DISPLAY_TYPE INT = N'{24}'
DECLARE @RED_EXPIRY_TIME NVARCHAR(7) = ''
DECLARE @redeem_display_type INT = N'{25}'
DECLARE @redeem_display_image NVARCHAR(300) = N'{26}'

DECLARE @period_type int = N'{27}'
DECLARE @customer_usage_per_period int = N'{28}'
DECLARE @period_quota int = N'{29}'
DECLARE @period_start_in_week int = N'{30}'
DECLARE @period_start_in_month int = N'{31}'
DECLARE @return_redeem_when_verify_expire bit = N'{32}'
DECLARE @customer_usage_with_car_total bit = N'{33}'
DECLARE @redeem_display_html nvarchar(MAX) = N'{34}'
DECLARE @redeem_display_height int = N'{35}'
DECLARE @redeem_display_width int = N'{36}'
DECLARE @repeat_redeem_when_verify_expire int = N'{37}'

IF @RED_EXPIRY_TYPE = '1' or @PRIVILEDGE_TYPE = '3' BEGIN
   SET @RED_EXPIRY_TIME = '00:00'
END
ELSE BEGIN
   SET @RED_EXPIRY_TIME = @RED_EXPIRY
   SET @RED_EXPIRY = '0'
END

INSERT INTO [T_PRIVILEDGES] ([TITLE],[DESC],[PERIOD],[PERIOD_START],[PERIOD_END],[IMAGE],[RED_CONDITION],[RED_PERIOD],[RED_LOCATION],[RED_EXPIRY],
[DISPLAY_START],[DISPLAY_END],[PRIVILEDGE_TYPE],[CREATE_DT],[CREATE_USER],[REED_TIME], desc_th, red_condition_th, red_location_th, thk_message, 
thk_message_th, red_expiry_type, is_show_remaining, is_show_stock, [order], IMAGE_1, display_type, RED_EXPIRY_TIME,redeem_display_type,redeem_display_image,
period_type, customer_usage_per_period, period_quota, period_start_in_week, period_start_in_month, return_redeem_when_verify_expire, customer_usage_with_car_total,
redeem_display_html, redeem_display_height, redeem_display_width, repeat_redeem_when_verify_expire)
VALUES (CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		CASE LEN(@DESC) WHEN 0 THEN NULL ELSE @DESC END,
		CASE LEN(@PERIOD) WHEN 0 THEN NULL ELSE @PERIOD END,
		CASE LEN(@PERIOD_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @PERIOD_START, 103) END,
		CASE LEN(@PERIOD_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @PERIOD_END, 103) END,
		CASE LEN(@IMAGE) WHEN 0 THEN NULL ELSE @IMAGE END,
		CASE LEN(@RED_CONDITION) WHEN 0 THEN NULL ELSE @RED_CONDITION END,
		CASE LEN(@RED_PERIOD) WHEN 0 THEN NULL ELSE @RED_PERIOD END,
		CASE LEN(@RED_LOACATION) WHEN 0 THEN NULL ELSE @RED_LOACATION END,
		CASE LEN(@RED_EXPIRY) WHEN 0 THEN NULL ELSE @RED_EXPIRY END,
		CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
        CASE LEN(@PRIVILEDGE_TYPE) WHEN 0 THEN NULL ELSE @PRIVILEDGE_TYPE END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER,
        CASE LEN(@MAX_RED_CNT) WHEN 0 THEN NULL ELSE @MAX_RED_CNT END,
        CASE LEN(@DESC_TH) WHEN 0 THEN NULL ELSE @DESC_TH END,
        CASE LEN(@RED_CONDITION_TH) WHEN 0 THEN NULL ELSE @RED_CONDITION_TH END,
        CASE LEN(@RED_LOACATION_TH) WHEN 0 THEN NULL ELSE @RED_LOACATION_TH END,
        CASE LEN(@THK_MESSAGE) WHEN 0 THEN NULL ELSE @THK_MESSAGE END,
        CASE LEN(@THK_MESSAGE_TH) WHEN 0 THEN NULL ELSE @THK_MESSAGE_TH END,
        CASE LEN(@RED_EXPIRY_TYPE) WHEN 0 THEN NULL ELSE @RED_EXPIRY_TYPE END,
        CASE LEN(@IS_SHOW_REMAINING) WHEN 0 THEN 0 ELSE @IS_SHOW_REMAINING END,
        CASE LEN(@IS_SHOW_REMAINING) WHEN 0 THEN 0 ELSE @IS_SHOW_REMAINING END,
        CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
        CASE LEN(@IMAGE_1) WHEN 0 THEN NULL ELSE @IMAGE_1 END,
        CASE LEN(@DISPLAY_TYPE) WHEN 0 THEN NULL ELSE @DISPLAY_TYPE END,
        CASE LEN(@RED_EXPIRY_TIME) WHEN 0 THEN NULL ELSE @RED_EXPIRY_TIME END,
        CASE LEN(@redeem_display_type) WHEN 0 THEN 1 ELSE @redeem_display_type END,
        CASE LEN(@redeem_display_image) WHEN 0 THEN NULL ELSE @redeem_display_image END,

        CASE LEN(@period_type) WHEN 0 THEN 1 ELSE @period_type END,
        CASE LEN(@customer_usage_per_period) WHEN 0 THEN 0 ELSE @customer_usage_per_period END,
        CASE LEN(@period_quota) WHEN 0 THEN 0 ELSE @period_quota END,
        CASE LEN(@period_start_in_week) WHEN 0 THEN 1 ELSE @period_start_in_week END,
        CASE LEN(@period_start_in_month) WHEN 0 THEN 1 ELSE @period_start_in_month END,
        CASE LEN(@return_redeem_when_verify_expire) WHEN 0 THEN 0 ELSE @return_redeem_when_verify_expire END,
        CASE LEN(@customer_usage_with_car_total) WHEN 0 THEN 0 ELSE @customer_usage_with_car_total END,
        CASE LEN(@redeem_display_html) WHEN 0 THEN NULL ELSE @redeem_display_html END,
        CASE LEN(@redeem_display_height) WHEN 0 THEN 0 ELSE @redeem_display_height END,
        CASE LEN(@redeem_display_width) WHEN 0 THEN 0 ELSE @redeem_display_width END,
        CASE LEN(@repeat_redeem_when_verify_expire) WHEN 0 THEN 0 ELSE @repeat_redeem_when_verify_expire END)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    sql = String.Format(cmd,
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(desc),
                        WebUtility.GetSQLTextValue(period),
                        WebUtility.GetSQLTextValue(periodStart),
                        WebUtility.GetSQLTextValue(periodEnd),
                        WebUtility.GetSQLTextValue(image),
                        WebUtility.GetSQLTextValue(redCondition),
                        WebUtility.GetSQLTextValue(redPeriod),
                        WebUtility.GetSQLTextValue(redLocation),
                        WebUtility.GetSQLTextValue(redExpiry),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(privilegeType),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(maxRedCount),
                        WebUtility.GetSQLTextValue(desc_th),
                        WebUtility.GetSQLTextValue(redCondition_th),
                        WebUtility.GetSQLTextValue(redLocation_th),
                        WebUtility.GetSQLTextValue(thk_message),
                        WebUtility.GetSQLTextValue(thk_message_th),
                        WebUtility.GetSQLTextValue(red_expiry_type),
                        WebUtility.GetSQLTextValue(is_show_remaining),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(image_1),
                        WebUtility.GetSQLTextValue(display_type),
                        WebUtility.GetSQLTextValue(redeem_display_type),
                        WebUtility.GetSQLTextValue(redeem_display_image),
                        WebUtility.GetSQLTextValue(period_type),
                        WebUtility.GetSQLTextValue(customer_usage_per_period),
                        WebUtility.GetSQLTextValue(period_quota),
                        WebUtility.GetSQLTextValue(period_start_in_week),
                        WebUtility.GetSQLTextValue(period_start_in_month),
                        WebUtility.GetSQLTextValue(return_redeem_when_verify_expire),
                        WebUtility.GetSQLTextValue(customer_usage_with_car_total),
                        WebUtility.GetSQLTextValue(redeem_display_html),
                        WebUtility.GetSQLTextValue(redeem_display_height),
                        WebUtility.GetSQLTextValue(redeem_display_width),
                        WebUtility.GetSQLTextValue(repeat_redeem_when_verify_expire));
                    db.ExecuteNonQueryFromCommandText(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddPrivilegeNew(string title, string desc, string period, string periodStart, string periodEnd, string image, string redCondition,
            string redPeriod, string redLocation, string redExpiry, string dispStart, string dispEnd, string privilegeType, string user,
            string maxRedCount, string displayType)
        {
            try
            {
                string cmd = @"
DECLARE @TITLE NVARCHAR(250) = N'{0}'
DECLARE @DESC NVARCHAR(MAX) = N'{1}'
DECLARE @PERIOD NVARCHAR(50) = N'{2}'
DECLARE @PERIOD_START NVARCHAR(20) = N'{3}'
DECLARE @PERIOD_END NVARCHAR(20) = N'{4}'
DECLARE @IMAGE NVARCHAR(50) = N'{5}'
DECLARE @RED_CONDITION NVARCHAR(1000) = N'{6}'
DECLARE @RED_PERIOD NVARCHAR(50) = N'{7}'
DECLARE @RED_LOACATION NVARCHAR(1000) = N'{8}'
DECLARE @RED_EXPIRY NVARCHAR(20) = N'{9}'
DECLARE @DISPLAY_START NVARCHAR(20) = N'{10}'
DECLARE @DISPLAY_END NVARCHAR(20) = N'{11}'
DECLARE @PRIVILEDGE_TYPE NVARCHAR(20) = N'{12}'
DECLARE @USER NVARCHAR(50) = N'{13}'
DECLARE @MAX_RED_CNT NVARCHAR(20) = N'{14}'
DECLARE @display_type NVARCHAR(20) = N'{15}'

INSERT INTO [T_PRIVILEDGES] ([TITLE],[DESC],[PERIOD],[PERIOD_START],[PERIOD_END],[IMAGE],[RED_CONDITION],[RED_PERIOD],[RED_LOCATION],[RED_EXPIRY],
[DISPLAY_START],[DISPLAY_END],[PRIVILEDGE_TYPE],[display_type],[CREATE_DT],[CREATE_USER],[REED_TIME])
VALUES (CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		CASE LEN(@DESC) WHEN 0 THEN NULL ELSE @DESC END,
		CASE LEN(@PERIOD) WHEN 0 THEN NULL ELSE @PERIOD END,
		CASE LEN(@PERIOD_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @PERIOD_START, 103) END,
		CASE LEN(@PERIOD_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @PERIOD_END + ' 23:59:59', 103) END,
		CASE LEN(@IMAGE) WHEN 0 THEN NULL ELSE @IMAGE END,
		CASE LEN(@RED_CONDITION) WHEN 0 THEN NULL ELSE @RED_CONDITION END,
		CASE LEN(@RED_PERIOD) WHEN 0 THEN NULL ELSE @RED_PERIOD END,
		CASE LEN(@RED_LOACATION) WHEN 0 THEN NULL ELSE @RED_LOACATION END,
		CASE LEN(@RED_EXPIRY) WHEN 0 THEN NULL ELSE @RED_EXPIRY END,
		CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START + ' 00:00:00', 103) END,
		CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END + ' 23:59:59', 103) END,
        CASE LEN(@PRIVILEDGE_TYPE) WHEN 0 THEN NULL ELSE @PRIVILEDGE_TYPE END,
        CASE LEN(@display_type) WHEN 0 THEN NULL ELSE @display_type END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER,
        CASE LEN(@MAX_RED_CNT) WHEN 0 THEN NULL ELSE @MAX_RED_CNT END)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(desc),
                        WebUtility.GetSQLTextValue(period),
                        WebUtility.GetSQLTextValue(periodStart),
                        WebUtility.GetSQLTextValue(periodEnd),
                        WebUtility.GetSQLTextValue(image),
                        WebUtility.GetSQLTextValue(redCondition),
                        WebUtility.GetSQLTextValue(redPeriod),
                        WebUtility.GetSQLTextValue(redLocation),
                        WebUtility.GetSQLTextValue(redExpiry),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(privilegeType),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(maxRedCount),
                        WebUtility.GetSQLTextValue(displayType)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePrivilege(string title, string desc, string period, string periodStart, string periodEnd, string image, string redCondition,
            string redPeriod, string redLocation, string redExpiry, string dispStart, string dispEnd, string privilegeType, string user, string id,
            string maxRedCount, string desc_th, string redCondition_th, string redLocation_th, string thk_message, string thk_message_th,
            string red_expiry_type, string is_show_remaining, string order, string image_1, string display_type, string redeem_display_image, 
            string redeem_display_type, string period_type, string customer_usage_per_period, string period_quota, string period_start_in_week,
            string period_start_in_month, string return_redeem_when_verify_expire, string customer_usage_with_car_total, String redeem_display_html,
            string redeem_display_height, string redeem_display_width, string repeat_redeem_when_verify_expire)
        {
            try
            {
                String cmd = @"
DECLARE @TITLE NVARCHAR(250) = N'{0}'
DECLARE @DESC NVARCHAR(MAX) = N'{1}'
DECLARE @PERIOD NVARCHAR(50) = N'{2}'
DECLARE @PERIOD_START NVARCHAR(20) = N'{3}'
DECLARE @PERIOD_END NVARCHAR(20) = N'{4}'
DECLARE @IMAGE NVARCHAR(400) = N'{5}'
DECLARE @RED_CONDITION NVARCHAR(MAX) = N'{6}'
DECLARE @RED_PERIOD NVARCHAR(50) = N'{7}'
DECLARE @RED_LOACATION NVARCHAR(MAX) = N'{8}'
DECLARE @RED_EXPIRY NVARCHAR(20) = N'{9}'
DECLARE @DISPLAY_START NVARCHAR(20) = N'{10}'
DECLARE @DISPLAY_END NVARCHAR(20) = N'{11}'
DECLARE @PRIVILEDGE_TYPE NVARCHAR(20) = N'{12}'
DECLARE @USER NVARCHAR(50) = N'{13}'
DECLARE @ID NVARCHAR(20) = N'{14}'
DECLARE @MAX_RED_CNT NVARCHAR(20) = N'{15}'
DECLARE @DESC_TH NVARCHAR(MAX) = N'{16}'
DECLARE @RED_CONDITION_TH NVARCHAR(MAX) = N'{17}'
DECLARE @RED_LOACATION_TH NVARCHAR(MAX) = N'{18}'
DECLARE @THK_MESSAGE NVARCHAR(MAX) = N'{19}'
DECLARE @THK_MESSAGE_TH NVARCHAR(MAX) = N'{20}'
DECLARE @RED_EXPIRY_TYPE INT = N'{21}'
DECLARE @IS_SHOW_REMAINING NVARCHAR(20) = N'{22}'
DECLARE @ORDER  INT = N'{23}'
DECLARE @IMAGE_1 NVARCHAR(400) = N'{24}'
DECLARE @DISPLAY_TYPE INT = N'{25}'
DECLARE @redeem_display_type NVARCHAR(300) = N'{26}'
DECLARE @redeem_display_image NVARCHAR(300) = N'{27}'

DECLARE @RED_EXPIRY_TIME NVARCHAR(7) = ''

DECLARE @period_type int = N'{28}'
DECLARE @customer_usage_per_period int = N'{29}'
DECLARE @period_quota int = N'{30}'
DECLARE @period_start_in_week int = N'{31}'
DECLARE @period_start_in_month int = N'{32}'
DECLARE @return_redeem_when_verify_expire bit = N'{33}'
DECLARE @customer_usage_with_car_total bit = N'{34}'
DECLARE @redeem_display_html nvarchar(MAX) = N'{35}'
DECLARE @redeem_display_height int = N'{36}'
DECLARE @redeem_display_width int = N'{37}'
DECLARE @repeat_redeem_when_verify_expire int = N'{38}'

IF @RED_EXPIRY_TYPE = '1' or @PRIVILEDGE_TYPE = '3' BEGIN
   SET @RED_EXPIRY_TIME = '00:00'
END
ELSE BEGIN
   SET @RED_EXPIRY_TIME = @RED_EXPIRY
   SET @RED_EXPIRY = '0'
END

UPDATE [T_PRIVILEDGES]
SET		TITLE = CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		[DESC] = CASE LEN(@DESC) WHEN 0 THEN NULL ELSE @DESC END,
		PERIOD = CASE LEN(@PERIOD) WHEN 0 THEN NULL ELSE @PERIOD END,
		PERIOD_START = CASE LEN(@PERIOD_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @PERIOD_START, 103) END,
		PERIOD_END = CASE LEN(@PERIOD_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @PERIOD_END, 103) END,
		[IMAGE] = CASE LEN(@IMAGE) WHEN 0 THEN NULL ELSE @IMAGE END,
		RED_CONDITION = CASE LEN(@RED_CONDITION) WHEN 0 THEN NULL ELSE @RED_CONDITION END,
		RED_PERIOD = CASE LEN(@RED_PERIOD) WHEN 0 THEN NULL ELSE @RED_PERIOD END,
		RED_LOCATION = CASE LEN(@RED_LOACATION) WHEN 0 THEN NULL ELSE @RED_LOACATION END,
		RED_EXPIRY = CASE LEN(@RED_EXPIRY) WHEN 0 THEN NULL ELSE @RED_EXPIRY END,
		DISPLAY_START = CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		DISPLAY_END = CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
        PRIVILEDGE_TYPE = CASE LEN(@PRIVILEDGE_TYPE) WHEN 0 THEN NULL ELSE @PRIVILEDGE_TYPE END,
        display_type = CASE LEN(@DISPLAY_TYPE) WHEN 0 THEN NULL ELSE @DISPLAY_TYPE END,
		UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
		UPDATE_USER = @USER,
        REED_TIME = CASE LEN(@MAX_RED_CNT) WHEN 0 THEN NULL ELSE @MAX_RED_CNT END,
        desc_th = CASE LEN(@DESC_TH) WHEN 0 THEN NULL ELSE @DESC_TH END,
        red_condition_th = CASE LEN(@RED_CONDITION_TH) WHEN 0 THEN NULL ELSE @RED_CONDITION_TH END,
        red_location_th = CASE LEN(@RED_LOACATION_TH) WHEN 0 THEN NULL ELSE @RED_LOACATION_TH END,
        thk_message = CASE LEN(@THK_MESSAGE) WHEN 0 THEN NULL ELSE @THK_MESSAGE END,
        thk_message_th = CASE LEN(@THK_MESSAGE_TH) WHEN 0 THEN NULL ELSE @THK_MESSAGE_TH END,
        red_expiry_type = CASE LEN(@RED_EXPIRY_TYPE) WHEN 0 THEN NULL ELSE @RED_EXPIRY_TYPE END,
        is_show_remaining = CASE LEN(@IS_SHOW_REMAINING) WHEN 0 THEN 0 ELSE @IS_SHOW_REMAINING END,
        is_show_stock = CASE LEN(@IS_SHOW_REMAINING) WHEN 0 THEN 0 ELSE @IS_SHOW_REMAINING END,
        [order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
        [IMAGE_1] = CASE LEN(@IMAGE_1) WHEN 0 THEN NULL ELSE @IMAGE_1 END,
        RED_EXPIRY_TIME = CASE LEN(@RED_EXPIRY_TIME) WHEN 0 THEN NULL ELSE @RED_EXPIRY_TIME END,
        redeem_display_type = CASE LEN(@redeem_display_type) WHEN 0 THEN 1 ELSE @redeem_display_type END,
        redeem_display_image = CASE LEN(@redeem_display_image) WHEN 0 THEN NULL ELSE @redeem_display_image END,

        period_type = CASE LEN(@period_type) WHEN 0 THEN 1 ELSE @period_type END,
        customer_usage_per_period = CASE LEN(@customer_usage_per_period) WHEN 0 THEN 0 ELSE @customer_usage_per_period END,
        period_quota = CASE LEN(@period_quota) WHEN 0 THEN 0 ELSE @period_quota END,
        period_start_in_week = CASE LEN(@period_start_in_week) WHEN 0 THEN 1 ELSE @period_start_in_week END,
        period_start_in_month = CASE LEN(@period_start_in_month) WHEN 0 THEN 1 ELSE @period_start_in_month END,
        return_redeem_when_verify_expire = CASE LEN(@return_redeem_when_verify_expire) WHEN 0 THEN 0 ELSE @return_redeem_when_verify_expire END,
        customer_usage_with_car_total = CASE LEN(@customer_usage_with_car_total) WHEN 0 THEN 0 ELSE @customer_usage_with_car_total END,
        redeem_display_html = CASE LEN(@redeem_display_html) WHEN 0 THEN NULL ELSE @redeem_display_html END,
        redeem_display_height = CASE LEN(@redeem_display_height) WHEN 0 THEN 0 ELSE @redeem_display_height END,
        redeem_display_width = CASE LEN(@redeem_display_width) WHEN 0 THEN 0 ELSE @redeem_display_width END,
        repeat_redeem_when_verify_expire = CASE LEN(@repeat_redeem_when_verify_expire) WHEN 0 THEN 0 ELSE @repeat_redeem_when_verify_expire END

WHERE	ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(String.Format(cmd,
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(desc),
                        WebUtility.GetSQLTextValue(period),
                        WebUtility.GetSQLTextValue(periodStart),
                        WebUtility.GetSQLTextValue(periodEnd),
                        WebUtility.GetSQLTextValue(image),
                        WebUtility.GetSQLTextValue(redCondition),
                        WebUtility.GetSQLTextValue(redPeriod),
                        WebUtility.GetSQLTextValue(redLocation),
                        WebUtility.GetSQLTextValue(redExpiry),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(privilegeType),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(maxRedCount),
                        WebUtility.GetSQLTextValue(desc_th),
                        WebUtility.GetSQLTextValue(redCondition_th),
                        WebUtility.GetSQLTextValue(redLocation_th),
                        WebUtility.GetSQLTextValue(thk_message),
                        WebUtility.GetSQLTextValue(thk_message_th),
                        WebUtility.GetSQLTextValue(red_expiry_type),
                        WebUtility.GetSQLTextValue(is_show_remaining),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(image_1),
                        WebUtility.GetSQLTextValue(display_type),
                        WebUtility.GetSQLTextValue(redeem_display_type),
                        WebUtility.GetSQLTextValue(redeem_display_image),
                        WebUtility.GetSQLTextValue(period_type),
                        WebUtility.GetSQLTextValue(customer_usage_per_period),
                        WebUtility.GetSQLTextValue(period_quota),
                        WebUtility.GetSQLTextValue(period_start_in_week),
                        WebUtility.GetSQLTextValue(period_start_in_month),
                        WebUtility.GetSQLTextValue(return_redeem_when_verify_expire),
                        WebUtility.GetSQLTextValue(customer_usage_with_car_total),
                        WebUtility.GetSQLTextValue(redeem_display_html),
                        WebUtility.GetSQLTextValue(redeem_display_height),
                        WebUtility.GetSQLTextValue(redeem_display_width),
                        WebUtility.GetSQLTextValue(repeat_redeem_when_verify_expire)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePrivilegeNew(string title, string desc, string period, string periodStart, string periodEnd, string image, string redCondition, string redPeriod, string redLocation, string redExpiry, string dispStart, string dispEnd, string privilegeType, string user, string id, string maxRedCount, string displayType)
        {
            try
            {
                string cmd = @"
DECLARE @TITLE NVARCHAR(250) = N'{0}'
DECLARE @DESC NVARCHAR(MAX) = N'{1}'
DECLARE @PERIOD NVARCHAR(50) = N'{2}'
DECLARE @PERIOD_START NVARCHAR(20) = N'{3}'
DECLARE @PERIOD_END NVARCHAR(20) = N'{4}'
DECLARE @IMAGE NVARCHAR(50) = N'{5}'
DECLARE @RED_CONDITION NVARCHAR(1000) = N'{6}'
DECLARE @RED_PERIOD NVARCHAR(50) = N'{7}'
DECLARE @RED_LOACATION NVARCHAR(1000) = N'{8}'
DECLARE @RED_EXPIRY NVARCHAR(20) = N'{9}'
DECLARE @DISPLAY_START NVARCHAR(20) = N'{10}'
DECLARE @DISPLAY_END NVARCHAR(20) = N'{11}'
DECLARE @PRIVILEDGE_TYPE NVARCHAR(20) = N'{12}'
DECLARE @USER NVARCHAR(50) = N'{13}'
DECLARE @ID NVARCHAR(20) = N'{14}'
DECLARE @MAX_RED_CNT NVARCHAR(20) = N'{15}'
DECLARE @display_type NVARCHAR(20) = N'{16}'

UPDATE [T_PRIVILEDGES]
SET		TITLE = CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		[DESC] = CASE LEN(@DESC) WHEN 0 THEN NULL ELSE @DESC END,
		PERIOD = CASE LEN(@PERIOD) WHEN 0 THEN NULL ELSE @PERIOD END,
		PERIOD_START = CASE LEN(@PERIOD_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @PERIOD_START, 103) END,
		PERIOD_END = CASE LEN(@PERIOD_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @PERIOD_END + ' 23:59:59', 103) END,
		[IMAGE] = CASE LEN(@IMAGE) WHEN 0 THEN NULL ELSE @IMAGE END,
		RED_CONDITION = CASE LEN(@RED_CONDITION) WHEN 0 THEN NULL ELSE @RED_CONDITION END,
		RED_PERIOD = CASE LEN(@RED_PERIOD) WHEN 0 THEN NULL ELSE @RED_PERIOD END,
		RED_LOCATION = CASE LEN(@RED_LOACATION) WHEN 0 THEN NULL ELSE @RED_LOACATION END,
		RED_EXPIRY = CASE LEN(@RED_EXPIRY) WHEN 0 THEN NULL ELSE @RED_EXPIRY END,
		DISPLAY_START = CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START + ' 00:00:00', 103) END,
		DISPLAY_END = CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END + ' 23:59:59', 103) END,
        PRIVILEDGE_TYPE = CASE LEN(@PRIVILEDGE_TYPE) WHEN 0 THEN NULL ELSE @PRIVILEDGE_TYPE END,
        display_type = CASE LEN(@display_type) WHEN 0 THEN NULL ELSE @display_type END,
		UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
		UPDATE_USER = @USER,
        REED_TIME = CASE LEN(@MAX_RED_CNT) WHEN 0 THEN NULL ELSE @MAX_RED_CNT END
WHERE	ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(desc),
                        WebUtility.GetSQLTextValue(period),
                        WebUtility.GetSQLTextValue(periodStart),
                        WebUtility.GetSQLTextValue(periodEnd),
                        WebUtility.GetSQLTextValue(image),
                        WebUtility.GetSQLTextValue(redCondition),
                        WebUtility.GetSQLTextValue(redPeriod),
                        WebUtility.GetSQLTextValue(redLocation),
                        WebUtility.GetSQLTextValue(redExpiry),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(privilegeType),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(maxRedCount),
                        WebUtility.GetSQLTextValue(displayType)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePrivilege(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	T_PRIVILEDGES
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	ID = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}