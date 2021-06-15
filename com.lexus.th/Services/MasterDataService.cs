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
    public class MasterDataService
    {
        private string conn;

        public MasterDataService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceAllMasterDataModel> GetAllMasterData(string v, string p, string lang, string token, string device_id)
        {
            ServiceAllMasterDataModel value = new ServiceAllMasterDataModel();
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
                    value.data = new _ServiceAllMasterData();
                    value.data.config = GetAllConfig();

                    string member_id =await GetMemberId(token);


                    int survey_id = GetRemainingSurveyId(member_id, device_id, p);

                    string token_string = "";
                    string device_string = "";
                    value.data.survey_url = "";

                    if (survey_id != 0)
                    {
                        if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(device_id))
                        {
                            token_string = "token=" + token;
                            device_string = "deviceid=" + device_id;

                            value.data.survey_url = string.Format(WebConfigurationManager.AppSettings["survey_url_member"].ToString(), survey_id, token_string, device_string, lang);
                        }
                        if (string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(device_id))
                        {
                            device_string = "deviceid=" + device_id;

                            value.data.survey_url = string.Format(WebConfigurationManager.AppSettings["survey_url_guest"].ToString(), survey_id, device_string, lang);
                        }
                    }
                    int sys_version = Convert.ToInt32(WebConfigurationManager.AppSettings["version"].ToString());
                    bool is_read_terms =await CheckReadTermsAndCondition(token, sys_version);

                    value.data.term_cond_url = "";
                    string version = "version=";
                    //version += CheckLastestVersionOfTermsAndCondition();
                    version += WebConfigurationManager.AppSettings["version"].ToString();
                    string lang_ = "lang=";
                    lang_ += lang;

                    //string content_type = "content_type=privacy";

                    if (!is_read_terms)
                    {
                        value.data.term_cond_url = string.Format(WebConfigurationManager.AppSettings["term_cond_url"].ToString(), version, lang_);
                    }

                    value.data.term_cond_booking_url = string.Format(WebConfigurationManager.AppSettings["term_cond_booking_url"].ToString(), version, lang_);
                    value.data.policy_url = string.Format(WebConfigurationManager.AppSettings["term_polic_url"].ToString(), version, lang_);
                    value.data.default_landmark_lat = WebConfigurationManager.AppSettings["default_landmark_lat"].ToString();
                    value.data.default_landmark_lng = WebConfigurationManager.AppSettings["default_landmark_lng"].ToString();
                    value.data.pickup_url = WebConfigurationManager.AppSettings["pickup_url"].ToString();
                    value.data.dealer_appointment_url = WebConfigurationManager.AppSettings["dealer_appointment_url"].ToString(); 

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };

                    //ValidationModel.InvalidState state;
                    //if (value.data.banners.Count > 0)
                    //{
                    //    value.success = true;
                    //    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    //}
                    //else
                    //{
                    //    state = ValidationModel.InvalidState.E506;
                    //    value.success = false;
                    //    value.msg = new MsgModel() { code = ValidationModel.GetInvalidCode(state), text = ValidationModel.GetInvalidMessageNew(state, lang), store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private List<Config> GetAllConfig()
        {
            List<Config> list = new List<Config>();

            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT id, name, data_config
FROM system_config
ORDER BY id";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {
                        Config data;
                        foreach (DataRow row in dt.Rows)
                        {
                            data = new Config();
                            data.loadDataConfig(row);
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


        public int GetRemainingSurveyId(string member_id, string device_id, string platform)
        {
            return Get_Survey_should_show(member_id, device_id, platform);
        }


        public async Task<string> GetMemberIdByToken(string token)
        {
            string member_id = "";
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"                    
		                   select MEMBERID from T_CUSTOMER_TOKEN where TOKEN_NO = N'{0}'
		                  ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            member_id = dt.Rows[0]["MEMBERID"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return member_id;
        }

        public int Get_Survey_should_show(string member_id, string device_id, string platfrom)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"   

                    DECLARE @member_id nvarchar(100)=N'{0}'
                    DECLARE @device_id nvarchar(100)=N'{1}'
                    DECLARE @platform nvarchar(20) = N'{2}'
                    
                    IF (@member_id IS NOT NULL AND @member_id <> '')
                    BEGIN
		                select top 1 s.id AS id from sv.survey s
		                left join sv.question sq on s.id = sq.survey_id
		                left join sv.survey_sub_type sst on s.survey_type = sst.survey_type_id and s.survey_sub_type = sst.id
		                left join (select b.survey_id, a.postpone_date from sv.customer_answer a join sv.question b on a.question_id = b.id 
							                where  a.member_id = @member_id) as sp on s.id = sp.survey_id -- survey in now < postpone
		                where ((survey_type = 1)
		                or (survey_type in (2,3) and (select count(id) from sv.survey_member_group mb where mb.survey_id = s.id and (mb.member_id = @member_id or mb.member_id = @device_id)) > 0)
		                or (survey_type = 4 and sst.[name] = @platform))
		                and s.deleted_flag is null
		                and DATEADD(hour, 7, GETDATE()) BETWEEN start_date and end_date
		                and s.id not in (select b.survey_id 
                                         from sv.customer_answer a 
                                           join sv.question b on a.question_id = b.id 
                                           inner join sv.survey s on s.id = b.survey_id
		                                 where a.member_id = @member_id and (a.postpone_date is null OR s.times = 1)) -- survey not in answer success
		                and sq.id is not null
		                    
		                and (DATEADD(HOUR, 7, getdate()) > DATEADD(DAY, s.[interval], sp.postpone_date) or sp.postpone_date is null)
		                    order by s.ordinal ASC
                    END 
                    ELSE IF (@device_id IS NOT NULL AND @device_id <> '') AND (@member_id IS NULL OR @member_id = '')
                    BEGIN

                    select top 1 s.id AS id from sv.survey s
		                left join sv.question sq on s.id = sq.survey_id
		                left join sv.survey_sub_type sst on s.survey_type = sst.survey_type_id and s.survey_sub_type = sst.id
		                left join (select b.survey_id, a.postpone_date from sv.customer_answer a join sv.question b on a.question_id = b.id 
							                where  a.member_id IS NULL and a.device_id = @device_id) as sp on s.id = sp.survey_id -- survey in now < postpone
		                where ((survey_type = 1)
		                or (survey_type in (2,3) and (select count(id) from sv.survey_member_group mb where mb.survey_id = s.id and mb.member_id = @device_id) > 0)
		                or (survey_type = 4 and sst.[name] = @platform))
		                and s.deleted_flag is null
		                and DATEADD(hour, 7, GETDATE()) BETWEEN start_date and end_date
		                and s.id not in (select b.survey_id 
                                         from sv.customer_answer a 
                                           join sv.question b on a.question_id = b.id 
                                           inner join sv.survey s on s.id = b.survey_id
		                                 where a.device_id = @device_id and (a.postpone_date is null OR s.times = 1)) -- survey not in answer success
		                and sq.id is not null
		 
		                and (DATEADD(HOUR, 7, getdate()) > DATEADD(DAY, s.[interval], sp.postpone_date) or sp.postpone_date is null)
		                    order by s.ordinal ASC
		 
                    END  
		                    ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, member_id, device_id, platfrom)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            return row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return 0;
        }

        public async Task<string> GetMemberId(string token)
        {
            string member_id = "";
            try
            {
                MasterDataService service = new MasterDataService();
                member_id = await service.GetMemberIdByToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return member_id;
        }

        public List<int> GetSurveyHasNotQuestion()
        {
            List<int> survey_ids = new List<int>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"   
                        SELECT survey.id AS survey_id from sv.survey AS survey
                        WHERE survey.deleted_flag IS NULL
                          AND survey.[start_date] IS NOT NULL
                          AND survey.[end_date] IS NOT NULL
                          AND(DATEADD(HOUR, 7, GETDATE()) BETWEEN survey.[start_date] AND survey.[end_date])
                          AND id not in (select distinct survey_id from sv.question where deleted_flag IS NULL)
		                    ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            int survey_id;
                            survey_id = row["survey_id"] != DBNull.Value ? Convert.ToInt32(row["survey_id"]) : 0;

                            survey_ids.Add(survey_id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return survey_ids;
        }

        //    public List<AnswerNotCompleteModel> Get_customer_answered_not_complete()
        //    {
        //        List<AnswerNotCompleteModel> list = new List<AnswerNotCompleteModel>();
        //        AnswerNotCompleteModel value = new AnswerNotCompleteModel();

        //       try {
        //            using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //            {
        //                string cmd = @"   
        //                         SELECT  
        //                            member_id AS member_id, 
        //                            device_id AS device_id,
        //                            survey_id AS survey_id,
        //                            times AS times,
        //                            interval AS interval 
        //                            FROM sv.vwCustomer_Answered_Not_Complete
        //                  ";

        //                using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
        //                {
        //                    foreach (DataRow row in dt.Rows)
        //                    {
        //                        value.member_id = row["member_id"] != DBNull.Value ? Convert.ToInt32(row["member_id"]) : 0;
        //                        value.device_id = row["device_id"] != DBNull.Value ? Convert.ToInt32(row["device_id"]) : 0;
        //                        value.survey_id = row["survey_id"] != DBNull.Value ? Convert.ToInt32(row["survey_id"]) : 0;
        //                        value.time_type = row["times"] != DBNull.Value ? Convert.ToBoolean(Convert.ToInt32(row["times"])) : false;
        //                        value.interval = row["interval"] != DBNull.Value ? Convert.ToInt32(row["interval"]) : 0;

        //                        list.Add(value);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //        return list;
        //    }
        //}

        public async Task<bool> CheckReadTermsAndCondition(string token, int sys_version)
        {
            bool value = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"   
                   DECLARE @member_id NVARCHAR(50);
                   DECLARE @is_read_terms bit;
                   DECLARE @lastest_terms_version int = N'{1}';
                   DECLARE @current_read_terms_version int;

                   SET @member_id = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')
                   SET @current_read_terms_version = (select top 1 [version] from terms_and_condition_stamp_read 
                                                      where member_id IS NOT NULL AND member_id <> '' AND member_id = @member_id 
                                                            ORDER BY [version] DESC)
                   
                   IF @lastest_terms_version = @current_read_terms_version
                	    SET @is_read_terms = 1
                   ELSE
                	    SET @is_read_terms = 0


                   SELECT @is_read_terms AS is_read_terms";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, sys_version)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            value = Convert.ToInt16(dt.Rows[0]["is_read_terms"]) == 1 ? true : false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Check read terms and condition");
                throw ex;
            }

            return value;
        }

        public string CheckLastestVersionOfTermsAndCondition()
        {
            string value = "";
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"SELECT TOP 1 version AS version FROM  terms_condition WHERE is_active = 1 AND delete_flag IS NULL ORDER BY version DESC";

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            value = row["version"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteExceptionLog(ex, "Check read terms and condition");
                throw ex;
            }
            return value;
        }
    }
}