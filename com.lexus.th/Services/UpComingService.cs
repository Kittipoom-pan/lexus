using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace com.lexus.th
{
    public class UpComingService
    {
        private string conn;

        public UpComingService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceAllUpComingModel> GetAllUpComing(string v, string p, string lang)
        {
            ServiceAllUpComingModel value = new ServiceAllUpComingModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceAllUpComingData();
                    value.data.Upcoming =await GetAllDisplayUpComing();
                    value.data.news = GetNews(lang);

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

        private async Task<List<UpcomingModel>> GetAllDisplayUpComing()
        {
            List<UpcomingModel> list = new List<UpcomingModel>();
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           SELECT up.id, e.TITLE, up.type, up.action_id, up.[order], CONVERT(datetime, e.DATE) AS create_date
                           FROM up_coming up LEFT JOIN T_EVENTS e ON up.action_id = e.ID
                           WHERE up.DEL_FLAG IS NULL AND up.type = 'Events'
                           	AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), up.start_date, 120) 
                            AND CONVERT(NVARCHAR(20), up.end_date, 120)
                           UNION
                           SELECT up.id, n.TITLE, up.type, up.action_id, up.[order], CONVERT(datetime, n.DATE) AS create_date
                           FROM up_coming up LEFT JOIN T_NEWS n ON up.action_id = n.ID
                           WHERE up.DEL_FLAG IS NULL AND up.type = 'News'
                           	AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), up.start_date, 120) 
                            AND CONVERT(NVARCHAR(20), up.end_date, 120)
                           ORDER BY up.[order], create_date DESC";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            UpcomingModel banner = new UpcomingModel();
                            banner.id = Convert.ToInt32(row["id"]);
                            banner.title = row["title"].ToString();
                            banner.type = row["type"].ToString();
                            banner.action_id = Convert.ToInt32(row["action_id"].ToString());
                            banner.order = Convert.ToInt32(row["order"].ToString());
                            //banner.image_gallery_count = Convert.ToInt32(row["image_gallery_count"].ToString());
                            banner.create_date = (row["create_date"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["create_date"])).ToString();
                            banner.images = GetAllUpComingPicture(Convert.ToInt32(row["id"]));

                            //Old Can use --------------------------------------
                            //if (!string.IsNullOrEmpty(row["IMAGES1"].ToString()))
                            //{
                            //    banner.images.Add(row["IMAGES1"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES2"].ToString()))
                            //{
                            //    banner.images.Add(row["IMAGES2"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES3"].ToString()))
                            //{
                            //    banner.images.Add(row["IMAGES3"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES4"].ToString()))
                            //{
                            //    banner.images.Add(row["IMAGES4"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES5"].ToString()))
                            //{
                            //    banner.images.Add(row["IMAGES5"].ToString());
                            //}
                            //END Old Can use --------------------------------------

                            //banner.image = new List<string>();
                            //string image_url = string.Empty;
                            //for (int i = 1; i <= banner.image_gallery_count; i++)
                            //{
                            //    image_url = string.Empty;
                            //    image_url = string.Format(WebConfigurationManager.AppSettings["upcoming_gallery_path"], banner.id + "_" + i + ".jpg");
                            //    banner.image.Add(image_url);
                            //}
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

        private List<NewsModel> GetNews(string lang)
        {
            List<NewsModel> list = new List<NewsModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @LANG NVARCHAR(5) = N'{0}'
                           
                           SELECT n.ID,
                                   CASE  
                             WHEN @LANG = 'EN' THEN n.TITLE 
                             ELSE n.TITLE END AS TITLE,
                                   CASE  
                             WHEN @LANG = 'EN' THEN n.[DESC]
                             ELSE n.desc_th END AS [DESC],
                                 n.[DATE],
                                 --n.IMAGES1,
                                 --n.IMAGES2,
                                 --n.IMAGES3,
                                 --n.IMAGES4,
                                 --n.IMAGES5,
                                 nc.start_date AS DISPLAY_START,
                                 nc.end_date AS DISPLAY_END
                           FROM new_control nc LEFT JOIN T_NEWS n ON nc.action_id = n.ID
                           WHERE nc.DEL_FLAG IS NULL AND nc.type = 'News'
                           	AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), nc.start_date, 120) AND CONVERT(NVARCHAR(20), nc.end_date, 120)
                           ORDER BY nc.[order], n.[DATE] DESC";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang)))
                    {
                        foreach (DataRow row in dt.Rows) 
                        {
                            NewsModel news = new NewsModel();
                            news.id = Convert.ToInt32(row["ID"]);
                            news.title = row["TITLE"].ToString();                       
                            news.date = (row["Date"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["Date"].ToString()));
                            //news.date_mmm = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("MMM");
                            //news.date_dd = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("dd");
                            news.desc = row["DESC"].ToString();
                            //news.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
                            //news.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);

                            news.images = new List<string>();

                            news.images = GetAllNewsPicture(Convert.ToInt32(row["id"]));


                            //if (!string.IsNullOrEmpty(row["IMAGES1"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES1"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES2"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES2"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES3"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES3"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES4"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES4"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES5"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES5"].ToString());
                            //}

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


        public List<string> GetAllUpComingPicture(int upcomming_id)
        {
            List<string> list = new List<string>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           SELECT upload.File_Path AS path FROM up_coming 
						   LEFT JOIN upload_Image AS upload ON upload.Parent_Id = up_coming.action_id AND up_coming.type = upload.Page
						   WHERE up_coming.DEL_FLAG IS NULL AND upload.Type = 'BANNER' AND up_coming.id = N'{0}' 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, upcomming_id)))
                    {
                        string picture = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            picture = row["path"].ToString();
                            list.Add(picture);
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

        public List<string> GetAllNewsPicture(int news_id)
        {
            List<string> list = new List<string>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           SELECT File_Path AS path
						   FROM upload_Image 
						   INNER JOIN new_control ON new_control.type = upload_Image.Page 
						   WHERE new_control.action_id = N'{0}'
						     AND new_control.Type ='News' 
                             AND upload_Image.Type = 'BANNER' 
                             AND upload_Image.Parent_Id = N'{0}'
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, news_id)))
                    {
                        string picture = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            picture = row["path"].ToString();
                            list.Add(picture);
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

        //public List<string> GetAllNewPicture()
        //{
        //    List<string> list = new List<string>();
        //    try
        //    {
        //        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //        {
        //            string cmd = @"
        //                   SELECT upload.File_Path AS path FROM up_coming 
        // LEFT JOIN upload_Image AS upload ON upload.Parent_Id = up_coming.action_id AND up_coming.type = upload.Page
        // WHERE up_coming.DEL_FLAG IS NULL AND upload.Type = 'BANNER' AND up_coming.id = N'{0}' 
        //                 ";

        //            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, upcomming_id)))
        //            {
        //                string picture = "";
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    picture = row["path"].ToString();
        //                    list.Add(picture);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //    return list;
        //}
    }
}