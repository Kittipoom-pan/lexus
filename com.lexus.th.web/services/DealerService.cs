using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
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

        public DataTable GetGeo()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT GEO_ID, GEO_NAME_EN FROM T_GEO ";

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetPinMapIcon()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @ID NVARCHAR(50) = N'{0}'
DECLARE @TABLE TABLE (ID NVARCHAR(50), NAME_EN NVARCHAR(255))

INSERT INTO @TABLE VALUES ('0', 'select')

INSERT INTO @TABLE
SELECT ID, NAME_EN FROM [dbo].[PIN_MAP_ICON]

SELECT * FROM @TABLE";
                  

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetPinMapIconCriteria()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @ID NVARCHAR(50) = N'{0}'
DECLARE @TABLE TABLE (ID NVARCHAR(50), NAME_EN NVARCHAR(255))

INSERT INTO @TABLE VALUES ('0', 'All')

INSERT INTO @TABLE
SELECT ID, NAME_EN FROM [dbo].[PIN_MAP_ICON]

SELECT * FROM @TABLE";


                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetGeoSearchServiceDealer()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT t.GEO_ID, t.GEO_NAME_EN 
FROM T_GEO t LEFT JOIN T_DEALER d ON t.GEO_ID = d.GEO_ID
WHERE d.type = 'SERVICE_CORNER' AND d.DEL_FLAG IS NULL
GROUP BY t.GEO_ID, t.GEO_NAME_EN ORDER BY t.GEO_ID";

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetGeoSearchAuthDealer()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT t.GEO_ID, t.GEO_NAME_EN 
FROM T_GEO t LEFT JOIN T_DEALER d ON t.GEO_ID = d.GEO_ID
WHERE d.type = 'DEALER' AND d.DEL_FLAG IS NULL
GROUP BY t.GEO_ID, t.GEO_NAME_EN ORDER BY t.GEO_ID";

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetDealers(string geo_id, string is_pickup_service)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'
SELECT		[DEALER_ID] AS [ID]
            ,[DEALER_CODE]
			,[DEALER_NAME]
            ,[BRANCH_CODE]
			,[BRANCH_NAME]
			,[DEALER_ADDRESS]
			,[DEALER_MOBILE]
			,[DEALER_OFFICE_HOURS]
            ,[DEALER_OFFICE_HOURS2]
			,[ACTIVE]
            ,[CREATE_DT]
            ,[CREATE_USER]
            ,[UPDATE_DT]
            ,[UPDATE_USER]
            ,latitude
            ,longitude
            ,dealer_name_th
            ,dealer_address_th
            ,branch_name_th
            ,DEALER_OFFICE_HOURS_TH
            ,DEALER_OFFICE_HOURS2_TH
            ,[is_pickup_service]
            ,p.name_en as pin_map_icon
FROM		[T_DEALER]
left join dbo.pin_map_icon p on pin_map_icon_id = p.id
WHERE	DEL_FLAG IS NULL AND GEO_ID = {0} AND type = 'DEALER'";
                if (!string.IsNullOrEmpty(is_pickup_service) && is_pickup_service != "-1")
                {
                    cmd += string.Format(" AND is_pickup_service = N'{0}'", is_pickup_service);
                }
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(geo_id)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetDealersService(string geo_id,string is_pickup_service)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
SELECT		[DEALER_ID] AS [ID]
            ,[DEALER_CODE]
			,[DEALER_NAME]
            ,[BRANCH_CODE]
			,[BRANCH_NAME]
			,[DEALER_ADDRESS]
			,[DEALER_MOBILE]
			,[DEALER_OFFICE_HOURS]
            ,[DEALER_OFFICE_HOURS2]
			,[ACTIVE]
            ,[CREATE_DT]
            ,[CREATE_USER]
            ,[UPDATE_DT]
            ,[UPDATE_USER]
            ,latitude
            ,longitude
            ,dealer_name_th
            ,dealer_address_th
            ,branch_name_th
            ,DEALER_OFFICE_HOURS_TH
            ,DEALER_OFFICE_HOURS2_TH   
            ,[is_pickup_service]
            ,p.name_en as pin_map_icon
FROM		[T_DEALER]
left join dbo.pin_map_icon p on pin_map_icon_id = p.id
WHERE	DEL_FLAG IS NULL AND GEO_ID = {0}
        AND type = 'SERVICE_CORNER'
