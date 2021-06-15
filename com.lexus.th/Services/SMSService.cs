using AppLibrary.WebHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace com.lexus.th
{
    public class SMSService
    {
        private string conn;
        private string smsURL;
        private string smsFrom;
        private string smsCharge;
        private string smsCode;
        public class CMDResult
        {
            public string Status { get; set; }
            public string Detail { get; set; }
            public string SMID { get; set; }
        }

        public SMSService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.smsURL = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusSMSURL"];
            this.smsFrom = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusSMSFrom"];
            this.smsCharge = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusSMSCharge"];
            this.smsCode = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusSMSCode"];
        }

        private string ToUnicodeString(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in str)
            {
                sb.Append(((int)c).ToString("X4"));
            }
            return sb.ToString();
        }

        private string hexEncode(string str)
        {
            int i = 0;
            var result = ToUnicodeString(str);
            var output = "";

            result = result.ToUpper();
            if (result.Length > 0)
            {
                output = "%";
                for (i = 0; i < result.Length; i++)
                {
                    output += result[i];
                    if (i % 2 != 0 && i < result.Length - 1)
                    {
                        output += "%";
                    }
                }
            }
            return output;
        }

        public async Task<SMSModel> SendSMSTest(string mobile, string msg)
        {
            SMSModel result = new SMSModel();
            try
            {
                WebRequest request = WebRequest.Create(smsURL);
                request.Method = "POST";
                string postData = string.Format("CMD=SENDMSG&FROM={0}&TO={1}&REPORT=Y&CHARGE={2}&CODE={3}&CTYPE=TEXT&CONTENT={4}", smsFrom, mobile, smsCharge, smsCode, msg);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);  
                dataStream.Close();

                WebResponse response = request.GetResponse();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                string statusMT = xmlDoc.GetElementsByTagName("STATUS").Item(0).InnerText;
                string detailMT = xmlDoc.GetElementsByTagName("DETAIL").Item(0).InnerText;
                string smid = xmlDoc.GetElementsByTagName("SMID").Item(0).InnerText;
                response.Close();

                if (!string.IsNullOrEmpty(smid) && statusMT == "OK")
                {
                    //result.success = true;
                    //result.msg = new MsgModel();
                    //result.msg.code = 0;
                    //result.msg.text = string.Format(@"Status={0}, Detail={1}, SMID={2}", statusMT, detailMT, smid);

                    ////request.Method = "POST";
                    //postData = string.Format("CMD=DLVRREP&NTYPE=REP&FROM={0}&SMID={1}&STATUS={2}&DETAIL={3}", mobile, smid, statusMT, detailMT);
                    //byteArray = Encoding.UTF8.GetBytes(postData);
                    //request.ContentLength = byteArray.Length;
                    //dataStream = request.GetRequestStream();
                    //dataStream.Write(byteArray, 0, byteArray.Length);
                    //dataStream.Close();

                    //response = request.GetResponse();
                    //xmlDoc = new XmlDocument();
                    //xmlDoc.Load(response.GetResponseStream());

                    //string statusDR = xmlDoc.GetElementsByTagName("STATUS").Item(0).InnerText;
                    //string detailDR = xmlDoc.GetElementsByTagName("DETAIL").Item(0).InnerText;
                    //response.Close();

                    //if (statusDR == "OK")
                    //{
                    //    result.success = true;
                    //    result.msg = new MsgModel();
                    //    result.msg.code = 0;
                    //    result.msg.text = string.Format(@"DLVRREP: Status={0}, Detail={1}, SMID={2}", statusMT, detailMT, smid);
                    //}
                    //else
                    //{
                    //    result.success = false;
                    //    result.msg = new MsgModel();
                    //    result.msg.code = 100;
                    //    result.msg.text = string.Format(@"DLVRREP: Status={0}, Detail={1}, SMID={2}", statusMT, detailMT, smid);
                    //}
                }
                else
                {
                    result.success = false;
                    result.msg = new MsgModel();
                    result.msg.code = 100;
                    result.msg.text = string.Format(@"SENDMSG: Status={0}, Detail={1}, SMID={2}", statusMT, detailMT, smid);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public async  Task<SMSModel> SendSMS(string mobile, string message)
        {
            SMSModel result = new SMSModel();
            result.msg = new MsgModel();

            try
            {
                if (mobile.Length > 0)
                {
                    if (mobile.Substring(0, 1) == "0")
                    {
                        mobile = "66" + mobile.Substring(1, mobile.Length - 1);
                    }
                }
                CMDResult cmdSend = await CallSendSMS(mobile, message);
                if (!string.IsNullOrEmpty(cmdSend.SMID) && cmdSend.Status == "OK")
                {
                    result.success = true;
                    result.msg.code = 0;
                    result.msg.text = string.Format(@"DLVRREP: Status={0}, Detail={1}", cmdSend.Status, cmdSend.Detail);

                    //CMDResult cmdResp = CallDLVRRepSMS(mobile, cmdSend);
                    //if (cmdResp.Status == "OK")
                    //{
                    //    result.success = true;
                    //    result.msg.code = 0;
                    //    result.msg.text = string.Format(@"DLVRREP: Status={0}, Detail={1}", cmdSend.Status, cmdSend.Detail);
                    //}
                    //else
                    //{
                    //    result.success = false;
                    //    result.msg.code = 100;
                    //    result.msg.text = string.Format(@"DLVRREP: Status={0}, Detail={1}", cmdSend.Status, cmdSend.Detail);
                    //}
                }
                else
                {
                    result.success = false;
                    result.msg.code = 100;
                    result.msg.text = "OTP Fail";
                    //result.msg.text = string.Format(@"SENDMSG: Status={0}, Detail={1}, SMID={2}", cmdSend.Status, cmdSend.Detail, cmdSend.SMID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private async Task<CMDResult> CallSendSMS(string mobile, string message)
        {
            CMDResult result = new CMDResult();
            WebRequest request = null;
            WebResponse response = null;
            XmlDocument xmlDoc = null;
            Stream dataStream = null;

            try
            {
                request = WebRequest.Create(smsURL);
                request.Method = "POST";
                string msg = hexEncode(message);
                string postData = string.Format("CMD=SENDMSG&FROM={0}&TO={1}&REPORT=Y&CHARGE={2}&CODE={3}&CTYPE=UNICODE&CONTENT={4}", smsFrom, mobile, smsCharge, smsCode, msg);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                response = request.GetResponse();
                xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                result.Status = xmlDoc.GetElementsByTagName("STATUS").Item(0).InnerText;
                result.Detail = xmlDoc.GetElementsByTagName("DETAIL").Item(0).InnerText;
                result.SMID = xmlDoc.GetElementsByTagName("SMID").Item(0).InnerText;
                response.Close();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
                xmlDoc = null;
                dataStream = null;
            }
        }
        private CMDResult CallDLVRRepSMS(string mobile, CMDResult input)
        {
            CMDResult result = new CMDResult();
            WebRequest request = null;
            WebResponse response = null;
            XmlDocument xmlDoc = null;
            Stream dataStream = null;

            try
            {
                request = WebRequest.Create(smsURL);
                request.Method = "POST";
                string postData = string.Format("CMD=DLVRREP&NTYPE=REP&FROM={0}&SMID={1}&STATUS={2}&DETAIL={3}", mobile, input.SMID, input.Status, input.Detail);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                response = request.GetResponse();
                xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());

                result.Status = xmlDoc.GetElementsByTagName("STATUS").Item(0).InnerText;
                result.Detail = xmlDoc.GetElementsByTagName("DETAIL").Item(0).InnerText;
                response.Close();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
                xmlDoc = null;
                dataStream = null;
            }
        }
        public SMSReportModel GetSMSReport(string CMD, string NTYPE, string FROM, string SMID, string STATUS, string DETAIL)
        {
            SMSReportModel value = new SMSReportModel();
            try
            {
                value.STATUS = "OK";
                value.DETAIL = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
    }
}