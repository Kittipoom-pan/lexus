using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class PrivilegeCodeService
    {
        private string conn;
        public PrivilegeCodeService()
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
        public DataTable GetPrivilegeCodeById(string privilegeId)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
DECLARE @PRIVILEGE_ID INT = N'{0}'

SELECT		PC.[PRIVILEGE_ID]
			,PC.[NO]
			,PC.[REDEEM_CODE]
            ,CASE PC.[STATUS] WHEN 'N' THEN 'Redeemed and verified' WHEN 'Y' THEN 'Not Redeem' WHEN 'T' THEN 'Redeemed and Not Verify' END AS [STATUS]
			,PC.[CREATE_DT]
			,PC.[CREATE_USER]
			,PC.[UPDATE_DT]
			,PC.[UPDATE_USER]
			,P.TITLE AS [PRIVILEGE_NAME]
FROM		[T_PRIVILEDGES] P
INNER JOIN	[T_PRIVILEDGES_CODE] PC ON P.[ID] = PC.[PRIVILEGE_ID]
WHERE PC.PRIVILEGE_ID = @PRIVILEGE_ID AND PC.DEL_FLAG IS NULL
order by PC.[NO]";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetPrivilegeCodeById(string privilegeId, string no)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @PRIVILEGE_ID INT = N'{0}'
DECLARE @NO INT = N'{1}'

SELECT		PC.[PRIVILEGE_ID]
			,PC.[NO]
			,PC.[REDEEM_CODE]
			,CASE PC.[STATUS] WHEN 'N' THEN 'Redeemed and verified' WHEN 'Y' THEN 'Not Redeem' WHEN 'T' THEN 'Redeemed and Not Verify' END AS [STATUS]
			,PC.[CREATE_DT]
			,PC.[CREATE_USER]
			,PC.[UPDATE_DT]
			,PC.[UPDATE_USER]
			,P.TITLE AS [PRIVILEGE_NAME]
FROM		[T_PRIVILEDGES] P
INNER JOIN	[T_PRIVILEDGES_CODE] PC ON P.[ID] = PC.[PRIVILEGE_ID]
WHERE PC.PRIVILEGE_ID = @PRIVILEGE_ID AND PC.[NO] = @NO AND PC.DEL_FLAG IS NULL";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeId),
                        WebUtility.GetSQLTextValue(no));

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
        public void AddPrivilegeCode(string privilegeId, string redeemCode, string user, string status)
        {
            try
            {
                string cmd = @"
DECLARE @PRIVILEGE_ID INT = N'{0}'
DECLARE @REDEEM_CODE NVARCHAR(50) = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'
DECLARE @STATUS NVARCHAR(10) = N'{3}'

DECLARE @NO INT = ISNULL((SELECT MAX([NO]) FROM [T_PRIVILEDGES_CODE] WHERE PRIVILEGE_ID = @PRIVILEGE_ID), 0) + 1
INSERT INTO [T_PRIVILEDGES_CODE] ([PRIVILEGE_ID],[NO],[REDEEM_CODE],[STATUS],[CREATE_DT],[CREATE_USER])
VALUES (@PRIVILEGE_ID, @NO, @REDEEM_CODE, @STATUS, DATEADD(HOUR, 7, GETDATE()), @USER)";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeId),
                        WebUtility.GetSQLTextValue(redeemCode),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(status));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePrivilegeCode(string privilegeId, string redeemCode, string user, string no, string status)
        {
            try
            {
                string cmd = @"
DECLARE @PRIVILEGE_ID INT = N'{0}'
DECLARE @REDEEM_CODE NVARCHAR(50) = N'{1}'
DECLARE @NO INT = N'{2}'
DECLARE @USER NVARCHAR(50) = N'{3}'
DECLARE @STATUS NVARCHAR(10) = N'{4}'

UPDATE  [T_PRIVILEDGES_CODE]
SET		[REDEEM_CODE] = @REDEEM_CODE,
        [STATUS] = @STATUS,
		[UPDATE_DT] = DATEADD(HOUR, 7, GETDATE()),
		[UPDATE_USER] = @USER
WHERE	PRIVILEGE_ID = @PRIVILEGE_ID AND [NO] = @NO";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeId),
                        WebUtility.GetSQLTextValue(redeemCode),
                        WebUtility.GetSQLTextValue(no),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(status)
                        );

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePrivilegeCode(string privilegeId, string no, string user)
        {
            try
            {
                string cmd = @"
DECLARE @PRIVILEGE_ID INT = N'{0}'
DECLARE @NO INT = N'{1}'
DECLARE @USER  NVARCHAR(50) = N'{2}'

UPDATE	T_PRIVILEDGES_CODE
SET		DEL_FLAG = 'Y',
        DEL_DT = DATEADD(HOUR, 7, GETDATE()),
        DEL_USER = @USER
WHERE	PRIVILEGE_ID = @PRIVILEGE_ID AND [NO] = @NO";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeId),
                        WebUtility.GetSQLTextValue(no),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsDupplicateRedeem(string privilegeId, string redeemCode)
        {
            bool isDupp = true;
            try
            {
                string cmd = @"
DECLARE @PRIVILEGE_ID INT = N'{0}'
DECLARE @REDEEM_CODE NVARCHAR(50) = N'{1}'

SELECT COUNT(1) FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = @PRIVILEGE_ID AND REDEEM_CODE = @REDEEM_CODE";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeId),
                        WebUtility.GetSQLTextValue(redeemCode));

                    int cnt = db.ExecuteScalarFromCommandText<int>(cmd);
                    if (cnt == 0)
                    {
                        isDupp = false;
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return isDupp;
        }
        public void UploadPrivilegeCode(string privilegeId, List<string> redeemCode, string user)
        {
            try
            {
                string cmd = @"
DECLARE @PRIVILEGE_ID INT = N'{0}'
DECLARE @REDEEM_CODE NVARCHAR(50) = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'

DECLARE @NO INT = ISNULL((SELECT MAX([NO]) FROM [T_PRIVILEDGES_CODE] WHERE PRIVILEGE_ID = @PRIVILEGE_ID), 0) + 1
INSERT INTO [T_PRIVILEDGES_CODE] ([PRIVILEGE_ID],[NO],[REDEEM_CODE],[STATUS],[CREATE_DT],[CREATE_USER])
VALUES (@PRIVILEGE_ID,@NO,@REDEEM_CODE,'Y',DATEADD(HOUR, 7, GETDATE()),@USER)";

                List<string> cmdList = new List<string>();
                foreach (string code in redeemCode)
                {
                    cmdList.Add(string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeId),
                        WebUtility.GetSQLTextValue(code),
                        WebUtility.GetSQLTextValue(user)));
                }

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ExecuteNonQueryFromCommandText(cmdList);
                }

                cmdList = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}