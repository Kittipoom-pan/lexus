using AppLibrary.Database;
using com.lexus.th.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.MobileControls;
using static com.lexus.th.OnlineBookingExportExcelModel;
using System.Drawing;

namespace com.lexus.th
{
    public class OnlineBookingService
    {
        private string conn;
        public OnlineBookingService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceOnlineBookingModel> GetAllOnlineBooking(string filter, string v, string p, string lang)
        {
            ServiceOnlineBookingModel value = new ServiceOnlineBookingModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceOnlineBookingData();
                    value.data.bookings = await GetOnlineBookingData(filter, lang);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetAllOnlineBooking");
                throw ex;
            }


            return value;

        }
        public async Task<CampaignListModel> GetOnlineCampaignList(string filter, string v, string p, string lang)
        {
            CampaignListModel value = new CampaignListModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data.campaigns = await GetCampaignsData(filter, lang);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetOnlineCampaignList");
                throw ex;
            }


            return value;

        }
        public async Task<HistoryOnlineBookingModel> GetHistory(string v, string p, string lang, string token)
        {
            HistoryOnlineBookingModel value = new HistoryOnlineBookingModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = await GetHistoryData(lang,token);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetOnlineCampaignList");
                throw ex;
            }


            return value;

        }

        public async Task<HistoryOnlineBookingModel> GetHistoryGuest(string v, string p, string lang, string device_id)
        {
            HistoryOnlineBookingModel value = new HistoryOnlineBookingModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = await GetHistoryGuestData(lang, device_id);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetOnlineCampaignList");
                throw ex;
            }


            return value;

        }

        public async Task<List<HistoryOnlineModel>> GetHistoryData(string lang,string token)
        {
            var maxHistoryOnlinebooking = WebConfigurationManager.AppSettings["max_history_onlinebooking"];

            List<HistoryOnlineModel> histories = new List<HistoryOnlineModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @member_id VARCHAR(50);
                           DECLARE @lang NVARCHAR(5) = N'{0}'
                           SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{1}'
                           select top {2} a.id as booking_register_id, b.id as booking_id,booking_code,a.preferred_model_name,a.created_date,
                           CASE WHEN b.type = 1 THEN 'repurchase' WHEN b.type = 2 THEN 'referral' ELSE 'guest' end type
                           , CASE WHEN @lang = 'EN' THEN title_en ELSE title_th END AS title,
                           CASE WHEN @lang = 'EN' THEN code_message_en ELSE code_message_th END AS code_message
                           ,a.plate_number,a.referral_name + ' '+a.referral_surname as referral_fullname,a.referal_code, d.CallCenterMobile as call_center,
						   CASE  
                             WHEN @LANG = 'EN' THEN e.display_en
                             ELSE e.display_th END AS dealer_name
                           from booking_register a
                           left join booking b on a.booking_id = b.id
                           left join T_DEALER c on a.dealer_id = c.DEALER_ID
                           left join T_DEALER_WORKINGTIME d on a.dealer_id = d.Dealer_ID
                           left join T_DEALER_MASTER e on e.dealer_code = c.DEALER_CODE and e.branch_code = c.BRANCH_CODE
                           where  a.created_user = @member_id and d.Service_Type = 'OnlineBooking'
                           order by a.created_date desc";

                    DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang,token,maxHistoryOnlinebooking));

                    histories = dt.AsEnumerable().Select(row => new HistoryOnlineModel
                    {
                        booking_register_id = row.Field<int>("booking_register_id"),
                        booking_id = row.Field<int>("booking_id"),
                        title = row["title"] != DBNull.Value ? row["title"].ToString() : "",
                        booking_code = row["booking_code"] != DBNull.Value ? row["booking_code"].ToString() : "",
                        dealer_name = row["dealer_name"] != DBNull.Value ? row["dealer_name"].ToString() : "",
                        plate_number = row["plate_number"] != DBNull.Value ? row["plate_number"].ToString() : "",
                        preferred_model_name = row["preferred_model_name"] != DBNull.Value ? row["preferred_model_name"].ToString() : "",
                        referral_name= row["referral_fullname"] != DBNull.Value ? row["referral_fullname"].ToString() : "",
                        register_date= row.Field<DateTime>("created_date").ToString("dd/MM/yyyy"),
                        type = row["type"] != DBNull.Value ? row["type"].ToString() : "",
                        code_message = row["code_message"] != DBNull.Value ? row["code_message"].ToString() : "",
                        call_center = row["call_center"] != DBNull.Value ? row["call_center"].ToString() : ""
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return histories;
        }

        public async Task<List<HistoryOnlineModel>> GetHistoryGuestData(string lang, string device_id)
        {
            var maxHistoryOnlinebooking = WebConfigurationManager.AppSettings["max_history_onlinebooking"];

            List<HistoryOnlineModel> histories = new List<HistoryOnlineModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @lang NVARCHAR(5) = N'{0}'
                           DECLARE @device_id VARCHAR(50) = '{1}'
                           select top {2} a.id as booking_register_id, b.id as booking_id,booking_code,a.preferred_model_name,a.created_date, d.dealer_id,
                           CASE WHEN b.type = 1 THEN 'repurchase' WHEN b.type = 2 THEN 'referral' ELSE 'guest' end type
                           , CASE WHEN @lang = 'EN' THEN title_en ELSE title_th END AS title,
                           CASE WHEN @lang = 'EN' THEN code_message_en ELSE code_message_th END AS code_message
                           ,a.plate_number,a.referral_name + ' '+a.referral_surname as referral_fullname,a.referal_code, d.CallCenterMobile as call_center,
						   CASE  
                             WHEN @LANG = 'EN' THEN e.display_en
                             ELSE e.display_th END AS dealer_name
                           from booking_register a
                           left join booking b on a.booking_id = b.id
                           left join T_DEALER c on a.dealer_id = c.DEALER_ID
                           left join T_DEALER_WORKINGTIME d on a.dealer_id = d.Dealer_ID
						   left join T_DEALER_MASTER e on e.dealer_code = c.DEALER_CODE and e.branch_code = c.BRANCH_CODE
                           where  a.created_device_id = @device_id and d.Service_Type = 'OnlineBooking'
                           order by a.created_date desc";

                    DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, device_id, maxHistoryOnlinebooking));

                    histories = dt.AsEnumerable().Select(row => new HistoryOnlineModel
                    {
                        booking_register_id = row.Field<int>("booking_register_id"),
                        booking_id = row.Field<int>("booking_id"),
                        title = row["title"] != DBNull.Value ? row["title"].ToString() : "",
                        booking_code = row["booking_code"] != DBNull.Value ? row["booking_code"].ToString() : "",
                        dealer_name = row["dealer_name"] != DBNull.Value ? row["dealer_name"].ToString() : "",
                        plate_number = row["plate_number"] != DBNull.Value ? row["plate_number"].ToString() : "",
                        preferred_model_name = row["preferred_model_name"] != DBNull.Value ? row["preferred_model_name"].ToString() : "",
                        referral_name = row["referral_fullname"] != DBNull.Value ? row["referral_fullname"].ToString() : "",
                        register_date = row.Field<DateTime>("created_date").ToString("dd/MM/yyyy"),
                        type = row["type"] != DBNull.Value ? row["type"].ToString() : "",
                        code_message = row["code_message"] != DBNull.Value ? row["code_message"].ToString() : "",
                        call_center = row["call_center"] != DBNull.Value ? row["call_center"].ToString() : ""
                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return histories;
        }

        public async Task<List<CampaignHeader>> GetCampaignsData(string filter, string lang)
        {
            List<CampaignHeader> campaigns = new List<CampaignHeader>();
            string config_path = WebConfigurationManager.AppSettings["booking_image_url"].ToString();

            int type = 0;
            switch (filter)
            {
                case "repurchase":
                    type = 1;
                    break;
                case "referral":
                    type = 2;
                    break;
                case "guest":
                    type = 3;
                    break;
            }

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @type int = {0}
                           DECLARE @lang NVARCHAR(5) = N'{1}'
                           DECLARE @now datetime = DATEADD(HOUR, 7, GETDATE())
                           
                           SELECT 
                           id,
                           CASE WHEN @lang = 'EN' THEN title_en ELSE title_th END AS title
                           FROM [dbo].[booking]
                           WHERE is_active = 1 AND deleted_flag IS NULL
                           AND @now between display_start AND display_end
                           AND type = @type ";

                    DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, type, lang));
                    campaigns = dt.AsEnumerable().Select(row => new CampaignHeader
                    {
                        id = row.Field<int>("id"),
                        title = row["title"] != DBNull.Value ? row["title"].ToString() : ""

                    }).ToList();

                    foreach (CampaignHeader row in campaigns)
                    {
                        row.images = await GetAllBannerPicture(row.id, config_path, "BANNER");
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return campaigns;
        }
        public async Task<CampaignDetailResponseModel> GetCampaignDetail(int campaign_id, string v, string p, string lang, string token)
        {
            CampaignDetailResponseModel value = new CampaignDetailResponseModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = await GetCampaignDetail(campaign_id, lang, token);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetAllOnlineBooking");
                throw ex;
            }


            return value;

        }
        public async Task<CampaignQuestionModel> GetCampaignQuestionDetail(int campaign_id, string v, string p, string lang)
        {
            CampaignQuestionModel value = new CampaignQuestionModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.questions = await GetCampaignQuestionDetail(campaign_id, lang);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetAllOnlineBooking");
                throw ex;
            }


            return value;

        }

        public async Task<List<OnlineBookingModel>> GetOnlineBookingData(string filter, string lang)
        {
            List<OnlineBookingModel> bookings = new List<OnlineBookingModel>();
            string config_path = WebConfigurationManager.AppSettings["booking_image_url"].ToString();

            int type = 0;
            switch (filter)
            {
                case "repurchase":
                    type = 1;
                    break;
                case "referral":
                    type = 2;
                    break;
                case "car_booking":
                    type = 3;
                    break;
                case "all":
                    type = 0;
                    break;
                default:
                    goto case "all";
            }



            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @type int = {0}
                           DECLARE @lang NVARCHAR(5) = N'{1}'
                           DECLARE @now datetime = DATEADD(HOUR, 7, GETDATE())
                           
                           SELECT 
                           id,
                           CASE WHEN @lang = 'EN' THEN title_en ELSE title_th END AS title,
                           CASE WHEN @lang = 'EN' THEN desc_en ELSE desc_th END AS [desc],
                           CASE WHEN @lang = 'EN' THEN condition_en ELSE condition_th END AS condition,
                           CASE WHEN @lang = 'EN' THEN reg_period_en ELSE reg_period_th END AS reg_period,
                           reg_start,
                           reg_end
                           
                           FROM [dbo].[booking]
                           WHERE is_active = 1 AND deleted_flag IS NULL
                           AND @now between display_start AND display_end
                           AND type = @type ";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, type, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            OnlineBookingModel booking = new OnlineBookingModel();
                            booking.id = Convert.ToInt32(row["id"]);
                            booking.title = row["title"] != DBNull.Value ? row["title"].ToString() : "";
                            booking.desc = row["desc"] != DBNull.Value ? row["desc"].ToString() : "";
                            booking.condition = row["condition"] != DBNull.Value ? row["condition"].ToString() : "";
                            booking.reg_period = row["reg_period"] != DBNull.Value ? row["reg_period"].ToString() : "";
                            //booking.reg_start = row["reg_start"] != DBNull.Value ? Convert.ToDateTime(row["reg_start"]).ToString() : "";
                            //booking.reg_end = row["reg_end"] != DBNull.Value ? Convert.ToDateTime(row["reg_end"]).ToString() : "";
                            booking.images = await GetAllBannerPicture(booking.id, config_path, "BANNER");

                            bookings.Add(booking);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bookings;
        }


        public async Task<List<string>> GetAllBannerPicture(int booking_id, string config_path, string image_type)
        {
            List<string> list = new List<string>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    if (image_type == "BANNER")
                    {
                        cmd = @" 
                                    DECLARE @booking_id int = N'{0}'
                                    DECLARE @type NVARCHAR(20) = (
									select 
									CASE WHEN type = 1 THEN 'REPURCHASE'
									WHEN type = 2 THEN 'REFERRAL'
									ELSE 'BOOKING' END AS type
									from booking
									where id = @booking_id
									)
                                    SELECT 
                                    File_Path AS path
                                    FROM booking
                                    inner join upload_Image AS upload ON upload.Parent_Id = booking.id 
                                    WHERE booking.id = @booking_id 
                                    AND booking.is_active = 1 AND upload.Type = 'BANNER' AND upload.Page = @type 
                                    ORDER BY upload.Created_Date asc";
                    }
                    else
                    {
                        cmd = @" 
                                    DECLARE @booking_id int = N'{0}'
                                    DECLARE @type NVARCHAR(20) = (
									select 
									CASE WHEN type = 1 THEN 'REPURCHASE'
									WHEN type = 2 THEN 'REFERRAL'
									ELSE 'BOOKING' END AS type
									from booking
									where id = @booking_id
									)
                                    SELECT 
                                    File_Path AS path
                                    FROM booking
                                    inner join upload_Image AS upload ON upload.Parent_Id = booking.id
                                    WHERE booking.id = @booking_id
                                    AND booking.is_active = 1 AND upload.Type = 'DETAIL' AND upload.Page = @type
                                    ORDER BY upload.Created_Date asc";
                    }



                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, booking_id)))
                    {
                        string picture = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            picture = config_path + row["path"].ToString();
                            list.Add(picture);
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
        public async Task<List<Answer>> GetAnswer(int question_id, string language)
        {
            List<Answer> list = new List<Answer>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";

                    cmd = @"
                           DECLARE @id int = N'{0}'
                           DECLARE @lang NVARCHAR(5) = N'{1}'
                           SELECT id as answer_id,
                           CASE WHEN @lang = 'EN' THEN answer_en ELSE answer_th END AS answer
                           ,[order] as seq
                           FROM [dbo].[booking_answer]
                           WHERE question_id = @id
                           AND is_active = 1 AND deleted_flag IS NULL ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, question_id, language)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Answer answer = new Answer()
                            {
                                answer_id = Convert.ToInt32(row["answer_id"]),
                                answer = row["answer"] != DBNull.Value ? row["answer"].ToString() : "",
                                seq = Convert.ToInt32(row["seq"])
                            };
                            list.Add(answer);
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


        public async Task<ServiceOnlineBookingModel> GeOnlineBooking(int booking_id_, string v, string p, string lang)
        {
            ServiceOnlineBookingModel value = new ServiceOnlineBookingModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceOnlineBookingData();
                    value.data.bookings = await GetOnlineBookingDetail(booking_id_, lang);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetAllOnlineBooking");
                throw ex;
            }


            return value;

        }

        public async Task<List<OnlineBookingModel>> GetOnlineBookingDetail(int booking_id_, string lang)
        {
            List<OnlineBookingModel> bookings = new List<OnlineBookingModel>();

            string config_path = WebConfigurationManager.AppSettings["booking_image_url"].ToString();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @id int = N'{0}'
                           DECLARE @lang NVARCHAR(5) = N'{1}'
                           
                           SELECT 
                           id,
                           CASE WHEN @lang = 'EN' THEN title_en ELSE title_th END AS title,
                           CASE WHEN @lang = 'EN' THEN desc_en ELSE desc_th END AS [desc],
                           CASE WHEN @lang = 'EN' THEN condition_en ELSE condition_th END AS condition,
                           CASE WHEN @lang = 'EN' THEN reg_period_en ELSE reg_period_th END AS reg_period,
                           reg_start,
                           reg_end
                           
                           FROM [dbo].[booking]
                           WHERE id = @id
                           AND is_active = 1 AND deleted_flag IS NULL ";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, booking_id_, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            OnlineBookingModel booking = new OnlineBookingModel();
                            booking.id = Convert.ToInt32(row["id"]);
                            booking.title = row["title"] != DBNull.Value ? row["title"].ToString() : "";
                            booking.desc = row["desc"] != DBNull.Value ? row["desc"].ToString() : "";
                            booking.condition = row["condition"] != DBNull.Value ? row["condition"].ToString() : "";
                            booking.reg_period = row["reg_period"] != DBNull.Value ? row["reg_period"].ToString() : "";
                            if (row["reg_start"] != DBNull.Value && row["reg_end"] != DBNull.Value)
                            {
                                DateTime regStart = Convert.ToDateTime(row["reg_start"]);
                                DateTime regEnd = Convert.ToDateTime(row["reg_end"]);
                                booking.allow_reg = (regStart <= DateTime.Now && regEnd >= DateTime.Now);
                            }
                            //booking.reg_start = row["reg_start"] != DBNull.Value ? Convert.ToDateTime(row["reg_start"]).ToString() : "";
                            //booking.reg_end = row["reg_end"] != DBNull.Value ? Convert.ToDateTime(row["reg_end"]).ToString() : "";
                            booking.images = await GetAllBannerPicture(booking.id, config_path, "DETAIL");

                            bookings.Add(booking);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bookings;
        }

        public async Task<CampaignDetail> GetCampaignDetail(int booking_id_, string lang, string token)
        {
            CampaignDetail campaignDetail = new CampaignDetail();

            string config_path = WebConfigurationManager.AppSettings["booking_image_url"].ToString();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @id int = N'{0}'
                           DECLARE @lang NVARCHAR(5) = N'{1}'
                           
                           SELECT 
                           id,
                           CASE WHEN @lang = 'EN' THEN title_en ELSE title_th END AS title,
                           CASE WHEN @lang = 'EN' THEN desc_en ELSE desc_th END AS [desc],
                           CASE WHEN @lang = 'EN' THEN condition_en ELSE condition_th END AS condition,
                           CASE WHEN @lang = 'EN' THEN reg_period_en ELSE reg_period_th END AS reg_period,
                           reg_start,
                           reg_end,
                           is_required_plate_no,
                           max_used,
                           referral_condition,
                           type,
                           preferred_model_ids
                           FROM [dbo].[booking]
                           WHERE id = @id
                           AND is_active = 1 AND deleted_flag IS NULL ";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, booking_id_, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {

                            campaignDetail.id = Convert.ToInt32(row["id"]);
                            campaignDetail.title = row["title"] != DBNull.Value ? row["title"].ToString() : "";
                            campaignDetail.desc = row["desc"] != DBNull.Value ? row["desc"].ToString() : "";
                            campaignDetail.condition = row["condition"] != DBNull.Value ? row["condition"].ToString() : "";
                            campaignDetail.reg_period = row["reg_period"] != DBNull.Value ? row["reg_period"].ToString() : "";
                            campaignDetail.is_required_plate_no = Convert.ToBoolean(row["is_required_plate_no"] != DBNull.Value ? row["is_required_plate_no"].ToString() : "false");
                            campaignDetail.is_expired = false;
                            if (row["reg_start"] != DBNull.Value && row["reg_end"] != DBNull.Value)
                            {
                                DateTime regStart = Convert.ToDateTime(row["reg_start"]);
                                DateTime regEnd = Convert.ToDateTime(row["reg_end"]);
                                campaignDetail.allow_reg = (regStart <= DateTime.Now && regEnd >= DateTime.Now);
                                campaignDetail.is_expired = DateTime.Now > regEnd;
                            }
                            campaignDetail.is_full = await CheckHaveActiveCampaignCode(booking_id_);
                            var max_used = Convert.ToInt32(row["max_used"] != DBNull.Value ? row["max_used"].ToString() : "0");
                            var referral_condition = row["referral_condition"] != DBNull.Value ? row["referral_condition"].ToString() : "";
                            campaignDetail.is_registered = await CheckUserCanRegister(booking_id_, referral_condition, max_used, token);
                            campaignDetail.question_answer_more = await CheckQuestionMore(booking_id_);
                            campaignDetail.preferred_model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PreferredModel>>(row["preferred_model_ids"] != DBNull.Value ? row["preferred_model_ids"].ToString() : "");
                            campaignDetail.images = await GetAllBannerPicture(campaignDetail.id, config_path, "DETAIL");

                            var type = Convert.ToInt32(row["type"] != DBNull.Value ? row["type"].ToString() : "0");
                            if (type == (int)BookingType.referral)
                            {
                                campaignDetail.plate_numbers = await GetAllPlateNo(campaignDetail.id, token);
                            }
                            else
                            {
                                campaignDetail.plate_numbers = new List<ReferralPlateModel>();
                            }
                            

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return campaignDetail;
        }

        public async Task<List<QuestionAnswer>> GetCampaignQuestionDetail(int booking_id_, string lang)
        {
            List<QuestionAnswer> questionAnswers = new List<QuestionAnswer>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @id int = N'{0}'
                           DECLARE @lang NVARCHAR(5) = N'{1}'
                           SELECT id as question_id,
                           CASE WHEN @lang = 'EN' THEN question_en ELSE question_th END AS question
                           ,question_key
                           ,answer_type
                           ,[order] as seq
                           ,is_optional
                           FROM [dbo].[booking_question]
                           WHERE booking_id = @id
                           AND is_active = 1 AND deleted_flag IS NULL ";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, booking_id_, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            QuestionAnswer questionAnswer = new QuestionAnswer();
                            questionAnswer.question_id = Convert.ToInt32(row["question_id"]);
                            questionAnswer.question = row["question"] != DBNull.Value ? row["question"].ToString() : "";
                            questionAnswer.question_key = row["question_key"] != DBNull.Value ? row["question_key"].ToString() : "";
                            questionAnswer.seq = Convert.ToInt32(row["seq"]);
                            questionAnswer.is_optional = Convert.ToBoolean(row["is_optional"] != DBNull.Value ? row["is_optional"].ToString() : "false");
                            questionAnswer.answers = await GetAnswer(questionAnswer.question_id, lang);
                            questionAnswer.answer_type = Convert.ToInt32(row["answer_type"]);
                            questionAnswers.Add(questionAnswer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return questionAnswers;
        }
        public async Task<bool> CheckHaveActiveCampaignCode(int booking_id_)
        {
            QuestionAnswer questionAnswer = new QuestionAnswer();


            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           SELECT count(*) as count
                           FROM [dbo].[booking_code]
                           WHERE booking_id = {0}
                           AND status_id = 1 AND deleted_flag IS NULL ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, booking_id_)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var count = Convert.ToInt32(row["count"]);
                            return count == 0;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CheckUserCanRegister(int booking_id_, string referral_condition, int maxUsed, string token)
        {
            QuestionAnswer questionAnswer = new QuestionAnswer();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    if(!string.IsNullOrEmpty(referral_condition) && referral_condition == "ByCar")
                    {
                        string cmd = @"
                            DECLARE @token VARCHAR(50) = N'{0}';
	                        DECLARE @member_id VARCHAR(50);

                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @token

                            select PLATE_NO from T_CUSTOMER_CAR where MEMBERID = @member_id and DEL_FLAG is null
                           ";

                        var count_car = 0;
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                        {
                            count_car = dt.Rows.Count;
                        }

                        if (count_car == 0)
                        {
                            return false;
                        }

                        cmd = @"
                            DECLARE @token VARCHAR(50) = N'{0}';
	                        DECLARE @member_id VARCHAR(50);

                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @token

                            SELECT count(*) FROM booking_register where booking_id = {1} and created_user = @member_id and plate_number is not null
                           ";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, booking_id_)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                var count = Convert.ToInt32(dt.Rows[0][0]);
                                if (count >= count_car)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                                return false;
                        }

                    }
                    else
                    {
                        string cmd = @"
                            DECLARE @token VARCHAR(50) = N'{0}';
	                        DECLARE @member_id VARCHAR(50);

                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @token

                            SELECT count(*) FROM booking_register where booking_id = {1} and created_user = @member_id
                           ";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, booking_id_)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                var count = Convert.ToInt32(dt.Rows[0][0]);
                                if (count == maxUsed && maxUsed != 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                                return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CheckQuestionMore(int booking_id_)
        {
            QuestionAnswer questionAnswer = new QuestionAnswer();


            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                                 SELECT count(id)
                                 FROM [dbo].[booking_question]
                                 WHERE booking_id = {0}
                                    AND is_active = 1 AND deleted_flag IS NULL ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, booking_id_)))
                    {
                        if (dt.Rows.Count != 0)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ReferralPlateModel>> GetAllPlateNo(int booking_id_, string token)
        {
            List<ReferralPlateModel> plate_no_list = new List<ReferralPlateModel>();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                                 DECLARE @token VARCHAR ( 50 ) =  N'{0}';
                                 DECLARE @member_id VARCHAR ( 50 ) = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN  WHERE TOKEN_NO = @token);

                                 select C.PLATE_NO,
                                 M.MODEL
                                 from T_CUSTOMER_CAR C 
                                 inner join T_CAR_MODEL M on C.MODEL_ID = M.MODEL_ID
                                 where C.MEMBERID = @member_id AND C.DEL_FLAG IS NULL
                                 and C.PLATE_NO not in (SELECT plate_number COLLATE SQL_Latin1_General_CP1_CI_AS FROM booking_register WHERE booking_id = N'{1}' AND created_user = @member_id)  ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, booking_id_)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            plate_no_list.Add(new ReferralPlateModel()
                            {
                                plate_number = row["plate_no"] != DBNull.Value ? row["plate_no"].ToString() : "",
                                model = row["MODEL"] != DBNull.Value ? row["MODEL"].ToString() : ""
                            });
                            
                        }
                        return plate_no_list;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<ServiceOnlineBookingRegister> RegisterOnlineBooking(Validate_Booking param, string v, string p, string lang, string token)
        //{
        //    ServiceOnlineBookingRegister value = new ServiceOnlineBookingRegister();
        //    try
        //    {
        //        value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

        //        SystemController syc = new SystemController();
        //        ValidationModel validation = await syc.CheckSystem(p, v);
        //        if (!validation.Success)
        //        {
        //            value.success = validation.Success;
        //            value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
        //            return value;
        //        }
        //        else
        //        {
        //            value.data = new _ServiceOnlineBookingRegisterData();
        //            switch (param.type)
        //            {
        //                case 1: //repurchase
        //                    value.data.id = await RegisterOnlineBooking_Repurchase(param, lang, token);
        //                    break;
        //                case 2: //referral
        //                    value.data.id = await RegisterOnlineBooking_Repurchase(param, lang, token);
        //                    break;
        //                case 3: //car booking
        //                    value.data.id = await RegisterOnlineBooking_Repurchase(param, lang, token);
        //                    break;
        //            }


        //            value.success = true;
        //            value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

        //            //todo after register
        //            //1. send email 
        //            this.SendMail(value.data.id, token, lang);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ServiceLog.WriteExceptionLog(ex, "GetAllOnlineBooking");
        //        throw ex;
        //    }

        //    return value;
        //}
        public async Task<_ServiceOnlineBookingRegisterData> Register_Campaign(CampaignRegisterModel model, string lang, string token, int type, DateTime? confirm_checkbox_date, DateTime? confirm_popup_date)
        {
            _ServiceOnlineBookingRegisterData value = new _ServiceOnlineBookingRegisterData();
            string sys_version = WebConfigurationManager.AppSettings["version"];

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Empty;
                    if (type == 1)
                    {
                        cmd = @"
                            DECLARE @member_id VARCHAR(50);
                            DECLARE @lang NVARCHAR(5) = N'{0}'
                            DECLARE @booking_code VARCHAR(20);
                            DECLARE @IdentityOutput table ( ID int )
                            SELECT top 1 @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date 
                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{1}'
                            SELECT @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date
                            IF @booking_code is not null
							BEGIN  
                            INSERT INTO booking_register (booking_id, [name], surname, contact_number, email, preferred_model_id, dealer_id, need_to_test_drive, remark, [type], booking_code, created_date, created_user,preferred_model_name)
                             OUTPUT inserted.id into @IdentityOutput
                            VALUES ({2}, N'{3}', N'{4}', N'{5}', N'{6}', {7}, {8}, '{9}', N'{10}',{11},@booking_code, DATEADD(HOUR, 7, GETDATE()), @member_id,N'{12}')

                            UPDATE booking_code set status_id =2,updated_date = DATEADD(HOUR, 7, GETDATE()), updated_user=@member_id where code= @booking_code and booking_id = '{2}'
                            SELECT @booking_code as code, CASE WHEN @lang = 'EN' THEN thankyou_message_en ELSE thankyou_message_th END AS thankyou_message, (select ID from @IdentityOutput)  as booking_register_id  from booking where id='{2}'
                            END
                            ELSE
							BEGIN
							select ''as code , '' as thankyou_message , '' as booking_register_id 
							end";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, token, model.booking_id, model.name, model.surname, model.contact_number, model.email, model.preferred_model_id, model.dealer_id, model.need_to_test_drive, model.remark, type,model.preferred_model_name)))

                        {
                            if (dt.Rows.Count > 0)
                            {
                                value.code = dt.Rows[0]["code"] != DBNull.Value ? dt.Rows[0]["code"].ToString() : "";
                                value.booking_register_id = dt.Rows[0]["booking_register_id"] != DBNull.Value ? dt.Rows[0]["booking_register_id"].ToString() : "";
                                if (!string.IsNullOrWhiteSpace(value.code))
                                    value.thankyou_message = dt.Rows[0]["thankyou_message"] != DBNull.Value ? dt.Rows[0]["thankyou_message"].ToString() : "";
                            }
                        }
                    }
                    else if (type == 2)
                    {
                        if (!string.IsNullOrWhiteSpace(model.car_model) && !string.IsNullOrWhiteSpace(model.plate_number))
                        {
                            cmd = @"
                            DECLARE @member_id VARCHAR(50);
                            DECLARE @lang NVARCHAR(5) = N'{0}'
                            DECLARE @booking_code VARCHAR(20);
                            DECLARE @IdentityOutput table ( ID int )
                            SELECT top 1 @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date 
                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{1}'
                            SELECT @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date
                            IF @booking_code is not null
							BEGIN  
                            INSERT INTO booking_register (booking_id, [name], surname, contact_number, email, preferred_model_id, dealer_id, need_to_test_drive, remark, [type], booking_code, created_date, created_user,plate_number,car_model,referral_name,referral_surname,referral_contact_number,referral_email,preferred_model_name )
                             OUTPUT inserted.id into @IdentityOutput
                            VALUES ({2}, N'{3}', N'{4}', N'{5}', N'{6}', {7}, {8}, '{9}', N'{10}',{11},@booking_code, DATEADD(HOUR, 7, GETDATE()), @member_id,N'{12}',N'{13}',N'{14}',N'{15}',N'{16}',N'{17}',N'{18}')

                            UPDATE booking_code set status_id =2,updated_date = DATEADD(HOUR, 7, GETDATE()), updated_user=@member_id where code= @booking_code and booking_id = '{2}'
                            SELECT @booking_code as code, CASE WHEN @lang = 'EN' THEN thankyou_message_en ELSE thankyou_message_th END AS thankyou_message , (select ID from @IdentityOutput)  as booking_register_id  from booking where id='{2}'
                            END
                            ELSE
							BEGIN
								select ''as code , '' as thankyou_message , '' as booking_register_id 
							end";
                        }
                        else if (string.IsNullOrWhiteSpace(model.car_model) && string.IsNullOrWhiteSpace(model.plate_number))
                        {
                            cmd = @"
                            DECLARE @member_id VARCHAR(50);
                            DECLARE @model VARCHAR(50);
                            DECLARE @plate_number NVARCHAR(50);
                            DECLARE @lang NVARCHAR(5) = N'{0}'
                            DECLARE @booking_code VARCHAR(20);
                            DECLARE @IdentityOutput table ( ID int )
                            SELECT top 1 @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date 
                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{1}'
                            SELECT @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date
                            SELECT
			                            @model = MD.MODEL
					                    ,@plate_number = CAR.PLATE_NO
                            FROM		[T_CUSTOMER_CAR] CAR
                            LEFT JOIN	[T_CAR_MODEL] MD ON CAR.MODEL_ID = MD.MODEL_ID
                            WHERE		[MEMBERID] IN (@member_id)

                            IF @booking_code is not null
							BEGIN  
                            INSERT INTO booking_register (booking_id, [name], surname, contact_number, email, preferred_model_id, dealer_id, need_to_test_drive, remark, [type], booking_code, created_date, created_user,plate_number,car_model,referral_name,referral_surname,referral_contact_number,referral_email,preferred_model_name )
                             OUTPUT inserted.id into @IdentityOutput
                            VALUES ({2}, N'{3}', N'{4}', N'{5}', N'{6}', {7}, {8}, '{9}', N'{10}',{11},@booking_code, DATEADD(HOUR, 7, GETDATE()), @member_id,@plate_number ,@model,N'{14}',N'{15}',N'{16}',N'{17}',N'{18}')

                            UPDATE booking_code set status_id =2,updated_date = DATEADD(HOUR, 7, GETDATE()), updated_user=@member_id where code= @booking_code and booking_id = '{2}'
                            SELECT @model as car_model,@plate_number as plate_number, @booking_code as code, CASE WHEN @lang = 'EN' THEN thankyou_message_en ELSE thankyou_message_th END AS thankyou_message , (select ID from @IdentityOutput)  as booking_register_id  from booking where id='{2}';
                            END
                            ELSE
							BEGIN
								select ''as code,''as plate_number ,''as car_model , '' as thankyou_message , '' as booking_register_id; 
							end
                            ";
                        }
                        else if (!string.IsNullOrWhiteSpace(model.car_model) && string.IsNullOrWhiteSpace(model.plate_number))
                        {
                            cmd = @"
                            DECLARE @member_id VARCHAR(50);
                            DECLARE @model VARCHAR(50);
                            DECLARE @plate_number NVARCHAR(50);
                            DECLARE @lang NVARCHAR(5) = N'{0}'
                            DECLARE @booking_code VARCHAR(20);
                            DECLARE @IdentityOutput table ( ID int )
                            SELECT top 1 @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date 
                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{1}'
                            SELECT @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date
                            SELECT
			                            @model = MD.MODEL
					                    ,@plate_number = CAR.PLATE_NO
                            FROM		[T_CUSTOMER_CAR] CAR
                            LEFT JOIN	[T_CAR_MODEL] MD ON CAR.MODEL_ID = MD.MODEL_ID
                            WHERE		[MEMBERID] IN (@member_id) AND MD.MODEL = '{13}' 

                            IF @booking_code is not null
							BEGIN  
                            INSERT INTO booking_register (booking_id, [name], surname, contact_number, email, preferred_model_id, dealer_id, need_to_test_drive, remark, [type], booking_code, created_date, created_user,plate_number,car_model,referral_name,referral_surname,referral_contact_number,referral_email,preferred_model_name )
                             OUTPUT inserted.id into @IdentityOutput
                            VALUES ({2}, N'{3}', N'{4}', N'{5}', N'{6}', {7}, {8}, '{9}', N'{10}',{11},@booking_code, DATEADD(HOUR, 7, GETDATE()), @member_id,@plate_number ,@model,N'{14}',N'{15}',N'{16}',N'{17}',N'{18}')

                            UPDATE booking_code set status_id =2,updated_date = DATEADD(HOUR, 7, GETDATE()), updated_user=@member_id where code= @booking_code and booking_id = '{2}'
                            SELECT @model as car_model,@plate_number as plate_number, @booking_code as code, CASE WHEN @lang = 'EN' THEN thankyou_message_en ELSE thankyou_message_th END AS thankyou_message , (select ID from @IdentityOutput)  as booking_register_id  from booking where id='{2}';
                            END
                            ELSE
							BEGIN
								select ''as code,''as plate_number ,''as car_model , '' as thankyou_message , '' as booking_register_id; 
							end
                            ";
                        }

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, token, model.booking_id, model.name, model.surname, model.contact_number, model.email, model.preferred_model_id, model.dealer_id, model.need_to_test_drive, model.remark, type, model.plate_number, model.car_model, model.referral_name, model.referral_surname, model.referral_contact_number, model.referral_email,model.preferred_model_name)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if((string.IsNullOrEmpty(model.plate_number) && string.IsNullOrEmpty(model.car_model)) || (string.IsNullOrEmpty(model.plate_number) && !string.IsNullOrEmpty(model.car_model)))
                                {
                                    model.plate_number = dt.Rows[0]["plate_number"] != DBNull.Value ? dt.Rows[0]["plate_number"].ToString() : "";
                                    model.car_model = dt.Rows[0]["car_model"] != DBNull.Value ? dt.Rows[0]["car_model"].ToString() : "";
                                }

                                value.code = dt.Rows[0]["code"] != DBNull.Value ? dt.Rows[0]["code"].ToString() : "";
                                value.booking_register_id = dt.Rows[0]["booking_register_id"] != DBNull.Value ? dt.Rows[0]["booking_register_id"].ToString() : "";
                                if (!string.IsNullOrWhiteSpace(value.code))
                                    value.thankyou_message = dt.Rows[0]["thankyou_message"] != DBNull.Value ? dt.Rows[0]["thankyou_message"].ToString() : "";
                            }
                        }
                    }
                    else if (type == 3)
                    {
                        cmd = @"
                            DECLARE @member_id VARCHAR(50);
                            DECLARE @lang NVARCHAR(5) = N'{0}'
                            DECLARE @IdentityOutput table ( ID int )
                            DECLARE @booking_code VARCHAR(20);

                            DECLARE @confirm_checkbox_date datetime = N'{16}';
                            DECLARE @confirm_popup_date datetime = N'{17}';  
                            DECLARE @confirm_checkbox bit;      
                            DECLARE @confirm_popup bit; 

                            SELECT top 1 @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date 
                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{1}'
                            SELECT @booking_code = code FROM booking_code WHERE booking_id = '{2}' and status_id = 1 order by created_date
                            IF @booking_code is not null
							BEGIN  
                            INSERT INTO booking_register (booking_id, [name], surname, contact_number, email, preferred_model_id, dealer_id, remark, [type], booking_code, created_date, created_user, current_car_brand, current_car_model, referal_code, preferred_model_name, created_device_id)
                            OUTPUT inserted.id into @IdentityOutput
                            VALUES ({2}, N'{3}', N'{4}', N'{5}', N'{6}', {7}, {8}, N'{9}', N'{10}',@booking_code, DATEADD(HOUR, 7, GETDATE()), @member_id,N'{11}',N'{12}',N'{13}',N'{14}',N'{15}')

                            UPDATE booking_code set status_id =2,updated_date = DATEADD(HOUR, 7, GETDATE()), updated_user=@member_id where code= @booking_code and booking_id = '{2}'

                            

                            SELECT @booking_code as code, CASE WHEN @lang = 'EN' THEN thankyou_message_en ELSE thankyou_message_th END AS thankyou_message ,(select ID from @IdentityOutput)  as booking_register_id  from booking where id='{2}'
                            END
                            ELSE
							BEGIN
								select ''as code , '' as thankyou_message , '' as booking_register_id 
							end
IF @confirm_checkbox_date IS NULL OR @confirm_checkbox_date = ''
                                            BEGIN
                                                SET @confirm_checkbox = 0;
                                                SET @confirm_checkbox_date = null;
                                            END
                                        ELSE
                                            BEGIN
                                                SET @confirm_checkbox = 1;
                                            END
        
            
                                        IF @confirm_popup_date IS NULL OR @confirm_popup_date = ''
                                            BEGIN
                                                SET @confirm_popup = 0;
                                                SET @confirm_popup_date = null;
                                            END
                                        ELSE
                                            BEGIN
                                                SET @confirm_popup = 1;
                                            END

                
                                        BEGIN
            	                            INSERT INTO [dbo].[terms_and_condition_stamp_read] 
                                                   (device_id, confirm_checkbox, confirm_checkbox_date, [version], confirm_popup, confirm_popup_date, read_from) 
                                               OUTPUT Inserted.id
            	                            VALUES
                                                   (N'{15}', @confirm_checkbox, @confirm_checkbox_date, N'{18}', @confirm_popup, @confirm_popup_date, 'guest_online_booking')
                                        END
                        ";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, token, model.booking_id, model.name, model.surname, model.contact_number, model.email, model.preferred_model_id, model.dealer_id, 
                            model.remark, type, model.current_car_brand, model.current_car_model, model.referral_code,model.preferred_model_name, model.device_id, confirm_checkbox_date, confirm_popup_date, sys_version)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                value.code = dt.Rows[0]["code"] != DBNull.Value ? dt.Rows[0]["code"].ToString() : "";
                                value.booking_register_id = dt.Rows[0]["booking_register_id"] != DBNull.Value ? dt.Rows[0]["booking_register_id"].ToString() : "";
                                if (!string.IsNullOrWhiteSpace(value.code))
                                    value.thankyou_message = dt.Rows[0]["thankyou_message"] != DBNull.Value ? dt.Rows[0]["thankyou_message"].ToString() : "";
                            }
                        }
                        if (model.question_answers.Count > 0 && value.booking_register_id != "")
                        {
                            foreach (var item in model.question_answers)
                            {
                                string sql = @"
                                        DECLARE @member_id VARCHAR(50);
                                        SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{0}'
                                        INSERT INTO booking_answer_user (booking_register_id,question_id, answer_id, answer_text, created_date,created_user, created_device_id)
                                        VALUES({1}, {2}, {3}, N'{4}',  DATEADD(HOUR, 7, GETDATE()), @member_id, N'{5}')";
                                db.ExecuteNonQueryFromCommandText(string.Format(sql, token, value.booking_register_id, item.question_id, item.answer_id, item.answer_text, model.device_id));
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        
        public async Task<ServiceOnlineBookingRegister> CampaignRegister(CampaignRegisterModel model, string v, string p, string lang, string token, DateTime? confirm_checkbox_date, DateTime? confirm_popup_date)
        {
            ServiceOnlineBookingRegister value = new ServiceOnlineBookingRegister();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceOnlineBookingRegisterData();
                    switch (model.type)
                    {
                        case "repurchase":
                            value.data = await Register_Campaign(model, lang, token, 1, confirm_checkbox_date, confirm_popup_date);
                            break;
                        case "referral":
                            var validate = await RegisterOnlineBooking_Referral_Validate(model, token);
                            if (!validate)
                            {
                                value.success = false;
                                ValidationModel validationRegis = new ValidationModel();
                                validationRegis.InvalidCode = ValidationModel.GetInvalidCode(ValidationModel.InvalidState.E820);
                                validationRegis.InvalidMessage = await ValidationModel.GetInvalidMessageNew(ValidationModel.InvalidState.E820, lang);
                                value.msg = new MsgModel() { code = validationRegis.InvalidCode, text = validationRegis.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                                return value;
                            }
                            value.data = await Register_Campaign(model, lang, token, 2, confirm_checkbox_date, confirm_popup_date);
                            break;
                        case "guest": //car booking
                            if (!string.IsNullOrWhiteSpace(model.device_id))
                            { 
                                value.data = await Register_Campaign(model, lang, token, 3, confirm_checkbox_date, confirm_popup_date);
                            }
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(value.data.code))
                    {
                        value.success = false;
                        ValidationModel validationRegis = new ValidationModel();
                        validationRegis.InvalidCode = ValidationModel.GetInvalidCode(ValidationModel.InvalidState.E906);
                        validationRegis.InvalidMessage = await ValidationModel.GetInvalidMessageNew(ValidationModel.InvalidState.E906, lang);
                        value.msg = new MsgModel() { code = validationRegis.InvalidCode, text = validationRegis.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                        this.SendMail(model, token, lang, value.data.booking_register_id);
                        var onlineBooking = this.DataForSms(value.data.booking_register_id, lang, model);
                        SMSService sms = new SMSService();
                        if (!string.IsNullOrEmpty(model.referral_contact_number))
                        {
                            foreach (var data in onlineBooking.message)
                            {
                                await sms.SendSMS(data.contact_number, data.msg);
                                //await sms.SendSMS(data.contact_number_referral, data.msg_referral);
                            }
                        }
                        else
                        {
                            foreach (var data in onlineBooking.message)
                            {
                                await sms.SendSMS(model.contact_number, data.msg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetAllOnlineBooking");
                throw ex;
            }
            return value;
        }

        public async Task<bool> RegisterOnlineBooking_Referral_Validate(CampaignRegisterModel model, string token)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            select b.id,b.max_used, b.referral_condition from booking b where b.id = N'{0}' 
                           ";

                    int max_use = 0;
                    string referral_condition_string = "";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, model.booking_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            max_use = dt.Rows[0]["max_used"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["max_used"]);
                            referral_condition_string = dt.Rows[0]["referral_condition"] == DBNull.Value ? "" :  dt.Rows[0]["referral_condition"].ToString();
                        }
                    }

                    if (referral_condition_string == "ByMember")
                    {
                        cmd = @"DECLARE @member_id VARCHAR(50);

                                SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{0}'

                                SELECT COUNT(*) AS register_count FROM booking_register WHERE booking_id = N'{1}' AND created_user = @member_id";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token,model.booking_id)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                var register_count = Convert.ToInt32(dt.Rows[0]["register_count"]);
                                if (register_count >= max_use)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public async Task<int> RegisterOnlineBooking_Repurchase(Validate_Booking param, string lang, string token)
        {
            int value = 0;

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @token VARCHAR(50) = N'{0}';
                            DECLARE @lang varchar(5) = N'{1}';
	                        DECLARE @member_id VARCHAR(50);

                            SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @token
							

                            INSERT INTO booking_register
                            (booking_id, [name], surname, contact_number, email, 
                             preferred_model_id, dealer_id, need_to_test_drive, remark, [type], 
                             repurchase_code, created_date, created_user)
                            OUTPUT Inserted.id
                            VALUES
                            ({2}, N'{3}', N'{4}', N'{5}', N'{6}', 
                             {7}, {8}, {9}, N'{10}', {11},         
                             N'{12}' , DATEADD(HOUR, 7, GETDATE()), @member_id)
                           ";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, lang, param.booking_id, param.name, param.surname, param.contact_number, param.email
                                                                                                       , param.preferred_model_id, param.dealer_id, Convert.ToInt16(param.need_to_test_drive), param.remark, param.type
                                                                                                       , "Repurchase001")))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            value = Convert.ToInt32(dt.Rows[0]["id"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public void SendMail(CampaignRegisterModel model, string token, string language, string register_id)
        {
            var dataForExcel = GetDataForExportExcel(model, token, language, register_id);
            byte[] attachFile = ExportExcelAppointment(model, dataForExcel, register_id);
            string attachFileName = "";
            if (attachFile.Length > 0)
            {
                if (model.type == "repurchase")
                {
                    attachFileName = "Repurchase_" + "RE" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}",Convert.ToInt32(register_id)) + ".xlsx";
                }
                else if(model.type == "referral")
                {
                    attachFileName = "Referral_" + "REF" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", Convert.ToInt32(register_id)) + ".xlsx";
                }
                else if (model.type == "guest")
                {
                    attachFileName = "Guest_" + "LX" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", Convert.ToInt32(register_id)) + ".xlsx";
                }
            } 
            //Send email to dealer call center.
            SendEmailService sendEmailService = new SendEmailService();
            sendEmailService.SendEmail("OnlinebookingService", Convert.ToInt32(register_id), token, language, attachFile, attachFileName);
        }

        public OnlineBookingExportExcelModel DataForSms(string booking_id, string lang, CampaignRegisterModel model)
        {
            var data = GetDataForSms(booking_id, lang);

            var onlineBooking = new OnlineBookingExportExcelModel();
            List<MeassgeSms> list = new List<MeassgeSms>();

            if (model.type == "repurchase" || model.type == "guest")
            {
                list.Add(new MeassgeSms
                {
                    contact_number = model.contact_number,
                    msg = "ขอบคุณที่ให้ความสนใจแคมเปญ " + data.title + " รหัสรับสิทธิ์ของท่านคือ " + data.booking_code + " กรุณาแสดงรหัสที่ผู้แทนจำหน่ายที่ท่านลงทะเบียนไว้ เพื่อรับสิทธิประโยชน์ตามเงื่อนไข สอบถามข้อมูลเพิ่มเติมได้ที่ LEXUS Elite Club Call Center 02-305-6799"
                });                                                                                           
            }
            if (model.type == "referral")
                {
                    list.Add(new MeassgeSms
                    {
                        contact_number = model.contact_number,
                        msg = "ขอบคุณที่ให้ความสนใจแคมเปญ " + data.title + " รหัสรับสิทธิ์ของท่านคือ " + data.booking_code + " กรุณาส่งรหัสอ้างอิงให้กับผู้ที่ท่านต้องการแนะนำเพื่อรับสิทธิประโยชน์ตามเงื่อนไข สอบถามเพิ่มเติมโทร 02-305-6799",
                    });
                    list.Add(new MeassgeSms
                    {
                        contact_number = model.referral_contact_number,
                        msg = "ท่านได้รับรหัสอ้างอิง " + data.booking_code + " จากการแนะนำของเพื่อนในแคมเปญ " + data.title + " กรุณาแสดงรหัสที่ผู้แทนจำหน่ายเพื่อรับสิทธิประโยชน์ตามเงื่อนไข สอบถามข้อมูลเพิ่มเติมได้ที่ LEXUS Elite Club Call Center 02-305-6799"
                    });
            }

                onlineBooking.message = list;
          
            return onlineBooking;
        }

        public OnlineBookingExportExcelModel GetDataForExportExcel(CampaignRegisterModel model, string token, string language, string register_id)
        {
            var data = new OnlineBookingExportExcelModel();
            List<QuestionAnswers> list = new List<QuestionAnswers>();

            int type_id = 0;
            switch (model.type)
            {      
                case "repurchase":   
                    type_id = 1;
                    break;
                case "referral":
                    type_id = 2;
                    break;
                case "guest":
                    type_id = 3;
                    break;
            }
            try
                {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Empty;
                    if (type_id == 1 || type_id == 2)
                    {
                        cmd = @"
                          		DECLARE @lang NVARCHAR(5) = N'{0}'
                                DECLARE @onlinebooking_id int = N'{1}'							 

								SELECT 
                                T.DEALER_ID,
                                dealer_master.display_en,
								R.preferred_model_id,
                               CASE WHEN @lang = 'EN' THEN B.title_en ELSE B.title_th END AS title,
                               R.booking_code,
						       TC.MODEL
                               FROM [dbo].[booking] B 
						       INNER JOIN [dbo].[booking_register] R
						       ON B.id = R.booking_id   
						       INNER JOIN T_DEALER T 
						       ON R.dealer_id = T.DEALER_ID
                               INNER JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = T.DEALER_CODE 
							   AND dealer_master.branch_code = T.branch_code AND dealer_master.is_active = 1
						       INNER JOIN T_CAR_MODEL TC
					           ON R.preferred_model_id = TC.MODEL_ID
                               WHERE B.is_active = 1 AND B.deleted_flag IS NULL
						       AND R.id = @onlinebooking_id ";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, language, register_id)))
                        {
                            DataRow dr = dt.Rows[0];
                            //value.booking_id = dr.Field<int>("id");
                            data.title = dr["title"] != DBNull.Value ? dr["title"].ToString() : "";
                            data.dealer_name = dr["display_en"] != DBNull.Value ? dr["display_en"].ToString() : "";
                            //list<preferredmodel> c_model = newtonsoft.json.jsonconvert.deserializeobject<list<preferredmodel>>(dr["preferred_model_ids"] != dbnull.value ? dr["preferred_model_ids"].tostring() : "");
                            //var car_name = "";
                            //foreach (var car in c_model)
                            //{
                            //    if (car.id == model.preferred_model_id.tostring())
                            //    { 
                            //        car_name = car.name;
                            //        break;
                            //    }
                            //}
                            //data.car_model = car_name;     
                            data.car_model = dr["MODEL"] != DBNull.Value ? dr["MODEL"].ToString() : "";
                            data.booking_code = dr["booking_code"] != DBNull.Value ? dr["booking_code"].ToString() : ""; ;
                        }
                    }
                    else if (type_id == 3)
                    {
                        cmd = @"
                           DECLARE @lang NVARCHAR(5) = N'{0}'
                           DECLARE @onlinebooking_id int = N'{1}'							 

					       SELECT 
                           T.DEALER_ID,
                           dealer_master.display_en,
						   R.preferred_model_id,
                           CASE WHEN @lang = 'EN' THEN B.title_en ELSE B.title_th END AS title,
                           R.booking_code,
						   TC.MODEL
                           FROM [dbo].[booking] B 
						   INNER JOIN [dbo].[booking_register] R
						   ON B.id = R.booking_id   
						   INNER JOIN T_DEALER T 
						   ON R.dealer_id = T.DEALER_ID
                           INNER JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = T.DEALER_CODE 
						   AND dealer_master.branch_code = T.branch_code AND dealer_master.is_active = 1
						   INNER JOIN T_CAR_MODEL TC
					       ON R.preferred_model_id = TC.MODEL_ID
                           WHERE B.is_active = 1 AND B.deleted_flag IS NULL
						   AND R.id = @onlinebooking_id ";

                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, language, register_id)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.Rows[0];
                                //value.booking_id = dr.Field<int>("id");
                                data.title = dr["title"] != DBNull.Value ? dr["title"].ToString() : "";
                                data.dealer_name = dr["display_en"] != DBNull.Value ? dr["display_en"].ToString() : "";

                                data.car_model = dr["MODEL"] != DBNull.Value ? dr["MODEL"].ToString() : "";
                                data.booking_code = dr["booking_code"] != DBNull.Value ? dr["booking_code"].ToString() : ""; ;
                            }
                            
                        }
                        if (model.question_answers.Count > 0)
                        {
                            cmd = @"
                         		    DECLARE @lang NVARCHAR(5) = N'{0}'
                                    DECLARE @onlinebooking_id int = N'{1}'
                                    DECLARE @device_id NVARCHAR(200) = N'{2}'

                                    select ans.answer_text, CASE WHEN @lang = 'EN' THEN bq.question_en ELSE bq.question_th END AS question
                                    from booking_answer_user ans 
                                    INNER JOIN booking_question bq ON ans.question_id = bq.id 
                                    where ans.created_device_id = @device_id
                                    AND ans.booking_register_id = @onlinebooking_id  ";

                            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, language, register_id, model.device_id)))
                            {
  
                                foreach (DataRow dr in dt.Rows)
                                {
                                    list.Add(new QuestionAnswers
                                    {
                                        questions = dr["question"] != DBNull.Value ? dr["question"].ToString() : "",
                                        answers = dr["answer_text"] != DBNull.Value ? dr["answer_text"].ToString() : ""
                                    }) ;
                                }
                            }
                            data.question_answer = list;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Get Onlinebooking Data For Attach Mail.");
                throw ex;
            }
            return data;
        }
           
        public byte[] ExportExcelAppointment(CampaignRegisterModel model, OnlineBookingExportExcelModel onlineBookingExportExcelModel, string register_id)
        {
            string filePath = "";
            byte[] fileConent = { };
            string newFilePath = "";

            if (model.type == "repurchase")
            {
                if (model.need_to_test_drive)
                {
                    filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\Repurchase_1.xlsx";
                    newFilePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\Repurchase_" + "RE" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", Convert.ToInt32(register_id)) + ".xlsx";
                }
                else
                {
                    filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\Repurchase_2.xlsx";
                    newFilePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\Repurchase_" + "RE" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", Convert.ToInt32(register_id)) + ".xlsx";
                }
            }
            else if (model.type == "referral")
            {
                filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\Referral.xlsx";
                newFilePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\Referral_" + "REF" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", Convert.ToInt32(register_id)) + ".xlsx";
            }
            else if (model.type == "guest")
            {
                filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\Guest.xlsx";
                newFilePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\Guest_" + "LX" + DateTime.Now.ToString("yyyyMMdd") + "-" + String.Format("{0:000}", Convert.ToInt32(register_id)) + ".xlsx";
            }

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                //if (File.Exists(filePath))
                //{
                //    File.Delete(filePath);
                //}
                int currentRow = 16;
                
                using (var package = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        package.Load(stream);
                    }
                    var ws = package.Workbook.Worksheets[1];

                    //var ws2 = package.workbook.worksheets[2].selectedrange["a1:a3"];
                    if (model.type == "repurchase")
                    {
                        ws.Cells[5, 3].Value = onlineBookingExportExcelModel.title;
                        ws.Cells[6, 3].Value = onlineBookingExportExcelModel.booking_code;
                        ws.Cells[7, 3].Value = onlineBookingExportExcelModel.dealer_name;
                        ws.Cells[12, 3].Value = model.name + " " + model.surname;
                        ws.Cells[13, 3].Value = model.contact_number;
                        ws.Cells[14, 3].Value = model.email;
                        ws.Cells[15, 3].Value = onlineBookingExportExcelModel.car_model;
                        ws.Cells[16, 3].Value = onlineBookingExportExcelModel.dealer_name;
                        ws.Cells[19, 3].Value = model.remark;
                    } else if (model.type == "referral") {
                        ws.Cells[5, 3].Value = onlineBookingExportExcelModel.title;
                        ws.Cells[6, 3].Value = onlineBookingExportExcelModel.booking_code;
                        ws.Cells[7, 3].Value = onlineBookingExportExcelModel.dealer_name;
                        ws.Cells[12, 3].Value = model.referral_name + " " + model.referral_surname;
                        ws.Cells[13, 3].Value = model.referral_contact_number;
                        ws.Cells[14, 3].Value = model.referral_email;
                        ws.Cells[15, 3].Value = onlineBookingExportExcelModel.car_model;
                        ws.Cells[16, 3].Value = onlineBookingExportExcelModel.dealer_name;
                        ws.Cells[17, 3].Value = model.remark;
                        ws.Cells[23, 3].Value = model.name + " " + model.surname;
                        ws.Cells[24, 3].Value = model.contact_number;
                        ws.Cells[25, 3].Value = model.email;
                        ws.Cells[26, 3].Value = model.plate_number;
                        ws.Cells[27, 3].Value = model.car_model;
                    } else if (model.type == "guest") {
                        ws.Cells[5, 3].Value = onlineBookingExportExcelModel.title;
                        ws.Cells[6, 3].Value = onlineBookingExportExcelModel.booking_code;
                        ws.Cells[7, 3].Value = onlineBookingExportExcelModel.dealer_name;
                        ws.Cells[12, 3].Value = model.name + " " + model.surname;
                        ws.Cells[13, 3].Value = model.contact_number;
                        ws.Cells[14, 3].Value = model.email;
                        ws.Cells[15, 3].Value = onlineBookingExportExcelModel.car_model;
                        ws.Cells[16, 3].Value = onlineBookingExportExcelModel.dealer_name;

                        if ( onlineBookingExportExcelModel.question_answer != null)
                        {
                            foreach (var data in onlineBookingExportExcelModel.question_answer)
                            {
                                currentRow++;

                                ws.Cells[currentRow, 3, currentRow, 5].Merge = true;
                                ws.Cells[currentRow, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                ws.Cells[currentRow, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                ws.Cells[currentRow, 1].Value = data.questions;
                                ws.Cells[currentRow, 3].Value = data.answers;
                            }
                        }
     
                        currentRow++;
                        ws.Cells[currentRow, 3, currentRow, 5].Merge = true;
                        ws.Cells[currentRow, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 1].Value = "หมายเหตุ";
                        ws.Cells[currentRow, 3].Value = model.remark;

                        foreach (var data in getDataExcel())
                        {
                            currentRow++;
                            ws.Cells[currentRow, 3, currentRow, 5].Merge = true;
                            ws.Cells[currentRow, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            ws.Cells[currentRow, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //ws.Cells["A18:J18"].Style.Border.Top.Color.SetColor(Color.Black);
                            ws.Cells[currentRow, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[currentRow, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(146,208,80));
                            ws.Cells[currentRow, 3].Value = "โปรดระบุ";
                            ws.Cells[currentRow, 3].Style.Font.Color.SetColor(System.Drawing.Color.Red);

                            ws.Cells[currentRow, data.column].Value = data.text;
                        }

                        foreach (var data in getDataPartTwo())
                        {
                            currentRow++;
                            
                            ws.Cells[currentRow, data.column].Value = data.text;
                        }

                        currentRow++;
                        ws.Cells[currentRow, 2, currentRow + 3, 4].Merge = true;
                        ws.Cells[currentRow, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        ws.Cells[currentRow, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[currentRow, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        ws.Cells[currentRow, 2].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        ws.Cells[currentRow, 2].Value = "โปรดระบุ";
                        ws.Cells[currentRow, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow +1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow + 1, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow + 2, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow +2 , 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow + 3, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow + 3, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow + 3, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow + 3, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells[currentRow + 3, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        int rowFooter = currentRow + 4;
                        
                        foreach (var data in getDataFooter())
                        {
                            rowFooter++;

                            ws.Cells[rowFooter, data.column].Value = data.text;
                        }
                    }
                    FileInfo file = new FileInfo(newFilePath);
                    package.SaveAs(file);

                    fileConent = File.ReadAllBytes(newFilePath);
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Export Excel");
                throw ex;
            }
            return fileConent;
        }

        private List<OnlineBookingGuestExcelData> getDataExcel()
        {
            var data = new List<OnlineBookingGuestExcelData>();
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = "วันส่งมอบรถ (Delivery Date)"
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = "หมายเลขตัวถัง"
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = "เลขที่ใบเสร็จ"
            });
            return data;
        }

        private List<OnlineBookingGuestExcelData> getBorderExcel()
        {
            var data = new List<OnlineBookingGuestExcelData>();
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 2,
                text = "โปรดระบุ"
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 2,
                text = ""
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 2,
                text = ""
            });
            return data;
        }

        private List<OnlineBookingGuestExcelData> getDataPartTwo()
        {
            var data = new List<OnlineBookingGuestExcelData>();
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = ""
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = ""
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = "ส่วนที่ 2 กรุณาตอบแบบสอบถามด้านล่าง และตอบกลับมาที่อีเมล lexus02@toyotaconnected.co.th ภายใน 1 วันหลังจากติดต่อลูกค้าได้แล้ว"
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 2,
                text = "ฟีดแบคหรือข้อเสนอแนะของลูกค้า"
            });
           
            return data;
        }

        private List<OnlineBookingGuestExcelData> getDataFooter()
        {
            var data = new List<OnlineBookingGuestExcelData>();
    
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = "กรุณาส่งกลับมาที่อีเมล lexus02@toyotaconnected.co.th"
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = "หรือติดต่อ LEXUS Elite Club Call Center โทร 02-305-6799"
            });
            data.Add(new OnlineBookingGuestExcelData()
            {
                column = 1,
                text = "ทุกวันเวลา 10.00 - 20.00 น. ไม่เว้นวันหยุดนักขัตฤกษ์"
            });
            return data;
        }

        public async Task<OnlineBookingExportExcelModel> GetDataForEmail(int onlinebooking_id, string token, string language)
        {
            OnlineBookingExportExcelModel data = new OnlineBookingExportExcelModel();
            CampaignRegisterModel campaignRegisterModel = new CampaignRegisterModel();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @onlinebooking_id int = '{0}'
                           DECLARE @lang NVARCHAR(5) = N'{1}'
                           DECLARE @now datetime = CONVERT(date, getdate())
                           DECLARE @tcap_call_center_email NVARCHAR(300)
						    SET @tcap_call_center_email = (SELECT data_config FROM system_config WHERE name = 'call_center_email')
                           SELECT    
						   BR.preferred_model_id,
    					   BR.name,
						   BR.surname,
						   BR.contact_number
						   ,BR.email
						   ,BR.booking_code
						   ,BR.need_to_test_drive AS test_drive 
                           ,BR.referral_name
						   ,BR.referral_surname
						   ,BR.referral_contact_number
						   ,BR.referral_email
						   ,BR.car_model
						   ,BR.type
                           ,BR.plate_number
                           ,T.DEALER_ID
                           ,dealer_master.display_en
                           ,B.preferred_model_ids,
                           TC.MODEL,
						   dealer_desc.CallCenterEmail AS call_center_email,
                           @tcap_call_center_email AS tcap_call_center_email ,
                           CASE WHEN @lang = 'EN' THEN B.title_en ELSE B.title_th END AS title
                           FROM [dbo].[booking_register] BR 
						   INNER JOIN [dbo].[booking] B
						   ON B.id = BR.booking_id
						   INNER JOIN T_DEALER T 
						   ON BR.dealer_id = T.DEALER_ID
						   INNER JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = T.DEALER_CODE 
						   AND dealer_master.branch_code = T.branch_code AND dealer_master.is_active = 1
						   LEFT JOIN [dbo].[T_DEALER_WORKINGTIME] AS dealer_desc 
						   ON dealer_desc.Dealer_ID = BR.dealer_id AND dealer_desc.Service_Type = 'OnlineBooking'
                           INNER JOIN T_CAR_MODEL TC
				           ON BR.preferred_model_id = TC.MODEL_ID
                           WHERE B.is_active = 1 AND BR.deleted_flag IS NULL
                           AND BR.id = @onlinebooking_id ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, onlinebooking_id, language)))
                    {
                        DataRow dr = dt.Rows[0];
                        //value.booking_id = dr.Field<int>("id");
                        string preferred_id = dr["preferred_model_id"] != DBNull.Value ? dr["preferred_model_id"].ToString() : "";
                        data.title = dr["title"] != DBNull.Value ? dr["title"].ToString() : "";
                        data.dealer_name = dr["display_en"] != DBNull.Value ? dr["display_en"].ToString() : "";
                        data.test_drive = dr["test_drive"] != DBNull.Value ? dr["test_drive"].ToString() : "";
                        campaignRegisterModel.name = dr["name"] != DBNull.Value ? dr["name"].ToString() : "";
                        campaignRegisterModel.surname = dr["surname"] != DBNull.Value ? dr["surname"].ToString() : "";
                        campaignRegisterModel.contact_number = dr["contact_number"] != DBNull.Value ? dr["contact_number"].ToString() : "";
                        campaignRegisterModel.email = dr["email"] != DBNull.Value ? dr["email"].ToString() : "";
                        campaignRegisterModel.booking_code = dr["booking_code"] != DBNull.Value ? dr["booking_code"].ToString() : "";
                        campaignRegisterModel.type = dr["type"] != DBNull.Value ? dr["type"].ToString() : "";
                        //List<PreferredModel> c_model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PreferredModel>>(dr["preferred_model_ids"] != DBNull.Value ? dr["preferred_model_ids"].ToString() : "");
                        //var car_name = "";
                        //foreach (var car in c_model)
                        //{
                        //    if (car.id == preferred_id)
                        //    {
                        //        car_name = car.name;
                        //        break;
                        //    }
                        //}
                        data.car_model = dr["MODEL"] != DBNull.Value ? dr["MODEL"].ToString() : "";
                        data.campaignRegisterModel = campaignRegisterModel;
                        data.call_center_email = dr["call_center_email"].ToString();
                        data.tcap_call_center_email = dr["tcap_call_center_email"].ToString();
                        if (campaignRegisterModel.type == "2")
                        {
                            campaignRegisterModel.referral_name = dr["referral_name"] != DBNull.Value ? dr["referral_name"].ToString() : "";
                            campaignRegisterModel.referral_surname = dr["referral_surname"] != DBNull.Value ? dr["referral_surname"].ToString() : "";
                            campaignRegisterModel.referral_contact_number = dr["referral_contact_number"] != DBNull.Value ? dr["referral_contact_number"].ToString() : "";
                            campaignRegisterModel.referral_email = dr["referral_email"] != DBNull.Value ? dr["referral_email"].ToString() : "";
                            campaignRegisterModel.car_model = dr["car_model"] != DBNull.Value ? dr["car_model"].ToString() : "";
                            campaignRegisterModel.plate_number = dr["plate_number"] != DBNull.Value ? dr["plate_number"].ToString() : "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Get Onlinebooking Data For Attach Mail.");
                throw ex;
            }
            return data;
        }

        public OnlineBookingExportExcelModel GetDataForSms(string onlinebooking_id, string language)
        {
            OnlineBookingExportExcelModel data = new OnlineBookingExportExcelModel();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
                           DECLARE @onlinebooking_id int = '{0}'
                           DECLARE @lang NVARCHAR(5) = N'en'
                         
                           SELECT  
						    BR.booking_code
							,BR.referral_contact_number
							,CASE WHEN @lang = 'EN' THEN B.title_en ELSE B.title_th END AS title
                           FROM [dbo].[booking_register] BR 
						   INNER JOIN [dbo].[booking] B
						   ON B.id = BR.booking_id
                           WHERE B.is_active = 1 AND BR.deleted_flag IS NULL
                           AND BR.id = @onlinebooking_id";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, onlinebooking_id, language)))
                    {
                        DataRow dr = dt.Rows[0];
                       
                        data.title = dr["title"] != DBNull.Value ? dr["title"].ToString() : "";
                        data.booking_code = dr["booking_code"] != DBNull.Value ? dr["booking_code"].ToString() : "";
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Get Onlinebooking Data For Attach Mail.");
                throw ex;
            }
            return data;
        }
    }
}