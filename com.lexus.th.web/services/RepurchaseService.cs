using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;
using System.Data.SqlClient;

namespace com.lexus.th.web
{
    public class RepurchaseService
    {
        private string conn;
        public RepurchaseService()
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
WHERE		deleted_flag IS NULL and Type = 1
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
                                ,[thankyou_message_th],[code_message_en],[code_message_th],[is_active],[created_date],[created_user],[type],[preferred_model_ids])
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
           ,1
          
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
           ,N'REPURCHASE'
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
                    db.ParamterList.Add(new SqlParameter("@reg_start", string.IsNullOrWhiteSpace(booking.RegStart) ? null : WebUtility.GetSQLTextValue(booking.RegStart)));
                    db.ParamterList.Add(new SqlParameter("@reg_end", string.IsNullOrWhiteSpace(booking.RegEnd) ? null : WebUtility.GetSQLTextValue(booking.RegEnd)));
                    db.ParamterList.Add(new SqlParameter("@display_start", string.IsNullOrWhiteSpace(booking.DisplayStart) ? null : WebUtility.GetSQLTextValue(booking.DisplayStart)));
                    db.ParamterList.Add(new SqlParameter("@display_end", string.IsNullOrWhiteSpace(booking.DisplayEnd) ? null : WebUtility.GetSQLTextValue(booking.DisplayEnd)));
                    db.ParamterList.Add(new SqlParameter("@thankyou_message_en", booking.ThankyouMessageEN));
                    db.ParamterList.Add(new SqlParameter("@thankyou_message_th", booking.ThankyouMessageTH));
                    db.ParamterList.Add(new SqlParameter("@code_message_en", booking.CodeMessageEN));
                    db.ParamterList.Add(new SqlParameter("@code_message_th", booking.CodeMessageTH));
                    db.ParamterList.Add(new SqlParameter("@is_active", booking.IsActive));
                    db.ParamterList.Add(new SqlParameter("@created_user", booking.User));

                    db.ParamterList.Add(new SqlParameter("@preferred_model_ids", booking.PreferredModelIds));

                    db.ExecuteNonQueryFromMultipleCommandText(cmds);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBooking(BookingModel booking, List<UploadImageModel> banner, List<UploadImageModel> bannerDetail)
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
                   ,N'REPURCHASE'
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

WHERE [Parent_Id]=@ParrentId AND [Type]=@Type AND [Page]='REPURCHASE'
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