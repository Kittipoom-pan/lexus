using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class PreferenceService
    {
        private string conn;
        public PreferenceService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServicePreferenceModel> GetDataPreference(string lang, int customer_id, string v, string p)
        {
            ServicePreferenceModel value = new ServicePreferenceModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    value.data = new _ServicePreferenceData();
                    value.data.preference = new List<PreferenceModel>();
                    value.data.preference = await GetPreference(lang);

                    value.success = true;
                    value.msg = new MsgModel() { code = 0, text = "Success", store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<List<PreferenceModel>> GetPreference(string lang)
        {
            List<PreferenceModel> list = new List<PreferenceModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{0}'

SELECT id, CASE  
  WHEN @LANG = 'EN' THEN name_en 
  ELSE name_th END AS name, is_require_answer
FROM preference 
WHERE is_active = 1 AND DEL_FLAG IS NULL
ORDER BY [order]
";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            PreferenceModel preference_model = new PreferenceModel();
                            preference_model.id = Convert.ToInt32(row["id"]);
                            preference_model.name = row["name"].ToString();
                            preference_model.is_require_answer = Convert.ToBoolean(int.Parse(row["is_require_answer"].ToString()));

                            preference_model.preference_choice = new List<PreferenceChoiceModel>();
                            preference_model.preference_choice = GetPreferenceChoice(lang, preference_model.id);

                            list.Add(preference_model);
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

        private List<PreferenceChoiceModel> GetPreferenceChoice(string lang, int preference_id)
        {
            List<PreferenceChoiceModel> list = new List<PreferenceChoiceModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{0}'

SELECT id, icon_image_url, is_optional,
    CASE  
      WHEN @LANG = 'EN' THEN name_en 
      ELSE name_th END AS name,  
    CASE  
      WHEN @LANG = 'EN' THEN optional_text 
      ELSE optional_text_th END AS optional_text,
    CASE  
      WHEN @LANG = 'EN' THEN optional_header 
      ELSE optional_header_th END AS optional_header
FROM preference_choice 
WHERE preference_id = {1} AND is_active = 1 AND DEL_FLAG IS NULL 
ORDER BY [order]
";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, preference_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            PreferenceChoiceModel preference_choice_model = new PreferenceChoiceModel();
                            preference_choice_model.id = Convert.ToInt32(row["id"]);
                            preference_choice_model.name = row["name"].ToString();
                            preference_choice_model.icon_image_url = row["icon_image_url"].ToString();
                            preference_choice_model.is_optional = Convert.ToBoolean(int.Parse(row["is_optional"].ToString()));
                            preference_choice_model.optional_text = row["optional_text"].ToString();
                            preference_choice_model.optional_header = row["optional_header"].ToString();

                            list.Add(preference_choice_model);
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