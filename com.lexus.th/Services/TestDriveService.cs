using AppLibrary.Database;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class TestDriveService
    {
        string conn;
        public TestDriveService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceTestDriveCreateModel> RegisterTestDrive(string v, string p, string token, string language, ValidateParameter parameter)
        {

            ServiceTestDriveCreateModel value = new ServiceTestDriveCreateModel();
            value.data = new TestDriveCreateData();
            try
            {
                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystemNew(p, v, language);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel()
                    {
                        code = validation.InvalidCode,
                        text = validation.InvalidMessage
                    };
                    return value;
                }
                else
                {

                    ValidationModel validationRegis = new ValidationModel();
                    validationRegis =await ValidateRegister(parameter, language);

                    if (!validationRegis.Success)
                    {
                        value.success = validationRegis.Success;
                        value.msg = new MsgModel()
                        {
                            code = validationRegis.InvalidCode,
                            text = validationRegis.InvalidMessage
                        };
                        return value;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(token.Trim()))
                        {
                            //save only member_id
                            value =await RegisterTestDriveByMember(token, parameter);
                        }
                        if (string.IsNullOrEmpty(token.Trim()) && !string.IsNullOrEmpty(parameter.device_id.Trim()))
                        {
                            //save only device_id
                            value =await RegisterTestDriveByDevice(parameter);
                        }

                        if (value.success) {
                            TreasureDataController treasureCtl = new TreasureDataController();
                            treasureCtl.AddEventTestDrive(value.data.id);
                        }

                        int test_drive_id = value.data.id;
                        SendEmailService sendEmailService = new SendEmailService();

                        TestDriveDetailDataForExportExcel exportExcelData =await GetDataForExportExcel(test_drive_id, language);
                        byte[] attachFile = ExportExcelTestDrive(exportExcelData);

                        string attachFileName = "";
                        if (attachFile.Length > 0)
                        {
                            attachFileName = "Test_Drive_" + exportExcelData.code + ".xlsx";
                        }

                        Task.Run(async () => { await sendEmailService.SendEmail("Test_Drive_CallCenter", test_drive_id, token, language, attachFile, attachFileName); });
                        value.msg = new MsgModel() { code = 200, text = "success" };
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return value;
        }


        public async Task<ServiceTestDriveModel> GetTestDriveHistory(string v, string p, string token, string language, string device_id)
        {
            ServiceTestDriveModel value = new ServiceTestDriveModel();

            try
            {
                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystemNew(p, v, language);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel()
                    {
                        code = validation.InvalidCode,
                        text = validation.InvalidMessage
                    };
                    return value;
                }
                else
                {

                    if (!string.IsNullOrEmpty(token.Trim()) && !string.IsNullOrEmpty(device_id.Trim()))
                    {
                        value.data = GetTestDrive(token, language, device_id);
                    }
                    if (string.IsNullOrEmpty(token.Trim()) && !string.IsNullOrEmpty(device_id.Trim()))
                    {
                        value.data = GetTestDriveByDeviceId(device_id, language);
                    }

                    value.success = true;
                    value.msg = new MsgModel()
                    {
                        code = 200,
                        text = "Success"
                    };
                    value.ts = DateTimeOffset.Now;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }


        public async Task<ServiceTestDriveDetailModel> GetTestDriveDetail(string v, string p, string token, string language, int test_drive_id)
        {
            ServiceTestDriveDetailModel value = new ServiceTestDriveDetailModel();

            try
            {
                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystemNew(p, v, language);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel()
                    {
                        code = validation.InvalidCode,
                        text = validation.InvalidMessage
                    };
                    return value;
                }
                else
                {

                    value.data = await GetTestDriveOnDetail(token, language, test_drive_id);
                    value.success = true;
                    value.msg = new MsgModel()
                    {
                        code = 200,
                        text = "Success"
                    };
                    value.ts = DateTimeOffset.Now;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;

        }

        public TreasureDataAddEventTestDriveReques GetTreasureDataTestDrive(int id)
        {
            TreasureDataAddEventTestDriveReques request = new TreasureDataAddEventTestDriveReques();
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

        #region Sub Method

        private async Task<ServiceTestDriveCreateModel> RegisterTestDriveByMember(string token, ValidateParameter parameter)
        {
            ServiceTestDriveCreateModel value = new ServiceTestDriveCreateModel();
            value.data = new TestDriveCreateData();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                                   DECLARE @generate_code NVARCHAR(20);
                                   DECLARE @member_id NVARCHAR(50);
                                   DECLARE @max_id NVARCHAR(10);

                                   SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{10}'
                                   SELECT @max_id = CONVERT(NVARCHAR(10),count(id)+1) FROM test_drive
                                   SELECT @generate_code = 'TD' + REPLACE(CONVERT(CHAR(10), GETDATE(), 23),'-','')+'-' + SUBSTRING(CAST((POWER(10, 3) + @max_id) AS NVARCHAR(4)), 2, 3)

                                   INSERT INTO test_drive
                                       (code, register_date, member_id, firstname, surname, mobile_number, email, preferred_model_id, 
                                        dealer_id, purchase_plan_id, remark, status_id, created_date, created_user)
                                   VALUES
                                       (@generate_code, N'{9}', @member_id, N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', 
                                        N'{5}', N'{6}', N'{7}', N'{8}', N'{9}', @member_id)
                                       
                                   SELECT SCOPE_IDENTITY() AS id";

                    DateTime datetime_now = DateTime.Now;
                    using (DataTable dt = db.GetDataTableFromCommandText
                          (string.Format(cmd, parameter.f_name, parameter.l_name, parameter.mobile, parameter.email, parameter.preferred_model_id,
                           parameter.dealer_id, parameter.purchase_plan_id, parameter.remark, parameter.status_id, datetime_now, token)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            value.data.id = Convert.ToInt32(dt.Rows[0]["id"]);
                            value.msg = new MsgModel() { code = 200, text = "Success" };
                            value.success = true;
                            value.ts = DateTimeOffset.Now;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


            return value;
        }
        private async Task<ServiceTestDriveCreateModel> RegisterTestDriveByDevice(ValidateParameter parameter)
        {
            ServiceTestDriveCreateModel value = new ServiceTestDriveCreateModel();
            value.data = new TestDriveCreateData();
            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
            {
                string cmd = @"
                                   DECLARE @generate_code NVARCHAR(20);
                                   DECLARE @member_id NVARCHAR(50);
                                   DECLARE @max_id NVARCHAR(10);

                                   
                                   SELECT @max_id = CONVERT(NVARCHAR(10),max(id)+1) FROM test_drive
                                   SELECT @generate_code = 'TD' + REPLACE(CONVERT(CHAR(10), GETDATE(), 23),'-','')+'-' + SUBSTRING(CAST((POWER(10, 3) + @max_id) AS NVARCHAR(4)), 2, 3)

                                   INSERT INTO test_drive
                                       (code, register_date, device_id, firstname, surname, mobile_number, email, preferred_model_id, 
                                        dealer_id, purchase_plan_id, remark, status_id, created_date, created_user)
                                   VALUES
                                       (@generate_code, N'{9}', N'{10}', N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', 
                                        N'{5}', N'{6}', N'{7}', N'{8}', N'{9}', N'{10}')
                                       
                                   SELECT SCOPE_IDENTITY() AS id";

                DateTime datetime_now = DateTime.Now;
                using (DataTable dt = db.GetDataTableFromCommandText
                      (string.Format(cmd, parameter.f_name, parameter.l_name, parameter.mobile, parameter.email, parameter.preferred_model_id,
                       parameter.dealer_id, parameter.purchase_plan_id, parameter.remark, parameter.status_id, datetime_now, parameter.device_id)))
                {
                    if (dt.Rows.Count > 0)
                    {
                        value.data.id = Convert.ToInt32(dt.Rows[0]["id"]);
                        value.success = true;
                        value.msg = new MsgModel() { code = 200, text = "Success" };
                        value.ts = DateTimeOffset.Now;
                    }
                }
            }

            return value;
        }

        private TestDriveHistoryData GetTestDrive(string token, string lang, string device_id)
        {
            TestDriveHistoryData value = new TestDriveHistoryData();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "DECLARE @lang nvarchar(5); " +
                        "SET @lang = N'" + lang + "' " +
                        "SELECT TOP 20 " +
                           "test_drive.id AS id, " +
                           "dealer.DEALER_ID AS dealer_id, " +
                           "CASE " +
                             "WHEN @lang = 'TH' THEN coalesce(dealer_master.display_th, dealer.dealer_name_th, dealer.DEALER_NAME) " +
                             "WHEN @lang = 'EN' THEN coalesce(dealer_master.display_en, dealer.DEALER_NAME, dealer.dealer_name_th) " +
                           "END AS dealer_name, " +
                           "dealer.DEALER_MOBILE AS dealer_mobile, " +
                           "test_drive.mobile_number AS contact_mobile, " +
                           "test_drive.email AS email, " +
                           "status.id AS status_id, " +
                           "status.value_name AS status_name, " +
                           "test_drive.firstname AS f_name, " +
                           "test_drive.surname AS l_name, " +
                           "CONVERT(varchar(5),DAY(test_drive.created_date)) + ' '+{fn MONTHNAME(test_drive.created_date)} +' ' + CONVERT(varchar(5), YEAR(test_drive.created_date)) as date_display, " +
                           "preferred.MODEL AS purchase_model, " +
                           "CASE " +
                               "WHEN @lang = 'TH' THEN coalesce(purchase_plan.name_th, purchase_plan.name_en) " +
                               "WHEN @lang = 'EN' THEN coalesce(purchase_plan.name_en, purchase_plan.name_th) " +
                           "END AS purchase_plan, " +
                           "FORMAT(test_drive.confirmed_date, 'dd MMM yyyy') AS booking_date, " +
                           "FORMAT(test_drive.confirmed_time, N'hh\\:mm')AS booking_time, " +
                           "test_drive.remark AS remark, " +
                           "dealer_work.CallCenterMobile AS call_center_mobile " +
                         "FROM test_drive " +
                           "INNER JOIN T_CUSTOMER_TOKEN AS cus_token ON cus_token.MEMBERID = test_drive.member_id " +
                           "INNER JOIN T_DEALER AS dealer ON dealer.DEALER_ID = test_drive.dealer_id AND dealer.ACTIVE = 1 " +
                           "LEFT JOIN [T_DEALER_WORKINGTIME] AS dealer_work ON dealer_work.Dealer_ID = dealer.DEALER_ID AND dealer_work.Service_Type ='TestDrive'" +
                           "INNER JOIN status AS status ON status.id = test_drive.status_id AND status.is_active = 1 " +
                           "INNER JOIN T_CAR_MODEL AS preferred ON preferred.MODEL_ID = test_drive.preferred_model_id AND preferred.is_test_drive = 1 " +
                           "INNER JOIN purchase_plan AS purchase_plan ON purchase_plan.id = test_drive.purchase_plan_id AND purchase_plan.is_active = 1 " +
                           "LEFT JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = dealer.DEALER_CODE AND dealer_master.branch_code = dealer.branch_code AND dealer_master.is_active = 1" +
                         "WHERE test_drive.deleted_flag IS NULL " +
                         "AND cus_token.TOKEN_NO = N'" + token + "' " +
                         //"AND test_drive.device_id = N'"+ device_id + "' " +
                         "ORDER BY test_drive.created_date DESC";


                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        value = new TestDriveHistoryData();
                        value.test_drive_datas = new List<_ServiceTestDriveHistoryDetailData>();

                        foreach (DataRow row in dt.Rows)
                        {
                            _ServiceTestDriveHistoryDetailData data = new _ServiceTestDriveHistoryDetailData();
                            data.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                            data.dealer_name = row["dealer_name"].ToString();
                            data.dealer_mobile = row["dealer_mobile"].ToString();
                            data.dealer_id = row["dealer_id"] != DBNull.Value ? Convert.ToInt32(row["dealer_id"]) : 0;
                            data.contact_mobile = row["contact_mobile"].ToString();
                            data.email = row["email"].ToString();
                            data.status = row["status_name"].ToString();
                            data.car_model = row["purchase_model"].ToString();
                            data.f_name = row["f_name"].ToString();
                            data.l_name = row["l_name"].ToString();
                            data.booking_date = (Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.cancel || Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.waiting_confirm) ? "" : row["booking_date"].ToString();
                            data.booking_time = (Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.cancel || Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.waiting_confirm) ? "" : row["booking_time"].ToString();
                            data.date = row["date_display"].ToString();
                            data.purchase_model = row["purchase_model"].ToString();
                            data.purchase_plan = row["purchase_plan"].ToString();
                            data.remark = row["remark"].ToString();
                            data.test_drive_call_center = row["call_center_mobile"].ToString();

                            value.test_drive_datas.Add(data);
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

        private TestDriveHistoryData GetTestDriveByDeviceId(string device_id, string lang)
        {
            TestDriveHistoryData value = new TestDriveHistoryData();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "DECLARE @lang nvarchar(5); " +
                        "SET @lang = N'" + lang + "' " +
                        "SELECT TOP 20 " +
                          "test_drive.id AS id, " +
                          "dealer.DEALER_ID AS dealer_id, " +
                          "CASE " +
                            "WHEN @lang = 'TH' THEN coalesce(dealer_master.display_th, dealer.dealer_name_th, dealer.DEALER_NAME) " +
                            "WHEN @lang = 'EN' THEN coalesce(dealer_master.display_en, dealer.DEALER_NAME, dealer.dealer_name_th) " +
                          "END AS dealer_name, " +
                          "dealer.DEALER_MOBILE AS dealer_mobile, " +
                          "test_drive.mobile_number AS contact_mobile, " +
                          "test_drive.email AS email, " +
                          "status.id AS status_id, " +
                           "status.value_name AS status_name, " +
                           "test_drive.firstname AS f_name, " +
                           "test_drive.surname AS l_name, " +
                           "CONVERT(varchar(5),DAY(test_drive.created_date)) + ' '+{fn MONTHNAME(test_drive.created_date)} +' ' + CONVERT(varchar(5), YEAR(test_drive.created_date)) as date_display, " +
                           "preferred.MODEL AS purchase_model, " +
                           "CASE " +
                               "WHEN @lang = 'TH' THEN coalesce(purchase_plan.name_th, purchase_plan.name_en) " +
                               "WHEN @lang = 'EN' THEN coalesce(purchase_plan.name_en, purchase_plan.name_th) " +
                           "END AS purchase_plan, " +
                           "FORMAT(test_drive.confirmed_date, 'dd MMM yyyy') AS booking_date, " +
                           "FORMAT(test_drive.confirmed_time, N'hh\\:mm')AS booking_time, " +
                           "test_drive.remark AS remark, " +
                           "dealer_work.CallCenterMobile AS call_center_mobile " +
                         "FROM test_drive " +
                           "INNER JOIN T_DEVICE AS device ON device.DEVICE_ID = test_drive.device_id  " +
                           "INNER JOIN T_DEALER AS dealer ON dealer.DEALER_ID = test_drive.dealer_id AND dealer.ACTIVE = 1 " +
                           "LEFT JOIN [T_DEALER_WORKINGTIME] AS dealer_work ON dealer_work.Dealer_ID = dealer.DEALER_ID AND dealer_work.Service_Type ='TestDrive'" +
                           "INNER JOIN status AS status ON status.id = test_drive.status_id AND status.is_active = 1 " +
                           "INNER JOIN T_CAR_MODEL AS preferred ON preferred.MODEL_ID = test_drive.preferred_model_id AND preferred.is_test_drive = 1 " +
                           "INNER JOIN purchase_plan AS purchase_plan ON purchase_plan.id = test_drive.purchase_plan_id AND purchase_plan.is_active = 1 " +
                            "LEFT JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = dealer.DEALER_CODE AND dealer_master.branch_code = dealer.branch_code AND dealer_master.is_active = 1" +
                           "WHERE test_drive.deleted_flag IS NULL " +
                         "AND device.DEVICE_ID = N'" + device_id + "' AND test_drive.member_id IS NULL " +
                         "ORDER BY test_drive.created_date DESC";


                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        value = new TestDriveHistoryData();
                        value.test_drive_datas = new List<_ServiceTestDriveHistoryDetailData>();

                        foreach (DataRow row in dt.Rows)
                        {
                            _ServiceTestDriveHistoryDetailData data = new _ServiceTestDriveHistoryDetailData();
                            data.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                            data.dealer_name = row["dealer_name"].ToString();
                            data.dealer_mobile = row["dealer_mobile"].ToString();
                            data.dealer_id = row["dealer_id"] != DBNull.Value ? Convert.ToInt32(row["dealer_id"]) : 0;
                            data.contact_mobile = row["contact_mobile"].ToString();
                            data.email = row["email"].ToString();
                            data.status = row["status_name"].ToString();
                            data.car_model = row["purchase_model"].ToString();
                            data.f_name = row["f_name"].ToString();
                            data.l_name = row["l_name"].ToString();
                            data.booking_date = (Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.cancel || Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.waiting_confirm) ? "" : row["booking_date"].ToString();
                            data.booking_time = (Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.cancel || Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.waiting_confirm) ? "" : row["booking_time"].ToString();
                            data.date = row["date_display"].ToString();
                            data.purchase_model = row["purchase_model"].ToString();
                            data.purchase_plan = row["purchase_plan"].ToString();
                            data.remark = row["remark"].ToString();
                            data.test_drive_call_center = row["call_center_mobile"].ToString();

                            value.test_drive_datas.Add(data);
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

        private async Task<TestDriveDetailData> GetTestDriveOnDetail(string token, string lang, int test_drive_id)
        {
            TestDriveDetailData value = new TestDriveDetailData();
            value.test_drive_data = new _ServiceTestDriveDetailData();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "DECLARE @lang nvarchar(5); " +
                        
                        "SET @lang = N'" + lang + "' " +
                        
                        "SELECT " +
                           "test_drive.id AS id, " +
                           "dealer.DEALER_ID AS dealer_id, " +
                           "CASE " +
                           "WHEN @lang = 'TH' THEN coalesce(dealer_master.display_th, dealer.dealer_name_th, dealer.DEALER_NAME) " +
                           "WHEN @lang = 'EN' THEN coalesce(dealer_master.display_en, dealer.DEALER_NAME, dealer.dealer_name_th) " +
                           "END AS dealer_name, " +
                           "dealer.DEALER_MOBILE AS dealer_mobile, " +
                           "test_drive.mobile_number AS contact_mobile, " +
                           "test_drive.email AS email, " +
                           "status.id AS status_id, " +
                           "status.value_name AS status_name, " +
                           "test_drive.firstname AS f_name, " +
                           "test_drive.surname AS l_name, " +
                           "CONVERT(varchar(5),DAY(test_drive.created_date)) + ' '+{fn MONTHNAME(test_drive.created_date)} +' ' + CONVERT(varchar(5), YEAR(test_drive.created_date)) as date_display, " +
                           "preferred.MODEL AS purchase_model, " +
                           "CASE " +
                               "WHEN @lang = 'TH' THEN coalesce(purchase_plan.name_th, purchase_plan.name_en) " +
                               "WHEN @lang = 'EN' THEN coalesce(purchase_plan.name_en, purchase_plan.name_th) " +
                           "END AS purchase_plan, " +
                           "FORMAT(test_drive.confirmed_date, 'dd MMM yyyy') AS booking_date, " +
                           "FORMAT(test_drive.confirmed_time, N'hh\\:mm')AS booking_time, " +
                           "test_drive.remark AS remark " +
                         "FROM test_drive " +
                           "INNER JOIN T_DEALER AS dealer ON dealer.DEALER_ID = test_drive.dealer_id AND dealer.ACTIVE = 1 " +
                           "INNER JOIN status AS status ON status.id = test_drive.status_id AND status.is_active = 1 " +
                           "INNER JOIN T_CAR_MODEL AS preferred ON preferred.MODEL_ID = test_drive.preferred_model_id AND preferred.is_test_drive = 1 " +
                           "INNER JOIN purchase_plan AS purchase_plan ON purchase_plan.id = test_drive.purchase_plan_id AND purchase_plan.is_active = 1 " +
                           "LEFT JOIN T_DEALER_MASTER AS dealer_master on dealer_master.dealer_code = dealer.DEALER_CODE AND dealer_master.branch_code = dealer.branch_code AND dealer_master.is_active = 1" +
                         "WHERE test_drive.deleted_flag IS NULL " +
                         "AND test_drive.id =N'" + test_drive_id + "'";


                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            value.test_drive_data.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                            value.test_drive_data.dealer_name = row["dealer_name"].ToString();
                            value.test_drive_data.dealer_mobile = row["dealer_mobile"].ToString();
                            value.test_drive_data.dealer_id = row["dealer_id"] != DBNull.Value ? Convert.ToInt32(row["dealer_id"]) : 0;
                            value.test_drive_data.contact_mobile = row["contact_mobile"].ToString();
                            value.test_drive_data.email = row["email"].ToString();
                            value.test_drive_data.status = row["status_name"].ToString();
                            value.test_drive_data.car_model = row["purchase_model"].ToString();
                            value.test_drive_data.f_name = row["f_name"].ToString();
                            value.test_drive_data.l_name = row["l_name"].ToString();
                            value.test_drive_data.booking_date = (Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.cancel || Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.waiting_confirm) ? "" : row["booking_date"].ToString();
                            value.test_drive_data.booking_time = (Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.cancel || Convert.ToInt16(row["status_id"]) == (byte)ProcessStatus.waiting_confirm) ? "" : row["booking_time"].ToString();
                            value.test_drive_data.date = row["date_display"].ToString();
                            value.test_drive_data.purchase_model = row["purchase_model"].ToString();
                            value.test_drive_data.purchase_plan = row["purchase_plan"].ToString();
                            value.test_drive_data.remark = row["remark"].ToString();
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

        public TestDriveDetailForSendEmail GetTestDriveDetailForEmail(string lang, int test_drive_id)
        {
            TestDriveDetailForSendEmail value = new TestDriveDetailForSendEmail();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        DECLARE @lang NVARCHAR(2)=N'{0}';
                        DECLARE @tcap_call_center_email NVARCHAR(300); 
                        SET @tcap_call_center_email = (SELECT data_config FROM system_config WHERE name = 'call_center_email') 

                        SELECT 
                        test_drive.id AS id,
                        test_drive.code AS code,
                        FORMAT(test_drive.created_date, 'dd MMM yyyy HH:mm:ss tt') AS create_date,
                        test_drive.firstname AS name,
                        test_drive.surname AS surname,
                        test_drive.mobile_number AS contact_number,
                        test_drive.email AS email,
                        preferred.MODEL AS preferred_model,
                        CASE
                        	WHEN @lang = 'TH' THEN dealer.dealer_name_th
                        	WHEN @lang = 'EN' THEN dealer.DEALER_NAME
                        END AS dealer,
                        dealer.BRANCH_NAME AS branch_name,
                        CASE
                        	WHEN @lang = 'TH' THEN purchase_plan.name_th
                        	WHEN @lang = 'EN' THEN purchase_plan.name_en
                        END AS car_purchase_plan,
                        dealer_work.CallCenterEmail AS call_center_email,
                        test_drive.remark AS remark,
                        @tcap_call_center_email AS tcap_call_center_email
                        FROM test_drive  
                        INNER JOIN T_DEALER AS dealer ON dealer.DEALER_ID = test_drive.dealer_id AND dealer.ACTIVE = 1  
                        INNER JOIN T_CAR_MODEL AS preferred ON preferred.MODEL_ID = test_drive.preferred_model_id AND preferred.is_test_drive = 1  
                        INNER JOIN purchase_plan AS purchase_plan ON purchase_plan.id = test_drive.purchase_plan_id AND purchase_plan.is_active = 1  
                        INNER JOIN T_DEALER_WORKINGTIME AS dealer_work ON dealer_work.Dealer_ID = dealer.DEALER_ID AND dealer_work.Service_Type ='TestDrive'
                        WHERE test_drive.deleted_flag IS NULL AND test_drive.id = N'{1}'";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, test_drive_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            value.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                            value.code = row["code"].ToString();
                            value.create_date = row["create_date"].ToString();
                            value.name = row["name"].ToString();
                            value.surname = row["surname"].ToString();
                            value.contact_number = row["contact_number"].ToString();
                            value.email = row["email"].ToString();
                            value.preferred_model = row["preferred_model"].ToString();
                            value.dealer = row["dealer"].ToString();
                            value.car_purchase_plan = row["car_purchase_plan"].ToString();
                            value.call_center_email = row["call_center_email"].ToString();
                            value.remark = row["remark"].ToString();
                            value.tcap_call_center_email = row["tcap_call_center_email"].ToString();
                            value.branch_name = row["branch_name"].ToString();
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

        public async Task<TestDriveDetailDataForExportExcel> GetDataForExportExcel(int test_drive_id, string lang)
        {
            TestDriveDetailDataForExportExcel data = new TestDriveDetailDataForExportExcel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        DECLARE @lang NVARCHAR(2)=N'{0}';
                        
                        SELECT 
                        test_drive.code AS code,
                        test_drive.firstname AS name,
                        test_drive.surname AS surname,
                        test_drive.mobile_number AS contact_number,
                        test_drive.email AS email,
                        preferred.MODEL AS preferred_model,
                        CASE
                        	WHEN @lang = 'TH' THEN dealer.dealer_name_th
                        	WHEN @lang = 'EN' THEN dealer.DEALER_NAME
                        END AS dealer_name,
                        dealer.BRANCH_NAME AS branch_name,
						test_drive.remark AS remark,
                        CASE
                            WHEN @lang = 'TH' THEN purchase_plan.name_th
                            WHEN @lang = 'EN' THEN purchase_plan.name_en
                        END AS purchase_plan
                        FROM test_drive  
                        INNER JOIN T_DEALER AS dealer ON dealer.DEALER_ID = test_drive.dealer_id AND dealer.ACTIVE = 1  
                        INNER JOIN T_CAR_MODEL AS preferred ON preferred.MODEL_ID = test_drive.preferred_model_id AND preferred.is_test_drive = 1  
                        INNER JOIN T_DEALER_WORKINGTIME AS dealer_work ON dealer_work.Dealer_ID = dealer.DEALER_ID AND dealer_work.Service_Type ='TestDrive'
                        INNER JOIN purchase_plan AS purchase_plan ON purchase_plan.id = test_drive.purchase_plan_id
                        WHERE test_drive.deleted_flag IS NULL AND test_drive.id = N'{1}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, test_drive_id)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            data.code = row["code"].ToString();
                            data.f_name = row["name"].ToString();
                            data.l_name = row["surname"].ToString();
                            data.contact_mobile = row["contact_number"].ToString();
                            data.email = row["email"].ToString();
                            data.preferred_model = row["preferred_model"].ToString();
                            data.dealer_name = row["dealer_name"].ToString();
                            data.remark = row["remark"].ToString();
                            data.purchase_plan = row["purchase_plan"].ToString();
                            data.branch_name = row["branch_name"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public byte[] ExportExcelTestDrive(TestDriveDetailDataForExportExcel data)
        {
            string filePath = "";
            byte[] fileContent = { };

            filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\TestDrive.xlsx";
            string newFilePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\Test_Drive_" +data.code+".xlsx";
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                using (var package = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        package.Load(stream);
                    }
                    var ws = package.Workbook.Worksheets[1];

                    //ClearDataInExcelFile(ws);

                    ws.Cells[4, 2].Value = data.dealer_name + " (" + data.branch_name + ")";
                    ws.Cells[9, 2].Value = data.f_name;
                    ws.Cells[10, 2].Value = data.l_name;
                    ws.Cells[11, 2].Value = data.contact_mobile;
                    ws.Cells[12, 2].Value = data.email;
                    ws.Cells[13, 2].Value = data.preferred_model;
                    ws.Cells[14, 2].Value = data.purchase_plan;
                    ws.Cells[15, 2].Value = data.remark;

                    FileInfo file = new FileInfo(newFilePath);
                    package.SaveAs(file);


                    fileContent = File.ReadAllBytes(newFilePath);
    
                }
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Test Drive Export Excel For Attach Mail.");
                throw ex;
            }

            return fileContent;
        }

        public void ClearDataInExcelFile(ExcelWorksheet ws)
        {
            ws.Cells[4, 2].Value = "";
            ws.Cells[5, 2].Value = "";
            ws.Cells[6, 2].Value = "";
            ws.Cells[9, 2].Value = "";
            ws.Cells[10, 2].Value = "";
            ws.Cells[11, 2].Value = "";
            ws.Cells[12, 2].Value = "";
            ws.Cells[13, 2].Value = "";
            ws.Cells[14, 2].Value = "";
            ws.Cells[15, 2].Value = "";
            ws.Cells[16, 2].Value = "";

        }
        #endregion

        public async Task<ValidationModel> ValidateRegister(ValidateParameter app_param, string lang)
        {
            ValidationModel value = new ValidationModel();
            value.Success = true;
            try
            {
                ValidationModel.InvalidState state;

                if (string.IsNullOrEmpty(app_param.f_name.ToString()))
                {
                    state = ValidationModel.InvalidState.E301;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (string.IsNullOrEmpty(app_param.l_name.ToString()))
                {
                    state = ValidationModel.InvalidState.E302;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (string.IsNullOrEmpty(app_param.mobile.ToString()))
                {
                    state = ValidationModel.InvalidState.E303;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }
                else if (!RegisterService.IsPhoneNumber(app_param.mobile.ToString().Trim()))
                {
                    state = ValidationModel.InvalidState.E304;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }


                if (string.IsNullOrEmpty(app_param.email.ToString().Trim()))
                {
                    state = ValidationModel.InvalidState.E305;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (!string.IsNullOrEmpty(app_param.email.ToString().Trim()) && !UtilityService.EmailIsValid(app_param.email))
                {
                    state = ValidationModel.InvalidState.E306;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (string.IsNullOrEmpty(app_param.preferred_model_id.ToString().Trim()))
                {
                    state = ValidationModel.InvalidState.E307;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }


                if (app_param.dealer_id == 0)
                {
                    state = ValidationModel.InvalidState.E308;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (string.IsNullOrEmpty(app_param.purchase_plan_id.ToString().Trim()))
                {
                    state = ValidationModel.InvalidState.E309;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                //impossible case.
                if (app_param.default_status == 0)
                {
                    //I set status = waiting for confirm.
                    app_param.default_status = 1;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return value;
        }

    }
}
