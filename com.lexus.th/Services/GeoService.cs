using AppLibrary.Database;
//using com.lexus.th.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class GeoService
    {
        private string conn;
        public GeoService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ServiceGeoModel> GetScreenData(string v, string p)
        {
            ServiceGeoModel value = new ServiceGeoModel();
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
                    value.data = new _ServiceGeoData();
                    value.data.geos = GetGeos("EN", "DEALER");

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

        public async Task<ServiceGeoModel> GetScreenDataNew(string v, string p, string lang, string type)
        {
            ServiceGeoModel value = new ServiceGeoModel();
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
                    value.data = new _ServiceGeoData();
                    value.data.geos = GetGeos(lang, type);

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

        private List<GeoModel> GetGeos(string lang, string type)
        {
            List<GeoModel> list = new List<GeoModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @LANG NVARCHAR(5) = N'{0}'

SELECT 
GEO_ID,
CASE  
  WHEN @LANG = 'EN' THEN GEO_NAME_EN 
  ELSE geo_name_th END AS GEO_NAME
FROM T_GEO
WHERE GEO_ID IN (SELECT GEO_ID FROM T_DEALER WHERE DEL_FLAG IS NULL AND ACTIVE = 1 AND type = N'{1}')
";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, type)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            GeoModel geo = new GeoModel();
                            geo.id = Convert.ToInt32(row["GEO_ID"]);
                            geo.name = row["GEO_NAME"].ToString();

                            list.Add(geo);
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