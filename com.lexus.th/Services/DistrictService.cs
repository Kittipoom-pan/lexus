using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class DistrictService
    {
        private string conn;
        public DistrictService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceDistrictModel> GetDataDistrict(string lang, int province_id, string v, string p)
        {
            ServiceDistrictModel value = new ServiceDistrictModel();
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
                    value.data = new _ServiceDistrictData();
                    value.data.district =await GetDistrict(lang, province_id);

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

        private async Task<List<DistrictModel>> GetDistrict(string lang, int province_id)
        {
            List<DistrictModel> list = new List<DistrictModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{0}'

SELECT DISTRICT_ID, CASE  
  WHEN @LANG = 'EN' THEN DISTRICT_NAME_ENG 
  ELSE DISTRICT_NAME_TH END AS DISTRICT_NAME
FROM T_DISTRICT WHERE PROVINCE_ID = {1}
ORDER BY DISTRICT_NAME
";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, province_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            DistrictModel district = new DistrictModel();
                            district.id = Convert.ToInt32(row["DISTRICT_ID"]);
                            district.name = row["DISTRICT_NAME"].ToString();

                            list.Add(district);
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