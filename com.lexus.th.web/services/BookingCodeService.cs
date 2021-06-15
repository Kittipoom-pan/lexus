using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class BookingCodeService
    {
        private string conn;
        public BookingCodeService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetBookingCodeStatus()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
SELECT [id] 
      ,[name_en]
FROM  [booking_code_status]
WHERE deleted_flag IS NULL";

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

        public DataTable GetBookingByType(string bookingType)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
DECLARE @type INT = N'{0}'
DECLARE @TABLE TABLE (ID INT, title_en NVARCHAR(250))

INSERT INTO @TABLE VALUES (0, '-- select --')

INSERT INTO @TABLE
SELECT		id,title_en FROM	[Booking] WHERE type = @type AND deleted_flag IS NULL

SELECT * FROM @TABLE ORDER BY  id,title_en ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(bookingType));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetBookingCodeByBookingId(string bookingId)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
DECLARE @id INT = N'{0}'

SELECT 
       bc.[id]
      ,bc.[booking_id]
      ,bc.[code]
      ,bs.[name_en] as status
      ,bc.[status_id]
      ,bc.[created_date]
      ,bc.[created_user]
      ,bc.[updated_date]
      ,bc.[updated_user]
      ,bc.[deleted_flag]
      ,bc.[delete_date]
      ,bc.[delete_user]
  FROM [dbo].[booking_code] bc
  left join [dbo].[booking_code_status] bs on bs.id = bc.status_id
WHERE bc.booking_id = @id AND bc.deleted_flag IS NULL";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(bookingId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataRow GetBookingCodeById(string bookingCodeId, string bookingId)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @id INT = N'{0}'
DECLARE @booking_id INT = N'{1}'

SELECT 
       [id]
      ,[booking_id]
      ,[code]
      ,[status_id]
      ,[created_date]
      ,[created_user]
      ,[updated_date]
      ,[updated_user]
      ,[deleted_flag]
      ,[delete_date]
      ,[delete_user]
  FROM [dbo].[booking_code] 
WHERE id = @id AND booking_id = @booking_id AND deleted_flag IS NULL";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(bookingCodeId),
                        WebUtility.GetSQLTextValue(bookingId));

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
        public void AddBookingCode(string bookingId, string code, string user, string status)
        {
            try
            {
                string cmd = @"
DECLARE @booking_id INT = N'{0}'
DECLARE @code NVARCHAR(50) = N'{1}'
DECLARE @user NVARCHAR(50) = N'{2}'
DECLARE @status NVARCHAR(10) = N'{3}'

INSERT INTO [booking_code] ([BOOKING_ID],[CODE],[STATUS_ID],[CREATED_DATE],[CREATED_USER])
VALUES (@booking_id, @code, @status, DATEADD(HOUR, 7, GETDATE()), @user)";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(bookingId),
                        WebUtility.GetSQLTextValue(code),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(status));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBookingCode(string bookingId, string code, string user, string id, string status)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @BOOKING_ID INT = N'{1}'
DECLARE @CODE NVARCHAR(50) = N'{2}'
DECLARE @USER NVARCHAR(50) = N'{3}'
DECLARE @STATUS_ID NVARCHAR(10) = N'{4}'

UPDATE  [BOOKING_CODE]
SET		[CODE] = @CODE,
        [STATUS_ID] = @STATUS_ID,
		[UPDATED_DATE] = DATEADD(HOUR, 7, GETDATE()),
		[UPDATED_USER] = @USER
WHERE	BOOKING_ID = @BOOKING_ID AND [ID] = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                         WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(bookingId),
                        WebUtility.GetSQLTextValue(code),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(status)
                        );

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteBookingCode(string bookingCodeId, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	[booking_code]
SET		[deleted_flag] = 'Y',
        [delete_date] = DATEADD(HOUR, 7, GETDATE()),
        [delete_user] = @USER
WHERE	ID = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(bookingCodeId),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsDupplicateBookingCode(string bookingId, string code)
        {
            bool isDupp = true;
            try
            {
                string cmd = @"
DECLARE @booking_id INT = N'{0}'
DECLARE @code NVARCHAR(50) = N'{1}'

SELECT COUNT(1) FROM Booking_Code WHERE booking_id = @booking_id AND code = @code";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(bookingId),
                        WebUtility.GetSQLTextValue(code));

                    int cnt = db.ExecuteScalarFromCommandText<int>(cmd);
                    if (cnt == 0)
                    {
                        isDupp = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDupp;
        }
        public void UploadBookingCode(string bookingId, List<string> codes, string user)
        {
            try
            {
                string cmd = @"
DECLARE @booking_id INT = N'{0}'
DECLARE @code NVARCHAR(50) = N'{1}'
DECLARE @user NVARCHAR(50) = N'{2}'

INSERT INTO [booking_code] ([BOOKING_ID],[CODE],[STATUS_ID],[CREATED_DATE],[CREATED_USER])
VALUES (@booking_id, @code, 1, DATEADD(HOUR, 7, GETDATE()), @user)";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    List<string> cmdList = new List<string>();
                    foreach (string code in codes)
                    {
                        cmdList.Add(string.Format(cmd,
                            WebUtility.GetSQLTextValue(bookingId),
                            WebUtility.GetSQLTextValue(code),
                            WebUtility.GetSQLTextValue(user)));
                    }

                    db.ExecuteNonQueryFromCommandText(cmdList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}