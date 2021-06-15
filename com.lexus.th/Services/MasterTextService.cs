using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class MasterTextService
    {
        private string conn;

        public MasterTextService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceAllMasterTextModel> GetAllMasterData(string v, string p, string lang, string platform, string group_data)
        {
            ServiceAllMasterTextModel value = new ServiceAllMasterTextModel();
            try
            {
                value.ts = UtilityService.GetDateTimeFormat(DateTime.Now);

                SystemController syc = new SystemController();
                ValidationModel validation =await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    value.data = new _ServiceAllMasterTextModel();
                    value.data.master_text =await GetText(lang, platform, group_data);

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

        private async Task<List<MasterText>> GetText(string lang, string platform, string group_data)
        {
            List<MasterText> list = new List<MasterText>();
            
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT *
FROM system_master_valuelist
where lang = N'{0}'
and platform = N'{1}'
and group_data = N'{2}'
ORDER BY display_order";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, platform, group_data)))
                    {
                        MasterText data;
                        foreach (DataRow row in dt.Rows)
                        {
                            data = new MasterText();
                            data.loadMasterText(row);
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