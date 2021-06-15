using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class CarPurchasePlanService
    {
        private string conn;

        public CarPurchasePlanService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceCarPurchasePlanModel> GetAllDataCarPurchasePlan(string lang, string v, string p)
        {
            ServiceCarPurchasePlanModel value = new ServiceCarPurchasePlanModel();
            try
            {
                value.ts = DateTime.Now;

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
                    value.data = new ServiceCarPurchasePlanData();
                    value.data.car_purchase_plan_datas =await GetAllCarPurchasePlanData(lang);
                    value.success = true;
                    value.msg = new MsgModel()
                    {
                        code = 200,
                        text = "Success",
                        store_link = validation2.InvalidStoreLink,
                        version = validation2.InvalidVersion
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        public async Task<List<_ServiceCarPurchasePlanData>> GetAllCarPurchasePlanData(string lang)
        {
            List<_ServiceCarPurchasePlanData> list = new List<_ServiceCarPurchasePlanData>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"          
                           DECLARE @lang varchar(2) = N'{0}';
                           SELECT id, 
                           	  CASE
                           	  	 WHEN @lang = 'TH' THEN name_th 
                           	  	 WHEN @lang = 'EN' THEN name_en
                           	  END AS name
                           FROM purchase_plan
                           WHERE is_active = 1 AND deleted_flag IS NULL
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang)))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _ServiceCarPurchasePlanData data = new _ServiceCarPurchasePlanData();
                            data.id = Convert.ToInt16(dr["id"]);
                            data.name = dr["name"].ToString();
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