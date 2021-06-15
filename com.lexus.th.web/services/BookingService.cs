using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;
using System.Data.SqlClient;

namespace com.lexus.th.web
{
    public class BookingService
    {
        private string conn;
        public BookingService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetBooking(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT [id]
      ,[title_en]
      ,[title_th]
      ,[desc_en]
      ,[desc_th]
      ,[condition_en]
      ,[condition_th]
      ,[reg_period_en]
      ,[reg_period_th]
      ,[reg_start]
      ,[reg_end]
      ,[display_start]
      ,[display_end]
      ,[thankyou_message_en]
      ,[thankyou_message_th]
      ,[is_active]
      ,[created_date]
      ,[created_user]
      ,[updated_date]
      ,[updated_user]
      ,[deleted_flag]
      ,[delete_date]
      ,[delete_user]
      ,[type]
      ,[is_required_plate_no]
  FROM [dbo].[booking]
WHERE		deleted_flag IS NULL and Type = 3
            AND (ISNULL([TITLE_EN], '') LIKE '%' + @Value + '%'
            OR ISNULL([TITLE_TH], '') LIKE '%' + @Value + '%'
			OR ISNULL([DESC_EN], '') LIKE '%' + @Value + '%'
            OR ISNULL([DESC_TH], '') LIKE '%' + @Value + '%'
			OR ISNULL([CONDITION_EN], '') LIKE '%' + @Value + '%'
            OR ISNULL([CONDITION_TH], '') LIKE '%' + @Value + '%'
			OR ISNULL([thankyou_message_en], '') LIKE '%' + @Value + '%'
            OR ISNULL([thankyou_message_th], '') LIKE '%' + @Value + '%'
			OR ISNULL([reg_period_en], '') LIKE '%' + @Value + '%'
			OR ISNULL([reg_period_th], '') LIKE '%' + @Value + '%'		
			OR CONVERT(NVARCHAR(10), [DISPLAY_START], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DISPLAY_END], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [reg_start], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [reg_end], 103) LIKE '%' + @Value + '%')";

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
        public DataRow GetBookingById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[id]
      ,[title_en]
      ,[title_th]
      ,[desc_en]
      ,[desc_th]
      ,[condition_en]
      ,[condition_th]
      ,[reg_period_en]
      ,[reg_period_th]
      ,[reg_start]
      ,[reg_end]
      ,[display_start]
      ,[display_end]
      ,[thankyou_message_en]
      ,[thankyou_message_th]
      ,[code_message_en]
      ,[code_message_th]
      ,[is_active]
      ,[created_date]
      ,[created_user]
      ,[updated_date]
      ,[updated_user]
      ,[deleted_flag]
      ,[delete_date]
      ,[delete_user]
      ,[type]
      ,[is_required_plate_no]
      ,[preferred_model_ids]
FROM		[dbo].[booking]
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
        public void AddBooking(BookingModel booking, List<UploadImageModel> banner, List<UploadImageModel> bannerDetail)
        {
            try
            {
                List<string> cmds = new List<string>();
     //DECLARE @title_en              nvarchar(250)
     //DECLARE @title_th              nvarchar(250)
     //DECLARE @desc_en               nvarchar(MAX)
     //DECLARE @desc_th               nvarchar(MAX)
     //DECLARE @condition_en          nvarchar(MAX)
     //DECLARE @condition_th          nvarchar(MAX)
     //DECLARE @reg_period_en         nvarchar(50)
     //DECLARE @reg_period_th         nvarchar(50)
     //DECLARE @reg_start             NVARCHAR(20)
     //DECLARE @reg_end               NVARCHAR(20)
     //DECLARE @display_start         NVARCHAR(20)
     //DECLARE @display_end           NVARCHAR(20)
     //DECLARE @thankyou_message_en   nvarchar(MAX)
     //DECLARE @thankyou_message_th   nvarchar(MAX)
     //DECLARE @is_active             bit
     //DECLARE @created_user nvarchar(50)
                string cmd = @"

DECLARE @ID  INT    

INSERT INTO [dbo].[booking]   ([title_en],[title_th],[desc_en],[desc_th],[condition_en],[condition_th],[reg_period_en]
                                ,[reg_period_th],[reg_start],[reg_end],[display_start],[display_end],[thankyou_message_en]
                                ,[thankyou_message_th],[code_message_en],[code_message_th],[is_active],[created_date],[created_user],[type],[is_required_plate_no],[preferred_model_ids])
		   VALUES
           (
            @title_en
           ,@title_th              
           ,@desc_en               
           ,@desc_th               
           ,@condition_en          
           ,@condition_th          
           ,@reg_period_en         
           ,@reg_period_th         
           , CASE LEN(@reg_start) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @reg_start, 103) END
           , CASE LEN(@reg_end) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @reg_end, 103) END
           , CASE LEN(@display_start) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @display_start, 103) END
           , CASE LEN(@display_end) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @display_end, 103) END
           ,@thankyou_message_en   
           ,@thankyou_message_th  
           ,@code_message_en   
           ,@code_message_th  
           ,@is_active
           ,DATEADD(HOUR, 7, GETDATE())        
           ,@created_user    
           ,3
           ,@is_required_plate_no
           ,@preferred_model_ids
            )  

