using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using System.Security.Cryptography;
using AppLibrary.Database;
using System.IO;
using System.Data;
using System.Threading.Tasks;

namespace com.lexus.th
{
    public class SendEmailService
    {
        private string conn;
        public SendEmailService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }


        public static string SAMPLE_KEY = "gCjK+DZ/GCYbKIGiAt1qCA==";
        public static string SAMPLE_IV = "47l5QsSe1POo31adQ/u7nQ==";
        private string Host
        {
            get
            {
                return WebConfigurationManager.AppSettings["mail_server"];
            }
        }

        private string UserName
        {
            get
            {
                Aes aes = new Aes(Utility.SAMPLE_KEY, Utility.SAMPLE_IV);
                string userName = aes.DecryptFromBase64String(WebConfigurationManager.AppSettings["mail_user_name"]);
                return userName;
            }
        }

        private string Password
        {
            get
            {
                Aes aes = new Aes(Utility.SAMPLE_KEY, Utility.SAMPLE_IV);
                string password = aes.DecryptFromBase64String(WebConfigurationManager.AppSettings["mail_password"]);
                return password;
            }
        }

        private int Port
        {
            get
            {
                return Convert.ToInt32(WebConfigurationManager.AppSettings["mail_port"]);
            }
        }

        private string DisplayName
        {
            get
            {
                Aes aes = new Aes(Utility.SAMPLE_KEY, Utility.SAMPLE_IV);
                string displayName = aes.DecryptFromBase64String(WebConfigurationManager.AppSettings["mail_display_name"]);
                return displayName;
            }
        }

        private string Sender
        {
            get
            {
                Aes aes = new Aes(Utility.SAMPLE_KEY, Utility.SAMPLE_IV);
                string sender = aes.DecryptFromBase64String(WebConfigurationManager.AppSettings["mail_sender"]);
                return sender;
            }
        }


        private bool EnableSsl
        {
            get
            {
                bool enableSsl = bool.Parse(WebConfigurationManager.AppSettings["enable_ssl"]);
                return enableSsl;
            }
        }


        public async Task<MsgModel> SendEmail(string send_email_to, int any_id, string token, string lang, byte[] attachFile, string attachFileName)
        {
            MsgModel msg = new MsgModel();
            //byte[] temp = attachFile;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                string port = WebConfigurationManager.AppSettings["mail_server"];

                if (!string.IsNullOrEmpty(send_email_to) && send_email_to == "Appointment_CallCenter")
                {
                    AppointmentService appointmentService = new AppointmentService();
                    AppointmentDetailDataForSendEmail data_for_email = new AppointmentDetailDataForSendEmail();

                    data_for_email =await appointmentService.GetDetailForEmail(any_id, token, lang);


                    if (data_for_email.car_owner_data == null && data_for_email.appointment_data == null)
                    {
                        msg.code = 200;
                        msg.text = "Save success but Query no data for send email to call center.";
                        return msg;
                    }
                    else
                    {    
                        if (attachFile != null && attachFile.Length > 0)
                        mail =await this.SendEmailToCallCenter_Appointment(data_for_email, token, lang, attachFile, attachFileName);
                    }
                }

                if (!string.IsNullOrEmpty(send_email_to) && send_email_to == "Test_Drive_CallCenter")
                {
                    TestDriveService testDriveService = new TestDriveService();
                    TestDriveDetailForSendEmail data_for_email = testDriveService.GetTestDriveDetailForEmail(lang, any_id);
                    if (string.IsNullOrEmpty(data_for_email.code))
                    {
                        msg.code = 200;
                        msg.text = "Save success but Query no data for send email to dealer.";
                        return msg;
                    }
                    else
                    {
                        mail = this.SendEmailToCallCenter_TestDrive(data_for_email, attachFile, attachFileName);
                    }
                }

                #region Test only send mail by customer enviroment.
                if (!string.IsNullOrEmpty(send_email_to) && send_email_to == "Me")
                {
                    mail = this.SendEmailOfficialTest(any_id);
                }
                #endregion

                // send_email_to = "XXXXX", use for tell to send mail 
                // Ex send_email_to == "Online_Booking"  will send mail to below list :
                //    1.Lexus Call Center.
                //    2.Dealer 
                //    3.TMT  Who?
                //    4.CarOwner || Guest
                if (!string.IsNullOrEmpty(send_email_to) && send_email_to == "OnlinebookingService")
                {
                    OnlineBookingService campaignRegisterModel = new OnlineBookingService();
                    OnlineBookingExportExcelModel onlineBookingExportExcelModel = new OnlineBookingExportExcelModel();

                    onlineBookingExportExcelModel = await campaignRegisterModel.GetDataForEmail(any_id, token, lang);

                    if (string.IsNullOrEmpty(onlineBookingExportExcelModel.title))
                    {
                        msg.code = 200;
                        msg.text = "Save success but Query no data for send email to call center.";
                        return msg;
                    }
                    else
                    {
                        if (attachFile != null && attachFile.Length > 0)
                        mail = await this.SendEmailToCallCenter_Onlinebooking(onlineBookingExportExcelModel, attachFile, attachFileName);
                    }
                }

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                smtp.Host = this.Host;
                smtp.Port = this.Port;
                smtp.Credentials = new System.Net.NetworkCredential(this.UserName, this.Password);
                smtp.EnableSsl = this.EnableSsl;

                LogManager.ServiceLog.WriteCustomLog("Before SMTP Send Email", " Host : " + smtp.Host + "||" + " Port : " +  smtp.Port + "||" + " Credentials : " + smtp.Credentials + "||" + " EnableSsl : " + smtp.EnableSsl);
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteCustomLog("Error On Send Email", ex.Message);
                throw ex;
            }

