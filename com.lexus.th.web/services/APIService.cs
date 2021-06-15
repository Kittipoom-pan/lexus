using AppLibrary.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Configuration;

namespace com.lexus.th.web
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
        public ServiceModel PostAPIRedeem(string token, string privilegeId)
        {
            ServiceModel srv = new ServiceModel();
            WebRequest request = null;
            WebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;

            try
            {
                request = WebRequest.Create(System.Web.Configuration.WebConfigurationManager.AppSettings["RedeemServiceURL"]);
                request.Method = "POST";
                string postData = string.Format("token={0}&privilege_id={1}", token, privilegeId);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                response = request.GetResponse();
                dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);

                ServiceRedeemModel redeem = JsonConvert.DeserializeObject<ServiceRedeemModel>(reader.ReadToEnd());
                srv.Success = redeem.success;
                srv.Message = redeem.msg.code.ToString() + " : " + redeem.msg.text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
                dataStream = null;
                reader = null;
            }
            return srv;
        }

        public ServiceModel PostFirebase(PushType type, string to, string body, string title, string reference_id, string notify_type, string link_url, string noti_id)
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
                firebase.data.notification_id = noti_id;


                var json = Newtonsoft.Json.JsonConvert.SerializeObject(firebase);
                var request = WebRequest.CreateHttp(System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseURL"]);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseAuth"]);
                var buffer = Encoding.UTF8.GetBytes(json);
                //request.ContentLength = buffer.Length;
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

        public ServiceModel PostFirebaseAndroid(PushType type, string to, string body, string title, string reference_id, string notify_type, string link_url, string noti_id)
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
                firebase.data.notification_id = noti_id;

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(firebase);

                var request = WebRequest.CreateHttp(System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseURL"]);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", System.Web.Configuration.WebConfigurationManager.AppSettings["FirebaseAuth"]);
                var buffer = Encoding.UTF8.GetBytes(json);
                //request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);


                var response = request.GetResponse();
                var responseString = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                JObject jsonObj = JObject.Parse(responseString);
                var failure = 0;
                if (jsonObj["failure"] != null)
                {
                    bool success = Int32.TryParse(jsonObj["failure"].ToString(), out failure);
                }

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
                //request = null;
                //response = null;
                //dataStream = null;
                //reader = null;
            }
            return srv;
        }

        public bool AddNotification(string title, string message, string type, string noti_type, string ref_id, string link_url)
        {
            bool isValid = false;
            try
            {
                string cmd = @"
DECLARE @TITLE  NVARCHAR(150) = N'{0}'
DECLARE @MESSAGE  NVARCHAR(250) = N'{1}'
DECLARE @TYPE  NVARCHAR(100) = N'{2}'
DECLARE @NOTI_TYPE  NVARCHAR(100) = N'{3}'
DECLARE @REF_ID  NVARCHAR(100) = N'{4}'
DECLARE @LINK_URL  NVARCHAR(100) = N'{5}'

INSERT INTO [NOTIFICATION] ([notify_title],[notify_message],[device_type],[create_date],[notify_type],[reference_id],[link_url])
VALUES (CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
		CASE LEN(@MESSAGE) WHEN 0 THEN NULL ELSE @MESSAGE END,
		CASE LEN(@TYPE) WHEN 0 THEN NULL ELSE @TYPE END,
        DATEADD(HOUR, 7, GETDATE()),
        CASE LEN(@NOTI_TYPE) WHEN 0 THEN NULL ELSE @NOTI_TYPE END,
        CASE LEN(@REF_ID) WHEN 0 THEN NULL ELSE @REF_ID END,
        CASE LEN(@LINK_URL) WHEN 0 THEN NULL ELSE @LINK_URL END)

SELECT COUNT(1) FROM NOTIFICATION WHERE notify_title = @TITLE";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(message),
                        WebUtility.GetSQLTextValue(type),
                        WebUtility.GetSQLTextValue(noti_type),
                        WebUtility.GetSQLTextValue(ref_id),
                        WebUtility.GetSQLTextValue(link_url));

                    if (db.ExecuteScalarFromCommandText<int>(cmd) > 0)
                    {
                        isValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isValid;
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
                // httpWebRequest.Headers.Add("Sender:key=" + senderId);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "";
                    // Construct payload based on each device platform
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
                //request = null;
                //response = null;
                //dataStream = null;
                //reader = null;
            }
            return srv;
        }

        public string preparePayloadAndroid(string token, string message, string eventType, int badge, string title, string reference_id, string notify_type, string link_url, string noti_id)
        {
            //string result = "{\"to\":\"" + token + "\",\"notification\": {\"body\": \"" + message + "\",\"title\": \""+ title + "\"},\"data\": {\"newsId\": \"1\"}}";
            string result = "{\"to\":\"" + token + "\",\"priority\":\"high\",\"data\":{\"notification\":{\"notify_message\":\"" + message + "\",\"badge\":" + badge + ",\"notify_title\":\"" + title + "\",\"reference_id\":" + reference_id + ",\"notify_type\":\"" + notify_type + "\",\"link_url\":\"" + link_url + "\", \"notification_id\":\""+ noti_id + "\"},\"event\": \"" + eventType + "\"}}";
            return result;
        }

        public string preparePayloadIOS(string token, string message, string eventType, int badge, string title, string reference_id, string notify_type, string link_url, string noti_id)
        {
            string result = "{\"to\":\"" + token + "\",\"priority\":\"high\",\"content_available\":true,\"notification\": {\"body\": \"" + message + "\",\"title\": \"" + title + "\",\"event\": \"" + eventType + "\",\"reference_id\":" + reference_id + ",\"notify_type\":\"" + notify_type + "\",\"link_url\":\"" + link_url + "\", \"notification_id\":\""+ noti_id + "\", \"badge\": 1}}";
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
                    cmd = string.Format(cmd,memberID);

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

SELECT		DEVICE_TOKEN, DEVICE_TYPE
FROM		T_DEVICE
WHERE		DEVICE_ID = @DEVICEID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(deviceID));

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
    }
}