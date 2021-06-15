using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class OnlineBookingService
    {
        private string conn;
        public OnlineBookingService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetDealer()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"SELECT 
                               d.DEALER_ID,
                               dealer_master.display_th
                               FROM T_DEALER d
                               INNER JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = d.DEALER_CODE AND dealer_master.branch_code = d.branch_code AND dealer_master.is_active = 1
                               WHERE d.ACTIVE = 1  AND d.DEL_FLAG IS NULL";

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

                                INSERT INTO @TABLE VALUES (0, 'ALL')

                                INSERT INTO @TABLE
                                SELECT id,title_en FROM [Booking] WHERE type = @type AND deleted_flag IS NULL

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

        public DataTable GetBookingBySaveType(string bookingType)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
                                DECLARE @TYPE INT = N'{1}'
                                
                                SELECT      b.id
                                            ,b.booking_id
                                            ,b.name
			                                ,b.surname
                                            ,b.contact_number
                                            ,b.email
                                            ,tc.MODEL
                                            ,b.remark
                                            ,b.created_date
                                            ,b.created_user
                                            ,b.updated_date
                                            ,b.updated_user
                                FROM		booking_register b LEFT JOIN T_CAR_MODEL tc ON b.preferred_model_id = tc.MODEL_ID
                                WHERE		deleted_flag IS NULL AND b.type =  @TYPE";

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

        public DataTable GetBookingCodeByBookingId(string bookingId, string bookingType)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = "";
                if (bookingType == "0" && string.IsNullOrEmpty(bookingId)) // default
                {
                    cmd = @"    DECLARE @id INT = N'{0}'

                                SELECT      b.id
                                            ,b.booking_id
                                            ,b.name
			                                ,b.surname
                                            ,b.contact_number
                                            ,b.email
                                            ,tc.MODEL
                                            ,b.remark
                                            ,b.created_date
                                            ,b.created_user
                                            ,b.updated_date
                                            ,b.updated_user
                                FROM		booking_register b LEFT JOIN T_CAR_MODEL tc ON b.preferred_model_id = tc.MODEL_ID
                                WHERE		deleted_flag IS NULL AND b.booking_id = @id 
                                ";
                }
                else if ((bookingType == "1" || bookingType == "2" || bookingType == "3") && (bookingId == "0")) // validate type
                {
                    cmd = @"
                                DECLARE @TYPE INT = N'{1}'
                                
                                SELECT      b.id
                                            ,b.booking_id
                                            ,b.name
			                                ,b.surname
                                            ,b.contact_number
                                            ,b.email
                                            ,tc.MODEL
                                            ,b.remark
                                            ,b.created_date
                                            ,b.created_user
                                            ,b.updated_date
                                            ,b.updated_user
                                FROM		booking_register b LEFT JOIN T_CAR_MODEL tc ON b.preferred_model_id = tc.MODEL_ID
                                WHERE		deleted_flag IS NULL AND b.type =  @TYPE";
                }
                else if (!string.IsNullOrEmpty(bookingType) && !string.IsNullOrEmpty(bookingId))  // validate booking
                {
                    cmd = @"    DECLARE @id INT = N'{0}'

                                SELECT      b.id
                                            ,b.booking_id
                                            ,b.name
			                                ,b.surname
                                            ,b.contact_number
                                            ,b.email
                                            ,tc.MODEL
                                            ,b.remark
                                            ,b.created_date
                                            ,b.created_user
                                            ,b.updated_date
                                            ,b.updated_user
                                FROM		booking_register b LEFT JOIN T_CAR_MODEL tc ON b.preferred_model_id = tc.MODEL_ID
                                WHERE		deleted_flag IS NULL AND b.booking_id = @id 
                                ";
                }

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(bookingId),
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

        public DataRow GetBookingById(string bookingId)
        {
            DataRow row = null;
           //List<string> a = new 
            try
            {
                string cmd = @"
                                   DECLARE @bookingId INT = N'{0}'
                            
                                   
                                   SELECT name,surname, contact_number,email,plate_number,remark, referral_name, referral_surname, referral_contact_number, referral_email, dealer_id, need_to_test_drive 
                                   FROM booking_register WHERE id = @bookingId
                              ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
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
       
        public void UpdateBooking(string id, string name, string surname, string contact_number, string email, string plate_number, string remark, string referral_name, string referral_surname, string referral_contact_number,string referral_email, string dealer, string test_drive)
        {
            try
            {
                string cmd = @"
                                DECLARE @ID INT = N'{0}'
                                DECLARE @NAME NVARCHAR(250) = N'{1}'
                                DECLARE @SURNAME NVARCHAR(250) = N'{2}'
                                DECLARE @CONTACT_NUMBER NVARCHAR(250) = N'{3}'
                                DECLARE @EMAIL NVARCHAR(250) = N'{4}'
                                DECLARE @PLATE_NUMBER NVARCHAR(250) = N'{5}'
                                DECLARE @REMARK NVARCHAR(250) = N'{6}'
                                DECLARE @REFERRAL_NAME NVARCHAR(250) = N'{7}'
                                DECLARE @REFERRAL_SURNAME NVARCHAR(250) = N'{8}'
                                DECLARE @REFERRAL_CONTACT_NUMBER NVARCHAR(250) = N'{9}'
                                DECLARE @REFERRAL_EMAIL NVARCHAR(250) = N'{10}'
                                DECLARE @DEALER NVARCHAR(250) = N'{11}'
                                DECLARE @TEST_DRIVE NVARCHAR(250) = N'{12}'
               

                                UPDATE  [booking_register]
                                SET		name = CASE LEN(@NAME) WHEN 0 THEN NULL ELSE @NAME END,
                                        surname = CASE LEN(@SURNAME) WHEN 0 THEN NULL ELSE @SURNAME END,
                                        contact_number = CASE LEN(@CONTACT_NUMBER) WHEN 0 THEN NULL ELSE @CONTACT_NUMBER END,
                                        email = CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
                                        plate_number = CASE LEN(@PLATE_NUMBER) WHEN 0 THEN NULL ELSE @PLATE_NUMBER END,
                                        remark = CASE LEN(@REMARK) WHEN 0 THEN NULL ELSE @REMARK END,
                                        referral_name = CASE LEN(@REFERRAL_NAME) WHEN 0 THEN NULL ELSE @REFERRAL_NAME END,
                                        referral_surname = CASE LEN(@REFERRAL_SURNAME) WHEN 0 THEN NULL ELSE @REFERRAL_SURNAME END,
                                        referral_contact_number = CASE LEN(@REFERRAL_CONTACT_NUMBER) WHEN 0 THEN NULL ELSE @REFERRAL_CONTACT_NUMBER END,
                                        referral_email = CASE LEN(@REFERRAL_EMAIL) WHEN 0 THEN NULL ELSE @REFERRAL_EMAIL END,
                                        dealer_id = CASE LEN(@DEALER) WHEN 0 THEN NULL ELSE @DEALER END,
                                        need_to_test_drive = CASE LEN(@TEST_DRIVE) WHEN 0 THEN NULL ELSE @TEST_DRIVE END

                                WHERE	id = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(name),
                        WebUtility.GetSQLTextValue(surname),
                        WebUtility.GetSQLTextValue(contact_number),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(plate_number),
                        WebUtility.GetSQLTextValue(remark),
                        WebUtility.GetSQLTextValue(referral_name),
                        WebUtility.GetSQLTextValue(referral_surname),
                        WebUtility.GetSQLTextValue(referral_contact_number),
                        WebUtility.GetSQLTextValue(referral_email),
                        WebUtility.GetSQLTextValue(dealer),
                        WebUtility.GetSQLTextValue(test_drive)
                        );

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetReportRegister(string type, string id)
        {
            DataTable table = new DataTable();
            try
            {
                string cmd = @"
                
                DECLARE @type NVARCHAR(5) = N'{0}'
                DECLARE @booking_id VARCHAR(50) = N'{1}'

                IF @booking_id = '0' --CASE ALL

                    SELECT 
                    boooking.id as ID,
                    boooking.booking_id as Booking_ID,
                    boooking.name as Name,
                    boooking.surname as Surname,
                    boooking.contact_number as Contact_number,
                    boooking.plate_number as Plate_number,
                    boooking.need_to_test_drive as Need_to_test_drive,
                    model.MODEL as Model,
                    dealer_m.display_en as Dealer,
                    boooking.remark as Remark,
                    boooking.booking_code as Booking_code,
                    boooking.referral_name as Referral_name,
                    boooking.referral_surname as Referral_surname,
                    boooking.referral_contact_number as Referral_contact_number,
                    boooking.created_user as Created_user,
                    boooking.created_device_id as Created_device_id,
                    FORMAT (boooking.created_date, 'dd-MM-yyyy hh:mm:ss') as Created_date,
                    boooking.updated_user as Updated_user,
                    FORMAT (boooking.updated_date, 'dd-MM-yyyy hh:mm:ss') as Updated_date

                    FROM booking_register boooking
                    LEFT JOIN T_CAR_MODEL model on model.MODEL_ID = boooking.preferred_model_id
                    LEFT JOIN T_DEALER dealer on dealer.DEALER_ID = boooking.dealer_id
                    LEFT JOIN T_DEALER_MASTER dealer_m on dealer.DEALER_CODE = dealer_m.dealer_code and dealer.BRANCH_CODE = dealer_m.branch_code
                    WHERE boooking.type = @type
                    ORDER BY boooking.id 

                ELSE

                    SELECT boooking.id as ID,
                    boooking.booking_id as Booking_ID,
                    boooking.name as Name,
                    boooking.surname as Surname,
                    boooking.contact_number as Contact_number,
                    boooking.plate_number as Plate_number,
                    boooking.need_to_test_drive as Need_to_test_drive,
                    model.MODEL as Model,
                    dealer_m.display_en as Dealer,
                    boooking.remark as Remark,
                    boooking.booking_code as Booking_code,
                    boooking.referral_name as Referral_name,
                    boooking.referral_surname as Referral_surname,
                    boooking.referral_contact_number as Referral_contact_number,
                    boooking.created_user as Created_user,
                    boooking.created_device_id as Created_device_id,
                    FORMAT (boooking.created_date, 'dd-MM-yyyy hh:mm:ss') as Created_date,
                    boooking.updated_user as Updated_user,
                    FORMAT (boooking.updated_date, 'dd-MM-yyyy hh:mm:ss') as Updated_date

                    FROM booking_register boooking
                    LEFT JOIN T_CAR_MODEL model on model.MODEL_ID = boooking.preferred_model_id
                    LEFT JOIN T_DEALER dealer on dealer.DEALER_ID = boooking.dealer_id
                    LEFT JOIN T_DEALER_MASTER dealer_m on dealer.DEALER_CODE = dealer_m.dealer_code and dealer.BRANCH_CODE = dealer_m.branch_code
                    where boooking.type = @type and booking_id = @booking_id
                    ORDER BY boooking.id 
                ";

                cmd = string.Format(cmd,
                    WebUtility.GetSQLTextValue(type),
                    WebUtility.GetSQLTextValue(id));

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    table = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table;
        }
    }
}