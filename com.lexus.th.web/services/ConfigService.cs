using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class ConfigService
    {
        private string conn;
        public ConfigService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataRow GetAllConfig()
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @otp_timing VARCHAR(250)
DECLARE @otp_maximum_input VARCHAR(250)
DECLARE @call_center_contact VARCHAR(250)
DECLARE @call_center_email VARCHAR(250)
DECLARE @picture_guest VARCHAR(250)
DECLARE @picture_blank_banner VARCHAR(250)
DECLARE @picture_authorized_dealer VARCHAR(250)
DECLARE @picture_authorized_service_dealer VARCHAR(250)
DECLARE @picture_blank_article VARCHAR(250)
DECLARE @picture_upcoming VARCHAR(250)
DECLARE @picture_news VARCHAR(250)
DECLARE @picture_blank_upcoming VARCHAR(250)
DECLARE @picture_blank_news VARCHAR(250)
DECLARE @article_banner VARCHAR(250)
DECLARE @service_appointment_banner VARCHAR(250)
DECLARE @auto_slide_time VARCHAR(250)
DECLARE @push_remaining_day VARCHAR(250)
DECLARE @car_booking_banner VARCHAR(250)


SET @otp_timing = (SELECT data_config
											 FROM system_config
											 WHERE name = 'otp_timing')
SET @otp_maximum_input = (SELECT data_config
											 FROM system_config
											 WHERE name = 'otp_maximum_input')
SET @call_center_contact = (SELECT data_config
											 FROM system_config
											 WHERE name = 'call_center_contact')
SET @call_center_email = (SELECT data_config
											 FROM system_config
											 WHERE name = 'call_center_email')

SET @picture_guest = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_guest')
SET @picture_blank_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_blank_banner')
SET @picture_authorized_dealer = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_authorized_dealer')
SET @picture_authorized_service_dealer = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_authorized_service_dealer')
SET @picture_blank_article = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_blank_article')
SET @picture_upcoming = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_upcoming')
SET @picture_news = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_news')
SET @picture_blank_upcoming = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_blank_upcoming')
SET @picture_blank_news = (SELECT data_config
											 FROM system_config
											 WHERE name = 'picture_blank_news')
SET @article_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'article_banner')
SET @service_appointment_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'service_appointment_banner')
SET @auto_slide_time = (SELECT data_config
											 FROM system_config
											 WHERE name = 'auto_slide_time')
SET @push_remaining_day = (SELECT data_config
											 FROM system_config
											 WHERE name = 'push_remaining_day')
SET @car_booking_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'car_booking_banner')

