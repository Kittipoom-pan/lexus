using AppLibrary.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class AddPreferenceService
    {
        private string conn;
        public AddPreferenceService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceAddPreferenceModel> AddDataPreference(string lang, string token, List<Preference> data_choice, string v, string p)
        {
            ServiceAddPreferenceModel value = new ServiceAddPreferenceModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation2 = await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage };
                    return value;
                }
                else
                {
                    int preference_id = 0;
                    int preference_choice_id = 0;
                    string preference_choice_optinal_text = string.Empty;

                    Preference[] ans = data_choice.ToArray();
                    if (ans.Length > 0)
                    {
                        for (int i = 0; i < ans.Length; i++)
                        {
                            preference_id = ans[i].preference_id;
                            PreferenceChoice[] choice = ans[i].preference_choice.ToArray();
                            //preference_choice_optinal_text = ans[i].preference_choice_optinal_text;

                            //string[] preference_choice_list = Regex.Split(ans[i].preference_choice, @"\,");

                            for (int ii = 0; ii < choice.Length; ii++)
                            {
                                preference_choice_id = choice[ii].choice_id;
                                preference_choice_optinal_text = choice[ii].choice_optinal_text;

                                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                                {
                                    //DECLARE @OTP NVARCHAR(10) = RIGHT(CONVERT(DECIMAL(8, 6), RAND()), 6)
                                    string cmd = @"
DECLARE @TOKEN NVARCHAR(100) = N'{0}'
DECLARE @CUS_ID INT = 0

SET  @CUS_ID = (SELECT c.ID FROM T_CUSTOMER c LEFT JOIN T_CUSTOMER_TOKEN ct ON c.MEMBERID = ct.MEMBERID WHERE ct.TOKEN_NO = @TOKEN)

INSERT preference_user ([customer_id], [preference_id], [preference_choice_id], preference_choice_optinal_text, [create_date], [create_by])
VALUES (COALESCE(@CUS_ID,0), {1}, {2}, N'{3}', DATEADD(HOUR, 7, GETDATE()), @CUS_ID)

UPDATE T_CUSTOMER SET is_preference = 1 WHERE ID = @CUS_ID

SELECT * FROM preference_user WHERE customer_id = @CUS_ID";

                                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, token, preference_id, preference_choice_id, preference_choice_optinal_text)))
                                    {
                                        if (dt.Rows.Count > 0)
                                        {
                                            value.success = true;
                                            value.msg = new MsgModel() { code = 0, text = "Success" };
                                        }
                                        else
                                        {
                                            value.success = false;
                                            value.msg = new MsgModel() { code = 0, text = "Fail" };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        value.success = true;
                        value.msg = new MsgModel() { code = 0, text = "Success" };
                    }
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