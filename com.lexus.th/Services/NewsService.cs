using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
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

        public async Task<ServiceNewsModel> GetScreenData(string v, string p, string news_id)
        {
            ServiceNewsModel value = new ServiceNewsModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceNewsData();
                    value.data.news =await GetNews(news_id, "EN");

                    ValidationModel.InvalidState state;
                    if (value.data.news.id != 0)
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        state = ValidationModel.InvalidState.E508;
                        value.success = false;
                        value.msg = new MsgModel() { code = ValidationModel.GetInvalidCode(state), text = ValidationModel.GetInvalidMessage(state), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceNewsModel> GetScreenDataNew(string v, string p, string news_id, string lang)
        {
            ServiceNewsModel value = new ServiceNewsModel();
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
                    value.data = new _ServiceNewsData();
                    value.data.news = await GetNews(news_id, lang);

                    ValidationModel.InvalidState state;
                    if (value.data.news.id != 0)
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        state = ValidationModel.InvalidState.E508;
                        value.success = false;
                        value.msg = new MsgModel() { code = ValidationModel.GetInvalidCode(state), text = await ValidationModel.GetInvalidMessageNew(state, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }


        private async Task<NewsModel> GetNews(string news_id, string lang)
        {
            NewsModel news = new NewsModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @LANG NVARCHAR(5) = N'{1}'
                           
                           SELECT ID,
                                 CASE  
                                 WHEN @LANG = 'EN' THEN [TITLE] 
                                 ELSE TITLE END AS [TITLE],
                                 CASE  
                                 WHEN @LANG = 'EN' THEN [DESC] 
                                 ELSE desc_th END AS [DESC],
                                 [DATE],
                                 DISPLAY_START,
                                 DISPLAY_END
                           FROM T_NEWS
                           WHERE DEL_FLAG IS NULL AND is_active = 1 
                           AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), DISPLAY_START, 120) AND CONVERT(NVARCHAR(10), DISPLAY_END, 120)
                           AND ID = {0}
                           ORDER BY [DATE] DESC";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, news_id, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            news = new NewsModel();
                            news.id = Convert.ToInt32(row["ID"]);
                            news.title = row["TITLE"].ToString();
                            news.date = (row["Date"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["Date"].ToString()));
                            //news.date_mmm = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("MMM");
                            //news.date_dd = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("dd");
                            news.desc = row["DESC"].ToString();
                            //news.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
                            //news.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);

                            news.images = new List<string>();
                            news.images = GetAllNewsPicture(Convert.ToInt32(row["ID"]));
                            //if (!string.IsNullOrEmpty(row["IMAGES1_1"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES1_1"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES2_1"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES2_1"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES3_1"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES3_1"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES4_1"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES4_1"].ToString());
                            //}
                            //if (!string.IsNullOrEmpty(row["IMAGES5_1"].ToString()))
                            //{
                            //    news.images.Add(row["IMAGES5_1"].ToString());
                            //}

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return news;
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
                             AND upload_Image.Type = 'DETAIL' 
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
    }
}