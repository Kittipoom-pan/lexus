using AppLibrary.Database;
using AppLibrary.WebHelper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web.Configuration;

namespace com.lexus.th.web
{
    public class ManageDeeplinkService 
    {
        private string conn;
        public ManageDeeplinkService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetManageDeeplink()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
SELECT		id
            ,name
            ,short_link
            ,page
            ,reference_id
            ,redirect_link
FROM		DynamicLink
WHERE       deleted_flag IS NULL
            ORDER BY id DESC"; 

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(searchValue));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public string AddManageDeepLink(string name, string short_link, string page, string reference_id,string link,string created_date, string user)
        {
            string result = "";
            try
            {
                string cmd = @"
DECLARE @name  NVARCHAR(250) = N'{0}'
DECLARE @short_link  NVARCHAR(250) = N'{1}'
DECLARE @page  NVARCHAR(150) = N'{2}'
DECLARE @reference_id  INT = N'{3}'
DECLARE @redirect_link  NVARCHAR(255) = N'{4}'
DECLARE @created_date  NVARCHAR(50) = N'{5}'
DECLARE @USER  NVARCHAR(50) = N'{6}'


INSERT INTO DynamicLink (name, short_link, page, reference_id, redirect_link, created_date,created_user)
VALUES (CASE LEN(@name) WHEN 0 THEN NULL ELSE @name END,
		CASE LEN(@short_link) WHEN 0 THEN NULL ELSE @short_link END,
		CASE LEN(@page) WHEN 0 THEN NULL ELSE @page END,
		CASE LEN(@reference_id) WHEN 0 THEN NULL ELSE @reference_id END,
        CASE LEN(@redirect_link) WHEN 0 THEN NULL ELSE @redirect_link END,
		CASE LEN(@created_date) WHEN 0 THEN NULL ELSE @created_date END,
        @USER)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(name),
                        WebUtility.GetSQLTextValue(short_link),
                        WebUtility.GetSQLTextValue(page),
                        WebUtility.GetSQLTextValue(reference_id),
                        WebUtility.GetSQLTextValue(link),
                        WebUtility.GetSQLTextValue(created_date),
                        WebUtility.GetSQLTextValue(user));


                    //dr = db.GetDataTableFromCommandText(cmd).AsEnumerable().FirstOrDefault();
                    result = db.ExecuteScalarFromCommandText<string>(cmd);
                    //result = dr[0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public void CreateDeeplink(DeeplinkRequest request, Action<string> OnComplete = null, Action<string> OnError = null)
        {
            var key = WebConfigurationManager.AppSettings["FirebaseDynamicLinkKey"];
            var url = new Uri("https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key="+key);
            var body = "{" +
                "\"dynamicLinkInfo\":" +
                   JsonConvert.SerializeObject(request) +
                "}";

            WebHelper webHelper = new WebHelper();
            webHelper.Post(url, body, null, OnComplete, OnError);
        }

        public void DeleteDeeplink(string id, string user)
        {
            try
            {
                string cmd = @"
                DECLARE @ID INT = N'{0}'
                DECLARE @USER  NVARCHAR(50) = N'{1}'

                UPDATE	DynamicLink
                SET		deleted_flag = 'Y',
                        delete_date = DATEADD(HOUR, 7, GETDATE()),
                        delete_user = @USER
                WHERE	id = @ID";

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
    }
}