using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class ShopVerifyService
    {
        private string conn;
        public ShopVerifyService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public async Task<ServiceShopVerifyModel> VerifyItem(string token, string code, int redeem_id, string v, string p)
        {
            ServiceShopVerifyModel value = new ServiceShopVerifyModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation2 =await syc.CheckSystem(p, v);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                    return value;
                }
                else
                {
                    ValidationModel validation =await CheckValidation(code, redeem_id);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    if (!CheckCodeActive(token, code))
                    {
                        value.msg = new MsgModel() { code = 100, text = "No active code" };
                        return value;
                    }

                    int privilegeId =await CheckExistsPrivilegeVerify(code);
                    if (privilegeId == 0)
                    {
                        value.msg = new MsgModel() { code = 100, text = "Not exists privilege" };
                        return value;
                    }

                    UpdateShopVerify2(token, code, privilegeId, redeem_id);

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

        public async Task<ServiceShopVerifyModel> VerifyItemNew(string token, string code, int redeem_id, string v, string p, string lang)
        {
            ServiceShopVerifyModel value = new ServiceShopVerifyModel();
            try
            {
                DateTime ts = DateTime.Now;
                value.ts = UtilityService.GetDateTimeFormat(ts);

                SystemController syc = new SystemController();
                ValidationModel validation2 = await syc.CheckSystemNew(p, v, lang);
                if (!validation2.Success)
                {
                    value.success = validation2.Success;
                    value.msg = new MsgModel() { code = validation2.InvalidCode, text = validation2.InvalidMessage, store_link = validation2.InvalidStoreLink, version = validation2.InvalidVersion };
                    return value;
                }
                else
                {
                    ValidationModel validation = await CheckValidationNew(code, redeem_id, lang);
                    if (!validation.Success)
                    {
                        value.success = validation.Success;
                        value.msg = new MsgModel() { code = validation.InvalidCode, text = validation.InvalidMessage, store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                        return value;
                    }

                    if (!CheckCodeActive(token, code))
                    {
                        value.msg = new MsgModel() { code = 602, text = "No active code" };
                        return value;
                    }

                    int privilegeId = await CheckExistsPrivilegeVerify(code);
                    if (privilegeId == 0)
                    {
                        value.msg = new MsgModel() { code = 603, text = "Not exists privilege" };
                        return value;
                    }

                    await UpdateShopVerify2(token, code, privilegeId, redeem_id);

                    value.success = true;
                    value.msg = new MsgModel() { code = 200, text = "Success", store_link = validation.InvalidStoreLink, version = validation.InvalidVersion };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private bool CheckCodeActive(string token, string code)
        {
            bool isVerify = false;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT COUNT(1) CODE_ACTIVE
FROM T_CUSTOMER_REDEEM CR
    INNER JOIN T_CUSTOMER_TOKEN T ON CR.MEMBERID = T.MEMBERID
    INNER JOIN T_PRIVILEGE_VERIFY P ON CR.[PRIVILEGE_ID] = P.[PRIVILEGE_ID]
WHERE P.VERIFY_CODE = N'{0}' AND T.TOKEN_NO = N'{1}'
    AND DATEADD(HOUR, 7, GETDATE()) BETWEEN REDEEM_DT AND EXPIRY_DT";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, code, token)))
                    {
                        if (Convert.ToInt32(dt.Rows[0]["CODE_ACTIVE"]) > 0)
                        {
                            isVerify = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isVerify;
        }
        private async Task<int> CheckExistsPrivilegeVerify(string code)
        {
            int privilegeId = 0;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT PRIVILEGE_ID FROM T_PRIVILEGE_VERIFY WHERE VERIFY_CODE = N'{0}'";

                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, code)))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            privilegeId = Convert.ToInt32(dt.Rows[0][0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return privilegeId;
        }
        private void UpdateShopVerify(string token, string code, int privilegeId)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @VERIFY_CODE NVARCHAR(50) = N'{0}'
DECLARE @PRIVILEGE_ID INT = {1}
DECLARE @TOKEN NVARCHAR(100) = N'{2}'

UPDATE T_PRIVILEDGES_CODE SET [STATUS] = 'N' WHERE PRIVILEGE_ID = @PRIVILEGE_ID 
AND [NO] = (  
SELECT TOP 1 CR.[NO] 
FROM T_CUSTOMER_REDEEM CR 
INNER JOIN T_CUSTOMER_TOKEN T ON CR.MEMBERID = T.MEMBERID
INNER JOIN T_PRIVILEGE_VERIFY P ON CR.[PRIVILEGE_ID] = P.[PRIVILEGE_ID]
INNER JOIN T_PRIVILEDGES_CODE C ON C.[PRIVILEGE_ID] = CR.[PRIVILEGE_ID] AND C.[NO] = CR.[NO] AND C.[STATUS] = 'T'
WHERE P.VERIFY_CODE = @VERIFY_CODE AND T.TOKEN_NO = @TOKEN     
AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, GETDATE()), 120) BETWEEN CONVERT(NVARCHAR(10), REDEEM_DT, 120) AND CONVERT(NVARCHAR(10), EXPIRY_DT, 120)       
ORDER BY EXPIRY_DT)

UPDATE T_CUSTOMER_REDEEM 
SET SHOP_NM = (SELECT SHOP_NM FROM T_PRIVILEGE_VERIFY WHERE VERIFY_CODE = @VERIFY_CODE)";

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, code, privilegeId, token));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task UpdateShopVerify2(string token, string code, int privilegeId, int redeem_id)
        {
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @VERIFY_CODE NVARCHAR(50) = N'{0}'
DECLARE @PRIVILEGE_ID INT = {1}
DECLARE @TOKEN NVARCHAR(100) = N'{2}'
DECLARE @REDEEM_ID NVARCHAR(100) = {3}

UPDATE T_PRIVILEDGES_CODE SET [STATUS] = 'N' WHERE PRIVILEGE_ID = @PRIVILEGE_ID 
AND [NO] = (  
SELECT CR.[NO] FROM T_CUSTOMER_REDEEM CR 
INNER JOIN T_PRIVILEDGES_CODE PC ON CR.[PRIVILEGE_ID] = PC.[PRIVILEGE_ID]  AND CR.[NO] = PC.[NO]
WHERE id = @REDEEM_ID)

UPDATE T_CUSTOMER_REDEEM 
SET SHOP_NM = (SELECT SHOP_NM FROM T_PRIVILEGE_VERIFY WHERE VERIFY_CODE = @VERIFY_CODE), 
    VERIFY_DT = DATEADD(HOUR, 7, GETDATE())
WHERE id = @REDEEM_ID";

                    db.ExecuteNonQueryFromCommandText(string.Format(cmd, code, privilegeId, token, redeem_id));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<ValidationModel> CheckValidation(string code, int redeem_id)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E601
                    state = ValidationModel.InvalidState.E601;
                    string cmd = @"
DECLARE @REDEEM_ID NVARCHAR(100) = {1}
DECLARE @PRIVILEGE_ID INT
SET @PRIVILEGE_ID = (SELECT PRIVILEGE_ID FROM T_CUSTOMER_REDEEM WHERE id = @REDEEM_ID)
SELECT COUNT(1) AS CNT FROM T_PRIVILEGE_VERIFY WHERE VERIFY_CODE = N'{0}' AND PRIVILEGE_ID = @PRIVILEGE_ID AND DEL_FLAG IS NULL";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, code, redeem_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = ValidationModel.GetInvalidMessage(state) };
                        }
                    }
                    #endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        private async Task<ValidationModel> CheckValidationNew(string code, int redeem_id, string lang)
        {
            ValidationModel value = new ValidationModel();
            try
            {
                ValidationModel.InvalidState state;
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    #region E601
                    state = ValidationModel.InvalidState.E601;
                    string cmd = @"
DECLARE @REDEEM_ID INT = {1}
DECLARE @PRIVILEGE_ID INT
SET @PRIVILEGE_ID = (SELECT PRIVILEGE_ID FROM T_CUSTOMER_REDEEM WHERE id = @REDEEM_ID)
SELECT COUNT(1) AS CNT FROM T_PRIVILEGE_VERIFY WHERE VERIFY_CODE = N'{0}' AND PRIVILEGE_ID = @PRIVILEGE_ID AND DEL_FLAG IS NULL";
                    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, code, redeem_id)))
                    {
                        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                        {
                            return new ValidationModel { Success = false, InvalidCode = ValidationModel.GetInvalidCode(state), InvalidMessage = await ValidationModel.GetInvalidMessageNew(state, lang) };
                        }
                    }
                    #endregion
                }

                value.Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
    }
}