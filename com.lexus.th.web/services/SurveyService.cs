using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;
using System.Data.SqlClient;

namespace com.lexus.th.web
{
    public class SurveyService
    {
        private string conn;
        public SurveyService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetSurvey(string searchValue)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'

SELECT		[id],
            [survey_th],
            [survey_en],
            [allow_postpone],
            [interval],
            [times],
            [start_date],
            [end_date],
            [ordinal],
            [created_date],
            [created_user],
            [updated_date],
            [updated_user],
            [deleted_flag],
            [delete_date],
            [delete_user]
FROM		[sv].[survey]
WHERE		[deleted_flag] IS NULL AND 
            (  ISNULL([survey_th], '') LIKE '%' + @Value + '%'
			OR ISNULL([survey_en], '') LIKE '%' + @Value + '%'
			)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(searchValue));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetSurveyById(string id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[id],
            [survey_th],
            [survey_en],
            [allow_postpone],
            [interval],
            [times],
            [start_date],
            [end_date],
            [ordinal],
            [created_date],
            [created_user],
            [updated_date],
            [updated_user],
            [deleted_flag],
            [delete_date],
            [delete_user],
            [survey_type],
            [survey_sub_type],
            [file_path]
FROM		[sv].[survey]
WHERE		[id] = @ID
ORDER BY [ordinal] ";

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
        public DataRow GetSurveyMemberGroupBySurveyId(string survey_id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[member_id]
FROM		[sv].[survey_member_group]
WHERE		[survey_id] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(survey_id));

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
        public DataTable GetSurveyMemberGroupListBySurveyId(string survey_id)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[member_id]
FROM		[sv].[survey_member_group]
WHERE		[survey_id] = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(survey_id));

