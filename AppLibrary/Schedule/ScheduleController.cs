using System;
using System.IO;
using System.Net;
using System.Web;

namespace AppLibrary.Schedule
{
    public class ScheduleController
    {
        public void CreateSchedule(string target, DateTime schedule_date, string method, string param, Action<string> OnComplete = null, Action<string> OnError = null)
        {
            try
            {
                string schedule_date_text = schedule_date.ToString("yyyy-MM-dd HH:mm:ss");
                string postString = string.Format("execute_time={0}&target={1}&method={2}&param={3}", schedule_date_text, HttpContext.Current.Server.UrlEncode(target), method, param);
                string URL = "https://task.fysvc.com/task/create/?auth_code=04ba5a93d6";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //Utility.InsertLog(this, "AddNotification", postString, "ManagePush");

                    streamWriter.Write(postString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                OnComplete?.Invoke(response);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.ToString());
            }
        }
    }
}
