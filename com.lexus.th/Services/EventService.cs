using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace com.lexus.th
{
    public class EventService
    {
        private string conn;

        public EventService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceEventModel> GetScreenData(string v, string p, string token, string event_id)
        {
            ServiceEventModel value = new ServiceEventModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceEventData();
                    value.data.events =await GetEvents(token, event_id, "EN", p);

                    ValidationModel.InvalidState state;
                    if (value.data.events.id != 0)
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        state = ValidationModel.InvalidState.E507;
                        value.success = false;
                        value.msg = new MsgModel() { code = ValidationModel.GetInvalidCode(state), text = ValidationModel.GetInvalidMessage(state), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceEventModel> GetScreenDataNew(string v, string p, string token, string event_id, string lang)
        {
            ServiceEventModel value = new ServiceEventModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceEventData();
                    value.data.events = await GetEvents(token, event_id, lang, p);

                    ValidationModel.InvalidState state;
                    if (value.data.events.id != 0)
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                    else
                    {
                        state = ValidationModel.InvalidState.E507;
                        value.success = false;
                        value.msg = new MsgModel() { code = ValidationModel.GetInvalidCode(state), text = await ValidationModel.GetInvalidMessageNew(state, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        #region GetEvents(Original) 
        //private EventModel GetEvents(string token, string event_id, string lang)
        //{
        //    EventModel evt = new EventModel();
        //    try
        //    {
        //        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //        {
        //            string cmd = @"
        //                   DECLARE @LANG NVARCHAR(5) = N'{2}'

        //                   SELECT 
        //                   ID,
        //                   IMAGES1_1,
        //                   IMAGES2_1,
        //                   IMAGES3_1,
        //                   IMAGES4_1,
        //                   IMAGES5_1,
        //                   CASE  
        //                     WHEN @LANG = 'EN' THEN TITLE 
        //                     ELSE TITLE END AS TITLE,
        //                   CASE  
        //                     WHEN @LANG = 'EN' THEN [DESC] 
        //                     ELSE desc_th END AS [DESC],
        //                   CASE  
        //                     WHEN @LANG = 'EN' THEN [CONDITION] 
        //                     ELSE condition_th END AS [CONDITION],
        //                   CASE  
        //                     WHEN @LANG = 'EN' THEN [REG_PERIOD] 
        //                     ELSE reg_period_th END AS REG_PERIOD,
        //                   [DATE],
        //                   REG_PERIOD_START,
        //                   REG_PERIOD_END,
        //                   DISPLAY_START,
        //                   DISPLAY_END,
        //                   EVENT_START,
        //                   CASE WHEN ISNULL(REG_PERIOD_END, '1800-01-01') < GETDATE() 
        //                            THEN 1 
        //                        ELSE 0 
        //                        END AS IS_EXPIRE,
        //                   CASE WHEN (SELECT COUNT(1) AS CNT FROM T_EVENTS_REGISTER WHERE EVENT_ID = E.ID AND MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')) = 1 
        //                            THEN 0 
        //                        ELSE 1 
        //                        END AS IS_REGISTER,
        //                   without_guest,
        //                   car_owner_only
        //                   FROM T_EVENTS AS E
        //                   WHERE DEL_FLAG IS NULL 
        //                       AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), DISPLAY_START, 120) AND CONVERT(NVARCHAR(20), DISPLAY_END, 120)
        //                   AND E.ID = N'{1}'";

        //            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, event_id, lang)))
        //            {
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    evt = new EventModel();
        //                    evt.id = Convert.ToInt32(row["ID"]);
        //                    evt.title = row["TITLE"].ToString();
        //                    evt.date = (row["DATE"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["DATE"]));
        //                    evt.time = (row["DATE"] == DBNull.Value) ? "" : UtilityService.GetTimeFormat(Convert.ToDateTime(row["DATE"]));
        //                    evt.desc = row["DESC"].ToString();
        //                    evt.condition = row["CONDITION"].ToString();
        //                    evt.reg_period = row["REG_PERIOD"].ToString();
        //                    evt.reg_period_start = (row["REG_PERIOD_START"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["REG_PERIOD_START"]));
        //                    evt.reg_period_end = (row["REG_PERIOD_END"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["REG_PERIOD_END"]));
        //                    //evt.DisplayStart = (row["DISPLAY_START"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_START"]);
        //                    //evt.DisplayEnd = (row["DISPLAY_END"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(row["DISPLAY_END"]);
        //                    evt.is_expire = Convert.ToBoolean(Convert.ToInt32(row["IS_EXPIRE"].ToString()));
        //                    evt.is_register = Convert.ToBoolean(Convert.ToInt32(row["IS_REGISTER"].ToString()));
        //                    evt.without_guest = Convert.ToBoolean(Convert.ToInt32(row["without_guest"].ToString()));
        //                    evt.car_owner_only = Convert.ToBoolean(Convert.ToInt32(row["car_owner_only"].ToString()));

        //                    evt.images = new List<string>();
        //                    if (!string.IsNullOrEmpty(row["IMAGES1_1"].ToString()))
        //                    {
        //                        evt.images.Add(row["IMAGES1_1"].ToString());
        //                    }
        //                    if (!string.IsNullOrEmpty(row["IMAGES2_1"].ToString()))
        //                    {
        //                        evt.images.Add(row["IMAGES2_1"].ToString());
        //                    }
        //                    if (!string.IsNullOrEmpty(row["IMAGES3_1"].ToString()))
        //                    {
        //                        evt.images.Add(row["IMAGES3_1"].ToString());
        //                    }
        //                    if (!string.IsNullOrEmpty(row["IMAGES4_1"].ToString()))
        //                    {
        //                        evt.images.Add(row["IMAGES4_1"].ToString());
        //                    }
        //                    if (!string.IsNullOrEmpty(row["IMAGES5_1"].ToString()))
        //                    {
        //                        evt.images.Add(row["IMAGES5_1"].ToString());
        //                    }

        //                    //events.Add(evt);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return evt;
        //}
        #endregion
        private async Task<EventModel> GetEvents(string token, string event_id, string lang, string p)
        {
            EventModel evt = new EventModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
        DECLARE @LANG NVARCHAR(5) = N'{2}';
        DECLARE @All_number_of_vin int;
        DECLARE @Remain_number_of_vin_only_event int;
        DECLARE @Remain_number_of_vin_all_event_same_type int;
        DECLARE @Event_Period_Start Date;
        DECLARE @Event_Period_End Date;
        DECLARE @Tmp_Start nvarchar(50);
        DECLARE @Tmp_End nvarchar(50);
        DECLARE @Now DateTime = (select GETDATE());
        DECLARE @Token NVARCHAR(100) = N'{0}';
        DECLARE @Event_id int = N'{1}';
        DECLARE @MEMBERID NVARCHAR(50);
        DECLARE @Enough_Car bit;
        DECLARE @Original_Car_Groups NVARCHAR(50); 
        SET @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @Token)           
   

		SELECT 
                ID,
                CASE  
                  WHEN @LANG = 'EN' THEN TITLE 
                  ELSE TITLE END AS TITLE,
                CASE  
                  WHEN @LANG = 'EN' THEN [DESC] 
                  ELSE desc_th END AS [DESC],
                CASE  
                  WHEN @LANG = 'EN' THEN [CONDITION] 
                  ELSE condition_th END AS [CONDITION],
                CASE  
                  WHEN @LANG = 'EN' THEN [REG_PERIOD] 
                  ELSE reg_period_th END AS REG_PERIOD,
                [DATE],
                REG_PERIOD_START,
                REG_PERIOD_END,
                DISPLAY_START,
                DISPLAY_END,
                EVENT_START,
                CASE WHEN ISNULL(REG_PERIOD_END, '1800-01-01') < GETDATE() 
                         THEN 1 
                     ELSE 0 
                     END AS IS_EXPIRE,
                
                CASE WHEN @Now BETWEEN E.REG_PERIOD_START AND E.REG_PERIOD_END
		                 THEN 1
                     ELSE 0 
                     END AS IS_REGISTER,
                without_guest,
                car_owner_only,
                one_member_per_event
                
                FROM T_EVENTS AS E
                WHERE DEL_FLAG IS NULL 
                    AND CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(20), DISPLAY_START, 120) AND CONVERT(NVARCHAR(20), DISPLAY_END, 120)
               
                AND E.ID = @Event_id 
                            ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, event_id, lang, p)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            evt = new EventModel();
                            evt.id = Convert.ToInt32(row["ID"]);
                            evt.title = row["TITLE"].ToString();
                            evt.date = (row["DATE"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["DATE"]));
                            evt.time = (row["DATE"] == DBNull.Value) ? "" : UtilityService.GetTimeFormat(Convert.ToDateTime(row["DATE"]));
                            evt.desc = row["DESC"].ToString();
                            evt.condition = row["CONDITION"].ToString();
                            evt.reg_period = row["REG_PERIOD"].ToString();
                            evt.reg_period_start = (row["REG_PERIOD_START"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["REG_PERIOD_START"]));
                            evt.reg_period_end = (row["REG_PERIOD_END"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["REG_PERIOD_END"]));
                            evt.is_expire = Convert.ToBoolean(Convert.ToInt32(row["IS_EXPIRE"].ToString()));
                            evt.without_guest = Convert.ToBoolean(Convert.ToInt32(row["without_guest"].ToString()));
                            evt.car_owner_only = Convert.ToBoolean(Convert.ToInt32(row["car_owner_only"].ToString()));
                            evt.images = new List<string>();
                            evt.images = GetAllEventPicture(Convert.ToInt32(row["ID"]));
                            evt.is_one_member_per_event = Convert.ToBoolean(Convert.ToInt32(row["one_member_per_event"].ToString()));

                            //evt.is_register = Convert.ToBoolean(Convert.ToInt32(row["IS_REGISTER"].ToString()));

                            EventDetailControlButton process = new EventDetailControlButton();
                            process =await EventStateProcess_(token, event_id, lang, p);
                            evt.is_register = process.is_register;
                            evt.text_message = process.text_message;
                            evt.remaining_count = process.remaining_count;

                            //evt.text_message = row["text_message"].ToString();

                            //events.Add(evt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return evt;
        }

        public async Task<HistoryEventModel> GetEventHistory(string lang, string v, string p, string token)
        {
            HistoryEventModel value = new HistoryEventModel();
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
                    value.data = await GetHistoryEventData(lang, token);

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

        public async Task<List<EventHistoryDataModel>> GetHistoryEventData(string lang, string token)
        {
            var startDate = WebConfigurationManager.AppSettings["start_history_event_date"];
            var maxHistory = WebConfigurationManager.AppSettings["max_history_event"];

            List<EventHistoryDataModel> histories = new List<EventHistoryDataModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @member_id VARCHAR(50);
                           DECLARE @lang NVARCHAR(5) = N'{0}'
                           SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = '{1}'
                           
                           select top {3} a.id as register_id
                           ,a.EVENT_ID as event_id
						   ,a.redeem_code as register_code
						   ,b.event_type
						   ,a.CREATE_DT
                           ,b.TITLE AS title
                           ,CASE WHEN @lang = 'EN' THEN b.CONDITION ELSE b.condition_th END AS condition
                           ,CASE WHEN @lang = 'EN' THEN b.[DESC] ELSE b.desc_th END AS 'description'
						   ,CASE WHEN @lang = 'EN' THEN b.REG_PERIOD ELSE b.reg_period_th END AS reg_period
						   ,CASE WHEN @lang = 'EN' THEN b.thankyou_message_en ELSE b.thankyou_message_th END AS 'thankyou_message'

                           from T_EVENTS_REGISTER a
						   left join T_EVENTS b on a.EVENT_ID = b.ID
                           where  a.MEMBERID = @member_id and a.CREATE_DT >= '{2}' order by a.CREATE_DT desc";

                    DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, token, startDate, maxHistory));

                    histories = dt.AsEnumerable().Select(row => new EventHistoryDataModel
                    {
                        register_id = row.Field<int>("register_id"),
                        title = row["title"] != DBNull.Value ? row["title"].ToString() : "",
                        date = row.Field<DateTime>("CREATE_DT").ToString("dd/MM/yyyy"),
                        time = row.Field<DateTime>("CREATE_DT").ToString("HH:mm"),
                        description = row["description"] != DBNull.Value ? row["description"].ToString() : "",
                        condition = row["condition"] != DBNull.Value ? row["condition"].ToString() : "",
                        reg_period = row["reg_period"] != DBNull.Value ? row["reg_period"].ToString() : "",
                        thankyou_message = row["thankyou_message"] != DBNull.Value ? row["thankyou_message"].ToString() : "",
                        register_code = row["register_code"] != DBNull.Value ? row["register_code"].ToString() : "",
                        images = GetEventBannerPicture(Convert.ToInt32(row["event_id"]))
                }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return histories;
        }

        public async Task<ServiceYearOfPurchaseModel> GetAllDataYearOfPurchase(string lang, string v, string p)
        {
            ServiceYearOfPurchaseModel value = new ServiceYearOfPurchaseModel();
            try
            {
                value.ts = DateTime.Now;

                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    value.data =await GetAllYearOfPurchaseData(lang, p);
                    value.success = true;
                    value.msg = new MsgModel()
                    {
                        code = 200,
                        text = "Success",
                        store_link = validation2.InvalidStoreLink,
                        version = validation2.InvalidVersion
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<_YearOfPurchaseModel> GetAllYearOfPurchaseData(string lang, string platform)
        {
            _YearOfPurchaseModel value = new _YearOfPurchaseModel();
            value.year_of_purchases = new List<YearOfPurchaseModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"          
                          DECLARE @lang NVARCHAR(2) = N'{0}'
                          DECLARE @currentYear_th NVARCHAR(10) = YEAR(getdate())+543
                          DECLARE @currentYear_en NVARCHAR(10) = YEAR(getdate())
                          print @currentYear_th
                          SELECT  
                            id AS id, 
                            content AS value,
                             1 AS order_display
                          FROM system_master_valuelist 
                          WHERE group_data ='event_register_optional' AND  platform =N'{1}' AND lang = @lang
                          UNION ALL
                          SELECT 
                            id AS id,
                            CASE 
                            	WHEN @lang = 'TH' THEN year_th 
                            	WHEN @lang = 'EN' THEN year_en 
                            END AS value,
                            0 AS order_display
                          FROM [dbo].[utility_generate_year] as master_date
                          WHERE
                          EXISTS (select id, year_th from [dbo].[utility_generate_year] as temp_date where temp_date.year_th >=  @currentYear_th-9 AND temp_date.year_th <= @currentYear_th AND master_date.id = temp_date.id AND @lang = 'TH')
                          OR EXISTS (select id, year_en from [dbo].[utility_generate_year] as temp_date where temp_date.year_en >=  @currentYear_en-9 AND temp_date.year_en <= @currentYear_en AND master_date.id = temp_date.id AND @lang = 'EN')
                          ORDER BY order_display asc, id desc  
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, platform)))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            YearOfPurchaseModel data = new YearOfPurchaseModel();
                            data.id = Convert.ToInt16(dr["id"]);
                            data.value = dr["value"].ToString();
                            value.year_of_purchases.Add(data);
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


        public async Task<ServiceEventVinModel> GetAllDataVin(string token, string lang, string v, string p, int event_id)
        {
            ServiceEventVinModel value = new ServiceEventVinModel();
            try
            {
                value.ts = DateTime.Now;

                SystemController syc = new SystemController();
                ValidationModel validation2 = await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    EventDetailForCalculate detail = await DefindValueForEvent(event_id);

                    MasterDataService data_svr = new MasterDataService();
                    string member_id =await data_svr.GetMemberIdByToken(token);

                   //List<string> used_vins = GetNumberOfVinFromThisEventType(member_id, detail.config_period_start, detail.config_period_end, event_id);

                    value.data = await GetAllVinData(token, lang, event_id);

                    //var result = value.data.cars.Where(t => used_vins.All(t2 => t2.ToString() != t.vin)).ToList();

                    
                    value.success = true;
                    value.msg = new MsgModel()
                    {
                        code = 200,
                        text = "Success",
                        store_link = validation2.InvalidStoreLink,
                        version = validation2.InvalidVersion
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<_ServiceEventVinData> GetAllVinData(string token, string lang, int event_id) 
        {
            _ServiceEventVinData value = new _ServiceEventVinData();
            value.cars = new List<CarModel>();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"  
                      
                        DECLARE @Event_Period_Start Date;
                        DECLARE @Event_Period_End Date;
                        DECLARE @Tmp_Start nvarchar(50);
                        DECLARE @Tmp_End nvarchar(50);
                        DECLARE @Now DateTime = (select GETDATE());
                        DECLARE @Token NVARCHAR(100) = N'{0}';
                        DECLARE @MEMBERID NVARCHAR(50);
                        DECLARE @LANG NVARCHAR(2)= N'{1}';
                        DECLARE @Event_id int = N'{2}';
                        DECLARE @Can_dupplicate_register bit = (select can_dupplicate_register from T_EVENTS where id = @Event_id);
                        DECLARE @can_dupplicate_register_follow_vin bit = (select can_dupplicate_register_follow_vin from T_EVENTS where id = @Event_id);
                      	
				        DECLARE @Date_diff int;
			
                      	
                        SET @MEMBERID = (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @Token)
                        SET @Tmp_Start = (select top 1 data_config FROM system_config where [name] ='event_period_start')
                        SET @Tmp_End = (select top 1 data_config FROM system_config where [name] ='event_period_end')             
                        
                        
                        SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_Start))
                        SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())+1)+ '-' + @Tmp_End))
                        
                        SET @Date_diff = (select datediff(day, @Event_Period_Start, @Event_Period_End))

    IF(@Date_diff < 0 OR @Date_diff = 0 )
   		BEGIN
			--SELECT 'X'
   				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())-1)+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_End))
   		END
	IF(@Date_diff > 366)
		BEGIN
			--SELECT 'Y'
				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+'-' + @Tmp_End))
		END
	IF(@Date_diff < 366 AND @Event_Period_Start < @Now AND @Event_Period_End < @Now)
		BEGIN
			--Select 'Z'
				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())-1)+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_End))
		END
	
	IF(@Date_diff < 366 AND @Event_Period_Start < @Now AND @Event_Period_End > @Now)
		BEGIN
			--Select 'Z1'
				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())+1)+ '-' + @Tmp_End))
		END
	IF(@Date_diff < 366 AND @Event_Period_Start > @Now AND @Event_Period_End > @Now)
		BEGIN
			--Select 'Z2'
				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())-1)+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_End))
		END

