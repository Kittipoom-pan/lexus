using AppLibrary.Schedule;
using MultipartDataMediaFormatter.Infrastructure;
﻿using com.lexus.th.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using com.lexus.th.Services;

namespace com.lexus.th.Controllers
{
    #region A P I  1.0

    //[RoutePrefix("api/1.0")]
    //public class OldServiceController : ApiController
    //{
    //    [Route("home")]
    //    [HttpPost]
    //    public IHttpActionResult GetHomeData(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        try
    //        {
    //            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");

    //            HomeService srv = new HomeService();
    //            var obj = srv.GetScreenData(v, p, token);
    //            return Ok(obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("profile/get")]
    //    [HttpPost]
    //    public IHttpActionResult GetProfileData(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                ProfileService srv = new ProfileService();
    //                var obj = srv.GetProfileData(token, v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("profile/update")]
    //    [HttpPost]
    //    public IHttpActionResult UpdateProfileData(FormData formData, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formData == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formData.GetValues("token", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();
    //            HttpFile image = formData.GetFiles("image", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();
    //            string name = formData.GetValues("name", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();

    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                ProfileService srv = new ProfileService();
    //                var obj = srv.UpdateProfileData(token, image, name, v, p);
    //                //return Ok(obj);
    //                return Json(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("history")]
    //    [HttpPost]
    //    public IHttpActionResult GetHistoryData(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");

    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                HistoryService srv = new HistoryService();
    //                var obj = srv.GetHistory(token, v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("redeem")]
    //    [HttpPost]
    //    public IHttpActionResult RedeemItem(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            int privilege_id = Convert.ToInt32(formDataCollection.Get("privilege_id"));

    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                RedeemService srv = new RedeemService();
    //                var obj = srv.RedeemItem(token, privilege_id, v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("shop_verify")]
    //    [HttpPost]
    //    public IHttpActionResult ShopVerifyItem(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            string code = formDataCollection.Get("code");
    //            int redeem_id = Convert.ToInt32(formDataCollection.Get("redeem_id"));

    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                ShopVerifyService srv = new ShopVerifyService();
    //                var obj = srv.VerifyItem(token, code, redeem_id, v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("event_reg")]
    //    [HttpPost]
    //    public IHttpActionResult EventRegister(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            string member_title = formDataCollection.Get("member_title");
    //            string member_name = formDataCollection.Get("member_name");
    //            string member_birthdate = formDataCollection.Get("member_birthdate");
    //            string member_email = formDataCollection.Get("member_email");
    //            string member_tel_no = formDataCollection.Get("member_tel_no");
    //            string member_dealer = formDataCollection.Get("member_dealer");
    //            string follower_title = formDataCollection.Get("follower_title");
    //            string follower_name = formDataCollection.Get("follower_name");
    //            string follower_birthdate = formDataCollection.Get("follower_birthdate");
    //            string follower_email = formDataCollection.Get("follower_email");
    //            string follower_tel_no = formDataCollection.Get("follower_tel_no");
    //            string follower_car_model = formDataCollection.Get("follower_car_model");
    //            string follower_year_purchase = formDataCollection.Get("follower_year_purchase");
    //            int event_id = Convert.ToInt32(formDataCollection.Get("event_id"));



    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                EventRegisterServie srv = new EventRegisterServie();
    //                var obj = srv.EventRegister(event_id, token, member_title, member_name, member_birthdate, member_email, member_tel_no, member_dealer, follower_title, follower_name, follower_birthdate, follower_email, follower_tel_no, follower_car_model, follower_year_purchase, v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("login")]
    //    [HttpPost]
    //    public IHttpActionResult Login(FormDataCollection formDataCollection, string v, string p, string m, string uid)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string member_id = formDataCollection.Get("member_id");
    //            string phone_no = formDataCollection.Get("phone_no");
    //            string notif_token = formDataCollection.Get("notif_token");

    //            LoginService srv = new LoginService();
    //            var obj = srv.Login(member_id, phone_no, v, p, m, uid, notif_token);
    //            return Ok(obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("login_otp")]
    //    [HttpPost]
    //    public IHttpActionResult LoginOTP(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string otp_token = formDataCollection.Get("otp_token");
    //            string otp = formDataCollection.Get("otp");

    //            LoginOTPService srv = new LoginOTPService();
    //            var obj = srv.LoginOTP(otp_token, otp, v, p);
    //            return Ok(obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("logout")]
    //    [HttpPost]
    //    public IHttpActionResult Logout(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                LogoutService srv = new LogoutService();
    //                var obj = srv.Logout(token, v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("test_sms")]
    //    [HttpPost]
    //    public IHttpActionResult TestSMS(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string mobile = formDataCollection.Get("mobile");
    //            string msg = formDataCollection.Get("msg");

    //            SMSService sms = new SMSService();
    //            SMSModel result = sms.SendSMSTest(mobile, msg);

    //            return Ok(result);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("sms_report")]
    //    [HttpPost]
    //    public HttpResponseMessage SMSReport(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string CMD = formDataCollection.Get("CMD");
    //            string NTYPE = formDataCollection.Get("NTYPE");
    //            string FROM = formDataCollection.Get("FROM");
    //            string SMID = formDataCollection.Get("SMID");
    //            string STATUS = formDataCollection.Get("STATUS");
    //            string DETAIL = formDataCollection.Get("DETAIL");

    //            SMSService sms = new SMSService();
    //            SMSReportModel result = sms.GetSMSReport(CMD, NTYPE, FROM, SMID, STATUS, DETAIL);