SET @ID = SCOPE_IDENTITY()
";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {            
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
           ,N'BOOKING'
           ,N'{1}'
           ,N'{2}'
           ,N'{3}'
           ,GETDATE()
           ,@created_user)", img.Type, img.FileName, img.FilePath, img.OriginalFileName);
                        }

                    }

                    cmds.Add(cmd);
                 
                    db.ParamterList.Add(new SqlParameter("@title_en", booking.TitleEN));
                    db.ParamterList.Add(new SqlParameter("@title_th", booking.TitleTH));
                    db.ParamterList.Add(new SqlParameter("@desc_en", booking.DescEN));
                    db.ParamterList.Add(new SqlParameter("@desc_th", booking.DescTH));
                    db.ParamterList.Add(new SqlParameter("@condition_en", booking.ConditionEN));
                    db.ParamterList.Add(new SqlParameter("@condition_th", booking.ConditionTH));
                    db.ParamterList.Add(new SqlParameter("@reg_period_en", booking.RegPeriodEN));
                    db.ParamterList.Add(new SqlParameter("@reg_period_th", booking.RegPeriodTH));
                    db.ParamterList.Add(new SqlParameter("@reg_start",        string.IsNullOrWhiteSpace(booking.RegStart     )?null : WebUtility.GetSQLTextValue(booking.RegStart     ) ));
                    db.ParamterList.Add(new SqlParameter("@reg_end",          string.IsNullOrWhiteSpace(booking.RegEnd       )?null : WebUtility.GetSQLTextValue(booking.RegEnd       ) ));
                    db.ParamterList.Add(new SqlParameter("@display_start",    string.IsNullOrWhiteSpace(booking.DisplayStart )?null : WebUtility.GetSQLTextValue(booking.DisplayStart ) ));
                    db.ParamterList.Add(new SqlParameter("@display_end",      string.IsNullOrWhiteSpace(booking.DisplayEnd   )?null : WebUtility.GetSQLTextValue(booking.DisplayEnd   ) ));
                    db.ParamterList.Add(new SqlParameter("@thankyou_message_en", booking.ThankyouMessageEN));
                    db.ParamterList.Add(new SqlParameter("@thankyou_message_th", booking.ThankyouMessageTH));
                    db.ParamterList.Add(new SqlParameter("@code_message_en", booking.CodeMessageEN));
                    db.ParamterList.Add(new SqlParameter("@code_message_th", booking.CodeMessageTH));
                    db.ParamterList.Add(new SqlParameter("@is_active", booking.IsActive));
                    db.ParamterList.Add(new SqlParameter("@created_user", booking.User));
                    db.ParamterList.Add(new SqlParameter("@is_required_plate_no", booking.IsRequirePlateNo));
                    db.ParamterList.Add(new SqlParameter("@preferred_model_ids", booking.PreferredModelIds));

                    db.ExecuteNonQueryFromMultipleCommandText(cmds);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBooking(BookingModel booking, List<UploadImageModel> banner, List<UploadImageModel> bannerDetail )
        {
            try
            {
                string cmd = @"
UPDATE [dbo].[booking]  
SET         title_en = @title_en
           ,title_th = @title_th              
           ,desc_en = @desc_en               
           ,desc_th = @desc_th               
           ,condition_en = @condition_en          
           ,condition_th = @condition_th          
           ,reg_period_en = @reg_period_en         
           ,reg_period_th = @reg_period_th         
           ,reg_start = CASE LEN(@reg_start) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @reg_start, 103) END
           ,reg_end = CASE LEN(@reg_end) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @reg_end, 103) END
           ,display_start = CASE LEN(@display_start) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @display_start, 103) END
           ,display_end = CASE LEN(@display_end) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @display_end, 103) END
           ,thankyou_message_en = @thankyou_message_en   
           ,thankyou_message_th = @thankyou_message_th   
           ,is_active = @is_active
           ,updated_date = DATEADD(HOUR, 7, GETDATE())        
           ,updated_user = @updated_user 
           ,is_required_plate_no = @is_required_plate_no
           ,preferred_model_ids = @preferred_model_ids         
           ,code_message_en = @code_message_en   
           ,code_message_th = @code_message_th  
