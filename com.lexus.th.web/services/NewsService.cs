using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace com.lexus.th.web
{
    public class NewsService
    {
        private string conn;
        public NewsService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetNews(string searchValue)
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
            ,[desc_th]
            ,[is_active]
			,[IMAGES1]
			,[IMAGES2]
			,[IMAGES3]
			,[IMAGES4]
			,[IMAGES5]
			,[DISPLAY_START]
			,[DISPLAY_END]
			,[CREATE_DT]
			,[CREATE_USER]
			,[UPDATE_DT]
			,[UPDATE_USER]
FROM		[T_NEWS]
WHERE		DEL_FLAG IS NULL
            AND (ISNULL([TITLE], '') LIKE '%' + @Value + '%'
			OR ISNULL([DESC], '') LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DATE], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DISPLAY_START], 103) LIKE '%' + @Value + '%'
			OR CONVERT(NVARCHAR(10), [DISPLAY_END], 103) LIKE '%' + @Value + '%')";

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
        public DataRow GetNewsById(string id)
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
            ,[desc_th]
            ,[is_active]
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
			,[DISPLAY_START]
			,[DISPLAY_END]
			,[CREATE_DT]
			,[CREATE_USER]
			,[UPDATE_DT]
			,[UPDATE_USER]
FROM		[T_NEWS]
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
        public void AddNews(string title, string date, string desc, string dispStart, string dispEnd, string user, string descTH, string active, List<UploadImageModel> banner, List<UploadImageModel> bannerDetail)
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
DECLARE @USER  NVARCHAR(50) = N'{5}'
DECLARE @DESCTH  NVARCHAR(MAX) = N'{6}'
DECLARE @ACTIVE  INT = N'{7}'

INSERT INTO [T_NEWS] ([TITLE],[DATE],[DESC],[DISPLAY_START],[DISPLAY_END],[CREATE_DT],[CREATE_USER],desc_th,is_active)
VALUES (CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		CASE LEN(@DATE) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DATE, 103) END,
		CASE LEN(@DESC) WHEN 0 THEN NULL ELSE @DESC END,
		CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
		CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER,
        CASE LEN(@DESCTH) WHEN 0 THEN NULL ELSE @DESCTH END,
        CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END) 
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
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(descTH.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(active));

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
           ,N'NEWS'
           ,N'{1}'
           ,N'{2}'
           ,N'{3}'
           ,GETDATE()
           ,@USER)",img.Type, img.FileName, img.FilePath, img.OriginalFileName);
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
        public void UpdateNews(string title, string date, string desc, string dispStart, string dispEnd, string user, string id, string descTH, string active, List<UploadImageModel> banner, List<UploadImageModel> bannerDetail)
        {
            try
            {
                string cmd = @"
DECLARE @TITLE  NVARCHAR(250) = N'{0}'
DECLARE @DATE  NVARCHAR(20) = N'{1}'
DECLARE @DESC  NVARCHAR(MAX) = N'{2}'
DECLARE @DISPLAY_START  NVARCHAR(20) = N'{3}'
DECLARE @DISPLAY_END  NVARCHAR(20) = N'{4}'
DECLARE @USER  NVARCHAR(50) = N'{5}'
DECLARE @ID INT = N'{6}'
DECLARE @DESCTH  NVARCHAR(MAX) = N'{7}'
DECLARE @ACTIVE  INT = N'{8}'


UPDATE		[T_NEWS]
SET			TITLE = CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
			[DATE] = CASE LEN(@DATE) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DATE, 103) END,
			[DESC] = CASE LEN(@DESC) WHEN 0 THEN NULL ELSE @DESC END,           
			DISPLAY_START = CASE LEN(@DISPLAY_START) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_START, 103) END,
			DISPLAY_END = CASE LEN(@DISPLAY_END) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @DISPLAY_END, 103) END,
			UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
			UPDATE_USER = @USER,
            [desc_th] = CASE LEN(@DESCTH) WHEN 0 THEN NULL ELSE @DESCTH END,
            [is_active] = CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END
