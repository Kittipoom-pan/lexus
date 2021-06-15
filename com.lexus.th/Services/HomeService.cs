using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class HomeService
    {
        private string conn;
        public HomeService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ServiceHomeModel> GetScreenData(string v, string p, string token)
        {
            ServiceHomeModel value = new ServiceHomeModel();
            try
            {
                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);
                    value.data = new _ServiceHomeData();
                    value.data.privileges =await GetPriviledgesOld();
                    value.data.events = GetEventsOld(token);
                    value.data.news = GetNewsOld();
                    value.data.force_logout = true;
                    value.data.call_center = "02-305-6799";
                
                    value.data.is_term = true;

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceHomeModel> GetScreenDataNew(string v, string p, string token, string lang)
        {
            ServiceHomeModel value = new ServiceHomeModel();
            try
            {
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
                    value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);
                    value.data = new _ServiceHomeData();
                    value.data.privileges =await GetPriviledges();
                    value.data.events =await GetEvents(token);
                    value.data.news =await  GetNews();
                    value.data.force_logout = true;
                    value.data.call_center = "02-305-6799";

                    value.data.is_term = true;

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<List<PrivilegeModel>> GetPriviledgesOld()
        {
            List<PrivilegeModel> list = new List<PrivilegeModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT 
ID,
TITLE,
[DESC],
DATEADD(HOUR, 7, GETDATE()) AS PERIOD,
PERIOD_START,
PERIOD_END,
IMAGE,
RED_CONDITION,
RED_PERIOD,
RED_LOCATION,
RED_EXPIRY,
PRIVILEDGE_TYPE,
IMAGE_GALLERY_COUNT,
IMAGE_GALLERY_NO,
CASE WHEN (SELECT COUNT(1) AS CNT FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = P.ID AND STATUS = 'Y' AND DEL_FLAG IS NULL) = 0 THEN 1 ELSE 0 END AS IS_OUT_OF_STOCK
FROM T_PRIVILEDGES AS P
WHERE ID = 32
";
                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            PrivilegeModel priviledge = new PrivilegeModel();
                            priviledge.id = Convert.ToInt32(row["ID"]);
                            priviledge.title = row["TITLE"].ToString();
                            priviledge.desc = row["DESC"].ToString();
                            priviledge.period = row["RED_PERIOD"].ToString();
                            priviledge.period_m = (row["PERIOD"] == DBNull.Value) ? "" : Convert.ToDateTime(row["PERIOD"]).ToString("MMMM").ToUpper();
                            priviledge.period_start = (row["PERIOD_START"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["PERIOD_START"]));
                            priviledge.period_end = (row["PERIOD_END"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["PERIOD_END"]));
                            priviledge.is_out_of_stock = Convert.ToBoolean(Convert.ToInt32(row["IS_OUT_OF_STOCK"].ToString()));

                            int image_gallery_count = int.Parse(row["IMAGE_GALLERY_COUNT"].ToString());
                            int image_gallery_no = int.Parse(row["IMAGE_GALLERY_NO"].ToString());
                            priviledge.images = new List<string>();
                            for (int i = 1; i <= image_gallery_count; i++)
                            {
                                string image_url = string.Empty;
                                image_url = string.Format("https://www.lexus-app.com/privilege_gallery/{0}", priviledge.id + "_" + i + "_" + image_gallery_no + ".jpg");
                                priviledge.images.Add(image_url);
                            }

                            priviledge.image = row["IMAGE"].ToString();
                            priviledge.privilege_type = int.Parse(row["PRIVILEDGE_TYPE"].ToString());

                            priviledge.redeem = new RedeemModel()
                            {
                                condition = row["RED_CONDITION"].ToString(),
                                period = row["RED_PERIOD"].ToString(),
                                location = row["RED_LOCATION"].ToString()
                                //expiry = row["RED_EXPIRY"].ToString()
                            };

                            list.Add(priviledge);
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

        private async Task<List<PrivilegeModel>> GetPriviledges()
        {
            List<PrivilegeModel> list = new List<PrivilegeModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT 
ID,
TITLE,
[DESC],
DATEADD(HOUR, 7, GETDATE()) AS PERIOD,
PERIOD_START,
PERIOD_END,
IMAGE,
RED_CONDITION,
RED_PERIOD,
RED_LOCATION,
RED_EXPIRY,
PRIVILEDGE_TYPE,
IMAGE_GALLERY_COUNT,
IMAGE_GALLERY_NO,
CASE WHEN (SELECT COUNT(1) AS CNT FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = P.ID AND STATUS = 'Y' AND DEL_FLAG IS NULL) = 0 THEN 1 ELSE 0 END AS IS_OUT_OF_STOCK
FROM T_PRIVILEDGES AS P
WHERE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)
    AND display_type = 0
";
                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            PrivilegeModel priviledge = new PrivilegeModel();
                            priviledge.id = Convert.ToInt32(row["ID"]);
                            priviledge.title = row["TITLE"].ToString();
                            priviledge.desc = row["DESC"].ToString();
                            priviledge.period = row["RED_PERIOD"].ToString();
                            priviledge.period_m = (row["PERIOD"] == DBNull.Value) ? "" : Convert.ToDateTime(row["PERIOD"]).ToString("MMMM").ToUpper();
                            priviledge.period_start = (row["PERIOD_START"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["PERIOD_START"]));
                            priviledge.period_end = (row["PERIOD_END"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["PERIOD_END"]));
                            priviledge.is_out_of_stock = Convert.ToBoolean(Convert.ToInt32(row["IS_OUT_OF_STOCK"].ToString()));

                            int image_gallery_count = int.Parse(row["IMAGE_GALLERY_COUNT"].ToString());
                            int image_gallery_no = int.Parse(row["IMAGE_GALLERY_NO"].ToString());
                            priviledge.images = new List<string>();
                            for (int i = 1; i <= image_gallery_count; i++)
                            {
                                string image_url = string.Empty;
                                image_url = string.Format("https://www.lexus-app.com/privilege_gallery/{0}", priviledge.id + "_" + i + "_" + image_gallery_no + ".jpg");
                                priviledge.images.Add(image_url);
                            }

                            priviledge.image = row["IMAGE"].ToString();
                            priviledge.privilege_type = int.Parse(row["PRIVILEDGE_TYPE"].ToString());

                            priviledge.redeem = new RedeemModel() {
                                condition = row["RED_CONDITION"].ToString(),
                                period = row["RED_PERIOD"].ToString(),
                                location = row["RED_LOCATION"].ToString()
                                //expiry = row["RED_EXPIRY"].ToString()
                            };

                            list.Add(priviledge);
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

        private List<EventModel> GetEventsOld(string token)
        {
            List<EventModel> list = new List<EventModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT 
ID,
IMAGES1,
IMAGES2,
IMAGES3,
IMAGES4,
IMAGES5,
TITLE,
[DATE],
[DESC],
CONDITION,
DATEADD(HOUR, 7, GETDATE()) AS REG_PERIOD,
REG_PERIOD_START,
REG_PERIOD_END,
DISPLAY_START,
DISPLAY_END,
EVENT_START,
CASE WHEN ISNULL(REG_PERIOD_END, '1800-01-01') < GETDATE() THEN 1 ELSE 0 END AS IS_EXPIRE,
CASE WHEN (SELECT COUNT(1) AS CNT FROM T_EVENTS_REGISTER WHERE EVENT_ID = E.ID AND MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')) = 1 THEN 0 ELSE 1 END AS IS_REGISTER
FROM T_EVENTS AS E
WHERE ID = 15";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            EventModel evt = new EventModel();
                            evt.id = Convert.ToInt32(row["ID"]);
                            evt.title = row["TITLE"].ToString();
                            evt.date = (row["EVENT_START"] == DBNull.Value) ? "" : UtilityService.GetDateFormat(Convert.ToDateTime(row["EVENT_START"]));
                            evt.time = (row["EVENT_START"] == DBNull.Value) ? "" : UtilityService.GetTimeFormat(Convert.ToDateTime(row["EVENT_START"]));
                            evt.desc = row["DESC"].ToString();
                            evt.condition = row["CONDITION"].ToString();
                            evt.reg_period = (row["REG_PERIOD"] == DBNull.Value) ? "" : UtilityService.GetDateFormat(Convert.ToDateTime(row["REG_PERIOD"]));
                            evt.reg_period_start = (row["REG_PERIOD_START"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["REG_PERIOD_START"]));
                            evt.reg_period_end = (row["REG_PERIOD_END"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["REG_PERIOD_END"]));
                            //evt.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
                            //evt.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);
                            evt.is_expire = Convert.ToBoolean(Convert.ToInt32(row["IS_EXPIRE"].ToString()));
                            evt.is_register = Convert.ToBoolean(Convert.ToInt32(row["IS_REGISTER"].ToString()));

                            evt.images = new List<string>();
                            if (!string.IsNullOrEmpty(row["IMAGES1"].ToString()))
                            {
                                evt.images.Add(row["IMAGES1"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES2"].ToString()))
                            {
                                evt.images.Add(row["IMAGES2"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES3"].ToString()))
                            {
                                evt.images.Add(row["IMAGES3"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES4"].ToString()))
                            {
                                evt.images.Add(row["IMAGES4"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES5"].ToString()))
                            {
                                evt.images.Add(row["IMAGES5"].ToString());
                            }

                            list.Add(evt);
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

        private async Task<List<EventModel>> GetEvents(string token)
        {
            List<EventModel> list = new List<EventModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT 
ID,
IMAGES1,
IMAGES2,
IMAGES3,
IMAGES4,
IMAGES5,
TITLE,
[DATE],
[DESC],
CONDITION,
DATEADD(HOUR, 7, GETDATE()) AS REG_PERIOD,
REG_PERIOD_START,
REG_PERIOD_END,
DISPLAY_START,
DISPLAY_END,
EVENT_START,
CASE WHEN ISNULL(REG_PERIOD_END, '1800-01-01') < GETDATE() THEN 1 ELSE 0 END AS IS_EXPIRE,
CASE WHEN (SELECT COUNT(1) AS CNT FROM T_EVENTS_REGISTER WHERE EVENT_ID = E.ID AND MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')) = 1 THEN 0 ELSE 1 END AS IS_REGISTER
FROM T_EVENTS AS E
WHERE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            EventModel evt = new EventModel();
                            evt.id = Convert.ToInt32(row["ID"]);
                            evt.title = row["TITLE"].ToString();
                            evt.date = (row["EVENT_START"] == DBNull.Value) ? "" : UtilityService.GetDateFormat(Convert.ToDateTime(row["EVENT_START"]));
                            evt.time = (row["EVENT_START"] == DBNull.Value) ? "" : UtilityService.GetTimeFormat(Convert.ToDateTime(row["EVENT_START"]));
                            evt.desc = row["DESC"].ToString();
                            evt.condition = row["CONDITION"].ToString();
                            evt.reg_period = (row["REG_PERIOD"] == DBNull.Value) ? "" : UtilityService.GetDateFormat(Convert.ToDateTime(row["REG_PERIOD"]));
                            evt.reg_period_start = (row["REG_PERIOD_START"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["REG_PERIOD_START"]));
                            evt.reg_period_end = (row["REG_PERIOD_END"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["REG_PERIOD_END"]));
                            //evt.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
                            //evt.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);
                            evt.is_expire = Convert.ToBoolean(Convert.ToInt32(row["IS_EXPIRE"].ToString()));
                            evt.is_register = Convert.ToBoolean(Convert.ToInt32(row["IS_REGISTER"].ToString()));

                            evt.images = new List<string>();
                            if (!string.IsNullOrEmpty(row["IMAGES1"].ToString()))
                            {
                                evt.images.Add(row["IMAGES1"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES2"].ToString()))
                            {
                                evt.images.Add(row["IMAGES2"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES3"].ToString()))
                            {
                                evt.images.Add(row["IMAGES3"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES4"].ToString()))
                            {
                                evt.images.Add(row["IMAGES4"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES5"].ToString()))
                            {
                                evt.images.Add(row["IMAGES5"].ToString());
                            }

                            list.Add(evt);
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
        private List<NewsModel> GetNewsOld()
        {
            List<NewsModel> list = new List<NewsModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT ID,
      TITLE,
      [DATE],
      [DESC],
      IMAGES1,
      IMAGES2,
      IMAGES3,
      IMAGES4,
      IMAGES5,
      DISPLAY_START,
      DISPLAY_END
FROM T_NEWS
WHERE ID = 21";

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            NewsModel news = new NewsModel();
                            news.id = Convert.ToInt32(row["ID"]);
                            news.title = row["TITLE"].ToString();
                            news.date = (row["Date"] == DBNull.Value) ? "" : UtilityService.GetDateFormat(Convert.ToDateTime(row["Date"].ToString()));
                            news.date_mmm = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("MMM");
                            news.date_dd = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("dd");
                            news.desc = row["DESC"].ToString();
                            //news.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
                            //news.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);

                            news.images = new List<string>();
                            if (!string.IsNullOrEmpty(row["IMAGES1"].ToString()))
                            {
                                news.images.Add(row["IMAGES1"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES2"].ToString()))
                            {
                                news.images.Add(row["IMAGES2"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES3"].ToString()))
                            {
                                news.images.Add(row["IMAGES3"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES4"].ToString()))
                            {
                                news.images.Add(row["IMAGES4"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES5"].ToString()))
                            {
                                news.images.Add(row["IMAGES5"].ToString());
                            }

                            list.Add(news);
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

        private async Task<List<NewsModel>> GetNews()
        {
            List<NewsModel> list = new List<NewsModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT ID,
      TITLE,
      [DATE],
      [DESC],
      IMAGES1,
      IMAGES2,
      IMAGES3,
      IMAGES4,
      IMAGES5,
      DISPLAY_START,
      DISPLAY_END
FROM T_NEWS
WHERE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)
ORDER BY [DATE] DESC";

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            NewsModel news = new NewsModel();
                            news.id = Convert.ToInt32(row["ID"]);
                            news.title = row["TITLE"].ToString();
                            news.date = (row["Date"] == DBNull.Value) ? "" : UtilityService.GetDateFormat(Convert.ToDateTime(row["Date"].ToString()));
                            news.date_mmm = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("MMM");
                            news.date_dd = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("dd");
                            news.desc = row["DESC"].ToString();
                            //news.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
                            //news.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);

                            news.images = new List<string>();
                            if (!string.IsNullOrEmpty(row["IMAGES1"].ToString()))
                            {
                                news.images.Add(row["IMAGES1"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES2"].ToString()))
                            {
                                news.images.Add(row["IMAGES2"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES3"].ToString()))
                            {
                                news.images.Add(row["IMAGES3"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES4"].ToString()))
                            {
                                news.images.Add(row["IMAGES4"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["IMAGES5"].ToString()))
                            {
                                news.images.Add(row["IMAGES5"].ToString());
                            }

                            list.Add(news);
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