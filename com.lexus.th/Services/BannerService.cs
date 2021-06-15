using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class BannerService
    {
        private string conn;

        public BannerService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceAllBannerModel> GetAllBanner(string v, string p, string lang)
        {
            ServiceAllBannerModel value = new ServiceAllBannerModel();
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
                    value.data = new _ServiceAllBannerData();
                    value.data.banners =await GetAllDisplayBanner();

                    ValidationModel.InvalidState state;
                    if (value.data.banners.Count > 0)
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        state = ValidationModel.InvalidState.E506;
                        value.success = false;
                        value.msg = new MsgModel() { code = ValidationModel.GetInvalidCode(state), text =await ValidationModel.GetInvalidMessageNew(state, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<List<BannerModel>> GetAllDisplayBanner()
        {
            List<BannerModel> list = new List<BannerModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT b.id,b.type,b.[order],e.TITLE,b.action_id,b.image_url
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN T_EVENTS e ON b.action_id = e.ID
WHERE		b.DEL_FLAG IS NULL AND b.type = 'Events' 
	AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), b.start_date, 120) AND CONVERT(NVARCHAR(20), b.end_date, 120)

UNION
SELECT b.id,b.type,b.[order],n.TITLE,b.action_id,b.image_url
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN T_NEWS n ON b.action_id = n.ID
WHERE		b.DEL_FLAG IS NULL AND b.type = 'News' 
	AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), b.start_date, 120) AND CONVERT(NVARCHAR(20), b.end_date, 120)
UNION

SELECT b.id,b.type,b.[order],p.TITLE,b.action_id,b.image_url
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN T_PRIVILEDGES p ON b.action_id = p.ID
WHERE		b.DEL_FLAG IS NULL 
	AND b.type = 'Privilege' AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), b.start_date, 120) AND CONVERT(NVARCHAR(20), b.end_date, 120)
UNION

SELECT b.id,b.type,b.[order],'',b.action_id,b.image_url
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b 
WHERE		b.DEL_FLAG IS NULL 
	AND b.type = 'Main_Online_Booking' AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), b.start_date, 120) AND CONVERT(NVARCHAR(20), b.end_date, 120)

UNION

SELECT b.id,b.type,b.[order],bk.title_en COLLATE SQL_Latin1_General_CP1_CI_AS AS TITLE,b.action_id,b.image_url
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN booking bk ON b.action_id = bk.ID
WHERE		b.DEL_FLAG IS NULL 
	AND b.type = 'Online_Booking_Repurchase' AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), b.start_date, 120) AND CONVERT(NVARCHAR(20), b.end_date, 120)

UNION

SELECT b.id,b.type,b.[order],bk.title_en COLLATE SQL_Latin1_General_CP1_CI_AS AS TITLE,b.action_id,b.image_url
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN booking bk ON b.action_id = bk.ID
WHERE		b.DEL_FLAG IS NULL 
	AND b.type = 'Online_Booking_Referral' AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), b.start_date, 120) AND CONVERT(NVARCHAR(20), b.end_date, 120)

UNION

SELECT b.id,b.type,b.[order],bk.title_en COLLATE SQL_Latin1_General_CP1_CI_AS AS TITLE,b.action_id,b.image_url
,b.start_date,b.end_date,b.create_date,b.create_by
,b.update_date,b.update_by
FROM		banner b LEFT JOIN booking bk ON b.action_id = bk.ID
WHERE		b.DEL_FLAG IS NULL 
	AND b.type = 'Online_Booking_Guest' AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), b.start_date, 120) AND CONVERT(NVARCHAR(20), b.end_date, 120)
ORDER BY [order]
";
                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            BannerModel banner = new BannerModel();
                            banner.id = Convert.ToInt32(row["id"]);
                            banner.title = row["TITLE"].ToString();
                            banner.image = row["image_url"].ToString();
                            banner.type = row["type"].ToString();
                            banner.action_id = Convert.ToInt32(row["action_id"].ToString());
                            banner.order = Convert.ToInt32(row["order"].ToString());
                            if (banner.type == "Main_Online_Booking")
                            {
                                banner.title = System.Web.Configuration.WebConfigurationManager.AppSettings["banner_online_booking_text"];
                            }

                            list.Add(banner);
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
    }
}