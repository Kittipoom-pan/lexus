using AppLibrary.Database;
using com.lexus.th;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class TypeOfServiceService
    {
        private string conn;

        public TypeOfServiceService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceTypeOfServiceModel> GetAllDataTypeOfService(string lang, string v, string p)
        {
            ServiceTypeOfServiceModel value = new ServiceTypeOfServiceModel();
            try
            {
                value.ts = DateTime.Now;

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
                    value.data = new ServiceTypeOfServiceData();


                    value.data.type_of_service_data =await  GetAllTypeOfService(lang);

                    value.success = true;
                    value.msg = new MsgModel()
                    {
                        code = 0,
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

        private async Task<List<_ServiceTypeOfServiceData>> GetAllTypeOfService(string lang)
        {
            List<_ServiceTypeOfServiceData> list = new List<_ServiceTypeOfServiceData>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                                    
                                DECLARE @lang varchar(2) = N'{0}';

                                IF @lang = 'TH'
	                                BEGIN
		                                select id, name_th as name  from [dbo].[type_of_service] where is_active =1 and deleted_flag is null
	                                END
                                IF @lang = 'EN' 
	                                BEGIN
		                                select id, name_en as name  from [dbo].[type_of_service] where is_active =1 and deleted_flag is null
	                                END
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            //GeneralModel data = new GeneralModel();
                            //data.id = Convert.ToInt32(row["id"]);
                            //data.name = row["name"].ToString();
                            //data.detail = GetAllTypeOfServiceDetail(data.id, lang);

                            //list.Add(data);
                            list.Add(new _ServiceTypeOfServiceData()
                            {
                                id = Convert.ToInt32(row["id"]),
                                name = row["name"].ToString(),
                                type_of_service_detail = GetAllTypeOfServiceDetail(Convert.ToInt32(row["id"]), lang)
                            });
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


        public List<_ServiceTypeOfServiceDetailData> GetAllTypeOfServiceDetail(int type_of_service_id, string lang)
        {

           // _GeneralDetailData value = new _GeneralDetailData();
            
            List<_ServiceTypeOfServiceDetailData> value = new List<_ServiceTypeOfServiceDetailData>();
        
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"     
                           DECLARE @lang varchar(2) = N'{1}';
        
                           IF @lang = 'TH'
        	                   BEGIN
                                   SELECT id, name_th AS name FROM [dbo].[type_of_service_detail] 
                                   WHERE is_active = 1 AND deleted_flag IS NULL
                                   AND type_of_service_id = {0}
        	                   END
                           IF @lang = 'EN' 
        	                   BEGIN
        		                   SELECT id, name_en AS name FROM [dbo].[type_of_service_detail] 
                                   WHERE is_active = 1 AND deleted_flag IS NULL
                                   AND type_of_service_id = {0}
        	                   END
                                 ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, type_of_service_id, lang)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            _ServiceTypeOfServiceDetailData data = new _ServiceTypeOfServiceDetailData();
                            data.id = Convert.ToInt32(row["id"]);
                            data.name = row["name"].ToString();

                            value.Add(data);
                        }
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