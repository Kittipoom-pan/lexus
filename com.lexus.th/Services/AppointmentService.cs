using AppLibrary.Database;
using com.lexus.th;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace com.lexus.th
{
    public class AppointmentService
    {
        private string conn;
        public AppointmentService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }


        public async Task<ServiceAppointmentCreateModel> RegisterAppointment(string v, string p, string token, string language, ValidateParameter appointmentParameter)
        {
            ServiceAppointmentCreateModel value = new ServiceAppointmentCreateModel();


            try
            {
                //Task.Run.SendMail()
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

                    ValidationModel validationRegis = new ValidationModel();
                    validationRegis = await ValidateRegister(appointmentParameter, language);

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
                        _ServiceAppointmentData appointment = new _ServiceAppointmentData();


                        if (appointmentParameter.is_pickup_service)
                        {
                            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                            {
                                string cmd = @"

                         DECLARE @member_id NVARCHAR(50);
                         DECLARE @generate_code NVARCHAR(20);
                         DECLARE @max_id NVARCHAR(10);
                         
                         SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{11}'
                         SELECT @max_id = CONVERT(NVARCHAR(10),count(id)+1) FROM appointment
                         SELECT @generate_code = 'SV' + REPLACE(CONVERT(CHAR(10), GETDATE(), 23),'-','')+'-' + SUBSTRING(CAST((POWER(10, 3) + @max_id) AS NVARCHAR(4)), 2, 3)

                            INSERT INTO appointment
                            (service_appointment_type, code, name, surname, mobile_number, plate_number, vehicle_no, 
                             dealer_id, remark, appointmentdate, appointmenttime_id, created_date, created_user, member_id, status_id,
                             is_pickup_service, latitude, longitude, location_detail, pickup_address,
                             pickup_date, pickup_time_id)
                        VALUES
                           ({0}, @generate_code, N'{1}', N'{2}', N'{3}', N'{4}', N'{5}',
                             {6}, N'{7}', N'{8}', N'{9}', N'{10}', @member_id, @member_id, N'{12}',
                             {13}, N'{14}', N'{15}', N'{16}', N'{17}',
                             N'{18}', N'{19}')
                            
                        SELECT SCOPE_IDENTITY() as id";

                                DateTime datetime_now = DateTime.Now;
                                using (DataTable dt = db.GetDataTableFromCommandText
                                      (string.Format(cmd, appointmentParameter.is_car_owner, appointmentParameter.f_name, appointmentParameter.l_name, appointmentParameter.mobile, appointmentParameter.plate_no, appointmentParameter.vehicle_no,
                                       //appointmentParameter.dealer_id, appointmentParameter.remark, appointmentParameter.appt_date, appointmentParameter.appt_time_id, datetime_now, token, appointmentParameter.status_id,
                                       //Convert.ToByte(appointmentParameter.is_pickup_service), appointmentParameter.latitude, appointmentParameter.longitude, appointmentParameter.location_detail, appointmentParameter.pickup_address,
                                       //appointmentParameter.pickup_date, appointmentParameter.pickup_time_id)))
                                       appointmentParameter.dealer_id, appointmentParameter.remark, appointmentParameter.appt_date, appointmentParameter.appt_time_id, datetime_now, token, appointmentParameter.status_id,
                                       Convert.ToByte(appointmentParameter.is_pickup_service), appointmentParameter.latitude, appointmentParameter.longitude, appointmentParameter.location_detail, appointmentParameter.pickup_address,
                                       appointmentParameter.pickup_date, appointmentParameter.pickup_time_id)))
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        appointment.id = Convert.ToInt32(dt.Rows[0]["id"]);
                                        value.success = true;
                                        value.msg = new MsgModel() { code = 200, text = "Success" };
                                        value.ts = DateTimeOffset.Now;
                                        value.data = appointment;
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                            {
                                string cmd = @"

                         DECLARE @member_id NVARCHAR(50);
                         DECLARE @generate_code NVARCHAR(20);
                         DECLARE @max_id NVARCHAR(10);
                         
                         SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{11}'
                         SELECT @max_id = CONVERT(NVARCHAR(10),count(id)+1) FROM appointment
                         SELECT @generate_code = 'SV' + REPLACE(CONVERT(CHAR(10), GETDATE(), 23),'-','')+'-' + SUBSTRING(CAST((POWER(10, 3) + @max_id) AS NVARCHAR(4)), 2, 3)

                        INSERT INTO appointment
                            (service_appointment_type, code, name, surname, mobile_number, plate_number, vehicle_no, 
                             dealer_id, remark, appointmentdate, appointmenttime_id, created_date, created_user, member_id, status_id)
                        VALUES
                            ({0}, @generate_code, N'{1}', N'{2}', N'{3}', N'{4}', N'{5}',
                             {6}, N'{7}', N'{8}', N'{9}', N'{10}', @member_id, @member_id, N'{12}')
                            
                        SELECT SCOPE_IDENTITY() as id";

                                DateTime datetime_now = DateTime.Now;
                                using (DataTable dt = db.GetDataTableFromCommandText
                                      (string.Format(cmd, appointmentParameter.is_car_owner, appointmentParameter.f_name, appointmentParameter.l_name, appointmentParameter.mobile, appointmentParameter.plate_no, appointmentParameter.vehicle_no,
                                       appointmentParameter.dealer_id, appointmentParameter.remark, appointmentParameter.appt_date, appointmentParameter.appt_time_id, datetime_now, token, appointmentParameter.status_id)))
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        appointment.id = Convert.ToInt32(dt.Rows[0]["id"]);
                                        value.success = true;
                                        value.msg = new MsgModel() { code = 200, text = "Success" };
                                        value.ts = DateTimeOffset.Now;
                                        value.data = appointment;
                                    }
                                }
                            }
                        }


                        bool insert_type_of_service = await this.AddTypeOfService(appointmentParameter.type_of_service_ids, appointment.id, token);

                        if (!insert_type_of_service)
                        {
                            value.success = false;
                            value.msg = new MsgModel() { code = 0, text = "Fail on insert_type_of_service" };
                            value.ts = DateTimeOffset.Now;
                        }

                        if (value.success)
                        {
                            TreasureDataController treasureCtl = new TreasureDataController();
                            treasureCtl.AddEventAppointment(value.data.id, appointmentParameter.is_pickup_service);
                        }


                        Task.Run(async () => { SendMail(appointment.id, token, language, appointmentParameter.is_pickup_service); });


                    }

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }



            return value;
        }


        public async Task<ServiceAppointmentModel> GetAppointmentHistory(string v, string p, string token, string language)
        {
            ServiceAppointmentModel value = new ServiceAppointmentModel();

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

                    value.data = await GetAppointment(token, language);
                    value.success = true;
                    value.msg = new MsgModel()
                    {
                        code = 0,
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



        public async Task<ServiceAppointmentDetailModel> GetAppointmentDetail(string v, string p, string token, string language, int appointment_id)
        {
            ServiceAppointmentDetailModel value = new ServiceAppointmentDetailModel();

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

                    value.data =await GetAppointmentOnDetail(token, appointment_id, language);
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


        public TreasureDataAddEventServiceAppointmentRequest GetTreasureDataAppointment(int id, bool is_pickup_service)
        {
            TreasureDataAddEventServiceAppointmentRequest request = new TreasureDataAddEventServiceAppointmentRequest();
            try
            {
                if (is_pickup_service)
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
                            INNER JOIN utility_generate_time_of_day as apt_time ON apm.pickup_time_id = apt_time.id

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
                else
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


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return request;
        }

        #region Sub Method

        private async Task<AppointmentHistoryData> GetAppointment(string token, string lang)
        {
            AppointmentHistoryData value = new AppointmentHistoryData();
            value.service_appointment_datas = new List<_ServiceAppointmentHistoryDetailData>();
            // List<_ServiceAppointmentHistoryData> value = new  List<_ServiceAppointmentHistoryData>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                                              DECLARE @lang NVARCHAR(10) = N'{0}';
                                              
                                              SELECT TOP 20 
	                                            appointment.id as id,
	                                            appointment.service_appointment_type as service_appointment_type,
	                                            appointment.name as name,
	                                            appointment.surname as surname,
	                                            appointment.mobile_number as mobile_number,
	                                            appointment.plate_number as plate_number,
	                                            dealer.dealer_id as dealer_id,
                                                CASE
													WHEN @lang = 'TH' THEN dealer.dealer_name_th
													WHEN @lang = 'EN' THEN dealer.DEALER_NAME
												END as dealer_name,
                                                CASE
													WHEN @lang = 'TH' THEN dealer.branch_name_th
													WHEN @lang = 'EN' THEN dealer.BRANCH_NAME
												END as dealer_branch_name,
	                                            dealer.DEALER_MOBILE as dealer_mobile,
                                                CASE 
                                                        WHEN appointment.call_center_remark IS NOT NULL AND appointment.call_center_remark <>'' THEN call_center_remark
                                                        ELSE appointment.remark
                                                END AS remark,
                                                FORMAT(appointment.confirm_date, 'dd/MM/yyyy')  as confirm_date,
                                                FORMAT(appointment.confirm_time, N'hh\:mm') as confirm_time,
	                                            appointment.appointmentdate as appointmentdate,
                                                SUBSTRING( convert(varchar, time_of_day.start_hour,108),1,5) as appointmenttime_start,
	                                            SUBSTRING( convert(varchar, time_of_day.end_hour,108),1,5) as appointmenttime_end, 
                                                appointment.pickup_date as pickup_date,
                                                SUBSTRING( convert(varchar, time_of_day_pick_up.start_hour,108),1,5) as pickup_time_start,
	                                            SUBSTRING( convert(varchar, time_of_day_pick_up.end_hour,108),1,5) as pickup_time_end, 
	                                            appointment.footer_remark_description as footer_remark_description,
	                                            appointment.created_date as created_date,
	                                            appointment.created_user as created_user,
	                                            appointment.updated_date as updated_date,
	                                            appointment.updated_user as updated_user,
	                                            appointment.vehicle_no as vehicle_no,
	                                            status.id as status_id,
	                                            status.value_name as status_name,
	                                            appointment.member_id as member_id,
                                                CONVERT(varchar(5),DAY(appointment.created_date)) + ' '+ {2} +' ' + CONVERT(varchar(5), YEAR(appointment.created_date)) as date_display,
                                                dealer_work.CallCenterMobile as call_center_mobile,
                                                appointment.pickup_address as pickup_address,
												appointment.location_detail as location_detail,
                                                appointment.is_pickup_service as is_pickup_service
	
	                                          FROM appointment
                                                INNER JOIN [T_CUSTOMER_TOKEN] AS cus_token ON cus_token.MEMBERID = appointment.member_id
											    INNER JOIN [status] AS status ON status.id = appointment.status_id
                                        LEFT JOIN [dbo].[utility_generate_time_of_day] AS time_of_day_pick_up ON time_of_day_pick_up.id = appointment.pickup_time_id
												LEFT JOIN [dbo].[utility_generate_time_of_day] AS time_of_day ON time_of_day.id = appointment.appointmenttime_id
	                                            INNER JOIN T_DEALER AS dealer ON dealer.DEALER_ID = appointment.dealer_id
                                                INNER JOIN T_DEALER_WORKINGTIME AS dealer_work ON dealer_work.Dealer_ID = dealer.DEALER_ID AND dealer_work.Service_Type ='ServiceAppointment'
                                              WHERE cus_token.TOKEN_NO = N'{1}' 
                                              ORDER BY appointment.created_date DESC";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, token, "{fn MONTHNAME(appointment.created_date)}")))
                    {
                        //value.data = new _ServiceAppointmentHistoryData();
                        // value.data = new List<_ServiceAppointmentHistoryData>();

                        foreach (DataRow row in dt.Rows)
                        {
                            _ServiceAppointmentHistoryDetailData data = new _ServiceAppointmentHistoryDetailData();
                            data.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                            var branch_name = row["dealer_branch_name"] != DBNull.Value ? row["dealer_branch_name"].ToString().Trim() : "";
                            data.dealer_name = row["dealer_name"].ToString();
                            if (!string.IsNullOrEmpty(branch_name))
                            {
                                data.dealer_name = data.dealer_name + " (" + branch_name + ")";
                            }
                            data.dealer_mobile = row["dealer_mobile"].ToString();
                            data.dealer_id = row["dealer_id"] != DBNull.Value ? Convert.ToInt32(row["dealer_id"]) : 0;
                            data.contact_mobile = row["mobile_number"].ToString();
                            data.status = row["status_name"].ToString();
                            data.remark = row["remark"].ToString();
                            data.car = GetCars(token, row["vehicle_no"].ToString());
                            data.type_of_service = GetTypeOfService(data.id, lang);
                            data.f_name = row["name"].ToString();
                            data.l_name = row["surname"].ToString();

                            data.date = row["date_display"].ToString();
                            data.appointment_call_center = row["call_center_mobile"].ToString();

                            var confirm_date = row["confirm_date"].ToString();
                            var confirm_time = row["confirm_time"].ToString();
                            var status = (ProcessStatus)Convert.ToInt32(row["status_id"].ToString());

                            if (status == ProcessStatus.waiting_confirm || status == ProcessStatus.cancel)
                            {
                                data.appt_date = row["appointmentdate"] == DBNull.Value ? "" : Convert.ToDateTime(row["appointmentdate"]).Date.ToString("dd/MM/yyyy");
                                data.appt_time = (row["appointmenttime_start"] == DBNull.Value ? "" : row["appointmenttime_start"].ToString()) + " - " + (row["appointmenttime_end"] == DBNull.Value ? "" : row["appointmenttime_end"].ToString());
                            }
                            else if (status == ProcessStatus.confirm || status == ProcessStatus.completed)
                            {
                                data.appt_date = confirm_date;
                                data.appt_time = confirm_time;
                            }


                            data.is_pickup_service = Convert.ToBoolean(row["is_pickup_service"]);
                            data.pickup_date = row["pickup_date"] == DBNull.Value ? "" : Convert.ToDateTime(row["pickup_date"]).Date.ToString("dd/MM/yyyy");
                            data.pickup_time = (row["pickup_time_start"] == DBNull.Value ? "" : row["pickup_time_start"].ToString()) + " - " + (row["pickup_time_end"] == DBNull.Value ? "" : row["pickup_time_end"].ToString());
                            data.pickup_address = row["pickup_address"] == DBNull.Value ? "" : row["pickup_address"].ToString();
                            data.location_detail = row["location_detail"] == DBNull.Value ? "" : row["location_detail"].ToString();

                            value.service_appointment_datas.Add(data);
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

        private async Task<AppointmentDetailData> GetAppointmentOnDetail(string token, int appointment_id, string lang)
        {
            AppointmentDetailData value = new AppointmentDetailData();
            value.service_appointment_data = new _ServiceAppointmentDetailData();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"               
                                               DECLARE @lang NVARCHAR(10) = N'{2}'
                                              SELECT 
	                                            appointment.id as id,
	                                            appointment.service_appointment_type as service_appointment_type,
	                                            appointment.name as name,
	                                            appointment.surname as surname,
	                                            appointment.mobile_number as mobile_number,
	                                            appointment.plate_number as plate_number,
	                                            dealer.dealer_id as dealer_id,
                                                CASE
													WHEN @lang = 'TH' THEN dealer.dealer_name_th
													WHEN @lang = 'EN' THEN dealer.DEALER_NAME
												END as dealer_name,
                                                CASE
													WHEN @lang = 'TH' THEN dealer.branch_name_th
													WHEN @lang = 'EN' THEN dealer.BRANCH_NAME
												END as dealer_branch_name,
	                                            dealer.DEALER_MOBILE as dealer_mobile,
                                                CASE WHEN appointment.call_center_remark IS NOT NULL AND appointment.call_center_remark <>'' THEN appointment.call_center_remark
                                                ELSE appointment.remark
                                                END AS remark,
	                                            appointment.appointmentdate as appointmentdate,
                                                FORMAT(appointment.confirm_date, 'dd/MM/yyyy')  as confirm_date,
                                                FORMAT(appointment.confirm_time, N'hh\:mm') as confirm_time,

	                                            SUBSTRING(convert(varchar, time_of_day.start_hour,108),1,5) as appointmenttime_start,
	                                            SUBSTRING(convert(varchar, time_of_day.end_hour,108),1,5) as appointmenttime_end,

                                                appointment.pickup_date as pickup_date,
                                                SUBSTRING(convert(varchar, time_of_day_pick_up.start_hour,108),1,5) as pickup_time_start,
	                                            SUBSTRING(convert(varchar, time_of_day_pick_up.end_hour,108),1,5) as pickup_time_end, 

	                                            appointment.footer_remark_description as footer_remark_description,
	                                            appointment.created_date as created_date,
	                                            appointment.created_user as created_user,
	                                            appointment.updated_date as updated_date,
	                                            appointment.updated_user as updated_user,
	                                            appointment.vehicle_no as vehicle_no,
	                                            status.id as status_id,
	                                            status.value_name as status_name,
	                                            appointment.member_id as member_id,
                                                CONVERT(varchar(5),DAY(appointment.created_date)) + ' '+{3} +' ' + CONVERT(varchar(5), YEAR(appointment.created_date)) as date_display,
	                                             
                                                appointment.pickup_address as pickup_address,
												appointment.location_detail as location_detail,
                                                appointment.is_pickup_service as is_pickup_service

	                                            FROM appointment
                                                INNER JOIN [T_CUSTOMER_TOKEN] AS cus_token ON cus_token.MEMBERID = appointment.member_id
											    INNER JOIN [status] AS status ON status.id = appointment.status_id
                                                LEFT JOIN [dbo].[utility_generate_time_of_day] AS time_of_day_pick_up ON time_of_day_pick_up.id = appointment.pickup_time_id
												LEFT JOIN [dbo].[utility_generate_time_of_day] AS time_of_day ON time_of_day.id = appointment.appointmenttime_id
	                                            LEFT JOIN T_DEALER AS dealer ON dealer.DEALER_ID = appointment.dealer_id
                                              WHERE cus_token.TOKEN_NO = N'{0}' AND appointment.id=N'{1}'";



                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, appointment_id, lang, "{fn MONTHNAME(appointment.created_date)}")))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            value.service_appointment_data.id = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                            value.service_appointment_data.dealer_mobile = row["dealer_mobile"].ToString();
                            value.service_appointment_data.dealer_id = row["dealer_id"] != DBNull.Value ? Convert.ToInt32(row["dealer_id"]) : 0;
                            value.service_appointment_data.contact_mobile = row["mobile_number"].ToString();
                            value.service_appointment_data.status = row["status_name"].ToString();
                            value.service_appointment_data.remark = row["remark"].ToString();
                            value.service_appointment_data.car = GetCars(token, row["vehicle_no"].ToString());
                            value.service_appointment_data.type_of_service = GetTypeOfService(value.service_appointment_data.id, lang);
                            value.service_appointment_data.f_name = row["name"].ToString();
                            value.service_appointment_data.l_name = row["surname"].ToString();
                            value.service_appointment_data.date = row["date_display"].ToString();
                            var branch_name = row["dealer_branch_name"] != DBNull.Value ? row["dealer_branch_name"].ToString().Trim() : "";
                            value.service_appointment_data.dealer_name = row["dealer_name"].ToString();
                            if (!string.IsNullOrEmpty(branch_name))
                            {
                                value.service_appointment_data.dealer_name = value.service_appointment_data.dealer_name + " (" + branch_name + ")";
                            }

                            var confirm_date = row["confirm_date"].ToString();
                            var confirm_time = row["confirm_time"].ToString();
                            var status = (ProcessStatus)Convert.ToInt32(row["status_id"].ToString());

                            if (status == ProcessStatus.waiting_confirm || status == ProcessStatus.cancel)
                            {
                                value.service_appointment_data.appt_date = Convert.ToDateTime(row["appointmentdate"]).Date.ToString("dd/MM/yyyy");
                                value.service_appointment_data.appt_time = (row["appointmenttime_start"] == DBNull.Value ? "" : row["appointmenttime_start"].ToString()) + " - " + (row["appointmenttime_end"] == DBNull.Value ? "" : row["appointmenttime_end"].ToString());
                            }
                            else if (status == ProcessStatus.confirm || status == ProcessStatus.completed)
                            {
                                value.service_appointment_data.appt_date = confirm_date;
                                value.service_appointment_data.appt_time = confirm_time;
                            }

                            value.service_appointment_data.is_pickup_service = Convert.ToBoolean(row["is_pickup_service"]);
                            value.service_appointment_data.pickup_date = row["pickup_date"] == DBNull.Value ? "" : Convert.ToDateTime(row["pickup_date"]).Date.ToString("dd/MM/yyyy");
                            value.service_appointment_data.pickup_time = (row["pickup_time_start"] == DBNull.Value ? "" : row["pickup_time_start"].ToString()) + " - " + (row["pickup_time_end"] == DBNull.Value ? "" : row["pickup_time_end"].ToString());
                            value.service_appointment_data.pickup_address = row["pickup_address"] == DBNull.Value ? "" : row["pickup_address"].ToString();
                            value.service_appointment_data.location_detail = row["location_detail"] == DBNull.Value ? "" : row["location_detail"].ToString();
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

        public async Task<AppointmentDetailDataForSendEmail> GetDetailForEmail(int appointment_id, string token, string lang)
        {
            AppointmentDetailDataForSendEmail value = new AppointmentDetailDataForSendEmail();
            value.car_owner_data =await GetCarOwnerDetailForEmail(appointment_id, token, lang);
            value.appointment_data =await GetAppointmentDetailForEmail(appointment_id, token, lang);

            return value;
        }



        public async Task<AppointmentDetailDataForSendEmail_Appointment> GetAppointmentDetailForEmail(int appointment_id, string token, string lang)
        {
            AppointmentDetailDataForSendEmail_Appointment appointment_data = new AppointmentDetailDataForSendEmail_Appointment();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "DECLARE @lang NVARCHAR(2); " +
                                 "DECLARE @tcap_call_center_email NVARCHAR(300); " +
                                 "SET @lang = N'" + lang + "' " +
                                 "SET @tcap_call_center_email = (SELECT data_config FROM system_config WHERE name = 'call_center_email') " +

                                 "SELECT " +
                                   "appointment.id as id, " +
                                   "appointment.code as code, " +
                                   "appointment.service_appointment_type as service_appointment_type, " +
                                   "appointment.name as name, " +
                                   "appointment.surname as surname, " +
                                   "appointment.mobile_number as mobile_number, " +
                                   "appointment.plate_number as plate_number, " +
                                   "dealer.dealer_id as dealer_id, " +
                                   "CASE " +
                                   "WHEN @lang = 'TH' THEN dealer.dealer_name_th " +
                                   "WHEN @lang = 'EN' THEN dealer.DEALER_NAME " +
                                   "END as dealer_name, " +
                                   "dealer.DEALER_MOBILE as dealer_mobile, " +
                                   "dealer.BRANCH_NAME as branch_name, " +
                                   "appointment.remark as remark, " +
                                   "FORMAT (appointment.appointmentdate, 'dd MMM yyyy') as appointmentdate, " +
                                   "SUBSTRING(convert(varchar, time_of_day.start_hour,108),1,5) as appointmenttime_start, " +
                                   "SUBSTRING(convert(varchar, time_of_day.end_hour,108),1,5) as appointmenttime_end,  " +
                                   "appointment.footer_remark_description as footer_remark_description, " +
                                   "FORMAT (appointment.created_date, 'dd MMM yyyy HH:mm:ss tt') as created_date, " +
                                   "appointment.created_user as created_user, " +
                                   "appointment.updated_date as updated_date, " +
                                   "appointment.updated_user as updated_user, " +
                                   "appointment.vehicle_no as vehicle_no, " +
                                   "appointment.member_id as member_id, " +

                                   "appointment.pickup_address as pickup_address, "+
                                   "appointment.location_detail as location_detail, "+
                                   "appointment.is_pickup_service as is_pickup_service, " +
                                   "FORMAT (appointment.pickup_date, 'dd MMM yyyy') as pickup_date, " +
                                   //"CONVERT(varchar(5), DAY(appointment.pickup_date)) + ' ' + CONVERT(NVARCHAR(3), { fn MONTHNAME(appointment.pickup_date)}) +' ' + CONVERT(varchar(5), YEAR(appointment.pickup_date)) as pickup_date, " +
                                   //"appointment.pickup_date as pickup_date, " +
                                   //"appointment.pickup_date as pickup_date, "+
                                   "SUBSTRING( convert(varchar, time_of_day_pick_up.start_hour, 108), 1, 5) as pickup_time_start, "+
	                               "SUBSTRING(convert(varchar, time_of_day_pick_up.end_hour, 108), 1, 5) as pickup_time_end, "+
                                   "appointment.latitude as latitude, " +
                                   "appointment.longitude as longitude, " +

                                   "model.MODEL as car_model, " +
                                   "appointment.vehicle_no as vin, " +
                                   "CONVERT(varchar(5),DAY(appointment.created_date)) + ' '+{fn MONTHNAME(appointment.created_date)} +' ' + CONVERT(varchar(5), YEAR(appointment.created_date)) as date_display, " +
                                   "STUFF((SELECT ',' + " +
                                                    "CASE " +
                                                      "WHEN @lang = 'TH' THEN type_of_service.name_th " +
                                                      "WHEN @lang = 'EN' THEN type_of_service.name_en " +
                                                    "END " +
                                           "FROM appointment AS appontment_ " +
                                             "INNER JOIN appointment_type_of_service AS app_t_o_s ON app_t_o_s.appointment_id = appontment_.id AND app_t_o_s.deleted_flag IS NULL AND app_t_o_s.delete_date IS NULL " +
                                             "INNER JOIN type_of_service AS type_of_service ON type_of_service.id = app_t_o_s.type_of_service_id " +
                                           "WHERE appontment_.id = appointment.id FOR XML PATH('')), 1, 1, '') AS type_of_service_name, " +
                                  "dealer_desc.CallCenterEmail AS call_center_email, " +
                                  "@tcap_call_center_email AS tcap_call_center_email " +
                                  "FROM appointment " +
                                  "INNER JOIN [T_CUSTOMER_TOKEN] AS cus_token ON cus_token.MEMBERID = appointment.member_id " +
                                  "INNER JOIN T_CUSTOMER AS cus ON cus.MEMBERID = appointment.member_id AND cus.DEL_DT IS NULL AND DEL_FLAG IS NULL " +
                                  "INNER JOIN T_CUSTOMER_CAR AS cus_car ON cus_car.VIN = appointment.vehicle_no " +
                                  "INNER JOIN T_CAR_MODEL AS model ON model.MODEL_ID = cus_car.MODEL_ID " +
                                  "INNER JOIN T_DEALER AS dealer ON dealer.DEALER_ID = appointment.dealer_id AND dealer.DEL_FLAG IS NULL AND dealer.DEL_DT IS NULL " +
                                  "INNER JOIN appointment_type_of_service AS app_type_of_service ON app_type_of_service.appointment_id = appointment.id " +
                                  "INNER JOIN type_of_service AS type_of_service ON type_of_service.id = app_type_of_service.type_of_service_id " +
                                  "LEFT JOIN[dbo].[utility_generate_time_of_day] AS time_of_day_pick_up ON time_of_day_pick_up.id = appointment.pickup_time_id "+
                                  "INNER JOIN [dbo].[utility_generate_time_of_day] AS time_of_day ON time_of_day.id = appointment.appointmenttime_id " +
                                  "LEFT JOIN [dbo].[T_DEALER_WORKINGTIME] AS dealer_desc ON dealer_desc.Dealer_ID = appointment.dealer_id AND dealer_desc.Service_Type = 'ServiceAppointment' " +
                                 "WHERE cus_token.TOKEN_NO = N'" + token + "' " +
                                 "AND appointment.id=N'" + appointment_id + "' ";


                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            appointment_data.service_appointment_type = Convert.ToInt16(row["service_appointment_type"].ToString());
                            appointment_data.id = Convert.ToInt16(row["id"].ToString());
                            appointment_data.code = row["code"].ToString();
                            appointment_data.f_name = row["name"].ToString();
                            appointment_data.l_name = row["surname"].ToString();
                            appointment_data.contact_mobile = row["mobile_number"].ToString();
                            appointment_data.vin = row["vehicle_no"].ToString();
                            appointment_data.plate_number = row["plate_number"].ToString();
                            appointment_data.car_model = row["car_model"].ToString();
                            appointment_data.type_of_service = row["type_of_service_name"].ToString();
                            appointment_data.dealer_name = row["dealer_name"].ToString();
                            appointment_data.branch_name = row["branch_name"].ToString();
                            appointment_data.remark = row["remark"].ToString();
                            appointment_data.appt_date = row["appointmentdate"].ToString();
                            appointment_data.appt_time = (row["appointmenttime_start"] == DBNull.Value ? "" : row["appointmenttime_start"].ToString()) + " - " + (row["appointmenttime_end"] == DBNull.Value ? "" : row["appointmenttime_end"].ToString());
                            appointment_data.created_date = row["created_date"].ToString();
                            appointment_data.call_center_email = row["call_center_email"].ToString();
                            appointment_data.tcap_call_center_email = row["tcap_call_center_email"].ToString();

                            appointment_data.is_pickup_service = Convert.ToBoolean(row["is_pickup_service"]);
                            //appointment_data.pickup_date = row["pickup_date"] == DBNull.Value ? "" : Convert.ToDateTime(row["pickup_date"]).Date.ToString();
                            appointment_data.pickup_date = row["pickup_date"].ToString();
                            appointment_data.pickup_time_id = (row["pickup_time_start"] == DBNull.Value ? "" : row["pickup_time_start"].ToString()) + " - " + (row["pickup_time_end"] == DBNull.Value ? "" : row["pickup_time_end"].ToString());
                            appointment_data.pickup_address = row["pickup_address"] == DBNull.Value ? "" : row["pickup_address"].ToString();
                            appointment_data.location_detail = row["location_detail"] == DBNull.Value ? "" : row["location_detail"].ToString();
                            appointment_data.latitude =row["latitude"].ToString();
                            appointment_data.longitude = row["longitude"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return appointment_data;
        }

        public async Task<AppointmentDetailDataForSendEmail_CarOwner> GetCarOwnerDetailForEmail(int appointment_id, string token, string lang)
        {
            AppointmentDetailDataForSendEmail_CarOwner car_owner_data = new AppointmentDetailDataForSendEmail_CarOwner();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "DECLARE @member_id NVARCHAR(50); " +
                                 "SET @member_id = (select top 1 MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = N'" + token + "') " +

                                 "SELECT " +
                                 "FNAME, LNAME, MOBILE " +
                                  "FROM [dbo].[T_CUSTOMER_CAR_OWNER] " +
                                  "WHERE MEMBERID = @member_id ";

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            car_owner_data.f_name = row["FNAME"].ToString();
                            car_owner_data.l_name = row["LNAME"].ToString();
                            car_owner_data.contact_mobile = row["MOBILE"].ToString();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return car_owner_data;
        }



        private CarModel GetCars(string token, string vin)
        {
            CarModel value = new CarModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
                           SELECT top 1 
                                  CM.MODEL	
	                             ,CAR.VIN
	                             ,CAR.PLATE_NO
	                             ,CM.IMAGE
                                 ,CAR.DEALER
                                 ,CAR.RS_Date
                           FROM T_CUSTOMER_CAR CAR
                           INNER JOIN T_CAR_MODEL CM ON CM.MODEL_ID = CAR.MODEL_ID
                           INNER JOIN T_CUSTOMER CUS ON CUS.MEMBERID = CAR.MEMBERID
                           INNER JOIN T_CUSTOMER_TOKEN TKN ON TKN.MEMBERID = CUS.MEMBERID
                           WHERE TKN.TOKEN_NO = N'{0}' AND VIN = N'{1}' ", token, vin);

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        DataRow dr = dt.Rows[0];

                        value.model = dr["MODEL"].ToString();
                        value.vin = dr["VIN"].ToString();
                        value.plate_no = dr["PLATE_NO"].ToString();
                        value.image = dr["IMAGE"].ToString();
                        value.dealer = dr["DEALER"].ToString();
                        value.rs_date = (dr["RS_Date"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["RS_Date"]).ToString("dd/MM/yyyy");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public List<TypeOfServiceModel> GetTypeOfService(int appointment_id, string lang)
        {
            List<TypeOfServiceModel> list = new List<TypeOfServiceModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
                            
                           DECLARE @lang NVARCHAR(10) = N'{1}';
                        
                           SELECT type_of_service.id AS type_of_service_id,
                                  CASE
                                  	WHEN @lang = 'TH' THEN type_of_service.name_th
                                  	WHEN @lang = 'EN' THEN type_of_service.name_en
                                  END AS type_of_service_name   
                           FROM  [dbo].[appointment_type_of_service] AS mapping_tbl
                           INNER JOIN [dbo].[type_of_service] AS type_of_service 
                                 ON type_of_service.id = mapping_tbl.type_of_service_id 
                                 AND type_of_service.is_active = 1
                           WHERE mapping_tbl.appointment_id = N'{0}' ", appointment_id, lang);
                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(new TypeOfServiceModel
                            {
                                id = Convert.ToInt16(row["type_of_service_id"]),
                                name = row["type_of_service_name"].ToString(),
                                type_of_service_detail = GetTypeOfServiceDetail(appointment_id, Convert.ToInt16(row["type_of_service_id"]))
                            });
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

        public List<TypeOfServiceDetail> GetTypeOfServiceDetail(int appointment_id, int type_of_service_id)
        {
            List<TypeOfServiceDetail> list = new List<TypeOfServiceDetail>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
                           SELECT type_of_service_detail.id AS type_of_service_detail_id, type_of_service_detail.name_en AS type_of_service_detail_name
                           FROM  [dbo].[appointment_type_of_service] AS mapping_tbl
                           INNER JOIN [dbo].[type_of_service_detail] AS type_of_service_detail 
                                 ON type_of_service_detail.id = mapping_tbl.type_of_service_detail_id 
                                 AND type_of_service_detail.is_active = 1
                           WHERE mapping_tbl.appointment_id = N'{0}'  
                                 AND mapping_tbl.type_of_service_id = N'{1}' ", appointment_id, type_of_service_id);
                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(new TypeOfServiceDetail
                            {
                                id = Convert.ToInt16(row["type_of_service_detail_id"]),
                                name = row["type_of_service_detail_name"].ToString()
                            });
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

        public async Task<bool> AddTypeOfService(List<int> type_of_service_ids, int appointment_id, string token)
        {
            int[] data = type_of_service_ids.ToArray();
            try
            {
                if (data.Length > 0 && appointment_id != 0)
                {
                    int insertSuccess = 0;
                    for (int n = 0; n < data.Length; n++)
                    {
                        int type_of_service_id = data[n];
                        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                        {
                            string cmd = @"

                        DECLARE @member_id NVARCHAR(50);
                        SELECT @member_id = MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{3}'
                        
                        INSERT INTO [dbo].[appointment_type_of_service]
                        (appointment_id, type_of_service_id, created_date, created_user)
                        VALUES
                        ('{0}', '{1}', '{2}', @member_id)
                        SELECT SCOPE_IDENTITY() AS id";

                            DateTime datetime_now = DateTime.Now;
                            using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, appointment_id, type_of_service_id, datetime_now, token)))
                            {

                                if (dt.Rows.Count > 0)
                                {
                                    insertSuccess++;
                                    //value.success = true;
                                    //value.msg = new MsgModel() { code = 0, text = "Success" };
                                    //value.ts = DateTimeOffset.Now;
                                }
                            }
                        }
                        if (insertSuccess == data.Length)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return false;
        }

        //private DealerModel GetDealerName(int dealer_id)
        //{
        //    DealerModel dealer = new DealerModel();

        //    try
        //    {
        //        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //        {
        //            string cmd = string.Format(@"
        //                                SELECT DEALER_ID, DEALER_NAME, DEALER_MOBILE FROM T_DEALER WHERE DEALER_ID = N'{0}'", dealer_id);

        //            using (DataTable dt = db.GetDataTableFromCommandText(cmd))
        //            {
        //                if (dt.Rows.Count > 0)
        //                {
        //                    //value.data.id = Convert.ToInt16(dt.Rows[0]["id"]);
        //                    dealer.id = Convert.ToInt32(dt.Rows[0]["DEALER_ID"]);
        //                    dealer.name = dt.Rows[0]["DEALER_NAME"].ToString();
        //                    dealer.mobile = dt.Rows[0]["DEALER_MOBILE"].ToString();
        //                }
        //            }


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return dealer;
        //}

        public AppointmentDetailDataForExportExcel GetDataForExportExcel(int appointment_id, string token, string language, bool is_pickup_service)
        {
            AppointmentDetailDataForExportExcel data = new AppointmentDetailDataForExportExcel();
            try
            {
                if (is_pickup_service)
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {

                        string cmd = @"DECLARE @lang NVARCHAR(2); " +
                            "SET @lang = '" + language + "'; " +
                            "SELECT " +
                            "appointment.code as code, " +
                            "appointment.name as name, " +
                            "appointment.surname as surname, " +
                            "appointment.mobile_number as mobile_number, " +
                            "appointment.plate_number as plate_number, " +
                            "CASE " +
                            "WHEN @lang = 'TH' THEN dealer.dealer_name_th " +
                            "WHEN @lang = 'EN' THEN dealer.DEALER_NAME " +
                            "END as dealer_name, " +
                            "dealer.BRANCH_NAME as branch_name, " +
                            "appointment.pickup_address as pickup_address, " +
                            "appointment.location_detail as location_detail, " +
                            "appointment.is_pickup_service as is_pickup_service, " +

                            "CONVERT(varchar(5), DAY(appointment.pickup_date)) + ' ' + CONVERT(NVARCHAR(3), { fn MONTHNAME(appointment.pickup_date)}) +' ' + CONVERT(varchar(5), YEAR(appointment.pickup_date)) as pickup_date, " +

                            "SUBSTRING( convert(varchar, time_of_day_pick_up.start_hour, 108), 1, 5) as pickup_time_start, " +
                            "SUBSTRING(convert(varchar, time_of_day_pick_up.end_hour, 108), 1, 5) as pickup_time_end, " +
                            "appointment.latitude as latitude, " +
                            "appointment.longitude as longitude, " +

                            "appointment.remark as remark, " +
                            "CONVERT(varchar(5), DAY(appointment.appointmentdate)) + ' ' + CONVERT(NVARCHAR(3), { fn MONTHNAME(appointment.appointmentdate)}) +' ' + CONVERT(varchar(5), YEAR(appointment.appointmentdate)) as appointmentdate, " +
                            //"SUBSTRING(convert(varchar, time_of_day.start_hour, 108), 1, 5) + ' - ' + SUBSTRING(convert(varchar, time_of_day.end_hour, 108), 1, 5) as appointmenttime, " +
                            "SUBSTRING(convert(varchar, time_of_day.start_hour,108),1,5) as appointmenttime_start, " +
                            "SUBSTRING(convert(varchar, time_of_day.end_hour,108),1,5) as appointmenttime_end,  " +
                            "car_model.Model AS model_name " +
                            "FROM appointment " +
                            "INNER JOIN[T_CUSTOMER_TOKEN] AS cus_token ON cus_token.MEMBERID = appointment.member_id " +
                            "INNER JOIN[dbo].[utility_generate_time_of_day] AS time_of_day ON time_of_day.id = appointment.appointmenttime_id " +
                            "LEFT JOIN[dbo].[utility_generate_time_of_day] AS time_of_day_pick_up ON time_of_day_pick_up.id = appointment.pickup_time_id " +

                            "LEFT JOIN T_DEALER AS dealer ON dealer.DEALER_ID = appointment.dealer_id " +
                            "LEFT JOIN t_customer_car AS cus_car ON cus_car.VIN = appointment.vehicle_no " +
                            "LEFT JOIN t_car_model AS car_model ON car_model.MODEL_ID = cus_car.model_id " +
                            "WHERE cus_token.TOKEN_NO = N'" + token + "' AND appointment.id= N'" + appointment_id + "' ";


                        using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                        {
                            DataRow dr = dt.Rows[0];

                            data.code = dr["code"].ToString();
                            data.f_name = dr["name"].ToString();
                            data.l_name = dr["surname"].ToString();
                            data.contact_mobile = dr["mobile_number"].ToString();
                            data.plate_number = dr["plate_number"].ToString();
                            data.dealer_name = dr["dealer_name"].ToString();
                            data.remark = dr["remark"].ToString();
                            data.appt_date = dr["appointmentdate"].ToString();
                            data.appt_time = (dr["appointmenttime_start"] == DBNull.Value ? "" : dr["appointmenttime_start"].ToString()) + " - " + (dr["appointmenttime_end"] == DBNull.Value ? "" : dr["appointmenttime_end"].ToString());
                            data.car_model = dr["model_name"].ToString();
                            data.branch_name = dr["branch_name"].ToString();

                            data.is_pickup_service = Convert.ToBoolean(dr["is_pickup_service"]);
                            data.pickup_date = dr["pickup_date"].ToString(); 
                            data.pickup_time_id = (dr["pickup_time_start"] == DBNull.Value ? "" : dr["pickup_time_start"].ToString()) + " - " + (dr["pickup_time_end"] == DBNull.Value ? "" : dr["pickup_time_end"].ToString());
                            data.pickup_address = dr["pickup_address"] == DBNull.Value ? "" : dr["pickup_address"].ToString();
                            data.location_detail = dr["location_detail"] == DBNull.Value ? "" : dr["location_detail"].ToString();
                            data.latitude = dr["latitude"].ToString();
                            data.longitude = dr["longitude"].ToString();
                        }
                    }
                }
                else
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {

                        string cmd = @"DECLARE @lang NVARCHAR(2); " +
                            "SET @lang = '" + language + "'; " +
                            "SELECT " +
                            "appointment.code as code, " +
                            "appointment.name as name, " +
                            "appointment.surname as surname, " +
                            "appointment.mobile_number as mobile_number, " +
                            "appointment.plate_number as plate_number, " +
                            "CASE " +
                            "WHEN @lang = 'TH' THEN dealer.dealer_name_th " +
                            "WHEN @lang = 'EN' THEN dealer.DEALER_NAME " +
                            "END as dealer_name, " +
                            "dealer.BRANCH_NAME as branch_name, " +
                            "appointment.remark as remark, " +
                            "CONVERT(varchar(5), DAY(appointment.appointmentdate)) + ' ' + CONVERT(NVARCHAR(3), { fn MONTHNAME(appointment.appointmentdate)}) +' ' + CONVERT(varchar(5), YEAR(appointment.appointmentdate)) as appointmentdate, " +
                            "SUBSTRING(convert(varchar, time_of_day.start_hour, 108), 1, 5) + ' - ' + SUBSTRING(convert(varchar, time_of_day.end_hour, 108), 1, 5) as appointmenttime, " +
                            "car_model.Model AS model_name " +
                            "FROM appointment " +
                            "INNER JOIN[T_CUSTOMER_TOKEN] AS cus_token ON cus_token.MEMBERID = appointment.member_id " +
                            "INNER JOIN[dbo].[utility_generate_time_of_day] AS time_of_day ON time_of_day.id = appointment.appointmenttime_id " +
                            "LEFT JOIN T_DEALER AS dealer ON dealer.DEALER_ID = appointment.dealer_id " +
                            "LEFT JOIN t_customer_car AS cus_car ON cus_car.VIN = appointment.vehicle_no " +
                            "LEFT JOIN t_car_model AS car_model ON car_model.MODEL_ID = cus_car.model_id " +
                            "WHERE cus_token.TOKEN_NO = N'" + token + "' AND appointment.id= N'" + appointment_id + "' ";


                        using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                        {
                            DataRow dr = dt.Rows[0];

                            data.code = dr["code"].ToString();
                            data.f_name = dr["name"].ToString();
                            data.l_name = dr["surname"].ToString();
                            data.contact_mobile = dr["mobile_number"].ToString();
                            data.plate_number = dr["plate_number"].ToString();
                            data.dealer_name = dr["dealer_name"].ToString();
                            data.remark = dr["remark"].ToString();
                            data.appt_date = dr["appointmentdate"].ToString();
                            data.appt_time = dr["appointmenttime"].ToString();
                            data.car_model = dr["model_name"].ToString();
                            data.branch_name = dr["branch_name"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Get Appointment Data For Attach Mail.");
                throw ex;
            }
            return data;
        }

        public List<int> GetTypeOfServiceOnly(int appointment_id)
        {
            List<int> list = new List<int>();
            string type_of_service_id;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"

                                    SELECT  type_of_service.id AS type_of_service_id
                                    FROM appointment
                                    INNER JOIN appointment_type_of_service ON appointment_type_of_service.appointment_id = appointment.id
									INNER JOIN type_of_service ON type_of_service.id = appointment_type_of_service.type_of_service_id
                                    WHERE appointment.id=N'" + appointment_id + "'";

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            int data;
                            data = Convert.ToInt32(dr["type_of_service_id"]);
                            list.Add(data);
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

        //public string GetTypeOfServiceDetailOnly(int appointment_id)
        //{
        //    string data = "";
        //    try
        //    {
        //        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //        {
        //            string cmd = @"

        //                            SELECT  type_of_service_detail.name_en AS type_of_service_detail_name
        //                            FROM appointment
        //                            INNER JOIN appointment_type_of_service ON appointment_type_of_service.appointment_id = appointment.id
        //	INNER JOIN type_of_service ON type_of_service.id = appointment_type_of_service.type_of_service_id
        //	LEFT JOIN type_of_service_detail ON type_of_service_detail.id = appointment_type_of_service.type_of_service_detail_id
        //                            WHERE appointment.id=N'" + appointment_id + "'";

        //            using (DataTable dt = db.GetDataTableFromCommandText(cmd))
        //            {
        //                if (dt.Rows.Count > 0)
        //                {
        //                    DataRow row = dt.Rows[0];
        //                    data = row["type_of_service_detail_name"].ToString();
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return data;
        //}

        public async Task<ValidationModel> ValidateRegister(ValidateParameter app_param, string lang)
        {
            ValidationModel value = new ValidationModel();
            value.Success = true;

            try
            {
                ValidationModel.InvalidState state;

                if (string.IsNullOrEmpty(app_param.f_name.ToString()))
                {
                    state = ValidationModel.InvalidState.E401;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (string.IsNullOrEmpty(app_param.l_name.ToString()))
                {
                    state = ValidationModel.InvalidState.E402;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (string.IsNullOrEmpty(app_param.mobile.ToString()))
                {
                    state = ValidationModel.InvalidState.E403;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }
                //Begin mock
                else if (!RegisterService.IsPhoneNumber(app_param.mobile.ToString().Trim()))
                {
                    state = ValidationModel.InvalidState.E404;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (string.IsNullOrEmpty(app_param.plate_no.ToString()))
                {
                    state = ValidationModel.InvalidState.E408;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (string.IsNullOrEmpty(app_param.vehicle_no.ToString()))
                {
                    state = ValidationModel.InvalidState.E403;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (app_param.dealer_id == 0)
                {
                    state = ValidationModel.InvalidState.E411;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

               
                if (string.IsNullOrEmpty(app_param.appt_date.ToString()))
                {
                    state = ValidationModel.InvalidState.E412;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (app_param.appt_time_id == 0)
                {
                    state = ValidationModel.InvalidState.E413;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }



                if (app_param.type_of_service_ids.ToList().Count <= 0)
                {
                    state = ValidationModel.InvalidState.E410;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (app_param.is_car_owner == 0)
                {
                    state = ValidationModel.InvalidState.E407;
                    return new ValidationModel
                    {
                        Success = false,
                        InvalidCode = ValidationModel.GetInvalidCode(state),
                        InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                    };
                }

                if (app_param.is_pickup_service)
                {
                    if (String.IsNullOrEmpty(app_param.location_detail) && (app_param.latitude == 0 || app_param.longitude == 0))
                    {
                        state = ValidationModel.InvalidState.E414;
                        return new ValidationModel
                        {
                            Success = false,
                            InvalidCode = ValidationModel.GetInvalidCode(state),
                            InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                        };
                    }

                    if (string.IsNullOrEmpty(app_param.pickup_date.ToString()))
                    {
                        state = ValidationModel.InvalidState.E415;
                        return new ValidationModel
                        {
                            Success = false,
                            InvalidCode = ValidationModel.GetInvalidCode(state),
                            InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                        };
                    }

                    if (app_param.pickup_time_id == 0)
                    {
                        state = ValidationModel.InvalidState.E416;
                        return new ValidationModel
                        {
                            Success = false,
                            InvalidCode = ValidationModel.GetInvalidCode(state),
                            InvalidMessage =await ValidationModel.GetInvalidMessageNew(state, lang)
                        };
                    }
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

        #endregion


        public async void SendMail(int appointment_id, string token, string language, bool is_pickup_service)
        {
            List<int> type_of_service_ids = GetTypeOfServiceOnly(appointment_id);
            //string type_of_service_detail_id = GetTypeOfServiceDetailOnly(appointment.id);
            //Make excel file for attachment with mail.
            AppointmentDetailDataForExportExcel exportExcelData = GetDataForExportExcel(appointment_id, token, language, is_pickup_service);
            byte[] attachFile = ExportExcelAppointment(exportExcelData, type_of_service_ids);
            string attachFileName = "";
            if (attachFile.Length > 0)
            {
                attachFileName = "Service_Appointment_" + exportExcelData.code + ".xlsx";
            }

            //Send email to dealer call center.
            SendEmailService sendEmailService = new SendEmailService();
            sendEmailService.SendEmail("Appointment_CallCenter", appointment_id, token, language, attachFile, attachFileName);
        }

        public byte[] ExportExcelAppointment(AppointmentDetailDataForExportExcel data, List<int> type_of_service_ids)
        {
            string filePath = "";
            byte[] fileConent = { };

            string newFilePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\Service_Appointment_" + data.code + ".xlsx";
            if (!data.is_pickup_service)
            {
                if (type_of_service_ids.Count == 1 && Convert.ToInt32(type_of_service_ids[0]) == 1)
                {
                    filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\ServiceAppointment_1.xlsx";
                }
                else if (type_of_service_ids.Count == 1 && Convert.ToInt32(type_of_service_ids[0]) == 2)
                {
                    filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\ServiceAppointment_2.xlsx";
                }
                else if (type_of_service_ids.Count == 2)
                {
                    filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\ServiceAppointment_3.xlsx";
                }
            }
            else
            {
                if (type_of_service_ids.Count == 1 && Convert.ToInt32(type_of_service_ids[0]) == 1)
                {
                    filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\ServiceAppointment_4.xlsx";
                }
                else if (type_of_service_ids.Count == 1 && Convert.ToInt32(type_of_service_ids[0]) == 2)
                {
                    filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\ServiceAppointment_5.xlsx";
                }
                else if (type_of_service_ids.Count == 2)
                {
                    filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcelOriginal"] + "\\ServiceAppointment_6.xlsx";
                }
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

                using (var package = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        package.Load(stream);
                    }
                    var ws = package.Workbook.Worksheets[1];
                    //var ws2 = package.workbook.worksheets[2].selectedrange["a1:a3"];

                    //ClearDataInExcelFile(ws);

                    ws.Cells[4, 2].Value = data.dealer_name + " ("+data.branch_name+")";
                    ws.Cells[9, 2].Value = data.f_name +" "+ data.l_name;
                    ws.Cells[10, 2].Value = data.contact_mobile;
                    ws.Cells[11, 2].Value = data.plate_number;
                    ws.Cells[12, 2].Value = data.car_model;

                    ws.Cells[15, 2].Value = data.appt_date;
                    ws.Cells[16, 2].Value = data.appt_time;
                    ws.Cells[19, 5].Value = data.pickup_address;
                    ws.Cells[19, 7].Value = data.pickup_date;
                    if (!String.IsNullOrEmpty(data.latitude) && !String.IsNullOrEmpty(data.longitude) 
                        && data.latitude!="0" && data.longitude!="0")
                    {
                        ws.Cells[20, 5].Value = "https://www.google.com/maps/search/?api=1&query=" + data.latitude + "," + data.longitude;
                        ws.Cells[20, 5].Hyperlink = new Uri("https://www.google.com/maps/search/?api=1&query=" + data.latitude + "," + data.longitude);
                    }
                    ws.Cells[20, 7].Value = data.pickup_time_id;
                    ws.Cells[21, 5].Value = data.location_detail;
                    ws.Cells[23, 2].Value = data.remark;
              
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
            ws.Cells[17, 2].Value = "";
            ws.Cells[18, 2].Value = "";
        }
    }
}