IF (@Can_dupplicate_register = 1 AND @can_dupplicate_register_follow_vin = 1)
	BEGIN
					   SELECT DISTINCT
                               CM.MODEL 
                           ,CAR.VIN
                           ,CAR.PLATE_NO
                           ,CM.IMAGE
                              ,CASE  
                          WHEN @LANG = 'EN' THEN dm.display_en 
                          ELSE dm.display_th END AS DEALER
                              ,CAR.RS_Date
                        
                        FROM T_CUSTOMER_CAR AS CAR
                        INNER JOIN T_CAR_MODEL AS CM ON CM.MODEL_ID = CAR.MODEL_ID
                        INNER JOIN T_CUSTOMER AS CUS ON CUS.MEMBERID = CAR.MEMBERID
                        INNER JOIN T_CUSTOMER_TOKEN AS TKN ON TKN.MEMBERID = CUS.MEMBERID
                        INNER JOIN T_DEALER_MASTER AS dm ON CAR.DEALER = dm.id
						where  CAR.MEMBERID = @MEMBERID  AND CAR.DEL_FLAG IS NULL 
					AND ABS(DATEDIFF(month, CAR.rs_date, getdate()))/12 < 10 
	END
ELSE IF (@Can_dupplicate_register = 0 AND @can_dupplicate_register_follow_vin = 1)
	BEGIN			  SELECT DISTINCT
                               CM.MODEL 
                           ,CAR.VIN
                           ,CAR.PLATE_NO
                           ,CM.IMAGE
                              ,CASE  
                          WHEN @LANG = 'EN' THEN dm.display_en 
                          ELSE dm.display_th END AS DEALER
                              ,CAR.RS_Date
                        
                        FROM T_CUSTOMER_CAR AS CAR
                        INNER JOIN T_CAR_MODEL AS CM ON CM.MODEL_ID = CAR.MODEL_ID
                        INNER JOIN T_CUSTOMER AS CUS ON CUS.MEMBERID = CAR.MEMBERID
                        INNER JOIN T_CUSTOMER_TOKEN AS TKN ON TKN.MEMBERID = CUS.MEMBERID
                        INNER JOIN T_DEALER_MASTER AS dm ON CAR.DEALER = dm.id where  CAR.MEMBERID = @MEMBERID  AND CAR.DEL_FLAG IS NULL 
					AND ABS(DATEDIFF(month, CAR.rs_date, getdate()))/12 < 10 
					AND CAR.VIN NOT IN
					(SELECT distinct VIN from T_EVENTS_REGISTER as event_regis 
					where event_regis.EVENT_ID = @Event_id AND event_regis.MEMBERID = @MEMBERID AND (event_regis.CREATE_DT between @Event_Period_Start AND @Event_Period_End))
				
	END