WHERE	Id = @Id";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {            
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
           ,N'BOOKING'
           ,N'{2}'
           ,N'{3}'
           ,N'{4}'
           ,GETDATE()
           ,@updated_user)", booking.Id, img.Type, img.FileName, img.FilePath, img.OriginalFileName);
                        }
                        else if (img.Status.Equals("DEL") && img.ID != "")
                        {
                            cmd += string.Format(@" DELETE [upload_Image] WHERE ID=N'{0}'", img.ID);
                        }
                    }
                    db.ParamterList.Add(new SqlParameter("@Id", booking.Id));
                    db.ParamterList.Add(new SqlParameter("@title_en", booking.TitleEN));
                    db.ParamterList.Add(new SqlParameter("@title_th", booking.TitleTH));
                    db.ParamterList.Add(new SqlParameter("@desc_en", booking.DescEN));
                    db.ParamterList.Add(new SqlParameter("@desc_th", booking.DescTH));
                    db.ParamterList.Add(new SqlParameter("@condition_en", booking.ConditionEN));
                    db.ParamterList.Add(new SqlParameter("@condition_th", booking.ConditionTH));
                    db.ParamterList.Add(new SqlParameter("@reg_period_en", booking.RegPeriodEN));
                    db.ParamterList.Add(new SqlParameter("@reg_period_th", booking.RegPeriodTH));
                    db.ParamterList.Add(new SqlParameter("@reg_start", string.IsNullOrWhiteSpace(booking.RegStart) ? null : WebUtility.GetSQLTextValue(booking.RegStart)));
                    db.ParamterList.Add(new SqlParameter("@reg_end", string.IsNullOrWhiteSpace(booking.RegEnd) ? null : WebUtility.GetSQLTextValue(booking.RegEnd)));
                    db.ParamterList.Add(new SqlParameter("@display_start", string.IsNullOrWhiteSpace(booking.DisplayStart) ? null : WebUtility.GetSQLTextValue(booking.DisplayStart)));
                    db.ParamterList.Add(new SqlParameter("@display_end", string.IsNullOrWhiteSpace(booking.DisplayEnd) ? null : WebUtility.GetSQLTextValue(booking.DisplayEnd)));
                    db.ParamterList.Add(new SqlParameter("@thankyou_message_en", booking.ThankyouMessageEN));
                    db.ParamterList.Add(new SqlParameter("@thankyou_message_th", booking.ThankyouMessageTH));
                    db.ParamterList.Add(new SqlParameter("@is_active", booking.IsActive));
                    db.ParamterList.Add(new SqlParameter("@updated_user", booking.User));
                    db.ParamterList.Add(new SqlParameter("@is_required_plate_no", booking.IsRequirePlateNo));
                    db.ParamterList.Add(new SqlParameter("@preferred_model_ids", booking.PreferredModelIds));
                    db.ParamterList.Add(new SqlParameter("@code_message_en", booking.CodeMessageEN));
                    db.ParamterList.Add(new SqlParameter("@code_message_th", booking.CodeMessageTH));
                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteBooking(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	[dbo].[booking]
SET		deleted_flag = 'Y',
        delete_date = DATEADD(HOUR, 7, GETDATE()),
        delete_user = @USER
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

WHERE [Parent_Id]=@ParrentId AND [Type]=@Type AND [Page]='BOOKING'
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

        public DataTable GetBookingQuestionByBookingId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                    DECLARE @ID INT = N'{0}'
                                    
                                    SELECT		  [id]
                                                 ,[booking_id]                                                 
                                                 ,[answer_type]
                                                 ,[question_key]
                                                 ,[question_en]
                                                 ,[question_th]   
                                                 ,[is_active] 
                                                 ,[order]
                                                 ,[is_optional]
                                                 ,[created_date]
                                                 ,[created_user]                                                          
                                    FROM		[booking_question]
                                    WHERE		[booking_id] = @ID and deleted_flag is NULL";


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
        public DataTable GetBookingAnswerByQuestionId(string id)
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
                                    FROM		[booking_answer]
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
,[booking_id]
,[question_en]
,[question_th]
,[answer_type]
,[question_key]
,[order]       
,[is_active]
,[Is_Optional]
FROM		[booking_question]
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
        public void AddQuestion(string booking_id, string question_th, string question_en, string active , string answer_type, string user, string order,string is_optional)
        {
            try
            {
                string cmd = @"
DECLARE @QUESTION_ID INT
DECLARE @NAMETH NVARCHAR(200) = N'{0}'
DECLARE @NAMEEN NVARCHAR(200) = N'{1}'
DECLARE @ACTIVE  INT = N'{2}'
DECLARE @ANSWER_TYPE  INT = N'{3}'
DECLARE @USER  NVARCHAR(50) = N'{4}'
DECLARE @ORDER  INT = N'{5}'
DECLARE @BOOKING_ID  INT = N'{6}'
DECLARE @IS_OPTIONAL  INT = N'{7}'


INSERT INTO booking_question (question_th, question_en, is_active, answer_type, created_date, created_user, [order], booking_id,is_optional)
VALUES (

    CASE LEN(@NAMETH) WHEN 0 THEN NULL ELSE @NAMETH END,
    CASE LEN(@NAMEEN) WHEN 0 THEN NULL ELSE @NAMEEN END,
    CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
  
    CASE LEN(@ANSWER_TYPE) WHEN 0 THEN NULL ELSE @ANSWER_TYPE END,  
    DATEADD(HOUR, 7, GETDATE()), 
    @USER,
    CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
    CASE LEN(@BOOKING_ID) WHEN 0 THEN NULL ELSE @BOOKING_ID END,
    CASE LEN(@IS_OPTIONAL) WHEN 0 THEN NULL ELSE @IS_OPTIONAL END )

 

";
               
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(question_th),
                        WebUtility.GetSQLTextValue(question_en),
                        WebUtility.GetSQLTextValue(active),                       
                        WebUtility.GetSQLTextValue(answer_type),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(order),
                        WebUtility.GetSQLTextValue(booking_id),
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
        public void UpdateQuestion(string question_id, string question_th, string question_en, string active, string answer_type, string user, string order,string is_optional)
        {
            try
            {
                string cmd = @"
DECLARE @ID  INT = N'{0}'
DECLARE @NAMETH NVARCHAR(200) = N'{1}'
DECLARE @NAMEEN NVARCHAR(200) = N'{2}'
DECLARE @ACTIVE  INT = N'{3}'
DECLARE @ANSWER_TYPE  INT = N'{4}'
DECLARE @USER  NVARCHAR(50) = N'{5}'
DECLARE @ORDER  INT = N'{6}'
DECLARE @IS_OPTIONAL  INT = N'{7}'

UPDATE	booking_question
SET		question_th = CASE LEN(@NAMETH) WHEN 0 THEN NULL ELSE @NAMETH END,
        question_en = CASE LEN(@NAMEEN) WHEN 0 THEN NULL ELSE @NAMEEN END,
        is_active = CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,        
        ANSWER_TYPE = CASE LEN(@ANSWER_TYPE) WHEN 0 THEN NULL ELSE @ANSWER_TYPE END,        
        updated_date = DATEADD(HOUR, 7, GETDATE()),
        updated_user = @USER,
        [order] = CASE LEN(@ORDER) WHEN 0 THEN NULL ELSE @ORDER END,
        [is_optional] = CASE LEN(@IS_OPTIONAL) WHEN 0 THEN NULL ELSE @IS_OPTIONAL END      
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(question_id),
                        WebUtility.GetSQLTextValue(question_th),
                        WebUtility.GetSQLTextValue(question_en),
                        WebUtility.GetSQLTextValue(active),                        
                        WebUtility.GetSQLTextValue(answer_type),
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
        public void DeleteQuestion(string question_id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'


UPDATE	BOOKING_QUESTION
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
FROM		[booking_answer]
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

INSERT INTO booking_answer (question_id,answer_th, answer_en, is_active, created_date, created_user, [order],is_optional)
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

UPDATE	booking_answer
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


UPDATE	BOOKING_ANSWER
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

        public DataTable GetPreferredModel()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
                    SELECT [MODEL_ID] AS ID
                          ,[MODEL]
                          ,[IMAGE]
                          ,[DEL_FLAG]
                          ,[DEL_DT]
                          ,[DEL_USER]
                          ,[is_test_drive]
                    FROM [dbo].[T_CAR_MODEL]
                    WHERE [DEL_FLAG] IS NULL
                    ORDER BY MODEL
                 ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {                   
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

    }
}