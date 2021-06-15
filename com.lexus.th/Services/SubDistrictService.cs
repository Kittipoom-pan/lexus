using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class SubDistrictService
    {
        private string conn;
        public SubDistrictService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceSubDistrictModel> GetDataSubDistrict(string lang, int province_id, int district_id, string v, string p)
        {
            ServiceSubDistrictModel value = new ServiceSubDistrictModel();
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
                    value.data = new _ServiceSubDistrictData();
                    value.data.sub_district =await GetSubDistrict(lang, province_id, district_id);

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

        private async Task<List<SubDistrictModel>> GetSubDistrict(string lang, int province_id, int district_id)
        {
            List<SubDistrictModel> list = new List<SubDistrictModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{0}'

SELECT SUBDISTRICT_ID, CASE  
  WHEN @LANG = 'EN' THEN SUBDISTRICT_NAME_ENG 
  ELSE SUBDISTRICT_NAME_TH END AS SUBDISTRICT_NAME,
  POSTCODE
FROM T_SUBDISTRICT 
WHERE PROVINCE_ID = {1} AND DISTRICT_ID = {2}
ORDER BY SUBDISTRICT_NAME
";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, province_id, district_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            SubDistrictModel sub_district = new SubDistrictModel();
                            sub_district.id = Convert.ToInt32(row["SUBDISTRICT_ID"]);
                            sub_district.name = row["SUBDISTRICT_NAME"].ToString();
                            sub_district.postcode = row["POSTCODE"].ToString();

                            list.Add(sub_district);
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