ELSE IF ((@Can_dupplicate_register = 0 AND @can_dupplicate_register_follow_vin = 0) OR (@Can_dupplicate_register = 1 AND @can_dupplicate_register_follow_vin = 0))
BEGIN
			SELECT DISTINCT
                               CM.MODEL 
                           ,CAR.VIN
                           ,CAR.PLATE_NO
                           ,CM.IMAGE
                              ,CASE  
                          WHEN @LANG = 'EN' THEN dm.display_en 
                          ELSE dm.display_th END AS DEALER
                              ,CAR.RS_Date
                        
                        FROM T_CUSTOMER_CAR AS CAR
                        INNER JOIN T_CAR_MODEL AS CM ON CM.MODEL_ID = CAR.MODEL_ID
                        INNER JOIN T_CUSTOMER AS CUS ON CUS.MEMBERID = CAR.MEMBERID
                        INNER JOIN T_CUSTOMER_TOKEN AS TKN ON TKN.MEMBERID = CUS.MEMBERID
                        INNER JOIN T_DEALER_MASTER AS dm ON CAR.DEALER = dm.id where  CAR.MEMBERID = @MEMBERID  AND CAR.DEL_FLAG IS NULL 
					AND ABS(DATEDIFF(month, CAR.rs_date, getdate()))/12 < 10 
					AND CAR.VIN NOT IN 
					(
					select event_regis.vin  from T_EVENTS_REGISTER event_regis 
					inner join t_events on t_events.ID = event_regis.EVENT_ID AND 
						(( t_events.can_dupplicate_register = 0 AND t_events.can_dupplicate_register_follow_vin = 0) 
						OR 
						( t_events.can_dupplicate_register = 1 AND t_events.can_dupplicate_register_follow_vin = 0))
					WHERE event_regis.CREATE_DT between @Event_Period_Start AND @Event_Period_End AND event_regis.memberID = @MEMBERID
					)
	
END ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, lang, event_id)))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            CarModel data = new CarModel();
                            data.model = dr["MODEL"].ToString();
                            data.vin = dr["VIN"].ToString();
                            data.plate_no = dr["PLATE_NO"].ToString();
                            data.image = dr["IMAGE"].ToString();
                            data.dealer = dr["DEALER"].ToString();
                            data.rs_date = dr["RS_Date"].ToString();
                            value.cars.Add(data);
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



        public async Task<ServiceEventVinModel> GetAllDataVins(string token, string lang, string v, string p, int event_id)
        {
            ServiceEventVinModel value = new ServiceEventVinModel();
            try
            {
                value.ts = DateTime.Now;

                SystemController syc = new SystemController();
                ValidationModel validation2 = await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    MasterDataService data_svr = new MasterDataService();
                    string member_id = await data_svr.GetMemberIdByToken(token);

                    value.data = await GetAllVinDatas(token, lang, event_id);

                    value.success = true;
                    value.msg = new MsgModel()
                    {
                        code = 200,
                        text = "Success",
                        store_link = validation2.InvalidStoreLink,
                        version = validation2.InvalidVersion
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<_ServiceEventVinData> GetAllVinDatas(string token, string lang, int event_id)
        {
            _ServiceEventVinData value = new _ServiceEventVinData();
            value.cars = new List<CarModel>();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"  
                        DECLARE @Token NVARCHAR(100) = N'{0}';
                        DECLARE @MEMBERID NVARCHAR(50);
                        DECLARE @LANG NVARCHAR(2)= N'{1}';
                        DECLARE @Event_id int = N'{2}';
                        DECLARE @preferred_model_ids NVARCHAR(MAX) = (SELECT preferred_model_ids FROM T_EVENTS where ID = @Event_id)
                      	
                        DECLARE @date_start int = (select period_car_age_start from t_events where id = @Event_id)
						DECLARE @date_end int = (select period_car_age_end from t_events where id = @Event_id)
						
						DECLARE @compleate_date_from date = dateadd(year, ABS(@date_start)*-1, cast(GETDATE() as date))
						DECLARE @compleate_date_to date = dateadd(year, ABS(@date_end)*-1, cast(GETDATE() as date))
                        
                        IF @date_start = @date_end
	                        BEGIN
		                        SET @compleate_date_to = dateadd(day, 1, dateadd(year, ABS(@date_end + 1)*-1, cast(GETDATE() as date))) 
	                        END

                        SET @MEMBERID = (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @Token)
                        

	BEGIN
					   SELECT 
					   DISTINCT
                               CM.MODEL 
                           ,CAR.VIN
                           ,CAR.PLATE_NO
                           ,CM.IMAGE
                              ,CASE  
                          WHEN @LANG = 'EN' THEN dm.display_en 
                          ELSE dm.display_th END AS DEALER
                              ,CAR.RS_Date
                        FROM T_CUSTOMER_CAR AS CAR
                        INNER JOIN T_CAR_MODEL AS CM ON CM.MODEL_ID = CAR.MODEL_ID AND CAR.DEL_FLAG IS NULL
                        INNER JOIN T_CUSTOMER AS CUS ON CUS.MEMBERID = CAR.MEMBERID
                        INNER JOIN T_CUSTOMER_TOKEN AS TKN ON TKN.MEMBERID = CAR.MEMBERID
                        INNER JOIN T_DEALER_MASTER AS dm ON CAR.DEALER = dm.id
						WHERE  CAR.MEMBERID = @MEMBERID
                        AND CAR.RS_Date BETWEEN @compleate_date_to AND @compleate_date_from
						AND CAR.VIN 
						NOT IN  
						(SELECT events_regis.vin AS used_vin from T_EVENTS_REGISTER AS events_regis
                        -- FULL OUTER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
                        WHERE MEMBERID = @MEMBERID 
						AND EVENT_ID = @event_id)
					    AND CM.MODEL_ID in 
                        (SELECT *
                        FROM OPENJSON(@preferred_model_ids)
                        WITH (
                            ID INT '$.id'
                        ) json
                        WHERE json.id in (SELECT DISTINCT CAR.MODEL_ID FROM T_CUSTOMER_CAR AS CAR WHERE CAR.MEMBERID = @MEMBERID AND CAR.DEL_FLAG is null))
	END
 ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, lang, event_id)))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            CarModel data = new CarModel();
                            data.model = dr["MODEL"].ToString();
                            data.vin = dr["VIN"].ToString();
                            data.plate_no = dr["PLATE_NO"].ToString();
                            data.image = dr["IMAGE"].ToString();
                            data.dealer = dr["DEALER"].ToString();
                            data.rs_date = dr["RS_Date"].ToString();
                            value.cars.Add(data);
                        }
                    }
                }
              
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetAllVinDatas");
                throw ex;
            }
            return value;
        }



        public List<string> GetAllEventPicture(int news_id)
        {
            List<string> list = new List<string>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           SELECT File_Path AS path
						   FROM upload_Image 
						   WHERE upload_Image.Parent_Id = N'{0}'
                           AND upload_Image.Type = 'DETAIL' 
                           AND Page = 'EVENTS'
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, news_id)))
                    {
                        string picture = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            picture = row["path"].ToString();
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
        
        public List<string> GetEventBannerPicture(int news_id)
        {
            List<string> list = new List<string>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           SELECT File_Path AS path
						   FROM upload_Image 
						   WHERE upload_Image.Parent_Id = N'{0}'
                           AND upload_Image.Type = 'DETAIL' 
                           AND Page = 'EVENTS'
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, news_id)))
                    {
                        string picture = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            picture = row["path"].ToString();
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

        public async Task<EventDetailControlButton> EventStateProcess(string token, string event_Id, string lang, string platform)
        {
            int event_id = Convert.ToInt32(event_Id.ToString());

            MasterDataService data_svr = new MasterDataService();
            string member_id = await data_svr.GetMemberIdByToken(token);

            EventDetailForCalculate detail =await DefindValueForEvent(event_id);
            DateTime today = DateTime.Now;

            EventDetailControlButton control_button = new EventDetailControlButton();

            int all_vin_by_member =await GetAllNumberOfCar(member_id);
            int used_vin_by_member_all_event =await GetAllUsedCarAllEvent(member_id, detail.config_period_start, detail.config_period_end);
            int used_vin_from_this_event_type =await GetVinFromThisEventType(member_id, detail.config_period_start, detail.config_period_end, event_id);
            int used_vin_from_this_event =await GetVinFromThisEvent(member_id, detail.config_period_start, detail.config_period_end, event_id);

            bool is_registered =await GetRegistered(member_id, detail.config_period_start, detail.config_period_end, event_id);
            bool is_registered_from_other_event =await CheckVinFromOtherEvent(member_id, detail.config_period_start, detail.config_period_end, event_id);

            int is_registered_from_other_event_false_false =await CheckVinFromOtherEvent_false_false(member_id, detail.config_period_start, detail.config_period_end, event_id);
            int remaining_vin = all_vin_by_member - is_registered_from_other_event_false_false;

            int remaining_vin_from_this_event_type = all_vin_by_member - used_vin_from_this_event_type;
            int remaining_vin_from_this_event = all_vin_by_member - used_vin_from_this_event;

            bool is_registered_from_other_event_true_false =await CheckVinFromOtherEvent_true_false(member_id, detail.config_period_start, detail.config_period_end, event_id);

            bool check_enough_car =await CheckEnoughCarForEvent(event_id, all_vin_by_member);

            bool user_registeg_other_event = CheckUserRegistedOtherEvent(member_id, event_id);

            //Guest (Not Login)

            if (today < detail.register_start)
            {
                control_button.remaining_count = 0;
                control_button.is_register = false;
                control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Comming_Soon), lang, platform);
            }
            else if (today > detail.register_end)
            {
                control_button.remaining_count = 0;
                control_button.is_register = false;
                control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Expire_Event), lang, platform);
            }
            else if (string.IsNullOrEmpty(member_id))
            {
                control_button.remaining_count = 0;
                control_button.is_register = true;
                control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                //this case has alert modal.
            }
            else if (!check_enough_car)//1.check vin enough
            {
                control_button.remaining_count = 0;
                control_button.is_register = true;
                control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
            }


            //Event ซ้าย = true, ขวา = false
            else if (detail.allow_dupplicate_register_event == true && detail.allow_dupplicate_follow_number_of_vin == false)
            {
                if (!is_registered && remaining_vin >= 1 && is_registered_from_other_event == false)
                {
                    control_button.remaining_count = 1;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                }
                else if (!is_registered && remaining_vin >= 1 && is_registered_from_other_event == true)
                {
                    control_button.remaining_count = 0;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                }
                else if (is_registered && remaining_vin >= 1 && is_registered_from_other_event == false)
                {
                    control_button.remaining_count = 0;
                    control_button.is_register = false;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
                }
                else if (is_registered && remaining_vin >= 1 && is_registered_from_other_event == true)
                {
                    control_button.remaining_count = 0;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                    //Alert------
                }
                else
                {
                    control_button.remaining_count = 0;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                    //Alert ------
                }
            }
            else if (detail.allow_dupplicate_register_event == false && detail.allow_dupplicate_follow_number_of_vin == false)
            {
                //int used_vin_by_member_per_event_type = GetAllUsedCarPerEventType(member_id, detail.config_period_start, detail.config_period_end);

                if (is_registered_from_other_event_true_false)
                {
                    remaining_vin -= 1;
                }
                if (is_registered == false && remaining_vin >= 1)
                {
                    control_button.remaining_count = remaining_vin;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                }
                else if (is_registered == true && remaining_vin >= 1)
                {
                    control_button.remaining_count = remaining_vin;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register_With_New_Win), lang, platform);
                }
                else if (is_registered && remaining_vin < 1)
                {
                    control_button.remaining_count = remaining_vin;
                    control_button.is_register = false;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
                }
                else if (is_registered == false && remaining_vin < 1)
                {
                    control_button.remaining_count = remaining_vin;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                    //must have pop up.********
                }
            }
            else if (detail.allow_dupplicate_register_event == true && detail.allow_dupplicate_follow_number_of_vin == true)
            {
                if (remaining_vin_from_this_event_type > 0 && is_registered == false)
                {
                    control_button.remaining_count = 1;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                }
                else if (remaining_vin_from_this_event_type <= 0 && is_registered == false)
                {
                    control_button.remaining_count = 0;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                }
                else if (is_registered == true)
                {
                    control_button.remaining_count = 0;
                    control_button.is_register = false;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
                }

            }
            else if (detail.allow_dupplicate_register_event == false && detail.allow_dupplicate_follow_number_of_vin == true)
            {

                if (remaining_vin_from_this_event > 0 && is_registered == false)
                {
                    control_button.remaining_count = remaining_vin_from_this_event;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                }
                else if (remaining_vin_from_this_event > 0 && is_registered == true)
                {
                    control_button.remaining_count = remaining_vin_from_this_event;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register_With_New_Win), lang, platform);
                }
                else if (remaining_vin_from_this_event <= 0 && is_registered == true)
                {
                    control_button.remaining_count = 0;
                    control_button.is_register = false;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
                }
                else if (remaining_vin_from_this_event <= 0 && is_registered == false)
                {
                    control_button.remaining_count = 0;
                    control_button.is_register = true;
                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                }
            }


                return control_button;
        }



        //public EventDetailControlButton EventStateProcess_Old(string token, string event_Id, string lang, string platform)
        //{
        //    #region Defind all criteria
        //    EventDetailControlButton control_button = new EventDetailControlButton();

        //    int event_id = Convert.ToInt32(event_Id);

        //    MasterDataService data_svr = new MasterDataService();
        //    EventDetailForCalculate data_calculate = new EventDetailForCalculate();
        //    data_calculate = DefindValueForEvent(event_id);

        //    string member_id = data_svr.GetMemberIdByToken(token);



        //    DateTime event_register_start = data_calculate.register_start;
        //    DateTime event_register_end = data_calculate.register_end;
        //    DateTime event_config_period_start = data_calculate.config_period_start;
        //    DateTime event_config_period_end = data_calculate.config_period_end;
        //    DateTime today = DateTime.Now;
        //    bool allow_dupplicate_follow_number_of_vin = data_calculate.allow_dupplicate_follow_number_of_vin;
        //    bool allow_dupplicate_register_event = data_calculate.allow_dupplicate_register_event;


        //    int all_number_of_vin = GetAllNumberOfCar(member_id);
        //    int remaining_vin_for_this_event = GetRemainingVinForEvent(member_id, event_id, event_config_period_start, event_config_period_end);
        //    int remaining_vin_for_other_event = GetRemainingVinForOtherEvent(member_id, event_id, event_config_period_start, event_config_period_end);

        //    bool check_enough_car = CheckEnoughCarForEvent(event_id, all_number_of_vin);
        //    bool user_registed = CheckUserRegistedEvent(member_id, event_id);
        //    bool user_registeg_other_event = CheckUserRegistedOtherEvent(member_id, event_id);

        //    #endregion



        //    //Guest (Not Login)
        //    if (string.IsNullOrEmpty(member_id))
        //    {
        //        control_button.remaining_count = 0;
        //        control_button.is_register = true;
        //        control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //        //this case has alert modal.
        //    }
        //    else if (!check_enough_car)//1.check vin enough
        //    {
        //        control_button.remaining_count = 0;
        //        control_button.is_register = true;
        //        control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //    }
        //    else if (today < event_register_start)
        //    {
        //        control_button.remaining_count = 0;
        //        control_button.is_register = false;
        //        control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Comming_Soon), lang, platform);
        //    }
        //    else if (today > event_register_end)
        //    {
        //        control_button.remaining_count = 0;
        //        control_button.is_register = false;
        //        control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Expire_Event), lang, platform);
        //    }
        //    else
        //    {
        //        //check event can dupplicate.
        //        if (!allow_dupplicate_register_event)
        //        {
        //            if (allow_dupplicate_follow_number_of_vin)
        //            {
        //                if (user_registed)
        //                {
        //                    control_button.remaining_count = 0;
        //                    control_button.is_register = false;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
        //                }
        //                else if (user_registeg_other_event)
        //                {
        //                    control_button.remaining_count = 0;
        //                    control_button.is_register = false;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //                    //has alert.
        //                }
        //                else
        //                {
        //                    control_button.remaining_count = 1;
        //                    control_button.is_register = true;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //                }
        //            }
        //            else
        //            {
        //                if (all_number_of_vin != remaining_vin_for_other_event)
        //                {
        //                    control_button.remaining_count = 1;
        //                    control_button.is_register = true;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //                    //this case must has alert when click.
        //                }
        //                else
        //                {
        //                    control_button.remaining_count = remaining_vin_for_this_event;
        //                    if (all_number_of_vin != remaining_vin_for_this_event && remaining_vin_for_this_event == 0)
        //                    {
        //                        control_button.is_register = false;
        //                        control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
        //                    }
        //                    else if (all_number_of_vin != remaining_vin_for_this_event && remaining_vin_for_this_event > 0)
        //                    {
        //                        control_button.is_register = true;
        //                        control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register_With_New_Win), lang, platform);
        //                    }
        //                    else if(all_number_of_vin == remaining_vin_for_this_event)
        //                    {
        //                        control_button.is_register = true;
        //                        control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //                    }

        //                }
        //            }

        //        }
        //        else
        //        {
        //            //allow_dupplicate_register_event
        //            if (allow_dupplicate_follow_number_of_vin)
        //            {
        //                if (user_registed)
        //                {
        //                    control_button.remaining_count = 0;
        //                    control_button.is_register = false;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
        //                }
        //                else
        //                {
        //                    control_button.remaining_count = 1;
        //                    control_button.is_register = true;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //                }
        //            }
        //            else
        //            {
        //                //control_button.remaining_count = remaining_vin_for_this_event;
        //                control_button.remaining_count = 8;
        //                if (remaining_vin_for_this_event == 0 && user_registed)
        //                {
        //                    control_button.remaining_count = remaining_vin_for_this_event;
        //                    control_button.is_register = false;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
        //                }
        //                else if (remaining_vin_for_this_event == 0 && !user_registed)
        //                {
        //                    control_button.remaining_count = remaining_vin_for_other_event;
        //                    control_button.is_register = true;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //                    //alert.
        //                }
        //                else if(remaining_vin_for_this_event > 0 && user_registed)
        //                {
        //                    control_button.is_register = true;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register_With_New_Win), lang, platform);
        //                }
        //                else if (remaining_vin_for_this_event > 0 && !user_registed)
        //                {
        //                    control_button.remaining_count = remaining_vin_for_this_event;
        //                    control_button.is_register = true;
        //                    control_button.text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
        //                }

        //            }

        //        }
        //    }

        //    return control_button;
        //}

        public async Task<int> GetAllNumberOfCar(string member_id)
        {
            int number_of_car = 0;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"      
                         DECLARE @MEMBERID NVARCHAR(20) = N'{0}';
                         SELECT count(VIN) AS number_of_vin 
                         FROM T_CUSTOMER_CAR AS CUS_CAR 
                         WHERE DEL_FLAG IS NULL 
                         -- AND ABS(DATEDIFF(month, rs_date, getdate()))/12 < 10  
                         AND MEMBERID = @MEMBERID
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            number_of_car = Convert.ToInt32(row["number_of_vin"]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return number_of_car;
        }


        public async Task<bool> CheckEnoughCarForEvent(int event_id, int all_number_of_vin)
        {
            bool enough_car = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"          
                          DECLARE @Event_id int = N'{0}'
						  DECLARE @All_number_of_vin int = N'{1}';
						  DECLARE @Str_event_need_number_of_car nvarchar(20);
                          DECLARE @Enough_car bit;

						  SET @Str_event_need_number_of_car = (SELECT TOP 1 ISNULL(event_group_ids , 0)FROM T_EVENTS WHERE ID = @Event_id)


						  SELECT COUNT(limit_min)  AS count
						  FROM [dbo].[event_group] 
						  WHERE event_group.id in 
						  					(SELECT CAST (a.value('.', 'varchar(max)') AS int)
						  					 FROM
						  				       (SELECT cast('<M>' + REPLACE(@Str_event_need_number_of_car, ',', '</M><M>') + '</M>' AS XML) AS col) AS A
						  						CROSS APPLY A.col.nodes ('/M') AS Split(a))
										     AND @All_number_of_vin BETWEEN limit_min AND limit_max
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id, all_number_of_vin)))
                    {
                        DataRow row = dt.Rows[0];
                        if (Convert.ToInt32(row["count"]) > 0)
                        {
                            enough_car = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return enough_car;
        }

        public string GetSystemMessage(int message_code, string lang, string platform)
        {
            string system_message = "";
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
        DECLARE @Message_code NVARCHAR(20) = N'{0}';
        DECLARE @LANG NVARCHAR(5) = N'{1}';
        DECLARE @Platform NVARCHAR(20) = N'{2}';
        
		select top 1 content from [dbo].[system_message] where code = @Message_code AND lang = @LANG AND platform = @Platform       
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, message_code, lang, platform)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            system_message = row["content"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return system_message;
        }

        public async Task<EventDetailForCalculate> DefindValueForEvent(int event_id)
        {
            EventDetailForCalculate detail = new EventDetailForCalculate();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"      
                           DECLARE @Event_Period_Start Date;
                           DECLARE @Event_Period_End Date;
                           DECLARE @Tmp_Start nvarchar(50);
                           DECLARE @Tmp_End nvarchar(50);
                           DECLARE @Now DateTime = (select GETDATE());
                           DECLARE @Allow_dupplicate_follow_number_of_vin bit;
                           DECLARE @Allow_dupplicate_register_event bit;
                           DECLARE @Event_id int = N'{0}'  
                           DECLARE @Register_start datetime
						   DECLARE @Register_end datetime;
                           
                           DECLARE @Date_diff int;


                           SET @Tmp_Start = (select top 1 data_config FROM system_config where [name] ='event_period_start')
                           SET @Tmp_End = (select top 1 data_config FROM system_config where [name] ='event_period_end')             
                                                          
                           SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_Start))
                           SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())+1)+ '-' + @Tmp_End))
                                 
                           SET @Date_diff = (select datediff(day, @Event_Period_Start, @Event_Period_End))

                           IF(@Date_diff < 0 OR @Date_diff = 0 )
   		BEGIN
			SELECT 'X'
   				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())-1)+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_End))
   		END
	IF(@Date_diff > 366)
		BEGIN
			--SELECT 'Y'
				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+'-' + @Tmp_End))
		END
	IF(@Date_diff < 366 AND @Event_Period_Start < @Now AND @Event_Period_End < @Now)
		BEGIN
			--Select 'Z'
				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())-1)+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_End))
		END
	
	IF(@Date_diff < 366 AND @Event_Period_Start < @Now AND @Event_Period_End > @Now)
		BEGIN
			--Select 'Z1'
				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())+1)+ '-' + @Tmp_End))
		END
	IF(@Date_diff < 366 AND @Event_Period_Start > @Now AND @Event_Period_End > @Now)
		BEGIN
			--Select 'Z2'
				SET @Event_Period_Start = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE())-1)+ '-' + @Tmp_Start))
   				SET @Event_Period_End   = (select CONVERT(DATE, CONVERT(varchar, YEAR(GETDATE()))+ '-' + @Tmp_End))
		END
                           
                            SET @Allow_dupplicate_follow_number_of_vin = (SELECT ISNULL(can_dupplicate_register_follow_vin, 0) FROM T_EVENTS WHERE ID = @Event_id)
                            SET @Allow_dupplicate_register_event = (SELECT ISNULL(can_dupplicate_register, 0) FROM T_EVENTS WHERE ID = @Event_id)
                           
                            SET @Register_start = (SELECT top 1 REG_PERIOD_START FROM T_EVENTS WHERE ID = @Event_id AND DEL_FLAG IS NULL)
							SET @Register_end = (SELECT top 1 REG_PERIOD_END FROM T_EVENTS WHERE ID = @Event_id AND DEL_FLAG IS NULL)

                           SELECT @Event_Period_Start AS event_period_start, 
                                  @Event_Period_End AS event_period_end,
                                  @Allow_dupplicate_follow_number_of_vin AS allow_dupplicate_follow_number_of_vin,
                                  @Allow_dupplicate_register_event AS allow_dupplicate_register_event,
                                  @Register_start AS register_start,
								  @Register_end AS register_end
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            detail.config_period_start = Convert.ToDateTime(row["event_period_start"].ToString());
                            detail.config_period_end = Convert.ToDateTime(row["event_period_end"].ToString());
                            detail.register_start = Convert.ToDateTime(row["register_start"].ToString());
                            detail.register_end = Convert.ToDateTime(row["register_end"].ToString());
                            detail.allow_dupplicate_follow_number_of_vin = Convert.ToBoolean(Convert.ToInt16(row["allow_dupplicate_follow_number_of_vin"]));
                            detail.allow_dupplicate_register_event = Convert.ToBoolean(Convert.ToInt16(row["allow_dupplicate_register_event"]));

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return detail;
        }

        public int GetRemainingVinForEvent(string member_id, int event_id, DateTime period_start, DateTime period_end)
        {
            int remaining_vin = 0;
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                            DECLARE @Event_id int = N'{1}';
                            DECLARE @Event_Period_Start Date = N'{2}';
                            DECLARE @Event_Period_End Date = N'{3}';

                			SELECT DISTINCT
                			       Count(CAR.VIN) AS vin_count
                			FROM T_CUSTOMER_CAR AS CAR
                			
                			WHERE CAR.MEMBERID = @MEMBERID AND CAR.DEL_FLAG IS NULL AND ABS(DATEDIFF(month, CAR.rs_date, getdate()))/12 < 10  AND VIN NOT IN
                				(SELECT event_regis.VIN 
                                 FROM T_CUSTOMER_CAR AS CUS_CAR
                				    INNER JOIN T_EVENTS_REGISTER AS event_regis ON event_regis.MEMBERID = CUS_CAR.MEMBERID 
                				 WHERE CUS_CAR.DEL_FLAG IS NULL AND CUS_CAR.MEMBERID = @MEMBERID 
                                    AND ABS(DATEDIFF(month, CUS_CAR.rs_date, getdate()))/12 < 10  
                				    AND event_regis.EVENT_ID = @Event_id 
                				    AND event_regis.vin = CUS_CAR.VIN
                				    AND event_regis.CREATE_DT BETWEEN @Event_Period_Start AND @Event_Period_End)
                							 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id, period_start, period_end)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            remaining_vin = Convert.ToInt32(row["vin_count"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return remaining_vin;
        }

        public int GetRemainingVinForOtherEvent(string member_id, int event_id, DateTime period_start, DateTime period_end)
        {
            int remaining_vin = 0;
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                            DECLARE @Event_id int = N'{1}';
                            DECLARE @Event_Period_Start Date = N'{2}';
                            DECLARE @Event_Period_End Date = N'{3}';

                			SELECT DISTINCT
                			Count(CAR.VIN) AS vin_count
                			FROM T_CUSTOMER_CAR AS CAR
                			
                			WHERE CAR.MEMBERID = @MEMBERID AND CAR.DEL_FLAG IS NULL AND ABS(DATEDIFF(month, CAR.rs_date, getdate()))/12 < 10 AND VIN NOT IN
                				(SELECT event_regis.VIN from T_CUSTOMER_CAR AS CUS_CAR
                				   INNER JOIN T_EVENTS_REGISTER AS event_regis ON event_regis.MEMBERID = CUS_CAR.MEMBERID 
							       INNER JOIN T_EVENTS AS events ON events.ID = event_regis.EVENT_ID
                				WHERE CUS_CAR.DEL_FLAG IS NULL AND CUS_CAR.MEMBERID = @MEMBERID 
                                   AND events.ID <> @Event_id
                                   AND ABS(DATEDIFF(month, CUS_CAR.rs_date, getdate()))/12 < 10  
                				   AND can_dupplicate_register_follow_vin = (select can_dupplicate_register_follow_vin from T_EVENTS where id = @Event_id)
								   AND can_dupplicate_register = (select can_dupplicate_register from T_EVENTS where id = @Event_id)
                				   AND event_regis.vin = CUS_CAR.VIN
                				   AND event_regis.CREATE_DT BETWEEN @Event_Period_Start AND @Event_Period_End)			 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id, period_start, period_end)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            remaining_vin = Convert.ToInt32(row["vin_count"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return remaining_vin;
        }
        public bool CheckUserRegistedEvent(string member_id, int event_id)
        {
            bool user_registed = false;
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                            DECLARE @Event_id int = N'{1}';

                            SELECT count(ID) AS regis_count from T_EVENTS_REGISTER AS event_regis
                            where event_regis.MEMBERID = @MEMBERID 
                            AND event_regis.EVENT_ID = @Event_id 
                							 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["regis_count"]) > 0)
                            {
                                user_registed = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user_registed;
        }

        public bool CheckUserRegistedOtherEvent(string member_id, int event_id)
        {
            bool user_registed = false;
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                            DECLARE @Event_id int = N'{1}';

                            SELECT count(event_regis.ID) AS regis_count from T_EVENTS_REGISTER AS event_regis
                            INNER JOIN T_EVENTS AS e on e.id = event_regis.event_id
                            WHERE event_regis.MEMBERID = @MEMBERID 
                              AND event_regis.EVENT_ID = @Event_id 
                			  AND e.can_dupplicate_register_follow_vin = (select can_dupplicate_register_follow_vin from T_EVENTS where id = @Event_id)
						      AND e.can_dupplicate_register = (select can_dupplicate_register from T_EVENTS where id = @Event_id)
                              AND e.id <> @Event_id
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["regis_count"]) > 0)
                            {
                                user_registed = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user_registed;
        }

        #region new method
        public async Task<int> GetAllUsedCarAllEvent(string member_id, DateTime period_start, DateTime period_end)
        {
            int event_count = 0;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                        DECLARE @Event_Period_Start Date = N'{1}';
                        DECLARE @Event_Period_End Date = N'{2}';
                        
                        
                        SELECT count(event_.ID)AS event_count from T_EVENTS_REGISTER AS events_regis
                        INNER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
                        WHERE MEMBERID = @MEMBERID 
                         AND (events_regis.CREATE_DT between @Event_Period_Start AND @Event_Period_End) 
 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, period_start, period_end)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["event_count"]) > 0)
                            {
                                event_count = Convert.ToInt32(row["event_count"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return event_count;
        }
        public async Task<int> GetVinFromThisEventType(string member_id, DateTime period_start, DateTime period_end, int event_id)
        {
            int event_count = 0;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                        DECLARE @Event_Period_Start Date = N'{1}';
                        DECLARE @Event_Period_End Date = N'{2}';
                        DECLARE @Event_Id int = N'{3}';
                        
                      SELECT count(events_regis.ID) AS event_count from T_EVENTS_REGISTER AS events_regis
                        INNER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
                      WHERE MEMBERID = @MEMBERID 
                         AND (events_regis.CREATE_DT between @Event_Period_Start AND @Event_Period_End) 
                 		 AND can_dupplicate_register_follow_vin = (select can_dupplicate_register_follow_vin from T_EVENTS where id = @Event_id) 
						 AND can_dupplicate_register = (select can_dupplicate_register from T_EVENTS where id = @Event_id)
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, period_start, period_end, event_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["event_count"]) > 0)
                            {
                                event_count = Convert.ToInt32(row["event_count"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return event_count;
        }

        public List<string> GetNumberOfVinFromThisEventType(string member_id, DateTime period_start, DateTime period_end, int event_id)
        {
            List<string> cars = new List<string>();
            string car = "";
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                        DECLARE @Event_Period_Start Date = N'{1}';
                        DECLARE @Event_Period_End Date = N'{2}';
                        DECLARE @Event_Id int = N'{3}';
                        
                       SELECT distinct events_regis.vin AS vin
					   FROM T_EVENTS_REGISTER AS events_regis
                        INNER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
                       WHERE MEMBERID = @MEMBERID 
                         AND (events_regis.CREATE_DT between @Event_Period_Start AND @Event_Period_End) 
                 		 AND can_dupplicate_register_follow_vin = (select can_dupplicate_register_follow_vin from T_EVENTS where id = @Event_id) 
						 AND can_dupplicate_register = (select can_dupplicate_register from T_EVENTS where id = @Event_id)
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, period_start, period_end, event_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            car = row["vin"].ToString();
                            cars.Add(car);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cars;
        }
        public async Task<int> GetVinFromThisEvent(string member_id, DateTime period_start, DateTime period_end, int event_id)
        {
            int event_count = 0;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                        DECLARE @Event_Period_Start Date = N'{1}';
                        DECLARE @Event_Period_End Date = N'{2}';
                        DECLARE @Event_Id int = N'{3}';
                        
                      SELECT count(events_regis.ID) AS event_count from T_EVENTS_REGISTER AS events_regis
                        INNER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
                      WHERE MEMBERID = @MEMBERID 
                         AND (events_regis.CREATE_DT between @Event_Period_Start AND @Event_Period_End) 
                 		 AND event_.ID = @Event_Id
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, period_start, period_end, event_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["event_count"]) > 0)
                            {
                                event_count = Convert.ToInt32(row["event_count"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return event_count;
        }

        public async Task<bool> GetRegistered(string member_id, DateTime period_start, DateTime period_end, int event_id)
        {
            bool is_registered = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                        DECLARE @Event_Period_Start Date = N'{1}';
                        DECLARE @Event_Period_End Date = N'{2}';
                        DECLARE @Event_id int = N'{3}';
                        
                        SELECT count(event_.ID) AS regis_count from T_EVENTS_REGISTER AS events_regis
                        INNER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
                        WHERE MEMBERID = @MEMBERID 
                          AND event_.ID = @Event_id
                          AND (events_regis.CREATE_DT between @Event_Period_Start AND @Event_Period_End) 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, period_start, period_end, event_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["regis_count"]) > 0)
                            {
                                is_registered = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return is_registered;
        }


        //count vin from same registered event type.
        // is_regitered_another_event = true  
        public async  Task<bool> CheckVinFromOtherEvent(string member_id, DateTime period_start, DateTime period_end, int event_id)
        {
            bool is_registered_another_event = false;
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                            DECLARE @Event_id int = N'{1}';
                            DECLARE @Event_Period_Start Date = N'{2}';
                            DECLARE @Event_Period_End Date = N'{3}';

                			SELECT DISTINCT
                			Count(CAR.VIN) AS regis_count
                			FROM T_CUSTOMER_CAR AS CAR
                			
                			WHERE CAR.MEMBERID = @MEMBERID AND CAR.DEL_FLAG IS NULL AND ABS(DATEDIFF(month, CAR.rs_date, getdate()))/12 < 10 AND VIN IN
                				(SELECT event_regis.VIN from T_CUSTOMER_CAR AS CUS_CAR
                				   INNER JOIN T_EVENTS_REGISTER AS event_regis ON event_regis.MEMBERID = CUS_CAR.MEMBERID 
							       INNER JOIN T_EVENTS AS events ON events.ID = event_regis.EVENT_ID
                				WHERE CUS_CAR.DEL_FLAG IS NULL AND CUS_CAR.MEMBERID = @MEMBERID 
                                   AND events.ID <> @Event_id
                                   -- AND ABS(DATEDIFF(month, CUS_CAR.rs_date, getdate()))/12 < 10  
                				   AND can_dupplicate_register_follow_vin = (select can_dupplicate_register_follow_vin from T_EVENTS where id = @Event_id)
								   AND can_dupplicate_register = (select can_dupplicate_register from T_EVENTS where id = @Event_id)
                				   AND event_regis.vin = CUS_CAR.VIN
                				   AND event_regis.CREATE_DT BETWEEN @Event_Period_Start AND @Event_Period_End)			 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id, period_start, period_end)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["regis_count"]) > 0)
                            {
                                is_registered_another_event = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return is_registered_another_event;
        }



        //public int GetAllUsedCarPerEventType()
        //{
        //    int count_car;
        //    try
        //    {
        //        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //        {
        //            string cmd = @"
        //                DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
        //                DECLARE @Event_Period_Start Date = N'{1}';
        //                DECLARE @Event_Period_End Date = N'{2}';


        //                SELECT count(event_.ID)AS event_count from T_EVENTS_REGISTER AS events_regis
        //                INNER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
        //                WHERE MEMBERID = @MEMBERID 
        //                 AND (events_regis.CREATE_DT between @Event_Period_Start AND @Event_Period_End) 

        //                 ";

        //            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, period_start, period_end)))
        //            {
        //                if (dt.Rows.Count > 0)
        //                {
        //                    DataRow row = dt.Rows[0];
        //                    if (Convert.ToInt32(row["event_count"]) > 0)
        //                    {
        //                        event_count = Convert.ToInt32(row["event_count"]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //    return count_car;
        //}
        public async Task<bool> CheckVinFromOtherEvent_true_false(string member_id, DateTime period_start, DateTime period_end, int event_id)
        {
            bool is_registered_other_event_true_false = false;
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                            DECLARE @Event_id int = N'{1}';
                            DECLARE @Event_Period_Start Date = N'{2}';
                            DECLARE @Event_Period_End Date = N'{3}';

                		    SELECT DISTINCT
                			Count(CAR.VIN) AS regis_count
                			FROM T_CUSTOMER_CAR AS CAR
                			
                			WHERE CAR.MEMBERID = @MEMBERID AND CAR.DEL_FLAG IS NULL 
                                   -- AND ABS(DATEDIFF(month, CAR.rs_date, getdate()))/12 < 10 
                                   AND VIN IN
                				(SELECT event_regis.VIN from T_CUSTOMER_CAR AS CUS_CAR
                				   INNER JOIN T_EVENTS_REGISTER AS event_regis ON event_regis.MEMBERID = CUS_CAR.MEMBERID 
							       INNER JOIN T_EVENTS AS events ON events.ID = event_regis.EVENT_ID
                				WHERE CUS_CAR.DEL_FLAG IS NULL AND CUS_CAR.MEMBERID = @MEMBERID 
                                   -- AND ABS(DATEDIFF(month, CUS_CAR.rs_date, getdate()))/12 < 10  
                				   AND can_dupplicate_register_follow_vin = 0
								   AND can_dupplicate_register = 1
                				   AND event_regis.vin = CUS_CAR.VIN
                				   AND event_regis.CREATE_DT BETWEEN @Event_Period_Start AND @Event_Period_End)			 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id, period_start, period_end)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["regis_count"]) > 0)
                            {
                                is_registered_other_event_true_false = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return is_registered_other_event_true_false;
        }

        public async Task<int> CheckVinFromOtherEvent_false_false(string member_id, DateTime period_start, DateTime period_end, int event_id)
        {
            int count_registered_other_event_true_false = 0;
            try
            {

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                            DECLARE @Event_id int = N'{1}';
                            DECLARE @Event_Period_Start Date = N'{2}';
                            DECLARE @Event_Period_End Date = N'{3}';

                		    SELECT DISTINCT
                			Count(CAR.VIN) AS regis_count
                			FROM T_CUSTOMER_CAR AS CAR
                			
                			WHERE CAR.MEMBERID = @MEMBERID AND CAR.DEL_FLAG IS NULL AND ABS(DATEDIFF(month, CAR.rs_date, getdate()))/12 < 10 AND VIN IN
                				(SELECT event_regis.VIN from T_CUSTOMER_CAR AS CUS_CAR
                				   INNER JOIN T_EVENTS_REGISTER AS event_regis ON event_regis.MEMBERID = CUS_CAR.MEMBERID 
							       INNER JOIN T_EVENTS AS events ON events.ID = event_regis.EVENT_ID
                				WHERE CUS_CAR.DEL_FLAG IS NULL AND CUS_CAR.MEMBERID = @MEMBERID 
                                   -- AND ABS(DATEDIFF(month, CUS_CAR.rs_date, getdate()))/12 < 10  
                				   AND can_dupplicate_register_follow_vin = 0
								   AND can_dupplicate_register = 0
                				   AND event_regis.vin = CUS_CAR.VIN
                				   AND event_regis.CREATE_DT BETWEEN @Event_Period_Start AND @Event_Period_End)			 
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id, period_start, period_end)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            if (Convert.ToInt32(row["regis_count"]) > 0)
                            {
                                count_registered_other_event_true_false = Convert.ToInt32(row["regis_count"]);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return count_registered_other_event_true_false;
        }




        #endregion


        #region Check Car Ober 10 Year
        public async Task<ServiceEventModel> CheckAllCarOverTenYear(string token, string lang, string v, string p, int event_id)
        {
            ServiceEventModel value = new ServiceEventModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value = await CheckBlackList(token, lang, p, event_id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<ServiceEventModel> CheckCarOverTenYear(string token, string lang, string platform)
        {
            ServiceEventModel evt = new ServiceEventModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                DECLARE @lang NVARCHAR(5) = N'{1}';      
                DECLARE @Token NVARCHAR(100) = N'{0}';
                DECLARE @member_id NVARCHAR(50) 
                DECLARE @platform NVARCHAR(20) = N'{2}'

                SET @member_id = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @Token)           
                
                DECLARE @all_car int = (select count(*) from t_customer_car where MEMBERID = @member_id )
                DECLARE @over_ten_year_car int = (select count(*) from t_customer_car where MEMBERID = @member_id AND ABS(DATEDIFF(month, rs_date, getdate()))/12 >= 10 )
                DECLARE @message NVARCHAR(MAX) = (select content from system_message where code='20023021' AND lang = @lang AND platform = @platform)
                
                
                IF @all_car = @over_ten_year_car
                	BEGIN
                		SELECT 
                		0 AS status,
                		@message AS message
                	END
                ELSE 
                	BEGIN
                		SELECT 
                			1 AS status,
                			'' AS message
                	END
                
                            ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, lang, platform)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            evt = new ServiceEventModel();
                            evt.msg = new MsgModel();
                            evt.success = Convert.ToBoolean(row["status"]);
                            evt.msg.text = row["message"].ToString();
                        }   
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return evt;
        }


        private async Task<ServiceEventModel> CheckBlackList(string token, string lang, string platform, int event_id)
        {
            ServiceEventModel evt = new ServiceEventModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                DECLARE @lang NVARCHAR(5) = N'{1}';      
                DECLARE @Token NVARCHAR(100) = N'{0}';
                DECLARE @member_id NVARCHAR(50);
                DECLARE @platform NVARCHAR(20) = N'{2}';
                DECLARE @event_id int = N'{3}';
                DECLARE @message NVARCHAR(MAX) = (select top 1 content from system_message where code='21023003' AND lang = @lang AND platform = @platform)
                
                SET @member_id = (SELECT top 1 MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @Token)           
                
                DECLARE @is_blacklist bit = (SELECT COUNT(*) FROM [dbo].[T_EVENTS_BLACKLIST] WHERE deleted_flag IS NULL AND member_id = @member_id AND event_id = @event_id ) 
 
                IF @is_blacklist = 1
                	BEGIN
                		SELECT 
                		0 AS status,
                		@message AS message
                	END
                ELSE 
                	BEGIN
                		SELECT 
                			1 AS status,
                			'' AS message
                	END
                            ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, lang, platform, event_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            evt = new ServiceEventModel();
                            evt.msg = new MsgModel();
                            evt.success = Convert.ToBoolean(row["status"]);
                            evt.msg.text = row["message"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return evt;
        }
        #endregion



        #region renovate event state
        public async Task<EventDetailControlButton> EventStateProcess_(string token, string event_id, string lang, string platform)
        {
            int count_car_can_use = 0;
            if (!string.IsNullOrEmpty(token))
            {
                var checkEventGroup = await CheckEventGroup(token, event_id);//เช็คจำนวนรถ
                var checkEventModel = await CheckEventModel(token, event_id);
                if(checkEventGroup && checkEventModel)
                {
                    _ServiceEventVinData car_can_use = new _ServiceEventVinData();
                    car_can_use = await GetAllVinDatas(token, lang, Convert.ToInt32(event_id));
                    count_car_can_use = car_can_use.cars.Count;
                }
            }
            
            EventDetailControlButton control_button = new EventDetailControlButton();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                DECLARE @token varchar(100) = N'{0}'
                DECLARE @event_Id int = N'{1}'
                DECLARE @count_car_can_use int = N'{2}'

                -- prepair variable for calculate ---------------------------------------------------------------------------
                DECLARE @now datetime = (select dateadd(HOUR, 7, getdate()))
                DECLARE @member_id varchar(100) = (select top 1 MEMBERID from t_customer_token where token_no = @token)
                DECLARE @is_one_member_per_event tinyint = (select top 1 isnull(one_member_per_event, 1) from t_events where id = @event_Id)
                DECLARE @count_registered int = (select count(id) from T_EVENTS_REGISTER where EVENT_ID = @event_Id and MEMBERID = @member_id)
                
                -- main ---------------------------------------------------------------------------------------
                select 
                --@now, REG_PERIOD_START, REG_PERIOD_END, @is_one_member_per_event as xx, @member_id as member_idddd, @count_registered as count,
                case
                	when REG_PERIOD_START > @now  then 'Comming soon'
                	when REG_PERIOD_END < @now  then 'Expire event'
                	when @is_one_member_per_event = 1  and @count_registered > 0 then 'Registered'
					when @is_one_member_per_event = 1  and @count_registered = 0 then 'Register'
                	when @is_one_member_per_event = 0  and @count_car_can_use > 0 and @count_registered > 0 then 'Register with new vin'
					when @is_one_member_per_event = 0  and @count_car_can_use > 0 and @count_registered = 0 then 'Register'
					when @is_one_member_per_event = 0  and @count_car_can_use = 0 and @count_registered > 0 then 'Registered'
                	else 'Register'
                end AS text_message
                ,case
                	when REG_PERIOD_START > @now  then 0
                	when REG_PERIOD_END < @now  then 0
                    when @count_car_can_use = 0 then 0
                	when @is_one_member_per_event = 1  and @count_registered > 0 then 0
					when @is_one_member_per_event = 1  and @count_registered = 0 then 1
                	when @is_one_member_per_event = 0  and @count_car_can_use > 0 then @count_car_can_use
                	else 0
                end as remaining_count
                ,case
                	when REG_PERIOD_START > @now  then 0
                	when REG_PERIOD_END < @now  then 0
                	when @is_one_member_per_event = 1  and @count_registered > 0 then 0
					when @is_one_member_per_event = 1  and @count_registered = 0 then 1
                	when @is_one_member_per_event = 0  and @count_car_can_use > 0 and @count_registered > 0 then 1
					when @is_one_member_per_event = 0  and @count_car_can_use > 0 and @count_registered = 0 then 1
					when @is_one_member_per_event = 0  and @count_car_can_use = 0 and @count_registered > 0 then 0
                	else 1
                end as is_register
                from t_events 
                where ID = @event_Id and DEL_FLAG is null 
                -- and @now between REG_PERIOD_START and REG_PERIOD_END
                            ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, Convert.ToInt16(event_id), count_car_can_use)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            var remaining_count = Convert.ToInt16(row["remaining_count"]);
                            control_button.is_register = Convert.ToBoolean(row["is_register"]);
                            control_button.remaining_count = string.IsNullOrEmpty(token) ? 0 : remaining_count;
                            control_button.text_message = BuildTextMessage(row["text_message"].ToString(), lang, platform);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return control_button;
        }

        public async Task<bool> CheckEventGroup(string token, string event_id)
        {
            bool result = false;
            string cmd = @"
                DECLARE @token varchar(100) = N'{0}'
                DECLARE @event_Id int = N'{1}'
                DECLARE @event_group_ids varchar(20) = (select event_group_ids from T_EVENTS where id = @event_Id)
                DECLARE @member_id NVARCHAR(50) = (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @token)
                DECLARE @count_car int = (SELECT COUNT(*) FROM T_CUSTOMER_CAR AS CAR WHERE CAR.MEMBERID = @member_id AND DEL_FLAG is null)

                SELECT
                (CASE WHEN Min(limit_min) <= @count_car AND @count_car <= Max(limit_max)
                THEN 1
                ELSE 0
                END) AS VALIDATE
                FROM event_group
                where charindex( CAST(ID as nvarchar(20)), @event_group_ids) > 0
                            ";
            EventDetailControlButton control_button = new EventDetailControlButton();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, Convert.ToInt16(event_id))))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            result = row["VALIDATE"] == DBNull.Value ? false : Convert.ToBoolean(row["VALIDATE"]);
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<bool> CheckEventModel(string token, string event_id)
        {
            bool result = false;
            string cmd = @"
                DECLARE @token varchar(100) = N'{0}'
                DECLARE @event_Id int = N'{1}'
                DECLARE @event_group_ids varchar(20) = (select event_group_ids from T_EVENTS where id = @event_Id)
                DECLARE @member_id NVARCHAR(50) = (select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = @token)

                DECLARE @preferred_model_ids NVARCHAR(MAX) = (SELECT preferred_model_ids FROM T_EVENTS where ID = @event_Id)

                SELECT *
                FROM OPENJSON(@preferred_model_ids)
                WITH (
                  ID INT 'strict $.id'
                )
                WHERE id in (SELECT DISTINCT CAR.MODEL_ID FROM T_CUSTOMER_CAR AS CAR WHERE CAR.MEMBERID = @member_id AND CAR.DEL_FLAG is null)
                 ";
            EventDetailControlButton control_button = new EventDetailControlButton();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, Convert.ToInt16(event_id))))
                    {
                        if(dt.Rows.Count > 0)
                        {
                            result = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public string BuildTextMessage(string message, string lang, string platform)
        {
            string text_message = "";

            switch (message) {
                case "Register":
                    text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register), lang, platform);
                    break;
                case "Registered":
                    text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Registered), lang, platform);
                    break;
                case "Register with new vin":
                    text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Register_With_New_Win), lang, platform);
                    break;
                case "Comming soon":
                    text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Comming_Soon), lang, platform);
                    break;
                case "Expire event":
                    text_message = GetSystemMessage(Convert.ToInt32(SystemMessage.Message_Expire_Event), lang, platform);
                    break;
            }
            return text_message;
        }
        #endregion

    }
}