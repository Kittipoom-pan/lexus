using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class AuthorizeService
    {
        private string conn;


        public AuthorizeService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public async Task<ServiceAuthorizeDealerModel> GetAllDataAuthorizeService(string lang, string v, string p)
        {
            ServiceAuthorizeDealerModel value = new ServiceAuthorizeDealerModel();
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
                    value.data = new ServiceAuthorizeDealerData();
                    value.data.dealer_groups =await GetAllAuthorizeServiceGroup(lang);

                   

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


        public async Task<List<AuthorizeServiceModel>> GetAllAuthorizeServiceGroup(string lang)
        {
            List<AuthorizeServiceModel> value = new List<AuthorizeServiceModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"                
                                select  type from T_DEALER WHERE ACTIVE = 1 AND DEL_FLAG IS NULL GROUP BY type
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd)))
                    {
                        int dummy_id = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            dummy_id++;
                            //GeneralModel data = new GeneralModel();
                            //data.id = Convert.ToInt32(row["id"]);
                            //data.name = row["name"].ToString();
                            //data.detail = GetAllTypeOfServiceDetail(data.id, lang);

                            //list.Add(data);
                            value.Add(new AuthorizeServiceModel()
                            {
                                id = dummy_id,
                                name = row["type"].ToString(),
                                geos = GetAllGeo(lang, row["type"].ToString())
                            });
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

        public List<GeoDealerModel> GetAllGeo(string lang, string dealer_type)
        {
            List<GeoDealerModel> value = new List<GeoDealerModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                                    
                                DECLARE @lang varchar(2) = N'{0}';

                                IF @lang = 'TH'
	                                BEGIN
		                                SELECT dealer.GEO_ID AS id, geo.geo_name_th  AS name
                                        FROM t_dealer as dealer
                                        INNER JOIN T_GEO as geo on geo.GEO_ID = dealer.GEO_ID
                                        WHERE dealer.type = N'{1}' GROUP BY dealer.GEO_ID, geo.geo_name_th
                                        ORDER BY dealer.GEO_ID, geo.geo_name_th 
	                                END
                                IF @lang = 'EN' 
	                                BEGIN
		                                SELECT dealer.GEO_ID AS id, geo.GEO_NAME_EN  AS name
                                        FROM t_dealer as dealer
                                        INNER JOIN T_GEO as geo on geo.GEO_ID = dealer.GEO_ID
                                        WHERE dealer.type = N'{1}' GROUP BY dealer.GEO_ID, geo.GEO_NAME_EN
                                        ORDER BY dealer.GEO_ID, geo.GEO_NAME_EN 
	                                END
                                ";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, lang, dealer_type)))
                    {
                        foreach (DataRow row in dt.Rows)
                        { 
                            if (CheckHasDealerInGeo(Convert.ToInt32(row["id"]), lang, dealer_type))
                            {
                                value.Add(new GeoDealerModel()
                                {
                                    id = Convert.ToInt32(row["id"]),
                                    name = row["name"].ToString(),
                                    dealers = GetAllDealer(Convert.ToInt32(row["id"]), lang, dealer_type)
                                });
                            }   
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


        private bool CheckHasDealerInGeo(int geo_id, string lang, string type)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @LANG NVARCHAR(5) = N'{1}'
                           
                           SELECT 
                           d.DEALER_ID,
                           d.BRANCH_CODE,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_NAME 
                             ELSE d.dealer_name_th END AS DEALER_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.BRANCH_NAME 
                             ELSE d.branch_name_th END AS BRANCH_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_ADDRESS 
                             ELSE d.dealer_address_th END AS DEALER_ADDRESS,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_OFFICE_HOURS 
                             ELSE d.DEALER_OFFICE_HOURS_TH END AS DEALER_OFFICE_HOURS,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_OFFICE_HOURS2 
                             ELSE d.DEALER_OFFICE_HOURS2_TH END AS DEALER_OFFICE_HOURS2,
                           d.DEALER_MOBILE,
                           latitude,
                           longitude
                           FROM T_DEALER d
                           WHERE d.ACTIVE = 1 AND d.GEO_ID = {0} AND d.DEL_FLAG IS NULL AND d.type = N'{2}'";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, geo_id, lang, type)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }


        private List<DealerModel> GetAllDealer(int geo_id, string lang, string type)
        {
            List<DealerModel> list = new List<DealerModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
                           DECLARE @LANG NVARCHAR(5) = N'{1}'
                           
                           SELECT 
                           d.DEALER_ID,
                           d.BRANCH_CODE,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_NAME 
                             ELSE d.dealer_name_th END AS DEALER_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.BRANCH_NAME 
                             ELSE d.branch_name_th END AS BRANCH_NAME,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_ADDRESS 
                             ELSE d.dealer_address_th END AS DEALER_ADDRESS,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_OFFICE_HOURS 
                             ELSE d.DEALER_OFFICE_HOURS_TH END AS DEALER_OFFICE_HOURS,
                           CASE  
                             WHEN @LANG = 'EN' THEN d.DEALER_OFFICE_HOURS2 
                             ELSE d.DEALER_OFFICE_HOURS2_TH END AS DEALER_OFFICE_HOURS2,
                           d.DEALER_MOBILE,
                           latitude,
                           longitude
                           FROM T_DEALER d
                           WHERE d.ACTIVE = 1 AND d.GEO_ID = {0} AND d.DEL_FLAG IS NULL AND d.type = N'{2}'
                           ORDER BY DEALER_ID";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, geo_id, lang, type)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            DealerModel dealer = new DealerModel();
                            dealer.id = Convert.ToInt32(row["DEALER_ID"]);
                            dealer.name = row["DEALER_NAME"].ToString();
                            dealer.branch_name = row["BRANCH_NAME"].ToString();
                            dealer.address = row["DEALER_ADDRESS"].ToString();
                            dealer.mobile = row["DEALER_MOBILE"].ToString();
                            dealer.office_hours = row["DEALER_OFFICE_HOURS"].ToString();
                            dealer.office_hours2 = row["DEALER_OFFICE_HOURS2"].ToString();
                            dealer.latitude = float.Parse(row["latitude"].ToString());
                            dealer.longitude = float.Parse(row["longitude"].ToString());

                            list.Add(dealer);
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