WHERE		[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(title.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(date),
                        WebUtility.GetSQLTextValue(desc.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(dispStart),
                        WebUtility.GetSQLTextValue(dispEnd),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(descTH.Replace("'", "’")),
                        WebUtility.GetSQLTextValue(active));

                    List<UploadImageModel> imgs = new List<UploadImageModel>();
                    imgs.AddRange(banner);
                    imgs.AddRange(bannerDetail);
                    foreach (UploadImageModel img in imgs.Where(c=> !string.IsNullOrEmpty(c.Status)))
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
           ,N'NEWS'
           ,N'{2}'
           ,N'{3}'
           ,N'{4}'
           ,GETDATE()
           ,@USER)",id, img.Type, img.FileName, img.FilePath, img.OriginalFileName);
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
        public void DeleteNews(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	T_NEWS
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

        public DataTable GetVendor(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT	contact_name,
    add_name,
    sap_no,
    add_tel,
    iif(cancel_date IS NULL, 'ใช้งาน', 'ยกเลิก') status
FROM		SB_Vendor
WHERE		(ISNULL(sap_no, '') LIKE '%' + @Value + '%'
			OR ISNULL(add_name, '') LIKE '%' + @Value + '%'
			OR ISNULL(add_tel, '') LIKE '%' + @Value + '%')";

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

        public void AddVender(string tsap_no,
string tven_code,
string tadd_name,
string ttax_id,
string tadd_no,
string tadd_moo,
string tadd_trog,
string tadd_soi,
string tadd_road,
string tadd_tumbol,
string tadd_amphur,
string tadd_province,
string tadd_zip,
string tadd_tel,
string tadd_phone,
string tadd_fax,
string tadd_email,
string tvat_type,
string tvat_code,
string tven_ship_term,
string tven_credit_term,
string tven_credit_limit,
string tcontact_ldate)
        {
            try
            {
                string cmd = @"
DECLARE @tsap_no  NVARCHAR(250) = N'{0}'
DECLARE @tven_code  NVARCHAR(20) = N'{1}'
DECLARE @tadd_name  NVARCHAR(20) = N'{2}'
DECLARE @ttax_id  NVARCHAR(150) = N'{3}'
DECLARE @tadd_no  NVARCHAR(150) = N'{4}'
DECLARE @tadd_moo  NVARCHAR(150) = N'{5}'
DECLARE @tadd_trog  NVARCHAR(150) = N'{6}'
DECLARE @tadd_soi  NVARCHAR(150) = N'{7}'
DECLARE @tadd_road  NVARCHAR(20) = N'{8}'
DECLARE @tadd_tumbol  NVARCHAR(20) = N'{9}'
DECLARE @tadd_amphur  NVARCHAR(50) = N'{10}'
DECLARE @tadd_province  NVARCHAR(MAX) = N'{11}'
DECLARE @tadd_zip  NVARCHAR(MAX) = N'{12}'
DECLARE @tadd_tel  NVARCHAR(MAX) = N'{13}'
DECLARE @tadd_phone  NVARCHAR(MAX) = N'{14}'
DECLARE @tadd_fax  NVARCHAR(MAX) = N'{15}'
DECLARE @tadd_email  NVARCHAR(MAX) = N'{16}'
DECLARE @tvat_type  NVARCHAR(MAX) = N'{17}'
DECLARE @tvat_code  NVARCHAR(MAX) = N'{18}'
DECLARE @tven_ship_term  NVARCHAR(MAX) = N'{19}'
DECLARE @tven_credit_term  NVARCHAR(MAX) = N'{20}'
DECLARE @tven_credit_limit  NVARCHAR(MAX) = N'{21}'
DECLARE @tcontact_ldate  NVARCHAR(MAX) = N'{22}'

INSERT INTO SB_Vendor
(sap_no,
add_name,
tax_id,
add_no,
add_moo,
add_trog,
add_soi,
add_road,
add_tumbol,
add_amphur,
add_province,
add_zip,
add_tel,
add_phone,
add_fax,
add_email,
vat_type,
vat_code,
ven_ship_term,
ven_credit_term,
ven_credit_limit,
contact_ldate)
VALUES (CASE LEN(@tven_code) WHEN 0 THEN NULL ELSE @tven_code END,
        CASE LEN(@tadd_name) WHEN 0 THEN NULL ELSE @tadd_name END,
        CASE LEN(@ttax_id) WHEN 0 THEN NULL ELSE @ttax_id END,
        CASE LEN(@tadd_no) WHEN 0 THEN NULL ELSE @tadd_no END,
        CASE LEN(@tadd_moo) WHEN 0 THEN NULL ELSE @tadd_moo END,
        CASE LEN(@tadd_trog) WHEN 0 THEN NULL ELSE @tadd_trog END,
        CASE LEN(@tadd_soi) WHEN 0 THEN NULL ELSE @tadd_soi END,
        CASE LEN(@tadd_road) WHEN 0 THEN NULL ELSE @tadd_road END,
        CASE LEN(@tadd_tumbol) WHEN 0 THEN NULL ELSE @tadd_tumbol END,
        CASE LEN(@tadd_amphur) WHEN 0 THEN NULL ELSE @tadd_amphur END,
        CASE LEN(@tadd_province) WHEN 0 THEN NULL ELSE @tadd_province END,
        CASE LEN(@tadd_zip) WHEN 0 THEN NULL ELSE @tadd_zip END,
        CASE LEN(@tadd_tel) WHEN 0 THEN NULL ELSE @tadd_tel END,
        CASE LEN(@tadd_phone) WHEN 0 THEN NULL ELSE @tadd_phone END,
        CASE LEN(@tadd_fax) WHEN 0 THEN NULL ELSE @tadd_fax END,
        CASE LEN(@tadd_email) WHEN 0 THEN NULL ELSE @tadd_email END,
        CASE LEN(@tvat_type) WHEN 0 THEN NULL ELSE @tvat_type END,
        CASE LEN(@tvat_code) WHEN 0 THEN NULL ELSE @tvat_code END,

        CASE LEN(@tven_ship_term) WHEN 0 THEN NULL ELSE @tven_ship_term END,
        CASE LEN(@tven_credit_term) WHEN 0 THEN NULL ELSE @tven_credit_term END,
        CASE LEN(@tven_credit_limit) WHEN 0 THEN NULL ELSE @tven_credit_limit END,
        CASE LEN(@tcontact_ldate) WHEN 0 THEN NULL ELSE @tcontact_ldate END)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        "",
                        WebUtility.GetSQLTextValue(tven_code),
                        WebUtility.GetSQLTextValue(tadd_name),
                        WebUtility.GetSQLTextValue(ttax_id),
                        WebUtility.GetSQLTextValue(tadd_no),
                        WebUtility.GetSQLTextValue(tadd_moo),
                        WebUtility.GetSQLTextValue(tadd_trog),
                        WebUtility.GetSQLTextValue(tadd_soi),
                        WebUtility.GetSQLTextValue(tadd_road),
                        WebUtility.GetSQLTextValue(tadd_tumbol),
                        WebUtility.GetSQLTextValue(tadd_amphur),
                        WebUtility.GetSQLTextValue(tadd_province),
                        WebUtility.GetSQLTextValue(tadd_zip),
                        WebUtility.GetSQLTextValue(tadd_tel),
                        WebUtility.GetSQLTextValue(tadd_phone),
                        WebUtility.GetSQLTextValue(tadd_fax),
                        WebUtility.GetSQLTextValue(tadd_email),
                        WebUtility.GetSQLTextValue(tvat_type),
                        WebUtility.GetSQLTextValue(tvat_code),
                        WebUtility.GetSQLTextValue(tven_ship_term),
                        WebUtility.GetSQLTextValue(tven_credit_term),
                        WebUtility.GetSQLTextValue(tven_credit_limit),
                        WebUtility.GetSQLTextValue(tcontact_ldate));

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

WHERE [Parent_Id]=@ParrentId AND [Type]=@Type AND [Page]='NEWS'
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
    }
}