";
                if (!string.IsNullOrEmpty(is_pickup_service) && is_pickup_service != "-1")
                {
                    cmd += string.Format(" AND is_pickup_service = N'{0}'", is_pickup_service);
                }
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(geo_id)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public void AddDealer(string geoId, string dealerName, string dealerCode, string branchName, string branchCode, string address, string businessHours, string businessHours2, string mobile, string active, string user, string lat, string lng, string dealer_name_th, string branch_name_th, string address_th, string office_hours_th, string office_hours2_th,
            string dealer_duration_min, string dealer_duration_max, string pin_map_icon_id, string is_pickup_service, DealerWorkingTimeModel appointMent, DealerWorkingTimeModel testDrive, List<HolidayDateModel> holidays, DealerWorkingTimeModel booking, string province_id
            )
        {
            try
            {
                string cmd = @"
                            DECLARE @GEO_ID INT = N'{0}'
                            DECLARE @DEALER_NAME NVARCHAR(150) = N'{1}'
                            DECLARE @DEALER_CODE NVARCHAR(50) = N'{2}'
                            DECLARE @BRANCH_NAME NVARCHAR(150) = N'{3}'
                            DECLARE @BRANCH_CODE NVARCHAR(50) = N'{4}'
                            DECLARE @DEALER_ADDRESS NVARCHAR(200) = N'{5}'
                            DECLARE @DEALER_OFFICE_HOURS NVARCHAR(250) = N'{6}'
                            DECLARE @DEALER_OFFICE_HOURS2 NVARCHAR(250) = N'{7}'
                            DECLARE @DEALER_MOBILE NVARCHAR(50) = N'{8}'
                            DECLARE @ACTIVE BIT = N'{9}'
                            DECLARE @USER NVARCHAR(50) = N'{10}'
                            DECLARE @LAT NVARCHAR(50) = N'{11}'
                            DECLARE @LNG NVARCHAR(50) = N'{12}'
                            DECLARE @DEALER_NAME_TH NVARCHAR(150) = N'{13}'
                            DECLARE @BRANCH_NAME_TH NVARCHAR(150) = N'{14}'
                            DECLARE @DEALER_ADDRESS_TH NVARCHAR(200) = N'{15}'
                            DECLARE @DEALER_OFFICE_HOURS_TH NVARCHAR(250) = N'{16}'
                            DECLARE @DEALER_OFFICE_HOURS2_TH NVARCHAR(250) = N'{17}'
                            DECLARE @DURATION_MIN INT = N'{18}'
                            DECLARE @DURATION_MAX INT = N'{19}'
                            DECLARE @PIN_MAP_ICON_ID  INT = N'{20}'
                            DECLARE @IS_PICKUP_SERVICE  BIT = N'{21}'
                            DECLARE @PROVINCE_ID INT = N'{22}'
                            

                            INSERT INTO [T_DEALER] ([GEO_ID],[DEALER_NAME],[DEALER_CODE],[BRANCH_NAME],[BRANCH_CODE],[DEALER_ADDRESS],[DEALER_OFFICE_HOURS],[DEALER_OFFICE_HOURS2],
                            [DEALER_MOBILE],[ACTIVE],[CREATE_DT],[CREATE_USER],type,latitude,longitude,dealer_name_th,branch_name_th,dealer_address_th,DEALER_OFFICE_HOURS_TH,
                            DEALER_OFFICE_HOURS2_TH,[DURATION_MIN],[DURATION_MAX],[pin_map_icon_id],[is_pickup_service], province_id)
                            VALUES (CASE LEN(@GEO_ID) WHEN 0 THEN NULL ELSE @GEO_ID END,
                            		CASE LEN(@DEALER_NAME) WHEN 0 THEN NULL ELSE @DEALER_NAME END,
                            		CASE LEN(@DEALER_CODE) WHEN 0 THEN NULL ELSE @DEALER_CODE END,
                            		CASE LEN(@BRANCH_NAME) WHEN 0 THEN NULL ELSE @BRANCH_NAME END,
                            		CASE LEN(@BRANCH_CODE) WHEN 0 THEN NULL ELSE @BRANCH_CODE END,
                            		CASE LEN(@DEALER_ADDRESS) WHEN 0 THEN NULL ELSE @DEALER_ADDRESS END,
                            		CASE LEN(@DEALER_OFFICE_HOURS) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS END,
                                    CASE LEN(@DEALER_OFFICE_HOURS2) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS2 END,
                            		CASE LEN(@DEALER_MOBILE) WHEN 0 THEN NULL ELSE @DEALER_MOBILE END,
                            		CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
                            		DATEADD(HOUR, 7, GETDATE()),
                            		@USER,
                                    'DEALER',
                                    @LAT,
                                    @LNG,
                                    CASE LEN(@DEALER_NAME_TH) WHEN 0 THEN NULL ELSE @DEALER_NAME_TH END,
                                    CASE LEN(@BRANCH_NAME_TH) WHEN 0 THEN NULL ELSE @BRANCH_NAME_TH END,
                                    CASE LEN(@DEALER_ADDRESS_TH) WHEN 0 THEN NULL ELSE @DEALER_ADDRESS_TH END,
                                    CASE LEN(@DEALER_OFFICE_HOURS_TH) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS_TH END,
                                    CASE LEN(@DEALER_OFFICE_HOURS2_TH) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS2_TH END,
                                    CASE LEN(@DURATION_MIN) WHEN 0 THEN NULL ELSE @DURATION_MIN END,
                                    CASE LEN(@DURATION_MAX) WHEN 0 THEN NULL ELSE @DURATION_MAX END,
                                    CASE LEN(@PIN_MAP_ICON_ID) WHEN 0 THEN NULL ELSE @PIN_MAP_ICON_ID END,
                                    CASE LEN(@IS_PICKUP_SERVICE) WHEN 0 THEN 0 ELSE @IS_PICKUP_SERVICE END,
                                    @PROVINCE_ID
)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(geoId),
                        WebUtility.GetSQLTextValue(dealerName),
                        WebUtility.GetSQLTextValue(dealerCode),
                        WebUtility.GetSQLTextValue(branchName),
                        WebUtility.GetSQLTextValue(branchCode),
                        WebUtility.GetSQLTextValue(address),
                        WebUtility.GetSQLTextValue(businessHours),
                        WebUtility.GetSQLTextValue(businessHours2),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(lat),
                        WebUtility.GetSQLTextValue(lng),
                        WebUtility.GetSQLTextValue(dealer_name_th),
                        WebUtility.GetSQLTextValue(branch_name_th),
                        WebUtility.GetSQLTextValue(address_th),
                        WebUtility.GetSQLTextValue(office_hours_th),
                        WebUtility.GetSQLTextValue(office_hours2_th),
                        WebUtility.GetSQLTextValue(dealer_duration_min),
                        WebUtility.GetSQLTextValue(dealer_duration_max),
                        WebUtility.GetSQLTextValue(pin_map_icon_id),
                        WebUtility.GetSQLTextValue(is_pickup_service),
                        WebUtility.GetSQLTextValue(province_id)
                        ));
                }
                DataRow drDealer = GetDealerByCode(dealerCode);
                if (drDealer != null)
                {
                    if (!DBNull.Value.Equals(drDealer["ID"]))
                    {
                        string id = drDealer["ID"].ToString();
                        if (appointMent != null)
                        {
                            AddWorkingTime(appointMent, id, user);

                        }
                        if (testDrive != null)
                        {
                            AddWorkingTime(testDrive, id, user);
                        }
                        if (holidays != null)
                        {
                            RenewHolliday(holidays, id, user);
                        }
                        if (booking != null)
                        {
                            AddWorkingTime(booking, id, user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddWorkingTime(DealerWorkingTimeModel workingTime, string dealerId, string user)
        {
            try
            {
                #region working Time               

                string cmd = @"
                                            DECLARE @Dealer_ID                  int              =N'{0}'  
                                            DECLARE @Service_Type               nvarchar(100)    =N'{1}'
                                            DECLARE @CallCenterMobile           nvarchar(50)     =N'{2}'
                                            DECLARE @CanncenterEmail            nvarchar(250)    =N'{3}'
                                            DECLARE @Is_Monday                  bit              =N'{4}'
                                            DECLARE @Is_Tuesday                 bit              =N'{5}'
                                            DECLARE @Is_Wednesday               bit              =N'{6}'
                                            DECLARE @Is_Thursday                bit              =N'{7}'
                                            DECLARE @Is_Friday                  bit              =N'{8}'
                                            DECLARE @Is_Satday                  bit              =N'{9}'
                                            DECLARE @Is_Sunday                  bit              =N'{10}'
                                            DECLARE @Mon_StartTime              time(7)          =N'{11}'
                                            DECLARE @Tues_StartTime             time(7)          =N'{12}'
                                            DECLARE @Wed_StartTime              time(7)          =N'{13}'
                                            DECLARE @Thur_StartTime             time(7)          =N'{14}'
                                            DECLARE @Fri_StartTime              time(7)          =N'{15}'
                                            DECLARE @Sat_StartTime              time(7)          =N'{16}'
                                            DECLARE @Sun_StartTime              time(7)          =N'{17}'
                                            DECLARE @Mon_EndTime                time(7)          =N'{18}'
                                            DECLARE @Tues_EndTime               time(7)          =N'{19}'
                                            DECLARE @Wed_EndTime                time(7)          =N'{20}'
                                            DECLARE @Thur_EndTime               time(7)          =N'{21}'
                                            DECLARE @Fri_EndTime                time(7)          =N'{22}'
                                            DECLARE @Sat_EndTime                time(7)          =N'{23}'
                                            DECLARE @Sun_EndTime                time(7)          =N'{24}' 
                                            DECLARE @USER                       nvarchar(50)     =N'{25}'

                                         INSERT INTO[T_Dealer_WorkingTime]
                                                  ([Dealer_ID],[Service_Type],    [CallCenterMobile],[CallCenterEmail] ,[Is_Monday],[Is_Tuesday],    [Is_Wednesday],[Is_Thursday],
                                                  [Is_Friday],[Is_Satday],    [Is_Sunday],[Mon_StartTime],    [Tues_StartTime],[Wed_StartTime],    [Thur_StartTime],[Fri_StartTime],    [Sat_StartTime],[Sun_StartTime],
                                                  [Mon_EndTime],[Tues_EndTime],    [Wed_EndTime],[Thur_EndTime],    [Fri_EndTime],[Sat_EndTime],    [Sun_EndTime],[Created_Date],    [Created_By]
                                            ) 
                                         VALUES ( 
                                                CASE LEN(@Dealer_ID            ) WHEN 0 THEN NULL ELSE @Dealer_ID         END,
                                                CASE LEN(@Service_Type         ) WHEN 0 THEN NULL ELSE @Service_Type      END,
                                                CASE LEN(@CallCenterMobile     ) WHEN 0 THEN NULL ELSE @CallCenterMobile  END,
                                                CASE LEN(@CanncenterEmail      ) WHEN 0 THEN NULL ELSE @CanncenterEmail   END,
                                                CASE LEN(@Is_Monday            ) WHEN 0 THEN NULL ELSE @Is_Monday         END,
                                                CASE LEN(@Is_Tuesday           ) WHEN 0 THEN NULL ELSE @Is_Tuesday        END,
                                                CASE LEN(@Is_Wednesday         ) WHEN 0 THEN NULL ELSE @Is_Wednesday      END,
                                                CASE LEN(@Is_Thursday          ) WHEN 0 THEN NULL ELSE @Is_Thursday       END,
                                                CASE LEN(@Is_Friday            ) WHEN 0 THEN NULL ELSE @Is_Friday         END,
                                                CASE LEN(@Is_Satday            ) WHEN 0 THEN NULL ELSE @Is_Satday         END,
                                                CASE LEN(@Is_Sunday            ) WHEN 0 THEN NULL ELSE @Is_Sunday         END,
                                                CASE LEN(@Mon_StartTime        ) WHEN 0 THEN NULL ELSE @Mon_StartTime     END,
                                                CASE LEN(@Tues_StartTime       ) WHEN 0 THEN NULL ELSE @Tues_StartTime    END,
                                                CASE LEN(@Wed_StartTime        ) WHEN 0 THEN NULL ELSE @Wed_StartTime     END,
                                                CASE LEN(@Thur_StartTime       ) WHEN 0 THEN NULL ELSE @Thur_StartTime    END,
                                                CASE LEN(@Fri_StartTime        ) WHEN 0 THEN NULL ELSE @Fri_StartTime     END,
                                                CASE LEN(@Sat_StartTime        ) WHEN 0 THEN NULL ELSE @Sat_StartTime     END,
                                                CASE LEN(@Sun_StartTime        ) WHEN 0 THEN NULL ELSE @Sun_StartTime     END,
                                                CASE LEN(@Mon_EndTime          ) WHEN 0 THEN NULL ELSE @Mon_EndTime       END,
                                                CASE LEN(@Tues_EndTime         ) WHEN 0 THEN NULL ELSE @Tues_EndTime      END,
                                                CASE LEN(@Wed_EndTime          ) WHEN 0 THEN NULL ELSE @Wed_EndTime       END,
                                                CASE LEN(@Thur_EndTime         ) WHEN 0 THEN NULL ELSE @Thur_EndTime      END,
                                                CASE LEN(@Fri_EndTime          ) WHEN 0 THEN NULL ELSE @Fri_EndTime       END,
                                                CASE LEN(@Sat_EndTime          ) WHEN 0 THEN NULL ELSE @Sat_EndTime       END,
                                                CASE LEN(@Sun_EndTime          ) WHEN 0 THEN NULL ELSE @Sun_EndTime       END,
                                                		DATEADD(HOUR, 7, GETDATE()),
                                                		@USER)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,

                       WebUtility.GetSQLTextValue(dealerId),
                       WebUtility.GetSQLTextValue(workingTime.Service_Type),
                       WebUtility.GetSQLTextValue(workingTime.CallCenterMobile),
                       WebUtility.GetSQLTextValue(workingTime.CanncenterEmail),
                       WebUtility.GetSQLTextValue(workingTime.Is_Monday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Tuesday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Wednesday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Thursday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Friday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Satday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Sunday),
                       WebUtility.GetSQLTextValue(workingTime.Mon_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Tues_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Wed_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Thur_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Fri_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Sat_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Sun_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Mon_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Tues_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Wed_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Thur_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Fri_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Sat_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Sun_EndTime),
                       WebUtility.GetSQLTextValue(user)
                        ));
                }
                #endregion               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateWorkingTime(DealerWorkingTimeModel workingTime, string user)
        {
            try
            {
                #region working Time

                string cmd = @"  DECLARE @Dealer_ID                  int               =N'{0}'  
                                            DECLARE @Service_Type               nvarchar(100)     =N'{1}'
                                            DECLARE @CallCenterMobile           nvarchar(50)      =N'{2}'
                                            DECLARE @CallCenterEmail            nvarchar(250)      =N'{3}'
                                            DECLARE @Is_Monday                  bit               =N'{4}'
                                            DECLARE @Is_Tuesday                 bit               =N'{5}'
                                            DECLARE @Is_Wednesday               bit               =N'{6}'
                                            DECLARE @Is_Thursday                bit               =N'{7}'
                                            DECLARE @Is_Friday                  bit               =N'{8}'
                                            DECLARE @Is_Satday                  bit               =N'{9}'
                                            DECLARE @Is_Sunday                  bit               =N'{10}'
                                            DECLARE @Mon_StartTime              time(7)           =N'{11}'
                                            DECLARE @Tues_StartTime             time(7)           =N'{12}'
                                            DECLARE @Wed_StartTime              time(7)           =N'{13}'
                                            DECLARE @Thur_StartTime             time(7)           =N'{14}'
                                            DECLARE @Fri_StartTime              time(7)           =N'{15}'
                                            DECLARE @Sat_StartTime              time(7)           =N'{16}'
                                            DECLARE @Sun_StartTime              time(7)           =N'{17}'
                                            DECLARE @Mon_EndTime                time(7)           =N'{18}'
                                            DECLARE @Tues_EndTime               time(7)           =N'{19}'
                                            DECLARE @Wed_EndTime                time(7)           =N'{20}'
                                            DECLARE @Thur_EndTime               time(7)           =N'{21}'
                                            DECLARE @Fri_EndTime                time(7)           =N'{22}'
                                            DECLARE @Sat_EndTime                time(7)           =N'{23}'
                                            DECLARE @Sun_EndTime                time(7)           =N'{24}' 
                                            DECLARE @USER                       nvarchar(50)     =N'{25}'

                                            UPDATE [T_Dealer_WorkingTime]
                                            SET		                                            		
                                                    CallCenterMobile    = CASE LEN(@CallCenterMobile ) WHEN 0 THEN NULL ELSE     @CallCenterMobile   END,
                                                    CallCenterEmail     = CASE LEN(@CallCenterEmail  ) WHEN 0 THEN NULL ELSE     @CallCenterEmail    END,
                                                    Is_Monday           = CASE LEN(@Is_Monday        ) WHEN 0 THEN NULL ELSE     @Is_Monday          END,
                                                    Is_Tuesday          = CASE LEN(@Is_Tuesday       ) WHEN 0 THEN NULL ELSE     @Is_Tuesday         END,
                                                    Is_Wednesday        = CASE LEN(@Is_Wednesday     ) WHEN 0 THEN NULL ELSE     @Is_Wednesday       END,
                                                    Is_Thursday         = CASE LEN(@Is_Thursday      ) WHEN 0 THEN NULL ELSE     @Is_Thursday        END,
                                                    Is_Friday           = CASE LEN(@Is_Friday        ) WHEN 0 THEN NULL ELSE     @Is_Friday          END,
                                                    Is_Satday           = CASE LEN(@Is_Satday        ) WHEN 0 THEN NULL ELSE     @Is_Satday          END,
                                                    Is_Sunday           = CASE LEN(@Is_Sunday        ) WHEN 0 THEN NULL ELSE     @Is_Sunday          END,
                                                    Mon_StartTime       = CASE LEN(@Mon_StartTime    ) WHEN 0 THEN NULL ELSE     @Mon_StartTime      END,
                                                    Tues_StartTime      = CASE LEN(@Tues_StartTime   ) WHEN 0 THEN NULL ELSE     @Tues_StartTime     END,
                                                    Wed_StartTime       = CASE LEN(@Wed_StartTime    ) WHEN 0 THEN NULL ELSE     @Wed_StartTime      END,
                                                    Thur_StartTime      = CASE LEN(@Thur_StartTime   ) WHEN 0 THEN NULL ELSE     @Thur_StartTime     END,
                                                    Fri_StartTime       = CASE LEN(@Fri_StartTime    ) WHEN 0 THEN NULL ELSE     @Fri_StartTime      END,
                                                    Sat_StartTime       = CASE LEN(@Sat_StartTime    ) WHEN 0 THEN NULL ELSE     @Sat_StartTime      END,
                                                    Sun_StartTime       = CASE LEN(@Sun_StartTime    ) WHEN 0 THEN NULL ELSE     @Sun_StartTime      END,
                                                    Mon_EndTime         = CASE LEN(@Mon_EndTime      ) WHEN 0 THEN NULL ELSE     @Mon_EndTime        END,
                                                    Tues_EndTime        = CASE LEN(@Tues_EndTime     ) WHEN 0 THEN NULL ELSE     @Tues_EndTime       END,
                                                    Wed_EndTime         = CASE LEN(@Wed_EndTime      ) WHEN 0 THEN NULL ELSE     @Wed_EndTime        END,
                                                    Thur_EndTime        = CASE LEN(@Thur_EndTime     ) WHEN 0 THEN NULL ELSE     @Thur_EndTime       END,
                                                    Fri_EndTime         = CASE LEN(@Fri_EndTime      ) WHEN 0 THEN NULL ELSE     @Fri_EndTime        END,
                                                    Sat_EndTime         = CASE LEN(@Sat_EndTime      ) WHEN 0 THEN NULL ELSE     @Sat_EndTime        END,
                                                    Sun_EndTime         = CASE LEN(@Sun_EndTime      ) WHEN 0 THEN NULL ELSE     @Sun_EndTime        END,		                                               
                                            		UPDATED_Date        = DATEADD(HOUR, 7, GETDATE()),
                                            		UPDATED_By        = @USER  
                                            WHERE	DEALER_ID =@Dealer_ID AND Service_Type = @Service_Type ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                       WebUtility.GetSQLTextValue(workingTime.Dealer_ID),
                       WebUtility.GetSQLTextValue(workingTime.Service_Type),
                       WebUtility.GetSQLTextValue(workingTime.CallCenterMobile),
                       WebUtility.GetSQLTextValue(workingTime.CanncenterEmail),
                       WebUtility.GetSQLTextValue(workingTime.Is_Monday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Tuesday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Wednesday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Thursday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Friday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Satday),
                       WebUtility.GetSQLTextValue(workingTime.Is_Sunday),
                       WebUtility.GetSQLTextValue(workingTime.Mon_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Tues_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Wed_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Thur_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Fri_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Sat_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Sun_StartTime),
                       WebUtility.GetSQLTextValue(workingTime.Mon_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Tues_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Wed_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Thur_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Fri_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Sat_EndTime),
                       WebUtility.GetSQLTextValue(workingTime.Sun_EndTime),
                       WebUtility.GetSQLTextValue(user)
                        ));
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RenewHolliday(List<HolidayDateModel> holidays, string dealerId, string user)
        {
            try
            {
                #region Holiday    



                string allCmd = @" DELETE FROM [T_Dealer_Holiday] WHERE Dealer_ID = N'" + WebUtility.GetSQLTextValue(dealerId) + "'";


                foreach (var hol in holidays)
                {
                    string cmd = @"   
                                        INSERT INTO[T_Dealer_Holiday]  ([Dealer_ID],[Holiday_Date],[Created_Date],[Created_By] ) 
                                         VALUES ( CASE LEN(N'" + WebUtility.GetSQLTextValue(dealerId) + @"') WHEN 0 THEN NULL ELSE " + WebUtility.GetSQLTextValue(dealerId) + @"         END,
                                                  CASE LEN(N'" + WebUtility.GetSQLTextValue(hol.Holiday_Date.ToString("dd/MM/yyyy")) + @"') WHEN 0 THEN NULL ELSE '" + WebUtility.GetSQLTextValue(hol.Holiday_Date.ToString("yyyy-MM-dd")) + @"'      END,                                               
                                                  DATEADD(HOUR, 7, GETDATE()),
                                                  '" + WebUtility.GetSQLTextValue(user) + @"' ) ";
                    allCmd += cmd;
                }
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(allCmd);
                }
                #endregion               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RenewHollidayFromUpload(List<HolidayDateModel> holidays, string user)
        {


            try
            {
                #region Holiday    

                string allCmd = @" ";


                foreach (var id in holidays.Select(c => c.Dealer_ID).Distinct().ToList())
                {
                    DataRow dr = GetDealerById(id.ToString());
                    if (dr == null)
                    {
                        throw new Exception("Dealer_ID Invalid!!");
                    }

                    string cmdDel = @" 
                                    DELETE FROM [T_Dealer_Holiday] WHERE Dealer_ID=" + WebUtility.GetSQLTextValue(id.ToString());
                    allCmd += cmdDel;

                    foreach (var hol in holidays.Where(c => c.Dealer_ID == id).ToList())
                    {
                        string cmd = @"   
                                        INSERT INTO[T_Dealer_Holiday]  ([Dealer_ID],[Holiday_Date],[Created_Date],[Created_By] ) 
                                         VALUES ( CASE LEN(" + WebUtility.GetSQLTextValue(hol.Dealer_ID.ToString()) + @") WHEN 0 THEN NULL ELSE " + WebUtility.GetSQLTextValue(hol.Dealer_ID.ToString()) + @"         END,
                                                  CASE LEN('" + WebUtility.GetSQLTextValue(hol.Holiday_Date.ToString("dd/MM/yyyy")) + @"') WHEN 0 THEN NULL ELSE '" + WebUtility.GetSQLTextValue(hol.Holiday_Date.ToString("yyyy-MM-dd")) + @"'      END,                                               
                                                  DATEADD(HOUR, 7, GETDATE()),
                                                  '" + WebUtility.GetSQLTextValue(user) + @"' ) ";
                        allCmd += cmd;
                    }

                }

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(allCmd);
                }
                #endregion               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void UpdateDealer(string geoId, string dealerName, string dealerCode, string branchName, string branchCode, string address, string businessHours, string businessHours2, string mobile, string active, string user, string id, string lat, string lng, string dealer_name_th, string branch_name_th, string address_th, string office_hours_th, string office_hours2_th,
            string dealer_duration_min, string dealer_duration_max, string pin_map_icon_id, string is_pickup_service, DealerWorkingTimeModel appointMent, DealerWorkingTimeModel testDrive, List<HolidayDateModel> holidays, DealerWorkingTimeModel booking, string province_id
            )
        {
            try
            {
                string cmd = @"
                                DECLARE @GEO_ID INT = N'{0}'
                                DECLARE @DEALER_NAME NVARCHAR(150) = N'{1}'
                                DECLARE @DEALER_CODE NVARCHAR(50) = N'{2}'
                                DECLARE @BRANCH_NAME NVARCHAR(150) = N'{3}'
                                DECLARE @BRANCH_CODE NVARCHAR(50) = N'{4}'
                                DECLARE @DEALER_ADDRESS NVARCHAR(200) = N'{5}'
                                DECLARE @DEALER_OFFICE_HOURS NVARCHAR(250) = N'{6}'
                                DECLARE @DEALER_OFFICE_HOURS2 NVARCHAR(250) = N'{7}'
                                DECLARE @DEALER_MOBILE NVARCHAR(50) = N'{8}'
                                DECLARE @ACTIVE BIT = {9}
                                DECLARE @USER NVARCHAR(50) = N'{10}'
                                DECLARE @ID NVARCHAR(20) = N'{11}'
                                DECLARE @LAT NVARCHAR(50) = N'{12}'
                                DECLARE @LNG NVARCHAR(50) = N'{13}'
                                DECLARE @DEALER_NAME_TH NVARCHAR(150) = N'{14}'
                                DECLARE @BRANCH_NAME_TH NVARCHAR(150) = N'{15}'
                                DECLARE @DEALER_ADDRESS_TH NVARCHAR(200) = N'{16}'
                                DECLARE @DEALER_OFFICE_HOURS_TH NVARCHAR(250) = N'{17}'
                                DECLARE @DEALER_OFFICE_HOURS2_TH NVARCHAR(250) = N'{18}'
                                DECLARE @DURATION_MIN INT = N'{19}'
                                DECLARE @DURATION_MAX INT = N'{20}'
                                DECLARE @PIN_MAP_ICON_ID  INT = N'{21}'
                                DECLARE @IS_PICKUP_SERVICE  BIT = N'{22}'
                                DECLARE @PROVINCE_ID INT = N'{23}'

                                UPDATE [T_DEALER]
                                SET		GEO_ID = CASE LEN(@GEO_ID) WHEN 0 THEN NULL ELSE @GEO_ID END,
                                		DEALER_NAME = CASE LEN(@DEALER_NAME) WHEN 0 THEN NULL ELSE @DEALER_NAME END,
                                		DEALER_CODE = CASE LEN(@DEALER_CODE) WHEN 0 THEN NULL ELSE @DEALER_CODE END,
                                		BRANCH_NAME = CASE LEN(@BRANCH_NAME) WHEN 0 THEN NULL ELSE @BRANCH_NAME END,
                                		BRANCH_CODE = CASE LEN(@BRANCH_CODE) WHEN 0 THEN NULL ELSE @BRANCH_CODE END,
                                		DEALER_ADDRESS = CASE LEN(@DEALER_ADDRESS) WHEN 0 THEN NULL ELSE @DEALER_ADDRESS END,
                                		DEALER_OFFICE_HOURS = CASE LEN(@DEALER_OFFICE_HOURS) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS END,
                                        DEALER_OFFICE_HOURS2 = CASE LEN(@DEALER_OFFICE_HOURS2) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS2 END,
                                		DEALER_MOBILE = CASE LEN(@DEALER_MOBILE) WHEN 0 THEN NULL ELSE @DEALER_MOBILE END,
                                		ACTIVE = CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
                                		UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
                                		UPDATE_USER = @USER,
                                        latitude = CASE LEN(@LAT) WHEN 0 THEN NULL ELSE @LAT END,
                                        longitude = CASE LEN(@LNG) WHEN 0 THEN NULL ELSE @LNG END,
                                        dealer_name_th = CASE LEN(@DEALER_NAME_TH) WHEN 0 THEN NULL ELSE @DEALER_NAME_TH END,
                                        branch_name_th = CASE LEN(@BRANCH_NAME_TH) WHEN 0 THEN NULL ELSE @BRANCH_NAME_TH END,
                                        dealer_address_th = CASE LEN(@DEALER_ADDRESS_TH) WHEN 0 THEN NULL ELSE @DEALER_ADDRESS_TH END,
                                		DEALER_OFFICE_HOURS_TH = CASE LEN(@DEALER_OFFICE_HOURS_TH) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS_TH END,
                                        DEALER_OFFICE_HOURS2_TH = CASE LEN(@DEALER_OFFICE_HOURS2_TH) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS2_TH END,
                                        DURATION_MIN = CASE LEN(@DURATION_MIN) WHEN 0 THEN NULL ELSE @DURATION_MIN END,
                                        DURATION_MAX = CASE LEN(@DURATION_MAX) WHEN 0 THEN NULL ELSE @DURATION_MAX END,
                                        PIN_MAP_ICON_ID = CASE LEN(@PIN_MAP_ICON_ID) WHEN 0 THEN NULL ELSE @PIN_MAP_ICON_ID END,
                                        IS_PICKUP_SERVICE = CASE LEN(@IS_PICKUP_SERVICE) WHEN 0 THEN 0 ELSE @IS_PICKUP_SERVICE END,
                                        province_id = @PROVINCE_ID

                                WHERE	DEALER_ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(geoId),
                        WebUtility.GetSQLTextValue(dealerName),
                        WebUtility.GetSQLTextValue(dealerCode),
                        WebUtility.GetSQLTextValue(branchName),
                        WebUtility.GetSQLTextValue(branchCode),
                        WebUtility.GetSQLTextValue(address),
                        WebUtility.GetSQLTextValue(businessHours),
                        WebUtility.GetSQLTextValue(businessHours2),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(lat),
                        WebUtility.GetSQLTextValue(lng),
                        WebUtility.GetSQLTextValue(dealer_name_th),
                        WebUtility.GetSQLTextValue(branch_name_th),
                        WebUtility.GetSQLTextValue(address_th),
                        WebUtility.GetSQLTextValue(office_hours_th),
                        WebUtility.GetSQLTextValue(office_hours2_th),
                        WebUtility.GetSQLTextValue(dealer_duration_min),
                        WebUtility.GetSQLTextValue(dealer_duration_max),
                        WebUtility.GetSQLTextValue(pin_map_icon_id),
                        WebUtility.GetSQLTextValue(is_pickup_service),
                        WebUtility.GetSQLTextValue(province_id)));
                }

                if (appointMent != null && testDrive != null)
                {
                    //if AppointMentService is null  will add  else is Update
                    DataRow drAppointMent = GetWorkingTimeByIdandService(appointMent.Dealer_ID, appointMent.Service_Type);
                    if (drAppointMent == null)
                    {
                        AddWorkingTime(appointMent, appointMent.Dealer_ID, user);
                    }
                    else
                    {
                        UpdateWorkingTime(appointMent, user);
                    }
                }
                if (appointMent != null && testDrive != null)
                {
                    //if TestDrive is null  will add  else is Update
                    DataRow drTestDrive = GetWorkingTimeByIdandService(testDrive.Dealer_ID, testDrive.Service_Type);
                    if (drTestDrive == null)
                    {
                        AddWorkingTime(testDrive, testDrive.Dealer_ID, user);
                    }
                    else
                    {
                        UpdateWorkingTime(testDrive, user);
                    }
                }
                if (holidays != null)
                {
                    RenewHolliday(holidays, id, user);
                }
                if (booking != null)
                {
                    //if AppointMentService is null  will add  else is Update
                    DataRow drBooking = GetWorkingTimeByIdandService(booking.Dealer_ID, booking.Service_Type);
                    if (drBooking == null)
                    {
                        AddWorkingTime(booking, booking.Dealer_ID, user);
                    }
                    else
                    {
                        UpdateWorkingTime(booking, user);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddDealerService(string geoId, string dealerName, string dealerCode, string branchName, string branchCode, string address, string businessHours, string businessHours2, string mobile, string active, string user, string lat, string lng, string dealer_name_th, string branch_name_th, string address_th, string office_hours_th, string office_hours2_th
           , string dealer_duration_min, string dealer_duration_max, string pin_map_icon_id, string is_pickup_service, DealerWorkingTimeModel appointMent, DealerWorkingTimeModel testDrive, List<HolidayDateModel> holidays, DealerWorkingTimeModel booking,string province_id)
        {
            try
            {
                string cmd = @"
DECLARE @GEO_ID INT = N'{0}'
DECLARE @DEALER_NAME NVARCHAR(150) = N'{1}'
DECLARE @DEALER_CODE NVARCHAR(50) = N'{2}'
DECLARE @BRANCH_NAME NVARCHAR(150) = N'{3}'
DECLARE @BRANCH_CODE NVARCHAR(50) = N'{4}'
DECLARE @DEALER_ADDRESS NVARCHAR(200) = N'{5}'
DECLARE @DEALER_OFFICE_HOURS NVARCHAR(250) = N'{6}'
DECLARE @DEALER_OFFICE_HOURS2 NVARCHAR(250) = N'{7}'
DECLARE @DEALER_MOBILE NVARCHAR(50) = N'{8}'
DECLARE @ACTIVE BIT = N'{9}'
DECLARE @USER NVARCHAR(50) = N'{10}'
DECLARE @LAT NVARCHAR(50) = N'{11}'
DECLARE @LNG NVARCHAR(50) = N'{12}'
DECLARE @DEALER_NAME_TH NVARCHAR(150) = N'{13}'
DECLARE @BRANCH_NAME_TH NVARCHAR(150) = N'{14}'
DECLARE @DEALER_ADDRESS_TH NVARCHAR(200) = N'{15}'
DECLARE @DEALER_OFFICE_HOURS_TH NVARCHAR(250) = N'{16}'
DECLARE @DEALER_OFFICE_HOURS2_TH NVARCHAR(250) = N'{17}'
DECLARE @DURATION_MIN INT = N'{18}'
DECLARE @DURATION_MAX INT = N'{19}'
DECLARE @PIN_MAP_ICON_ID  INT = N'{20}'
DECLARE @IS_PICKUP_SERVICE  BIT = N'{21}'
DECLARE @PROVINCE_ID INT = N'{22}'

INSERT INTO [T_DEALER] ([GEO_ID],[DEALER_NAME],[DEALER_CODE],[BRANCH_NAME],[BRANCH_CODE],[DEALER_ADDRESS],[DEALER_OFFICE_HOURS],[DEALER_OFFICE_HOURS2],
[DEALER_MOBILE],[ACTIVE],[CREATE_DT],[CREATE_USER],type,latitude,longitude,dealer_name_th,branch_name_th,dealer_address_th,DEALER_OFFICE_HOURS_TH,
DEALER_OFFICE_HOURS2_TH,[DURATION_MIN],[DURATION_MAX],[pin_map_icon_id],[is_pickup_service],[province_id])
VALUES (CASE LEN(@GEO_ID) WHEN 0 THEN NULL ELSE @GEO_ID END,
		CASE LEN(@DEALER_NAME) WHEN 0 THEN NULL ELSE @DEALER_NAME END,
		CASE LEN(@DEALER_CODE) WHEN 0 THEN NULL ELSE @DEALER_CODE END,
		CASE LEN(@BRANCH_NAME) WHEN 0 THEN NULL ELSE @BRANCH_NAME END,
		CASE LEN(@BRANCH_CODE) WHEN 0 THEN NULL ELSE @BRANCH_CODE END,
		CASE LEN(@DEALER_ADDRESS) WHEN 0 THEN NULL ELSE @DEALER_ADDRESS END,
		CASE LEN(@DEALER_OFFICE_HOURS) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS END,
        CASE LEN(@DEALER_OFFICE_HOURS2) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS2 END,
		CASE LEN(@DEALER_MOBILE) WHEN 0 THEN NULL ELSE @DEALER_MOBILE END,
		CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
		DATEADD(HOUR, 7, GETDATE()),
		@USER,
        'SERVICE_CORNER',
        @LAT,
        @LNG,
        CASE LEN(@DEALER_NAME_TH) WHEN 0 THEN NULL ELSE @DEALER_NAME_TH END,
        CASE LEN(@BRANCH_NAME_TH) WHEN 0 THEN NULL ELSE @BRANCH_NAME_TH END,
        CASE LEN(@DEALER_ADDRESS_TH) WHEN 0 THEN NULL ELSE @DEALER_ADDRESS_TH END,
        CASE LEN(@DEALER_OFFICE_HOURS_TH) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS_TH END,
        CASE LEN(@DEALER_OFFICE_HOURS2_TH) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS2_TH END,
        CASE LEN(@DURATION_MIN) WHEN 0 THEN NULL ELSE @DURATION_MIN END,
        CASE LEN(@DURATION_MAX) WHEN 0 THEN NULL ELSE @DURATION_MAX END,
        CASE LEN(@PIN_MAP_ICON_ID) WHEN 0 THEN NULL ELSE @PIN_MAP_ICON_ID END,
        CASE LEN(@IS_PICKUP_SERVICE) WHEN 0 THEN 0 ELSE @IS_PICKUP_SERVICE END,
        @PROVINCE_ID)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(geoId),
                        WebUtility.GetSQLTextValue(dealerName),
                        WebUtility.GetSQLTextValue(dealerCode),
                        WebUtility.GetSQLTextValue(branchName),
                        WebUtility.GetSQLTextValue(branchCode),
                        WebUtility.GetSQLTextValue(address),
                        WebUtility.GetSQLTextValue(businessHours),
                        WebUtility.GetSQLTextValue(businessHours2),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(lat),
                        WebUtility.GetSQLTextValue(lng),
                        WebUtility.GetSQLTextValue(dealer_name_th),
                        WebUtility.GetSQLTextValue(branch_name_th),
                        WebUtility.GetSQLTextValue(address_th),
                        WebUtility.GetSQLTextValue(office_hours_th),
                        WebUtility.GetSQLTextValue(office_hours2_th),
                        WebUtility.GetSQLTextValue(dealer_duration_min),
                        WebUtility.GetSQLTextValue(dealer_duration_max),
                        WebUtility.GetSQLTextValue(pin_map_icon_id),
                        WebUtility.GetSQLTextValue(is_pickup_service),
                        WebUtility.GetSQLTextValue(province_id)
                        ));
                }

                DataRow drDealer = GetDealerByCode(dealerCode);
                if (drDealer != null)
                {
                    if (!DBNull.Value.Equals(drDealer["ID"]))
                    {
                        string id = drDealer["ID"].ToString();
                        if (appointMent != null)
                        {
                            AddWorkingTime(appointMent, id, user);

                        }
                        if (testDrive != null)
                        {
                            AddWorkingTime(testDrive, id, user);
                        }
                        if (holidays != null)
                        {
                            RenewHolliday(holidays, id, user);
                        }
                        if (booking != null)
                        {
                            AddWorkingTime(booking, id, user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDealerService(string geoId, string dealerName, string dealerCode, string branchName, string branchCode, string address, string businessHours, string businessHours2, string mobile, string active, string user, string id, string lat, string lng, string dealer_name_th, string branch_name_th, string address_th, string office_hours_th, string office_hours2_th
           , string dealer_duration_min, string dealer_duration_max, string pin_map_icon_id, string is_pickup_service, DealerWorkingTimeModel appointMent, DealerWorkingTimeModel testDrive, List<HolidayDateModel> holidays, DealerWorkingTimeModel booking, string province_id)


        {
            try
            {
                string cmd = @"
DECLARE @GEO_ID INT = N'{0}'
DECLARE @DEALER_NAME NVARCHAR(150) = N'{1}'
DECLARE @DEALER_CODE NVARCHAR(50) = N'{2}'
DECLARE @BRANCH_NAME NVARCHAR(150) = N'{3}'
DECLARE @BRANCH_CODE NVARCHAR(50) = N'{4}'
DECLARE @DEALER_ADDRESS NVARCHAR(200) = N'{5}'
DECLARE @DEALER_OFFICE_HOURS NVARCHAR(250) = N'{6}'
DECLARE @DEALER_OFFICE_HOURS2 NVARCHAR(250) = N'{7}'
DECLARE @DEALER_MOBILE NVARCHAR(50) = N'{8}'
DECLARE @ACTIVE BIT = {9}
DECLARE @USER NVARCHAR(50) = N'{10}'
DECLARE @ID NVARCHAR(20) = N'{11}'
DECLARE @LAT NVARCHAR(50) = N'{12}'
DECLARE @LNG NVARCHAR(50) = N'{13}'
DECLARE @DEALER_NAME_TH NVARCHAR(150) = N'{14}'
DECLARE @BRANCH_NAME_TH NVARCHAR(150) = N'{15}'
DECLARE @DEALER_ADDRESS_TH NVARCHAR(200) = N'{16}'
DECLARE @DEALER_OFFICE_HOURS_TH NVARCHAR(250) = N'{17}'
DECLARE @DEALER_OFFICE_HOURS2_TH NVARCHAR(250) = N'{18}'
DECLARE @DURATION_MIN INT = N'{19}'
DECLARE @DURATION_MAX INT = N'{20}'
DECLARE @PIN_MAP_ICON_ID  INT = N'{21}'
DECLARE @IS_PICKUP_SERVICE  BIT = N'{22}'
DECLARE @PROVINCE_ID INT = N'{23}'

UPDATE [T_DEALER]
SET		GEO_ID = CASE LEN(@GEO_ID) WHEN 0 THEN NULL ELSE @GEO_ID END,
		DEALER_NAME = CASE LEN(@DEALER_NAME) WHEN 0 THEN NULL ELSE @DEALER_NAME END,
		DEALER_CODE = CASE LEN(@DEALER_CODE) WHEN 0 THEN NULL ELSE @DEALER_CODE END,
		BRANCH_NAME = CASE LEN(@BRANCH_NAME) WHEN 0 THEN NULL ELSE @BRANCH_NAME END,
		BRANCH_CODE = CASE LEN(@BRANCH_CODE) WHEN 0 THEN NULL ELSE @BRANCH_CODE END,
		DEALER_ADDRESS = CASE LEN(@DEALER_ADDRESS) WHEN 0 THEN NULL ELSE @DEALER_ADDRESS END,
		DEALER_OFFICE_HOURS = CASE LEN(@DEALER_OFFICE_HOURS) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS END,
        DEALER_OFFICE_HOURS2 = CASE LEN(@DEALER_OFFICE_HOURS2) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS2 END,
		DEALER_MOBILE = CASE LEN(@DEALER_MOBILE) WHEN 0 THEN NULL ELSE @DEALER_MOBILE END,
		ACTIVE = CASE LEN(@ACTIVE) WHEN 0 THEN NULL ELSE @ACTIVE END,
		UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
		UPDATE_USER = @USER,
        latitude = CASE LEN(@LAT) WHEN 0 THEN NULL ELSE @LAT END,
        longitude = CASE LEN(@LNG) WHEN 0 THEN NULL ELSE @LNG END,
        dealer_name_th = CASE LEN(@DEALER_NAME_TH) WHEN 0 THEN NULL ELSE @DEALER_NAME_TH END,
        branch_name_th = CASE LEN(@BRANCH_NAME_TH) WHEN 0 THEN NULL ELSE @BRANCH_NAME_TH END,
        dealer_address_th = CASE LEN(@DEALER_ADDRESS_TH) WHEN 0 THEN NULL ELSE @DEALER_ADDRESS_TH END,
		DEALER_OFFICE_HOURS_TH = CASE LEN(@DEALER_OFFICE_HOURS_TH) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS_TH END,
        DEALER_OFFICE_HOURS2_TH = CASE LEN(@DEALER_OFFICE_HOURS2_TH) WHEN 0 THEN NULL ELSE @DEALER_OFFICE_HOURS2_TH END,
        DURATION_MIN = CASE LEN(@DURATION_MIN) WHEN 0 THEN NULL ELSE @DURATION_MIN END,
        DURATION_MAX = CASE LEN(@DURATION_MAX) WHEN 0 THEN NULL ELSE @DURATION_MAX END,
        PIN_MAP_ICON_ID = CASE LEN(@PIN_MAP_ICON_ID) WHEN 0 THEN NULL ELSE @PIN_MAP_ICON_ID END,
        IS_PICKUP_SERVICE = CASE LEN(@IS_PICKUP_SERVICE) WHEN 0 THEN 0 ELSE @IS_PICKUP_SERVICE END,
        province_id = @PROVINCE_ID

WHERE	DEALER_ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(geoId),
                        WebUtility.GetSQLTextValue(dealerName),
                        WebUtility.GetSQLTextValue(dealerCode),
                        WebUtility.GetSQLTextValue(branchName),
                        WebUtility.GetSQLTextValue(branchCode),
                        WebUtility.GetSQLTextValue(address),
                        WebUtility.GetSQLTextValue(businessHours),
                        WebUtility.GetSQLTextValue(businessHours2),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(active),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(lat),
                        WebUtility.GetSQLTextValue(lng),
                        WebUtility.GetSQLTextValue(dealer_name_th),
                        WebUtility.GetSQLTextValue(branch_name_th),
                        WebUtility.GetSQLTextValue(address_th),
                        WebUtility.GetSQLTextValue(office_hours_th),
                        WebUtility.GetSQLTextValue(office_hours2_th),
                        WebUtility.GetSQLTextValue(dealer_duration_min),
                        WebUtility.GetSQLTextValue(dealer_duration_max),
                        WebUtility.GetSQLTextValue(pin_map_icon_id),
                        WebUtility.GetSQLTextValue(is_pickup_service),
                        WebUtility.GetSQLTextValue(province_id)
                        ));
                }
                if (appointMent != null && testDrive != null)
                {
                    //if AppointMentService is null  will add  else is Update
                    DataRow drAppointMent = GetWorkingTimeByIdandService(appointMent.Dealer_ID, appointMent.Service_Type);
                    if (drAppointMent == null)
                    {
                        AddWorkingTime(appointMent, appointMent.Dealer_ID, user);
                    }
                    else
                    {
                        UpdateWorkingTime(appointMent, user);
                    }
                }
                if (appointMent != null && testDrive != null)
                {
                    //if TestDrive is null  will add  else is Update
                    DataRow drTestDrive = GetWorkingTimeByIdandService(testDrive.Dealer_ID, testDrive.Service_Type);
                    if (drTestDrive == null)
                    {
                        AddWorkingTime(testDrive, testDrive.Dealer_ID, user);
                    }
                    else
                    {
                        UpdateWorkingTime(testDrive, user);
                    }
                }
                if (holidays != null)
                {
                    RenewHolliday(holidays, id, user);
                }
                if (booking != null)
                {
                    //if AppointMentService is null  will add  else is Update
                    DataRow drBooking = GetWorkingTimeByIdandService(booking.Dealer_ID, booking.Service_Type);
                    if (drBooking == null)
                    {
                        AddWorkingTime(booking, booking.Dealer_ID, user);
                    }
                    else
                    {
                        UpdateWorkingTime(booking, user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteDealer(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	T_DEALER
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	DEALER_ID = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd,
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetDealerById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
SELECT		[DEALER_ID] AS [ID]
            ,[DEALER_CODE]
			,[DEALER_NAME]
            ,[BRANCH_CODE]
			,[BRANCH_NAME]
			,[DEALER_ADDRESS]
			,[DEALER_MOBILE]
			,[DEALER_OFFICE_HOURS]
            ,[DEALER_OFFICE_HOURS2]
			,[ACTIVE]
            ,[GEO_ID]
            ,[latitude]
            ,[longitude]
            ,dealer_name_th
            ,dealer_address_th
            ,branch_name_th
            ,DEALER_OFFICE_HOURS_TH
            ,DEALER_OFFICE_HOURS2_TH
            ,[DURATION_MIN]
            ,[DURATION_MAX]
            ,[is_pickup_service]
            ,[pin_map_icon_id]
            , province_id
FROM		[T_DEALER]
WHERE		[DEALER_ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id))))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            row = r;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        public DataRow GetDealerByCode(string code)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @DEALER_CODE NVARCHAR(50) = N'{0}'
SELECT		[DEALER_ID] AS [ID]
            ,[DEALER_CODE]
			,[DEALER_NAME]
            ,[BRANCH_CODE]
			,[BRANCH_NAME]
			,[DEALER_ADDRESS]
			,[DEALER_MOBILE]
			,[DEALER_OFFICE_HOURS]
            ,[DEALER_OFFICE_HOURS2]
			,[ACTIVE]
            ,[GEO_ID]
            ,[latitude]
            ,[longitude]
            ,dealer_name_th
            ,dealer_address_th
            ,branch_name_th
            ,DEALER_OFFICE_HOURS_TH
            ,DEALER_OFFICE_HOURS2_TH
FROM		[T_DEALER]
WHERE		[DEALER_CODE] = @DEALER_CODE";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(code))))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            row = r;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public DataRow GetWorkingTimeByIdandService(string id, string serviceType)
        {
            DataRow row = null;
            try
            {


                string cmd = @"
                                DECLARE @ID            INT = N'{0}'
                                DECLARE @ServiceType   nvarchar(50) = N'{1}'
                                            SELECT		
                                            [Dealer_ID],
                                            [Service_Type],
                                            [CallCenterMobile],
                                            [CallCenterEmail],
                                            [Is_Monday],
                                            [Mon_StartTime],
                                            [Mon_EndTime],
                                            [Is_Tuesday],
                                            [Tues_StartTime],
                                            [Tues_EndTime],
                                            [Is_Wednesday],
                                            [Wed_StartTime],
                                            [Wed_EndTime],
                                            [Is_Thursday],
                                            [Thur_StartTime],
                                            [Thur_EndTime],
                                            [Is_Friday],
                                            [Fri_StartTime],
                                            [Fri_EndTime],
                                            [Is_Satday],
                                            [Sat_StartTime],
                                            [Sat_EndTime],
                                            [Is_Sunday],
                                            [Sun_StartTime],
                                            [Sun_EndTime],
                                            [Created_Date],
                                            [Created_By],
                                            [Updated_Date],
                                            [Updated_By] 
                                            FROM		[T_Dealer_WorkingTime]
                                            WHERE		[DEALER_ID] = @ID  AND Service_Type = @ServiceType";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id), WebUtility.GetSQLTextValue(serviceType))))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            row = r;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public DataTable GetHolidayByDealerId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                DECLARE @ID            INT = N'{0}'                                
                                            SELECT		
                                                [Holiday_ID],
                                                [Dealer_ID],
                                                [Holiday_Date],
                                                [Created_Date],
                                                [Created_By],
                                                [Updated_Date],
                                                [Updated_By]                                                
                                            FROM		[T_Dealer_Holiday]
                                            WHERE		[DEALER_ID] = @ID ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id)));

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