using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class HistoryService
    {
        private string conn;
        public HistoryService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ServiceHistoryModel> GetHistory(string token, string v, string p)
        {
            ServiceHistoryModel value = new ServiceHistoryModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

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
                    value.data = new _ServiceHistoryData();
                    value.data.items = new List<HistoryModel>();

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = string.Format(@"
SELECT CR.CUS_ID
 ,P.TITLE
 ,P.[DESC]
 ,CR.REDEEM_DT
 ,CR.EXPIRY_DT
 ,CASE WHEN GETDATE() > CR.EXPIRY_DT THEN 1 ELSE 0 END AS IS_EXPIRE
 ,CASE WHEN PC.STATUS = 'N' THEN 1 ELSE 0 END AS IS_VERIFIED
 ,P.[IMAGE]
 ,CR.REDEEM_CODE
 ,CR.[ID] REDEEM_ID
FROM T_CUSTOMER_REDEEM CR
INNER JOIN T_CUSTOMER_TOKEN T ON CR.MEMBERID = T.MEMBERID 
INNER JOIN T_PRIVILEDGES P ON CR.PRIVILEGE_ID = P.ID
INNER JOIN T_PRIVILEDGES_CODE PC ON CR.PRIVILEGE_ID = PC.PRIVILEGE_ID AND CR.[NO] = PC.[NO]
WHERE T.TOKEN_NO = N'{0}'
ORDER BY CR.REDEEM_DT DESC", token);

                        using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                value.data.items.Add(new HistoryModel
                                {
                                    id = Convert.ToInt32(row["REDEEM_ID"]),
                                    title = row["TITLE"].ToString(),
                                    //desc = row["DESC"].ToString(),
                                    is_expire = Convert.ToBoolean(row["IS_EXPIRE"]),
                                    image = row["IMAGE"].ToString(),
                                    expiry_ts = (row["EXPIRY_DT"] == DBNull.Value) ? "" : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY_DT"])),
                                    redeem_code = row["REDEEM_CODE"].ToString(),
                                    is_verified = Convert.ToBoolean(row["IS_VERIFIED"]),
                                    redeem_date = (row["REDEEM_DT"] == DBNull.Value) ? "" : Convert.ToDateTime(row["REDEEM_DT"]).ToString("dd/MM/yyyy")
                                });
                            }
                        }
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

        public async Task<ServiceHistoryModel> GetHistoryNew(string token, string v, string p, string lang)
        {
            ServiceHistoryModel value = new ServiceHistoryModel();
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
                    value.data = new _ServiceHistoryData();
                    value.data.items = new List<HistoryModel>();

                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = string.Format(@"
SELECT CR.CUS_ID
 ,P.TITLE
 ,P.[DESC]
 ,CR.REDEEM_DT
 ,CR.VERIFY_DT
 ,CR.EXPIRY_DT
 ,CASE WHEN DATEADD(HOUR, 7, GETDATE()) > CR.EXPIRY_DT and CR.REDEEM_CODE is null THEN 1 ELSE 0 END AS IS_EXPIRE
 ,CASE WHEN PC.STATUS = 'N' THEN 1 ELSE 0 END AS IS_VERIFIED
 ,P.[IMAGE_1]
 ,CR.REDEEM_CODE
 ,CR.[ID] REDEEM_ID
 ,P.redeem_display_type
 ,P.redeem_display_height
 ,P.redeem_display_width
 ,P.redeem_display_image
 ,P.redeem_display_html
FROM T_CUSTOMER_REDEEM CR
INNER JOIN T_CUSTOMER_TOKEN T ON CR.MEMBERID = T.MEMBERID 
INNER JOIN T_PRIVILEDGES P ON CR.PRIVILEGE_ID = P.ID
left JOIN T_PRIVILEDGES_CODE PC ON CR.PRIVILEGE_ID = PC.PRIVILEGE_ID AND CR.[NO] = PC.[NO]
WHERE T.TOKEN_NO = N'{0}' --AND PC.STATUS = 'N'
and not (CR.REDEEM_CODE is null and DATEADD(HOUR, 7, GETDATE()) <= CR.EXPIRY_DT)
ORDER BY CR.REDEEM_DT DESC", token);

                        using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                int iRedeemDisplayType = (int)row["redeem_display_type"];
                                string RedeemDisplayType = "";
                                switch (iRedeemDisplayType)
                                {
                                    case 1:
                                        RedeemDisplayType = "text";
                                        break;
                                    case 2:
                                        RedeemDisplayType = "qr_code";
                                        break;
                                    case 3:
                                        RedeemDisplayType = "barcode";
                                        break;
                                    case 4:
                                        RedeemDisplayType = "html";
                                        break;
                                    default:
                                        RedeemDisplayType = "text";
                                        break;
                                }
                                string RedeemCode, RedeemDisplayImage = "", redeem_html, URLBARCODE, URLQRCODE, RedeemDisplayHTML = "";
                                int RedeemDisplayHeight = 0, RedeemDisplayWidth = 0;
                                if ((int)row["redeem_display_type"] == 4) //html
                                {
                                    RedeemCode = row["REDEEM_CODE"].ToString();
                                    RedeemDisplayHeight = (int)row["redeem_display_height"];
                                    RedeemDisplayWidth = (int)row["redeem_display_width"];
                                    RedeemDisplayImage = row["redeem_display_image"].ToString();
                                    redeem_html = row["redeem_display_html"].ToString();
                                    URLBARCODE = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusURL"]
                                        + "/Page/GenerateBarcodeImage.aspx?d=" + RedeemCode
                                        + "&h=200&w=600&bc=ffffff&fc=000000&t=Code 128&il=false&if=jpeg&align=c";
                                    URLQRCODE = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusURL"]
                                        + "/Page/GenerateQRCodeImage.aspx?d=" + RedeemCode;
                                    string URLREDEEMBACKGROUND = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusURL"]
                                        + "/images/" + RedeemDisplayImage;
                                    RedeemDisplayHTML = redeem_html
                                        .Replace("URLBARCODE", URLBARCODE)
                                        .Replace("URLQRCODE", URLQRCODE)
                                        .Replace("REDEEMEDCODE", RedeemCode)
                                        .Replace("URLREDEEMBACKGROUND", URLREDEEMBACKGROUND);
                                }
                                
                                value.data.items.Add(new HistoryModel
                                {
                                    id = Convert.ToInt32(row["REDEEM_ID"]),
                                    title = row["TITLE"].ToString(),
                                    //desc = row["DESC"].ToString(),
                                    is_expire = Convert.ToBoolean(row["IS_EXPIRE"]),
                                    image = row["IMAGE_1"].ToString(),
                                    expiry_ts = (row["EXPIRY_DT"] == DBNull.Value || row["REDEEM_CODE"].ToString() != "") ?
                                        UtilityService.GetDateTimeFormat(DateTime.Now.AddYears(100)) : UtilityService.GetDateTimeFormat(Convert.ToDateTime(row["EXPIRY_DT"])),
                                    redeem_code = row["REDEEM_CODE"].ToString(),
                                    is_verified = Convert.ToBoolean(row["IS_VERIFIED"]),
                                    redeem_date = (row["VERIFY_DT"] == DBNull.Value) ?
                                        ((row["REDEEM_DT"] == DBNull.Value) ? "" : Convert.ToDateTime(row["REDEEM_DT"]).ToString("dd/MM/yyyy")) :
                                        Convert.ToDateTime(row["VERIFY_DT"]).ToString("dd/MM/yyyy"),
                                    redeem_datetime = (row["VERIFY_DT"] == DBNull.Value) ?
                                        ((row["REDEEM_DT"] == DBNull.Value) ? "" : Convert.ToDateTime(row["REDEEM_DT"]).ToString("dd/MM/yyyy HH:mm")) :
                                        Convert.ToDateTime(row["VERIFY_DT"]).ToString("dd/MM/yyyy HH:mm"),

                                    iredeem_display_type = (int)row["redeem_display_type"],
                                    redeem_display_type = RedeemDisplayType,
                                    redeem_display_height = RedeemDisplayHeight,
                                    redeem_display_width = RedeemDisplayWidth,
                                    redeem_display_image = RedeemDisplayImage,
                                    redeem_display_html = RedeemDisplayHTML
                                });
                            }
                        }
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
    }
}