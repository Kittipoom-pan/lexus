using AppLibrary.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Configuration;

namespace com.lexus.th
{
    public class APIService
    {
        private string conn;

        public enum PushType { Token, Topic }

        public APIService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
      

        public ServiceModel PostFirebase(PushType type, string to, string body, string title, string reference_id, string notify_type, string link_url)
        {
            ServiceModel srv = new ServiceModel();
           
            try
            {
                FirebasePostModel firebase = new FirebasePostModel();
                firebase.to = to;
               
                firebase.notification = new _FirebasePostModel();
                firebase.notification.title = title;
                firebase.notification.body = body;
                firebase.notification.sound = System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseNotifSound"];
                firebase.notification.content_available = true;
                firebase.notification.badge = 1;

                firebase.data = new _FirebaseData();
                firebase.data.reference_id = int.Parse(reference_id);
                firebase.data.notify_type = notify_type;
                firebase.data.link_url = link_url;
                firebase.data.notify_message = body;

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(firebase);
               
                var request = WebRequest.CreateHttp(System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseURL"]);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseAuth"]);
                var buffer = Encoding.UTF8.GetBytes(json);
                request.GetRequestStream().Write(buffer, 0, buffer.Length);


                var response = request.GetResponse();
                var responseString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

                JObject jsonObj = JObject.Parse(responseString);
                var failure = 0;
                if (jsonObj["failure"] != null)
                {
                    bool success = Int32.TryParse(jsonObj["failure"].ToString(), out failure);
                }

                json = null;
                request = null;
                buffer = null;
                response = null;

                if (failure > 0)
                {
                    srv.Success = false;
                    srv.Message = responseString;
                }
                else
                {
                    srv.Success = true;
                    srv.Message = responseString;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
            return srv;
        }

        public ServiceModel PostFirebaseAndroid(PushType type, string to, string body, string title, string reference_id, string notify_type, string link_url)
        {
            ServiceModel srv = new ServiceModel();

            try
            {
                FirebasePostAndroidModel firebase = new FirebasePostAndroidModel();
                firebase.to = to;

                firebase.data = new _FirebaseAndroidData();
                firebase.data.reference_id = int.Parse(reference_id);
                firebase.data.notify_type = notify_type;
                firebase.data.notify_title = title;
                firebase.data.link_url = link_url;
                firebase.data.notify_message = body;

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(firebase);

                var request = WebRequest.CreateHttp(System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseURL"]);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseAuth"]);
                var buffer = Encoding.UTF8.GetBytes(json);
                request.GetRequestStream().Write(buffer, 0, buffer.Length);


                var response = request.GetResponse();
                json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var failure = 0;

                JObject jsonObj = JObject.Parse(json);

                if (jsonObj["failure"] != null)
                {
                    bool success = Int32.TryParse(jsonObj["failure"].ToString(), out failure);
                }

                if (failure > 0)
                {
                    srv.Success = false;
                    srv.Message = json;
                }
                else
                {
                    srv.Success = true;
                    srv.Message = json;
                }
                json = null;
                request = null;
                buffer = null;
                response = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
              
            }
            return srv;
        }

        public ServiceModel SendPush(string tokenID, string messageText, string eventText, int badge, string platForm, string title, string reference_id, string link_type, string link_url, string noti_id)
        {
            ServiceModel srv = new ServiceModel();
            string serverKey = WebConfigurationManager.AppSettings["FirebaseAuth"];

            try
            {
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:" + serverKey);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "";
                    if (platForm.ToLower() == "ios")
                    {
                        json = preparePayloadIOS(tokenID, messageText, eventText, badge, title, reference_id, link_type, link_url, noti_id);
                    }
                    else
                    {
                        json = preparePayloadAndroid(tokenID, messageText, eventText, badge, title, reference_id, link_type, link_url, noti_id);
                    }

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                JObject jsonObj = JObject.Parse(result);
                var failure = 0;
                if (jsonObj["failure"] != null)
                {
                    bool success = Int32.TryParse(jsonObj["failure"].ToString(), out failure);
                }

                if (failure > 0)
                {
                    srv.Success = false;
                    srv.Message = result;
                }
                else
                {
                    srv.Success = true;
                    srv.Message = result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            
            }
            return srv;
        }

        public string preparePayloadAndroid(string token, string message, string eventType, int badge, string title, string reference_id, string notify_type, string link_url, string noti_id)
        {
            string result = "{\"to\":\"" + token + "\",\"priority\":\"high\",\"data\":{\"notification\":{\"notify_message\":\"" + message + "\",\"badge\":" + badge + ",\"notify_title\":\"" + title + "\",\"reference_id\":" + reference_id + ",\"notify_type\":\"" + notify_type + "\",\"link_url\":\"" + link_url + "\", \"notification_id\":\"" + noti_id + "\"},\"event\": \"" + eventType + "\"}}";
            return result;
        }

        public string preparePayloadIOS(string token, string message, string eventType, int badge, string title, string reference_id, string notify_type, string link_url, string noti_id)
        {
            string result = "{\"to\":\"" + token + "\",\"priority\":\"high\",\"content_available\":true,\"notification\": {\"body\": \"" + message + "\",\"title\": \"" + title + "\",\"event\": \"" + eventType + "\",\"reference_id\":" + reference_id + ",\"notify_type\":\"" + notify_type + "\",\"link_url\":\"" + link_url + "\", \"notification_id\":\"" + noti_id + "\", \"badge\": 1}}";
            return result;
        }

        public DataTable GetDeviceToken(string memberID)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @MEMBERID NVARCHAR(50) = N'{0}'

SELECT		TOP 1 D.DEVICE_TOKEN, D.DEVICE_TYPE
FROM		[T_DEVICE] D LEFT JOIN [T_CUSTOMER_TOKEN] CT ON D.TOKEN_NO = CT.TOKEN_NO
WHERE		CT.MEMBERID = @MEMBERID
ORDER BY LOGIN_DT DESC";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, memberID);

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetDeviceTokenByDevice(string deviceID)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @DEVICEID NVARCHAR(200) = N'{0}'

SELECT distinct DEVICE_TOKEN, DEVICE_TYPE
FROM		T_DEVICE
WHERE		DEVICE_ID = @DEVICEID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        GetSQLTextValue(deviceID));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetNotification(string notiID)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
                DECLARE @NotiID INT = {0}

                SELECT destination, sub_destination, message, title, reference_id, link_type, link_url, member_id, device_id
                FROM notification2
                WHERE id = @NotiID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, notiID);

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetMemberList(string notiID)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
                DECLARE @NotiID INT = {0}

                select * from notification2_device where notification2_id = @NotiID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, notiID);

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public static string GetSQLTextValue(string value)
        {
            return value.Replace("''", "''");
        }

        public void UpdateNotificationSended(string noti_id, string firebase_response, bool success, bool is_sended)
        {
            try
            {
                string cmd = @"
                DECLARE @firebase_response NVARCHAR(MAX) = N'{0}'
                DECLARE @success BIT = N'{1}'
                DECLARE @is_sended BIT = N'{2}'
                DECLARE @id INT = N'{3}'

                UPDATE notification2 
                SET [firebase_response] = @firebase_response, 
                    [firebase_success] = @success,
                    [is_sended] = @is_sended
                WHERE ID = @id";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        firebase_response,
                        success,
                        is_sended,
                        noti_id);

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