            return msg;
        }

        public async Task<MailMessage> SendEmailToCallCenter_Appointment(AppointmentDetailDataForSendEmail data, string token, string lang, byte[] attachFile, string attachFileName)
        {
            MailMessage mail = new MailMessage();
            //if (data.car_owner_data.service_appointment_type == 1)//Car Owner
            //{
            try
            {
                DateTime date_time_now = DateTime.Now;

                mail.From = new MailAddress(this.Sender, this.DisplayName);
                //mail.To.Add(new MailAddress(data.appointment_data.call_center_email.ToString()));
                foreach (var address_call_center in data.appointment_data.call_center_email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address_call_center);
                }
                foreach (var address_tcap in data.appointment_data.tcap_call_center_email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address_tcap);
                }
                mail.Subject = "มีรายการนัดหมายใหม่เข้ามา ทะเบียน: " + data.appointment_data.plate_number;
                mail.Attachments.Add(new Attachment(new MemoryStream(attachFile), attachFileName));
                if (data.appointment_data.is_pickup_service == true)
                {
                    if (data.appointment_data.latitude == "0" && data.appointment_data.longitude == "0")
                    {
                        mail.Body = string.Format(
                                                                   "<html>" +
                                                                    "<head> " +
                                                                   "<meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\" > " +
                                                                   "</head> " +

                                                                       "<b>เรียน ท่านผู้แทนจำหน่าย และศูนย์บริการเลกซัส<b> " +
                                                                        "<br> " +
                                                                        "<br> " +
                                                                        "<br> " +
                                                                        "ขณะนี้มีรายการนัดหมายเข้ารับบริการมายังศูนย์บริการของท่าน<br> " +
                                                                        "กรุณาติดต่อกลับหาลูกค้าภายใน 10 นาที ในเวลาทำการ หลังจากท่านได้รับอีเมลฉบับนี้ " +
                                                                        "<br> " +
                                                                        "<br> " +
                                                                        "<b>เลขที่รายการนัดหมาย : {1}<b><br> " +
                                                                        "<b>วันที่ทำรายการเข้ามา : {2}<b> " +
                                                                        "<br> " +
                                                                        "<br> " +
                                                                        "<u>ข้อมูลเจ้าของรถ</u><br> " +
                                                                        "ชื่อ-นามสกุล : {14} {15}<br> " +
                                                                        "เบอร์โทรติดต่อ : {16}<br> " +
                                                                        "หมายเลขตัวถังรถยนต์ : {6}<br> " +
                                                                        "เลขทะเบียนรถยนต์ : {7}<br> " +
                                                                        "รุ่นรถยนต์ : {8}<br> " +
                                                                        "รายการนัดหมาย : {9}<br> " +
                                                                        "ผู้แทนจำหน่ายหรือศูนย์บริการที่ต้องการนัดหมาย : {10} ({23})<br> " +
                                                                        "ที่อยู่ลูกค้าที่ต้องการให้ไปรับรถ : {22}<br> " +
                                                                        "พิกัดตำแหน่ง : - <br> " +
                                                                        "รายละเอียดที่อยู่ : {19}<br> " +
                                                                        "วันที่ลูกค้าต้องการให้ไปรับรถ : {20}<br> " +
                                                                        "เวลาที่ลูกค้าต้องการให้ไปรับรถ : {21}<br> " +
                                                                        "หมายเหตุ : {11}<br> " +
                                                                        "วันที่ต้องการนัดหมายเข้ารับบริการ : {12}<br> " +
                                                                        "เวลาที่ต้องการนัดหมาย : {13}<br> " +
                                                                        "<br> " +
                                                                        "<br> " +
                                                                        "<u> ข้อมูลผู้ติดต่อเข้ารับบริการ </u><br> " +
                                                                        "ชื่อ-นามสกุล : {3} {4}<br> " +
                                                                        "เบอร์โทรติดต่อ : {5}<br> " +
                                                                        "<br> " +
                                                                        "<br> " +
                                                                        "<br> " +
                                                                        "หลังจากคอนเฟิร์มรายละเอียดการนัดหมายกับลูกค้าเรียบร้อยแล้ว กรุณากรอกรายละเอียดลงในแบบฟอร์มที่แนบมาในอีเมลฉบับนี้<br> " +
                                                                        "แล้วส่งกลับมาที่ LEXUS Elite Club Center ภายใน 30 นาที (หลังจากได้รับอีเมลฉบับนี้)<br> " +
                                                                        "อีเมล lexus02@toyotaconnected.co.th<br> " +
                                                                        "เบอร์โทรศัพท์ 02-305-6799 ทุกวัน เวลา 10.00-20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์ " +

                                                                     "</html> ", data.appointment_data.id, data.appointment_data.code, data.appointment_data.created_date, data.appointment_data.f_name, data.appointment_data.l_name, data.appointment_data.contact_mobile
                                                       , data.appointment_data.vin, data.appointment_data.plate_number, data.appointment_data.car_model, data.appointment_data.type_of_service, data.appointment_data.dealer_name
                                                       , data.appointment_data.remark, data.appointment_data.appt_date, data.appointment_data.appt_time, data.car_owner_data.f_name, data.car_owner_data.l_name, data.car_owner_data.contact_mobile
                                                       , data.appointment_data.latitude, data.appointment_data.longitude, data.appointment_data.location_detail, data.appointment_data.pickup_date, data.appointment_data.pickup_time_id
                                                       , data.appointment_data.pickup_address, data.appointment_data.branch_name);
                    }
                    else 
                    { 
                    mail.Body = string.Format(
                                           "<html>" +
                                            "<head> " +
                                           "<meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\" > " +
                                           "</head> " +

                                               "<b>เรียน ท่านผู้แทนจำหน่าย และศูนย์บริการเลกซัส<b> " +
                                                "<br> " +
                                                "<br> " +
                                                "<br> " +
                                                "ขณะนี้มีรายการนัดหมายเข้ารับบริการมายังศูนย์บริการของท่าน<br> " +
                                                "กรุณาติดต่อกลับหาลูกค้าภายใน 10 นาที ในเวลาทำการ หลังจากท่านได้รับอีเมลฉบับนี้ " +
                                                "<br> " +
                                                "<br> " +
                                                "<b>เลขที่รายการนัดหมาย : {1}<b><br> " +
                                                "<b>วันที่ทำรายการเข้ามา : {2}<b> " +
                                                "<br> " +
                                                "<br> " +
                                                "<u>ข้อมูลเจ้าของรถ</u><br> " +
                                                "ชื่อ-นามสกุล : {14} {15}<br> " +
                                                "เบอร์โทรติดต่อ : {16}<br> " +
                                                "หมายเลขตัวถังรถยนต์ : {6}<br> " +
                                                "เลขทะเบียนรถยนต์ : {7}<br> " +
                                                "รุ่นรถยนต์ : {8}<br> " +
                                                "รายการนัดหมาย : {9}<br> " +
                                                "ผู้แทนจำหน่ายหรือศูนย์บริการที่ต้องการนัดหมาย : {10} ({23})<br> " +
                                                "ที่อยู่ลูกค้าที่ต้องการให้ไปรับรถ : {22}<br> " +
                                                "พิกัดตำแหน่ง : https://www.google.com/maps/search/?api=1&query={17},{18}<br> " +
                                                "รายละเอียดที่อยู่ : {19}<br> " +
                                                "วันที่ลูกค้าต้องการให้ไปรับรถ : {20}<br> " +
                                                "เวลาที่ลูกค้าต้องการให้ไปรับรถ : {21}<br> " +
                                                "หมายเหตุ : {11}<br> " +
                                                "วันที่ต้องการนัดหมายเข้ารับบริการ : {12}<br> " +
                                                "เวลาที่ต้องการนัดหมาย : {13}<br> " +
                                                "<br> " +
                                                "<br> " +
                                                "<u> ข้อมูลผู้ติดต่อเข้ารับบริการ </u><br> " +
                                                "ชื่อ-นามสกุล : {3} {4}<br> " +
                                                "เบอร์โทรติดต่อ : {5}<br> " +
                                                "<br> " +
                                                "<br> " +
                                                "<br> " +
                                                "หลังจากคอนเฟิร์มรายละเอียดการนัดหมายกับลูกค้าเรียบร้อยแล้ว กรุณากรอกรายละเอียดลงในแบบฟอร์มที่แนบมาในอีเมลฉบับนี้<br> " +
                                                "แล้วส่งกลับมาที่ LEXUS Elite Club Center ภายใน 30 นาที (หลังจากได้รับอีเมลฉบับนี้)<br> " +
                                                "อีเมล lexus02@toyotaconnected.co.th<br> " +
                                                "เบอร์โทรศัพท์ 02-305-6799 ทุกวัน เวลา 10.00-20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์ " +

                                             "</html> ", data.appointment_data.id, data.appointment_data.code, data.appointment_data.created_date, data.appointment_data.f_name, data.appointment_data.l_name, data.appointment_data.contact_mobile
                                                       , data.appointment_data.vin, data.appointment_data.plate_number, data.appointment_data.car_model, data.appointment_data.type_of_service, data.appointment_data.dealer_name
                                                       , data.appointment_data.remark, data.appointment_data.appt_date, data.appointment_data.appt_time, data.car_owner_data.f_name, data.car_owner_data.l_name, data.car_owner_data.contact_mobile
                                                       , data.appointment_data.latitude, data.appointment_data.longitude, data.appointment_data.location_detail, data.appointment_data.pickup_date, data.appointment_data.pickup_time_id
                                                       , data.appointment_data.pickup_address, data.appointment_data.branch_name);
                    }
                }
                else
                {
                    mail.Body = string.Format(
                        "<html>" +
                         "<head> " +
                        "<meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\" > " +
                        "</head> " +

                            "<b>เรียน ท่านผู้แทนจำหน่าย และศูนย์บริการเลกซัส<b> " +
                             "<br> " +
                             "<br> " +
                             "<br> " +
                             "ขณะนี้มีรายการนัดหมายเข้ารับบริการมายังศูนย์บริการของท่าน<br> " +
                             "กรุณาติดต่อกลับหาลูกค้าภายใน 10 นาที ในเวลาทำการ หลังจากท่านได้รับอีเมลฉบับนี้ " +
                             "<br> " +
                             "<br> " +
                             "<b>เลขที่รายการนัดหมาย : {1}<b><br> " +
                             "<b>วันที่ทำรายการเข้ามา : {2}<b> " +
                             "<br> " +
                             "<br> " +
                             "<u>ข้อมูลเจ้าของรถ</u><br> " +
                             "ชื่อ-นามสกุล : {14} {15}<br> " +
                             "เบอร์โทรติดต่อ : {16}<br> " +
                             "หมายเลขตัวถังรถยนต์ : {6}<br> " +
                             "เลขทะเบียนรถยนต์ : {7}<br> " +
                             "รุ่นรถยนต์ : {8}<br> " +
                             "รายการนัดหมาย : {9}<br> " +
                             "ผู้แทนจำหน่ายหรือศูนย์บริการที่ต้องการนัดหมาย : {10} ({23})<br> " +
                             "ที่อยู่ลูกค้าที่ต้องการให้ไปรับรถ : -<br> " +
                             "พิกัดตำแหน่ง : -<br> " +
                             "รายละเอียดที่อยู่ : -<br> " +
                             "วันที่ลูกค้าต้องการให้ไปรับรถ : -<br> " +
                             "เวลาที่ลูกค้าต้องการให้ไปรับรถ : -<br> " +
                             "หมายเหตุ : {11}<br> " +
                             "วันที่ต้องการนัดหมายเข้ารับบริการ : {12}<br> " +
                             "เวลาที่ต้องการนัดหมาย : {13}<br> " +
                             "<br> " +
                             "<br> " +
                             "<u> ข้อมูลผู้ติดต่อเข้ารับบริการ </u><br> " +
                             "ชื่อ-นามสกุล : {3} {4}<br> " +
                             "เบอร์โทรติดต่อ : {5}<br> " +
                             "<br> " +
                             "<br> " +
                             "<br> " +
                             "หลังจากคอนเฟิร์มรายละเอียดการนัดหมายกับลูกค้าเรียบร้อยแล้ว กรุณากรอกรายละเอียดลงในแบบฟอร์มที่แนบมาในอีเมลฉบับนี้<br> " +
                             "แล้วส่งกลับมาที่ LEXUS Elite Club Center ภายใน 30 นาที (หลังจากได้รับอีเมลฉบับนี้)<br> " +
                             "อีเมล lexus02@toyotaconnected.co.th<br> " +
                             "เบอร์โทรศัพท์ 02-305-6799 ทุกวัน เวลา 10.00-20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์ " +

                          "</html> ", data.appointment_data.id, data.appointment_data.code, data.appointment_data.created_date, data.appointment_data.f_name, data.appointment_data.l_name, data.appointment_data.contact_mobile
                                    , data.appointment_data.vin, data.appointment_data.plate_number, data.appointment_data.car_model, data.appointment_data.type_of_service, data.appointment_data.dealer_name
                                    , data.appointment_data.remark, data.appointment_data.appt_date, data.appointment_data.appt_time, data.car_owner_data.f_name, data.car_owner_data.l_name, data.car_owner_data.contact_mobile
                                    , data.appointment_data.latitude, data.appointment_data.longitude, data.appointment_data.location_detail, data.appointment_data.pickup_date, data.appointment_data.pickup_time_id
                                    , data.appointment_data.pickup_address, data.appointment_data.branch_name);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}

            //if (data.car_owner_data.service_appointment_type == 2)//Contact Person
            //{
            //    try
            //    {
            //        DateTime date_time_now = DateTime.Now;

            //        mail.From = new MailAddress(this.Sender, this.DisplayName);
            //        mail.To.Add(new MailAddress(data.call_center_email.ToString()));
            //        foreach (var address in data.tcap_call_center_email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            //        {
            //            mail.To.Add(address);
            //        }
            //        mail.Subject = "มีรายการนัดหมายใหม่เข้ามา ทะเบียน: " + data.plate_number;
            //        mail.Attachments.Add(new Attachment(new MemoryStream(attachFile), attachFileName));
            //        mail.Body = string.Format(
            //            "<html>" +
            //             "<head> " +
            //            "<meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\" > " +
            //            "</head> " +

            //                "<b>เรียน ท่านผู้แทนจำหน่าย และศูนย์บริการเลกซัส<b> " +
            //                 "<br> " +
            //                 "<br> " +
            //                 "<br> " +
            //                 "ขณะนี้มีรายการนัดหมายเข้ารับบริการมายังศูนย์บริการของท่าน<br> " +
            //                 "กรุณาติดต่อกลับหาลูกค้าภายใน 10 นาที ในเวลาทำการ หลังจากท่านได้รับอีเมลฉบับนี้ " +
            //                 "<br> " +
            //                 "<br> " +
            //                 "<b>เลขที่รายการนัดหมาย : {1}<b><br> " +
            //                 "<b>วันที่ทำรายการเข้ามา : {2}<b> " +
            //                 "<br> " +
            //                 "<br> " +
            //                 "<u>ข้อมูลเจ้าของรถ</u></br> " +
            //                 "ชื่อ : {3}<br> " +
            //                 "นามสกุล : {4}<br> " +
            //                 "เบอร์โทรติดต่อ : {5}<br> " +
            //                 "หมายเลขตัวถังรถยนต์ : {6}<br> " +
            //                 "เลขทะเบียนรถยนต์ : {7}<br> " +
            //                 "รุ่นรถยนต์ : {8}<br> " +
            //                 "รายการนัดหมาย : {9}<br> " +
            //                 "ผู้แทนจำหน่ายหรือศูนย์บริการที่ต้องการนัดหมาย : {10}<br> " +
            //                 "หมายเหตุ : {11}<br> " +
            //                 "วันที่ต้องการนัดหมายเข้ารับบริการ : {12}<br> " +
            //                 "เวลาที่ต้องการนัดหมาย : {13}<br> " +
            //                 "<br> " +
            //                 "<br> " +
            //                 "<u> ข้อมูลผู้ติดต่อเข้ารับบริการ </u><br> " +
            //                 "ชื่อ : {3}<br> " +
            //                 "นามสกุล : {4}<br> " +
            //                 "เบอร์โทรติดต่อ : {5}<br> " +
            //                 "หมายเลขตัวถังรถยนต์ : {6}<br> " +
            //                 "เลขทะเบียนรถยนต์ : {7}<br> " +
            //                 "รุ่นรถยนต์ : {8}<br> " +
            //                 "รายการนัดหมาย : {9}<br> " +
            //                 "ผู้แทนจำหน่ายหรือศูนย์บริการที่ต้องการนัดหมาย : {10}<br> " +
            //                 "หมายเหตุ : {11}<br> " +
            //                 "วันที่ต้องการนัดหมายเข้ารับบริการ : {12}<br> " +
            //                 "เวลาที่ต้องการนัดหมาย : {13}<br> " +
            //                 "<br> " +
            //                 "<br> " +
            //                 "<br> " +
            //                 "หลังจากคอนเฟิร์มรายละเอียดการนัดหมายกับลูกค้าเรียบร้อยแล้ว กรุณากรอกรายละเอียดลงในแบบฟอร์มที่แนบมาในอีเมลฉบับนี้<br> " +
            //                 "แล้วส่งกลับมาที่ LEXUS Elite Club Center ภายนใน 30 นาที (หลังจากได้รับอีเมลฉบับนี้)<br> " +
            //                 "อีเมล lexus02@toyotaconnected.co.th<br> " +
            //                 "เบอร์โทรศัพท์ 02-305-6799 ทุกวัน เวลา 10.00-20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์ " +

            //              "</html> ", data.id, data.code, data.created_date, data.f_name, data.l_name, data.contact_mobile
            //                        , data.vin, data.plate_number, data.car_model, data.type_of_service, data.dealer_name
            //                        , data.remark, data.appt_date, data.appt_time);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}


            return mail;
        }

        //public MailMessage SendEmailToDealer(AppointmentDetailDataForSendEmail data, string token, string lang)
        //{
        //    MailMessage mail = new MailMessage();
        //    try
        //    {
        //        DateTime date_time_now = DateTime.Now;

        //        mail.From = new MailAddress("palapong@feyverly.com", "I'm God");
        //        mail.To.Add(new MailAddress(data.call_center_email.ToString()));
        //        mail.Subject = "Service Appointment Appointment ID: " + data.id + "  " + date_time_now;
        //        mail.Body = string.Format(
        //            "<html>" +
        //                 "Dear All, " +
        //                 "<br> " +
        //                 "<br> " +
        //                 "<br> " +
        //                 "<u> Car Owner Information </u><br> " +
        //                 "Appointment Code: {1}<br> " +
        //                 "Create Date: {2}<br> " +
        //                 "<br> " +
        //                 "<br> " +
        //                 "<br> " +
        //                 "Name : {3}<br> " +
        //                 "Surname : {4}<br> " +
        //                 "Contact number: {5}<br> " +
        //                 "VIN : {6}<br> " +
        //                 "Plate Number: {7}<br> " +
        //                 "Car Model: {8}<br> " +
        //                 "Type of service: {9}<br> " +
        //                 "Dealer : {10}<br> " +
        //                 "Remark : {11}<br> " +
        //                 "Appointment Date: {12}<br> " +
        //                 "Appointment Time: {13}<br> " +
        //                 "<br> " +
        //                 "<br> " +
        //                 "<u> Contact Person Information </u><br> " +
        //                 "Appointment Code: {1}<br> " +
        //                 "Create Date: {2}<br> " +
        //                 "<br> " +
        //                 "<br> " +
        //                 "<br> " +
        //                 "Name : {3}<br> " +
        //                 "Surname : {4}<br> " +
        //                 "Contact number: {5}<br> " +
        //                 "VIN : {6}<br> " +
        //                 "Plate Number: {7}<br> " +
        //                 "Car Model: {8}<br> " +
        //                 "Type of service: {9}<br> " +
        //                 "Dealer : {10}<br> " +
        //                 "Remark : {11}<br> " +
        //                 "Appointment Date: {12}<br> " +
        //                 "Appointment Time: {13}<br> " +
        //              "</html> ", data.id, data.code, data.created_date, data.f_name, data.l_name, data.contact_mobile
        //                            , data.vin, data.plate_number, data.car_model, data.type_of_service, data.dealer_name
        //                            , data.remark, data.appt_date, data.appt_time);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return mail;
        //}

        public MailMessage SendEmailToCallCenter_TestDrive(TestDriveDetailForSendEmail data, byte[] attachFile, string attachFileName)
        {
            MailMessage mail = new MailMessage();
            string error_message;
            try
            {
                DateTime date_time_now = DateTime.Now;

                mail.From = new MailAddress(this.Sender, this.DisplayName);
                //mail.To.Add(new MailAddress(data.call_center_email.ToString()));
                foreach (var address_call_center in data.call_center_email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address_call_center);
                }
                foreach (var address_tcap in data.tcap_call_center_email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address_tcap);
                }
                mail.Attachments.Add(new Attachment(new MemoryStream(attachFile), attachFileName));
                mail.Subject = "มีรายการนัดหมายทดลองขับเข้ามาใหม่ เลขที่รายการนัดหมาย: " + data.code;
                mail.Body = string.Format(
                    "<html>" +
                      "<head> " +
                        "<meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\" > " +
                      "</head> " +
                         "<b>เรียน ท่านผู้แทนจำหน่ายเลกซัส<b> " +
                         "<br> " +
                         "<br> " +
                         "<br> " +
                         "ขณะนี้มีรายการนัดหมายทดลองขับที่ผู้แทนจำหน่ายเลกซัสของท่าน<br> " +
                         "กรุณาติดต่อกลับหาลูกค้าภายใน 24 ชม. ในเวลาทำการ หลังจากท่านได้รับอีเมลฉบับนี้ " +
                         "<br> " +
                         "<br> " +
                         "<b>เลขที่รายการนัดหมาย : {0}<b><br> " +
                         "<b>วันที่ทำรายการเข้ามา : {1}<b> " +
                         "<br> " +
                         "<br> " +
                         "<u>ข้อมูลเจ้าของรถ</u><br> " +
                         "ชื่อ : {2}<br> " +
                         "นามสกุล : {3}<br> " +
                         "เบอร์โทรติดต่อ : {4}<br> " +
                         "อีเมล : {5}<br> " +
                         "รุ่นรถที่ต้องการทดลองขับ : {6}<br> " +
                         "ผู้แทนจำหน่ายที่นัดหมายทดลองขับ : {7} ({10})<br> " +
                         "แผนการซื้อรถยนต์ : {8}<br> " +
                         "หมายเหตุ : {9}" +
                         "<br> " +
                         "<br> " +
                         "<br> " +
                         "หลังจากคอนเฟิร์มรายละเอียดการนัดหมายกับลูกค้าเรียบร้อยแล้ว กรุณากรอกรายละเอียดลงในแบบฟอร์มที่แนบมาในอีเมลฉบับนี้<br> " +
                         "แล้วส่งกลับมาที่ LEXUS Elite Club Call Center <br> " +
                         "อีเมล lexus02@toyotaconnected.co.th<br> " +
                         "เบอร์โทรศัพท์ 02-305-6799 ทุกวัน เวลา 10.00-20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์ " +
                      "</html> "
                      , data.code, data.create_date, data.name, data.surname, data.contact_number, data.email, data.preferred_model, data.dealer, data.car_purchase_plan, data.remark, data.branch_name);
            }
            catch (Exception ex)
            {
                error_message = ex.Message; //need to handle but not now.
                throw ex;
            }
            finally
            {
                //save to email log;
            }

            return mail;
        }

        public async Task<MailMessage> SendEmailToCallCenter_Onlinebooking(OnlineBookingExportExcelModel onlineBookingExportExcelModel,byte[] attachFile, string attachFileName)
        {
            MailMessage mail = new MailMessage();
            string error_message;
            try
            {
                DateTime date_time_now = DateTime.Now;

                if (bool.Parse(onlineBookingExportExcelModel.test_drive))
                {
                    onlineBookingExportExcelModel.test_drive = "สนใจ";
                }
                else
                {
                    onlineBookingExportExcelModel.test_drive = "ไม่สนใจ";
                }

                mail.From = new MailAddress(this.Sender, this.DisplayName);
                //mail.To.Add(new MailAddress(onlineBookingExportExcelModel.call_center_email.ToString()));
                foreach (var address_call_center in onlineBookingExportExcelModel.call_center_email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address_call_center);
                }
                foreach (var address_tcap in onlineBookingExportExcelModel.tcap_call_center_email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address_tcap);
                }

                mail.Attachments.Add(new Attachment(new MemoryStream(attachFile), attachFileName));
                mail.Subject = "ขณะนี้มีลูกค้าสนใจแคมเปญ " + onlineBookingExportExcelModel.title + " เลขที่รายการ " + attachFileName.Substring(11, 14);
                if(onlineBookingExportExcelModel.campaignRegisterModel.type == "1")
                {
                    mail.Body = string.Format(
                                       "<html>" +
                                         "<head> " +
                                           "<meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\" > " +
                                         "</head> " +
                                            "<b>เรียน ท่านผู้แทนจำหน่ายเลกซัส<b> " +
                                            "<br> " +
                                            "<br> " +
                                            "ขณะนี้มีลูกค้าสนใจแคมเปญ " + onlineBookingExportExcelModel.title + "<br> " +
                                            "รหัสอ้างอิง (Repurchase Code) : {0} <br> " +
                                            "กรุณาติดต่อกลับหาลูกค้าภายใน 24 ชม. ในเวลาทำการ หลังจากท่านได้รับอีเมลฉบับนี้ " +
                                            "<br> " +
                                            "<br> " +
                                            "ชื่อ-นามสกุล : {1} {2}<br> " +
                                            "เบอร์โทรติดต่อ : {3}<br> " +
                                            "อีเมล : {4}<br> " +
                                            "รุ่นรถที่สนใจ : {5}<br> " +
                                            "ดีลเลอร์ที่สนใจ : {6}<br> " +
                                            "สนใจ Test Drive : {7}<br> " +
                                            "<br> " +
                                            "<br> " +
                                            "หลังจากติดต่อกลับหาลูกค้าเรียบร้อยแล้ว กรุณากรอกรายละเอียดลงในแบบฟอร์มที่แนบมาในอีเมลฉบับนี้<br> " +
                                            "แล้วส่งกลับมาที่ LEXUS Elite Club Call Center <br> " +
                                            "อีเมล lexus02@toyotaconnected.co.th<br> " +
                                            "เบอร์โทรศัพท์ 02-305-6799 ทุกวัน เวลา 10.00-20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์ " +
                                         "</html> "
                                         , onlineBookingExportExcelModel.campaignRegisterModel.booking_code, onlineBookingExportExcelModel.campaignRegisterModel.name, onlineBookingExportExcelModel.campaignRegisterModel.surname, onlineBookingExportExcelModel.campaignRegisterModel.contact_number, onlineBookingExportExcelModel.campaignRegisterModel.email,
                                         onlineBookingExportExcelModel.car_model, onlineBookingExportExcelModel.dealer_name, onlineBookingExportExcelModel.test_drive);
                }
                else if (onlineBookingExportExcelModel.campaignRegisterModel.type == "2")
                {
                    mail.Body = string.Format(
                                                          "<html>" +
                                                            "<head> " +
                                                              "<meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\" > " +
                                                            "</head> " +
                                                               "<b>เรียน ท่านผู้แทนจำหน่ายเลกซัส<b> " +
                                                               "<br> " +
                                                               "<br> " +
                                                               "ขณะนี้มีลูกค้าสนใจแคมเปญ " + onlineBookingExportExcelModel.title + "<br> " +
                                                               "รหัสอ้างอิง (Referral Code) : {0} <br> " +
                                                               "กรุณาติดต่อกลับหาลูกค้าภายใน 24 ชม. ในเวลาทำการ หลังจากท่านได้รับอีเมลฉบับนี้ " +
                                                               "<br> " +
                                                               "<br> " +
                                                               "<b>รายละเอียดผู้ได้รับการแนะนำ<b><br> " +
                                                               "ชื่อ-นามสกุล : {7} {8}<br> " +
                                                               "เบอร์โทรติดต่อ : {9}<br> " +
                                                               "อีเมล : {10}<br> " +
                                                               "รุ่นรถที่สนใจ : {5}<br> " +
                                                               "ดีลเลอร์ที่สนใจ : {6}<br> " +
                                                               "<br> " +
                                                               "<br> " +
                                                               "<b>รายละเอียดลูกค้าเลกซัสผู้แนะนำ<b><br> " +
                                                               "ชื่อ-นามสกุล : {1} {2}<br> " +
                                                               "เบอร์โทรติดต่อ : {3}<br> " +
                                                               "อีเมล : {4}<br> " +
                                                               "เลขทะเบียนรถ : {11}<br> " +
                                                               "รุ่นรถ : {12}<br> " +
                                                               "<br> " +
                                                               "<br> " +
                                                               "หลังจากติดต่อกลับหาลูกค้าเรียบร้อยแล้ว กรุณากรอกรายละเอียดลงในแบบฟอร์มที่แนบมาในอีเมลฉบับนี้<br> " +
                                                               "แล้วส่งกลับมาที่ LEXUS Elite Club Call Center <br> " +
                                                               "อีเมล lexus02@toyotaconnected.co.th<br> " +
                                                               "เบอร์โทรศัพท์ 02-305-6799 ทุกวัน เวลา 10.00-20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์ " +
                                                            "</html> "
                                                            , onlineBookingExportExcelModel.campaignRegisterModel.booking_code, onlineBookingExportExcelModel.campaignRegisterModel.name, onlineBookingExportExcelModel.campaignRegisterModel.surname, onlineBookingExportExcelModel.campaignRegisterModel.contact_number, onlineBookingExportExcelModel.campaignRegisterModel.email,
                                                            onlineBookingExportExcelModel.car_model, onlineBookingExportExcelModel.dealer_name, onlineBookingExportExcelModel.campaignRegisterModel.referral_name, onlineBookingExportExcelModel.campaignRegisterModel.referral_surname, onlineBookingExportExcelModel.campaignRegisterModel.referral_contact_number
                                                            , onlineBookingExportExcelModel.campaignRegisterModel.referral_email, onlineBookingExportExcelModel.campaignRegisterModel.plate_number, onlineBookingExportExcelModel.campaignRegisterModel.car_model);
                }
                else if (onlineBookingExportExcelModel.campaignRegisterModel.type == "3")
                {
                    mail.Body = string.Format(
                                                          "<html>" +
                                                            "<head> " +
                                                              "<meta http - equiv = \"Content-Type\" content = \"text/html; charset=utf-8\" > " +
                                                            "</head> " +
                                                               "<b>เรียน ท่านผู้แทนจำหน่ายเลกซัส<b> " +
                                                               "<br> " +
                                                               "<br> " +
                                                               "ขณะนี้มีลูกค้าสนใจแคมเปญ " + onlineBookingExportExcelModel.title + "<br> " +
                                                               "รหัสอ้างอิง (Booking Code) : {0} <br> " +
                                                               "กรุณาติดต่อกลับหาลูกค้าภายใน 24 ชม. ในเวลาทำการ หลังจากท่านได้รับอีเมลฉบับนี้ " +
                                                               "<br> " + 
                                                               "<br> " +
                                                               "ชื่อ-นามสกุล : {1} {2}<br> " +
                                                               "เบอร์โทรติดต่อ : {3}<br> " +
                                                               "อีเมล : {4}<br> " +
                                                               "รุ่นรถที่สนใจ : {5}<br> " +
                                                               "ดีลเลอร์ที่สนใจ : {6}<br> " +
                                                               "<br> " +
                                                               "<br> " +
                                                               "หลังจากติดต่อกลับหาลูกค้าเรียบร้อยแล้ว กรุณากรอกรายละเอียดลงในแบบฟอร์มที่แนบมาในอีเมลฉบับนี้<br> " +
                                                               "แล้วส่งกลับมาที่ LEXUS Elite Club Call Center <br> " +
                                                               "อีเมล lexus02@toyotaconnected.co.th<br> " +
                                                               "เบอร์โทรศัพท์ 02-305-6799 ทุกวัน เวลา 10.00-20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์ " +
                                                            "</html> "
                                                            , onlineBookingExportExcelModel.campaignRegisterModel.booking_code, onlineBookingExportExcelModel.campaignRegisterModel.name, onlineBookingExportExcelModel.campaignRegisterModel.surname, onlineBookingExportExcelModel.campaignRegisterModel.contact_number, onlineBookingExportExcelModel.campaignRegisterModel.email,
                                                            onlineBookingExportExcelModel.car_model, onlineBookingExportExcelModel.dealer_name);
                }
            }
            catch (Exception ex)
            {
                error_message = ex.Message; //need to handle but not now.
                throw ex;
            }
            finally
            {
                //save to email log;
            }

            return mail;
        }

        public MailMessage SendEmailOfficialTest(int refKey)
        {
            MailMessage mail = new MailMessage();
            string error_message;
            try
            {
                DateTime date_time_now = DateTime.Now;

                mail.From = new MailAddress("palapong@feyverly.com", "Official Test E-mail");
                mail.To.Add(new MailAddress("palapong@feyverly.com"));
                mail.Subject = "This is office test send email from server with ref key: " + refKey.ToString() + date_time_now;
                mail.Body = string.Format(
                    "<html>" +
                         "<u> Official Test Mail. </u><br> " +
                         "<br> " +
                         "<br> " +
                         "<br> " +
                         "Mail send by customer enviroment <br> " +
                      "</html> ");
            }
            catch (Exception ex)
            {
                error_message = ex.Message; //need to handle but not now.
                throw ex;
            }
            finally
            {
                //save to email log;
            }

            return mail;
        }

        public int SaveEmailLog(string template_no,string subject, string header, string body, string send_from, string send_to, string cc, string bcc, bool is_attach_file, int status, string user_name)
        {
            int inserted_id = 0;
            string status_ = "";

            if (string.IsNullOrEmpty(user_name))
            {
                user_name = "ADMIN";
            }


            switch (status)
            {
                case 1:
                    status_ = "success";
                    break;
                case 2:
                    status_ = "error";
                    break;
            }
           


            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                              INSERT INTO utility_email_log
                                  (template_no, subject, header, body, send_from, send_to, cc, bcc, is_attach_file, status, created_date, created_user)
                              OUTPUT Inserted.id
                              VALUES
                              (N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}', N'{6}', N'{7}', N'{8}', N'{9}', SYSDATETIME(), N'{10}') ";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, template_no, subject, header, body, send_from, send_to, cc, bcc, is_attach_file, status_, user_name)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            inserted_id = Convert.ToInt16(row["id"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "SaveEmailLog");
                throw ex;
            }
            return inserted_id;
        }

        public bool UpdateEmailLog(int log_id, int status)
        {
            bool save_log_success = false;
            string status_ = "";


            switch (status)
            {
                case 1:
                    status_ = "success";
                    break;
                case 2:
                    status_ = "error";
                    break;
            }

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @" UPDATE [dbo].[utility_email_log]
                                    SET status = N'{0}'
                                    OUTPUT INSERTED.PrimaryKeyID
                                    WHERE id = N{1} ";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, status_, log_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt16(row["id"]) > 0)
                            {
                                save_log_success = true;
                            } 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "UpdateEmailLog");
                throw ex;
            }

            return save_log_success;
        }

    }
}