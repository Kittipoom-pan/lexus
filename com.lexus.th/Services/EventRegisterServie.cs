using AppLibrary.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class EventRegisterServie
    {
        private string conn;
        public EventRegisterServie()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public ServiceEventRegisterModel EventRegister(int event_id, string token, string member_title, string member_name,
            string member_birthdate, string member_email, string member_tel_no, string member_dealer, string follower_title,
            string follower_name, string follower_birthdate, string follower_email, string follower_tel_no, string follower_car_model,
            string follower_year_purchase, string v, string p)
        {
            ServiceEventRegisterModel value = new ServiceEventRegisterModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation2 = syc.CheckSystem(p, v).Result;
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    ValidationModel validation = CheckValidation(event_id, token);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = string.Format(@"
DECLARE
	@MEMBERID NVARCHAR(50) = N'{0}',
	@EVENT_ID INT = {1},
	@member_title NVARCHAR(50) = N'{2}',
	@member_name NVARCHAR(300) = N'{3}',
	@member_birthdate NVARCHAR(50) = N'{4}',
	@member_email NVARCHAR(100) = N'{5}',
	@member_tel_no NVARCHAR(100) = N'{6}',
	@member_dealer NVARCHAR(250) = N'{7}',
	@follower_title NVARCHAR(50) = N'{8}',
	@follower_name NVARCHAR(300) = N'{9}',
	@follower_birthdate NVARCHAR(50) = N'{10}',
	@follower_email NVARCHAR(100) = N'{11}',
	@follower_tel_no NVARCHAR(100) = N'{12}',
	@follower_dealer NVARCHAR(250) = N'{13}',
	@STATUS NVARCHAR(5) = N'{14}',
	@Token NVARCHAR(100) = N'{15}',
    @follower_car_model NVARCHAR(100) = N'{16}',
    @follower_year_purchase NVARCHAR(100) = N'{17}'

SET @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @Token)

INSERT INTO T_EVENTS_REGISTER (MEMBERID,EVENT_ID,member_title,member_name,member_birthdate,member_email,member_tel_no,member_dealer,follower_title,follower_name,follower_birthdate,follower_email,follower_tel_no,follower_dealer,CREATE_DT,[STATUS],follower_car_model,follower_year_purchase)
VALUES (@MEMBERID,@EVENT_ID,@member_title,@member_name,
    CASE LEN(@member_birthdate) WHEN 0 THEN NULL ELSE @member_birthdate END,
    @member_email,@member_tel_no,@member_dealer,@follower_title,@follower_name,
    CASE LEN(@follower_birthdate) WHEN 0 THEN NULL ELSE @follower_birthdate END,
    @follower_email,@follower_tel_no,@follower_dealer,DATEADD(HOUR, 7, GETDATE()),@STATUS,
    @follower_car_model,@follower_year_purchase)"
                            , "", event_id, member_title, member_name, member_birthdate, member_email, member_tel_no, member_dealer, follower_title, follower_name, follower_birthdate, follower_email, follower_tel_no, "", "", token, follower_car_model, follower_year_purchase);

                        db.ExecuteNonQueryFromCommandText(cmd);
                    }

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

        public async Task<ServiceEventRegisterModel> EventRegisterNew(int event_id, string token, string member_title,
            string member_name, string member_birthdate, string member_email, string member_tel_no, string member_dealer,
            string follower_title, string follower_name, string follower_birthdate, string follower_email,
            string follower_tel_no, string follower_car_model, int follower_year_purchase_id, string v, string p,
            string lang, string vin, int relationship_id, string relationship_special, int car_brand_id,
            string car_brand_special, bool is_car_owner, List<EventAnswer> member_answers, List<EventAnswer> follower_answers, bool has_follower)
        {
            ServiceEventRegisterModel value = new ServiceEventRegisterModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);


                bool is_complete_answer = false;
                bool save_member_answer_success = false;
                bool save_follower_answer_success = false;
                bool check_event_has_question = false;
                int event_register_id = 0;
                bool has_follow_question = false;

                SystemController syc = new SystemController();
                ValidationModel validation2 = syc.CheckSystemNew(p, v, lang).Result;
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    CheckEventCountAllQuestionModel model = new CheckEventCountAllQuestionModel();

                    //count_event_question = CountEventHasQuestion(event_id, Convert.ToInt16(is_car_owner), Convert.ToInt16(has_follower));
                    model = CountEventHasQuestion(event_id, Convert.ToInt16(is_car_owner), Convert.ToInt16(has_follower));
                    if (model.question_for_member_follow > 0 || model.question_for_represent_follow > 0)
                        has_follow_question = true;

                    if (model.count_event > 0)
                        check_event_has_question = true;

                    if (model.count_event == (member_answers.Count + follower_answers.Count))
                        is_complete_answer = true;


                    ValidationModel validation = await CheckValidationNew_Invitaion(event_id, token, lang, vin, is_car_owner, is_complete_answer);
                    //ValidationModel validation = CheckValidationNew(event_id, token, lang, vin, is_car_owner, is_complete_answer).Result;
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    var eventRedeemCode = EventRedeemCode(event_id);

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = @"
                               DECLARE
                               	@MEMBERID NVARCHAR(50) = N'{0}',
                               	@EVENT_ID INT = {1},
                               	@member_title NVARCHAR(50) = N'{2}',
                               	@member_name NVARCHAR(300) = N'{3}',
                               	@member_birthdate NVARCHAR(50) = N'{4}',
                               	@member_email NVARCHAR(100) = N'{5}',
                               	@member_tel_no NVARCHAR(100) = N'{6}',
                               	@member_dealer NVARCHAR(250) = N'{7}',
                               	@follower_title NVARCHAR(50) = N'{8}',
                               	@follower_name NVARCHAR(300) = N'{9}',
                               	@follower_birthdate NVARCHAR(50) = N'{10}',
                               	@follower_email NVARCHAR(100) = N'{11}',
                               	@follower_tel_no NVARCHAR(100) = N'{12}',
                               	@follower_dealer NVARCHAR(250) = N'{13}',
                               	@STATUS NVARCHAR(5) = N'{14}',
                               	@Token NVARCHAR(100) = N'{15}',
                                   @follower_car_model NVARCHAR(100) = N'{16}',
                                   @follower_year_purchase_id INT = N'{17}',
                                   @relationship_id INT = {18},
                                   @relationship_special NVARCHAR(250) = N'{19}',
                                   @car_brand_id INT = {20},
                                   @car_brand_special NVARCHAR(250) = N'{21}',
                                   @vin NVARCHAR(100) = N'{22}',      
                                @redeem_code NVARCHAR(50) = N'{23}'

                               SET @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @Token)
                               -- SET @member_birthdate = CASE WHEN LEN(@member_birthdate) = 0 THEN NULL ELSE @member_birthdate END
                               -- SET @follower_birthdate = CASE WHEN LEN(@follower_birthdate) = 0 THEN NULL ELSE @follower_birthdate END
                               
                               
                               INSERT INTO T_EVENTS_REGISTER (MEMBERID,EVENT_ID,member_title,member_name,member_birthdate,member_email,member_tel_no,
                                   member_dealer,follower_title,follower_name,follower_birthdate,follower_email,follower_tel_no,follower_dealer,CREATE_DT,
                                   [STATUS],follower_car_model,follower_year_purchase_id,relationship_id,relationship_special,car_brand_id,car_brand_special, vin, REDEEM_CODE)
                               VALUES (@MEMBERID,@EVENT_ID,@member_title,@member_name,
                                   CASE LEN(@member_birthdate) WHEN 0 THEN NULL ELSE @member_birthdate END,
                                   @member_email,@member_tel_no,@member_dealer,@follower_title,@follower_name,
                                   CASE LEN(@follower_birthdate) WHEN 0 THEN NULL ELSE @follower_birthdate END,
                                   @follower_email,@follower_tel_no,@follower_dealer,DATEADD(HOUR, 7, GETDATE()),@STATUS,@follower_car_model,
                                   @follower_year_purchase_id,@relationship_id,@relationship_special,@car_brand_id,@car_brand_special, @vin, @redeem_code)

                                    SELECT SCOPE_IDENTITY() as id";
                     

                        using (DataTable dt = db.GetDataTableFromCommandText
                                 (string.Format(cmd, "", event_id, member_title, member_name, member_birthdate, member_email, member_tel_no,
                                                           member_dealer, follower_title, follower_name, follower_birthdate, follower_email,
                                                           follower_tel_no, "", "", token, follower_car_model, follower_year_purchase_id,
                                                           relationship_id, relationship_special, car_brand_id, car_brand_special, vin, eventRedeemCode.redeem_code)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                event_register_id = Convert.ToInt32(dt.Rows[0]["id"]);

                                //check event has question ? 

                                if (check_event_has_question)
                                {
                                    if (member_answers.Count > 0)
                                    {
                                        save_member_answer_success = SaveUserAnswer(token, event_register_id, is_car_owner, member_answers);
                                    }
                                    if (has_follower && has_follow_question)
                                    {
                                        if (follower_answers.Count == model.question_for_member_follow || follower_answers.Count == model.question_for_represent_follow)
                                        {
                                            save_follower_answer_success = SaveUserAnswer(token, event_register_id, is_car_owner, follower_answers);
                                        }
                                    }
                                }
                            }
                        }

                    }



                    value.data = GetEventThankyouMessage(lang, event_register_id);


                    string temp_error_text = "";

                    if (check_event_has_question)
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                        if (!save_member_answer_success)
                        {
                            value.success = false;
                            temp_error_text += "Cannot save member answer";
                            temp_error_text += " ";
                            value.msg = new MsgModel() { code = 0, text = temp_error_text, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        }
                        else if (has_follower && has_follow_question && !save_follower_answer_success)
                        {
                            value.success = false;
                            if (!save_member_answer_success) { temp_error_text += " and "; }
                            temp_error_text += "Cannot save follow answer";
                            temp_error_text += " ";
                            value.msg = new MsgModel() { code = 0, text = temp_error_text, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        }
                    }
                    else
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public EventCode EventRedeemCode(int event_id)
        {
            var value = new EventCode();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    var event_type = "";
                    string cmd = @"SELECT event_type FROM T_EVENTS WHERE [ID] = {0} AND REG_PERIOD_END > GETDATE()";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                    {
                        if (dt.Rows.Count >= 0)
                        {
                            event_type = dt.Rows[0]["event_type"] != DBNull.Value ? dt.Rows[0]["event_type"].ToString() : "";
                        }
                    }

                    if (event_type == "invitation")
                    {
                        cmd = @"
                            select EC.REDEEM_CODE
							from [dbo].[T_EVENTS_CODE] EC
							where EC.EVENT_ID = {0} and EC.DEL_FLAG IS NULL and EC.STATUS = 'NotUsed'";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                var data = dt.Rows[0];
                                value.redeem_code = data["REDEEM_CODE"] != DBNull.Value ? data["REDEEM_CODE"].ToString() : "";
                            }
                        }

                        cmd = @"
                            update [dbo].[T_EVENTS_CODE]
							set STATUS = 'RedeemUsed'
							where REDEEM_CODE = N'{0}' and EVENT_ID = {1}";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, value.redeem_code, event_id)))
                        {
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

        public bool SaveUserAnswer(string token, int register_event_id, bool is_car_owner, List<EventAnswer> someone_answers)
        {
            int temp_check_success = 0;
            try
            {
                DateTime date_time = DateTime.Now;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        DECLARE @Token NVARCHAR(100) = N'{0}',
                        @MEMBERID NVARCHAR(50)

                        SET @MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = @Token)
                        INSERT INTO [dbo].[event_answer_user]
                        ([question_id], [answer_id], [answer_text], [created_date], [created_user], [event_register_id], [is_car_owner])
                        VALUES
                        (N'{1}', N'{2}', N'{3}', N'{4}', @MEMBERID, N'{5}', N'{6}')";
                    foreach (var anser in someone_answers)
                    {
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, anser.question_id, anser.answer_id, anser.text, date_time, register_event_id, is_car_owner)))
                        {
                            temp_check_success++;
                        }
                    }
                    if (someone_answers.Count() == temp_check_success)
                    {
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

        //public ServiceEventRegisterData GetEventThankyouMessage(string lang, int event_id)
        //{
        //    ServiceEventRegisterData value = new ServiceEventRegisterData();
        //    try
        //    {
        //        using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //        {
        //            string cmd = string.Format(@"
        //                   DECLARE @lang nvarchar(2) =N'{0}'
        //                   SELECT 
        //                   	CASE
        //                   		WHEN @lang = 'TH' THEN thankyou_message_th
        //                   		WHEN @lang = 'EN' THEN thankyou_message_en
        //                   	END AS message,
        //        event_type,
        //                    redeem_code,
        //                    TITLE,
        //                    CREATE_DT
        //                   FROM T_EVENTS 
        //                   WHERE DEL_FLAG IS NULL AND ID=N'{1}'", lang, event_id);


        //            using (DataTable dt = db.GetDataTableFromCommandText(cmd))
        //            {
        //                //if (dt.Rows.Count > 0)
        //                //{
        //                //    DataRow dr = dt.Rows[0];
        //                //    value.thankyou_message = dr["message"].ToString();
        //                //    value.event_type = dr["event_type"].ToString();
        //                //    value.invitation_code = dr["redeem_code"].ToString();
        //                //    value.name = dr["TITLE"].ToString();
        //                //    value.registered_date = dr["CREATE_DT"].ToString();
        //                //}

        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    DataRow dr = dt.Rows[0];
        //                    value.thankyou_message = dr["message"].ToString();
        //                    value.event_type = dr["event_type"].ToString();
        //                    value.invitation_code = dr["redeem_code"].ToString();
        //                    value.name = dr["TITLE"].ToString();
        //                    value.registered_date = dr["CREATE_DT"].ToString();
        //                    value.images = row["IMAGE1"].ToString();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return value;
        //}

        public ServiceEventRegisterData GetEventThankyouMessage(string lang, int event_register_id)
        {
            ServiceEventRegisterData value = new ServiceEventRegisterData();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
                           DECLARE @lang nvarchar(2) =N'{0}'

                           SELECT
                           E.ID,					
                           	CASE
                           		WHEN @lang = 'TH' THEN thankyou_message_th
                           		WHEN @lang = 'EN' THEN thankyou_message_en
                           	END AS message,
	                        E.event_type,
                            E.TITLE,
                            ER.CREATE_DT,
							ER.STATUS,
							ER.REDEEM_CODE
                           FROM T_EVENTS_REGISTER ER
						   inner join T_EVENTS E on ER.EVENT_ID = E.ID
                           WHERE ER.ID=N'{1}';
                            ", lang, event_register_id);

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            DataRow dr = dt.Rows[0];
                            value.thankyou_message = dr["message"].ToString();
                            value.event_type = dr["event_type"].ToString();
                            value.invitation_code = dr["REDEEM_CODE"].ToString();
                            value.name = dr["TITLE"].ToString();
                            value.registered_date = (row["CREATE_DT"] == DBNull.Value) ? "" : Convert.ToDateTime(row["CREATE_DT"]).ToString("yyyy-MM-dd HH:mm:ss");
                            value.images = new List<string>();
                            value.images = GetAllEventPicture(Convert.ToInt32(row["ID"]));
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
        public async Task<ServiceEventRegisterModel> CheckEventRegister(int event_id, string token, string v, string p, string lang)
        {
            ServiceEventRegisterModel value = new ServiceEventRegisterModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation_system = await syc.CheckSystemNew(p, v, lang);
                if (!validation_system.Success)
                {
                    value.success = validation_system.Success;
                    value.msg = new MsgModel() { code = validation_system.InvalidCode, text = validation_system.InvalidMessage };
                    return value;
                }
                else
                {
                    //ValidationModel validation = await CheckValidationNew(event_id, token, lang, "", null, null);
                    ValidationModel validation = await CheckValidationNew_Invitaion(event_id, token, lang, "", null, null);
                    value.msg = new MsgModel();

                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg.code = validation.InvalidCode;
                        value.msg.text = validation.InvalidMessage;
                        value.msg.store_link = validation.InvalidStoreLink;
                        value.msg.version = validation.InvalidVersion;

                        return value;
                    }

                    value.success = true;
                    value.msg.code = 0;
                    value.msg.text = "Success";
                    value.msg.store_link = validation.InvalidStoreLink;
                    value.msg.version = validation.InvalidVersion;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return value;
        }


        public CheckEventCountAllQuestionModel CountEventHasQuestion(int event_id, int is_car_owner, int has_follower)
        {
            CheckEventCountAllQuestionModel model = new CheckEventCountAllQuestionModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                        
                             DECLARE @is_car_owner bit = N'{1}';
                             DECLARE @has_follow bit = N'{2}';
                             DECLARE @count_event int = 0;
                             
                             DECLARE @question_for_member int = 0;
							 DECLARE @question_for_member_follow int = 0;
							 DECLARE @question_for_represent int = 0;
							 DECLARE @question_for_represent_follow int = 0;

                             	IF (@is_car_owner = 1)
                             		BEGIN
                             			SET @count_event += (SELECT count(id) FROM event_question where deleted_flag IS NULL AND event_id = N'{0}' AND (question_type = 1))
										SET @question_for_member += (SELECT count(id) FROM event_question where deleted_flag IS NULL AND event_id = N'{0}' AND (question_type = 1))
										IF (@has_follow = 1)
											BEGIN
                             					SET @count_event += (SELECT count(id)  FROM event_question where deleted_flag IS NULL AND event_id = N'{0}' AND (question_type = 3))
												SET @question_for_member_follow += (SELECT count(id)  FROM event_question where deleted_flag IS NULL AND event_id = N'{0}' AND (question_type = 3))
											END
                             		END
                                 ELSE
                             		BEGIN
                             			SET @count_event += (SELECT count(id)  FROM event_question where deleted_flag IS NULL AND event_id = N'{0}' AND (question_type = 2))
                             			SET @question_for_represent += (SELECT count(id)  FROM event_question where deleted_flag IS NULL AND event_id = N'{0}' AND (question_type = 2))
										IF (@has_follow = 1)
											BEGIN
                             					SET @count_event += (SELECT count(id)  FROM event_question where deleted_flag IS NULL AND event_id = N'{0}' AND (question_type = 4))
												SET @question_for_represent_follow += (SELECT count(id)  FROM event_question where deleted_flag IS NULL AND event_id = N'{0}' AND (question_type = 4))
											END
                             		END
                             
                             		SELECT @count_event AS count_event, 
									@question_for_member AS question_for_member, 
									@question_for_member_follow AS question_for_member_follow,
									@question_for_represent AS question_for_represent,
									@question_for_represent_follow AS question_for_represent_follow";

                    using (DataTable dt = db.GetDataTableFromCommandText
                                (string.Format(cmd, event_id, is_car_owner, has_follower)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            model.count_event = Convert.ToInt32(row["count_event"]);
                            model.question_for_member = Convert.ToInt32(row["question_for_member"]);
                            model.question_for_member_follow = Convert.ToInt32(row["question_for_member_follow"]);
                            model.question_for_represent = Convert.ToInt32(row["question_for_represent"]);
                            model.question_for_represent_follow = Convert.ToInt32(row["question_for_represent_follow"]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }


        private async Task<ValidationModel> CheckValidationNew(int event_id, string token, string lang, string vin, bool? is_car_owner, bool? is_complete_answer)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    var event_type = "";
                    #region E701
                    state = ValidationModel.InvalidState.E701;
                    string cmd = @"SELECT event_type FROM T_EVENTS WHERE [ID] = {0} AND REG_PERIOD_END > GETDATE()";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                    {
                        if (dt.Rows.Count == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                        event_type = dt.Rows[0]["event_type"] != DBNull.Value ? dt.Rows[0]["event_type"].ToString() : "";
                    }
                    #endregion
                    #region E702
                    state = ValidationModel.InvalidState.E702;

                    if (event_type == "invitation")
                    {
                        cmd = @"select count(REDEEM_CODE) count
							from T_EVENTS_CODE EC
							where EC.EVENT_ID = {0} and EC.DEL_FLAG IS NULL and EC.STATUS = 'NotUsed'";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                        {
                            if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }
                    }

                    cmd = @"SELECT CASE WHEN LIMIT_GUEST > (SELECT COUNT(1) FROM T_EVENTS_REGISTER WHERE EVENT_ID = {0} ) THEN 1 ELSE 0 END AS VALIDATE FROM T_EVENTS WHERE ID = {0}";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(token))
                    {

                        #region E708
                        state = ValidationModel.InvalidState.E708;

                        if (is_car_owner != null && is_complete_answer != null)
                        {
                            if (is_complete_answer == false)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }

                        }
                        #endregion

                        EventService svr = new EventService();
                        MasterDataService data_svr = new MasterDataService();
                        string member_id = await data_svr.GetMemberIdByToken(token);

                        EventDetailForCalculate detail = await svr.DefindValueForEvent(event_id);


                        EventDetailControlButton control_button = new EventDetailControlButton();

                        int all_vin_by_member = await svr.GetAllNumberOfCar(member_id);
                        int used_vin_by_member_all_event = await svr.GetAllUsedCarAllEvent(member_id, detail.config_period_start, detail.config_period_end);
                        int used_vin_from_this_event_type = await svr.GetVinFromThisEventType(member_id, detail.config_period_start, detail.config_period_end, event_id);
                        int used_vin_from_this_event = await svr.GetVinFromThisEvent(member_id, detail.config_period_start, detail.config_period_end, event_id);


                        bool is_registered = await svr.GetRegistered(member_id, detail.config_period_start, detail.config_period_end, event_id);
                        bool is_registered_from_other_event = await svr.CheckVinFromOtherEvent(member_id, detail.config_period_start, detail.config_period_end, event_id);

                        int is_registered_from_other_event_false_false = await svr.CheckVinFromOtherEvent_false_false(member_id, detail.config_period_start, detail.config_period_end, event_id);
                        int remaining_vin = all_vin_by_member - is_registered_from_other_event_false_false;
                        bool is_registered_from_other_event_true_false = await svr.CheckVinFromOtherEvent_true_false(member_id, detail.config_period_start, detail.config_period_end, event_id);
                        bool check_enough_car = await svr.CheckEnoughCarForEvent(event_id, all_vin_by_member);
                        int remaining_vin_from_this_event_type = all_vin_by_member - used_vin_from_this_event_type;
                        int remaining_vin_from_this_event = all_vin_by_member - used_vin_from_this_event;

                        if (detail.allow_dupplicate_register_event == true && detail.allow_dupplicate_follow_number_of_vin == false)
                        {
                            state = ValidationModel.InvalidState.E703;
                            if (is_registered_from_other_event || remaining_vin <= 0)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }
                        if (detail.allow_dupplicate_register_event == false && detail.allow_dupplicate_follow_number_of_vin == false)
                        {
                            if (is_registered_from_other_event_true_false)
                            {
                                remaining_vin -= 1;
                            }
                            state = ValidationModel.InvalidState.E703;
                            if (remaining_vin <= 0)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }
                        if (detail.allow_dupplicate_register_event == true && detail.allow_dupplicate_follow_number_of_vin == true)
                        {
                            state = ValidationModel.InvalidState.E703;
                            if (remaining_vin_from_this_event_type <= 0 && is_registered == false)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }
                        if (detail.allow_dupplicate_register_event == false && detail.allow_dupplicate_follow_number_of_vin == true)
                        {
                            state = ValidationModel.InvalidState.E703;
                            if (remaining_vin_from_this_event <= 0 && is_registered == false)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }



                        //#region E703
                        ////state = ValidationModel.InvalidState.E703;
                        ////if (is_registered_from_other_event_true_false)
                        ////{
                        ////    remaining_vin -= 1;
                        ////}
                        ////if (remaining_vin <= 0 && !is_registered)
                        ////{
                        ////    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessageNew(state, lang) };
                        ////}
                        //#endregion

                        #region E706
                        state = ValidationModel.InvalidState.E706;
                        if (!check_enough_car)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                        #endregion


                    }


                    #region E705
                    state = ValidationModel.InvalidState.E705;
                    if (string.IsNullOrEmpty(token))
                    {
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                    }
                    #endregion




                    #region E815
                    //Not use in Phase 2.3.0
                    //if (vin != "")
                    //{

                    //    state = ValidationModel.InvalidState.E815;
                    //    cmd = @"SELECT COUNT(1) AS CNT FROM T_CUSTOMER_CAR WHERE vin = N'{0}' AND ABS(DATEDIFF(month, rs_date, getdate()))/12 >= 10 ";
                    //    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, vin)))
                    //    {
                    //        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                    //        {
                    //            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                    //        }
                    //    }
                    //}
                    #endregion



                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private ValidationModel CheckValidation(int event_id, string token)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E701
                    state = ValidationModel.InvalidState.E701;
                    string cmd = @"SELECT COUNT(1) AS CNT FROM T_EVENTS WHERE [ID] = {0} AND REG_PERIOD_END > GETDATE()";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                    }
                    #endregion
                    #region E702
                    state = ValidationModel.InvalidState.E702;
                    cmd = @"SELECT CASE WHEN LIMIT_GUEST > (SELECT COUNT(1) FROM T_EVENTS_REGISTER WHERE EVENT_ID = {0} ) THEN 1 ELSE 0 END AS VALIDATE FROM T_EVENTS WHERE ID = {0}";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                    }

                    #endregion
                    //#region E703
                    //state = ValidationModel.InvalidState.E703;
                    //cmd = @"SELECT COUNT(1) AS CNT FROM T_EVENTS_REGISTER WHERE EVENT_ID = {0} AND MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{1}')";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id, token)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) > 0)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                    //    }
                    //}
                    //#endregion

                    //#region E704
                    //state = ValidationModel.InvalidState.E704;
                    //cmd = @"SELECT CASE WHEN ISNULL(EXPIRY, '1800-01-01') < GETDATE() THEN 1 ELSE 0 END AS IS_EXPIRE FROM T_CUSTOMER WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')";
                    //using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    //{
                    //    if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 1)
                    //    {
                    //        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                    //    }
                    //}
                    //#endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }



        private async Task<ValidationModel> CheckValidationNew_Invitaion(int event_id, string token, string lang, string vin, bool? is_car_owner, bool? is_complete_answer)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    var event_type = "";
                    #region E701
                    state = ValidationModel.InvalidState.E701;
                    string cmd = @"SELECT event_type FROM T_EVENTS WHERE [ID] = {0} AND REG_PERIOD_END > GETDATE()";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                    {
                        if (dt.Rows.Count == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                        event_type = dt.Rows[0]["event_type"] != DBNull.Value ? dt.Rows[0]["event_type"].ToString() : "";
                    }
                    #endregion 

                    #region Check invitation type before limit guest.
                    //state = ValidationModel.InvalidState.E702;

                    if (event_type == "invitation")
                    {
                        cmd = @"select count(REDEEM_CODE) count
							from T_EVENTS_CODE EC
							where EC.EVENT_ID = {0} and EC.DEL_FLAG IS NULL and EC.STATUS = 'NotUsed'";
                        using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                        {
                            if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }
                        }
                    }
                    #endregion

                    #region E702
                    state = ValidationModel.InvalidState.E702;

                    cmd = @"SELECT CASE WHEN LIMIT_GUEST > (SELECT COUNT(1) FROM T_EVENTS_REGISTER WHERE EVENT_ID = {0} ) THEN 1 ELSE 0 END AS VALIDATE FROM T_EVENTS WHERE ID = {0}";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, event_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(token))
                    {

                        #region E708
                        state = ValidationModel.InvalidState.E708;

                        if (is_car_owner != null && is_complete_answer != null)
                        {
                            if (is_complete_answer == false)
                            {
                                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                            }

                        }
                        #endregion

                        EventService svr = new EventService();
                        MasterDataService data_svr = new MasterDataService();
                        string member_id = await data_svr.GetMemberIdByToken(token);

                        int all_vin_by_member = await svr.GetAllNumberOfCar(member_id);

                        bool check_enough_car = await svr.CheckEnoughCarForEvent(event_id, all_vin_by_member);

                        #region E706
                        state = ValidationModel.InvalidState.E706;
                        if (!check_enough_car)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                        #endregion

                        #region new since in Phase 2.3.0
                        //Not use in Phase 2.3.0

                        state = ValidationModel.InvalidState.E817;
                        //find car model 
                        //------------- vin can use -----------------------------
                        List<CarModel> my_car_vin_model = new List<CarModel>();
                        my_car_vin_model = await findAllMyVinModel(member_id);
                        List<string> my_vin = new List<string>();
                        List<int> my_model = new List<int>();
                        for (int i = 0; i < my_car_vin_model.Count; i++)
                        {
                            my_vin.Add(my_car_vin_model[i].vin);
                            my_model.Add(Convert.ToInt16(my_car_vin_model[i].model));
                        }

                        string car_models_json = await findCarModelForEvent(event_id);
                        List<DeserializeObject> car_model_object = new List<DeserializeObject>();
                        car_model_object = JsonConvert.DeserializeObject<List<DeserializeObject>>(car_models_json);
                        List<string> vin_can_use = new List<string>();

                        for (int i = 0; i < my_model.Count; i++)
                        {
                            for (int n = 0; n < car_model_object.Count(); n++)
                            {
                                if (my_model[i] == car_model_object[n].id)
                                {
                                    vin_can_use.Add(my_vin[i]);
                                }
                            }
                        }

                        if (vin_can_use.Count == 0)
                        {
                            state = ValidationModel.InvalidState.E818;
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }


                        //find car follow period
                        List<string> vin_follow_period = new List<string>();
                        vin_follow_period = await findAllVinFollowPeriod(event_id, member_id);

                        _ServiceEventVinData remaining_vins_can_registered = await svr.GetAllVinDatas(token, lang, event_id);
                        List<string> used_vin = await this.GetAllUsedCarFoThisEvent(member_id, event_id);
                        //var query_remaining_vins = from vin_a in vin_can_use
                        //                           join vin_b in vin_follow_period
                        //                           on vin_can_use equals vin_follow_period
                        //                           select vin_can_use;

                        //bool remaining_all_vins_not_check_register = vin_can_use.Intersect(vin_follow_period).Any();
                        int remaining_all_vins_not_check_register = 0;

                        for (int i = 0; i < vin_can_use.Count; i++)
                        {
                            for (int n = 0; n < vin_follow_period.Count; n++)
                            {
                                if (vin_can_use[i].ToString().Trim() == vin_follow_period[n].ToString().Trim())
                                {
                                    remaining_all_vins_not_check_register++;
                                }
                            }
                          
                        }

                        

                        //bool remaining_all_vins_not_check_register = query_remaining_vins.Count();

                        if (remaining_vins_can_registered.cars.Count == 0 && used_vin.Count == 0)
                        {
                            state = ValidationModel.InvalidState.E818;
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                        //--------------------------- old -------------------------------------------------------

                        //string[] period_type = await find_period(event_id); 

                        //if (period_type == null)
                        //{
                        //    return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        //    //return new ValidationModel { Success = false, InvalidCode = 99999, InvalidMessage = "case 1 : not maintain for car in this event" };
                        //}
                        //else
                        //{
                        //    cmd = CheckPeriodType(period_type);
                        //    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id)))
                        //    {
                        //        if (dt.Rows.Count == 0)
                        //        {
                        //            state = ValidationModel.InvalidState.E818;
                        //            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        //            //return new ValidationModel { Success = false, InvalidCode = 99999, InvalidMessage = "case 2 : not has any car for event condition" };
                        //        }
                        //        else
                        //        {
                        //string car_models_json = await findCarModelForEvent(event_id);
                        //            List<DeserializeObject> car_model_object = new List<DeserializeObject>();
                        //            car_model_object = JsonConvert.DeserializeObject<List<DeserializeObject>>(car_models_json);


                        //            List<string> vin_can_use = new List<string>();
                        //            foreach (DataRow row in dt.Rows)
                        //            {
                        //                for (int n = 0; n < car_model_object.Count(); n++)
                        //                {
                        //                    if (Convert.ToInt16(row["MODEL_ID"]) == car_model_object[n].id)
                        //                    {
                        //                        vin_can_use.Add(row["vin"].ToString());
                        //                    }
                        //                }
                        //            }

                        //            if (vin_can_use.Count() <= 0)
                        //            {
                        //                state = ValidationModel.InvalidState.E819;
                        //                return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        //                //return new ValidationModel { Success = false, InvalidCode = 99999, InvalidMessage = "case 3 : my vin is not in event model car condition." };
                        //            }

                        //        }
                        //    }
                        //}

                        //-------------------------- end old -----------------------------------------

                        #region checkEventType
                        bool is_one_member_per_event = this.CheckEventType(event_id);
                        //check event
                        //all_vin_by_member
                        
                        if (used_vin.Count > 0)
                        {
                            if (is_one_member_per_event)
                            {
                                return new ValidationModel { Success = false, InvalidCode = 99999, InvalidMessage = "Registered" };
                            }
                            else
                            {
                                //if (remaining_vins.cars.Count == 0)
                                //{
                                //    return new ValidationModel { Success = false, InvalidCode = 99999, InvalidMessage = "Registered" };
                                //}
                                if (remaining_all_vins_not_check_register - used_vin.Count == 0)
                                {
                                    return new ValidationModel { Success = false, InvalidCode = 99999, InvalidMessage = "Register full amount" };
                                }
                            }
                        }

                        #endregion


                        #endregion




                    }


                    #region E705
                    state = ValidationModel.InvalidState.E705;
                    if (string.IsNullOrEmpty(token))
                    {
                        return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                    }
                    #endregion





                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private string CheckPeriodType(string[] period_type)
        {

            ////1. check car age
            //ValidationModel.InvalidState state;
            //state = ValidationModel.InvalidState.E701;
            //string[] period_type = await find_period_type(event_id);
            //if (period_type == null)
            //{
            //    //return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
            //    return new ValidationModel { Success = false, InvalidCode = 99999, InvalidMessage = "case 1 : not maintain for car in this event" };
            //}
            //else
            //{
            // string temp = Array.Find(period_type, i => i.Equals("1"));
            int index = 0;
            string cmd = @"
                        DECLARE @MEMBERID NVARCHAR(20) = N'{0}';
                        DECLARE @date_now date =  cast(GETDATE() as date)
                        DECLARE @date_target_under_5 date = dateadd(year, -5, cast(GETDATE() as date))
                        DECLARE @date_target_under_10 date = dateadd(year, -10, cast(GETDATE() as date))
                        --SELECT @date_now AS XNOW, @date_target_under_5 X5, @date_target_under_10 X10
                        SELECT RS_Date, vin, MODEL_ID
                        FROM T_CUSTOMER_CAR AS CUS_CAR 
                        WHERE DEL_FLAG IS NULL   
                        AND MEMBERID = @MEMBERID";
            foreach (string i in period_type)
            {
                switch (i)
                {
                    //0-4 years
                    case "1":
                        cmd += @" AND RS_Date between @date_target_under_5 and @date_now";
                        index++;
                        break;

                    //5-10 years
                    case "2":

                        if (index == 0)
                        {
                            cmd += @" AND (RS_Date between @date_target_under_10 and @date_target_under_5) ";
                        }
                        else
                        {
                            cmd += @" UNION
                                        SELECT RS_Date, vin, MODEL_ID
                                        FROM T_CUSTOMER_CAR AS CUS_CAR 
                                        WHERE DEL_FLAG IS NULL   
                                        AND MEMBERID = @MEMBERID
                                        AND (RS_Date between @date_target_under_10 and @date_target_under_5) ";

                        }
                        index++;
                        break;

                    //10 years more
                    case "3":
                        if (index == 0)
                        {
                            cmd += @" AND RS_Date < @date_target_under_10 ";
                        }
                        else
                        {
                            cmd += @" UNION
                                        SELECT RS_Date, vin, MODEL_ID
                                        FROM T_CUSTOMER_CAR AS CUS_CAR 
                                        WHERE DEL_FLAG IS NULL   
                                        AND MEMBERID = @MEMBERID
                                        AND RS_Date < @date_target_under_10";
                        }
                        index++;
                        break;
                }
            }


            return cmd;
        }

        private async Task<string[]> find_period_type_old(int event_id)
        {
            string tmp_period_type = "";
            string[] period_type = { };
            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
            {
                string cmd = @"
                        DECLARE @event_id int = N'{0}'  
                        SELECT top 1 period_type FROM T_EVENTS where ID = @event_id";

                using (DataTable dt = db.GetDataTableFromCommandText
                            (string.Format(cmd, event_id)))
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        tmp_period_type = row["period_type"].ToString();
                        period_type = tmp_period_type.Split(',');
                    }
                }
                return period_type;

            }

        }

        private async Task<string> findCarModelForEvent(int event_id)
        {
            string car_model = "";
            string[] period_type = { };
            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
            {
                string cmd = @"
                        DECLARE @event_id int = N'{0}'  
                        select preferred_model_ids from t_events where id = @event_id";
                using (DataTable dt = db.GetDataTableFromCommandText
                            (string.Format(cmd, event_id)))
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        car_model = row["preferred_model_ids"].ToString();

                    }
                }
                return car_model;

            }
        }

        private bool CheckEventType(int event_id)
        {

            bool is_one_member_per_event = false;
            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
            {
                string cmd = @"
                        DECLARE @event_id int = N'{0}'  
                        select one_member_per_event from t_events where id = @event_id";
                using (DataTable dt = db.GetDataTableFromCommandText
                            (string.Format(cmd, event_id)))
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        is_one_member_per_event = row["one_member_per_event"] != DBNull.Value ? Convert.ToBoolean(row["one_member_per_event"]) : false;

                    }
                }


            }

            return is_one_member_per_event;
        }

        public async Task<List<string>> GetAllUsedCarFoThisEvent(string member_id, int event_id)
        {
            List<string> used_vin = new List<string>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                         DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                         DECLARE @event_id int = N'{1}'
                        
                        SELECT events_regis.vin AS used_vin from T_EVENTS_REGISTER AS events_regis
                        INNER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
                        WHERE MEMBERID = @MEMBERID AND event_.ID = @event_id
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            used_vin.Add(row["used_vin"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return used_vin;
        }

        public async Task<List<string>> GetAllCarCanUseForThisEvent(string member_id, int event_id)
        {
            List<string> used_vin = new List<string>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                         DECLARE @MEMBERID NVARCHAR(50) = N'{0}';
                         DECLARE @event_id int = N'{1}'
                        
                        SELECT VIN AS can_use_vin FROM T_CUSTOMER_CAR WHERE MEMBERID = 'AG1132' AND DEL_FLAG IS NULL
						AND VIN 
						NOT IN  
						(SELECT events_regis.vin AS used_vin from T_EVENTS_REGISTER AS events_regis
                        FULL OUTER JOIN T_EVENTS AS event_ ON event_.ID = events_regis.EVENT_ID
                        WHERE MEMBERID = @MEMBERID 
						AND event_.ID = @event_id)
                         ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, event_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            used_vin.Add(row["can_use_vin"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return used_vin;
        }

        //private async Task<string[]> find_period(int event_id)
        //{
        //    string tmp_period_type = "";
        //    string[] period_type = { };
        //    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //    {
        //        string cmd = @"
        //                DECLARE @event_id int = N'{0}'  
        //                SELECT top 1 period_type FROM T_EVENTS where ID = @event_id";

        //        using (DataTable dt = db.GetDataTableFromCommandText
        //                    (string.Format(cmd, event_id)))
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                DataRow row = dt.Rows[0];
        //                tmp_period_type = row["period_type"].ToString();
        //                period_type = tmp_period_type.Split(',');
        //            }
        //        }
        //        return period_type;

        //    }
        //}








        private async Task<List<CarModel>> findAllMyVinModel(string member_id)
        {
            List<CarModel> carModels = new List<CarModel>();

            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
            {
                string cmd = @"                     
                        SELECT model_id, vin, PLATE_NO, RS_Date FROM T_CUSTOMER_CAR AS cus_car
                        WHERE MEMBERID = N'{0}' AND cus_car.DEL_FLAG Is NULL
                        ";

                using (DataTable dt = db.GetDataTableFromCommandText
                            (string.Format(cmd, member_id)))
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        CarModel carModel = new CarModel();
                        carModel.model = row["model_id"].ToString();
                        carModel.vin = row["vin"].ToString();
                        carModel.plate_no = row["PLATE_NO"].ToString();
                        carModel.rs_date = row["RS_Date"].ToString();

                        carModels.Add(carModel);
                    }
                }
                return carModels;

            }

        }


        private async Task<List<string>> findAllVinFollowPeriod(int event_id, string member_id)
        {
            List<string> vins = new List<string>();
            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
            {
                string cmd = @"                     
                        DECLARE @event_id int = N'{0}'
                        DECLARE @member_id VARCHAR(20) = N'{1}'
                        
                        
                        DECLARE @date_start int = (select period_car_age_start from t_events where id = @event_id)
                        DECLARE @date_end int = (select period_car_age_end from t_events where id = @event_id)
                        
                        DECLARE @compleate_date_from date = dateadd(year, ABS(@date_start)*-1, cast(GETDATE() as date))
                        DECLARE @compleate_date_to date = dateadd(year, ABS(@date_end)*-1, cast(GETDATE() as date))
                        
                        IF @date_start = @date_end
	                    BEGIN
		                    SET @compleate_date_to = dateadd(day, 1, dateadd(year, ABS(@date_end + 1)*-1, cast(GETDATE() as date))) 
	                    END

                        SELECT RS_Date, vin FROM t_customer_car AS cus_car
                        WHERE MEMBERID = @member_id AND DEL_FLAG IS NULL
                        AND RS_Date BETWEEN @compleate_date_to AND @compleate_date_from
                        ";

                using (DataTable dt = db.GetDataTableFromCommandText
                            (string.Format(cmd, event_id, member_id)))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        vins.Add(row["vin"].ToString());
                    }

                }

            }

            return vins;
        }
    }
}