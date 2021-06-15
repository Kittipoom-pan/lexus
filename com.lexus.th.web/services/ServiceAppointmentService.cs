using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;
using System.Data.SqlClient;

namespace com.lexus.th.web
{
    public class ServiceAppointmentService
    {
        private string conn;
        public ServiceAppointmentService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetServiceAppointment(ServiceAppointmentCriteria criteria)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
                               
                                
                                
                             SELECT DISTINCT * FROM(   SELECT   A.[ID]
                                        ,A.[CODE]    
                                        ,A.[CREATED_DATE]
                                        ,A.[MEMBER_ID]
                                        ,A.[NAME]
                                        ,A.[SURNAME]
                                        ,A.[MOBILE_NUMBER]
                                        ,A.[PLATE_NUMBER]
                                        ,TCM.[MODEL] AS MODEL_NAME   
                          ,(SELECT    
    STUFF
    (
        (
                       SELECT ',' + t.name_en 
			+ REPLACE(CONCAT('(',
			   STUFF
    (
        (
            SELECT ',' + d.name_en
            FROM type_of_service_detail d			
			WHERE ap.appointment_id =A.id AND d.type_of_service_id = ap.type_of_service_id AND d.id=ap.type_of_service_detail_id
            FOR XML PATH('')
        ), 1, 1, ''
    ) ,')'),'()','')
            FROM appointment_type_of_service ap
			LEFT JOIN type_of_service t  on t.id= ap.type_of_service_id
			WHERE ap.appointment_id =A.id
            ORDER BY t.id
            FOR XML PATH('')
        ), 1, 1, ''
    )) AS Type_Of_Service                                    
                                        ,D.[DEALER_NAME]
                                        ,W.CallCenterMobile AS [DEALER_MOBILE]
                                        ,A.[APPOINTMENTDATE]
                                        ,ud.[START_HOUR] as APPOINTMENTTIME
                                        ,A.[UPDATED_DATE]
                                        ,A.[CONFIRM_DATE]
                                        ,A.[CONFIRM_TIME]
                                        ,A.[CANCEL_DATE]
                                        ,A.[CANCEL_REASON]             
                                        ,(CASE WHEN A.[status_id]=1 THEN 'Waiting to confirm' ELSE 
                                          CASE WHEN A.[status_id]=2 THEN 'Confirm' ELSE 
                                          CASE WHEN A.[status_id]=3 THEN 'Cancel' ELSE 
										  CASE WHEN A.[status_id]=4 THEN 'Complete' ELSE '' END END END END) AS STATUS_NAME
                                        ,A.[is_pickup_service] 
                                        ,LTRIM(STR(A.[Latitude], 25, 5))+','+LTRIM(STR(A.[Longitude], 25, 5)) as pickup_location                                       
                                        ,CONVERT(NVARCHAR(MAX),A.[pickup_address]) as [pickup_address]
										,A.[location_detail] as pickup_location_detail
										,A.[pickup_date]										
										,ud2.[start_hour] as pickup_time
                              
            
                                      
                                  FROM [DBO].[APPOINTMENT] A
LEFT JOIN [APPOINTMENT_TYPE_OF_SERVICE] ap on ap.appointment_id = A.id
                                  LEFT JOIN [T_DEALER] D ON D.[DEALER_ID] = A.[DEALER_ID]    
                                  LEFT JOIN [T_DEALER_WORKINGTIME ] W ON W.Dealer_id = A.dealer_id and W.Service_Type = 'ServiceAppointment'                              
                                  LEFT JOIN [T_CUSTOMER_CAR] TC ON TC.[VIN] = A.[vehicle_no]  
								  LEFT JOIN [T_CAR_MODEL] TCM ON TC.[MODEL_ID] = TCM.[MODEL_ID] 
                                  LEFT JOIN [utility_generate_time_of_day] ud ON ud.id = A.[APPOINTMENTTIME_ID]
                                  LEFT JOIN [utility_generate_time_of_day] ud2 ON ud2.id = A.[pickup_time_id]
                                  WHERE A.[DELETED_FLAG] IS NULL 
                                        AND ( 1=1  ";


                if (!string.IsNullOrEmpty(criteria.DealerId) && criteria.DealerId != "0")
                {
                    cmd += string.Format(" AND A.[DEALER_ID] = {0}", criteria.DealerId);
                }
                if (!string.IsNullOrEmpty(criteria.TypeOfServiceId) && criteria.TypeOfServiceId != "0")
                {
                    cmd += string.Format(" AND AP.[TYPE_OF_SERVICE_ID] = N'{0}'", criteria.TypeOfServiceId);
                }
                if (!string.IsNullOrEmpty(criteria.AppointmentCode))
                {
                    cmd += string.Format(" AND (A.[CODE]  LIKE N'%{0}%')", criteria.AppointmentCode);
                }
                if (!string.IsNullOrEmpty(criteria.Name))
                {
                    cmd += string.Format(" AND (A.[NAME]  LIKE N'%{0}%')", criteria.Name);
                }
                if (!string.IsNullOrEmpty(criteria.Surname))
                {
                    cmd += string.Format(" AND (A.[SURNAME]  LIKE N'%{0}%')", criteria.Surname);
                }
                if (!string.IsNullOrEmpty(criteria.MobileNumber))
                {
                    cmd += string.Format(" AND (A.[MOBILE_NUMBER]  LIKE N'%{0}%')", criteria.MobileNumber);
                }
                if (!string.IsNullOrEmpty(criteria.Status) && criteria.Status != "-1")
                {
                    cmd += string.Format(" AND A.[STATUS_ID] = N'{0}'", criteria.Status);
                }
                if (!string.IsNullOrEmpty(criteria.RegisDateFrom))
                {
                    var date = criteria.RegisDateFrom;
                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, A.[CREATED_DATE], 120), 120)  >= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.RegisDateTo))
                {
                    var date = criteria.RegisDateTo;

                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, A.[CREATED_DATE], 120), 120)  <= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.AppointmentDateFrom))
                {
                    var date = criteria.AppointmentDateFrom;
                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, A.[APPOINTMENTDATE], 120), 120)  >= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.AppointmentDateTo))
                {
                    var date = criteria.AppointmentDateTo;

                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, A.[APPOINTMENTDATE], 120), 120)  <= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.IsPickupService) && criteria.IsPickupService != "-1")
                {
                    cmd += string.Format(" AND A.[Is_Pickup_Service] = N'{0}'", criteria.IsPickupService);
                }

                cmd += @"  ) )AS APPOINT ORDER BY CREATED_DATE";



                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd);

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetServiceAppointmentExport(ServiceAppointmentCriteria criteria)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
                               
                                
                                
                           SELECT DISTINCT * FROM ( SELECT  ROW_NUMBER() OVER(ORDER BY A.CREATED_DATE) as [No.] 
                                        ,A.[CODE] as [Appointment Code]   
                                        ,A.[CREATED_DATE] as [Register Date]
                                        ,A.[MEMBER_ID] as [MemberID]
                                        ,A.[NAME] as [First Name]
                                        ,A.[SURNAME] as [Surname]
                                        ,A.[MOBILE_NUMBER] as [Mobile]
                                        ,A.[PLATE_NUMBER] as [Plate no]
                                        ,TCM.[MODEL] AS [Car Model] 
 ,(SELECT    
    STUFF
    (
        (
                       SELECT ',' + t.name_th 
			+ REPLACE(CONCAT('(',
			   STUFF
    (
        (
            SELECT ',' + d.name_th
            FROM type_of_service_detail d			
			WHERE ap.appointment_id =A.id AND d.type_of_service_id = ap.type_of_service_id AND d.id=ap.type_of_service_detail_id
            FOR XML PATH('')
        ), 1, 1, ''
    ) ,')'),'()','')
            FROM appointment_type_of_service ap
			LEFT JOIN type_of_service t  on t.id= ap.type_of_service_id
			WHERE ap.appointment_id =A.id
            ORDER BY t.id
            FOR XML PATH('')
        ), 1, 1, ''
    )) AS Type_Of_Service                                      
                                        ,A.[REMARK] as [Remark]
                                        ,D.[DEALER_NAME] as [Dealer Name]
                                        ,W.CallCenterMobile AS  [Dealer Mobile]
                                        ,A.[APPOINTMENTDATE] as [Appointment Date]   
                                        ,ud.[START_HOUR] as [Appointment Time]       
                                        ,A.[UPDATED_DATE] as [Update Date]
                                        ,A.[CALL_CENTER_REMARK] as [Call Center Remark]
                                        ,A.[CONFIRM_DATE] as [Confirm Date]
                                        ,A.[CONFIRM_TIME] as [Confirm Time]
                                        ,A.[CANCEL_DATE] as [CancelDate]
                                        ,A.[CANCEL_REASON] as [Cancel Detail]        
                                        ,(CASE WHEN A.[status_id]=1 THEN 'Waiting to confirm' ELSE 
                                          CASE WHEN A.[status_id]=2 THEN 'Confirm' ELSE 
                                          CASE WHEN A.[status_id]=3 THEN 'Cancel' ELSE 
										  CASE WHEN A.[status_id]=4 THEN 'Complete' ELSE '' END END END END) AS [Status]
                                         ,A.[is_pickup_service] as [Pickup Service]
                                        ,LTRIM(STR(A.[Latitude], 25, 5))+','+LTRIM(STR(A.[Longitude], 25, 5)) as [Pickup Location]                                       
                                        ,CONVERT(NVARCHAR(MAX),A.[pickup_address]) as [Pickup Address]
										,A.[location_detail] as [Pickup Location Detail]
										,A.[pickup_date] as [Pickup Date]										
										,ud2.[start_hour] as [Pickup Time]
                                      
                                  FROM [DBO].[APPOINTMENT] A
LEFT JOIN [APPOINTMENT_TYPE_OF_SERVICE] ap on ap.appointment_id = A.id
                                  LEFT JOIN [T_DEALER] D ON D.[DEALER_ID] = A.[DEALER_ID]    
                                  LEFT JOIN [T_DEALER_WORKINGTIME ] W ON W.Dealer_id = A.dealer_id and W.Service_Type = 'ServiceAppointment'                                    
                                  LEFT JOIN [T_CUSTOMER_CAR] TC ON TC.[VIN] = A.[vehicle_no]  
								  LEFT JOIN [T_CAR_MODEL] TCM ON TC.[MODEL_ID] = TCM.[MODEL_ID] 
                                  LEFT JOIN [utility_generate_time_of_day] ud ON ud.id = A.[APPOINTMENTTIME_ID]
                                  LEFT JOIN [utility_generate_time_of_day] ud2 ON ud2.id = A.[pickup_time_id]
                                  WHERE A.[DELETED_FLAG] IS NULL 
                                        AND ( 1=1  ";


                if (!string.IsNullOrEmpty(criteria.DealerId) && criteria.DealerId != "0")
                {
                    cmd += string.Format(" AND A.[DEALER_ID] = {0}", criteria.DealerId);
                }
                if (!string.IsNullOrEmpty(criteria.TypeOfServiceId) && criteria.TypeOfServiceId != "0")
                {
                    cmd += string.Format(" AND A.[TYPE_OF_SERVICE_ID] = {0}", criteria.TypeOfServiceId);
                }
                if (!string.IsNullOrEmpty(criteria.AppointmentCode))
                {
                    cmd += string.Format(" AND (A.[CODE]  LIKE N'%{0}%')", criteria.AppointmentCode);
                }
                if (!string.IsNullOrEmpty(criteria.Name))
                {
                    cmd += string.Format(" AND (A.[NAME]  LIKE N'%{0}%')", criteria.Name);
                }
                if (!string.IsNullOrEmpty(criteria.Surname))
                {
                    cmd += string.Format(" AND (A.[SURNAME]  LIKE N'%{0}%')", criteria.Surname);
                }
                if (!string.IsNullOrEmpty(criteria.MobileNumber))
                {
                    cmd += string.Format(" AND (A.[MOBILE_NUMBER]  LIKE N'%{0}%')", criteria.MobileNumber);
                }
                if (!string.IsNullOrEmpty(criteria.Status) && criteria.Status != "-1")
                {
                    cmd += string.Format(" AND A.[STATUS_ID] = {0}", criteria.Status);
                }
                if (!string.IsNullOrEmpty(criteria.RegisDateFrom))
                {
                    var date = criteria.RegisDateFrom;
                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, A.[CREATED_DATE], 120), 120)  >= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.RegisDateTo))
                {
                    var date = criteria.RegisDateTo;

                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, A.[CREATED_DATE], 120), 120)  <= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.AppointmentDateFrom))
                {
                    var date = criteria.AppointmentDateFrom;
                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, A.[APPOINTMENTDATE], 120), 120)  >= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.AppointmentDateTo))
                {
                    var date = criteria.AppointmentDateTo;

                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, A.[APPOINTMENTDATE], 120), 120)  <= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.IsPickupService) && criteria.IsPickupService != "-1")
                {
                    cmd += string.Format(" AND A.[Is_Pickup_Service] = N'{0}'", criteria.IsPickupService);
                }

                cmd += @"  )) AS APPOINT ORDER BY [Register Date]";



                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd);

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetServiceAppointmentTypeOfServiceByAppointmentId(string appointmentId)
        {
            DataTable dt = null;
            try
            {
                string cmd = @"                               
DECLARE @APPOINTMENT_ID INT = N'{0}'

SELECT [ID]
      ,[APPOINTMENT_ID]
      ,[TYPE_OF_SERVICE_ID]
      ,[TYPE_OF_SERVICE_DETAIL_ID]
      ,[CREATED_DATE]
      ,[CREATED_USER]
      ,[UPDATED_DATE]
      ,[UPDATED_USER]
      ,[DELETED_FLAG]
      ,[DELETE_DATE]
      ,[DELETE_USER]
  FROM [DBO].[APPOINTMENT_TYPE_OF_SERVICE]
  WHERE [DELETED_FLAG] IS NULL AND [APPOINTMENT_ID] = @APPOINTMENT_ID ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, WebUtility.GetSQLTextValue(appointmentId));

                    dt = db.GetDataTableFromCommandText(cmd);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetServiceAppointmentTypeOfServiceByAppointmentIdAndTypeId(string appointmentId, string typeOfServiceId)
        {
            DataRow dr = null;
            try
            {
                string cmd = @"                               
DECLARE @APPOINTMENT_ID INT = N'{0}'
DECLARE @TYPE_OF_SERVICE_ID INT = N'{1}'

SELECT [ID]
      ,[APPOINTMENT_ID]
      ,[TYPE_OF_SERVICE_ID]
      ,[TYPE_OF_SERVICE_DETAIL_ID]
      ,[CREATED_DATE]
      ,[CREATED_USER]
      ,[UPDATED_DATE]
      ,[UPDATED_USER]
      ,[DELETED_FLAG]
      ,[DELETE_DATE]
      ,[DELETE_USER]
  FROM [DBO].[APPOINTMENT_TYPE_OF_SERVICE]
  WHERE [DELETED_FLAG] IS NULL AND [APPOINTMENT_ID] = @APPOINTMENT_ID AND [TYPE_OF_SERVICE_ID] = @TYPE_OF_SERVICE_ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd, WebUtility.GetSQLTextValue(appointmentId), WebUtility.GetSQLTextValue(typeOfServiceId));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            dr = r;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dr;
        }

        public DataRow GetServiceAppointmentById(string id)
        {
            DataRow row = null;
            try
            { // ,A.[MODEL_NAME]
                string cmd = @"
                DECLARE @ID INT = N'{0}'
                
                            SELECT			 
                             A.[ID]
                            ,A.[CODE]
                          
                            ,A.[SERVICE_APPOINTMENT_TYPE]
                            ,A.[NAME]
                            ,A.[SURNAME]
                            ,A.[MOBILE_NUMBER]
                            ,A.[PLATE_NUMBER]
                            ,TCM.[MODEL] AS MODEL_NAME
                                                 
                            ,A.[DEALER_ID]                       
                            ,A.[REMARK]
                            ,A.[CALL_CENTER_REMARK]
                            ,A.[APPOINTMENTDATE]
                            ,UGY.start_hour AS [APPOINTMENTTIME]
                            ,A.[CONFIRM_DATE]
                            ,A.[CONFIRM_TIME]
                            ,A.[IS_CANCEL]
                            ,A.[CANCEL_DATE]
                            ,A.[CANCEL_REASON]
                            ,A.[FOOTER_REMARK_DESCRIPTION]
                            ,A.[CREATED_DATE]
                            ,A.[CREATED_USER]
                            ,A.[UPDATED_DATE]
                            ,A.[UPDATED_USER]
                            ,A.[DELETED_FLAG]
                            ,A.[DELETE_DATE]
                            ,A.[DELETE_USER]
                            ,A.[VEHICLE_NO]
                            ,A.[STATUS_ID]
                            ,A.[MEMBER_ID]
                            ,(CASE WHEN A.[status_id]=0 THEN 'Waiting to confirm' else 
                            CASE WHEN A.[status_id]=1 THEN 'Confirm' else 
                            CASE WHEN A.[status_id]=2 THEN 'Cancel' ELSE '' END END END) AS STATUS_NAME
                            ,[is_pickup_service]
                            ,[latitude]
                            ,[longitude]
                            ,[location_detail]
                            ,[pickup_address]
                            ,[pickup_date]
                            ,[pickup_time_id]
                FROM		[APPOINTMENT] A
	            LEFT JOIN [T_CUSTOMER_CAR] TC ON TC.[PLATE_NO] = A.[PLATE_NUMBER] 
				LEFT JOIN [T_CAR_MODEL] TCM ON TC.[MODEL_ID] = TCM.[MODEL_ID]
                LEFT JOIN utility_generate_time_of_day UGY ON A.APPOINTMENTTIME_ID = UGY.ID
                WHERE		A.[ID] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
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
        public DataRow GetCarModelAndVinByPlateNo(string plate_no)
        {
            DataRow row = null;
            try
            { // ,A.[MODEL_NAME]
                string cmd = @"
                DECLARE @PLATE_NO NVARCHAR(100) = N'{0}'
                
                SELECT	TOP(1) TCM.[MODEL] AS MODEL_NAME
                              ,TC.[VIN] AS VIN
                FROM		 [T_CUSTOMER_CAR] TC 
				LEFT JOIN [T_CAR_MODEL] TCM ON TC.[MODEL_ID] = TCM.[MODEL_ID]
                WHERE	TC.[PLATE_NO]	= @PLATE_NO ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(plate_no));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
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
        public DataTable GetPlateNoByMemberId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
DECLARE @ID NVARCHAR(50) = N'{0}'
DECLARE @TABLE TABLE (MEMBERID NVARCHAR(50), PLATE_NO NVARCHAR(100))

INSERT INTO @TABLE VALUES ('0', 'select')

INSERT INTO @TABLE
SELECT [MEMBERID],[PLATE_NO] FROM [dbo].[T_CUSTOMER_CAR]
WHERE [DEL_FLAG] IS NULL AND [MEMBERID] = @ID 

SELECT * FROM @TABLE";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public void UpdateAppointment(AppointmentModel appointment, List<AppointmentTypeOfServiceDetailModel> appServiceDetails, string user)
        {
            try
            {
                string cmd = @" 
DECLARE @ID                   INT             = N'{0}'
DECLARE @NAME                 NVARCHAR(250)   = N'{1}'
DECLARE @SURNAME              NVARCHAR(250)   = N'{2}'
DECLARE @MOBILE_NUMBER        NVARCHAR(50)    = N'{3}'
DECLARE @PLATE_NUMBER         NVARCHAR(100)   = N'{4}'
DECLARE @DEALER_ID            INT             = N'{5}'
DECLARE @CONFIRM_DATE         NVARCHAR(20)    = N'{6}'
DECLARE @CONFIRM_TIME         NVARCHAR(20)    = N'{7}'
DECLARE @IS_CANCEL            BIT             = N'{8}'
DECLARE @CANCEL_DATE          NVARCHAR(20)    = N'{9}'
DECLARE @CANCEL_REASON        NVARCHAR(255)   = N'{10}'
DECLARE @CALL_CENTER_REMARK   NVARCHAR(MAX)   = N'{11}'
DECLARE @STATUS_ID            INT             = N'{12}'
DECLARE @USER                 NVARCHAR(50)    = N'{13}'
DECLARE @vehicle_no           NVARCHAR(50)    = N'{14}'

DECLARE @latitude                   float             = N'{15}'
DECLARE @longitude                  float             = N'{16}'
DECLARE @pickup_address      NVARCHAR(MAX)             = N'{17}'
DECLARE @location_detail     NVARCHAR(500)     = N'{18}'
DECLARE @pickup_date         NVARCHAR(20)             = N'{19}'
DECLARE @pickup_time_id             INT             = N'{20}'

UPDATE [APPOINTMENT]
SET 
         NAME               = CASE LEN(@NAME) WHEN 0 THEN NULL ELSE @NAME END,
         SURNAME            = CASE LEN(@SURNAME) WHEN 0 THEN NULL ELSE @SURNAME END,
         MOBILE_NUMBER      = CASE LEN(@MOBILE_NUMBER) WHEN 0 THEN NULL ELSE @MOBILE_NUMBER END,
         PLATE_NUMBER       = CASE LEN(@PLATE_NUMBER) WHEN 0 THEN NULL ELSE @PLATE_NUMBER END,
         DEALER_ID          = CASE LEN(@DEALER_ID) WHEN 0 THEN NULL ELSE  @DEALER_ID END,
         CONFIRM_DATE       = CASE LEN(@CONFIRM_DATE) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @CONFIRM_DATE, 103) END,
         CONFIRM_TIME       = CASE LEN(@CONFIRM_TIME) WHEN 0 THEN NULL ELSE @CONFIRM_TIME END,
         IS_CANCEL          = CASE LEN(@IS_CANCEL) WHEN 0 THEN NULL ELSE  @IS_CANCEL END,
         CANCEL_DATE        = CASE LEN(@CANCEL_DATE) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @CANCEL_DATE, 103) END,
         CANCEL_REASON      = CASE LEN(@CANCEL_REASON) WHEN 0 THEN NULL ELSE  @CANCEL_REASON       END,
         CALL_CENTER_REMARK = CASE LEN(@CALL_CENTER_REMARK) WHEN 0 THEN NULL ELSE  @CALL_CENTER_REMARK  END,
         STATUS_ID          = CASE LEN(@STATUS_ID) WHEN 0 THEN NULL ELSE  @STATUS_ID END,
	     UPDATED_DATE       = DATEADD(HOUR, 7, GETDATE()),
	     UPDATED_USER       = @USER,
         vehicle_no          = CASE LEN(@vehicle_no) WHEN 0 THEN NULL ELSE  @vehicle_no END,
         latitude                 = CASE LEN(@latitude) WHEN 0 THEN NULL ELSE  @latitude END,
         longitude                = CASE LEN(@longitude) WHEN 0 THEN NULL ELSE  @longitude END,
         pickup_address           = CASE LEN(@pickup_address) WHEN 0 THEN NULL ELSE  @pickup_address END,
         location_detail   = CASE LEN(@location_detail) WHEN 0 THEN NULL ELSE  @location_detail END,
         pickup_date              = CASE LEN(@pickup_date) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @pickup_date, 103) END,
         pickup_time_id           = CASE LEN(@pickup_time_id) WHEN 0 THEN NULL ELSE  @pickup_time_id END
WHERE		[ID] = @ID  ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    List<string> cmds = new List<string>();
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(appointment.Id),
                        WebUtility.GetSQLTextValue(appointment.Name),
                        WebUtility.GetSQLTextValue(appointment.Surname),
                        WebUtility.GetSQLTextValue(appointment.MobileNumber),
                        WebUtility.GetSQLTextValue(appointment.PlateNo),
                        WebUtility.GetSQLTextValue(appointment.DealerId),
                        WebUtility.GetSQLTextValue(appointment.ConfirmDate),
                        WebUtility.GetSQLTextValue(appointment.ConfirmTime),
                        WebUtility.GetSQLTextValue(appointment.IsCancel),
                        WebUtility.GetSQLTextValue(appointment.CancelDate),
                        WebUtility.GetSQLTextValue(appointment.CancelReason),
                        WebUtility.GetSQLTextValue(appointment.CallCenterRemark),
                        WebUtility.GetSQLTextValue(appointment.StatusId),
                        user,
                        WebUtility.GetSQLTextValue(appointment.VehicleNo),

                        WebUtility.GetSQLTextValue(appointment.PickupLatitude),
                        WebUtility.GetSQLTextValue(appointment.PickupLongitude),
                        WebUtility.GetSQLTextValue(appointment.PickupAddress),
                        WebUtility.GetSQLTextValue(appointment.PickupLocationDetail),
                        WebUtility.GetSQLTextValue(appointment.PickupDate),
                        WebUtility.GetSQLTextValue(appointment.PickupTimeId)

                        );
                    cmds.Add(cmd);

                    if (appServiceDetails != null && appServiceDetails.Any())
                    {
                        string cmdDetail = string.Format(@"
DECLARE @APPOINTMENT_ID INT = N'{0}'
DECLARE @USER                 NVARCHAR(50)       = N'{1}'
DECLARE  @TABLE TABLE (
	[APPOINTMENT_ID] [INT] NOT NULL,
	[TYPE_OF_SERVICE_ID] [INT] NOT NULL,
	[TYPE_OF_SERVICE_DETAIL_ID] [INT] NULL,
	[CREATED_DATE] [DATETIME] NULL,
	[CREATED_USER] [NVARCHAR](50) NULL,
	[UPDATED_DATE] [DATETIME] NULL,
	[UPDATED_USER] [NVARCHAR](50) NULL,
	[DELETED_FLAG] [NVARCHAR](1) NULL,
	[DELETE_DATE] [DATETIME] NULL,
	[DELETE_USER] [NVARCHAR](50) NULL)
", appointment.Id, user);
                        foreach (AppointmentTypeOfServiceDetailModel detail in appServiceDetails)
                        {
                            cmdDetail += string.Format(@"

        INSERT INTO @TABLE VALUES (CASE LEN(N'{0}') WHEN 0 THEN NULL ELSE N'{0}' END,
         CASE LEN(N'{1}') WHEN 0 THEN NULL ELSE N'{1}' END,
         CASE LEN(N'{2}') WHEN 0 THEN NULL ELSE N'{2}' END,NULL,NULL,NULL,NULL,NULL,NULL,NULL)	",
               detail.AppointmentId, detail.TypeOfServiceId, detail.TypeOfServiceDetailId);


                        }
                        cmdDetail += @"
MERGE  [APPOINTMENT_TYPE_OF_SERVICE]  AS TARGET 
USING @TABLE AS SOURCE 
ON (TARGET.TYPE_OF_SERVICE_ID = SOURCE.TYPE_OF_SERVICE_ID AND TARGET.[APPOINTMENT_ID] = SOURCE.[APPOINTMENT_ID])
WHEN MATCHED 
THEN UPDATE 
SET TYPE_OF_SERVICE_DETAIL_ID = (CASE LEN(SOURCE.TYPE_OF_SERVICE_DETAIL_ID) WHEN 0 THEN NULL ELSE SOURCE.TYPE_OF_SERVICE_DETAIL_ID END),
	UPDATED_DATE = DATEADD(HOUR, 7, GETDATE()),
	UPDATED_USER =@USER

WHEN NOT MATCHED
THEN INSERT ([APPOINTMENT_ID],[TYPE_OF_SERVICE_ID],[TYPE_OF_SERVICE_DETAIL_ID],[CREATED_DATE],[CREATED_USER])
VALUES (
        CASE LEN(SOURCE.APPOINTMENT_ID) WHEN 0 THEN NULL ELSE SOURCE.APPOINTMENT_ID END,
        CASE LEN(SOURCE.TYPE_OF_SERVICE_ID) WHEN 0 THEN NULL ELSE SOURCE.TYPE_OF_SERVICE_ID END,	    
		CASE LEN(SOURCE.TYPE_OF_SERVICE_DETAIL_ID) WHEN 0 THEN NULL ELSE SOURCE.TYPE_OF_SERVICE_DETAIL_ID END,	
		DATEADD(HOUR, 7, GETDATE()),
		@USER);

        	DELETE FROM [APPOINTMENT_TYPE_OF_SERVICE] WHERE TYPE_OF_SERVICE_ID NOT IN (SELECT TYPE_OF_SERVICE_ID FROM @TABLE) AND [APPOINTMENT_ID] = @APPOINTMENT_ID

";

                        //    WHEN NOT MATCHED BY SOURCE
                        //AND 
                        //TARGET.[APPOINTMENT_ID] NOT IN (SELECT [APPOINTMENT_ID] FROM [APPOINTMENT_TYPE_OF_SERVICE] where [APPOINTMENT_ID] = TARGET.[APPOINTMENT_ID])
                        //    THEN  DELETE;                       

                        cmds.Add(cmdDetail);
                    }


                    db.ExecuteNonQueryFromMultipleCommandText(cmds);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal DataTable GetDealerMaster(bool isCriteria)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {


                    string cmd = string.Format(@"
                                DECLARE @TABLE TABLE (DEALER_ID INT, DEALER_NAME NVARCHAR(250),GEO_ID INT)

                                INSERT INTO @TABLE VALUES ('0', '{0}', '0')

                                INSERT INTO @TABLE
                                SELECT DEALER_ID,
                                CASE
                                    WHEN BRANCH_NAME is null OR BRANCH_NAME = '' THEN DEALER_NAME
                                    ELSE FORMATMESSAGE('%s (%s)', DEALER_NAME, BRANCH_NAME ) 
                                END AS DEALER_NAME,  GEO_ID
                                FROM T_DEALER WHERE DEL_FLAG IS NULL AND ACTIVE = 1 

                                SELECT * FROM @TABLE ORDER BY GEO_ID ,DEALER_NAME", (isCriteria) ? "All" : "select");

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;


        }
        internal DataTable GetTypeOfServiceMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {

                    string cmd = @"
DECLARE @TABLE TABLE (ID INT, NAME_EN NVARCHAR(250))

INSERT INTO @TABLE VALUES ('0', 'All')

INSERT INTO @TABLE
SELECT ID, NAME_EN FROM TYPE_OF_SERVICE WHERE DELETED_FLAG IS NULL

SELECT * FROM @TABLE ORDER BY ID";
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;


        }
        internal DataTable GetPickupTimes()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {

                    string cmd = @"
DECLARE @TABLE TABLE (ID INT, START_HOUR NVARCHAR(250))

INSERT INTO @TABLE VALUES ('0', '-')

INSERT INTO @TABLE
SELECT ID, START_HOUR FROM utility_generate_time_of_day 

SELECT * FROM @TABLE ORDER BY ID";
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;


        }
        internal DataTable GetRPTypeOfServiceMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT S.ID, S.NAME_EN,
(SELECT COUNT(*) FROM TYPE_OF_SERVICE_DETAIL T WHERE T.TYPE_OF_SERVICE_ID=S.ID ) AS HAS_CHILD
FROM TYPE_OF_SERVICE S WHERE S.DELETED_FLAG IS NULL AND IS_ACTIVE = 1 ORDER BY ID
";
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;


        }
        internal DataTable GetTypeOfServiceDetailMasterByTypeOfServiceId(string typeOfServiceId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {



                    string cmd = @"
DECLARE @ID INT =N'{0}'
DECLARE @TABLE TABLE (ID INT, NAME_EN NVARCHAR(250))

INSERT INTO @TABLE VALUES ('0', 'select')

INSERT INTO @TABLE
SELECT ID, NAME_EN FROM TYPE_OF_SERVICE_DETAIL 
WHERE  DELETED_FLAG IS NULL AND TYPE_OF_SERVICE_ID = @ID  AND IS_ACTIVE = 1 

SELECT * FROM @TABLE ORDER BY ID";
                    dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(typeOfServiceId)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;


        }

        public TreasureDataAddEventServiceAppointmentRequest GetTreasureDataAppointment(int id)
        {
            TreasureDataAddEventServiceAppointmentRequest request = new TreasureDataAddEventServiceAppointmentRequest();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            SELECT  apm.member_id,
                            CASE WHEN apm.service_appointment_type = 1 THEN 'car_owner'
							WHEN apm.service_appointment_type = 2 THEN 'contact_person'
                            ELSE null
                            END AS user_type,

                            CASE WHEN apm.service_appointment_type = 1 THEN apm.mobile_number
                            ELSE null
                            END AS own_mobile,

                            CASE WHEN apm.service_appointment_type = 1 THEN customer.EMAIL
                            ELSE null
                            END AS own_email,

                            CASE WHEN apm.service_appointment_type = 2 THEN apm.mobile_number
                            ELSE null
                            END AS contact_mobile,

                            CASE WHEN apm.service_appointment_type = 2 THEN customer.EMAIL
                            ELSE null
                            END AS contact_email,

                            apm.plate_number as plate_no,
                            car_model.MODEL as car_model,

                            tof.service_type as service_type,

                            dealer.dealer_name as dealer,

                            FORMAT(apm.appointmentdate, 'yyyy-MM-dd') as service_date,
                            CONCAT(LEFT(apt_time.start_hour, 5), ' - ', LEFT(apt_time.end_hour, 5)) as service_time,

                            apm.remark,
                            status.value_name as status
    
                            FROM appointment as apm
                            INNER JOIN status AS status ON apm.status_id = status.id
                            INNER JOIN T_CUSTOMER AS customer ON apm.member_id = customer.MEMBERID
                            INNER JOIN T_CUSTOMER_CAR AS customer_car ON apm.vehicle_no = customer_car.VIN
                            INNER JOIN T_CAR_MODEL AS car_model ON customer_car.MODEL_ID = car_model.MODEL_ID
                            INNER JOIN T_DEALER AS dealer ON apm.dealer_id = dealer.DEALER_ID
                            INNER JOIN utility_generate_time_of_day as apt_time ON apm.appointmenttime_id = apt_time.id

                            INNER JOIN (select apt_tof.appointment_id, string_agg(tof.name_en, ', ') as service_type
                               from appointment_type_of_service as apt_tof
                               inner join type_of_service as tof on apt_tof.type_of_service_id = tof.id  
                               WHERE apt_tof.appointment_id = {0}
                               GROUP BY apt_tof.appointment_id) as tof on tof.appointment_id = apm.id

                            WHERE apm.id = {1}";



                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, id.ToString(), id.ToString())))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            request.member_id = dt.Rows[0]["member_id"].ToString();
                            request.user_type = dt.Rows[0]["user_type"].ToString();
                            request.own_mobile = dt.Rows[0]["own_mobile"].ToString();
                            request.own_email = dt.Rows[0]["own_email"].ToString();
                            request.contact_mobile = dt.Rows[0]["contact_mobile"].ToString();
                            request.contact_email = dt.Rows[0]["contact_email"].ToString();
                            request.plate_no = dt.Rows[0]["plate_no"].ToString();
                            request.car_model = dt.Rows[0]["car_model"].ToString();
                            request.service_type = dt.Rows[0]["service_type"].ToString();
                            request.dealer = dt.Rows[0]["dealer"].ToString();
                            request.service_date = dt.Rows[0]["service_date"].ToString();
                            request.service_time = dt.Rows[0]["service_time"].ToString();
                            request.remark = dt.Rows[0]["remark"].ToString();
                            request.status = dt.Rows[0]["status"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return request;
        }
    }
}