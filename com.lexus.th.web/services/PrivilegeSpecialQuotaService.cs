using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class PrivilegeSpecialQuotaService
    {
        private string conn;
        public PrivilegeSpecialQuotaService()
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
        public DataTable GetPrivilegeSpecialQuotaByPrivilege(string privilegeId, bool isBlockQuota)
        {
            DataTable dt = new DataTable();
            try
            {

                string cmd = @"
DECLARE @PRIVILEGE_ID INT = {0}
DECLARE @is_block_quota bit = {1}

if @is_block_quota = 0
begin
    SELECT SQ.privilege_id, 
    SQ.member_id, 
    SQ.special_quota, 
    SQ.special_usage, 
    SQ.enabled, 
    SQ.create_date, 
    SQ.create_by, 
    SQ.update_date, 
    SQ.update_by, 
    P.TITLE AS [PRIVILEGE_NAME] 
    FROM [T_PRIVILEGE_SPECIAL_QUOTA] SQ
    INNER JOIN	[T_PRIVILEDGES] P ON P.[ID] = SQ.[privilege_id]
    where enabled = 1
    and P.ID = @PRIVILEGE_ID
    and special_quota = -1
end
else
begin
    SELECT SQ.privilege_id, 
    SQ.member_id, 
    SQ.special_quota, 
    SQ.special_usage, 
    SQ.enabled, 
    SQ.create_date, 
    SQ.create_by, 
    SQ.update_date, 
    SQ.update_by, 
    P.TITLE AS [PRIVILEGE_NAME] 
    FROM [T_PRIVILEGE_SPECIAL_QUOTA] SQ
    INNER JOIN	[T_PRIVILEDGES] P ON P.[ID] = SQ.[privilege_id]
    where enabled = 1
    and P.ID = @PRIVILEGE_ID
    and special_quota > -1
end";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        privilegeId, isBlockQuota? 0 : 1);

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetPrivilegeSpecialQuotaByID(string privilegeId, string member_id)
        {
            DataRow row = null;
            try
            {
                string cmd = @"
DECLARE @PRIVILEGE_ID INT = N'{0}'
DECLARE @member_id NVARCHAR(50) = N'{1}'

SELECT SQ.privilege_id, 
SQ.member_id, 
SQ.special_quota, 
SQ.special_usage, 
SQ.enabled, 
SQ.create_date, 
SQ.create_by, 
SQ.update_date, 
SQ.update_by, 
P.TITLE AS [PRIVILEGE_NAME] 
FROM [T_PRIVILEGE_SPECIAL_QUOTA] SQ
INNER JOIN	[T_PRIVILEDGES] P ON P.[ID] = SQ.[privilege_id]
where enabled = 1
and P.ID = @PRIVILEGE_ID
and SQ.member_id = @member_id";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeId),
                        WebUtility.GetSQLTextValue(member_id));

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
        public void AddPrivilegeSpecialQuota(string privilege_id, string member_id, string special_quota, string user)
        {
            int insertCount = 0;
            try
            {
                string cmd = @"
DECLARE @privilege_id INT = N'{0}'
DECLARE @member_id NVARCHAR(50) = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'
DECLARE @special_quota INT = N'{3}'
DECLARE @DupSpecialQuota int = 0
DECLARE @DupBlockQuota int = 0
DECLARE @MemberExit int = 0

set @DupSpecialQuota = (select count(id) from T_PRIVILEGE_SPECIAL_QUOTA where privilege_id = @privilege_id and member_id = @member_id and enabled = 1 and special_quota >= 0)
set @DupBlockQuota = (select count(id) from T_PRIVILEGE_SPECIAL_QUOTA where privilege_id = @privilege_id and member_id = @member_id and enabled = 1 and special_quota = -1)
set @MemberExit = (SELECT count(ID) FROM [T_CUSTOMER] WHERE [MEMBERID] = @member_id and DEL_DT is null)

if @DupSpecialQuota > 0
begin
    select -1
end
else if @DupBlockQuota > 0
begin
    select -2
end
else if @MemberExit = 0
begin
    select -3
end
else
begin
    INSERT INTO [T_PRIVILEGE_SPECIAL_QUOTA] ([privilege_id], [member_id], [special_quota], [special_usage], [enabled], create_date, create_by) 
    VALUES (@privilege_id, @MEMBER_ID, @special_quota, 0, 1, DATEADD(HOUR, 7, GETDATE()), @USER)
    select CAST(SCOPE_IDENTITY() AS INT)
end";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilege_id),
                        WebUtility.GetSQLTextValue(member_id),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(special_quota));

                    insertCount = db.ExecuteScalarFromCommandText<int>(cmd);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            switch (insertCount)
            {
                case -1:
                    throw new System.Exception("Member is already special quota.");
                case -2:
                    throw new System.Exception("Member is already block quota.");
                case -3:
                    throw new System.Exception("Member ID is not found.");
                case 0:
                    throw new System.Exception("Cannot insert please try again.");
                default:
                    break;
            }
        }
        public void UpdatePrivilegeSpecialQuota(string privilege_id, string member_id, string special_quota, string special_usage, string user)
        {
            int updateCount = 0;
            try
            {
                string cmd = @"
DECLARE @privilege_id INT = N'{0}'
DECLARE @member_id NVARCHAR(50) = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'
DECLARE @special_quota INT = N'{3}'
DECLARE @special_usage INT = N'{4}'

UPDATE  [T_PRIVILEGE_SPECIAL_QUOTA]
SET		[special_quota] = @special_quota,
        [special_usage] = @special_usage,
		[update_date] = DATEADD(HOUR, 7, GETDATE()),
		[update_by] = @USER
WHERE	PRIVILEGE_ID = @PRIVILEGE_ID and member_id = @member_id AND enabled = 1";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilege_id),
                        WebUtility.GetSQLTextValue(member_id),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(special_quota),
                        WebUtility.GetSQLTextValue(special_usage));

                    updateCount = db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (updateCount == 0)
            {
                throw new System.Exception("Update quota not found privilege or member.");
            }
        }
        public void DeletePrivilegeSpecialQuota(string privilege_id, string member_id, string user)
        {
            int updateCount = 0;
            try
            {
                string cmd = @"
DECLARE @privilege_id INT = N'{0}'
DECLARE @member_id NVARCHAR(50) = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'

UPDATE  [T_PRIVILEGE_SPECIAL_QUOTA]
SET		[enabled] = 0,
		[update_date] = DATEADD(HOUR, 7, GETDATE()),
		[update_by] = @USER
WHERE	PRIVILEGE_ID = @PRIVILEGE_ID and member_id = @member_id AND enabled = 1";
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilege_id),
                        WebUtility.GetSQLTextValue(member_id),
                        WebUtility.GetSQLTextValue(user));

                    updateCount = db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (updateCount == 0)
            {
                throw new System.Exception("Update quota not found privilege or member.");
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