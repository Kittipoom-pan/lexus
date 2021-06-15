using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class EventService
    {
        private string conn;
        public EventService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetEvents(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT		[ID]
			,[TITLE]
			,[DATE]
			,[DESC]
			,[IMAGES1]
			,[IMAGES2]
			,[IMAGES3]
			,[IMAGES4]
			,[IMAGES5]
			,[CONDITION]
			,[REG_PERIOD]
			,[REG_PERIOD_START]
			,[REG_PERIOD_END]
			,[EVENT_START]
			,[EVENT_END]
			,[LIMIT_GUEST]
			,[DISPLAY_START]
			,[DISPLAY_END]
			,[CREATE_DT]
			,[CREATE_USER]
			,[UPDATE_DT]
			,[UPDATE_USER]
            ,[desc_th]
            ,[condition_th]
            ,[reg_period_th]
FROM		[T_EVENTS]
WHERE		DEL_FLAG IS NULL
            AND (ISNULL([TITLE], '') LIKE '%' + @Value + '%'
			OR ISNULL([DESC], '') LIKE '%' + @Value + '%'
			OR ISNULL([CONDITION], '') LIKE '%' + @Value + '%'
			OR ISNULL([REG_PERIOD], '') LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DATE], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DISPLAY_START], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DISPLAY_END], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [REG_PERIOD_START], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [REG_PERIOD_END], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [EVENT_START], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [EVENT_END], 103) LIKE '%' + @Value + '%')";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(searchValue));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetEventById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[ID]
			,[TITLE]
			,[DATE]
			,[DESC]
			,[IMAGES1]
			,[IMAGES2]
			,[IMAGES3]
			,[IMAGES4]
			,[IMAGES5]
            ,IMAGES1_1
            ,IMAGES2_1
            ,IMAGES3_1
            ,IMAGES4_1
            ,IMAGES5_1
			,[CONDITION]
			,[REG_PERIOD]
			,[REG_PERIOD_START]
			,[REG_PERIOD_END]
			,[EVENT_START]
			,[EVENT_END]
			,[LIMIT_GUEST]
			,[DISPLAY_START]
			,[DISPLAY_END]
			,[CREATE_DT]
			,[CREATE_USER]
			,[UPDATE_DT]
			,[UPDATE_USER]
            ,[desc_th]
            ,[condition_th]
            ,[reg_period_th]
            ,[without_guest]
            ,[car_owner_only]
            ,[thankyou_message_en]
            ,[thankyou_message_th]
            ,[event_group_ids]
            ,one_member_per_event
            ,event_type 
            ,period_type
            ,preferred_model_ids
            ,period_car_age_start
            ,period_car_age_end
