using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class ArticleService
    {
        private string conn;

        public ArticleService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceArticleModel> GetAllArticleData(string v, string p, string lang)
        {
            ServiceArticleModel value = new ServiceArticleModel();
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
                    value.data = new _ServiceArticleData();
                    value.data.article =await GetArticle(lang);

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

        private async Task<List<ArticleModel>> GetArticle(string lang)
        {
            List<ArticleModel> list = new List<ArticleModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{0}'

SELECT ID,
      topic_th AS topic,
      topic_date,
      display_start_date,
      display_end_date,
      topic_url,
      images1,
      images2,
      images3,
      images4,
      images5
FROM article
WHERE DEL_FLAG IS NULL AND is_active = 1 
    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), display_start_date, 120) AND CONVERT(NVARCHAR(10), display_end_date, 120)

ORDER BY [order]";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ArticleModel article = new ArticleModel();
                            article.id = Convert.ToInt32(row["id"]);
                            article.topic = row["topic"].ToString();
                            article.topic_date = (row["topic_date"] == DBNull.Value) ? "" : UtilityService.GetDateFormat(Convert.ToDateTime(row["topic_date"].ToString()));
                            //news.date_mmm = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("MMM");
                            //news.date_dd = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("dd");
                            article.topic_url = row["topic_url"].ToString();
                            //news.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
                            //news.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);

                            article.images = new List<string>();
                            if (!string.IsNullOrEmpty(row["images1"].ToString()))
                            {
                                article.images.Add(row["images1"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["images2"].ToString()))
                            {
                                article.images.Add(row["images2"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["images3"].ToString()))
                            {
                                article.images.Add(row["images3"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["images4"].ToString()))
                            {
                                article.images.Add(row["images4"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["images5"].ToString()))
                            {
                                article.images.Add(row["images5"].ToString());
                            }
                            list.Add(article);
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

        public async Task<ServiceArticleDetailModel> GetArticleDetail(string v, string p, string lang, int id)
        {
            ServiceArticleDetailModel value = new ServiceArticleDetailModel();
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
                    value.data = new _ServiceArticleDetailData();
                    value.data.article = await GetArticleDetail(lang, id);

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

        private async Task<ArticleModel> GetArticleDetail(string lang, int id)
        {
            ArticleModel article = new ArticleModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{0}'

SELECT ID,
      topic_th AS topic,
      topic_date,
      display_start_date,
      display_end_date,
      topic_url,
      images1,
      images2,
      images3,
      images4,
      images5
FROM article
WHERE ID = {1}";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, id)))
                    {
                        DataRow row = dt.Rows[0];

                        article.id = Convert.ToInt32(row["id"]);
                        article.topic = row["topic"].ToString();
                        article.topic_date = (row["topic_date"] == DBNull.Value) ? "" : UtilityService.GetDateFormat(Convert.ToDateTime(row["topic_date"].ToString()));
                        //news.date_mmm = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("MMM");
                        //news.date_dd = (row["Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["Date"]).ToString("dd");
                        article.topic_url = row["topic_url"].ToString();
                        //news.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
                        //news.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);

                        article.images = new List<string>();
                        if (!string.IsNullOrEmpty(row["images1"].ToString()))
                        {
                            article.images.Add(row["images1"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["images2"].ToString()))
                        {
                            article.images.Add(row["images2"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["images3"].ToString()))
                        {
                            article.images.Add(row["images3"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["images4"].ToString()))
                        {
                            article.images.Add(row["images4"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["images5"].ToString()))
                        {
                            article.images.Add(row["images5"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return article;
        }

    }
}