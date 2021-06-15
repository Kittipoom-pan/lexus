using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;
using System.Data.SqlClient;

namespace com.lexus.th.web
{
    public class ServiceTestDriveService
    {
        private string conn;
        public ServiceTestDriveService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetServiceTestDrive(ServiceTestDriveCriteria criteria)
        {
            DataTable dt = new DataTable();
            try
            {
    
                                                                           string cmd = @"
                               
      SELECT T.[ID]
      ,T.[CODE]

      ,T.[MEMBER_ID]
      ,T.[FIRSTNAME]
      ,T.[SURNAME]
      ,T.[MOBILE_NUMBER]
      ,T.[EMAIL]
      ,C.[MODEL] AS PREFERRED_MODEL
      ,D.[DEALER_NAME]
      ,W.CallCenterMobile AS [DEALER_MOBILE]
      ,P.[NAME_EN] AS PURCHASE_PLAN
      ,T.[CONFIRMED_DATE]
	  ,T.[UPDATED_DATE]
      ,T.[CONFIRMED_TIME]
	  ,T.[CANCEL_DATE]      
      ,T.[CANCEL_REASON]
      ,(CASE WHEN T.[status_id]=1 THEN 'Waiting to confirm' ELSE 
                                          CASE WHEN T.[status_id]=2 THEN 'Confirm' ELSE 
                                          CASE WHEN T.[status_id]=3 THEN 'Cancel' ELSE 
										  CASE WHEN T.[status_id]=4 THEN 'Complete' ELSE '' END END END END) AS STATUS_NAME
      ,T.[CREATED_DATE]
                                      
        FROM [DBO].[TEST_DRIVE] T
        LEFT JOIN [T_DEALER] D ON D.[DEALER_ID] = T.[DEALER_ID]
        LEFT JOIN [T_DEALER_WORKINGTIME ] W ON W.Dealer_id = T.dealer_id and W.Service_Type = 'TestDrive'
        LEFT JOIN [PURCHASE_PLAN] P ON T.[PURCHASE_PLAN_ID] = P.[ID] 
        LEFT JOIN [T_CAR_MODEL] C ON C.[MODEL_ID] = T.[PREFERRED_MODEL_ID] 

                                  WHERE T.[DELETED_FLAG] IS NULL 
                                        AND ( 1=1  ";


                if (!string.IsNullOrEmpty(criteria.DealerId) && criteria.DealerId != "0")
                {
                    cmd += string.Format(" AND T.[DEALER_ID] = {0}", criteria.DealerId);
                }
                if (!string.IsNullOrEmpty(criteria.PreferredModel) && criteria.PreferredModel != "0")
                {
                    cmd += string.Format(" AND T.[PREFERRED_MODEL_ID] = {0}", criteria.PreferredModel);
                }
                if (!string.IsNullOrEmpty(criteria.TestDriveCode))
                {
                    cmd += string.Format(" AND (T.[CODE]  LIKE N'%{0}%')", criteria.TestDriveCode);
                }
                if (!string.IsNullOrEmpty(criteria.Name))
                {
                    cmd += string.Format(" AND (T.[FIRSTNAME]  LIKE N'%{0}%')", criteria.Name);
                }
                if (!string.IsNullOrEmpty(criteria.Surname))
                {
                    cmd += string.Format(" AND (T.[SURNAME]  LIKE N'%{0}%')", criteria.Surname);
                }
                if (!string.IsNullOrEmpty(criteria.MobileNumber))
                {
                    cmd += string.Format(" AND (T.[MOBILE_NUMBER]  LIKE N'%{0}%')", criteria.MobileNumber);
                }
                if (!string.IsNullOrEmpty(criteria.Status) && criteria.Status != "-1")
                {
                    cmd += string.Format(" AND T.[STATUS_ID] = {0}", criteria.Status);
                }
                if (!string.IsNullOrEmpty(criteria.RegisDateFrom))
                {
                    var date = criteria.RegisDateFrom;
                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, T.[CREATED_DATE], 120), 120)  >= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.RegisDateTo))
                {
                    var date = criteria.RegisDateTo;

                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, T.[CREATED_DATE], 120), 120)  <= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }

                cmd += @"  ) ORDER BY T.CREATED_DATE";



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
        public DataTable GetServiceTestDriveExport(ServiceTestDriveCriteria criteria)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
                               
      SELECT ROW_NUMBER() OVER(ORDER BY T.CREATED_DATE) as [No.] 
              ,T.[CODE] as [Test Drive Code]  
              ,T.[CREATED_DATE] as [Register Date]      
              ,T.[MEMBER_ID] as [Member ID]
              ,T.[FIRSTNAME] as [First Name]
              ,T.[SURNAME] as [Surname]
              ,T.[MOBILE_NUMBER] as [Mobile]
              ,T.[EMAIL] as [Email]
              ,C.[MODEL] AS [Preferred Model]
              ,D.[DEALER_NAME] as [Dealer Name]
              ,W.CallCenterMobile AS [Dealer Mobile]
              ,P.[NAME_EN] AS [Purchase Plan]
              ,T.[CONFIRMED_DATE] as [Confirm Date]
              ,T.[CONFIRMED_TIME] as [Confirm Time]
        	  ,T.[UPDATED_DATE] as [Update Date]
              ,T.[CALL_CENTER_REMARK] as [Call Center Remark]
        	  ,T.[CANCEL_DATE] as [Cancel]     
              ,T.[CANCEL_REASON] as [Cancel Reason]
              ,(CASE WHEN T.[status_id]=1 THEN 'Waiting to confirm' ELSE 
                                          CASE WHEN T.[status_id]=2 THEN 'Confirm' ELSE 
                                          CASE WHEN T.[status_id]=3 THEN 'Cancel' ELSE 
										  CASE WHEN T.[status_id]=4 THEN 'Complete' ELSE '' END END END END) AS [Status]
              ,T.[Remark] as [Remark]
                                              
                FROM [DBO].[TEST_DRIVE] T
                LEFT JOIN [T_DEALER] D ON D.[DEALER_ID] = T.[DEALER_ID]
                LEFT JOIN [T_DEALER_WORKINGTIME ] W ON W.Dealer_id = T.dealer_id and W.Service_Type = 'TestDrive'
                LEFT JOIN [PURCHASE_PLAN] P ON T.[PURCHASE_PLAN_ID] = P.[ID] 
                LEFT JOIN [T_CAR_MODEL] C ON C.[MODEL_ID] = T.[PREFERRED_MODEL_ID] 

                                  WHERE T.[DELETED_FLAG] IS NULL 
                                        AND ( 1=1  ";


                if (!string.IsNullOrEmpty(criteria.DealerId) && criteria.DealerId != "0")
                {
                    cmd += string.Format(" AND T.[DEALER_ID] = {0}", criteria.DealerId);
                }
                if (!string.IsNullOrEmpty(criteria.PreferredModel) && criteria.PreferredModel != "0")
                {
                    cmd += string.Format(" AND T.[PREFERRED_MODEL_ID] = {0}", criteria.PreferredModel);
                }
                if (!string.IsNullOrEmpty(criteria.TestDriveCode))
                {
                    cmd += string.Format(" AND (T.[CODE]  LIKE N'%{0}%')", criteria.TestDriveCode);
                }
                if (!string.IsNullOrEmpty(criteria.Name))
                {
                    cmd += string.Format(" AND (T.[FIRSTNAME]  LIKE N'%{0}%')", criteria.Name);
                }
                if (!string.IsNullOrEmpty(criteria.Surname))
                {
                    cmd += string.Format(" AND (T.[SURNAME]  LIKE N'%{0}%')", criteria.Surname);
                }
                if (!string.IsNullOrEmpty(criteria.MobileNumber))
                {
                    cmd += string.Format(" AND (T.[MOBILE_NUMBER]  LIKE N'%{0}%')", criteria.MobileNumber);
                }
                if (!string.IsNullOrEmpty(criteria.Status) && criteria.Status != "-1")
                {
                    cmd += string.Format(" AND T.[STATUS_ID] = {0}", criteria.Status);
                }
                if (!string.IsNullOrEmpty(criteria.RegisDateFrom))
                {
                    var date = criteria.RegisDateFrom;
                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, T.[CREATED_DATE], 120), 120)  >= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }
                if (!string.IsNullOrEmpty(criteria.RegisDateTo))
                {
                    var date = criteria.RegisDateTo;

                    cmd += string.Format(" AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, T.[CREATED_DATE], 120), 120)  <= CONVERT(NVARCHAR(10), CONVERT(DATETIME, N'{0}' , 103), 120) ", date);
                }

                cmd += @"  ) ORDER BY T.CREATED_DATE";



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

        public DataRow GetTestDriveById(string id)
        {
            DataRow row = null;
            try
            { // ,A.[MODEL_NAME]
                string cmd = @"
                DECLARE @ID INT = N'{0}'
                
      SELECT T.[ID]
      ,T.[CODE]
     
      ,T.[MEMBER_ID]
      ,T.[FIRSTNAME]
      ,T.[SURNAME]
      ,T.[MOBILE_NUMBER]
      ,T.[EMAIL]
      ,T.[PREFERRED_MODEL_ID]
      ,T.[DEALER_ID]   
      ,T.[PURCHASE_PLAN_ID]
      ,T.[CONFIRMED_DATE]
      ,T.[CONFIRMED_TIME]
	  ,T.[UPDATED_DATE]
      ,T.[IS_CANCEL]
	  ,T.[CANCEL_DATE]      
      ,T.[CANCEL_REASON]
      ,T.[STATUS_ID]
      ,T.[CALL_CENTER_REMARK]
      ,T.CREATED_DATE                                
        FROM [DBO].[TEST_DRIVE] T
        WHERE T.[ID] = @ID";

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

        public void UpdateTestDrive(TestDriveModel testDrive, string user)
        {
            try
            {
                string cmd = @" 
DECLARE @ID                   INT             = N'{0}'
DECLARE @FIRSTNAME            NVARCHAR(250)   = N'{1}'
DECLARE @SURNAME              NVARCHAR(250)   = N'{2}'
DECLARE @MOBILE_NUMBER        NVARCHAR(50)    = N'{3}'
DECLARE @EMAIL                NVARCHAR(100)   = N'{4}'
DECLARE @PREFERRED_MODEL_ID   INT             = N'{5}'
DECLARE @DEALER_ID            INT             = N'{6}'
DECLARE @PURCHASE_PLAN_ID     INT             = N'{7}'
DECLARE @CONFIRM_DATE         NVARCHAR(20)    = N'{8}'
DECLARE @CONFIRM_TIME         NVARCHAR(20)    = N'{9}'
DECLARE @IS_CANCEL            BIT             = N'{10}'
DECLARE @CANCEL_DATE          NVARCHAR(20)    = N'{11}'
DECLARE @CANCEL_REASON        NVARCHAR(255)   = N'{12}'
DECLARE @CALL_CENTER_REMARK   NVARCHAR(255)   = N'{13}'
DECLARE @STATUS_ID            INT             = N'{14}'
DECLARE @USER                 NVARCHAR(50)    = N'{15}'

UPDATE [TEST_DRIVE]
SET 
         FIRSTNAME          = CASE LEN(@FIRSTNAME) WHEN 0 THEN NULL ELSE @FIRSTNAME END,
         SURNAME            = CASE LEN(@SURNAME) WHEN 0 THEN NULL ELSE @SURNAME END,
         MOBILE_NUMBER      = CASE LEN(@MOBILE_NUMBER) WHEN 0 THEN NULL ELSE @MOBILE_NUMBER END,
         EMAIL              = CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
         PREFERRED_MODEL_ID = CASE LEN(@PREFERRED_MODEL_ID) WHEN 0 THEN NULL ELSE  @PREFERRED_MODEL_ID END,
         DEALER_ID          = CASE LEN(@DEALER_ID) WHEN 0 THEN NULL ELSE  @DEALER_ID END,
         PURCHASE_PLAN_ID   = CASE LEN(@PURCHASE_PLAN_ID) WHEN 0 THEN NULL ELSE  @PURCHASE_PLAN_ID END,
         CONFIRMED_DATE       = CASE LEN(@CONFIRM_DATE) WHEN 0 THEN NULL ELSE  CONVERT(DATETIME, @CONFIRM_DATE, 103) END,  
         CONFIRMED_TIME          = CASE LEN(@CONFIRM_TIME) WHEN 0 THEN NULL ELSE @CONFIRM_TIME END,
         IS_CANCEL          = CASE LEN(@IS_CANCEL) WHEN 0 THEN NULL ELSE  @IS_CANCEL END,
         CANCEL_DATE        = CASE LEN(@CANCEL_DATE) WHEN 0 THEN NULL ELSE   CONVERT(DATETIME, @CANCEL_DATE, 103) END,
         CANCEL_REASON      = CASE LEN(@CANCEL_REASON) WHEN 0 THEN NULL ELSE  @CANCEL_REASON       END,
         CALL_CENTER_REMARK = CASE LEN(@CALL_CENTER_REMARK) WHEN 0 THEN NULL ELSE  @CALL_CENTER_REMARK  END,
         STATUS_ID          = CASE LEN(@STATUS_ID) WHEN 0 THEN NULL ELSE  @STATUS_ID END,
	     UPDATED_DATE       = DATEADD(HOUR, 7, GETDATE()),
	     UPDATED_USER       = @USER
         WHERE		[ID] = @ID  ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    List<string> cmds = new List<string>();
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(testDrive.Id),
                        WebUtility.GetSQLTextValue(testDrive.Name),
                        WebUtility.GetSQLTextValue(testDrive.Surname),
                        WebUtility.GetSQLTextValue(testDrive.MobileNumber),
                        WebUtility.GetSQLTextValue(testDrive.Email),
                        WebUtility.GetSQLTextValue(testDrive.PreferredModelId),
                        WebUtility.GetSQLTextValue(testDrive.DealerId),
                        WebUtility.GetSQLTextValue(testDrive.PurchasePlanId),
                        WebUtility.GetSQLTextValue(testDrive.ConfirmDate),
                        WebUtility.GetSQLTextValue(testDrive.ConfirmTime),
                        WebUtility.GetSQLTextValue(testDrive.IsCancel),
                        WebUtility.GetSQLTextValue(testDrive.CancelDate),
                        WebUtility.GetSQLTextValue(testDrive.CancelReason),
                        WebUtility.GetSQLTextValue(testDrive.CallCenterRemark),
                        WebUtility.GetSQLTextValue(testDrive.StatusId),
                        user);
                    cmds.Add(cmd);

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
                               SELECT 
                               d.DEALER_ID,
                               dealer_master.display_th
                               FROM T_DEALER d
                               INNER JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = d.DEALER_CODE AND dealer_master.branch_code = d.branch_code AND dealer_master.is_active = 1
                               WHERE d.ACTIVE = 1  AND d.DEL_FLAG IS NULL");
                  
                    dt = db.GetDataTableFromCommandText(cmd);
                    DataRow toInsert = dt.NewRow();
                    if(isCriteria)
                    {
                        toInsert["DEALER_ID"] = 0;
                        toInsert["display_th"] = "All";
                        dt.Rows.InsertAt(toInsert, 0);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;


        }
        internal DataTable GetPreferredModel(bool isCriteria)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {

                    string cmd = string.Format(@"
DECLARE @TABLE TABLE (MODEL_ID INT, MODEL NVARCHAR(250))

INSERT INTO @TABLE VALUES ('0', '{0}')

INSERT INTO @TABLE
SELECT MODEL_ID, MODEL FROM T_CAR_MODEL WHERE IS_TEST_DRIVE=1 AND DEL_FLAG IS NULL

SELECT * FROM @TABLE ORDER BY MODEL_ID", (isCriteria) ? "All" : "select");
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;


        }
        public DataTable GetPurchasePlan()
        {
            DataTable dt = null;
            try
            {

                string cmd = @"

DECLARE @TABLE TABLE (ID INT, NAME_EN NVARCHAR(100))

INSERT INTO @TABLE VALUES ('0', 'select')

INSERT INTO @TABLE
SELECT [ID],[NAME_EN] FROM [dbo].[PURCHASE_PLAN]
WHERE [DELETED_FLAG] IS NULL AND IS_ACTIVE = 1

SELECT * FROM @TABLE";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public TreasureDataAddEventTestDriveRequest GetTreasureDataTestDrive(int id)
        {
            TreasureDataAddEventTestDriveRequest request = new TreasureDataAddEventTestDriveRequest();
            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
            {
                string cmd = @"
                                   SELECT 	test_drive.id,
				                    test_drive.member_id,
				                    test_drive.firstname as name,
				                    test_drive.surname,
				                    test_drive.mobile_number as contact_no,
				                    test_drive.email,
				                    model.MODEL as interested_model,
				                    status.value_name as status,
				                    dealer.DEALER_NAME as dealer,
				                    purchase_plan.name_en as buying_plan,
				                    test_drive.remark,
				                    FORMAT (COALESCE(test_drive.updated_date, test_drive.created_date), 'yyyy-MM-dd HH:mm') as datetime
                                    FROM test_drive 
                                    INNER JOIN status AS status ON test_drive.status_id = status.id
                                    INNER JOIN T_CAR_MODEL AS model ON test_drive.purchase_plan_id = model.MODEL_ID
                                    INNER JOIN T_DEALER AS dealer ON test_drive.dealer_id = dealer.DEALER_ID
                                    INNER JOIN purchase_plan AS purchase_plan ON test_drive.purchase_plan_id = purchase_plan.id
                                    WHERE test_drive.id = N'{0}'";

                DateTime datetime_now = DateTime.Now;
                using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, id.ToString())))
                {
                    if (dt.Rows.Count > 0)
                    {
                        request.member_id = dt.Rows[0]["member_id"].ToString();
                        request.name = dt.Rows[0]["name"].ToString();
                        request.surname = dt.Rows[0]["surname"].ToString();
                        request.contact_no = dt.Rows[0]["contact_no"].ToString();
                        request.email = dt.Rows[0]["email"].ToString();
                        request.interested_model = dt.Rows[0]["interested_model"].ToString();
                        request.dealer = dt.Rows[0]["dealer"].ToString();
                        request.buying_plan = dt.Rows[0]["buying_plan"].ToString();
                        request.remark = dt.Rows[0]["remark"].ToString();
                        request.status = dt.Rows[0]["status"].ToString();
                        request.datetime = dt.Rows[0]["datetime"].ToString();
                    }
                }
            }
            return request;
        }

    }
}