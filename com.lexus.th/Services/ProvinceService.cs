using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class ProvinceService
    {
        private string conn;
        public ProvinceService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceProvinceModel> GetDataProvince(string lang, string v, string p, string criteria)
        {
            ServiceProvinceModel value = new ServiceProvinceModel();
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
                    value.data = new _ServiceProvinceData();
                    value.data.province = GetProvince(lang, criteria);

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

        public List<ProvinceModel> GetProvince(string lang, string criteria)
        {
            List<ProvinceModel> list = new List<ProvinceModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    if (!string.IsNullOrEmpty(criteria))
                    {
                        cmd = @"
                                DECLARE @LANG NVARCHAR(5) = N'{0}'
                                SELECT distinct P.PROVINCE_ID, CASE  
                                WHEN @LANG = 'EN' THEN LOWER(P.PROVINCE_NAME_ENG) 
                                ELSE P.PROVINCE_NAME_TH END AS PROVINCE_NAME
                                FROM T_DEALER D
                                INNER JOIN T_PROVINCE P ON P.PROVINCE_ID = D.province_id
                                WHERE D.ACTIVE = 1 AND D.DEL_FLAG is null
                                ORDER BY PROVINCE_NAME 
                                ";
                    }
                    else
                    {
                        cmd = @"
                                DECLARE @LANG NVARCHAR(5) = N'{0}'
                                
                                SELECT PROVINCE_ID, CASE  
                                  WHEN @LANG = 'EN' THEN LOWER(PROVINCE_NAME_ENG) 
                                  ELSE PROVINCE_NAME_TH END AS PROVINCE_NAME
                                FROM T_PROVINCE
                                ORDER BY PROVINCE_NAME
                                ";
                    }


                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ProvinceModel province = new ProvinceModel();
                            province.id = Convert.ToInt32(row["PROVINCE_ID"]);
                            province.name = UppercaseWords(row["PROVINCE_NAME"].ToString());
                            var a = UppercaseWords("AMNAT CHAROEN");

                            list.Add(province);
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

        public List<ProvinceModel> GetProvinceByRegion(string lang, int region_id)
        {
            List<ProvinceModel> list = new List<ProvinceModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                                DECLARE @LANG NVARCHAR(5) = N'{0}'
                                DECLARE @GEO_ID int = N'{1}'

                                SELECT PROVINCE_ID, CASE  
                                WHEN @LANG = 'EN' THEN LOWER(PROVINCE_NAME_ENG) 
                                ELSE PROVINCE_NAME_TH END AS PROVINCE_NAME
                                FROM T_PROVINCE
                                WHERE GEO_ID = @GEO_ID
                                ORDER BY PROVINCE_NAME 
                                ";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, region_id)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ProvinceModel province = new ProvinceModel();
                            province.id = Convert.ToInt32(row["PROVINCE_ID"]);
                            province.name = UppercaseWords(row["PROVINCE_NAME"].ToString());
                            list.Add(province);
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

        public string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
    }
}