using AppLibrary.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace com.lexus.th
{
    public class DeviceService
    {
        private string conn;
        public DeviceService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public ServiceDeviceModel UpdateDevice(string device_id, string device_token, string device_type, string token)
        {
            ServiceDeviceModel value = new ServiceDeviceModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //DECLARE @OTP NVARCHAR(10) = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
                    string cmd = @"
DECLARE @DEVICEID NVARCHAR(200) = N'{0}'
DECLARE @DEVICETOKEN NVARCHAR(500) = N'{1}'
DECLARE @DEVICETYPE NVARCHAR(100) = N'{2}'
DECLARE @TOKEN_NO NVARCHAR(100) = N'{3}'

DECLARE @CHECK_DEVICE INT = (SELECT COUNT(ID) FROM T_DEVICE WHERE DEVICE_ID = @DEVICEID)

IF @CHECK_DEVICE = 0 BEGIN
    INSERT T_DEVICE (DEVICE_ID, DEVICE_TOKEN, DEVICE_TYPE, CREATE_DATE, TOKEN_NO)
    VALUES (@DEVICEID, @DEVICETOKEN, @DEVICETYPE, DATEADD(HOUR, 7, GETDATE()), @TOKEN_NO)
END

IF @CHECK_DEVICE > 0 BEGIN
    UPDATE T_DEVICE SET DEVICE_TOKEN = @DEVICETOKEN, DEVICE_TYPE = @DEVICETYPE, UPDATE_DATE = DATEADD(HOUR, 7, GETDATE()), TOKEN_NO = @TOKEN_NO WHERE DEVICE_ID = @DEVICEID
END

SELECT ID FROM T_DEVICE WHERE DEVICE_ID = @DEVICEID";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, device_id, device_token, device_type, token)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            value.success = true;
                            value.msg = new MsgModel() { code = 0, text = "Success", store_link = "", version = "" };
                        }
                    }
                }
                //SystemController syc = new SystemController();
                //ValidationModel validation2 = syc.CheckSystemNew(p, v, lang);
                //if (!validation2.Success)
                //{
                //    value.success = validation2.Success;
                //    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                //    return value;
                //}
                //else
                //{
                //    ValidationModel validation = CheckValidation(member_id, phone_no);
                //    if (!validation.Success)
                //    {
                //        value.success = validation.Success;
                //        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                //        return value;
                //    }


                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceDeviceModel> UpdateDeviceNew(string device_id, string device_token, string device_type, string token, string lang)
        {
            ServiceDeviceModel value = new ServiceDeviceModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //DECLARE @OTP NVARCHAR(10) = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
                    string cmd = @"
DECLARE @DEVICEID NVARCHAR(200) = N'{0}'
DECLARE @DEVICETOKEN NVARCHAR(500) = N'{1}'
DECLARE @DEVICETYPE NVARCHAR(100) = N'{2}'
DECLARE @TOKEN_NO NVARCHAR(100) = N'{3}'

DECLARE @CHECK_DEVICE INT = (SELECT COUNT(ID) FROM T_DEVICE WHERE DEVICE_ID = @DEVICEID)

IF @CHECK_DEVICE = 0 BEGIN
    INSERT T_DEVICE (DEVICE_ID, DEVICE_TOKEN, DEVICE_TYPE, CREATE_DATE, TOKEN_NO)
    VALUES (@DEVICEID, @DEVICETOKEN, @DEVICETYPE, DATEADD(HOUR, 7, GETDATE()), @TOKEN_NO)
END

IF @CHECK_DEVICE > 0 BEGIN
    UPDATE T_DEVICE 
    SET DEVICE_TOKEN = @DEVICETOKEN, DEVICE_TYPE = @DEVICETYPE, 
        UPDATE_DATE = DATEADD(HOUR, 7, GETDATE()), TOKEN_NO = @TOKEN_NO 
    WHERE DEVICE_ID = @DEVICEID
END
";

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, device_id, device_token, device_type, token));
                        value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = "", version = "" };
                    await UpdateGuestSurveyToMember(token, device_id, db);
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public ServiceDeviceModel PushDevice(string device_id, string device_token, string device_type)
        {
            ServiceDeviceModel value = new ServiceDeviceModel();
            try
            {
                string serverKey = "AIzaSyC8s7oAvS2GOmJsKC2zpYiW_iTST2slwOM";
                //string senderId = WebConfigurationManager.AppSettings["FCM_SID"];

                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                // httpWebRequest.Headers.Add("Sender:key=" + senderId);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "";
                    // Construct payload based on each device platform
                    //if (platForm.ToLower() == "ios")
                    //{

                    //}
                    //else
                    //{
                    //    json = preparePayloadAndroid(tokenID, messageText, eventText, badge);
                    //}
                    json = preparePayloadIOS(device_token, "test1", "TEST", 1);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                value.success = true;
                value.msg = new MsgModel() { code = 0, text = "Success", store_link = "", version = "" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceDeviceModel> PushDeviceNew(string device_id, string device_token, string device_type, string lang)
        {
            ServiceDeviceModel value = new ServiceDeviceModel();
            try
            {
                string serverKey = "AIzaSyC8s7oAvS2GOmJsKC2zpYiW_iTST2slwOM";
                //string senderId = WebConfigurationManager.AppSettings["FCM_SID"];

                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                // httpWebRequest.Headers.Add("Sender:key=" + senderId);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "";
                    json = preparePayloadIOS(device_token, "test1", "TEST", 1);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                value.success = true;
                value.msg = new MsgModel() { code = 0, text = "Success", store_link = "", version = "" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public string preparePayloadIOS(string token, string message, string eventType, int badge)
        {
            string result = "{\"to\":\"" + token + "\",\"priority\":\"high\",\"content_available\":true,\"notification\": {\"body\": \"" + message + "\",\"event\": \"" + eventType + "\",\"badge\": " + badge + "}}";
            return result;
        }

        private async Task UpdateGuestSurveyToMember(string token, string device_id, DBAccess db)
        {
            MasterDataService service = new MasterDataService();
            string member_id =await service.GetMemberIdByToken(token);
            if (string.IsNullOrEmpty(member_id))
            {
                return;
            }
            try
            {
                    string cmd = @"
                          DECLARE @member_id NVARCHAR(100) = N'{0}'
                          DECLARE @device_id NVARCHAR(100) = N'{1}'

                          update sv.customer_answer SET member_id = @member_id
                          where member_id IS NULL AND device_id = @device_id
                         ";

                using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, device_id)))
                { }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}