FROM		[T_EVENTS]
WHERE		[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
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
        public void AddEvent(string title, string date, string desc, string dispStart, string dispEnd, string condition, string regPeriod, string regPeriodStart, string regPeriodEnd, string eventStart, string eventEnd, string limitGuest, string user, string descTH, string conditionTH, string regPeriodTH, string without_guest, string car_owner_only, string thank_you_message_en, string thank_you_message_th,string one_member_per_event, List<UploadImageModel> banner, List<UploadImageModel> bannerDetail, string event_group_ids, string event_type, string preferredmodelid,string car_age_start,string car_age_end)
        {
            try
            {
                List<string> cmds = new List<string>();
                string cmd = @"
DECLARE @ID  INT
DECLARE @TITLE  NVARCHAR(250) = N'{0}'
DECLARE @DATE  NVARCHAR(20) = N'{1}'
DECLARE @DESC  NVARCHAR(MAX) = N'{2}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{3}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{4}'
DECLARE @CONDITION  NVARCHAR(MAX) = N'{5}'
DECLARE @REG_PERIOD  NVARCHAR(50) = N'{6}'
DECLARE @REG_PERIOD_START  NVARCHAR(20) = N'{7}'
DECLARE @REG_PERIOD_END  NVARCHAR(20) = N'{8}'
DECLARE @EVENT_START  NVARCHAR(20) = N'{9}'
DECLARE @EVENT_END  NVARCHAR(20) = N'{10}'
DECLARE @LIMIT_GUEST  INT = N'{11}'
DECLARE @USER  NVARCHAR(50) = N'{12}'
DECLARE @DESCTH  NVARCHAR(MAX) = N'{13}'
DECLARE @CONDITIONTH  NVARCHAR(MAX) = N'{14}'
DECLARE @REG_PERIODTH  NVARCHAR(50) = N'{15}'
DECLARE @WITHOUTGUEST INT = N'{16}'
DECLARE @CAROWNERONLY INT = N'{17}'
DECLARE @THANKYOU_MESSAGE_EN NVARCHAR(MAX) = N'{18}'
DECLARE @THANKYOU_MESSAGE_TH NVARCHAR(MAX) = N'{19}'
DECLARE @event_group_ids NVARCHAR(100) = N'{20}'
DECLARE @one_member_per_event TINYINT = N'{21}'
DECLARE @event_type NVARCHAR(50) = N'{22}'
DECLARE @preferred_model_ids NVARCHAR(MAX) = N'{23}'
DECLARE @period_car_age_start INT = N'{24}'
DECLARE @period_car_age_end INT = N'{25}'

INSERT INTO [T_EVENTS] ([TITLE],[DATE],[DESC],[DISPLAY_START],[DISPLAY_END],[CONDITION],[REG_PERIOD],[REG_PERIOD_START],[REG_PERIOD_END],[EVENT_START],[EVENT_END],[LIMIT_GUEST],[CREATE_DT],[CREATE_USER],desc_th,condition_th,reg_period_th,without_guest,car_owner_only,THANKYOU_MESSAGE_EN,THANKYOU_MESSAGE_TH,event_group_ids,one_member_per_event,event_type,preferred_model_ids, period_car_age_start, period_car_age_end)
VALUES (CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		CASE LEN(@DATE) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DATE, 103) END,
		CASE LEN(@DESC) WHEN 0 THEN NULL ELSE @DESC END,	
		CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
		CASE LEN(@CONDITION) WHEN 0 THEN NULL ELSE @CONDITION END,
        CASE LEN(@REG_PERIOD) WHEN 0 THEN NULL ELSE @REG_PERIOD END,
		CASE LEN(@REG_PERIOD_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @REG_PERIOD_START, 103) END,
		CASE LEN(@REG_PERIOD_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @REG_PERIOD_END, 103) END,
		CASE LEN(@EVENT_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @EVENT_START, 103) END,
		CASE LEN(@EVENT_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @EVENT_END, 103) END,
		CASE LEN(@LIMIT_GUEST) WHEN 0 THEN NULL ELSE @LIMIT_GUEST END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER,
        CASE LEN(@DESCTH) WHEN 0 THEN NULL ELSE @DESCTH END,
        CASE LEN(@CONDITIONTH) WHEN 0 THEN NULL ELSE @CONDITIONTH END,
        CASE LEN(@REG_PERIODTH) WHEN 0 THEN NULL ELSE @REG_PERIODTH END,
        CASE LEN(@WITHOUTGUEST) WHEN 0 THEN NULL ELSE @WITHOUTGUEST END,
        CASE LEN(@CAROWNERONLY) WHEN 0 THEN NULL ELSE @CAROWNERONLY END,
        CASE LEN(@THANKYOU_MESSAGE_EN) WHEN 0 THEN NULL ELSE @THANKYOU_MESSAGE_EN END,
        CASE LEN(@THANKYOU_MESSAGE_TH) WHEN 0 THEN NULL ELSE @THANKYOU_MESSAGE_TH END,
        CASE LEN(@event_group_ids) WHEN 0 THEN NULL ELSE @event_group_ids END,
        CASE LEN(@one_member_per_event) WHEN 0 THEN NULL ELSE @one_member_per_event END,
        CASE LEN(@event_type) WHEN 0 THEN NULL ELSE @event_type END,
        CASE LEN(@preferred_model_ids) WHEN 0 THEN NULL ELSE @preferred_model_ids END,
        CASE LEN(@period_car_age_start) WHEN 0 THEN NULL ELSE @period_car_age_start END,
        CASE LEN(@period_car_age_end) WHEN 0 THEN NULL ELSE @period_car_age_end END
)
SET @ID = SCOPE_IDENTITY()
";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(title.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(date),
                        WebUtility.GetSQLTextValue(desc.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(condition.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(regPeriod.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(regPeriodStart),
                        WebUtility.GetSQLTextValue(regPeriodEnd),
                        WebUtility.GetSQLTextValue(eventStart),
                        WebUtility.GetSQLTextValue(eventEnd),
                        WebUtility.GetSQLTextValue(limitGuest),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(descTH.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(conditionTH.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(regPeriodTH.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(without_guest),
                        WebUtility.GetSQLTextValue(car_owner_only),
                        WebUtility.GetSQLTextValue(thank_you_message_en),
                        WebUtility.GetSQLTextValue(thank_you_message_th),
                        WebUtility.GetSQLTextValue(event_group_ids),
                        WebUtility.GetSQLTextValue(one_member_per_event), 
                        WebUtility.GetSQLTextValue(event_type),
                        WebUtility.GetSQLTextValue(preferredmodelid),
                        WebUtility.GetSQLTextValue(car_age_start),
                        WebUtility.GetSQLTextValue(car_age_end)
                        );

                    List<UploadImageModel> imgs = new List<UploadImageModel>();
                    imgs.AddRange(banner);
                    imgs.AddRange(bannerDetail);

                    foreach (UploadImageModel img in imgs.Where(c => !string.IsNullOrEmpty(c.Status)))
                    {
                        if (img.Status.Equals("NEW"))
                        {
                            cmd += string.Format(@"
INSERT INTO [upload_Image]
           ([Parent_Id]
           ,[Type]
           ,[Page]
           ,[File_Name]
           ,[File_Path]
           ,[Original_File_Name]
           ,[Created_Date]
           ,[Created_User])
     VALUES
           (@ID
           ,N'{0}'
           ,N'EVENTS'
           ,N'{1}'
           ,N'{2}'
           ,N'{3}'
           ,GETDATE()
           ,@USER)", img.Type, img.FileName, img.FilePath, img.OriginalFileName);
                        }

                    }

                    cmds.Add(cmd);

                    db.ExecuteNonQueryFromMultipleCommandText(cmds);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEvent(string title, string date, string desc, string dispStart, string dispEnd, string condition, string regPeriod, string regPeriodStart, string regPeriodEnd, string eventStart, string eventEnd, string limitGuest, string user, string id, string descTH, string conditionTH, string regPeriodTH, string without_guest, string car_owner_only, string thank_you_message_en, string thank_you_message_th,string one_member_per_event, List<UploadImageModel> banner, List<UploadImageModel> bannerDetail, string event_group_ids, string event_type, string preferredmodelid, string car_age_start, string car_age_end)
        {
            try
            {
                string cmd = @"
DECLARE @TITLE  NVARCHAR(250) = N'{0}'
DECLARE @DATE  NVARCHAR(20) = N'{1}'
DECLARE @DESC  NVARCHAR(MAX) = N'{2}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{3}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{4}'
DECLARE @CONDITION  NVARCHAR(MAX) = N'{5}'
DECLARE @REG_PERIOD  NVARCHAR(50) = N'{6}'
DECLARE @REG_PERIOD_START  NVARCHAR(20) = N'{7}'
DECLARE @REG_PERIOD_END  NVARCHAR(20) = N'{8}'
DECLARE @EVENT_START  NVARCHAR(20) = N'{9}'
DECLARE @EVENT_END  NVARCHAR(20) = N'{10}'
DECLARE @LIMIT_GUEST  INT = N'{11}'
DECLARE @USER  NVARCHAR(50) = N'{12}'
DECLARE @ID INT = N'{13}'
DECLARE @DESCTH  NVARCHAR(MAX) = N'{14}'
DECLARE @CONDITIONTH  NVARCHAR(MAX) = N'{15}'
DECLARE @REG_PERIODTH  NVARCHAR(50) = N'{16}'
DECLARE @WITHOUTGUEST INT = N'{17}'
DECLARE @CAROWNERONLY INT = N'{18}'
DECLARE @THANKYOU_MESSAGE_EN NVARCHAR(MAX) = N'{19}'
DECLARE @THANKYOU_MESSAGE_TH NVARCHAR(MAX) = N'{20}'
DECLARE @event_group_ids NVARCHAR(MAX) = N'{21}'
DECLARE @one_member_per_event TINYINT = N'{22}'
DECLARE @event_type NVARCHAR(50) = N'{23}'
DECLARE @preferred_model_ids NVARCHAR(MAX) = N'{24}'
DECLARE @period_car_age_start INT = N'{25}'
DECLARE @period_car_age_end INT = N'{26}'

UPDATE [T_EVENTS]
SET		TITLE = CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		[DATE] = CASE LEN(@DATE) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DATE, 103) END,
		[DESC] = CASE LEN(@DESC) WHEN 0 THEN NULL ELSE @DESC END,		
		DISPLAY_START = CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		DISPLAY_END = CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
		CONDITION = CASE LEN(@CONDITION) WHEN 0 THEN NULL ELSE @CONDITION END,
		REG_PERIOD = CASE LEN(@REG_PERIOD) WHEN 0 THEN NULL ELSE @REG_PERIOD END,
		REG_PERIOD_START = CASE LEN(@REG_PERIOD_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @REG_PERIOD_START, 103) END,
		REG_PERIOD_END = CASE LEN(@REG_PERIOD_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @REG_PERIOD_END, 103) END,
		EVENT_START = CASE LEN(@EVENT_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @EVENT_START, 103) END,
		EVENT_END = CASE LEN(@EVENT_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @EVENT_END, 103) END,
		LIMIT_GUEST = CASE LEN(@LIMIT_GUEST) WHEN 0 THEN NULL ELSE @LIMIT_GUEST END,
		UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
		UPDATE_USER = @USER,
        [desc_th] = CASE LEN(@DESCTH) WHEN 0 THEN NULL ELSE @DESCTH END,
        condition_th = CASE LEN(@CONDITIONTH) WHEN 0 THEN NULL ELSE @CONDITIONTH END,
		reg_period_th = CASE LEN(@REG_PERIODTH) WHEN 0 THEN NULL ELSE @REG_PERIODTH END,
        without_guest = CASE LEN(@WITHOUTGUEST) WHEN 0 THEN NULL ELSE @WITHOUTGUEST END,
        car_owner_only = CASE LEN(@CAROWNERONLY) WHEN 0 THEN NULL ELSE @CAROWNERONLY END,
        THANKYOU_MESSAGE_EN = CASE LEN(@THANKYOU_MESSAGE_EN) WHEN 0 THEN NULL ELSE @THANKYOU_MESSAGE_EN END,
        THANKYOU_MESSAGE_TH = CASE LEN(@THANKYOU_MESSAGE_TH) WHEN 0 THEN NULL ELSE @THANKYOU_MESSAGE_TH END,
        event_group_ids = CASE LEN(@event_group_ids) WHEN 0 THEN NULL ELSE @event_group_ids END,
        one_member_per_event = CASE LEN(@one_member_per_event) WHEN 0 THEN NULL ELSE @one_member_per_event END,
        event_type = CASE LEN(@event_type) WHEN 0 THEN NULL ELSE @event_type END,
        preferred_model_ids = CASE LEN(@preferred_model_ids) WHEN 0 THEN NULL ELSE @preferred_model_ids END,
        period_car_age_start = CASE LEN(@period_car_age_start) WHEN 0 THEN NULL ELSE @period_car_age_start END,
        period_car_age_end = CASE LEN(@period_car_age_end) WHEN 0 THEN NULL ELSE @period_car_age_end END

WHERE	ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(title.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(date),
                        WebUtility.GetSQLTextValue(desc.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(condition.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(regPeriod.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(regPeriodStart),
                        WebUtility.GetSQLTextValue(regPeriodEnd),
                        WebUtility.GetSQLTextValue(eventStart),
                        WebUtility.GetSQLTextValue(eventEnd),
                        WebUtility.GetSQLTextValue(limitGuest),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(descTH.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(conditionTH.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(regPeriodTH.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(without_guest),
                        WebUtility.GetSQLTextValue(car_owner_only),
                        WebUtility.GetSQLTextValue(thank_you_message_en),
                        WebUtility.GetSQLTextValue(thank_you_message_th),
                        WebUtility.GetSQLTextValue(event_group_ids),
                        WebUtility.GetSQLTextValue(one_member_per_event),
                        WebUtility.GetSQLTextValue(event_type),
                        WebUtility.GetSQLTextValue(preferredmodelid),
                        WebUtility.GetSQLTextValue(car_age_start),
                        WebUtility.GetSQLTextValue(car_age_end)
                        );

                    List<UploadImageModel> imgs = new List<UploadImageModel>();
                    imgs.AddRange(banner);
                    imgs.AddRange(bannerDetail);
                    foreach (UploadImageModel img in imgs.Where(c => !string.IsNullOrEmpty(c.Status)))
                    {
                        if (img.Status.Equals("NEW"))
                        {
                            cmd += string.Format(@"
INSERT INTO [upload_Image]
           ([Parent_Id]
           ,[Type]
           ,[Page]
           ,[File_Name]
           ,[File_Path]
           ,[Original_File_Name]
           ,[Created_Date]
           ,[Created_User])
     VALUES
           (N'{0}'
           ,N'{1}'
           ,N'EVENTS'
           ,N'{2}'
           ,N'{3}'
           ,N'{4}'
           ,GETDATE()
           ,@USER)", id, img.Type, img.FileName, img.FilePath, img.OriginalFileName);
                        }
                        else if (img.Status.Equals("DEL") && img.ID != "")
                        {
                            cmd += string.Format(@" DELETE [upload_Image] WHERE ID=N'{0}'", img.ID);
                        }
                    }

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEvent(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	T_EVENTS
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	ID = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUploadImage(string parrentId, string type)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @ParrentId INT = N'{0}'
DECLARE @Type NVARCHAR(10) = N'{1}'
SELECT [Id]
      ,[Parent_Id]
      ,[Type]
      ,[Page]
      ,[File_Name]
      ,[File_Path]
      ,[Original_File_Name]
      ,[Created_Date]
      ,[Created_User]
  FROM [dbo].[upload_Image]

WHERE [Parent_Id]=@ParrentId AND [Type]=@Type AND [Page]='EVENTS'
ORDER BY ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(parrentId), WebUtility.GetSQLTextValue(type));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        internal DataTable GetEventGroup()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT ID, NAME_EN
FROM event_group ORDER BY ID
";
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        internal DataTable GetPeriodTime()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT ID, NAME_EN
FROM event_period_time ORDER BY ID
";
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;


        }
        public DataTable GetEventQuestionByEventId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                    DECLARE @ID INT = N'{0}'
                                    
                                    SELECT		  [id]
                                                 ,[event_id]
                                                 ,[question_type]
                                                 ,[answer_type]
                                                 ,[question_en]
                                                 ,[question_th]   
                                                 ,[is_active] 
                                                 ,[order]
                                                 ,[created_date]
                                                 ,[created_user]                                                          
                                    FROM		[event_question]
                                    WHERE		[event_id] = @ID and deleted_flag is NULL";


                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    dt = db.GetDataTableFromCommandText(cmd);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetEventAnswerByQuestionId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                    DECLARE @ID INT = N'{0}'
                                    
                                    SELECT		 [id]                                            
                                                ,[question_id]
                                                ,[Answer_en]
                                                ,[Answer_th]
                                                ,[is_optional]  
                                                ,[is_active]
                                                ,[order] 
                                                ,[created_date],[created_user]     
                                    FROM		[event_answer]
                                    WHERE		[question_id] = @ID and deleted_flag is NULL";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    dt = db.GetDataTableFromCommandText(cmd);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetQuestionById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[id]
,[event_id]
,[question_en]
,[question_th]
,[question_type]
,[answer_type]
,[order]       
,[is_active]
FROM		[event_question]
WHERE		[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
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
        public void AddQuestion(string event_id, string question_th, string question_en, string active, string question_type, string answer_type, string user, string order)
        {
            try
            {
                string cmd = @"
DECLARE @QUESTION_ID INT
DECLARE @NAMETH NVARCHAR(200) = N'{0}'
DECLARE @NAMEEN NVARCHAR(200) = N'{1}'
DECLARE @ACTIVE  INT = N'{2}'
DECLARE @QUESTION_TYPE  INT = N'{3}'
DECLARE @ANSWER_TYPE  INT = N'{4}'
DECLARE @USER  NVARCHAR(50) = N'{5}'
DECLARE @ORDER  INT = N'{6}'
DECLARE @EVENT_ID  INT = N'{7}'

INSERT INTO event_question (question_th, question_en, is_active,question_type, answer_type, created_date, created_user, [order], event_id)
VALUES (

    CASE LEN(@NAMETH) WHEN 0 THEN NULL ELSE @NAMETH END,
    CASE LEN(@NAMEEN) WHEN 0 THEN NULL ELSE @NAMEEN END,
    CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
    CASE LEN(@QUESTION_TYPE) WHEN 0 THEN NULL ELSE @QUESTION_TYPE END,
    CASE LEN(@ANSWER_TYPE) WHEN 0 THEN NULL ELSE @ANSWER_TYPE END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER,
    CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
    CASE LEN(@EVENT_ID) WHEN 0 THEN NULL ELSE @EVENT_ID END)

 

";
                if (answer_type == "1")
                {
                    cmd += @"SET @QUESTION_ID = SCOPE_IDENTITY()

                                INSERT INTO event_answer(question_id, answer_th, answer_en, is_active, created_date, created_user, [order], is_optional)
                                                  VALUES(@QUESTION_ID,'None','None',1, DATEADD(HOUR, 7, GETDATE()), @USER,1,0)";
                }
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(question_th),
                        WebUtility.GetSQLTextValue(question_en),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(question_type),
                        WebUtility.GetSQLTextValue(answer_type),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(event_id)
                        );

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateQuestion(string question_id, string question_th, string question_en, string active, string question_type, string answer_type, string user, string order)
        {
            try
            {
                string cmd = @"
DECLARE @ID  INT = N'{0}'
DECLARE @NAMETH NVARCHAR(200) = N'{1}'
DECLARE @NAMEEN NVARCHAR(200) = N'{2}'
DECLARE @ACTIVE  INT = N'{3}'
DECLARE @QUESTION_TYPE  INT = N'{4}'
DECLARE @ANSWER_TYPE  INT = N'{5}'
DECLARE @USER  NVARCHAR(50) = N'{6}'
DECLARE @ORDER  INT = N'{7}'


UPDATE	event_question
SET		question_th = CASE LEN(@NAMETH) WHEN 0 THEN NULL ELSE @NAMETH END,
        question_en = CASE LEN(@NAMEEN) WHEN 0 THEN NULL ELSE @NAMEEN END,
        is_active = CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
        QUESTION_TYPE = CASE LEN(@QUESTION_TYPE) WHEN 0 THEN NULL ELSE @QUESTION_TYPE END,
        ANSWER_TYPE = CASE LEN(@ANSWER_TYPE) WHEN 0 THEN NULL ELSE @ANSWER_TYPE END,
        updated_date = DATEADD(HOUR, 7, GETDATE()),
        updated_user = @USER,
        [order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END        
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(question_id),
                        WebUtility.GetSQLTextValue(question_th),
                        WebUtility.GetSQLTextValue(question_en),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(question_type),
                        WebUtility.GetSQLTextValue(answer_type),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(order));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteEventQuestion(string question_id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'


UPDATE	EVENT_QUESTION
SET		DELETED_FLAG = 'Y',
        DELETE_DATE = DATEADD(HOUR, 7, GETDATE()),
        DELETE_USER = @USER
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(question_id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetAnswerById(string id)
        {
            DataRow row = null;
            try
            {

                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[id]
        ,[question_id]
        ,[Answer_en]
        ,[Answer_th]
        ,[is_active]
        ,[order]
        ,[is_optional]
FROM		[event_answer]
WHERE		[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
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
        public void AddAnswer(string question_id, string answer_en, string answer_th, string active, string user, string order, string is_optional)
        {
            try
            {
                string cmd = @"
DECLARE @QUESTION_ID INT = N'{0}'
DECLARE @ANSWER_TH NVARCHAR(200) = N'{1}'
DECLARE @ANSWER_EN NVARCHAR(200) = N'{2}'
DECLARE @ACTIVE  INT = N'{3}'
DECLARE @USER  NVARCHAR(50) = N'{4}'
DECLARE @ORDER  INT = N'{5}'
DECLARE @IS_OPTIONAL  INT = N'{6}'

INSERT INTO event_answer (question_id,answer_th, answer_en, is_active, created_date, created_user, [order],is_optional)
VALUES (
    CASE LEN(@QUESTION_ID) WHEN 0 THEN NULL ELSE @QUESTION_ID END,
    CASE LEN(@ANSWER_TH) WHEN 0 THEN NULL ELSE @ANSWER_TH END,
    CASE LEN(@ANSWER_EN) WHEN 0 THEN NULL ELSE @ANSWER_EN END,
    CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER,
    CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
    CASE LEN(@IS_OPTIONAL) WHEN 0 THEN NULL ELSE @IS_OPTIONAL END
        )";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(question_id),
                        WebUtility.GetSQLTextValue(answer_th),
                        WebUtility.GetSQLTextValue(answer_en),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(is_optional)
                        );

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateAnswer(string answer_en, string answer_th, string active, string user, string answerId, string order, string is_optional)
        {
            try
            {
                string cmd = @"
DECLARE @ANSWER_ID INT = N'{0}'
DECLARE @ANSWER_TH NVARCHAR(200) = N'{1}'
DECLARE @ANSWER_EN NVARCHAR(200) = N'{2}'
DECLARE @ACTIVE  INT = N'{3}'
DECLARE @USER  NVARCHAR(50) = N'{4}'
DECLARE @ORDER  INT = N'{5}'
DECLARE @IS_OPTIONAL  INT = N'{6}'

UPDATE	event_answer
SET		answer_th = CASE LEN(@ANSWER_TH) WHEN 0 THEN NULL ELSE @ANSWER_TH END,
        answer_en = CASE LEN(@ANSWER_EN) WHEN 0 THEN NULL ELSE @ANSWER_EN END,
        is_active =   CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,   
        updated_date = DATEADD(HOUR, 7, GETDATE()),
        updated_user = @USER,
        [order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END  ,
        IS_OPTIONAL =       CASE LEN(@IS_OPTIONAL) WHEN 0 THEN NULL ELSE @IS_OPTIONAL END
WHERE	id = @ANSWER_ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(answerId),
                        WebUtility.GetSQLTextValue(answer_th),
                        WebUtility.GetSQLTextValue(answer_en),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(is_optional));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteAnswer(string answer_id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'


UPDATE	EVENT_ANSWER
SET		DELETED_FLAG = 'Y',
        DELETE_DATE = DATEADD(HOUR, 7, GETDATE()),
        DELETE_USER = @USER
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(answer_id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}