SELECT @otp_timing otp_timing, @otp_maximum_input otp_maximum_input, @call_center_contact call_center_contact, @call_center_email call_center_email,
    @picture_guest picture_guest, 
    @picture_blank_banner picture_blank_banner,
    @picture_authorized_dealer picture_authorized_dealer, @picture_authorized_service_dealer picture_authorized_service_dealer,
    @picture_blank_article picture_blank_article, @picture_upcoming picture_upcoming, @picture_news picture_news,
    @picture_blank_upcoming picture_blank_upcoming, @picture_blank_news picture_blank_news, @article_banner article_banner, 
    @service_appointment_banner service_appointment_banner, @auto_slide_time auto_slide_time, 
    @push_remaining_day push_remaining_day,
    @car_booking_banner car_booking_banner";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(id));

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

        public void UpdateAllConfig(string picture_authorized_dealer, string picture_authorized_service_dealer, string picture_upcoming, string picture_news, string user, string call_center_contact, string call_center_email, string article_banner, string service_appointment_banner, string car_booking_banner, Double auto_slide_time,int push_remaining_day)
        {
            try
            {
                string cmd = @"
DECLARE @picture_authorized_dealer  VARCHAR(250) = N'{0}'
DECLARE @picture_authorized_service_dealer  VARCHAR(250) = N'{1}'
DECLARE @picture_upcoming  VARCHAR(250) = N'{2}'
DECLARE @picture_news  VARCHAR(250) = N'{3}'
DECLARE @user  NVARCHAR(50) = N'{4}'
DECLARE @call_center_contact  NVARCHAR(50) = N'{5}'
DECLARE @call_center_email  NVARCHAR(250) = N'{6}'
DECLARE @article_banner  NVARCHAR(50) = N'{7}'
DECLARE @service_appointment_banner  NVARCHAR(50) = N'{8}'
DECLARE @auto_slide_time  NVARCHAR(50) = N'{9}'
DECLARE @push_remaining_day  NVARCHAR(3) = N'{10}'
DECLARE @car_booking_banner  NVARCHAR(50) = N'{11}'

UPDATE		system_config
SET			data_config = CASE LEN(@picture_authorized_dealer) WHEN 0 THEN NULL ELSE @picture_authorized_dealer END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'picture_authorized_dealer'
UPDATE		system_config
SET			data_config = CASE LEN(@picture_authorized_service_dealer) WHEN 0 THEN NULL ELSE @picture_authorized_service_dealer END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'picture_authorized_service_dealer'
UPDATE		system_config
SET			data_config = CASE LEN(@picture_upcoming) WHEN 0 THEN NULL ELSE @picture_upcoming END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'picture_upcoming'
UPDATE		system_config
SET			data_config = CASE LEN(@picture_news) WHEN 0 THEN NULL ELSE @picture_news END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'picture_news'
UPDATE		system_config
SET			data_config = CASE LEN(@call_center_contact) WHEN 0 THEN NULL ELSE @call_center_contact END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'call_center_contact'
UPDATE		system_config
SET			data_config = CASE LEN(@call_center_email) WHEN 0 THEN NULL ELSE @call_center_email END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'call_center_email'
UPDATE		system_config
SET			data_config = CASE LEN(@article_banner) WHEN 0 THEN NULL ELSE @article_banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'article_banner'
UPDATE		system_config
SET			data_config = CASE LEN(@service_appointment_banner) WHEN 0 THEN NULL ELSE @service_appointment_banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'service_appointment_banner'
UPDATE		system_config
SET			data_config = CASE LEN(@auto_slide_time) WHEN 0 THEN NULL ELSE @auto_slide_time END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'auto_slide_time'

UPDATE		system_config
SET			data_config = CASE LEN(@push_remaining_day) WHEN 0 THEN NULL ELSE @push_remaining_day END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'push_remaining_day'
UPDATE		system_config
SET			data_config = CASE LEN(@car_booking_banner) WHEN 0 THEN NULL ELSE @car_booking_banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'car_booking_banner'";                

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(picture_authorized_dealer),
                        WebUtility.GetSQLTextValue(picture_authorized_service_dealer),
                        WebUtility.GetSQLTextValue(picture_upcoming),
                        WebUtility.GetSQLTextValue(picture_news),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(call_center_contact),
                        WebUtility.GetSQLTextValue(call_center_email),
                        WebUtility.GetSQLTextValue(article_banner),
                        WebUtility.GetSQLTextValue(service_appointment_banner),
                        WebUtility.GetSQLTextValue(auto_slide_time.ToString()),
                        WebUtility.GetSQLTextValue(push_remaining_day.ToString()),
                        WebUtility.GetSQLTextValue(car_booking_banner));
                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetConfigRoadsite()
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @roadside_banner NVARCHAR(250)
DECLARE @roadside_contact NVARCHAR(250)
SET @roadside_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'roadside_banner')
SET @roadside_contact = (SELECT data_config
											 FROM system_config
											 WHERE name = 'roadside_contact')