                    dt = db.GetDataTableFromCommandText(cmd);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public void AddSurvey(string survey_th, string survey_en, string allow_postpone, string interval, string times, string start_date, string end_date, string ordinal, string survey_type, string survey_sub_type, string user, string file_path, List<SurveyMemberGroupModel> member_group)
        {
            try
            {
//DECLARE @survey_th  NVARCHAR(200) = N'{0}'
//DECLARE @survey_en  NVARCHAR(200) = N'{1}'
                string cmd = @"


DECLARE @allow_postpone  INT = N'{0}'
DECLARE @interval  INT = N'{1}'
DECLARE @times  INT = N'{2}'
DECLARE @start_date  NVARCHAR(20) = N'{3}'
DECLARE @end_date  NVARCHAR(20) = N'{4}'
DECLARE @ordinal  INT = N'{5}'
DECLARE @user  NVARCHAR(50) = N'{6}'
DECLARE @survey_type  INT = N'{7}'
DECLARE @survey_sub_type  INT = N'{8}'
DECLARE @file_path  NVARCHAR(500) = N'{9}'
DECLARE @id INT 

INSERT INTO [sv].[survey] ( [survey_th],[survey_en],[allow_postpone],[interval],[times],[start_date],[end_date],[ordinal],[survey_type],[survey_sub_type],[file_path],[created_date],[created_user])
VALUES (
CASE LEN(@survey_th) WHEN 0 THEN NULL ELSE @survey_th END,
CASE LEN(@survey_en) WHEN 0 THEN NULL ELSE @survey_en END,
CASE LEN(@allow_postpone) WHEN 0 THEN NULL ELSE @allow_postpone END,
CASE LEN(@interval) WHEN 0 THEN NULL ELSE @interval END,
CASE LEN(@times) WHEN 0 THEN NULL ELSE @times END,
CASE LEN(@start_date) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @start_date, 103) END,
CASE LEN(@end_date) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @end_date + ' 23:59:59', 103) END,
CASE LEN(@ordinal) WHEN 0 THEN NULL ELSE @ordinal END,
CASE LEN(@survey_type) WHEN 0 THEN NULL ELSE @survey_type END,
CASE LEN(@survey_sub_type) WHEN 0 THEN NULL ELSE @survey_sub_type END,
CASE LEN(@file_path) WHEN 0 THEN NULL ELSE @file_path END,
DATEADD(HOUR, 7, GETDATE()),
@USER     
)

SET @id = SCOPE_IDENTITY()

";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        //WebUtility.GetSQLTextValue(survey_th),
                        //WebUtility.GetSQLTextValue(survey_en),
                        WebUtility.GetSQLTextValue(allow_postpone),
                        WebUtility.GetSQLTextValue(interval),
                        WebUtility.GetSQLTextValue(times),
                        WebUtility.GetSQLTextValue(start_date),
                        WebUtility.GetSQLTextValue(end_date),
                        WebUtility.GetSQLTextValue(ordinal),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(survey_type),
                        WebUtility.GetSQLTextValue(survey_sub_type),
                          WebUtility.GetSQLTextValue(file_path)
                        );
                    db.ParamterList.Add(new SqlParameter("@survey_th", survey_th));
                    db.ParamterList.Add(new SqlParameter("@survey_en", survey_th));
                   
                    if (survey_type == "2" || survey_type == "3")
                    {
                        if (member_group != null && member_group.Any())
                        {
                           
                            foreach (SurveyMemberGroupModel member in member_group)
                            {

                                cmd += string.Format(@"
                                                          INSERT INTO [sv].[survey_member_group]
                                                                 ([survey_id],
                                                                 [member_id]
                                                                )
                                                           VALUES
                                                                 (@id          
                                                                 ,N'{0}'
                                                                )", member.MemberId);

                            }
                        }
                    }
                    
                    db.ExecuteNonQueryFromCommandText(cmd);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateSurvey(string survey_th, string survey_en, string allow_postpone, string interval, string times, string start_date, string end_date, string ordinal, string survey_type, string survey_sub_type, string user, string id, string file_path, List<SurveyMemberGroupModel> member_group)
        {
            try
            {
//DECLARE @survey_th  NVARCHAR(200) = N'{0}'
//DECLARE @survey_en  NVARCHAR(200) = N'{1}'
                string cmd = @"


DECLARE @allow_postpone  INT = N'{0}'
DECLARE @interval  INT = N'{1}'
DECLARE @times  INT = N'{2}'
DECLARE @start_date  NVARCHAR(20) = N'{3}'
DECLARE @end_date  NVARCHAR(20) = N'{4}'
DECLARE @ordinal  INT = N'{5}'
DECLARE @user  NVARCHAR(50) = N'{6}'
DECLARE @id INT = N'{7}'
DECLARE @survey_type  INT = N'{8}'
DECLARE @survey_sub_type  INT = N'{9}'
DECLARE @file_path  NVARCHAR(500) = N'{10}'

UPDATE [sv].[survey]
SET		survey_th = CASE LEN(@survey_th) WHEN 0 THEN NULL ELSE @survey_th END,
        survey_en = CASE LEN(@survey_en) WHEN 0 THEN NULL ELSE @survey_en END,
        allow_postpone = CASE LEN(@allow_postpone) WHEN 0 THEN NULL ELSE @allow_postpone END,
        interval= CASE LEN(@interval) WHEN 0 THEN NULL ELSE @interval END,
        times= CASE LEN(@times) WHEN 0 THEN NULL ELSE @times END,
        start_date= CASE LEN(@start_date) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @start_date, 103) END,
        end_date= CASE LEN(@end_date) WHEN 0 THEN NULL ELSE CONVERT(DATETIME, @end_date + ' 23:59:59', 103) END,
        ordinal= CASE LEN(@ordinal) WHEN 0 THEN NULL ELSE @ordinal END,
        survey_type= CASE LEN(@survey_type) WHEN 0 THEN NULL ELSE @survey_type END,
        survey_sub_type= CASE LEN(@survey_sub_type) WHEN 0 THEN NULL ELSE @survey_sub_type END,
        file_path= CASE LEN(@file_path) WHEN 0 THEN NULL ELSE @file_path END,
        updated_date= DATEADD(HOUR, 7, GETDATE()),
        updated_user= @USER 
        
WHERE	id = @id";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                       //WebUtility.GetSQLTextValue(survey_th),
                       //WebUtility.GetSQLTextValue(survey_en),
                       WebUtility.GetSQLTextValue(allow_postpone),
                       WebUtility.GetSQLTextValue(interval),
                       WebUtility.GetSQLTextValue(times),
                       WebUtility.GetSQLTextValue(start_date),
                       WebUtility.GetSQLTextValue(end_date),
                       WebUtility.GetSQLTextValue(ordinal),
                       WebUtility.GetSQLTextValue(user),
                       WebUtility.GetSQLTextValue(id),
                       WebUtility.GetSQLTextValue(survey_type),
                       WebUtility.GetSQLTextValue(survey_sub_type),
                        WebUtility.GetSQLTextValue(file_path)

                       );
                    db.ParamterList.Add(new SqlParameter("@survey_th", survey_th));
                    db.ParamterList.Add(new SqlParameter("@survey_en", survey_th));

                    if (survey_type == "1" || survey_type == "4")
                    {
                        cmd += string.Format(@"
                                                DELETE FROM [sv].[survey_member_group] WHERE [survey_id] = @id 
                                            ");
                    }
                     if (survey_type == "2" || survey_type == "3")
                    {
                        if(survey_sub_type=="1" || survey_sub_type=="3")
                        {
                            cmd += string.Format(@"
    DELETE FROM [sv].[survey_member_group] WHERE [survey_id] = @id 
");
                        }
                  
                            
                    
                        if (member_group != null && member_group.Any())
                        {
cmd += string.Format(@"
    DELETE FROM [sv].[survey_member_group] WHERE [survey_id] = @id 
");
                            foreach (SurveyMemberGroupModel member in member_group)
                            {

                                cmd += string.Format(@"
    INSERT INTO [sv].[survey_member_group]
           ([survey_id],
           [member_id]
          )
     VALUES
           (@id          
           ,N'{0}'
          )", member.MemberId);

                            }
                        }
                    }
                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteSurvey(string id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'

UPDATE	[sv].[survey]
SET		DELETED_FLAG = 'Y',
        DELETE_DATE = DATEADD(HOUR, 7, GETDATE()),
        DELETE_USER = @USER
WHERE	ID = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUploadImage(string parrentId, string type)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @ParrentId INT = N'{0}'
DECLARE @Type NVARCHAR(10) = N'{1}'
SELECT [Id]
      ,[Parent_Id]
      ,[Type]
      ,[Page]
      ,[File_Name]
      ,[File_Path]
      ,[Original_File_Name]
      ,[Created_Date]
      ,[Created_User]
  FROM [dbo].[upload_Image]

WHERE [Parent_Id]=@ParrentId AND [Type]=@Type AND [Page]='EVENTS'
ORDER BY ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(parrentId), WebUtility.GetSQLTextValue(type));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        internal DataTable GetEventGroup()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT ID, NAME_EN
FROM event_group ORDER BY ID
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

        public DataTable GetQuestionBySurveyId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                    DECLARE @ID INT = N'{0}'
                                    
                                    SELECT		  [id],
                                                  [survey_id],
                                                  [question_th],
                                                  [question_en],
                                                  [question_type],
                                                  [allow_suggestion],
                                                  [allow_back],
                                                  [is_depend_on_other_questions],
                                                  [is_require_all],
                                                  [is_required],
                                                  [ordinal],
                                                  [created_date],
                                                  [created_user],
                                                  [updated_date],
                                                  [updated_user],
                                                  [deleted_flag],
                                                  [delete_date],
                                                  [delete_user]                                                       
                                    FROM		[sv].[question]
                                    WHERE		[survey_id] = @ID and deleted_flag is NULL
                                    ORDER BY [ordinal]
";


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
        public DataTable GetChoiceByQuestionId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                    DECLARE @id INT = N'{0}'
                                    
                                    SELECT		[id],
                                                [question_id],
                                                [choice_th],
                                                [choice_en],
                                                [is_image],
                                                [image_path],
                                                [image_url],
                                                [ordinal],
                                                [created_date],
                                                [created_user],
                                                [updated_date],
                                                [updated_user],
                                                [deleted_flag],
                                                [delete_date],
                                                [delete_user]                                     
                                    FROM		[sv].[choice]
                                    WHERE		[question_id] = @id and deleted_flag is NULL
                                    ORDER BY [ordinal]";

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
        public DataTable GetChoiceRPByQuestionId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                    DECLARE @id INT = N'{0}'
                                    
                                    SELECT		[id],
                                                [question_id],
                                                [choice_th]+' / '+[choice_en] as [choice_th],
                                                [choice_en],
                                                [is_image],
                                                [image_path],
                                                [image_url],
                                                [ordinal],
                                                [created_date],
                                                [created_user],
                                                [updated_date],
                                                [updated_user],
                                                [deleted_flag],
                                                [delete_date],
                                                [delete_user]                                     
                                    FROM		[sv].[choice]
                                    WHERE		[question_id] = @id and deleted_flag is NULL 
                                    ORDER BY [ordinal]
";


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
        public DataRow GetQuestionById(string id)
        {
            DataRow row = null;
            try
            {




                string cmd = @"
DECLARE @id INT = N'{0}'

SELECT		[id],
            [survey_id],
            [question_th],
            [question_en],
            [question_type],
            [allow_suggestion],
            [allow_back],
            [is_depend_on_other_questions],
            [is_require_all],
            [is_required],
            [ordinal],
            [suggestion_th],
            [suggestion_en]                                              
FROM		[sv].[question]
WHERE		[id] = @id";

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
        public DataTable GetPreviousQuestionBySurveyIdAndQuestionId(string surveyId, string questionId)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                    DECLARE @survey_id INT = N'{0}'
                                    DECLARE @id INT = N'{1}'
                                    
                                    SELECT		  [id],
                                                  [survey_id],
                                                  CONCAT('(', [id],')',[question_th] ) as question_th,
                                                  CONCAT('(', [id],')',[question_en] ) as question_en,
                                                  [question_type],
                                                  [allow_suggestion],
                                                  [allow_back],
                                                  [is_depend_on_other_questions],
                                                  [is_require_all],
                                                  [is_required],
                                                  [ordinal]                                                                                            
                                    FROM		[sv].[question]
                                    where survey_id = @survey_id AND id != @id AND question_type in (3,4) AND deleted_flag is NULL 
                                    ORDER BY [ordinal]
";


                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(surveyId),
                        WebUtility.GetSQLTextValue(questionId));

                    dt = db.GetDataTableFromCommandText(cmd);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public void AddQuestion(string survay_id, string question_th, string question_en, string question_type, string allow_suggestion, string allow_back, string is_depend, string is_required, string is_require_all, string ordinal, string user,string suggestion_th,string suggestion_en, List<QuestionRuleModel> questionRules)
        {
            try
            {
//DECLARE @question_th NVARCHAR(200) = N'{1}'
//DECLARE @question_en NVARCHAR(200) = N'{2}'
//DECLARE @suggestion_th NVARCHAR(200) = N'{11}'
//DECLARE @suggestion_en NVARCHAR(200) = N'{12}'
                string cmd = @"
DECLARE @id INT
DECLARE @survey_id  INT = N'{0}'

DECLARE @question_type  INT = N'{1}'
DECLARE @allow_suggestion  INT = N'{2}'
DECLARE @allow_back  INT = N'{3}'
DECLARE @is_depend_on_other_questions  INT = N'{4}'
DECLARE @is_required  INT = N'{5}'
DECLARE @is_require_all  INT = N'{6}'
DECLARE @ordinal  INT = N'{7}'
DECLARE @user  NVARCHAR(50) = N'{8}'


INSERT INTO [sv].[question] (survey_id, question_th,question_en,question_type,allow_suggestion,allow_back,is_depend_on_other_questions,is_required,is_require_all,ordinal,suggestion_th,suggestion_en, created_date, created_user)
VALUES (
        CASE LEN(@survey_id) WHEN 0 THEN NULL ELSE @survey_id END,
        CASE LEN(@question_th) WHEN 0 THEN NULL ELSE @question_th END,
        CASE LEN(@question_en) WHEN 0 THEN NULL ELSE @question_en END,
        CASE LEN(@question_type) WHEN 0 THEN NULL ELSE @question_type END,
        CASE LEN(@allow_suggestion) WHEN 0 THEN NULL ELSE @allow_suggestion END,
        CASE LEN(@allow_back) WHEN 0 THEN NULL ELSE @allow_back END,
        CASE LEN(@is_depend_on_other_questions) WHEN 0 THEN NULL ELSE @is_depend_on_other_questions END,
        CASE LEN(@is_required) WHEN 0 THEN NULL ELSE @is_required END,
        CASE LEN(@is_require_all) WHEN 0 THEN NULL ELSE @is_require_all END,
        CASE LEN(@ordinal) WHEN 0 THEN NULL ELSE @ordinal END,
        CASE LEN(@suggestion_th) WHEN 0 THEN NULL ELSE @suggestion_th END,  
        CASE LEN(@suggestion_en) WHEN 0 THEN NULL ELSE @suggestion_en END,
        DATEADD(HOUR, 7, GETDATE()), 
        @USER)

SET @id = SCOPE_IDENTITY()
";
                List<string> questionTypeAddDefaultChoice = new List<string>() { "3", "4" };
                if (questionTypeAddDefaultChoice.Contains(question_type)) //dropdown , checkbox , radio
                {
                    cmd += @"

                                INSERT INTO [sv].[choice](question_id, choice_th, choice_en, is_image, image_path,ordinal,created_date, created_user)
                                                  VALUES(@id,N'None',N'None',0,NULL,0, DATEADD(HOUR, 7, GETDATE()), @USER)";
                }
                cmd = string.Format(cmd,
                WebUtility.GetSQLTextValue(survay_id),
                //WebUtility.GetSQLTextValue(question_th),
                //WebUtility.GetSQLTextValue(question_en),
                WebUtility.GetSQLTextValue(question_type),
                WebUtility.GetSQLTextValue(allow_suggestion),
                WebUtility.GetSQLTextValue(allow_back),
                WebUtility.GetSQLTextValue(is_depend),
                WebUtility.GetSQLTextValue(is_required),
                WebUtility.GetSQLTextValue(is_require_all),
                WebUtility.GetSQLTextValue(ordinal),
                WebUtility.GetSQLTextValue(user)
                //WebUtility.GetSQLTextValue(suggestion_th),
                //WebUtility.GetSQLTextValue(suggestion_en)


                );
               
               
                if (is_depend == "1")
                {
                    foreach (var rule in questionRules)
                    {


                        if (rule.Status == "ADD")
                        {
                            cmd += string.Format(@"
INSERT INTO [sv].[question_rule] ([question_id],[previous_question_id],[choice_id],[created_date],[created_user])
VALUES (
        @id, N'{0}', N'{1}',       
        DATEADD(HOUR, 7, GETDATE()), 
        @USER) 
", rule.PreviousQuestionId, rule.ChoiceId);
                        }
                        //                    else if (rule.Status == "DEL")
                        //                    {
                        //                        cmd = string.Format(@"
                        //DELETE [sv].[question_rule] WHERE  [question_id]= N'{0}' AND [previous_question_id] =  N'{1}' AND [choice_id] = N'{2}'
                        //", rule.QuestionId, rule.PreviousQuestionId, rule.ChoiceId);
                        //                    }
                    }
                }

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ParamterList.Add(new SqlParameter("@question_th", question_th));
                    db.ParamterList.Add(new SqlParameter("@question_en", question_en));
                    db.ParamterList.Add(new SqlParameter("@suggestion_th", suggestion_th));
                    db.ParamterList.Add(new SqlParameter("@suggestion_en", suggestion_en));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateQuestion(string survay_id, string question_th, string question_en, string question_type, string allow_suggestion, string allow_back, string is_depend, string is_required, string is_require_all, string ordinal, string user, string id, string suggestion_th, string suggestion_en, List<QuestionRuleModel> questionRules)
        {
            try
            {
//DECLARE @question_th NVARCHAR(200) = N'{1}'
//DECLARE @question_en NVARCHAR(200) = N'{2}'
//DECLARE @suggestion_th NVARCHAR(200) = N'{12}'
//DECLARE @suggestion_en NVARCHAR(200) = N'{13}'
                string cmd = @"

DECLARE @survey_id  INT = N'{0}'
DECLARE @question_type  INT = N'{1}'
DECLARE @allow_suggestion  INT = N'{2}'
DECLARE @allow_back  INT = N'{3}'
DECLARE @is_depend_on_other_questions  INT = N'{4}'
DECLARE @is_required  INT = N'{5}'
DECLARE @is_require_all  INT = N'{6}'
DECLARE @ordinal  INT = N'{7}'
DECLARE @user  NVARCHAR(50) = N'{8}'
DECLARE @id  INT = N'{9}'


UPDATE	[sv].[question]
SET		                                           
        [survey_id]                         = CASE LEN(@survey_id) WHEN 0 THEN NULL ELSE @survey_id END,    
        [question_th]                       = CASE LEN(@question_th) WHEN 0 THEN NULL ELSE @question_th END,    
        [question_en]                       = CASE LEN(@question_en) WHEN 0 THEN NULL ELSE @question_en END,    
        [question_type]                     = CASE LEN(@question_type) WHEN 0 THEN NULL ELSE @question_type END,    
        [allow_suggestion]                  = CASE LEN(@allow_suggestion) WHEN 0 THEN NULL ELSE @allow_suggestion END,    
        [allow_back]                        = CASE LEN(@allow_back) WHEN 0 THEN NULL ELSE @allow_back END,    
        [is_depend_on_other_questions]      = CASE LEN(@is_depend_on_other_questions) WHEN 0 THEN NULL ELSE @is_depend_on_other_questions END,    
        [is_require_all]                    = CASE LEN(@is_require_all) WHEN 0 THEN NULL ELSE @is_require_all END,    
        [is_required]                       = CASE LEN(@is_required) WHEN 0 THEN NULL ELSE @is_required END,    
        [ordinal]                           = CASE LEN(@ordinal) WHEN 0 THEN NULL ELSE @ordinal END,   
        [suggestion_th]                     = CASE LEN(@suggestion_th) WHEN 0 THEN NULL ELSE @suggestion_th END,   
        [suggestion_en]                     = CASE LEN(@suggestion_en) WHEN 0 THEN NULL ELSE @suggestion_en END,  
        [updated_date] = DATEADD(HOUR, 7, GETDATE()),
        [updated_user] = @USER
   
WHERE	id = @id";
                cmd = string.Format(cmd,
        WebUtility.GetSQLTextValue(survay_id),
        //WebUtility.GetSQLTextValue(question_th),
        //WebUtility.GetSQLTextValue(question_en),
        WebUtility.GetSQLTextValue(question_type),
        WebUtility.GetSQLTextValue(allow_suggestion),
        WebUtility.GetSQLTextValue(allow_back),
        WebUtility.GetSQLTextValue(is_depend),
        WebUtility.GetSQLTextValue(is_required),
        WebUtility.GetSQLTextValue(is_require_all),
        WebUtility.GetSQLTextValue(ordinal),
        WebUtility.GetSQLTextValue(user),
        WebUtility.GetSQLTextValue(id)
        //WebUtility.GetSQLTextValue(suggestion_th),
        //WebUtility.GetSQLTextValue(suggestion_en)
        );
                if (is_depend == "1")
                {
                    foreach (var rule in questionRules)
                    {


                        if (rule.Status == "ADD")
                        {
                            cmd += string.Format(@"
INSERT INTO [sv].[question_rule] ([question_id],[previous_question_id],[choice_id],[created_date],[created_user])
VALUES (
        @id, N'{1}', N'{2}',       
        DATEADD(HOUR, 7, GETDATE()), 
        @USER) 
", rule.QuestionId, rule.PreviousQuestionId, rule.ChoiceId);
                        }
                        else if (rule.Status == "DEL")
                        {
                            cmd += string.Format(@"
                    DELETE [sv].[question_rule] WHERE  [question_id]= @id AND [previous_question_id] =  N'{0}' AND [choice_id] = N'{1}'
                    ", rule.PreviousQuestionId, rule.ChoiceId);
                        }
                    }
                }
                else if (is_depend == "0")
                {
                    cmd += @"
                    DELETE [sv].[question_rule] WHERE  [question_id]= @id 
                    ";
                }
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ParamterList.Add(new SqlParameter("@question_th", question_th));
                    db.ParamterList.Add(new SqlParameter("@question_en", question_en));
                    db.ParamterList.Add(new SqlParameter("@suggestion_th", suggestion_th));
                    db.ParamterList.Add(new SqlParameter("@suggestion_en", suggestion_en));
                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteQuestion(string question_id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'


UPDATE	[sv].[question]
SET		DELETED_FLAG = 'Y',
        DELETE_DATE = DATEADD(HOUR, 7, GETDATE()),
        DELETE_USER = @USER
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(question_id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow GetChoiceById(string id)
        {
            DataRow row = null;
            try
            {


                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT		[id],
            [question_id],
            [choice_th],
            [choice_en],
            [is_image],
            [image_path],
            [image_url],
            [ordinal]
FROM		[sv].[choice]
WHERE		[ID] = @ID";

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
        public void AddChoice(string question_id, string choice_th, string choice_en, string is_image, string image_path, string image_url, string ordinal, string user)
        {
            try
            {
//DECLARE @choice_th NVARCHAR(200) = N'{1}'
//DECLARE @choice_en NVARCHAR(200) = N'{2}'
                string cmd = @"
DECLARE @question_id INT = N'{0}'
DECLARE @is_image  INT = N'{1}'
DECLARE @image_path NVARCHAR(500) = N'{2}'
DECLARE @image_url NVARCHAR(500) = N'{3}'
DECLARE @ordinal  INT = N'{4}'
DECLARE @user  NVARCHAR(50) = N'{5}'

   INSERT INTO [sv].[choice](question_id, choice_th, choice_en, is_image, image_path,image_url,ordinal,created_date, created_user)
   VALUES (
    CASE LEN(@question_id) WHEN 0 THEN NULL ELSE @question_id END,
    CASE LEN(@choice_th) WHEN 0 THEN NULL ELSE @choice_th END,
    CASE LEN(@choice_en) WHEN 0 THEN NULL ELSE @choice_en END,
    CASE LEN(@is_image) WHEN 0 THEN NULL ELSE @is_image END,
    CASE LEN(@image_path) WHEN 0 THEN NULL ELSE @image_path END,
    CASE LEN(@image_url) WHEN 0 THEN NULL ELSE @image_url END,
    CASE LEN(@ordinal) WHEN 0 THEN NULL ELSE @ordinal END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER
   
        )";


                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(question_id),
                        //WebUtility.GetSQLTextValue(choice_th),
                        //WebUtility.GetSQLTextValue(choice_en),
                        WebUtility.GetSQLTextValue(is_image),
                        WebUtility.GetSQLTextValue(image_path),
                        WebUtility.GetSQLTextValue(image_url),
                        WebUtility.GetSQLTextValue(ordinal),
                        WebUtility.GetSQLTextValue(user)
                        );
                    db.ParamterList.Add(new SqlParameter("@choice_th", choice_th));
                    db.ParamterList.Add(new SqlParameter("@choice_en", choice_en));
                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateChoice(string id, string choice_th, string choice_en, string is_image, string image_path, string image_url, string ordinal, string user)
        {
            try
            {
//DECLARE @choice_th NVARCHAR(200) = N'{1}'
//DECLARE @choice_en NVARCHAR(200) = N'{2}'
                string cmd = @"
DECLARE @id INT = N'{0}'
DECLARE @is_image  INT = N'{1}'
DECLARE @image_path NVARCHAR(500) = N'{2}'
DECLARE @image_url NVARCHAR(500) = N'{3}'
DECLARE @ordinal  INT = N'{4}'
DECLARE @user  NVARCHAR(50) = N'{5}'


UPDATE [sv].[choice]
SET		choice_th = CASE LEN(@choice_th) WHEN 0 THEN NULL ELSE @choice_th END,
        choice_en = CASE LEN(@choice_en) WHEN 0 THEN NULL ELSE @choice_en END,
        is_image = CASE LEN(@is_image) WHEN 0 THEN NULL ELSE @is_image END,
        image_path= CASE LEN(@image_path) WHEN 0 THEN NULL ELSE @image_path END,
        image_url= CASE LEN(@image_url) WHEN 0 THEN NULL ELSE @image_url END,       
        ordinal= CASE LEN(@ordinal) WHEN 0 THEN NULL ELSE @ordinal END,
        updated_date= DATEADD(HOUR, 7, GETDATE()),
        updated_user= @USER     
WHERE	id = @id";



                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                       WebUtility.GetSQLTextValue(id),
                       //WebUtility.GetSQLTextValue(choice_th),
                       //WebUtility.GetSQLTextValue(choice_en),
                       WebUtility.GetSQLTextValue(is_image),
                       WebUtility.GetSQLTextValue(image_path),
                       WebUtility.GetSQLTextValue(image_url),
                       WebUtility.GetSQLTextValue(ordinal),
                       WebUtility.GetSQLTextValue(user)
                       );
                    db.ParamterList.Add(new SqlParameter("@choice_th", choice_th));
                    db.ParamterList.Add(new SqlParameter("@choice_en", choice_en));
                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteChoice(string choice_id, string user)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'
DECLARE @USER  NVARCHAR(50) = N'{1}'


UPDATE	[sv].[choice]
SET		DELETED_FLAG = 'Y',
        DELETE_DATE = DATEADD(HOUR, 7, GETDATE()),
        DELETE_USER = @USER
WHERE	id = @ID";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(choice_id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetQuestionRuleByQuestionId(string id)
        {
            DataTable dt = null;
            try
            {

                string cmd = @"
                                    DECLARE @ID INT = N'{0}'
                                    
                                    SELECT		qr.[id],
                                                qr.[question_id],
                                                qr.[previous_question_id],
                                                qr.[choice_id],    
                                                q.[question_th],  
                                                q.[question_en],
                                                q.[question_type]                                               
                                    FROM		[sv].[question_rule] qr                                   
                                    LEFT JOIN [sv].[question] q on   qr.[previous_question_id] = q.id
                                    WHERE		[question_id] = @ID and q.deleted_flag is NULL";


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

    }
}