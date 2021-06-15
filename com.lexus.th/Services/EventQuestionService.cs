using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class EventQuestionService
    {
        private string conn;

        public EventQuestionService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceEventQuestionModel> GetEventQuestions(string v, string p, string token, string lang, int event_id)
        {
            ServiceEventQuestionModel value = new ServiceEventQuestionModel();
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
                    value.data = new _ServiceEventQuestionData();
                    value.data.car_owner =await GetQuestionByType(event_id, lang, (int)EventQuestionUserType.car_owner);
                    value.data.representative =await GetQuestionByType(event_id, lang, (int)EventQuestionUserType.representative);
                    value.data.follower_car_owner =await  GetQuestionByType(event_id, lang, (int)EventQuestionUserType.follower_car_owner);
                    value.data.follower_representative =await GetQuestionByType(event_id, lang, (int)EventQuestionUserType.follower_representative);


                    value.success = true;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }



        public async Task<List<_EventQuestionData>> GetQuestionByType (int event_id, string lang, int question_type)
        {
            List<_EventQuestionData> list = new List<_EventQuestionData>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                       DECLARE @lang NVARCHAR(5) = N'{0}'
                       SELECT
                       id,
                       CASE
                           WHEN @lang = 'TH' THEN question_th
                           WHEN @lang = 'EN' THEN question_en
                       END AS question,
                       answer_type
                       FROM[dbo].[event_question]
                       WHERE is_active = 1 AND deleted_flag IS NULL AND event_id = N'{1}' AND question_type = N'{2}'
                       ORDER BY [order]";


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, event_id, question_type)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            _EventQuestionData data = new _EventQuestionData();
                            data.id = Convert.ToInt32(row["id"]);
                            data.name = row["question"].ToString();
                            data.answer_type = row["answer_type"] != DBNull.Value ? Convert.ToInt32(row["answer_type"]) == (int)AnswerType.choice ? "choice" : Convert.ToInt32(row["answer_type"]) == (int)AnswerType.text ? "text"  : "" : "";
                            data.choices = await GetChoices(lang, Convert.ToInt32(row["id"]));
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

        public async Task<List<Choices>> GetChoices(string lang, int question_id)
        {
            List<Choices> list = new List<Choices>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                            DECLARE @lang NVARCHAR(5)= N'{0}';
                            SELECT 
                            id,
                            CASE
                            	WHEN @lang ='TH' THEN Answer_th
                            	WHEN @lang ='EN' THEN Answer_en
                            END AS answer,
                            is_optional
                            FROM [dbo].[event_answer]
                            WHERE is_active = 1 AND deleted_flag IS NULL AND question_id = N'{1}'
                            ORDER BY [order]";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, question_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Choices data = new Choices();
                            data.id = int.Parse(row["id"].ToString());
                            data.name = row["answer"].ToString();
                            data.is_optional = (bool)row["is_optional"]; //can use (bool) because in db i setup not allow null for this column.
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

        
    }
}