using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace com.lexus.th.Services
{
    public class NotificationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //WriteExceptionLog("fff", "ggg");
            APIService api = new APIService();
            ServiceModel firebase = new ServiceModel();

            string fulljob = context.JobDetail.Key.ToString();
            string[] job = fulljob.Split('.');
            string notiID = job[0];
            string jobname = job[1];

            DataTable dtNoti = new DataTable();
            dtNoti = api.GetNotification(notiID);
            string destination, sub_destination, message, title, reference_id, link_type, link_url, member_id, device_id = string.Empty;
            if (dtNoti.Rows.Count > 0)
            {
                DataRow drNoti = dtNoti.Rows[0];
                destination = drNoti["destination"].ToString();
                sub_destination = drNoti["sub_destination"].ToString();
                message = drNoti["message"].ToString();
                title = drNoti["title"].ToString();
                reference_id = drNoti["reference_id"].ToString();
                link_type = drNoti["link_type"].ToString();
                link_url = drNoti["link_url"].ToString();
                member_id = drNoti["member_id"].ToString();
                device_id = drNoti["device_id"].ToString();

                #region Member
                if (destination == "Member")
                {
                    DataTable dt;
                    if (sub_destination == "Group Member ID")
                    {
                        //get member_list
                        dt = api.GetMemberList(notiID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                string device_token = dr["device_token"].ToString();
                                string device_Type = dr["os_type"].ToString();
                                firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                            }
                        }
                    }
                    else
                    {
                        DataTable dtMember = api.GetDeviceToken(member_id);
                        if (dtMember.Rows.Count > 0)
                        {
                            DataRow drMember = dtMember.Rows[0];
                            string device_token = drMember["DEVICE_TOKEN"].ToString();
                            string device_Type = drMember["DEVICE_TYPE"].ToString();
                            firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                        }
                    }
                }
                #endregion

                #region Device
                else if (destination == "Device")
                {
                    DataTable dt;
                    if (sub_destination == "Group Device")
                    {
                        dt = api.GetMemberList(notiID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                string device_token = dr["device_token"].ToString();
                                string device_Type = dr["os_type"].ToString();
                                firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                            }
                        }
                    }
                    else
                    {
                        DataTable dtDevice = api.GetDeviceTokenByDevice(device_id);
                        if (dtDevice.Rows.Count > 0)
                        {
                            DataRow drDevice = dtDevice.Rows[0];
                            string device_token = drDevice["DEVICE_TOKEN"].ToString();
                            string device_Type = drDevice["DEVICE_TYPE"].ToString();
                            firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, notiID);
                            //firebase = api.PostFirebase(APIService.PushType.Topic, to, tbMessage.Text, tbTitle.Text, ref_id, noti_type, link_url);
                        }
                    }
                }
                #endregion

                #region Mobile OS
                else if (destination == "Mobile OS")
                {
                    string to = "";
                    if (sub_destination == "Android")
                    {
                        to = "/topics/Android";
                        firebase = api.PostFirebaseAndroid(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url);
                    }
                    else
                    {
                        to = "/topics/iOS";
                        firebase = api.PostFirebase(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url);
                    }
                }
                #endregion
            }

            context.Scheduler.DeleteJob(context.JobDetail.Key);
        }

        public void WriteExceptionLog(string ex, string servicename)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            string format = "D:\\Notification_log_{0}.txt";

            string filename = DateTime.Today.ToString("ddMMyyyy");

            string path = string.Format(format, filename);

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                string description = ex.ToString();

                string[] split = description.Split('\\');

                string timestamp = DateTime.Now.ToLongTimeString();

                writer.WriteLine(string.Format("{0} --> {1} {2} {3} --> {4}", timestamp, split[split.Count() - 1], "", "", servicename));
            }
        }
    }
}