SELECT @roadside_banner roadside_banner, @roadside_contact roadside_contact";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(id));

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

        public void UpdateConfigRoadsite(string roadside_banner, string roadside_contact, string user)
        {
            try
            {
                string cmd = @"
DECLARE @banner  NVARCHAR(250) = N'{0}'
DECLARE @contact  NVARCHAR(250) = N'{1}'
DECLARE @user  NVARCHAR(50) = N'{2}'

UPDATE		system_config
SET			data_config = CASE LEN(@banner) WHEN 0 THEN NULL ELSE @banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'roadside_banner'

UPDATE		system_config
SET			data_config = CASE LEN(@contact) WHEN 0 THEN NULL ELSE @contact END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'roadside_contact'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(roadside_banner),
                        WebUtility.GetSQLTextValue(roadside_contact),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetConfigECatalogue()
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @e_catalog_banner NVARCHAR(250)
DECLARE @e_catalog_url_address NVARCHAR(250)
SET @e_catalog_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'e_catalog_banner')
SET @e_catalog_url_address = (SELECT data_config
											 FROM system_config
											 WHERE name = 'e_catalog_url_address')

SELECT @e_catalog_banner e_catalog_banner, @e_catalog_url_address e_catalog_url_address";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(id));

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

        public void UpdateConfigECatalogue(string e_catalog_banner, string e_catalog_url_address, string user)
        {
            try
            {
                string cmd = @"
DECLARE @banner  NVARCHAR(250) = N'{0}'
DECLARE @url_address  NVARCHAR(250) = N'{1}'
DECLARE @user  NVARCHAR(50) = N'{2}'

UPDATE		system_config
SET			data_config = CASE LEN(@banner) WHEN 0 THEN NULL ELSE @banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'e_catalog_banner'

UPDATE		system_config
SET			data_config = CASE LEN(@url_address) WHEN 0 THEN NULL ELSE @url_address END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'e_catalog_url_address'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(e_catalog_banner),
                        WebUtility.GetSQLTextValue(e_catalog_url_address),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetConfigPaymentCalculator()
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @payment_calculator_banner NVARCHAR(250)
DECLARE @payment_calculator_url_address NVARCHAR(250)
SET @payment_calculator_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'payment_calculator_banner')
SET @payment_calculator_url_address = (SELECT data_config
											 FROM system_config
											 WHERE name = 'payment_calculator_url_address')

SELECT @payment_calculator_banner payment_calculator_banner, @payment_calculator_url_address payment_calculator_url_address";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(id));

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

        public void UpdateConfigPaymentCalculator(string payment_calculator_banner, string payment_calculator_url_address, string user)
        {
            try
            {
                string cmd = @"
DECLARE @banner  NVARCHAR(250) = N'{0}'
DECLARE @url_address  NVARCHAR(250) = N'{1}'
DECLARE @user  NVARCHAR(50) = N'{2}'

UPDATE		system_config
SET			data_config = CASE LEN(@banner) WHEN 0 THEN NULL ELSE @banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'payment_calculator_banner'

UPDATE		system_config
SET			data_config = CASE LEN(@url_address) WHEN 0 THEN NULL ELSE @url_address END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'payment_calculator_url_address'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(payment_calculator_banner),
                        WebUtility.GetSQLTextValue(payment_calculator_url_address),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetConfigTestDrive()
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @test_drive_banner NVARCHAR(250)
DECLARE @test_drive_url_address NVARCHAR(250)
SET @test_drive_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'test_drive_banner')
SET @test_drive_url_address = (SELECT data_config
											 FROM system_config
											 WHERE name = 'test_drive_url_address')

SELECT @test_drive_banner test_drive_banner, @test_drive_url_address test_drive_url_address";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(id));

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

        public void UpdateConfigTestDrive(string test_drive_banner, string test_drive_url_address, string user)
        {
            try
            {
                string cmd = @"
DECLARE @banner  NVARCHAR(250) = N'{0}'
DECLARE @url_address  NVARCHAR(250) = N'{1}'
DECLARE @user  NVARCHAR(50) = N'{2}'

UPDATE		system_config
SET			data_config = CASE LEN(@banner) WHEN 0 THEN NULL ELSE @banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'test_drive_banner'

UPDATE		system_config
SET			data_config = CASE LEN(@url_address) WHEN 0 THEN NULL ELSE @url_address END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'test_drive_url_address'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(test_drive_banner),
                        WebUtility.GetSQLTextValue(test_drive_url_address),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetConfigPriceList()
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @pricelist_banner NVARCHAR(250)
DECLARE @pricelist_url_address NVARCHAR(250)
SET @pricelist_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'pricelist_banner')
SET @pricelist_url_address = (SELECT data_config
											 FROM system_config
											 WHERE name = 'pricelist_url_address')

SELECT @pricelist_banner pricelist_banner, @pricelist_url_address pricelist_url_address";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(id));

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

        public DataRow GetConfigCalculator()
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @payment_calculator_banner NVARCHAR(250)
DECLARE @payment_calculator_url_address NVARCHAR(250)
SET @payment_calculator_banner = (SELECT data_config
											 FROM system_config
											 WHERE name = 'payment_calculator_banner')
SET @payment_calculator_url_address = (SELECT data_config
											 FROM system_config
											 WHERE name = 'payment_calculator_url_address')

SELECT @payment_calculator_banner payment_calculator_banner, @payment_calculator_url_address payment_calculator_url_address";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(id));

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

        public void UpdateConfigPriceList(string pricelist_banner, string pricelist_url_address, string user)
        {
            try
            {
                string cmd = @"
DECLARE @banner  NVARCHAR(250) = N'{0}'
DECLARE @url_address  NVARCHAR(250) = N'{1}'
DECLARE @user  NVARCHAR(50) = N'{2}'

UPDATE		system_config
SET			data_config = CASE LEN(@banner) WHEN 0 THEN NULL ELSE @banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'pricelist_banner'

UPDATE		system_config
SET			data_config = CASE LEN(@url_address) WHEN 0 THEN NULL ELSE @url_address END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'pricelist_url_address'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(pricelist_banner),
                        WebUtility.GetSQLTextValue(pricelist_url_address),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateConfigCalculator(string payment_calculator_banner, string payment_calculator_url_address, string user)
        {
            try
            {
                string cmd = @"
DECLARE @banner  NVARCHAR(250) = N'{0}'
DECLARE @url_address  NVARCHAR(250) = N'{1}'
DECLARE @user  NVARCHAR(50) = N'{2}'

UPDATE		system_config
SET			data_config = CASE LEN(@banner) WHEN 0 THEN NULL ELSE @banner END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'payment_calculator_banner'

UPDATE		system_config
SET			data_config = CASE LEN(@url_address) WHEN 0 THEN NULL ELSE @url_address END,
			update_date = DATEADD(HOUR, 7, GETDATE()),
			update_by = @USER
WHERE		[name] = 'payment_calculator_url_address'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(payment_calculator_banner),
                        WebUtility.GetSQLTextValue(payment_calculator_url_address),
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