    //            return new HttpResponseMessage()
    //            {
    //                Content = new StringContent(
    //                    string.Format(@"<XML><STATUS>{0}</STATUS><DETAIL>{1}</DETAIL></XML>", result.STATUS, result.DETAIL),
    //                    System.Text.Encoding.UTF8,
    //                    "application/xml"
    //                )
    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("geo")]
    //    [HttpPost]
    //    public IHttpActionResult GetGeoData(string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            GeoService srv = new GeoService();
    //            var obj = srv.GetScreenData(v, p);
    //            return Ok(obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("dealer")]
    //    [HttpPost]
    //    public IHttpActionResult GetDealerData(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            //string token = formData.GetValues("token", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();
    //            //string geo_id = formDataCollection.Get("GEOID");
    //            int geoID= Convert.ToInt32(formDataCollection.Get("GEOID"));
    //            //formData.GetValues("geo_id", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();

    //            DealerService srv = new DealerService();
    //            var obj = srv.GetScreenData(geoID, v, p);
    //            return Json(obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("system_app")]
    //    [HttpPost]
    //    public IHttpActionResult CheckMaintenanceAndUpdate(FormDataCollection formDataCollection)
    //    {
    //        if (formDataCollection == null)
    //        {
    //            throw new Exception("Not found form-data.");
    //        }

    //        string device_type = formDataCollection.Get("device_type");
    //        string app_version = formDataCollection.Get("app_version");

    //        SystemAppService srv = new SystemAppService();
    //        var obj = srv.CheckSystemApp(device_type, app_version);
    //        return Ok(obj);
    //    }

    //    [Route("redeem_new")]
    //    [HttpPost]
    //    public IHttpActionResult RedeemNewItem(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            int privilege_id = Convert.ToInt32(formDataCollection.Get("privilege_id"));

    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                RedeemNewService srv = new RedeemNewService();
    //                var obj = srv.RedeemItem(token, privilege_id, v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceNewRedeemModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("notification")]
    //    [HttpPost]
    //    public IHttpActionResult GetNotification(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");

    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                NotificationService srv = new NotificationService();
    //                var obj = srv.GetScreenData(v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceNotificationModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("privilege/detail")]
    //    [HttpPost]
    //    public IHttpActionResult GetPrivilegeDetail(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            string privilege_id = formDataCollection.Get("privilege_id");

    //            PrivilegeService srv = new PrivilegeService();
    //            var obj = srv.GetScreenData(v, p, privilege_id);
    //            return Ok(obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("news/detail")]
    //    [HttpPost]
    //    public IHttpActionResult GetNewsDetail(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            string news_id = formDataCollection.Get("news_id");

    //            NewsService srv = new NewsService();
    //            var obj = srv.GetScreenData(v, p, news_id);
    //            return Ok(obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("event/detail")]
    //    [HttpPost]
    //    public IHttpActionResult GetEventDetail(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            string event_id = formDataCollection.Get("event_id");

    //            EventService srv = new EventService();
    //            var obj = srv.GetScreenData(v, p, token, event_id);
    //            return Ok(obj);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("allnotification")]
    //    [HttpPost]
    //    public IHttpActionResult GetAllNotification(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");

    //            TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, "EN");
    //            if (tokenResult.ResultCode == 1)
    //            {
    //                NotificationService srv = new NotificationService();
    //                var obj = srv.GetScreenData2(v, p);
    //                return Ok(obj);
    //            }
    //            else
    //            {
    //                return Ok(new ServiceNotificationModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("updatetoken")]
    //    [HttpPost]
    //    public IHttpActionResult GetUpdateToken(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string token = formDataCollection.Get("token");
    //            string device_token = formDataCollection.Get("device_token");
    //            string device_id = formDataCollection.Get("device_id");
    //            string device_type = formDataCollection.Get("device_type");

    //            DeviceService srv = new DeviceService();
    //            var obj = srv.UpdateDevice(device_id, device_token, device_type, token);
    //            return Ok(obj);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("push/device")]
    //    [HttpPost]
    //    public IHttpActionResult PushToken(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string device_token = formDataCollection.Get("device_token");
    //            string device_id = formDataCollection.Get("device_id");
    //            string device_type = formDataCollection.Get("device_type");

    //            DeviceService srv = new DeviceService();
    //            var obj = srv.PushDevice(device_id, device_token, device_type);
    //            return Ok(obj);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("message/mobile")]
    //    [HttpPost]
    //    public IHttpActionResult GetMessageMobile(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string device_type = formDataCollection.Get("device_type");
    //            string lang = formDataCollection.Get("lang");

    //            MessageMobileService srv = new MessageMobileService();
    //            var obj = srv.GetMessageMobile(device_type, lang);
    //            return Ok(obj);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("province")]
    //    [HttpPost]
    //    public IHttpActionResult GetProvince(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string lang = formDataCollection.Get("lang");

    //            ProvinceService srv = new ProvinceService();
    //            var obj = srv.GetDataProvince(lang, v, p);
    //            return Ok(obj);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("district")]
    //    [HttpPost]
    //    public IHttpActionResult GetDistrict(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string lang = formDataCollection.Get("lang");
    //            int province_id = Convert.ToInt32(formDataCollection.Get("province_id"));

    //            DistrictService srv = new DistrictService();
    //            var obj = srv.GetDataDistrict(lang, province_id, v, p);
    //            return Ok(obj);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("sub_district")]
    //    [HttpPost]
    //    public IHttpActionResult GetSubDistrict(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string lang = formDataCollection.Get("lang");
    //            int province_id = Convert.ToInt32(formDataCollection.Get("province_id"));
    //            int district_id = Convert.ToInt32(formDataCollection.Get("district_id"));

    //            SubDistrictService srv = new SubDistrictService();
    //            var obj = srv.GetDataSubDistrict(lang, province_id, district_id, v, p);
    //            return Ok(obj);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("preference")]
    //    [HttpPost]
    //    public IHttpActionResult GetPreference(FormDataCollection formDataCollection, string v, string p)
    //    {
    //        //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
    //        try
    //        {
    //            if (formDataCollection == null)
    //            {
    //                throw new Exception("Not found form-data.");
    //            }

    //            string lang = formDataCollection.Get("lang");
    //            int customer_id = Convert.ToInt32(formDataCollection.Get("customer_id"));

    //            PreferenceService srv = new PreferenceService();
    //            var obj = srv.GetDataPreference(lang, customer_id, v, p);
    //            return Ok(obj);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
    //        }
    //    }

    //    [Route("ip")]
    //    [HttpGet]
    //    public string GetIP()
    //    {


    //        var context = System.Web.HttpContext.Current;
    //        string ip = String.Empty;

    //        //if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
    //        //    ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
    //        //else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
    //        //    ip = context.Request.UserHostAddress;

    //        //if (ip == "::1")
    //        //    ip = "127.0.0.1";

    //        //ip = ip.Split(':')[0];

    //        string json = ip;
    //        LogService _log = new LogService();
    //        _log.InsertLogReceiveData("ทดสอบ schedule", "ทดสอบ");

    //        return ip;
    //    }
    //}

    #endregion



    [RoutePrefix("api/2.0")]
    public class NewServiceController : ApiController
    {
        [Route("home")]
        [HttpPost]
        public async Task<IHttpActionResult> GetHomeData(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetHomeData", json);


                HomeService srv = new HomeService();
                var obj = await srv.GetScreenDataNew(v, p, token, lang);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("profile/get")]
        [HttpPost]
        public async Task<IHttpActionResult> GetProfileData(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetProfileData", json);

                TokenService.TokenServicResult tokenResult = await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    ProfileService srv = new ProfileService();
                    var obj = await srv.GetProfileDataNew(token, v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("profile/update")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateProfileData(HttpRequestMessage request, string v, string p)
        {

            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (request == null)    
                {
                    throw new Exception("Not found form-data.");
                }

                ProfileUpdateModel profile = new ProfileUpdateModel();
                profile.token = HttpContext.Current.Request.Form["token"];
                HttpPostedFile imageFile = HttpContext.Current.Request.Files["image"];
                profile.lang = HttpContext.Current.Request.Form["lang"];
                profile.name = HttpContext.Current.Request.Form["name"];

                Stream fs = imageFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                profile.image = JsonConvert.SerializeObject(
                new
                {
                    FileName = imageFile.FileName,
                    MediaType = imageFile.ContentType,
                    Buffer = bytes
                });

                //ProfileUpdateModel[] arr = new ProfileUpdateModel[] { profile };
                //string json_data = JsonConvert.SerializeObject(arr);

                //string token = formData.GetValues("token", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();
                //HttpFile image = formData.GetFiles("image", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();
                //string name = formData.GetValues("name", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();
                //string lang = formData.GetValues("lang", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();

                string json = JsonConvert.SerializeObject(profile);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("UpdateProfileData", json);

                TokenService.TokenServicResult tokenResult = await TokenService.CheckTokenResult(profile.token, profile.lang);
                //if (tokenResult.ResultCode == 1)
                //{
                    ProfileService srv = new ProfileService();
                    var obj = await srv.UpdateProfileDataNew(profile.token, profile.image, profile.name, v, p, profile.lang, bytes);
                    //return Ok(obj);
                    return Json(obj);
                //}
                //else
                //{
                //    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                //}
                //return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = 200, text = "" } });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("history")]
        [HttpPost]
        public async Task<IHttpActionResult> GetHistoryData(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await _log.InsertLogReceiveData("GetHistoryData", json);

                TokenService.TokenServicResult tokenResult =await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    HistoryService srv = new HistoryService();
                    var obj = await srv.GetHistoryNew(token, v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        //[Route("redeem")]
        //[HttpPost]
        //public IHttpActionResult RedeemItem(FormDataCollection formDataCollection, string v, string p)
        //{
        //    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
        //    try
        //    {
        //        if (formDataCollection == null)
        //        {
        //            throw new Exception("Not found form-data.");
        //        }

        //        string token = formDataCollection.Get("token");
        //        int privilege_id = Convert.ToInt32(formDataCollection.Get("privilege_id"));
        //        string lang = formDataCollection.Get("lang");

        //        string json = JsonConvert.SerializeObject(formDataCollection);
        //        LogService _log = new LogService();
        //        _log.InsertLogReceiveData("RedeemItem", json);

        //        TokenService.TokenServicResult tokenResult = TokenService.CheckTokenResult(token, lang);
        //        if (tokenResult.ResultCode == 1)
        //        {
        //            RedeemService srv = new RedeemService();
        //            var obj = srv.RedeemItemNew(token, privilege_id, v, p, lang);
        //            return Ok(obj);
        //        }
        //        else
        //        {
        //            return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}

        [Route("redeem/old")]
        [HttpPost]
        public async Task<IHttpActionResult> RedeemItemOld(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                int privilege_id = Convert.ToInt32(formDataCollection.Get("privilege_id"));
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("RedeemItemOld", json);

                TokenService.TokenServicResult tokenResult = await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    RedeemService srv = new RedeemService();
                    var obj = await srv.RedeemItemOld(token, privilege_id, v, p, lang);

                    //_log.InsertLogReceiveData("RedeemItemOld", JsonConvert.SerializeObject(obj));
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("redeem_new")]
        [HttpPost]
        public async Task<IHttpActionResult> RedeemNewItem(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                int privilege_id = Convert.ToInt32(formDataCollection.Get("privilege_id"));
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("RedeemNewItem", json);

                TokenService.TokenServicResult tokenResult =await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    RedeemNewService srv = new RedeemNewService();
                    var obj = await srv.RedeemItemNew(token, privilege_id, v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceNewRedeemModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("shop_verify")]
        [HttpPost]
        public async Task<IHttpActionResult> ShopVerifyItem(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string code = formDataCollection.Get("code");
                int redeem_id = Convert.ToInt32(formDataCollection.Get("redeem_id"));
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("ShopVerifyItem", json);

                TokenService.TokenServicResult tokenResult = await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    ShopVerifyService srv = new ShopVerifyService();
                    var obj = await srv.VerifyItemNew(token, code, redeem_id, v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("event_reg")]
        [HttpPost]
        public async Task<IHttpActionResult> EventRegister(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string member_title = formDataCollection.Get("member_title");
                string member_name = formDataCollection.Get("member_name");
                string member_birthdate = formDataCollection.Get("member_birthdate");
                string member_email = formDataCollection.Get("member_email");
                string member_tel_no = formDataCollection.Get("member_tel_no");
                string member_dealer = formDataCollection.Get("member_dealer");
                string follower_title = formDataCollection.Get("follower_title");
                string follower_name = formDataCollection.Get("follower_name");
                string follower_birthdate = formDataCollection.Get("follower_birthdate");
                string follower_email = formDataCollection.Get("follower_email");
                string follower_tel_no = formDataCollection.Get("follower_tel_no");
                string follower_car_model = formDataCollection.Get("follower_car_model");
                //string follower_year_purchase = formDataCollection.Get("follower_year_purchase");
                int follower_year_purchase_id = Convert.ToInt32(formDataCollection.Get("follower_year_purchase_id"));
                int event_id = Convert.ToInt32(formDataCollection.Get("event_id"));
                string lang = formDataCollection.Get("lang");
                string vin = formDataCollection.Get("vin");

                int relationship_id = Convert.ToInt32(formDataCollection.Get("relationship_id"));
                string relationship_special = formDataCollection.Get("relationship_special");
                int car_brand_id = Convert.ToInt32(formDataCollection.Get("car_brand_id"));
                string car_brand_special = formDataCollection.Get("car_brand_special");

                bool is_car_owner = formDataCollection.Get("is_car_owner") == "1" ? true : false;
                bool has_follower = formDataCollection.Get("has_follower") == "1" ? true : false;
                List<EventAnswer> member_answers = JsonConvert.DeserializeObject<List<EventAnswer>>(formDataCollection.Get("member_answers"));
                List<EventAnswer> follower_answers = JsonConvert.DeserializeObject<List<EventAnswer>>(formDataCollection.Get("follower_answers"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("EventRegister", json);

                TokenService.TokenServicResult tokenResult =await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    EventRegisterServie srv = new EventRegisterServie();
                    var obj = await srv.EventRegisterNew(event_id, token, member_title, member_name, member_birthdate,
                        member_email, member_tel_no, member_dealer, follower_title, follower_name, follower_birthdate,
                        follower_email, follower_tel_no, follower_car_model, follower_year_purchase_id, v, p, lang, vin,
                        relationship_id, relationship_special, car_brand_id, car_brand_special, is_car_owner, member_answers, follower_answers, has_follower);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("event_reg/check")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckEventRegister(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                int event_id = Convert.ToInt32(formDataCollection.Get("event_id"));
                string lang = formDataCollection.Get("lang");


                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await  _log.InsertLogReceiveData("CheckEventRegister", json);

                TokenService.TokenServicResult tokenResult =await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1 || string.IsNullOrEmpty(token))
                {
                    EventRegisterServie srv = new EventRegisterServie();
                    var obj = await srv.CheckEventRegister(event_id, token, v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(FormDataCollection formDataCollection, string v, string p, string m, string uid)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string member_id = formDataCollection.Get("member_id");
                string phone_no = formDataCollection.Get("phone_no");
                string notif_token = formDataCollection.Get("notif_token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("Login", json);

                LoginService srv = new LoginService();
                var obj = await srv.Login(member_id, phone_no, v, p, m, uid, notif_token);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("login/new")]
        [HttpPost]
        public async Task<IHttpActionResult> LoginNew(FormDataCollection formDataCollection, string v, string p, string m, string uid)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string phone_no = formDataCollection.Get("phone_no");
                string lang = formDataCollection.Get("lang");
                string type = formDataCollection.Get("type");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("LoginNew", json);

                if (type == null)
                {
                    type = "";
                }

                LoginService srv = new LoginService();
                var obj = await srv.LoginNew(phone_no, v, p, m, uid, lang, type);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("first_login")]
        [HttpPost]
        public async Task<IHttpActionResult> first_login(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("first_login", json);

                FirstLoginService srv = new FirstLoginService();
                var obj = await srv.StampFirstLogin(v, p, token, lang);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("login_otp")]
        [HttpPost]
        public async Task<IHttpActionResult> LoginOTP(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }
                string otp_token = formDataCollection.Get("otp_token");
                string otp = formDataCollection.Get("otp");
                string lang = formDataCollection.Get("lang");
                string type = formDataCollection.Get("type");
                string device_id = formDataCollection.Get("device_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await  _log.InsertLogReceiveData("LoginOTP", json);

                if (type == null)
                {
                    type = "";
                }

                LoginOTPService srv = new LoginOTPService();
                var obj = await srv.LoginOTPNew(otp_token, otp, v, p, lang, type, device_id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("request/otp")]
        [HttpPost]
        public async Task<IHttpActionResult> RequestOTP(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string mobile = formDataCollection.Get("mobile");
                string type = formDataCollection.Get("type");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("RequestOTP", json);

                if (type == null)
                {
                    type = "";
                }

                OTPService srv = new OTPService();
                var obj = await srv.RequestOTP(v, p, lang, mobile, type);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("check/otp")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckOTP(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string mobile = formDataCollection.Get("mobile");
                string refer_code = formDataCollection.Get("refer_code");
                string otp_code = formDataCollection.Get("otp_code");
                string type = formDataCollection.Get("type");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("CheckOTP", json);

                if (type == null)
                {
                    type = "";
                }

                OTPService srv = new OTPService();
                var obj = await srv.CheckOTP(v, p, lang, mobile, refer_code, otp_code, type);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("logout")]
        [HttpPost]
        public async Task<IHttpActionResult> Logout(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await _log.InsertLogReceiveData("Logout", json);

                TokenService.TokenServicResult tokenResult = await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    LogoutService srv = new LogoutService();
                    var obj = await srv.LogoutNew(token, v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("test_sms")]
        [HttpPost]
        public async Task<IHttpActionResult> TestSMS(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string mobile = formDataCollection.Get("mobile");
                string msg = formDataCollection.Get("msg");

                SMSService sms = new SMSService();
                SMSModel result = await sms.SendSMS(mobile, msg);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sms_report")]
        [HttpPost]
        public async Task<HttpResponseMessage>  SMSReport(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string CMD = formDataCollection.Get("CMD");
                string NTYPE = formDataCollection.Get("NTYPE");
                string FROM = formDataCollection.Get("FROM");
                string SMID = formDataCollection.Get("SMID");
                string STATUS = formDataCollection.Get("STATUS");
                string DETAIL = formDataCollection.Get("DETAIL");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await  _log.InsertLogReceiveData("SMSReport", json);

                SMSService sms = new SMSService();
                SMSReportModel result = sms.GetSMSReport(CMD, NTYPE, FROM, SMID, STATUS, DETAIL);

                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                        string.Format(@"<XML><STATUS>{0}</STATUS><DETAIL>{1}</DETAIL></XML>", result.STATUS, result.DETAIL),
                        System.Text.Encoding.UTF8,
                        "application/xml"
                    )
                };
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("geo")]
        [HttpPost]
        public async Task<IHttpActionResult> GetGeoData(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                string lang = formDataCollection.Get("lang");
                string type = formDataCollection.Get("type");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetGeoData", json);

                GeoService srv = new GeoService();
                var obj = await srv.GetScreenDataNew(v, p, lang, type);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("dealer")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDealerData(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                //string token = formData.GetValues("token", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();
                //string geo_id = formDataCollection.Get("GEOID");
                int geoID = Convert.ToInt32(formDataCollection.Get("GEOID"));
                string lang = formDataCollection.Get("lang");
                string type = formDataCollection.Get("type");
                //formData.GetValues("geo_id", new System.Globalization.CultureInfo("en-US")).FirstOrDefault();

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetDealerData", json);

                DealerService srv = new DealerService();
                var obj = await srv.GetScreenDataNew(geoID, v, p, lang, type);
                return Json(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("system_app")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckMaintenanceAndUpdate(FormDataCollection formDataCollection, string v, string p)
        {
            if (formDataCollection == null)
            {
                throw new Exception("Not found form-data.");
            }

            string device_type = formDataCollection.Get("device_type");
            string app_version = (device_type.ToLower() == "ios") ? v : formDataCollection.Get("app_version");
            string lang = formDataCollection.Get("lang");

            string json = JsonConvert.SerializeObject(formDataCollection);
            LogService _log = new LogService();
            await _log.InsertLogReceiveData("CheckMaintenanceAndUpdate", json);

            SystemAppService srv = new SystemAppService();
            var obj = await srv.CheckSystemAppNew(device_type, app_version, lang);
            return Ok(obj);
        }

        [Route("notification")]
        [HttpPost]
        public async Task<IHttpActionResult> GetNotification(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetNotification", json);

                TokenService.TokenServicResult tokenResult = await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    NotificationService srv = new NotificationService();
                    var obj = await srv.GetScreenDataNew(v, p, lang, token);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceNotificationModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("privilege/detail")]
        [HttpPost]
        public async Task<IHttpActionResult> GetPrivilegeDetail(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string privilege_id = formDataCollection.Get("privilege_id");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await  _log.InsertLogReceiveData("GetPrivilegeDetail", json);

                PrivilegeService srv = new PrivilegeService();
                ServicePrivilegeModel obj =await srv.GetScreenDataNew(v, p, token, privilege_id, lang);
                if (obj.msg.code == 20014002 || obj.msg.code == 20014001)
                {
                    obj.success = true;
                    obj.msg.code = 0;
                    obj.msg.text = "";
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("news/detail")]
        [HttpPost]
        public async Task<IHttpActionResult> GetNewsDetail(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string news_id = formDataCollection.Get("news_id");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetNewsDetail", json);

                NewsService srv = new NewsService();
                var obj = await srv.GetScreenDataNew(v, p, news_id, lang);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("event/detail")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEventDetail(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string event_id = formDataCollection.Get("event_id");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetEventDetail", json);

                EventService srv = new EventService();
                var obj =await srv.GetScreenDataNew(v, p, token, event_id, lang);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("allnotification")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllNotification(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await _log.InsertLogReceiveData("GetAllNotification", json);

                TokenService.TokenServicResult tokenResult =await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1)
                {
                    NotificationService srv = new NotificationService();
                    var obj =await srv.GetScreenDataAll(v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceNotificationModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("updatetoken")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUpdateToken(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string device_token = formDataCollection.Get("device_token");
                string device_id = formDataCollection.Get("device_id");
                string device_type = formDataCollection.Get("device_type");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetUpdateToken", json);

                DeviceService srv = new DeviceService();
                var obj = await srv.UpdateDeviceNew(device_id, device_token, device_type, token, lang);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "updatetoken");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("push/device")]
        [HttpPost]
        public async Task<IHttpActionResult> PushToken(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string device_token = formDataCollection.Get("device_token");
                string device_id = formDataCollection.Get("device_id");
                string device_type = formDataCollection.Get("device_type");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("PushToken", json);

                DeviceService srv = new DeviceService();
                var obj = await srv.PushDeviceNew(device_id, device_token, device_type, lang);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("message/mobile")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMessageMobile(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string device_type = formDataCollection.Get("device_type");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await  _log.InsertLogReceiveData("GetMessageMobile", json);

                MessageMobileService srv = new MessageMobileService();
                var obj = await srv.GetMessageMobile(device_type, lang);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("check/new/customer")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckRegisterCustomer(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string mobile = formDataCollection.Get("mobile");
                string citizen_id = formDataCollection.Get("citizen_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("CheckRegisterCustomer", json);

                RegisterService srv = new RegisterService();
                var obj =await srv.CheckRegisterCustomer(lang, mobile, citizen_id, v, p);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("new/customer")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterCustomer(FormDataCollection formDataCollection, string v, string p, string uid)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string f_name = formDataCollection.Get("f_name");
                string l_name = formDataCollection.Get("l_name");
                string gender = formDataCollection.Get("gender");
                string birthdate = formDataCollection.Get("birthdate");
                string email = formDataCollection.Get("email");
                string address = formDataCollection.Get("address");
                string sub_distinct = formDataCollection.Get("sub_distinct");
                string distinct = formDataCollection.Get("distinct");
                string province = formDataCollection.Get("province");
                string postcode = formDataCollection.Get("postcode");
                string mobile = formDataCollection.Get("mobile");
                string citizen_id = formDataCollection.Get("citizen_id");
                string vehicle_no = formDataCollection.Get("vehicle_no");
                string register_type = formDataCollection.Get("register_type");
                string member_id = formDataCollection.Get("member_id");
                string plate_no = formDataCollection.Get("plate_no");
                string title_name = formDataCollection.Get("title_name");
                int is_app_user = int.Parse(formDataCollection.Get("is_app_user"));
                DateTime? confirm_checkbox_date = null;
                if (formDataCollection.Get("confirm_checkbox_date") != null && formDataCollection.Get("confirm_checkbox_date") != "")
                {
                    confirm_checkbox_date = Convert.ToDateTime(formDataCollection.Get("confirm_checkbox_date"));
                }

                DateTime? confirm_popup_date = null;
                if (formDataCollection.Get("confirm_popup_date") != null && formDataCollection.Get("confirm_popup_date") != "")
                {
                    confirm_popup_date = Convert.ToDateTime(formDataCollection.Get("confirm_popup_date"));
                }



                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("RegisterCustomer", json);

                RegisterService srv = new RegisterService();
                var obj =await srv.RegisterCustomer(lang, f_name, l_name, gender, birthdate, email, address, sub_distinct, distinct, province, postcode, mobile, citizen_id, vehicle_no, register_type, member_id, v, p, plate_no, title_name, is_app_user, uid, confirm_checkbox_date, confirm_popup_date);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("edit/customer")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCustomer(FormDataCollection formDataCollection, string v, string p, string uid)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string f_name = formDataCollection.Get("f_name");
                string l_name = formDataCollection.Get("l_name");
                string gender = formDataCollection.Get("gender");
                string birthdate = formDataCollection.Get("birthdate");
                string email = formDataCollection.Get("email");
                string address = formDataCollection.Get("address");
                string sub_distinct = formDataCollection.Get("sub_distinct");
                string distinct = formDataCollection.Get("distinct");
                string province = formDataCollection.Get("province");
                string postcode = formDataCollection.Get("postcode");
                string mobile = formDataCollection.Get("mobile");
                string citizen_id = formDataCollection.Get("citizen_id");
                string vehicle_no = formDataCollection.Get("vehicle_no");
                string register_type = formDataCollection.Get("register_type");
                string member_id = formDataCollection.Get("member_id");
                string plate_no = formDataCollection.Get("plate_no");
                string title_name = formDataCollection.Get("title_name");
                int is_app_user = int.Parse(formDataCollection.Get("is_app_user"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("UpdateCustomer", json);

                RegisterService srv = new RegisterService();
                var obj =await srv.UpdateCustomer(lang, f_name, l_name, gender, birthdate, email, address, sub_distinct, distinct, province, postcode, mobile, citizen_id, vehicle_no, register_type, member_id, v, p, plate_no, title_name, is_app_user, uid);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("province")]
        [HttpPost]
        public async Task<IHttpActionResult> GetProvince(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string criteria_ = formDataCollection.Get("criteria");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetProvince", json);

                ProvinceService srv = new ProvinceService();
                var obj = await srv.GetDataProvince(lang, v, p, criteria_);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("district")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDistrict(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                int province_id = Convert.ToInt32(formDataCollection.Get("province_id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetDistrict", json);

                DistrictService srv = new DistrictService();
                var obj =await srv.GetDataDistrict(lang, province_id, v, p);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sub_district")]
        [HttpPost]
        public async Task<IHttpActionResult> GetSubDistrict(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                int province_id = Convert.ToInt32(formDataCollection.Get("province_id"));
                int district_id = Convert.ToInt32(formDataCollection.Get("district_id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetSubDistrict", json);

                SubDistrictService srv = new SubDistrictService();
                var obj =await srv.GetDataSubDistrict(lang, province_id, district_id, v, p);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("preference")]
        [HttpPost]
        public async Task<IHttpActionResult> GetPreference(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                //string token = formDataCollection.Get("token");
                int customer_id = Convert.ToInt32(formDataCollection.Get("customer_id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetPreference", json);

                PreferenceService srv = new PreferenceService();
                var obj =await srv.GetDataPreference(lang, customer_id, v, p);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("new/preference")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPreference(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");
                List<Preference> data_choice = JsonConvert.DeserializeObject<List<Preference>>(formDataCollection.Get("data_choice"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("AddPreference", json);

                AddPreferenceService srv = new AddPreferenceService();
                var obj =await srv.AddDataPreference(lang, token, data_choice, v, p);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("privilege")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllPrivilege(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetAllPrivilege", json);

                PrivilegeService srv = new PrivilegeService();
                var obj =await srv.GetAllPrivilege(v, p, lang, token);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("banner/all")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllBanner(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetAllBanner", json);

                BannerService srv = new BannerService();
                var obj =await srv.GetAllBanner(v, p, lang);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("term/update")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateTerm(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("UpdateTerm", json);

                TermService srv = new TermService();
                var obj =await srv.UpdateReadTermCondition(token, v, p, lang);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("upcoming/all")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllUpComing(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetAllUpComing", json);

                UpComingService srv = new UpComingService();
                var obj =await srv.GetAllUpComing(v, p, lang);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("master/data")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMasterData(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");
                string device_id = formDataCollection.Get("device_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetMasterData", json);

                MasterDataService srv = new MasterDataService();
                var obj = await srv.GetAllMasterData(v, p, lang, token, device_id);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("master/text")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMasterText(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");
                string group_data = formDataCollection.Get("group_data");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetMasterText", json);

                MasterTextService srv = new MasterTextService();
                var obj =await srv.GetAllMasterData(v, p, lang, p, group_data);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("article/all")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllArticle(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetAllArticle", json);

                ArticleService srv = new ArticleService();
                var obj = await srv.GetAllArticleData(v, p, lang);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("article/detail")]
        [HttpPost]
        public async Task<IHttpActionResult> GetArticleDetail(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                int id = int.Parse(formDataCollection.Get("id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetArticleDetail", json);

                ArticleService srv = new ArticleService();
                var obj =await srv.GetArticleDetail(v, p, lang, id);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("initial/check")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckInitial(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string vin = formDataCollection.Get("vin");
                string citizen_id = formDataCollection.Get("citizen_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("CheckInitial", json);

                InitialService srv = new InitialService();
                var obj =await srv.CheckDataInitial(v, p, lang, vin, citizen_id);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("initial")]
        [HttpPost]
        public async Task<IHttpActionResult> GetInitial(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string vin = formDataCollection.Get("vin");
                //string citizen_id = formDataCollection.Get("citizen_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetInitial", json);

                InitialService srv = new InitialService();
                var obj =await srv.GetInitial(v, p, lang, vin);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("check/privilege/redeemcode")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckPrivilegeRedeemCode(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");
                string privilege_id = formDataCollection.Get("privilege_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("CheckPrivilegeRedeemCode", json);

                PrivilegeService srv = new PrivilegeService();
                ServiceCheckPrivilegeRedeemModel obj =await srv.CheckPrivilegeRedeem(v, p, lang, token, privilege_id);
                if (obj.msg.code == 20014002 || obj.msg.code == 20014001 || obj.msg.code == 300509)
                {
                    obj.success = true;
                    obj.msg.code = 0;
                    obj.msg.text = "";
                    obj.data.status = obj.data.status == "BLOCK" ? "REDEEM" : obj.data.status;
                }

                await _log.InsertLogReceiveData("CheckPrivilegeRedeemCode", JsonConvert.SerializeObject(obj));
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("privilege/check")]
        [HttpPost]
        public async Task<IHttpActionResult> PrivilegeCheck(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");
                string privilege_id = formDataCollection.Get("privilege_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("PrivilegeCheck", json);

                PrivilegeService srv = new PrivilegeService();
                var obj = await srv.CheckPrivilegeRedeem(v, p, lang, token, privilege_id);

                await _log.InsertLogReceiveData("PrivilegeCheck", JsonConvert.SerializeObject(obj));
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("new/tracking")]
        [HttpPost]
        public async Task<IHttpActionResult> AddTracking(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string name = formDataCollection.Get("name");
                string device_id = formDataCollection.Get("device_id");
                string session_id = formDataCollection.Get("session_id");
                string language = formDataCollection.Get("language");
                string other = formDataCollection.Get("other");
                string treasure_data = formDataCollection.Get("treasure_data");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("AddTracking", json);
                TrackingService srv = new TrackingService();
                ServiceTrackingModel ret = new ServiceTrackingModel();
                ret.success = true;
                ret.msg = new MsgModel() { code = 0, text = "Success" };
                await srv.AddTracking(v, p, token, name, device_id, session_id, language, other, treasure_data);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("privilege/reservequota")]
        [HttpPost]
        public async Task<IHttpActionResult> ReserveQuotaLuxury(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");
                string privilege_id = formDataCollection.Get("privilege_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("ReserveQuotaLuxury", json);

                PrivilegeService srv = new PrivilegeService();
                var obj = await srv.ReserveQuota(v, p, lang, token, privilege_id);

               await  _log.InsertLogReceiveData("ReserveQuotaLuxury", JsonConvert.SerializeObject(obj));
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("privilege/verify")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyCodeLuxury(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string lang = formDataCollection.Get("lang");
                string token = formDataCollection.Get("token");
                string privilege_id = formDataCollection.Get("privilege_id");
                string verify_code = formDataCollection.Get("verify_code");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("VerifyCodeLuxury", json);

                PrivilegeService srv = new PrivilegeService();
                var obj = await srv.VerifyCode(v, p, lang, token, privilege_id, verify_code);

                await _log.InsertLogReceiveData("VerifyCodeLuxury", JsonConvert.SerializeObject(obj));
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }



        //[Route("notsend/push")]
        //[HttpGet]
        //public IHttpActionResult GetNotSendPush(string v, string p)
        //{
        //    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
        //    try
        //    {
        //        LogService _log = new LogService();
        //        _log.InsertLogReceiveData("GetNotSendPush", "");

        //        NotificationService srv = new NotificationService();
        //        var obj = srv.AddTracking();
        //        return Ok(obj);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}

        [Route("ip")]
        [HttpGet]
        public async Task<string> GetIP()
        {
            //var context = System.Web.HttpContext.Current;
            string ip = String.Empty;

            //if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            //    ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
            //    ip = context.Request.UserHostAddress;

            //if (ip == "::1")
            //    ip = "127.0.0.1";

            //ip = ip.Split(':')[0];

            string json = ip;
            LogService _log = new LogService();
           await  _log.InsertLogReceiveData("ทดสอบ schedule", "ทดสอบ");

            return "ทดสอบ";
        }


        #region Phase 2.1.0 

        #region Appointment
        [Route("service_appointment/register")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterAppointment(FormDataCollection formDataCollection, string v, string p)
        {
            ValidateParameter appointmentParameter = new ValidateParameter();
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                //string token = null;

                //IEnumerable<string> keys = null;
                //if (!httpRequestMessage.Headers.TryGetValues("token", out keys))
                //    token = null;

                //token = keys.First();
                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");

                appointmentParameter.is_car_owner = Convert.ToInt16(formDataCollection.Get("is_car_owner"));
                appointmentParameter.f_name = formDataCollection.Get("f_name");
                appointmentParameter.l_name = formDataCollection.Get("l_name");
                appointmentParameter.mobile = formDataCollection.Get("mobile");
                appointmentParameter.plate_no = formDataCollection.Get("plate_no");
                appointmentParameter.vehicle_no = formDataCollection.Get("vehicle_no");
                //int type_id = Convert.ToInt16(formDataCollection.Get("type_id"));
                appointmentParameter.dealer_id = Convert.ToInt16(formDataCollection.Get("dealer_id"));
                appointmentParameter.remark = formDataCollection.Get("remark");
                appointmentParameter.remark = appointmentParameter.remark.Replace("'", "''");

                appointmentParameter.appt_date = formDataCollection.Get("appt_date");
                appointmentParameter.appt_time_id = Convert.ToInt32(formDataCollection.Get("appt_time_id"));

                appointmentParameter.type_of_service_ids = JsonConvert.DeserializeObject<List<int>>(formDataCollection.Get("type_of_service_ids"));

                appointmentParameter.is_pickup_service = Convert.ToBoolean(formDataCollection.Get("is_pickup_service"));
                if (appointmentParameter.is_pickup_service)
                {
                    appointmentParameter.latitude = formDataCollection.Get("latitude") != null && formDataCollection.Get("latitude") != "" ? Convert.ToDouble(formDataCollection.Get("latitude")) : 0;
                    appointmentParameter.longitude = formDataCollection.Get("longitude") != null && formDataCollection.Get("longitude") != "" ? Convert.ToDouble(formDataCollection.Get("longitude")) : 0;
                    appointmentParameter.location_detail = formDataCollection.Get("location_detail");
                    appointmentParameter.pickup_date = formDataCollection.Get("pickup_date");
                    appointmentParameter.pickup_time_id = formDataCollection.Get("pickup_time_id") != null && formDataCollection.Get("pickup_time_id") != "" ? Convert.ToInt32(formDataCollection.Get("pickup_time_id")) : 0;
                    appointmentParameter.pickup_address = formDataCollection.Get("pickup_address");
                }

                appointmentParameter.status_id = (int)ProcessStatus.waiting_confirm;
                //bellow is methos for get List<T> from List<T>
                //List<TypeOfServiceModel> type_of_service_ids = JsonConvert.DeserializeObject<List<TypeOfServiceModel>>(formDataCollection.Get("type_of_service_ids"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                json = json.Replace("'", "''");
                LogService _log = new LogService();
               await  _log.InsertLogReceiveData("RegisterAppointment", json);

                AppointmentService srv = new AppointmentService();

                var obj =await srv.RegisterAppointment(v, p, token, language, appointmentParameter);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "RegisterAppointment");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("service_appointment/history")]
        [HttpPost]
        public async Task<IHttpActionResult> AppointmentHistory(FormDataCollection formDataCollection, string v, string p)
        {

            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("AppointmentHistory", json);

                AppointmentService svr = new AppointmentService();
                var obj = await svr.GetAppointmentHistory(v, p, token, language);

                return Ok(obj);

            }
            catch (Exception ex)
            {
            
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("service_appointment/detail")]
        [HttpPost]
        public async Task<IHttpActionResult> AppointmentDetail(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");
                int id = Convert.ToInt16(formDataCollection.Get("id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("AppointmentDetail", json);

                AppointmentService svr = new AppointmentService();
                var obj =await svr.GetAppointmentDetail(v, p, token, language, id);

                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("service_appointment/get/type_of_services")]
        [HttpPost]
        public async Task<IHttpActionResult> AppointmentGetTypeOfService(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("AppointmentGetTypeOfService", json);

                TypeOfServiceService srv = new TypeOfServiceService();
                var obj =await srv.GetAllDataTypeOfService(lang, v, p);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }



        [Route("service_appointment/get/dealer_holiday")]
        [HttpPost]
        public async Task<IHttpActionResult> AppointmentGetDealerHoliday(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");
                int dealer_id = Convert.ToInt16(formDataCollection.Get("id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await _log.InsertLogReceiveData("AppointmentGetDealerHoliday", json);

                DealerService srv = new DealerService();
                var obj = await srv.GetAllDataDealerHoliday(lang, v, p, dealer_id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("service_appointment/get/dealer_working_time")]
        [HttpPost]
        public async Task<IHttpActionResult> AppointmentGetDealerWorkingTime(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");
                int dealer_id = Convert.ToInt16(formDataCollection.Get("dealer_id"));
                DateTime dealer_working_date = Convert.ToDateTime(formDataCollection.Get("date"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("AppointmentGetDealerWorkingTime", json);

                DealerService srv = new DealerService();
                var obj =await srv.GetAllDataDealerWorkingTimeByDate(lang, v, p, dealer_id, dealer_working_date);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("dealer/all")]
        [HttpPost]
        public async Task<IHttpActionResult> AppointmentGetAuthorizeGeoDealer(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("AppointmentGetAuthorizeGeoDealer", json);

                AuthorizeService srv = new AuthorizeService();
                var obj = await srv.GetAllDataAuthorizeService(lang, v, p);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        #endregion

        #region Test Drive

        [Route("test_drive/register")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterTestDrive(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");

                ValidateParameter parameter = new ValidateParameter();
                parameter.device_id = formDataCollection.Get("device_id");
                parameter.f_name = formDataCollection.Get("f_name");
                parameter.l_name = formDataCollection.Get("l_name");
                parameter.mobile = formDataCollection.Get("contact_mobile");
                parameter.email = formDataCollection.Get("email");
                parameter.preferred_model_id = Convert.ToInt16(formDataCollection.Get("preferred_model_id"));
                parameter.dealer_id = Convert.ToInt16(formDataCollection.Get("dealer_id"));
                parameter.purchase_plan_id = Convert.ToInt16(formDataCollection.Get("purchase_plan_id"));
                parameter.remark = formDataCollection.Get("remark");


                parameter.status_id = (int)ProcessStatus.waiting_confirm;//Waitting for confirm

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("RegisterTestDrive", json);

                TestDriveService srv = new TestDriveService();

                var obj =await srv.RegisterTestDrive(v, p, token, language, parameter);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "RegisterTestDrive");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("test_drive/history")]
        [HttpPost]
        public async Task<IHttpActionResult> TestDriveHistory(FormDataCollection formDataCollection, string v, string p)
        {

            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");
                string device_id = formDataCollection.Get("device_id");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await  _log.InsertLogReceiveData("TestDriveHistory", json);

                TestDriveService svr = new TestDriveService();
                var obj =await svr.GetTestDriveHistory(v, p, token, language, device_id);

                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("test_drive/detail")]
        [HttpPost]
        public async Task<IHttpActionResult> TestDriveDetail(FormDataCollection formDataCollection, string v, string p)
        {

            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");
                int test_drive_id = Convert.ToInt16(formDataCollection.Get("id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("TestDriveDetail", json);

                TestDriveService svr = new TestDriveService();
                var obj =await svr.GetTestDriveDetail(v, p, token, language, test_drive_id);

                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("test_drive/get/preferred_model")]
        [HttpPost]
        public async Task<IHttpActionResult> TestDriveGetPreferredModel(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("TestDriveGetPreferredModel", json);

                PreferredModelService srv = new PreferredModelService();
                var obj =await srv.GetAllDataPreferredModel(lang, v, p);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("test_drive/get/car_purchase_plan")]
        [HttpPost]
        public async Task<IHttpActionResult> TestDriveGetCarPurchasePlan(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("TestDriveGetCarPurchasePlan", json);

                CarPurchasePlanService srv = new CarPurchasePlanService();
                var obj = await srv.GetAllDataCarPurchasePlan(lang, v, p);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("year_of_purchase")]
        [HttpPost]
        public async Task<IHttpActionResult> YearOfPurchase(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("YearOfPurchase", json);

                EventService srv = new EventService();
                var obj = await srv.GetAllDataYearOfPurchase(lang, v, p);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        #endregion





        [Route("event/vin")]
        [HttpPost]

        public async Task<IHttpActionResult> RemainVinForRegisterEvent(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");
                int event_id = Convert.ToInt32(formDataCollection.Get("event_id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("RemainVinForRegisterEvent", json);

                EventService svr = new EventService();
                var obj = await svr.GetAllDataVin(token, language, v, p, event_id);

                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }




        [Route("event/check_user")]
        [HttpPost]

        public async Task<IHttpActionResult> ValidateUserForEvent(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");
                int event_id = Convert.ToInt32(formDataCollection.Get("event_id"));


                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("CheckAllCarOverTenYear", json);

                EventService svr = new EventService();
                var obj = await svr.CheckAllCarOverTenYear(token, language, v, p, event_id);

                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        #endregion


        #region Phase 2.3.0

        [Route("event/history")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEventHistory(string v, string p)
        {
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}", lang, v, p);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetEventHistory", non_json);


                EventService srv = new EventService();
                var obj = await srv.GetEventHistory(lang, v, p, token);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("get-dealers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDealersPickUp(string keyword, string lat, string lng, string radius, string v, string p)
        {
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string keyword_ = keyword;
                double lat_ = lat != null && lat != "" ? Convert.ToDouble(lat) : 0.0;
                double lon_ = lng != null && lng != "" ? Convert.ToDouble(lng) : 0.0;
                int radius_ = radius != null && radius != "" ? Convert.ToInt16(radius) : 0;

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}, radius : {3}, keyword : {4}", lang, v, p, radius, keyword);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("Get Dealer Pick up", non_json);


                DealerService srv = new DealerService();
                var obj = await srv.GetDealersPickup(v, p, lang, keyword_, lat_, lon_, radius_);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }


        [Route("dealers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDealerProvince(string v, string p, int province_id)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Please Update Application"));
            try
            {
                var headers = Request.Headers;


                string lang = headers.GetValues("lang").First().ToString();
                int province_id_ = province_id;


                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}, province_id : {3}", lang, v, p, province_id);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetDealerProvince", non_json);

                DealerService srv = new DealerService();
                var obj = await srv.GetDealersByProvince(v, p, lang, province_id_);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }


        [Route("dealer/branches")]
        public async Task<IHttpActionResult> GetBranches(string v, string p, int dealer_id)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Please Update Application"));
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                int dealer_id_ = dealer_id;

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}, token : {3}, dealer_id : {4}", lang, v, p, token, dealer_id);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetBranches", non_json);

                DealerService srv = new DealerService();
                var obj = await srv.GetBranchesByDealer(v, p, lang, dealer_id_);
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }



        #region Pick up
        [Route("pickup/get/dealer_holiday")]
        [HttpPost]
        public async Task<IHttpActionResult> PickupGetDealerHoliday(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");
                int dealer_id = Convert.ToInt16(formDataCollection.Get("id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("PickupGetDealerHoliday", json);

                DealerService srv = new DealerService();
                var obj = await srv.GetAllDataDealerHoliday(lang, v, p, dealer_id);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("pickup/get/dealer_working_time")]
        [HttpPost]
        public async Task<IHttpActionResult> PickupGetDealerWorkingTime(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string lang = formDataCollection.Get("lang");
                int dealer_id = Convert.ToInt16(formDataCollection.Get("dealer_id"));
                DateTime dealer_working_date = Convert.ToDateTime(formDataCollection.Get("date"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
               await _log.InsertLogReceiveData("PickupGetDealerWorkingTime", json);

                DealerService srv = new DealerService();
                var obj =await srv.GetAllDataDealerWorkingTimeByDate(lang, v, p, dealer_id, dealer_working_date);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }





        #endregion


        #region Booking

        [Route("online-booking")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllOnlineBookingByFilter(string v, string p, string filter)
        {
            try
            {

                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}, filter : {3}", lang, v, p, filter);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetAllOnlineBookingByFilter", non_json);

                OnlineBookingService srv = new OnlineBookingService();
                var obj = await srv.GetAllOnlineBooking(filter, v, p, lang);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("online-booking/{booking_id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOnlineBookingById(string v, string p, int booking_id)
        {
            int booking_id_ = booking_id;
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}, booking_id : {3}", lang, v, p, booking_id_);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("GetOnlineBookingById", non_json);


                OnlineBookingService srv = new OnlineBookingService();
                var obj = await srv.GeOnlineBooking(booking_id_, v, p, lang);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("service-reminder/send")]
        [HttpPost]
        public async Task<IHttpActionResult> ServiceReminder(ServiceReminderBody body)
        {
            var response = new Dictionary<string, ServiceReminderResponse>();
            try
            {
                var headers = Request.Headers;
                var headerModel = new ServiceReminderHeader(headers);

                if (!headerModel.ValidateBadRequest())
                {
                    ServiceReminderResponse value = new ServiceReminderResponse();
                    var result = value.getHeaderNotFound(body.nextservicefollowup.vin, body.nextservicefollowup.maintenanceItem);
                    response["response"] = result;
                    return Content(HttpStatusCode.BadRequest, response);
                }
                if (!headerModel.ValidateUnauthorized())
                {
                    ServiceReminderResponse value = new ServiceReminderResponse();
                    var result = value.getUnauthorized(body.nextservicefollowup.vin, body.nextservicefollowup.maintenanceItem);
                    response["response"] = result;
                    return Content(HttpStatusCode.Unauthorized, response);
                }

                if (body == null || body.nextservicefollowup == null)
                {
                    ServiceReminderResponse value = new ServiceReminderResponse();
                    var result = value.getNoBody("", "", "Body or nextservicefollowup");
                    response["response"] = result;
                    return Content(HttpStatusCode.OK, response);
                }

                var (validate, param) = body.nextservicefollowup.ValidateBody();
                if (!validate)
                {
                    ServiceReminderResponse value = new ServiceReminderResponse();
                    var result = value.getNoBody(body.nextservicefollowup.vin, body.nextservicefollowup.maintenanceItem, param);
                    response["response"] = result;
                    return Content(HttpStatusCode.OK, response);
                }

                if (!body.nextservicefollowup.ValidateDateTime())
                {
                    ServiceReminderResponse value = new ServiceReminderResponse();
                    var result = value.getValidateDate(body.nextservicefollowup.vin, body.nextservicefollowup.maintenanceItem);
                    response["response"] = result;
                    return Content(HttpStatusCode.OK, response);
                }

                var (validateLength, paramLength, length) = body.nextservicefollowup.ValidateMaxLength();
                if (!validateLength)
                {
                    ServiceReminderResponse value = new ServiceReminderResponse();
                    var result = value.getMaxLength(body.nextservicefollowup.vin, body.nextservicefollowup.maintenanceItem, paramLength, length);
                    response["response"] = result;
                    return Content(HttpStatusCode.OK, response);
                }

                var (validateDatatype, paramType, dataType) = body.nextservicefollowup.ValidateDataType();
                if (!validateDatatype)
                {
                    ServiceReminderResponse value = new ServiceReminderResponse();
                    var result = value.getMessageTypeCharacter(body.nextservicefollowup.vin, body.nextservicefollowup.maintenanceItem, paramType, dataType);
                    response["response"] = result;
                    return Content(HttpStatusCode.OK, response);
                }

                ServiceReminderService svc = new ServiceReminderService();
                var (responseData, noti) = await svc.CreateResponse(headerModel, body);
                response["response"] = responseData;

                svc.LogServiceReminder(headerModel, body, responseData, noti);

                return Ok(response);
            }
            catch (Exception ex)
            {
                ServiceReminderResponse value = new ServiceReminderResponse();
                ServiceReminderResponse result = null;
                if (body != null && body.nextservicefollowup != null)
                {
                    result = value.getException(body.nextservicefollowup.vin, body.nextservicefollowup.maintenanceItem);
                }
                else
                {
                    result = value.getException("", "");
                }
                
                string json = JsonConvert.SerializeObject(result);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("ServiceReminder", json);
                response["response"] = result;
                return Content(HttpStatusCode.OK, response);
            }
        }


        [Route("service-reminder/tricker")]
        [HttpGet]
        public async Task<IHttpActionResult> ServiceReminderTricker(ServiceReminderBody body)
        {
            try
            {
                ServiceReminderService svc = new ServiceReminderService();
                svc.ServiceReminderSchedule();
                return Ok();
            }
            catch (Exception ex)
            {
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("ServiceReminderTricker", ex.Message);
                return Content(HttpStatusCode.OK, ex.Message);
            }
        }

        [Route("notification/tricker")]
        [HttpGet]
        public async Task<IHttpActionResult> GetNotificationData(ServiceReminderBody body)
        {
            try
            {
                NotificationScheduleService srv = new NotificationScheduleService();
                await srv.GetNotificationSchedule();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("notification/count")]
        [HttpGet]
        public async Task<IHttpActionResult> GetNotificationCount(ServiceReminderBody body, string v, string p)
        {
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                TokenService.TokenServicResult tokenResult = await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1 || string.IsNullOrEmpty(token))
                {
                    GetNotificationCountService srv = new GetNotificationCountService();
                    var obj = await srv.GetNotification(token,v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("onlinebooking/register")]
        [HttpPost]
        public async Task<IHttpActionResult> OnlineBookingRegister(CampaignRegisterModel model, string v, string p)
        {
            Validate_Booking param = new Validate_Booking();
            try
            {
                if (model == null)
                {
                    throw new Exception("Not found form-data.");
                }
                var headers = Request.Headers;
     
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string json = JsonConvert.SerializeObject(model);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("OnlineBookingRegister", json);

                //DateTime? confirm_checkbox_date = null;
                //if (formDataCollection.Get("confirm_checkbox_date") != null && formDataCollection.Get("confirm_checkbox_date") != "")
                //{
                //    confirm_checkbox_date = Convert.ToDateTime(formDataCollection.Get("confirm_checkbox_date"));
                //}

                //DateTime? confirm_popup_date = null;
                //if (formDataCollection.Get("confirm_popup_date") != null && formDataCollection.Get("confirm_popup_date") != "")
                //{
                //    confirm_popup_date = Convert.ToDateTime(formDataCollection.Get("confirm_popup_date"));
                //}

                DateTime? confirm_checkbox = null;
                if (!string.IsNullOrEmpty(model.confirm_checkbox_date))
                {
                    confirm_checkbox = Convert.ToDateTime(model.confirm_checkbox_date);
                }
                //else
                //{

                //}

                DateTime? confirm_popup = null;
                if (!string.IsNullOrEmpty(model.confirm_popup_date))
                {
                    confirm_popup = Convert.ToDateTime(model.confirm_popup_date);
                }

                OnlineBookingService srv = new OnlineBookingService();
                var obj = await srv.CampaignRegister(model, v, p, lang, token, confirm_checkbox, confirm_popup);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("onlinebooking/campaignlist")]
        [HttpGet]
        public async Task<IHttpActionResult> CampaignList(string v, string p, string type)
        {
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}, type : {3}", lang, v, p, type);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("CampaignList", non_json);
                OnlineBookingService srv = new OnlineBookingService();
                var obj = await srv.GetOnlineCampaignList(type, v, p, lang);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("onlinebooking/campaigndetail/{campaign_id}")]
        [HttpGet]
        public async Task<IHttpActionResult> CampaignDetailById(string v, string p, int campaign_id)
        {
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}, campaign_id : {3}", lang, v, p, campaign_id);
                LogService _log = new LogService();
                Task.Run(async () => { await _log.InsertLogReceiveData("CampaignDetailById", non_json); });

                OnlineBookingService srv = new OnlineBookingService();
                var obj = await srv.GetCampaignDetail(campaign_id, v, p, lang, token);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("onlinebooking/campaignquestion/{campaign_id}")]
        [HttpGet]
        public async Task<IHttpActionResult> CampaignQuestionById(string v, string p, int campaign_id)
        {
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}, booking_id : {3}", lang, v, p, campaign_id);
                LogService _log = new LogService();
                Task.Run(async () => { await _log.InsertLogReceiveData("CampaignQuestionById", non_json); });

                OnlineBookingService srv = new OnlineBookingService();
                var obj = await srv.GetCampaignQuestionDetail(campaign_id, v, p, lang);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("onlinebooking/history")]
        [HttpGet]
        public async Task<IHttpActionResult> History(string v, string p, string device_id)
        {
            try
            {
                var headers = Request.Headers;
                string token = headers.GetValues("token").First().ToString();
                string lang = headers.GetValues("lang").First().ToString();

                string non_json = string.Format("lang : {0}, version : {1}, platform : {2}", lang, v, p);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("History", non_json);
                OnlineBookingService srv = new OnlineBookingService();
                if (token != "")
                {
                    var obj = await srv.GetHistory(v, p, lang, token);
                    return Ok(obj);
                }
                else
                {
                    var obj = await srv.GetHistoryGuest(v, p, lang, device_id);
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        #endregion

        #endregion





        #region P D P A
        [Route("accept/terms")]
        [HttpPost]
        public async Task<IHttpActionResult> AcceptTerms(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }


                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");
                string device_id = formDataCollection.Get("device_id");
                string read_from = formDataCollection.Get("read_from");

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("RegisterTestDrive", json);

                TermService srv = new TermService();

                var obj =await srv.UpdateReadTerms(token, v, p, language, device_id, read_from);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Event Question
        [Route("event/questions")]
        [HttpPost]
        public async Task<IHttpActionResult> EventQuestions(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");
                int event_id = Convert.ToInt16(formDataCollection.Get("event_id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("EventQuestions", json);

                EventQuestionService srv = new EventQuestionService();
                var obj =await srv.GetEventQuestions(v, p, token, language, event_id);

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        #endregion

        #region Survey

        #endregion

        #region Secret
        [Route("send_email")]
        [HttpPost]
        public async Task<IHttpActionResult> SendEmail(FormDataCollection formDataCollection)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                int refKey = Convert.ToInt16(formDataCollection.Get("refKey"));
                SendEmailService svr = new SendEmailService();
                byte[] temp = new byte[0];
                var obj = await svr.SendEmail("Me", refKey, "", "", temp, "");

                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        //[Route("send_mms")]
        //[HttpPost]
        //public string SendMultimediaMessagingService(FormDataCollection formDataCollection)
        //{
        //    try
        //    {
        //        if (formDataCollection == null)
        //        {
        //            throw new Exception("Not found form-data.");

        //        }

        //        SendMultimediaMessagingService svr = new SendMultimediaMessagingService();
        //        string obj = svr.SendMMS();
        //        return obj;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}
        #endregion

        #region event invitation (2.3.0)
        [Route("event_reg/checks")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckEventForRegister(FormDataCollection formDataCollection, string v, string p)
        {
            //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Please Update Application"));
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                int event_id = Convert.ToInt32(formDataCollection.Get("event_id"));
                string lang = formDataCollection.Get("lang");


                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("CheckEventRegister", json);

                TokenService.TokenServicResult tokenResult = await TokenService.CheckTokenResult(token, lang);
                if (tokenResult.ResultCode == 1 || string.IsNullOrEmpty(token))
                {
                    EventRegisterServie srv = new EventRegisterServie();
                    var obj = await srv.CheckEventRegister(event_id, token, v, p, lang);
                    return Ok(obj);
                }
                else
                {
                    return Ok(new ServiceHomeModel { success = false, msg = new MsgModel { code = tokenResult.ResultCode, text = tokenResult.ResultMsg } });
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("event/vins")]
        [HttpPost]

        public async Task<IHttpActionResult> RemainVinForRegisterEvents(FormDataCollection formDataCollection, string v, string p)
        {
            try
            {
                if (formDataCollection == null)
                {
                    throw new Exception("Not found form-data.");
                }

                string token = formDataCollection.Get("token");
                string language = formDataCollection.Get("lang");
                int event_id = Convert.ToInt32(formDataCollection.Get("event_id"));

                string json = JsonConvert.SerializeObject(formDataCollection);
                LogService _log = new LogService();
                await _log.InsertLogReceiveData("RemainVinForRegisterEvent", json);

                EventService svr = new EventService();
                var obj = await svr.GetAllDataVins(token, language, v, p, event_id);

                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        #endregion
    }

}
