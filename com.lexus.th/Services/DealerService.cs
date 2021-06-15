using AppLibrary.Database;
using com.lexus.th.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace com.lexus.th
{
    public class DealerService
    {
        private string conn;
        public DealerService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ServiceDealerModel> GetScreenData(int geo_id, string v, string p)
        {
            ServiceDealerModel value = new ServiceDealerModel();
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
                    value.data = new _ServiceDealerData();
                    value.data.dealers = await GetDealers(geo_id, "EN", "DEALER");

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<ServiceDealerModel> GetScreenDataNew(int geo_id, string v, string p, string lang, string type)
        {
            ServiceDealerModel value = new ServiceDealerModel();
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
                    value.data = new _ServiceDealerData();
                    value.data.dealers = await GetDealers(geo_id, lang, type);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<List<DealerModel>> GetDealers(int geo_id, string lang, string type)
        {
            List<DealerModel> list = new List<DealerModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    if (geo_id == 0)
                    {
                        cmd = @"
                           DECLARE @LANG NVARCHAR(5) = N'{1}'
                           
                           SELECT 
                           d.DEALER_ID,
                           d.BRANCH_CODE,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_NAME 
                             ELSE d.dealer_name_th END AS DEALER_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN dealer_master.display_en
                             ELSE dealer_master.display_th END AS DEALER_SHORT_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.BRANCH_NAME 
                             ELSE d.branch_name_th END AS BRANCH_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_ADDRESS 
                             ELSE d.dealer_address_th END AS DEALER_ADDRESS,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_OFFICE_HOURS 
                             ELSE d.DEALER_OFFICE_HOURS_TH END AS DEALER_OFFICE_HOURS,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_OFFICE_HOURS2 
                             ELSE d.DEALER_OFFICE_HOURS2_TH END AS DEALER_OFFICE_HOURS2,
                           d.DEALER_MOBILE,
                           latitude,
                           longitude
                           FROM T_DEALER d
                           LEFT JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = d.DEALER_CODE AND dealer_master.branch_code = d.branch_code AND dealer_master.is_active = 1
                           WHERE d.ACTIVE = 1  AND d.DEL_FLAG IS NULL AND d.type = N'{2}'";
                    }
                    else
                    {
                        cmd = @"
                           DECLARE @LANG NVARCHAR(5) = N'{1}'
                           
                           SELECT 
                           d.DEALER_ID,
                           d.BRANCH_CODE,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_NAME 
                             ELSE d.dealer_name_th END AS DEALER_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN dealer_master.display_en
                             ELSE dealer_master.display_th END AS DEALER_SHORT_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.BRANCH_NAME 
                             ELSE d.branch_name_th END AS BRANCH_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_ADDRESS 
                             ELSE d.dealer_address_th END AS DEALER_ADDRESS,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_OFFICE_HOURS 
                             ELSE d.DEALER_OFFICE_HOURS_TH END AS DEALER_OFFICE_HOURS,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_OFFICE_HOURS2 
                             ELSE d.DEALER_OFFICE_HOURS2_TH END AS DEALER_OFFICE_HOURS2,
                           d.DEALER_MOBILE,
                           latitude,
                           longitude
                           FROM T_DEALER d
                           LEFT JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = d.DEALER_CODE AND dealer_master.branch_code = d.branch_code AND dealer_master.is_active = 1
                           WHERE d.ACTIVE = 1 AND d.GEO_ID = {0} AND d.DEL_FLAG IS NULL AND d.type = N'{2}'";
                    }






                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, geo_id, lang, type)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            DealerModel dealer = new DealerModel();
                            dealer.id = Convert.ToInt32(row["DEALER_ID"]);
                            dealer.name = row["DEALER_NAME"].ToString();
                            dealer.short_name = row["DEALER_SHORT_NAME"].ToString();
                            dealer.branch_name = row["BRANCH_NAME"].ToString();
                            dealer.address = row["DEALER_ADDRESS"].ToString();
                            dealer.mobile = row["DEALER_MOBILE"].ToString();
                            dealer.office_hours = row["DEALER_OFFICE_HOURS"].ToString();
                            dealer.office_hours2 = row["DEALER_OFFICE_HOURS2"].ToString();
                            dealer.latitude = float.Parse(row["latitude"].ToString());
                            dealer.longitude = float.Parse(row["longitude"].ToString());

                            list.Add(dealer);
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


        public async Task<ServiceDealerHolidayModel> GetAllDataDealerHoliday(string lang, string v, string p, int dealer_id)
        {
            ServiceDealerHolidayModel value = new ServiceDealerHolidayModel();
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
                    value.data = new ServiceDealerHolidayData();

                    value.data.dealer_duration_max = await GetDealerDuration(dealer_id, "MAX");
                    value.data.dealer_duration_min = await GetDealerDuration(dealer_id, "MIN");


                    value.data.dealer_holidays = await GetDealerHoliday(dealer_id, value.data.dealer_duration_min, value.data.dealer_duration_max);




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


        public async Task<List<string>> GetDealerHoliday(int dealer_id, int dealer_duration_min, int dealer_duration_max)
        {
            var isMonday = true;
            var isTuesday = true;
            var isWednesday = true;
            var isThursday = true;
            var isFriday = true;
            var isSaturday = false;
            var isSunday = false;

            List<string> list = new List<string>();
            string value = "";
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @date_rang_start datetime = getdate() - 1 + {1}
                           DECLARE @date_rang_end datetime = getdate() + {2}
                           
                           SELECT Holiday_Date AS holiday_date 
                           FROM [dbo].[T_DEALER_HOLIDAY] 
                           WHERE Dealer_ID = N'{0}'
                             AND Holiday_Date > @date_rang_start 
                             AND Holiday_Date < @date_rang_end 
                                 ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, dealer_id, dealer_duration_min, dealer_duration_max)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            value = (row["holiday_date"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormatStyle2(Convert.ToDateTime(row["holiday_date"].ToString()));
                            list.Add(value);
                        }
                    }

                    string cmd2 = @"
	                            SELECT Is_Monday, Is_Tuesday, Is_Wednesday, Is_Thursday, Is_Friday, Is_Satday, Is_Sunday
	                            FROM T_DEALER_WORKINGTIME 
	                            WHERE Dealer_ID = {0}
	                            AND Service_Type = 'ServiceAppointment'  
                                    ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd2, dealer_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            isMonday = Convert.ToBoolean(row["Is_Monday"]);
                            isTuesday = Convert.ToBoolean(row["Is_Tuesday"]);
                            isWednesday = Convert.ToBoolean(row["Is_Wednesday"]);
                            isThursday = Convert.ToBoolean(row["Is_Thursday"]);
                            isFriday = Convert.ToBoolean(row["Is_Friday"]);
                            isSaturday = Convert.ToBoolean(row["Is_Satday"]);
                            isSunday = Convert.ToBoolean(row["Is_Sunday"]);
                        }
                    }

                    var min = dealer_duration_min;
                    var max = dealer_duration_max;

                    DateTime now = DateTime.Now;
                    List<String> result = new List<String>();
                    for (int i = min; i <= max; i++)
                    {
                        var newDay = now.AddDays(i);
                        switch (newDay.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                if (!isMonday)
                                {
                                    result.Add(createDateHoliday(newDay));
                                }
                                break;
                            case DayOfWeek.Tuesday:
                                if (!isTuesday)
                                {
                                    result.Add(createDateHoliday(newDay));
                                }
                                break;
                            case DayOfWeek.Wednesday:
                                if (!isWednesday)
                                {
                                    result.Add(createDateHoliday(newDay));
                                }
                                break;
                            case DayOfWeek.Thursday:
                                if (!isThursday)
                                {
                                    result.Add(createDateHoliday(newDay));
                                }
                                break;
                            case DayOfWeek.Friday:
                                if (!isFriday)
                                {
                                    result.Add(createDateHoliday(newDay));
                                }
                                break;
                            case DayOfWeek.Saturday:
                                if (!isSaturday)
                                {
                                    result.Add(createDateHoliday(newDay));
                                }
                                break;
                            case DayOfWeek.Sunday:
                                if (!isSunday)
                                {
                                    result.Add(createDateHoliday(newDay));
                                }
                                break;
                        }


                    }

                    list.AddRange(result);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public string createDateHoliday(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public async Task<int> GetDealerDuration(int dealer_id, string tmp_duration_type)
        {
            int value = 0;

            if (!string.IsNullOrEmpty(tmp_duration_type) && tmp_duration_type == "MAX")
            {
                try
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = @"
                           SELECT duration_max AS duration FROM T_DEALER WHERE DEALER_ID =N'{0}'
                                 ";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, dealer_id)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];

                                value = row["duration"] == DBNull.Value ? 0 : Convert.ToInt32(row["duration"]);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (!string.IsNullOrEmpty(tmp_duration_type) && tmp_duration_type == "MIN")
            {
                try
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = @"
                           SELECT duration_min AS duration FROM T_DEALER WHERE DEALER_ID =N'{0}'
                                 ";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, dealer_id)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];

                                value = row["duration"] == DBNull.Value ? 0 : Convert.ToInt32(row["duration"]);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return value;
        }

        public async Task<ServiceDealerWorkingTimeModel> GetAllDataDealerWorkingTimeByDate(string lang, string v, string p, int dealer_id, DateTime dealer_working_date)
        {
            ServiceDealerWorkingTimeModel value = new ServiceDealerWorkingTimeModel();
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
                    value.data = new ServiceDealerWorkingTimeData();
                    value.data.times = await GetDealerWorkingTimeByDate(dealer_id, dealer_working_date);

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

        public async Task<List<ServiceDealerWorkingTimeData_>> GetDealerWorkingTimeByDate(int dealer_id, DateTime dealer_working_date)
        {
            List<ServiceDealerWorkingTimeData_> list = new List<ServiceDealerWorkingTimeData_>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @StartTime time;
                           DECLARE @EndTime time;
                           DECLARE @DealerId int = N'{0}'
                           DECLARE @date datetime2 = N'{1}'
                           DECLARE @dayName NVARCHAR(20) = (SELECT FORMAT(@date, 'dddd', 'en-US'));
                           
                           SELECT
                           	@StartTime = CASE 
                           					WHEN @dayName = 'Monday' THEN Mon_StartTime 
                           					WHEN @dayName = 'Tuesday' THEN Tues_StartTime 
                           					WHEN @dayName = 'Wednesday' THEN Wed_StartTime
                           					WHEN @dayName = 'Thursday' THEN Thur_StartTime
                           					WHEN @dayName = 'Friday' THEN Fri_StartTime
                           					WHEN @dayName = 'Saturday' THEN Sat_StartTime
                           					WHEN @dayName = 'Sunday' THEN Sun_StartTime
                           				END,
                           	@EndTime = CASE 
                           					WHEN @dayName = 'Monday' THEN Mon_EndTime 
                           					WHEN @dayName = 'Tuesday' THEN Tues_EndTime 
                           					WHEN @dayName = 'Wednesday' THEN Wed_EndTime
                           					WHEN @dayName = 'Thursday' THEN Thur_EndTime
                           					WHEN @dayName = 'Friday' THEN Fri_EndTime
                           					WHEN @dayName = 'Saturday' THEN Sat_EndTime
                           					WHEN @dayName = 'Sunday' THEN Sun_EndTime
                           				END
                           FROM [dbo].[T_DEALER_WORKINGTIME] 
                           WHERE Service_Type = 'ServiceAppointment' AND Dealer_ID = @DealerId 
                           
                           /*------------------------------------------------------------------------------------	*/
                          SELECT id,convert(varchar, start_hour, 8) as start_hour , convert(varchar, end_hour, 8) as end_hour FROM utility_generate_time_of_day WHERE start_hour >= @StartTime AND start_hour <= @EndTime AND end_hour <= @EndTime
                                 ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, dealer_id, dealer_working_date)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ServiceDealerWorkingTimeData_ value = new ServiceDealerWorkingTimeData_();
                            value.id = Convert.ToInt32(row["id"]);

                            string start = (row["start_hour"] == DBNull.Value) ? "" : UtilityService.GetTimeFormat(Convert.ToDateTime(row["start_hour"]));
                            string end = (row["end_hour"] == DBNull.Value) ? "" : UtilityService.GetTimeFormat(Convert.ToDateTime(row["end_hour"]));
                            value.name = start.ToString() + " - " + end.ToString();
                            list.Add(value);
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


        //public int GetAllDataDealerByOptional(string v, string p, string lang, string type, )
        //{

        //}

        public async Task<ServiceDealerModel> GetDealersPickup(string v, string p, string lang, string keyword_, double lat_, double lon_, int radius_)
        {
            if (radius_ == 0)
            {
                radius_ = Convert.ToInt16(WebConfigurationManager.AppSettings["default_radius"]);
            }

            ServiceDealerModel value = new ServiceDealerModel();
            value.data = new _ServiceDealerData();
            value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);
            try
            {
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
                    value.data = new _ServiceDealerData();
                    value.data.dealers = await GetDealersPickUp_(keyword_, lat_, lon_, radius_, lang);
                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "UpdateReadTerms");
                throw ex;
            }

            return value;
        }


        public async Task<List<DealerModel>> GetDealersPickUp_(string keyword_, double lat_, double lon_, int radius_, string lang)
        {
            List<DealerModel> list = new List<DealerModel>();

            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
            {
                string cmd = "";

                if (!string.IsNullOrEmpty(keyword_) && lat_ == 0 && lon_ == 0)
                {
                    cmd = @"  
                            DECLARE @keyword NVARCHAR(100) = N'{1}';
                            DECLARE @lat_ float = N'{2}';
                            DECLARE @lon_ float = N'{3}';
                            DECLARE @lang NVARCHAR(2) = N'{4}';

							select T.DEALER_ID AS id, 
                                        CASE  
                                            WHEN @LANG = 'EN' THEN T.DEALER_NAME 
                                              ELSE dealer_name_th 
                                        END AS dealer_name,
                                        CASE  
                                        WHEN @LANG = 'EN' THEN T.BRANCH_NAME 
                                        ELSE T.branch_name_th 
                                        END AS branch_name,
                                        T.latitude AS lat, 
                                        T.longitude AS lon,
                                        T.DEALER_MOBILE AS mobile,
                                        CASE  
                                        WHEN @LANG = 'EN' THEN T.DEALER_ADDRESS 
                                        ELSE T.dealer_address_th 
                                        END AS address,
                                        T.is_pickup_service, 
										T.DEL_FLAG,
                                        T.pin_map_icon_id,
                            [dbo].[fn_get_lat_lng_distance] (@lat_, @lon_,latitude,longitude) as distance
							from T_DEALER T where DEL_FLAG IS NULL and ACTIVE = '1' and (T.DEALER_NAME like '%' + @keyword + '%' OR T.dealer_name_th like '%' + @keyword + '%') ORDER BY distance ASC";
                }
                else
                {
                    cmd = @"
                            DECLARE @radius int = N'{0}';
                            DECLARE @keyword NVARCHAR(100) = N'{1}';
                            DECLARE @lat_ float = N'{2}';
                            DECLARE @lon_ float = N'{3}';
                            DECLARE @lang NVARCHAR(2) = N'{4}';
 
                            SELECT 
                            original_dealer.DEALER_ID as id, 
                            CASE  
                               WHEN @LANG = 'EN' THEN original_dealer.DEALER_NAME 
                               ELSE original_dealer.dealer_name_th 
                               END AS dealer_name,
                            CASE  
                               WHEN @LANG = 'EN' THEN original_dealer.BRANCH_NAME 
                               ELSE original_dealer.branch_name_th 
                               END AS branch_name,
                            original_dealer.DEALER_MOBILE as mobile,
                            original_dealer.is_pickup_service as is_pickup_service,
                            CASE  
                               WHEN @LANG = 'EN' THEN original_dealer.DEALER_ADDRESS 
                               ELSE original_dealer.dealer_address_th 
                               END AS address,
                            original_dealer.latitude as lat, 
                            original_dealer.longitude as lon,
                            original_dealer.pin_map_icon_id,
                            [dbo].[fn_get_lat_lng_distance] (@lat_, @lon_,original_dealer.latitude,  original_dealer.longitude) as distance
                            FROM T_DEALER as original_dealer where DEL_FLAG IS NULL and ACTIVE = '1' and (original_dealer.DEALER_NAME like '%' + @keyword + '%' OR original_dealer.dealer_name_th like '%' + @keyword + '%') ORDER BY distance ASC";
                }

                using (DataTable dt = db.GetDataTableFromCommandText
                         (string.Format(cmd, radius_, keyword_, lat_, lon_, lang)))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DealerModel dealer = new DealerModel();
                        dealer.id = Convert.ToInt32(row["id"]);
                        dealer.branch_name = row["branch_name"] != DBNull.Value ? row["branch_name"].ToString().Trim() : "";
                        dealer.name = row["dealer_name"].ToString();
                        if (!string.IsNullOrEmpty(dealer.branch_name)) {
                            dealer.name = dealer.name + " ("+ dealer.branch_name + ")";
                        }
                        dealer.address = row["address"].ToString();
                        dealer.mobile = row["mobile"].ToString();
                        dealer.latitude = float.Parse(row["lat"].ToString());
                        dealer.longitude = float.Parse(row["lon"].ToString());
                        dealer.is_pickup_service = bool.Parse(row["is_pickup_service"].ToString());
                        dealer.distance = float.Parse(row["distance"].ToString());

                        if (row["pin_map_icon_id"] is DBNull)
                        {
                            dealer.pin_path = GetPinPath((int)Company.Lexus);
                        }
                        else
                        {
                            dealer.pin_path = GetPinPath(Convert.ToInt32(row["pin_map_icon_id"]));
                            if(dealer.pin_path == "")
                            {
                                dealer.pin_path = GetPinPath((int)Company.Lexus);
                            }
                        }
                        list.Add(dealer);
                    }
                }

            }
            return list;
        }


        public string GetPinPath(int company_type)
        {
            string path = "";

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "select path from pin_map_icon where id = N'{0}'";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, company_type)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            path = row["path"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetPinPath");
            }
            return path;
        }



        public async Task<ServiceDealerModel> GetDealersByProvince(string v, string p, string lang, int province_id)
        {
            ServiceDealerModel value = new ServiceDealerModel();
            value.data = new _ServiceDealerData();
            value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);
            try
            {
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
                    value.data = new _ServiceDealerData();
                    value.data.dealers = await GetDealersByProvince(lang, province_id);
                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetDealersByProvince");
                throw ex;
            }

            return value;
        }

        public async Task<List<DealerModel>> GetDealersByProvince(string lang, int province_id)
        {
            List<DealerModel> dealers = new List<DealerModel>();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @" 
                            DECLARE @province_id int = N'{0}'
                            DECLARE @lang VARCHAR(2) = N'{1}'
                            SELECT DISTINCT
                            (SELECT TOP 1 (CASE WHEN @LANG = 'EN' THEN DEALER_NAME ELSE dealer_name_th END) FROM T_DEALER WHERE D.DEALER_CODE = DEALER_CODE AND PROVINCE_ID = @province_id) AS 'DEALER_NAME'
                            ,(SELECT TOP 1 DEALER_ID FROM T_DEALER WHERE D.DEALER_CODE = DEALER_CODE AND PROVINCE_ID = @province_id)  AS 'DEALER_ID'
                            FROM T_DEALER AS d
                            INNER JOIN T_PROVINCE AS p ON p.PROVINCE_ID = d.province_id
                            WHERE d.ACTIVE = 1 and d.DEL_FLAG is null and p.PROVINCE_ID = @province_id
                            ";


                    using (DataTable dt = db.GetDataTableFromCommandText
                             (string.Format(cmd, province_id, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            DealerModel dealer = new DealerModel();
                            dealer.id = Convert.ToInt32(row["DEALER_ID"]);
                            dealer.name = row["DEALER_NAME"].ToString();
                            dealer.address = "";
                            dealer.mobile = "";
                            dealer.latitude = 0;
                            dealer.longitude = 0;
                            dealer.is_pickup_service = false;
                            dealer.pin_path = GetPinPath((int)Company.Lexus);

                            dealers.Add(dealer);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Sub_GetDealersByProvince");
            }


            return dealers;

        }


        public async Task<ServiceBranchModel> GetBranchesByDealer(string v, string p, string lang, int dealer_id)
        {
            ServiceBranchModel value = new ServiceBranchModel();
            value.data = new _ServiceBranchData();
            value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);
            try
            {
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
                    value.data = new _ServiceBranchData();
                    value.data.branches = await GetBranchesByDealer(lang, dealer_id);
                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "GetBranchesByDealer");
                throw ex;
            }

            return value;
        }

        public async Task<List<BranchModel>> GetBranchesByDealer(string lang, int dealer_id)
        {
            List<BranchModel> branches = new List<BranchModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @" 
                            DECLARE @dealer_id int = N'{0}'
                            DECLARE @lang VARCHAR(2) = N'{1}'
                            DECLARE @target_dealer_code VARCHAR(20)= (SELECT DEALER_CODE FROM T_DEALER WHERE DEALER_ID = @dealer_id)
                            DECLARE @province_id int = (SELECT province_id FROM T_DEALER WHERE DEALER_ID = @dealer_id)

                            SELECT 
                               DEALER_ID AS id,
                               CASE  
                            		WHEN @lang = 'EN' THEN BRANCH_NAME
                            		ELSE branch_name_th
                            		END AS name,
                            	CASE  
                            		WHEN @lang = 'EN' THEN DEALER_ADDRESS
                            		ELSE dealer_address_th
                            		END AS address,	
                            		DEALER_MOBILE as mobile,
                                is_pickup_service
                            FROM T_DEALER
                            WHERE ACTIVE = 1 and DEL_FLAG is null and DEALER_CODE = @target_dealer_code and province_id = @province_id
                            ";


                    using (DataTable dt = db.GetDataTableFromCommandText
                             (string.Format(cmd, dealer_id, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            BranchModel branch = new BranchModel();
                            branch.id = row["id"].ToString();
                            branch.name = row["name"].ToString();
                            branch.address = row["address"].ToString();
                            branch.mobile = row["mobile"].ToString();
                            branch.is_pickup_service = row["is_pickup_service"] != DBNull.Value ? Convert.ToBoolean(Convert.ToInt32(row["is_pickup_service"])) : false;

                            branches.Add(branch);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Sub_GetBranchesByDealer");
                throw ex;
            }


            return branches;

        }
    }

}