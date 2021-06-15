using AppLibrary.Database;
using MultipartDataMediaFormatter.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Threading.Tasks;

namespace com.lexus.th
{
    public class ProfileService
    {
        private string conn;
        private string custImgPath;
        public ProfileService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
            this.custImgPath = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusCustImgPath"];
        }
        public async Task<ServiceProfileGetModel> GetProfileData(string token, string v, string p)
        {
            ServiceProfileGetModel value = new ServiceProfileGetModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    CustomerModel cust = GetCustomer(token);
                    value.data = new _ServiceProfileGetData
                    {
                        id = cust.id,
                        fname = cust.fname,
                        lname = cust.lname,
                        disp_name = cust.disp_name,
                        privilege_cnt = cust.privilege_cnt,
                        email = cust.email,
                        tel_no = cust.tel_no,
                        expiry = cust.expiry,
                        expiry_ts = cust.expiry_ts,
                        profile_image = cust.profile_image,
                        cars = GetCars(token),
                        member_id = cust.member_id,
                        title = cust.title,
                        birthdate = cust.birthdate,
                    };

                    
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

        public async Task<ServiceProfileNewGetModel> GetProfileDataNew(string token, string v, string p, string lang)
        {
            ServiceProfileNewGetModel value = new ServiceProfileNewGetModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

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
                    value.data = new _ServiceProfileNewGetData();
                    CustomerModel cust = GetCustomerNewAppUser(token);
                    value.data.app_user = new ProfileGetData
                    {
                        id = cust.id,
                        fname = cust.fname,
                        lname = cust.lname,
                        disp_name = cust.disp_name,
                        privilege_cnt = cust.privilege_cnt,
                        email = cust.email,
                        tel_no = cust.tel_no,
                        expiry = cust.expiry,
                        expiry_ts = cust.expiry_ts,
                        profile_image = cust.profile_image,
                        cars = GetCarsNew(token, lang),
                        member_id = cust.member_id,
                        title = cust.title,
                        birthdate = cust.birthdate,
                        plate_no = cust.plate_no,
                        citizen_id = cust.citizen_id,
                        vehicle_no = cust.vehicle_no
                    };

                    CustomerModel cust2 = GetCustomerNewCarOwner(token);
                    value.data.car_owner = new ProfileGetData
                    {
                        id = cust2.id,
                        fname = cust2.fname,
                        lname = cust2.lname,
                        disp_name = cust2.disp_name,
                        privilege_cnt = cust2.privilege_cnt,
                        email = cust2.email,
                        tel_no = cust2.tel_no,
                        expiry = cust2.expiry,
                        expiry_ts = cust2.expiry_ts,
                        profile_image = cust2.profile_image,
                        cars = GetCarsNew(token,lang),
                        member_id = cust2.member_id,
                        title = cust2.title,
                        birthdate = cust2.birthdate,
                        plate_no = cust2.plate_no,
                        citizen_id = cust.citizen_id,
                        vehicle_no = cust.vehicle_no
                    };

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

        private CustomerModel GetCustomer(string token)
        {
            CustomerModel value = new CustomerModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
SELECT A.ID
      ,A.FNAME
      ,A.LNAME
	  ,COALESCE(A.PRIVILEGE_CNT,0) PRIVILEGE_CNT
      ,A.EMAIL
	  ,A.MOBILE   
      ,A.EXPIRY    
      ,A.NICKNAME
      ,A.PROFILE_IMAGE
	  ,A.MEMBERID
	  ,A.TITLENAME
	  ,A.BIRTHDATE
      ,A.plate_no
      ,A.citizen_id
      ,A.vehicle_no
FROM T_CUSTOMER A
INNER JOIN T_CUSTOMER_TOKEN T ON A.MEMBERID = T.MEMBERID
WHERE T.TOKEN_NO = N'{0}'", token);

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            value.id = Convert.ToInt32(row["ID"]);
                            value.fname = row["FNAME"].ToString();
                            value.lname = row["LNAME"].ToString();
                            value.privilege_cnt = Convert.ToInt32(row["PRIVILEGE_CNT"]);
                            value.email = row["EMAIL"].ToString();
                            value.tel_no = row["MOBILE"].ToString();
                            value.expiry = (row["EXPIRY"] == DBNull.Value) ? "" : Convert.ToDateTime(row["EXPIRY"]).ToString("dd/MM/yyyy");
                            value.expiry_ts = (row["EXPIRY"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY"]));
                            value.disp_name = row["NICKNAME"].ToString();
                            value.profile_image = row["PROFILE_IMAGE"].ToString();
                            value.member_id = row["MEMBERID"].ToString();
                            value.title = row["TITLENAME"].ToString();
                            //value.birthdate = (row["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["BIRTHDATE"]).ToString("dd/MM/yyyy");
                            value.birthdate = (row["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["BIRTHDATE"]).ToString("yyyy-MM-dd");
                            value.plate_no = row["plate_no"].ToString();
                            value.citizen_id = row["citizen_id"].ToString();
                            value.vehicle_no = row["vehicle_no"].ToString();
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

        private CustomerModel GetCustomerNewCarOwner(string token)
        {
            CustomerModel value = new CustomerModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
SELECT A.ID
      ,A.FNAME
      ,A.LNAME
	  ,A.PRIVILEGE_CNT
      ,A.EMAIL
	  ,A.MOBILE   
      ,A.EXPIRY    
      ,A.NICKNAME
      ,A.PROFILE_IMAGE
	  ,A.MEMBERID
	  ,A.TITLENAME
	  ,A.BIRTHDATE
      ,A.plate_no
      ,A.citizen_id
      ,A.vehicle_no
FROM T_CUSTOMER_CAR_OWNER A
INNER JOIN T_CUSTOMER_TOKEN T ON A.MEMBERID = T.MEMBERID
WHERE T.TOKEN_NO = N'{0}' AND A.DEL_FLAG IS NULL", token);

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            value.id = Convert.ToInt32(row["ID"]);
                            value.fname = row["FNAME"].ToString();
                            value.lname = row["LNAME"].ToString();
                            value.privilege_cnt = Convert.ToInt32(row["PRIVILEGE_CNT"]);
                            value.email = row["EMAIL"].ToString();
                            value.tel_no = row["MOBILE"].ToString();
                            value.expiry = (row["EXPIRY"] == DBNull.Value) ? "" : Convert.ToDateTime(row["EXPIRY"]).ToString("dd/MM/yyyy");
                            value.expiry_ts = (row["EXPIRY"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY"]));
                            value.disp_name = row["NICKNAME"].ToString();
                            value.profile_image = row["PROFILE_IMAGE"].ToString();
                            value.member_id = row["MEMBERID"].ToString();
                            value.title = row["TITLENAME"].ToString();
                            //value.birthdate = (row["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["BIRTHDATE"]).ToString("dd/MM/yyyy");
                            value.birthdate = (row["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["BIRTHDATE"]).ToString("yyyy-MM-dd");
                            value.plate_no = row["plate_no"].ToString();
                            value.citizen_id = row["citizen_id"].ToString();
                            value.vehicle_no = row["vehicle_no"].ToString();
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

        private CustomerModel GetCustomerNewAppUser(string token)
        {
            CustomerModel value = new CustomerModel();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
SELECT A.ID
      ,A.FNAME
      ,A.LNAME
	  ,A.PRIVILEGE_CNT
      ,A.EMAIL
	  ,A.MOBILE   
      ,A.EXPIRY    
      ,A.NICKNAME
      ,A.PROFILE_IMAGE
	  ,A.MEMBERID
	  ,A.TITLENAME
	  ,A.BIRTHDATE
      ,A.plate_no
      ,A.citizen_id
      ,A.vehicle_no
FROM T_CUSTOMER A
INNER JOIN T_CUSTOMER_TOKEN T ON A.MEMBERID = T.MEMBERID
WHERE T.TOKEN_NO = N'{0}' AND A.DEL_FLAG IS NULL", token);

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            value.id = Convert.ToInt32(row["ID"]);
                            value.fname = row["FNAME"].ToString();
                            value.lname = row["LNAME"].ToString();
                            value.privilege_cnt = Convert.ToInt32(row["PRIVILEGE_CNT"]);
                            value.email = row["EMAIL"].ToString();
                            value.tel_no = row["MOBILE"].ToString();
                            value.expiry = (row["EXPIRY"] == DBNull.Value) ? "" : Convert.ToDateTime(row["EXPIRY"]).ToString("dd/MM/yyyy");
                            value.expiry_ts = (row["EXPIRY"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY"]));
                            value.disp_name = row["NICKNAME"].ToString();
                            value.profile_image = row["PROFILE_IMAGE"].ToString();
                            value.member_id = row["MEMBERID"].ToString();
                            value.title = row["TITLENAME"].ToString();
                            //value.birthdate = (row["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["BIRTHDATE"]).ToString("dd/MM/yyyy");
                            value.birthdate = (row["BIRTHDATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["BIRTHDATE"]).ToString("yyyy-MM-dd");
                            value.plate_no = row["plate_no"].ToString();
                            value.citizen_id = row["citizen_id"].ToString();
                            value.vehicle_no = row["vehicle_no"].ToString();
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

        private List<CarModel> GetCars(string token)
        {
            List<CarModel> list = new List<CarModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
SELECT CM.MODEL	
	  ,CAR.VIN
	  ,CAR.PLATE_NO
	  ,CM.IMAGE
      ,CAR.DEALER
      ,CAR.RS_Date
FROM T_CUSTOMER_CAR CAR
INNER JOIN T_CAR_MODEL CM ON CM.MODEL_ID = CAR.MODEL_ID
INNER JOIN T_CUSTOMER CUS ON CUS.MEMBERID = CAR.MEMBERID
INNER JOIN T_CUSTOMER_TOKEN TKN ON TKN.MEMBERID = CUS.MEMBERID
WHERE TKN.TOKEN_NO = N'{0}' AND CAR.DEL_FLAG IS NULL", token);

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(new CarModel {
                                model = row["MODEL"].ToString(),
                                vin = row["VIN"].ToString(),
                                plate_no = row["PLATE_NO"].ToString(),
                                image = row["IMAGE"].ToString(),
                                dealer = row["DEALER"].ToString(),
                                rs_date = (row["RS_Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["RS_Date"]).ToString("dd/MM/yyyy")
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

        private List<CarModel> GetCarsNew(string token, string lang)
        {
            List<CarModel> list = new List<CarModel>();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = string.Format(@"
DECLARE @LANG NVARCHAR(5) = N'{1}'

SELECT CM.MODEL	
	  ,CAR.VIN
	  ,CAR.PLATE_NO
	  ,CM.IMAGE
      ,CASE  
  WHEN @LANG = 'EN' THEN dm.display_en 
  ELSE dm.display_th END AS DEALER
      ,CAR.RS_Date
FROM T_CUSTOMER_CAR CAR
INNER JOIN T_CAR_MODEL CM ON CM.MODEL_ID = CAR.MODEL_ID
INNER JOIN T_CUSTOMER CUS ON CUS.MEMBERID = CAR.MEMBERID
INNER JOIN T_CUSTOMER_TOKEN TKN ON TKN.MEMBERID = CUS.MEMBERID
INNER JOIN T_DEALER_MASTER dm ON CAR.DEALER = dm.id
WHERE TKN.TOKEN_NO = N'{0}' AND CAR.DEL_FLAG IS NULL ", token, lang);

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            list.Add(new CarModel
                            {
                                model = row["MODEL"].ToString(),
                                vin = row["VIN"].ToString(),
                                plate_no = row["PLATE_NO"].ToString(),
                                image = row["IMAGE"].ToString(),
                                dealer = row["DEALER"].ToString(),
                                rs_date = (row["RS_Date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["RS_Date"]).ToString("dd/MM/yyyy")
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

        public async Task<ServiceProfileUpdateModel> UpdateProfileData(string token, string image, string name, string v, string p)
        {
            ServiceProfileUpdateModel value = new ServiceProfileUpdateModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystem(p, v);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    //string imagePath =await SaveCustImage(token, image);
                    string imagePath = "";
                    name = (string.IsNullOrEmpty(name)) ? "" : name;

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = string.Format(@"
UPDATE T_CUSTOMER
SET PROFILE_IMAGE = CASE WHEN LEN(N'{1}') = 0 THEN PROFILE_IMAGE ELSE N'{1}' END
, NICKNAME = CASE WHEN LEN(N'{2}') = 0 THEN NICKNAME ELSE N'{2}' END
WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')", token, imagePath, name);

                        db.ExecuteNonQueryFromCommandText(cmd);
                    }

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

        public async Task<ServiceProfileUpdateModel> UpdateProfileDataNew(string token, string image, string name, string v, string p, string lang, byte[] buffer)
        {
            ServiceProfileUpdateModel value = new ServiceProfileUpdateModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation = await syc.CheckSystemNew(p, v, lang);
                if (!validation.Success)
                {
                    value.success = validation.Success;
                    value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                    return value;
                }
                else
                {
                    string imagePath = await SaveCustImage(token, image, buffer);
                    name = (string.IsNullOrEmpty(name)) ? "" : name;

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = string.Format(@"
UPDATE T_CUSTOMER
SET PROFILE_IMAGE = CASE WHEN LEN(N'{1}') = 0 THEN PROFILE_IMAGE ELSE N'{1}' END
, NICKNAME = CASE WHEN LEN(N'{2}') = 0 THEN NICKNAME ELSE N'{2}' END
WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}')", token, imagePath, name);

                        db.ExecuteNonQueryFromCommandText(cmd);
                    }

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

        private async Task<string> SaveCustImage(string token, string image, byte[] buffer)
        {
            string file = "";
            try
            {
                if (image != null && !string.IsNullOrEmpty(image))
                {
                    string memberId;
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = string.Format(@"SELECT MEMBERID FROM T_CUSTOMER_TOKEN WHERE TOKEN_NO = N'{0}'", token);
                        memberId = db.ExecuteScalarFromCommandText<string>(cmd);
                    }
                    if (!string.IsNullOrEmpty(memberId))
                    {
                        //string directory = HttpContext.Current.Server.MapPath("/" + custImgPath + "/");
                        string directory = System.Web.Configuration.WebConfigurationManager.AppSettings["ProfileImagePath"];
                        
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        //file = directory + Path.GetFileNameWithoutExtension(image.FileName) + ".jpg";
                        LogManager.ServiceLog.WriteCustomLog("CustomerImagePath", "directoty : " + directory);

                        file = directory + memberId + ".jpg";
                        // open comment
                        File.WriteAllBytes(file, buffer);
                        
                        // return only folder and file name
                        //file = custImgPath + "/" + Path.GetFileName(file);
                        file = new DirectoryInfo(Path.GetDirectoryName(file)).Name + "/" + Path.GetFileName(file) + "?ts=" + DateTime.Now.ToString("HHmmssffffff");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return file;
        }

    }
}