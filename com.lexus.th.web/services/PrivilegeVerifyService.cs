using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;
using System.Data.SqlClient;

namespace com.lexus.th.web
{
    public class PrivilegeVerifyService
    {
        private string conn;
        public PrivilegeVerifyService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetPrivileges()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
SELECT [ID] AS [PRIVILEGE_ID]
      ,[TITLE] AS [PRIVILEGE_NAME]
FROM  [T_PRIVILEDGES]
WHERE DEL_FLAG IS NULL";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetPrivilegeVerifyById(string privilegeId)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
--DECLARE @PRIVILEGE_ID INT = N'{0}'

SELECT		PV.[PRIVILEGE_ID]
			,PV.[SHOP_NM]
			,PV.[VERIFY_CODE]
			,CASE PV.[STATUS] WHEN 'N' THEN 'Redeemed and verified' WHEN 'Y' THEN 'Not Redeem' WHEN 'T' THEN 'Redeemed and Not Verify' END AS [STATUS]
			,PV.[CREATE_DT]
			,PV.[CREATE_USER]
			,PV.[UPDATE_DT]
			,PV.[UPDATE_USER]
			,P.TITLE AS [PRIVILEGE_NAME]
FROM		[T_PRIVILEDGES] P
INNER JOIN	[T_PRIVILEGE_VERIFY] PV ON P.[ID] = PV.[PRIVILEGE_ID]
WHERE PV.PRIVILEGE_ID = @PRIVILEGE_ID AND PV.DEL_FLAG IS NULL
order by PV.[CREATE_DT]";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(privilegeId));
                    db.ParamterList.Add(new SqlParameter("@PRIVILEGE_ID", privilegeId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetPrivilegeVerifyById(string privilegeId, string shopName)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
--DECLARE @PRIVILEGE_ID INT = N'{0}'
--DECLARE @SHOP_NM NVARCHAR(150) = N'{1}'

SELECT		PV.[PRIVILEGE_ID]
			,PV.[SHOP_NM]
			,PV.[VERIFY_CODE]
			,CASE PV.[STATUS] WHEN 'N' THEN 'Redeemed and verified' WHEN 'Y' THEN 'Not Redeem' WHEN 'T' THEN 'Redeemed and Not Verify' END AS [STATUS]
			,PV.[CREATE_DT]
			,PV.[CREATE_USER]
			,PV.[UPDATE_DT]
			,PV.[UPDATE_USER]
			,P.TITLE AS [PRIVILEGE_NAME]
FROM		[T_PRIVILEDGES] P
INNER JOIN	[T_PRIVILEGE_VERIFY] PV ON P.[ID] = PV.[PRIVILEGE_ID]
WHERE PV.PRIVILEGE_ID = @PRIVILEGE_ID AND PV.[SHOP_NM] = @SHOP_NM AND PV.DEL_FLAG IS NULL";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(privilegeId),
                    //    WebUtility.GetSQLTextValue(shopName));
                    db.ParamterList.Add(new SqlParameter("@PRIVILEGE_ID", privilegeId));
                    db.ParamterList.Add(new SqlParameter("@SHOP_NM", shopName));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            row = r;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        public void AddPrivilegeVerify(string privilegeId, string shopName, string verifyCode, string user)
        {
            try
            {
                string cmd = @"
--DECLARE @PRIVILEGE_ID INT = N'{0}'
--DECLARE @SHOP_NM NVARCHAR(150) = N'{1}'
--DECLARE @VERIFY_CODE NVARCHAR(50) = N'{2}'
--DECLARE @USER NVARCHAR(50) = N'{2}'

INSERT INTO [T_PRIVILEGE_VERIFY] ([PRIVILEGE_ID],[SHOP_NM],[VERIFY_CODE],[STATUS],[CREATE_DT],[CREATE_USER])
VALUES (@PRIVILEGE_ID,@SHOP_NM,@VERIFY_CODE,'Y',DATEADD(HOUR, 7, GETDATE()),@USER)";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(privilegeId),
                    //    WebUtility.GetSQLTextValue(shopName),
                    //    WebUtility.GetSQLTextValue(verifyCode),
                    //    WebUtility.GetSQLTextValue(user));

                    db.ParamterList.Add(new SqlParameter("@PRIVILEGE_ID", privilegeId));
                    db.ParamterList.Add(new SqlParameter("@SHOP_NM", shopName));
                    db.ParamterList.Add(new SqlParameter("@VERIFY_CODE", verifyCode));
                    db.ParamterList.Add(new SqlParameter("@USER", user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePrivilegeVerify(string privilegeId, string shopName, string shopNameOld, string verifyCode, string user)
        {
            try
            {
                string cmd = @"
--DECLARE @PRIVILEGE_ID INT = N'{0}'
--DECLARE @SHOP_NM NVARCHAR(150) = N'{1}'
--DECLARE @SHOP_NM_OLD NVARCHAR(150) = N'{2}'
--DECLARE @VERIFY_CODE NVARCHAR(50) = N'{3}'
--DECLARE @USER NVARCHAR(50) = N'{3}'

UPDATE  [T_PRIVILEGE_VERIFY]
SET		[VERIFY_CODE] = @VERIFY_CODE,
		[SHOP_NM] = @SHOP_NM,
		[UPDATE_DT] = DATEADD(HOUR, 7, GETDATE()),
		[UPDATE_USER] = @USER
WHERE	PRIVILEGE_ID = @PRIVILEGE_ID AND [SHOP_NM] = @SHOP_NM_OLD";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(privilegeId),
                    //    WebUtility.GetSQLTextValue(shopName),
                    //    WebUtility.GetSQLTextValue(shopNameOld),
                    //    WebUtility.GetSQLTextValue(verifyCode),
                    //    WebUtility.GetSQLTextValue(user));

                    db.ParamterList.Add(new SqlParameter("@PRIVILEGE_ID", privilegeId));
                    db.ParamterList.Add(new SqlParameter("@SHOP_NM", shopName));
                    db.ParamterList.Add(new SqlParameter("@SHOP_NM_OLD", shopNameOld));
                    db.ParamterList.Add(new SqlParameter("@VERIFY_CODE", verifyCode));
                    db.ParamterList.Add(new SqlParameter("@USER", user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePrivilegeVerify(string privilegeId, string shopName, string user)
        {
            try
            {
                string cmd = @"
--DECLARE @PRIVILEGE_ID INT = N'{0}'
--DECLARE @SHOP_NM NVARCHAR(150) = N'{1}'
--DECLARE @USER  NVARCHAR(50) = N'{2}'

UPDATE	T_PRIVILEGE_VERIFY
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE PRIVILEGE_ID = @PRIVILEGE_ID AND [SHOP_NM] = @SHOP_NM";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(privilegeId),
                    //    WebUtility.GetSQLTextValue(shopName),
                    //    WebUtility.GetSQLTextValue(user));

                    db.ParamterList.Add(new SqlParameter("@PRIVILEGE_ID", privilegeId));
                    db.ParamterList.Add(new SqlParameter("@SHOP_NM", shopName));
                    db.ParamterList.Add(new SqlParameter("@USER", user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsDupplicateVerify(string privilegeId, string verifyCode)
        {
            bool isDupp = true;
            try
            {
                string cmd = @"
--DECLARE @VERIFY_CODE NVARCHAR(50) = N'{0}'

SELECT COUNT(1) AS CNT FROM T_PRIVILEGE_VERIFY 
WHERE PRIVILEGE_ID = @PRIVILEGE_ID AND VERIFY_CODE = @VERIFY_CODE AND DEL_FLAG IS NULL ";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(verifyCode));
                    db.ParamterList.Add(new SqlParameter("@PRIVILEGE_ID", privilegeId));
                    db.ParamterList.Add(new SqlParameter("@VERIFY_CODE", verifyCode));
                    int cnt = db.ExecuteScalarFromCommandText<int>(cmd);
                    if (cnt == 0)
                    {
                        isDupp = false;
                    }
                }

                //using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                //{
                //    string cmd = @"SELECT COUNT(1) AS CNT FROM T_PRIVILEGE_VERIFY WHERE PRIVILEGE_ID = N'{0}' AND VERIFY_CODE = N'{1}'";
                //    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilegeId, verifyCode)))
                //    {
                //        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                //        {
                //            isDupp = false;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDupp;
        }

        public bool IsDupplicateShopNM(string privilegeId, string shop_nm)
        {
            bool isDupp = true;
            try
            {
                string cmd = @"
--DECLARE @VERIFY_CODE NVARCHAR(50) = N'{0}'

SELECT COUNT(1) AS CNT FROM T_PRIVILEGE_VERIFY 
WHERE PRIVILEGE_ID = @PRIVILEGE_ID AND SHOP_NM = @SHOP_NM";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    //cmd = string.Format(cmd,
                    //    WebUtility.GetSQLTextValue(verifyCode));
                    db.ParamterList.Add(new SqlParameter("@PRIVILEGE_ID", privilegeId));
                    db.ParamterList.Add(new SqlParameter("@SHOP_NM", shop_nm));
                    int cnt = db.ExecuteScalarFromCommandText<int>(cmd);
                    if (cnt == 0)
                    {
                        isDupp = false;
                    }
                }

                //using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                //{
                //    string cmd = @"SELECT COUNT(1) AS CNT FROM T_PRIVILEGE_VERIFY WHERE PRIVILEGE_ID = N'{0}' AND VERIFY_CODE = N'{1}'";
                //    using (DataTable dt = db.GetDataTableFromCommandText(string.Format(cmd, privilegeId, verifyCode)))
                //    {
                //        if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                //        {
                //            isDupp = false;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDupp;
        }

    }
}