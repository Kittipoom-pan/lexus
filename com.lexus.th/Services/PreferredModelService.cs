using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class PreferredModelService
    {
        private string conn;

        public PreferredModelService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }


        public async Task<ServicePreferredModel> GetAllDataPreferredModel(string lang, string v, string p)
        {
            ServicePreferredModel value = new ServicePreferredModel();
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
                    value.data = new ServicePreferredData();
                    value.data.preferred_model_datas =await GetAllPreferredData();
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

        public async Task<List<_ServicePreferredData>> GetAllPreferredData()
        {
            List<_ServicePreferredData> list = new List<_ServicePreferredData>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"         
                           SELECT MODEL_ID AS id, MODEL AS name FROM [dbo].[T_CAR_MODEL]
                           WHERE is_test_drive = 1 AND DEL_FLAG IS NULL
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _ServicePreferredData data = new _ServicePreferredData();
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