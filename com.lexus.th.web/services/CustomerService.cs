using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;
using System.Globalization;
using System.Data.SqlClient;

namespace com.lexus.th.web
{
    public class CustomerService
    {
        private string conn;
        public CustomerService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetCustomerSearch(string searchValue, bool isMemberId, bool isFname, bool isMobile, bool isVin, bool isCreateDate, string startDate, string endDate, string role)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'
DECLARE @S_MEMBERID BIT = N'{1}'
DECLARE @S_FNAME BIT = N'{2}'
DECLARE @S_MOBILE BIT = N'{3}'
DECLARE @S_VIN BIT = N'{4}'
DECLARE @S_CREATEDATE BIT = N'{5}'
DECLARE @START NVARCHAR(20) = N'{6}'
DECLARE @END NVARCHAR(20) = N'{7}'

SELECT		DISTINCT CUS.[ID]
			,ISNULL(CUS.[MEMBERID], '') AS [MEMBERID]
			,CUS.[FNAME]
			,CUS.[LNAME]
			,CUS.[GENDER]
			,CUS.[AGE]
			,CUS.[EMAIL]
			,CUS.citizen_id AS [SSN]
			,CUS.[MOBILE]
			,CUS.[PRIVILEGE_CNT]
			,CUS.[EXPIRY]
			,CUS.[CREATE_DT]
			,CUS.[CREATE_USER]
			,CUS.[UPDATE_DT]
			,CUS.[UPDATE_USER]
			,CUS.[NICKNAME]
			,CUS.[PROFILE_IMAGE]
			,CUS.[BIRTHDATE]
			,CUS.[ADDRESS1]
			,CUS.[ADDRESS2]
			,CUS.[SUBDISTRICT]
			,CUS.[DISTRICT]
			,CUS.[PROVINCE]
			,CUS.[POSTALCODE]
			,CUS.[HOME_NO]
			,CUS.[TITLENAME]
            ,CUS.[DEL_FLAG]
            ,CASE CUS.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
            ,ISNULL(CUS.SELLERID, '') AS [SELLERID]
FROM		T_CUSTOMER_CAR_OWNER CUS
LEFT JOIN	T_CUSTOMER_CAR CAR ON CAR.MEMBERID = CUS.MEMBERID
WHERE		(
                ISNULL(CUS.MEMBERID, '') LIKE CASE @S_MEMBERID WHEN 1 THEN @Value + '%' END
                OR ISNULL(CUS.MOBILE, '') LIKE CASE @S_MOBILE WHEN 1 THEN @Value + '%' END
                OR ISNULL(CAR.VIN, '') LIKE CASE @S_VIN WHEN 1 THEN @Value + '%' END
			    OR ISNULL(CUS.FNAME, '') LIKE CASE @S_FNAME WHEN 1 THEN @Value + '%' END
			    OR (CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) >= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END
				    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) <= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END)
            )
ORDER BY CUS.ID";
                    //                    if (role == "2") // Admin
                    //                    {
                    //                        cmd = @"
                    //DECLARE @Value NVARCHAR(255) = N'{0}'
                    //DECLARE @S_MEMBERID BIT = N'{1}'
                    //DECLARE @S_FNAME BIT = N'{2}'
                    //DECLARE @S_MOBILE BIT = N'{3}'
                    //DECLARE @S_VIN BIT = N'{4}'
                    //DECLARE @S_CREATEDATE BIT = N'{5}'
                    //DECLARE @START NVARCHAR(20) = N'{6}'
                    //DECLARE @END NVARCHAR(20) = N'{7}'

                    //SELECT		DISTINCT CUS.[ID]
                    //			,ISNULL(CUS.[MEMBERID], '') AS [MEMBERID]
                    //			,CUS.[FNAME]
                    //			,CUS.[LNAME]
                    //			,CUS.[GENDER]
                    //			,CUS.[AGE]
                    //			,CUS.[EMAIL]
                    //			,CUS.citizen_id AS [SSN]
                    //			,CUS.[MOBILE]
                    //			,CUS.[PRIVILEGE_CNT]
                    //			,CUS.[EXPIRY]
                    //			,CUS.[CREATE_DT]
                    //			,CUS.[CREATE_USER]
                    //			,CUS.[UPDATE_DT]
                    //			,CUS.[UPDATE_USER]
                    //			,CUS.[NICKNAME]
                    //			,CUS.[PROFILE_IMAGE]
                    //			,CUS.[BIRTHDATE]
                    //			,CUS.[ADDRESS1]
                    //			,CUS.[ADDRESS2]
                    //			,CUS.[SUBDISTRICT]
                    //			,CUS.[DISTRICT]
                    //			,CUS.[PROVINCE]
                    //			,CUS.[POSTALCODE]
                    //			,CUS.[HOME_NO]
                    //			,CUS.[TITLENAME]
                    //            ,CUS.[DEL_FLAG]
                    //            ,CASE CUS.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
                    //            ,ISNULL(CUS.SELLERID, '') AS [SELLERID]
                    //FROM		T_CUSTOMER_CAR_OWNER CUS
                    //LEFT JOIN	T_CUSTOMER_CAR CAR ON CAR.MEMBERID = CUS.MEMBERID
                    //WHERE		(
                    //                ISNULL(CUS.MEMBERID, '') LIKE CASE @S_MEMBERID WHEN 1 THEN @Value + '%' END
                    //                OR ISNULL(CUS.MOBILE, '') LIKE CASE @S_MOBILE WHEN 1 THEN @Value + '%' END
                    //                OR ISNULL(CAR.VIN, '') LIKE CASE @S_VIN WHEN 1 THEN @Value + '%' END
                    //			    OR ISNULL(CUS.FNAME, '') LIKE CASE @S_FNAME WHEN 1 THEN @Value + '%' END
                    //			    OR (CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) >= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END
                    //				    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) <= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END)
                    //            )
                    //ORDER BY CUS.ID";
                    //                    }
                    //                    else
                    //                    {
                    //                        cmd = @"
                    //DECLARE @Value NVARCHAR(255) = N'{0}'
                    //DECLARE @S_MEMBERID BIT = N'{1}'
                    //DECLARE @S_FNAME BIT = N'{2}'
                    //DECLARE @S_MOBILE BIT = N'{3}'
                    //DECLARE @S_VIN BIT = N'{4}'
                    //DECLARE @S_CREATEDATE BIT = N'{5}'
                    //DECLARE @START NVARCHAR(20) = N'{6}'
                    //DECLARE @END NVARCHAR(20) = N'{7}'

                    //SELECT		DISTINCT CUS.[ID]
                    //			,ISNULL(CUS.[MEMBERID], '') AS [MEMBERID]
                    //			,CUS.[FNAME]
                    //			,CUS.[LNAME]
                    //			,CUS.[GENDER]
                    //			,CUS.[AGE]
                    //			,CUS.[EMAIL]
                    //			,CUS.citizen_id AS [SSN]
                    //			,CUS.[MOBILE]
                    //			,CUS.[PRIVILEGE_CNT]
                    //			,CUS.[EXPIRY]
                    //			,CUS.[CREATE_DT]
                    //			,CUS.[CREATE_USER]
                    //			,CUS.[UPDATE_DT]
                    //			,CUS.[UPDATE_USER]
                    //			,CUS.[NICKNAME]
                    //			,CUS.[PROFILE_IMAGE]
                    //			,CUS.[BIRTHDATE]
                    //			,CUS.[ADDRESS1]
                    //			,CUS.[ADDRESS2]
                    //			,CUS.[SUBDISTRICT]
                    //			,CUS.[DISTRICT]
                    //			,CUS.[PROVINCE]
                    //			,CUS.[POSTALCODE]
                    //			,CUS.[HOME_NO]
                    //			,CUS.[TITLENAME]
                    //            ,CUS.[DEL_FLAG]
                    //            ,CASE CUS.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
                    //            ,ISNULL(CUS.SELLERID, '') AS [SELLERID]
                    //FROM		T_CUSTOMER_CAR_OWNER CUS
                    //LEFT JOIN	T_CUSTOMER_CAR CAR ON CAR.MEMBERID = CUS.MEMBERID
                    //WHERE		(
                    //                ISNULL(CUS.MEMBERID, '') = CASE @S_MEMBERID WHEN 1 THEN @Value END
                    //                OR ISNULL(CUS.MOBILE, '') = CASE @S_MOBILE WHEN 1 THEN @Value END
                    //                OR ISNULL(CAR.VIN, '') = CASE @S_VIN WHEN 1 THEN @Value END
                    //			    OR ISNULL(CUS.FNAME, '') LIKE CASE @S_FNAME WHEN 1 THEN @Value + '%' END
                    //			    OR (CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) >= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END
                    //				    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) <= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END)
                    //            )
                    //ORDER BY CUS.ID";
                    //                    }

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(searchValue),
                        (isMemberId) ? 1 : 0,
                        (isFname) ? 1 : 0,
                        (isMobile) ? 1 : 0,
                        (isVin) ? 1 : 0,
                        (isCreateDate) ? 1 : 0,
                        WebUtility.GetSQLTextValue(startDate),
                        WebUtility.GetSQLTextValue(endDate));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetAppSearch(string searchValue, bool isMemberId, bool isFname, bool isMobile, bool isVin, bool isCreateDate, string startDate, string endDate, string role)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";
                    cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'
DECLARE @S_MEMBERID BIT = N'{1}'
DECLARE @S_FNAME BIT = N'{2}'
DECLARE @S_MOBILE BIT = N'{3}'
DECLARE @S_VIN BIT = N'{4}'
DECLARE @S_CREATEDATE BIT = N'{5}'
DECLARE @START NVARCHAR(20) = N'{6}'
DECLARE @END NVARCHAR(20) = N'{7}'

SELECT		DISTINCT CUS.[ID]
			,ISNULL(CUS.[MEMBERID], '') AS [MEMBERID]
            ,CUS.[TITLENAME]
			,CUS.[FNAME]
			,CUS.[LNAME]
            ,CUS.[EMAIL]
            ,CUS.[MOBILE]
            ,CUS.citizen_id AS [SSN]
            ,CUS.[PRIVILEGE_CNT]
            ,CUS.[EXPIRY]
            ,CUS.[DEL_FLAG]
			,CUS.[GENDER]
			,CUS.[AGE]
			,CUS.[CREATE_DT]
			,CUS.[CREATE_USER]
			,CUS.[UPDATE_DT]
			,CUS.[UPDATE_USER]
			,CUS.[NICKNAME]
			,CUS.[PROFILE_IMAGE]
			,CUS.[BIRTHDATE]
			,CUS.[ADDRESS1]
			,CUS.[ADDRESS2]
			,CUS.[SUBDISTRICT]
			,CUS.[DISTRICT]
			,CUS.[PROVINCE]
			,CUS.[POSTALCODE]
			,CUS.[HOME_NO]
            ,CASE CUS.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
            ,ISNULL(CUS.SELLERID, '') AS [SELLERID]
FROM		T_CUSTOMER CUS
LEFT JOIN	T_CUSTOMER_CAR CAR ON CAR.MEMBERID = CUS.MEMBERID
WHERE		(
                ISNULL(CUS.MEMBERID, '') LIKE CASE @S_MEMBERID WHEN 1 THEN @Value + '%' END
                OR ISNULL(CUS.MOBILE, '') LIKE CASE @S_MOBILE WHEN 1 THEN @Value + '%' END
                OR ISNULL(CAR.VIN, '') LIKE CASE @S_VIN WHEN 1 THEN @Value + '%' END
			    OR ISNULL(CUS.FNAME, '') LIKE CASE @S_FNAME WHEN 1 THEN @Value + '%' END
			    OR (CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) >= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END
				    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) <= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END)
            )
ORDER BY CUS.ID";
                    //                    if (role == "2") // Admin
                    //                    {
                    //                        cmd = @"
                    //DECLARE @Value NVARCHAR(255) = N'{0}'
                    //DECLARE @S_MEMBERID BIT = N'{1}'
                    //DECLARE @S_FNAME BIT = N'{2}'
                    //DECLARE @S_MOBILE BIT = N'{3}'
                    //DECLARE @S_VIN BIT = N'{4}'
                    //DECLARE @S_CREATEDATE BIT = N'{5}'
                    //DECLARE @START NVARCHAR(20) = N'{6}'
                    //DECLARE @END NVARCHAR(20) = N'{7}'

                    //SELECT		DISTINCT CUS.[ID]
                    //			,ISNULL(CUS.[MEMBERID], '') AS [MEMBERID]
                    //            ,CUS.[TITLENAME]
                    //			,CUS.[FNAME]
                    //			,CUS.[LNAME]
                    //            ,CUS.[EMAIL]
                    //            ,CUS.[MOBILE]
                    //            ,CUS.citizen_id AS [SSN]
                    //            ,CUS.[PRIVILEGE_CNT]
                    //            ,CUS.[EXPIRY]
                    //            ,CUS.[DEL_FLAG]
                    //			,CUS.[GENDER]
                    //			,CUS.[AGE]
                    //			,CUS.[CREATE_DT]
                    //			,CUS.[CREATE_USER]
                    //			,CUS.[UPDATE_DT]
                    //			,CUS.[UPDATE_USER]
                    //			,CUS.[NICKNAME]
                    //			,CUS.[PROFILE_IMAGE]
                    //			,CUS.[BIRTHDATE]
                    //			,CUS.[ADDRESS1]
                    //			,CUS.[ADDRESS2]
                    //			,CUS.[SUBDISTRICT]
                    //			,CUS.[DISTRICT]
                    //			,CUS.[PROVINCE]
                    //			,CUS.[POSTALCODE]
                    //			,CUS.[HOME_NO]
                    //            ,CASE CUS.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
                    //            ,ISNULL(CUS.SELLERID, '') AS [SELLERID]
                    //FROM		T_CUSTOMER CUS
                    //LEFT JOIN	T_CUSTOMER_CAR CAR ON CAR.MEMBERID = CUS.MEMBERID
                    //WHERE		(
                    //                ISNULL(CUS.MEMBERID, '') LIKE CASE @S_MEMBERID WHEN 1 THEN @Value + '%' END
                    //                OR ISNULL(CUS.MOBILE, '') LIKE CASE @S_MOBILE WHEN 1 THEN @Value + '%' END
                    //                OR ISNULL(CAR.VIN, '') LIKE CASE @S_VIN WHEN 1 THEN @Value + '%' END
                    //			    OR ISNULL(CUS.FNAME, '') LIKE CASE @S_FNAME WHEN 1 THEN @Value + '%' END
                    //			    OR (CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) >= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END
                    //				    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) <= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END)
                    //            )
                    //ORDER BY CUS.ID";
                    //                    }
                    //                    else
                    //                    {
                    //                        cmd = @"
                    //DECLARE @Value NVARCHAR(255) = N'{0}'
                    //DECLARE @S_MEMBERID BIT = N'{1}'
                    //DECLARE @S_FNAME BIT = N'{2}'
                    //DECLARE @S_MOBILE BIT = N'{3}'
                    //DECLARE @S_VIN BIT = N'{4}'
                    //DECLARE @S_CREATEDATE BIT = N'{5}'
                    //DECLARE @START NVARCHAR(20) = N'{6}'
                    //DECLARE @END NVARCHAR(20) = N'{7}'

                    //SELECT		DISTINCT CUS.[ID]
                    //			,ISNULL(CUS.[MEMBERID], '') AS [MEMBERID]
                    //			,CUS.[FNAME]
                    //			,CUS.[LNAME]
                    //			,CUS.[GENDER]
                    //			,CUS.[AGE]
                    //			,CUS.[EMAIL]
                    //			,CUS.citizen_id AS [SSN]
                    //			,CUS.[MOBILE]
                    //			,CUS.[PRIVILEGE_CNT]
                    //			,CUS.[EXPIRY]
                    //			,CUS.[CREATE_DT]
                    //			,CUS.[CREATE_USER]
                    //			,CUS.[UPDATE_DT]
                    //			,CUS.[UPDATE_USER]
                    //			,CUS.[NICKNAME]
                    //			,CUS.[PROFILE_IMAGE]
                    //			,CUS.[BIRTHDATE]
                    //			,CUS.[ADDRESS1]
                    //			,CUS.[ADDRESS2]
                    //			,CUS.[SUBDISTRICT]
                    //			,CUS.[DISTRICT]
                    //			,CUS.[PROVINCE]
                    //			,CUS.[POSTALCODE]
                    //			,CUS.[HOME_NO]
                    //			,CUS.[TITLENAME]
                    //            ,CUS.[DEL_FLAG]
                    //            ,CASE CUS.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
                    //            ,ISNULL(CUS.SELLERID, '') AS [SELLERID]
                    //FROM		T_CUSTOMER CUS
                    //LEFT JOIN	T_CUSTOMER_CAR CAR ON CAR.MEMBERID = CUS.MEMBERID
                    //WHERE		(
                    //                ISNULL(CUS.MEMBERID, '') = CASE @S_MEMBERID WHEN 1 THEN @Value END
                    //                OR ISNULL(CUS.MOBILE, '') = CASE @S_MOBILE WHEN 1 THEN @Value END
                    //                OR ISNULL(CAR.VIN, '') = CASE @S_VIN WHEN 1 THEN @Value END
                    //			    OR ISNULL(CUS.FNAME, '') LIKE CASE @S_FNAME WHEN 1 THEN @Value + '%' END
                    //			    OR (CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) >= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END
                    //				    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) <= CASE @S_CREATEDATE WHEN 1 THEN CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, CUS.[CREATE_DT]), 120) END END)
                    //            )
                    //ORDER BY CUS.ID";
                    //                    }

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(searchValue),
                        (isMemberId) ? 1 : 0,
                        (isFname) ? 1 : 0,
                        (isMobile) ? 1 : 0,
                        (isVin) ? 1 : 0,
                        (isCreateDate) ? 1 : 0,
                        WebUtility.GetSQLTextValue(startDate),
                        WebUtility.GetSQLTextValue(endDate));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataRow GetCustomerById(string member_id)
        {
            DataRow row = null;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"

DECLARE @MemberID NVARCHAR(50) = N'{0}'

SELECT [ID]
      ,ISNULL([MEMBERID], '') AS [MEMBERID]
      ,[FNAME]
      ,[LNAME]
      ,[GENDER]
      ,[AGE]
      ,[EMAIL]
      ,citizen_id AS [SSN]
      ,[MOBILE]
      ,[PRIVILEGE_CNT]
      ,[EXPIRY]
      ,[CREATE_DT]
      ,[CREATE_USER]
      ,[UPDATE_DT]
      ,[UPDATE_USER]
      ,[NICKNAME]
      ,[PROFILE_IMAGE]
      ,[BIRTHDATE]
      ,[ADDRESS1]
      ,[ADDRESS2]
      ,[SUBDISTRICT]
      ,[DISTRICT]
      ,[PROVINCE]
      ,[POSTALCODE]
      ,[HOME_NO]
      ,[TITLENAME]
      ,[DEL_FLAG]
      ,CASE [DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
      ,[REMARK]
      ,ISNULL(SELLERID, '') AS [SELLERID]
  FROM T_CUSTOMER_CAR_OWNER
  WHERE	MEMBERID = @MemberID";

                    row = db.GetDataTableFromCommandText(string.Format(cmd, member_id)).AsEnumerable().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        public DataRow GetAppUserById(string id)
        {
            DataRow row = null;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @Value NVARCHAR(250) = N'{0}'

SELECT [ID]
      ,ISNULL([MEMBERID], '') AS [MEMBERID]
      ,[FNAME]
      ,[LNAME]
      ,[GENDER]
      ,[AGE]
      ,[EMAIL]
      ,citizen_id AS [SSN]
      ,[MOBILE]
      ,[PRIVILEGE_CNT]
      ,[EXPIRY]
      ,[CREATE_DT]
      ,[CREATE_USER]
      ,[UPDATE_DT]
      ,[UPDATE_USER]
      ,[NICKNAME]
      ,[PROFILE_IMAGE]
      ,[BIRTHDATE]
      ,[ADDRESS1]
      ,[ADDRESS2]
      ,[SUBDISTRICT]
      ,[DISTRICT]
      ,[PROVINCE]
      ,[POSTALCODE]
      ,[HOME_NO]
      ,[TITLENAME]
      ,[DEL_FLAG]
      ,CASE [DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
      ,[REMARK]
      ,ISNULL(SELLERID, '') AS [SELLERID]
  FROM T_CUSTOMER
  WHERE	ID = {0}";

                    row = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id))).AsEnumerable().FirstOrDefault();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public DataRow GetAppUserByMemberID(string member_id)
        {
            DataRow row = null;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @Value NVARCHAR(250) = N'{0}'

SELECT [ID]
      ,ISNULL([MEMBERID], '') AS [MEMBERID]
      ,[FNAME]
      ,[LNAME]
      ,[GENDER]
      ,[AGE]
      ,[EMAIL]
      ,[SSN]
      ,[MOBILE]
      ,[PRIVILEGE_CNT]
      ,[EXPIRY]
      ,[CREATE_DT]
      ,[CREATE_USER]
      ,[UPDATE_DT]
      ,[UPDATE_USER]
      ,[NICKNAME]
      ,[PROFILE_IMAGE]
      ,[BIRTHDATE]
      ,[ADDRESS1]
      ,[ADDRESS2]
      ,[SUBDISTRICT]
      ,[DISTRICT]
      ,[PROVINCE]
      ,[POSTALCODE]
      ,[HOME_NO]
      ,[TITLENAME]
      ,[DEL_FLAG]
      ,CASE [DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
      ,[REMARK]
      ,ISNULL(SELLERID, '') AS [SELLERID]
  FROM T_CUSTOMER
  WHERE	MEMBERID = N'{0}'";

                    row = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(member_id))).AsEnumerable().FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        public DataTable GetCustomerSearchById(string id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @Value NVARCHAR(250) = N'{0}'

SELECT [ID]
      ,ISNULL([MEMBERID], '') AS [MEMBERID]
      ,[FNAME]
      ,[LNAME]
      ,[GENDER]
      ,[AGE]
      ,[EMAIL]
      ,[SSN]
      ,[MOBILE]
      ,[PRIVILEGE_CNT]
      ,[EXPIRY]
      ,[CREATE_DT]
      ,[CREATE_USER]
      ,[UPDATE_DT]
      ,[UPDATE_USER]
      ,[NICKNAME]
      ,[PROFILE_IMAGE]
      ,[BIRTHDATE]
      ,[ADDRESS1]
      ,[ADDRESS2]
      ,[SUBDISTRICT]
      ,[DISTRICT]
      ,[PROVINCE]
      ,[POSTALCODE]
      ,[HOME_NO]
      ,[TITLENAME]
      ,[DEL_FLAG]
      ,CASE [DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
      ,ISNULL(SELLERID, '') AS [SELLERID]
  FROM T_CUSTOMER
  WHERE	ID = {0}";

                    dt = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public string AddCustomer(string firstName, string lastName, string nickName, string gender, string age, string email, string ssn, string mobile, string memberId, string user, string title, string birthdate, string addr1, string addr2, string subDistrict, string district, string province, string postcode, string homeNumber, string sellerId)
        {
            string id = "";
            try
            {
                string cmd = @"
DECLARE @FNAME NVARCHAR(250) = N'{0}'
DECLARE @LNAME NVARCHAR(250) = N'{1}'
DECLARE @GENDER NVARCHAR(100) = N'{2}'
DECLARE @EMAIL NVARCHAR(100) = N'{3}'
DECLARE @SSN NVARCHAR(50) = N'{4}'
DECLARE @MOBILE NVARCHAR(50) = N'{5}'
DECLARE @MEMBERID NVARCHAR(50) = N'{6}'
DECLARE @USER NVARCHAR(50) = N'{7}'
DECLARE @TITLENAME NVARCHAR(50) = N'{8}'
DECLARE @BIRTHDATE NVARCHAR(20) = N'{9}'
DECLARE @ADDRESS1 NVARCHAR(300) = N'{10}'
DECLARE @ADDRESS2 NVARCHAR(300) = N'{11}'
DECLARE @SUBDISTRICT NVARCHAR(100) = N'{12}'
DECLARE @DISTRICT NVARCHAR(100) = N'{13}'
DECLARE @PROVINCE NVARCHAR(100) = N'{14}'
DECLARE @POSTALCODE NVARCHAR(10) = N'{15}'
DECLARE @HOME_NO NVARCHAR(30) = N'{16}'
DECLARE @SELLERID NVARCHAR(100) = N'{17}'

DECLARE @RS_Date NVARCHAR(100) = ''
DECLARE @Expiry_Date DATE
DECLARE @rs_year INT
DECLARE @rs_month INT
DECLARE @rs_day INT
DECLARE @regis_date DATE
SET @RS_Date = (SELECT rs_date FROM initial_data WHERE vin = @SSN)
SET @rs_year = (SELECT YEAR(@RS_Date))
SET @rs_month = (SELECT MONTH(@RS_Date))
SET @rs_day = (SELECT DAY(@RS_Date))
SET @regis_date = (select cast(cast(YEAR(GETDATE())*10000 + @rs_month*100 + @rs_day as varchar(255)) as date))
SET @Expiry_Date = (SELECT CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 4, @RS_Date) 
			         THEN DATEADD(YEAR, 4, @RS_Date) 
			         ELSE CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 10, @RS_Date) 
				            THEN  CASE WHEN CONVERT(DATE,GETDATE()) < CONVERT(DATE,@regis_date)
									        THEN @regis_date 
									        ELSE DATEADD(YEAR, 1, @regis_date) 
									        END 
						        ELSE DATEADD(YEAR, 10, @RS_Date) 
						        END 
			         END)

INSERT INTO T_CUSTOMER_CAR_OWNER (MEMBERID, FNAME, LNAME, GENDER, AGE, EMAIL, SSN, MOBILE, CREATE_DT, CREATE_USER, TITLENAME, BIRTHDATE, 
ADDRESS1, ADDRESS2, SUBDISTRICT, DISTRICT, PROVINCE, POSTALCODE, HOME_NO, PRIVILEGE_CNT, EXPIRY, NICKNAME, SELLERID, citizen_id, register_type)
VALUES (
    CASE LEN(@MEMBERID) WHEN 0 THEN NULL ELSE @MEMBERID END,
	CASE LEN(@FNAME) WHEN 0 THEN NULL ELSE @FNAME END,
	CASE LEN(@LNAME) WHEN 0 THEN NULL ELSE @LNAME END,
	CASE LEN(@GENDER) WHEN 0 THEN NULL ELSE @GENDER END,
	CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE DATEDIFF(YEAR, CONVERT(DATE, @BIRTHDATE, 103), GETDATE()) END,
	CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
	CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END,
	CASE LEN(@MOBILE) WHEN 0 THEN NULL ELSE @MOBILE END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER,
	CASE LEN(@TITLENAME) WHEN 0 THEN NULL ELSE @TITLENAME END,
	CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 103) END,
	CASE LEN(@ADDRESS1) WHEN 0 THEN NULL ELSE @ADDRESS1 END,
	CASE LEN(@ADDRESS2) WHEN 0 THEN NULL ELSE @ADDRESS2 END,
	CASE LEN(@SUBDISTRICT) WHEN 0 THEN NULL ELSE @SUBDISTRICT END,
	CASE LEN(@DISTRICT) WHEN 0 THEN NULL ELSE @DISTRICT END,
	CASE LEN(@PROVINCE) WHEN 0 THEN NULL ELSE @PROVINCE END,
	CASE LEN(@POSTALCODE) WHEN 0 THEN NULL ELSE @POSTALCODE END,
	CASE LEN(@HOME_NO) WHEN 0 THEN NULL ELSE @HOME_NO END,
	0,
	@Expiry_Date,
	CASE LEN(@FNAME) + LEN(@LNAME) WHEN 0 THEN NULL ELSE @FNAME + ' ' + @LNAME END,
    CASE LEN(@SELLERID) WHEN 0 THEN NULL ELSE @SELLERID END,
    CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END,
    'CAR_OWNER')

SELECT CONVERT(NVARCHAR, @@IDENTITY) AS [ID]";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(firstName),
                        WebUtility.GetSQLTextValue(lastName),
                        WebUtility.GetSQLTextValue(gender),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(ssn),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(memberId),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(birthdate),
                        WebUtility.GetSQLTextValue(addr1),
                        WebUtility.GetSQLTextValue(addr2),
                        WebUtility.GetSQLTextValue(subDistrict),
                        WebUtility.GetSQLTextValue(district),
                        WebUtility.GetSQLTextValue(province),
                        WebUtility.GetSQLTextValue(postcode),
                        WebUtility.GetSQLTextValue(homeNumber),
                        WebUtility.GetSQLTextValue(sellerId));

                    id = db.ExecuteScalarFromCommandText<string>(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }
        public void UpdateCustomer(string firstName, string lastName, string nickName, string gender, string age, string email, string ssn, string mobile, string id, string memberId, string user, string title, string birthdate, string addr1, string addr2, string subDistrict, string district, string province, string postcode, string homeNumber, string remark, string sellerId)
        {
            try
            {
                string cmd = @"
DECLARE @FNAME NVARCHAR(250) = N'{0}'
DECLARE @LNAME NVARCHAR(250) = N'{1}'
DECLARE @GENDER NVARCHAR(100) = N'{2}'
DECLARE @EMAIL NVARCHAR(100) = N'{3}'
DECLARE @SSN NVARCHAR(50) = N'{4}'
DECLARE @MOBILE NVARCHAR(50) = N'{5}'
DECLARE @MEMBERID NVARCHAR(50) = N'{6}'
DECLARE @USER NVARCHAR(50) = N'{7}'
DECLARE @TITLENAME NVARCHAR(50) = N'{8}'
DECLARE @BIRTHDATE NVARCHAR(20) = N'{9}'
DECLARE @ADDRESS1 NVARCHAR(300) = N'{10}'
DECLARE @ADDRESS2 NVARCHAR(300) = N'{11}'
DECLARE @SUBDISTRICT NVARCHAR(100) = N'{12}'
DECLARE @DISTRICT NVARCHAR(100) = N'{13}'
DECLARE @PROVINCE NVARCHAR(100) = N'{14}'
DECLARE @POSTALCODE NVARCHAR(10) = N'{15}'
DECLARE @HOME_NO NVARCHAR(30) = N'{16}'
DECLARE @REMARK NVARCHAR(MAX) = N'{17}'
DECLARE @ID INT = N'{18}'
DECLARE @SELLERID NVARCHAR(100) = N'{19}'

DECLARE @CountCarOwner INT = (select count(id) from T_CUSTOMER_CAR_OWNER where ID = @ID)

DECLARE @RS_Date NVARCHAR(100) = ''
DECLARE @Expiry_Date DATE
DECLARE @rs_year INT
DECLARE @rs_month INT
DECLARE @rs_day INT
DECLARE @regis_date DATE
SET @RS_Date = (SELECT rs_date FROM initial_data WHERE vin = @SSN)
SET @rs_year = (SELECT YEAR(@RS_Date))
SET @rs_month = (SELECT MONTH(@RS_Date))
SET @rs_day = (SELECT DAY(@RS_Date))
SET @regis_date = (select cast(cast(YEAR(GETDATE())*10000 + @rs_month*100 + @rs_day as varchar(255)) as date))
SET @Expiry_Date = (SELECT CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 4, @RS_Date) 
			         THEN DATEADD(YEAR, 4, @RS_Date) 
			         ELSE CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 10, @RS_Date) 
				            THEN  CASE WHEN CONVERT(DATE,GETDATE()) < CONVERT(DATE,@regis_date)
									        THEN @regis_date 
									        ELSE DATEADD(YEAR, 1, @regis_date) 
									        END 
						        ELSE DATEADD(YEAR, 10, @RS_Date) 
						        END 
			         END)



IF @CountCarOwner > 0 begin
UPDATE T_CUSTOMER_CAR_OWNER
SET FNAME = CASE LEN(@FNAME) WHEN 0 THEN NULL ELSE @FNAME END,
	LNAME = CASE LEN(@LNAME) WHEN 0 THEN NULL ELSE @LNAME END,
	GENDER = CASE LEN(@GENDER) WHEN 0 THEN NULL ELSE @GENDER END,
	EMAIL = CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
	SSN = CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END,
	MOBILE = CASE LEN(@MOBILE) WHEN 0 THEN NULL ELSE @MOBILE END,
    MEMBERID = CASE LEN(@MEMBERID) WHEN 0 THEN NULL ELSE @MEMBERID END,
    UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
    UPDATE_USER = @USER,
	AGE = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE DATEDIFF(YEAR, CONVERT(DATE, @BIRTHDATE, 103), GETDATE()) END,
	TITLENAME = CASE LEN(@TITLENAME) WHEN 0 THEN NULL ELSE @TITLENAME END,
	BIRTHDATE = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 103) END,
	ADDRESS1 = CASE LEN(@ADDRESS1) WHEN 0 THEN NULL ELSE @ADDRESS1 END,
	ADDRESS2 = CASE LEN(@ADDRESS2) WHEN 0 THEN NULL ELSE @ADDRESS2 END,
	SUBDISTRICT = CASE LEN(@SUBDISTRICT) WHEN 0 THEN NULL ELSE @SUBDISTRICT END,
	DISTRICT = CASE LEN(@DISTRICT) WHEN 0 THEN NULL ELSE @DISTRICT END,
	PROVINCE = CASE LEN(@PROVINCE) WHEN 0 THEN NULL ELSE @PROVINCE END,
	POSTALCODE = CASE LEN(@POSTALCODE) WHEN 0 THEN NULL ELSE @POSTALCODE END,
	HOME_NO = CASE LEN(@HOME_NO) WHEN 0 THEN NULL ELSE @HOME_NO END, 
    REMARK = ISNULL(CONVERT(NVARCHAR(MAX), REMARK), '') + '|[' + CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) + '] ' + @REMARK,
    SELLERID = CASE LEN(@SELLERID) WHEN 0 THEN NULL ELSE @SELLERID END,
    citizen_id = CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END
WHERE ID = @ID
end
else begin
INSERT INTO T_CUSTOMER_CAR_OWNER (MEMBERID, FNAME, LNAME, GENDER, AGE, EMAIL, SSN, MOBILE, CREATE_DT, CREATE_USER, TITLENAME, BIRTHDATE, 
ADDRESS1, ADDRESS2, SUBDISTRICT, DISTRICT, PROVINCE, POSTALCODE, HOME_NO, PRIVILEGE_CNT, EXPIRY, NICKNAME, SELLERID, citizen_id, register_type)
VALUES (
    CASE LEN(@MEMBERID) WHEN 0 THEN NULL ELSE @MEMBERID END,
	CASE LEN(@FNAME) WHEN 0 THEN NULL ELSE @FNAME END,
	CASE LEN(@LNAME) WHEN 0 THEN NULL ELSE @LNAME END,
	CASE LEN(@GENDER) WHEN 0 THEN NULL ELSE @GENDER END,
	CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE DATEDIFF(YEAR, CONVERT(DATE, @BIRTHDATE, 103), GETDATE()) END,
	CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
	CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END,
	CASE LEN(@MOBILE) WHEN 0 THEN NULL ELSE @MOBILE END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER,
	CASE LEN(@TITLENAME) WHEN 0 THEN NULL ELSE @TITLENAME END,
	CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 103) END,
	CASE LEN(@ADDRESS1) WHEN 0 THEN NULL ELSE @ADDRESS1 END,
	CASE LEN(@ADDRESS2) WHEN 0 THEN NULL ELSE @ADDRESS2 END,
	CASE LEN(@SUBDISTRICT) WHEN 0 THEN NULL ELSE @SUBDISTRICT END,
	CASE LEN(@DISTRICT) WHEN 0 THEN NULL ELSE @DISTRICT END,
	CASE LEN(@PROVINCE) WHEN 0 THEN NULL ELSE @PROVINCE END,
	CASE LEN(@POSTALCODE) WHEN 0 THEN NULL ELSE @POSTALCODE END,
	CASE LEN(@HOME_NO) WHEN 0 THEN NULL ELSE @HOME_NO END,
	0,
	@Expiry_Date,
	CASE LEN(@FNAME) + LEN(@LNAME) WHEN 0 THEN NULL ELSE @FNAME + ' ' + @LNAME END,
    CASE LEN(@SELLERID) WHEN 0 THEN NULL ELSE @SELLERID END,
    CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END,
    'CAR_OWNER')

end";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(firstName),
                        WebUtility.GetSQLTextValue(lastName),
                        WebUtility.GetSQLTextValue(gender),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(ssn),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(memberId),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(birthdate),
                        WebUtility.GetSQLTextValue(addr1),
                        WebUtility.GetSQLTextValue(addr2),
                        WebUtility.GetSQLTextValue(subDistrict),
                        WebUtility.GetSQLTextValue(district),
                        WebUtility.GetSQLTextValue(province),
                        WebUtility.GetSQLTextValue(postcode),
                        WebUtility.GetSQLTextValue(homeNumber),
                        WebUtility.GetSQLTextValue(remark),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(sellerId));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteCustomer(string id, string remark, string user)
        {
            try
            {
                //string cmd = @"DELETE FROM T_CUSTOMER WHERE ID = @ID";
                string cmd = @"
DECLARE @REMARK NVARCHAR(MAX) = N'{0}'
DECLARE @ID INT = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'

UPDATE T_CUSTOMER_CAR_OWNER 
SET DEL_FLAG = 'Y', [EXPIRY] = DATEADD(HOUR, 7, GETDATE()), 
    REMARK = ISNULL(CONVERT(NVARCHAR(MAX), REMARK), '') + '|[' + CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) + '] ' + @REMARK,
    DEL_DT = DATEADD(HOUR, 7, GETDATE()),
    DEL_USER = @USER
WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(remark),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistsMemberId(string memberId)
        {
            bool isExists = true;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    int cnt = db.ExecuteScalarFromCommandText<int>(string.Format("SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MEMBERID = N'{0}'", WebUtility.GetSQLTextValue(memberId)));
                    if (cnt == 0)
                    {
                        isExists = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isExists;
        }
        public bool IsExistsMobile(string mobile)
        {
            bool isExists = true;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    int cnt = db.ExecuteScalarFromCommandText<int>(string.Format("SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE MOBILE = N'{0}' AND ISNULL(DEL_FLAG, 'N') = 'N'", WebUtility.GetSQLTextValue(mobile)));
                    if (cnt == 0)
                    {
                        isExists = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isExists;
        }
        public bool IsExistsCitizen(string citizen_id)
        {
            bool isExists = true;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    int cnt = db.ExecuteScalarFromCommandText<int>(string.Format("SELECT COUNT(1) AS CNT FROM T_CUSTOMER WHERE citizen_id = N'{0}' AND ISNULL(DEL_FLAG, 'N') = 'N'", WebUtility.GetSQLTextValue(citizen_id)));
                    if (cnt == 0)
                    {
                        isExists = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isExists;
        }
        public string GetLastMemberId(string id)
        {
            string value = "";
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT ISNULL(MEMBERID, '') AS MEMBERID FROM T_CUSTOMER WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    value = db.ExecuteScalarFromCommandText<string>(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public string GetLastMobile(string id)
        {
            string value = "";
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT ISNULL(MOBILE, '') AS MOBILE FROM T_CUSTOMER_CAR_OWNER WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    value = db.ExecuteScalarFromCommandText<string>(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public string GetLastMobileApp(string id)
        {
            string value = "";
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT ISNULL(MOBILE, '') AS MOBILE FROM T_CUSTOMER WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    value = db.ExecuteScalarFromCommandText<string>(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public string GetLastCitizenApp(string id)
        {
            string value = "";
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

SELECT ISNULL(citizen_id, '') AS citizen_id FROM T_CUSTOMER WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    value = db.ExecuteScalarFromCommandText<string>(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public DataTable GetMembers()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"SELECT MEMBERID FROM T_CUSTOMER WHERE ISNULL(DEL_FLAG, 'N') <> 'Y'";
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetCarByMemberId(string memberId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @MEMBERID NVARCHAR(50) = N'{0}'

SELECT		CAR.[CUS_ID]
			,CAR.[MEMBERID]
			--,CASE WHEN [DEALER] = '1' THEN 'LEXUS BANGKOK' WHEN [DEALER] = '2' THEN 'LEXUS SUKHUMVIT' WHEN [DEALER] = '3' THEN 'LEXUS RAMINTRA' END AS [DEALER]
			,CAR.[DEALER]
			,CAR.[MODEL_ID]
			,MD.MODEL
			,CAR.[PLATE_NO]
			,CAR.[VIN]
			,CAR.[CREATE_DT]
			,CAR.[CREATE_USER]
			,CAR.[UPDATE_DT]
			,CAR.[UPDATE_USER]
			,CLR.BODYCLR_NAME
            ,CASE CAR.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
            ,CAR.[CUS_TYPE]
            ,CAR.[OWNERSHIP_TYPE]
            ,CAR.[RS_Date]
            ,ISNULL(CAR.SELLERID, '') AS [SELLERID]
FROM		[T_CUSTOMER_CAR] CAR
LEFT JOIN	[T_CAR_MODEL] MD ON CAR.MODEL_ID = MD.MODEL_ID
LEFT JOIN	[T_CAR_MODEL_CLR] CLR ON CLR.BODYCLR_CD = CAR.BODYCLR_CD AND CLR.MODEL_ID = MD.MODEL_ID
WHERE		ISNULL(CAR.DEL_FLAG, 'N') <> 'Y' AND CAR.[MEMBERID] = @MEMBERID";


                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(memberId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public void AddCar(string memberId, string dealer, string modelId, string plateNo, string vin, string user, string colorId, string custType, string ownerShip, string rsDate, string sellerId)
        {
            DateTime rs_date = DateTime.ParseExact(rsDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string date = rs_date.ToString("yyyy-MM-dd");
            //DateTime rs_date = Convert.ToDateTime(rsDate).ToString("yyyy-MM-dd");//DateTime.ParseExact(rsDate, "yyyy-MM-dd", culture);
            //DateTime rs_date = DateTime.ParseExact(rsDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            try
            {
                string cmd = @"
DECLARE @MEMBERID NVARCHAR(50) = N'{0}'
DECLARE @DEALER INT = N'{1}'
DECLARE @MODELID NVARCHAR(250) = N'{2}'
DECLARE @PLATENO NVARCHAR(250) = N'{3}'
DECLARE @VIN NVARCHAR(250) = N'{4}'
DECLARE @USER NVARCHAR(50) = N'{5}'
DECLARE @RS_Date NVARCHAR(20) = N'{6}'


DECLARE @RS_Date_max NVARCHAR(20)
DECLARE @privilege_cnt INT
DECLARE @Expiry_Date DATE
DECLARE @rs_year INT
DECLARE @rs_month INT
DECLARE @rs_day INT
DECLARE @regis_date DATE
SET @RS_Date_max = (SELECT (CASE WHEN MAX(RS_Date) > @RS_Date THEN MAX(RS_Date) ELSE @RS_Date END) FROM T_CUSTOMER_CAR WHERE MEMBERID = @MEMBERID)
SET @rs_year = (SELECT YEAR(@RS_Date_max))
SET @rs_month = (SELECT MONTH(@RS_Date_max))
SET @rs_day = (SELECT DAY(@RS_Date_max))
SET @regis_date = (select cast(cast(YEAR(GETDATE())*10000 + @rs_month*100 + @rs_day as varchar(255)) as date))
SET @Expiry_Date = (SELECT 
		CASE WHEN CONVERT(DATE,GETDATE()) < CONVERT(DATE,DATEADD(YEAR, 4, @RS_Date_max))
		THEN DATEADD(YEAR, 4, @RS_Date_max) 
		ELSE 
            /*(SELECT EXPIRY FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID)*/

			CASE WHEN CONVERT(DATE,GETDATE()) < CONVERT(DATE,DATEADD(YEAR, 10, @RS_Date_max))
			THEN 	CASE WHEN CONVERT(DATE,GETDATE()) < @regis_date
						THEN @regis_date 
						ELSE DATEADD(YEAR, 1, @regis_date) 
						END
			ELSE DATEADD(YEAR, 10, @RS_Date_max) 
			END 
		END)
SET @privilege_cnt = (SELECT CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 4, @RS_Date) THEN 4 ELSE 0 END)

INSERT INTO T_CUSTOMER_CAR ([MEMBERID],[DEALER],[MODEL_ID],[PLATE_NO],[VIN],[CREATE_DT],[CREATE_USER],[RS_Date])
VALUES (@MEMBERID,@DEALER,@MODELID,@PLATENO,@VIN,DATEADD(HOUR, 7, GETDATE()),@USER, @RS_Date)

UPDATE	T_CUSTOMER
SET		PRIVILEGE_CNT = PRIVILEGE_CNT + @privilege_cnt, EXPIRY = @Expiry_Date
WHERE	MEMBERID = @MEMBERID

UPDATE	T_CUSTOMER_CAR_OWNER
SET		PRIVILEGE_CNT = PRIVILEGE_CNT + @privilege_cnt, EXPIRY = @Expiry_Date
WHERE	MEMBERID = @MEMBERID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(memberId),
                        WebUtility.GetSQLTextValue(dealer),
                        WebUtility.GetSQLTextValue(modelId),
                        WebUtility.GetSQLTextValue(plateNo),
                        WebUtility.GetSQLTextValue(vin),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(date));

                    //db.ParamterList.Add(new SqlParameter("@MEMBERID", memberId));
                    //db.ParamterList.Add(new SqlParameter("@DEALER", dealer));
                    //db.ParamterList.Add(new SqlParameter("@MODELID", modelId));
                    //db.ParamterList.Add(new SqlParameter("@PLATENO", plateNo));
                    //db.ParamterList.Add(new SqlParameter("@VIN", vin));
                    //db.ParamterList.Add(new SqlParameter("@USER", user));
                    //db.ParamterList.Add(new SqlParameter("@RS_Date", date));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EditCar(string memberId, string dealer, string modelId, string plateNo, string vin, string user, 
            string colorId, string carId, string remark, string custType, string ownerShip, string rsDate, string sellerId)
        {
            try
            {
                string cmd = @"
DECLARE @MEMBERID NVARCHAR(50) = N'{0}'
DECLARE @DEALER NVARCHAR(250) = N'{1}'
DECLARE @MODELID NVARCHAR(250) = N'{2}'
DECLARE @PLATENO NVARCHAR(250) = N'{3}'
DECLARE @VIN NVARCHAR(250) = N'{4}'
DECLARE @USER NVARCHAR(50) = N'{5}'
DECLARE @BODYCLR_CD NVARCHAR(10) = N'{6}'
DECLARE @REMARK NVARCHAR(MAX) = N'{7}'
DECLARE @CUS_ID INT = N'{8}'
DECLARE @CUS_TYPE NVARCHAR(50) = N'{9}'
DECLARE @OWNERSHIP_TYPE NVARCHAR(50) = N'{10}'
DECLARE @RS_Date NVARCHAR(20) = N'{11}'
DECLARE @SELLERID NVARCHAR(100) = N'{12}'

UPDATE	T_CUSTOMER_CAR
SET		MEMBERID = @MEMBERID,
		DEALER = @DEALER,
		MODEL_ID = @MODELID,
		PLATE_NO = @PLATENO,
		VIN = @VIN,
		UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
		UPDATE_USER = @USER,
		BODYCLR_CD = @BODYCLR_CD,
        REMARK = @REMARK,
        --REMARK = ISNULL(CONVERT(NVARCHAR(MAX), REMARK), '') + '|[' + CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) + '] ' + @REMARK,
        [CUS_TYPE] = @CUS_TYPE,
        [OWNERSHIP_TYPE] = @OWNERSHIP_TYPE,
        [RS_Date] = CASE LEN(@RS_Date) WHEN 0 THEN NULL ELSE CONVERT(DATE, @RS_Date, 103) END,
        SELLERID = CASE LEN(@SELLERID) WHEN 0 THEN NULL ELSE @SELLERID END
WHERE	CUS_ID = @CUS_ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(memberId),
                        WebUtility.GetSQLTextValue(dealer),
                        WebUtility.GetSQLTextValue(modelId),
                        WebUtility.GetSQLTextValue(plateNo),
                        WebUtility.GetSQLTextValue(vin),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(colorId),
                        WebUtility.GetSQLTextValue(remark),
                        WebUtility.GetSQLTextValue(carId),
                        WebUtility.GetSQLTextValue(custType),
                        WebUtility.GetSQLTextValue(ownerShip),
                        WebUtility.GetSQLTextValue(rsDate),
                        WebUtility.GetSQLTextValue(sellerId));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetCarModels()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @TABLE TABLE ([MODEL_ID] INT,[MODEL] NVARCHAR(255),[IMAGE] NVARCHAR(255))

--INSERT INTO @TABLE VALUES (0, '', '')
INSERT INTO @TABLE
SELECT [MODEL_ID],[MODEL],[IMAGE] FROM [T_CAR_MODEL]

SELECT * FROM @TABLE";

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetCarColor(string modelId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @MODELID INT = N'{0}'
DECLARE @TABLE TABLE ([MODEL_ID] INT,[BODYCLR_CD] NVARCHAR(10),[BODYCLR_NAME] NVARCHAR(150))

--INSERT INTO @TABLE VALUES (0, 0, '')
INSERT INTO @TABLE
SELECT		[MODEL_ID]
			,[BODYCLR_CD]
			,[BODYCLR_NAME]
FROM		[T_CAR_MODEL_CLR]
WHERE	    [MODEL_ID] = @MODELID

SELECT * FROM @TABLE";

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(modelId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetProvinces()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT		[PROVINCE_ID]
			,[PROVINCE_CODE]
			,[PROVINCE_NAME_ENG]
            ,[PROVINCE_NAME_TH]
			,[GEO_ID]
FROM		[T_PROVINCE]";

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetDistrict(string province_id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @PROVINCE_ID  INT = N'{0}'

SELECT		[DISTRICT_ID]
			,[DISTRICT_CODE]
			,[DISTRICT_NAME_ENG]
            ,[DISTRICT_NAME_TH]
			,[GEO_ID]
            ,[PROVINCE_ID]
FROM		[T_DISTRICT]
WHERE [PROVINCE_ID] = @PROVINCE_ID";

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(province_id));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetSubDistrict(string province_id, string district_id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @PROVINCE_ID  INT = N'{0}'
DECLARE @DISTRICT_ID  INT = N'{1}'

SELECT		[SUBDISTRICT_ID]
			,[SUBDISTRICT_CODE]
			,[SUBDISTRICT_NAME_ENG]
            ,[SUBDISTRICT_NAME_TH]
			,[GEO_ID]
            ,[PROVINCE_ID]
            ,[DISTRICT_ID]
FROM		[T_SUBDISTRICT]
WHERE [PROVINCE_ID] = @PROVINCE_ID AND [DISTRICT_ID] = @DISTRICT_ID";

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(province_id),
                        WebUtility.GetSQLTextValue(district_id));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetCarsByMemberList(List<string> memberIdList)
        {
            DataTable dt = new DataTable();
            try
            {
                if (memberIdList.Count > 0)
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = @"

SELECT		CAR.[CUS_ID]
			,CAR.[MEMBERID]
			,CAR.[DEALER]
            ,D.[BRANCH_NAME]
			,CAR.[MODEL_ID]
			,MD.MODEL
			,CAR.[PLATE_NO]
			,CAR.[VIN]
			,CAR.[CREATE_DT]
			,CAR.[CREATE_USER]
			,CAR.[UPDATE_DT]
			,CAR.[UPDATE_USER]
			,CLR.BODYCLR_NAME
			,CLR.BODYCLR_CD
			,CASE CAR.[INACTIVE_FLAG] WHEN 'Y' THEN 'Yes' ELSE 'No' END AS [INACTIVE]
            ,ISNULL(CAR.[INACTIVE_FLAG], 'N') AS [INACTIVE_FLAG]
            ,ISNULL(RSN.[INACTIVE_REASON_ID], 0) AS [INACTIVE_REASON_ID]
			,RSN.[INACTIVE_REASON]
            ,CASE CAR.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
            ,CAR.[DEL_FLAG]
            ,CAR.[CUS_TYPE]
            ,CAR.[OWNERSHIP_TYPE]
            ,CAR.[RS_Date]
            ,ISNULL(CAR.SELLERID, '') AS [SELLERID]
FROM		[T_CUSTOMER_CAR] CAR
LEFT JOIN	[T_DEALER] D ON CAR.DEALER = D.DEALER_ID
LEFT JOIN	[T_CAR_MODEL] MD ON CAR.MODEL_ID = MD.MODEL_ID
LEFT JOIN	[T_CAR_MODEL_CLR] CLR ON CLR.BODYCLR_CD = CAR.BODYCLR_CD AND CLR.MODEL_ID = MD.MODEL_ID
LEFT JOIN	[T_INACTIVE_RSN] RSN ON RSN.[INACTIVE_REASON_ID] = CAR.[INACTIVE_REASON_ID]
WHERE		[MEMBERID] IN ({0})";

                        string _memberIdList = "";
                        foreach (string s in memberIdList)
                        {
                            _memberIdList += "," + "'" + s + "'";
                        }
                        _memberIdList = _memberIdList.Substring(1);

                        cmd = string.Format(cmd, _memberIdList);

                        dt = db.GetDataTableFromCommandText(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetCarReasons()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @TABLE TABLE (INACTIVE_REASON_ID INT, INACTIVE_REASON NVARCHAR(200))
INSERT INTO @TABLE VALUES (0, '')
INSERT INTO @TABLE
SELECT * FROM T_INACTIVE_RSN

SELECT * FROM @TABLE ORDER BY INACTIVE_REASON_ID";
                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetCarById(string carId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @CUS_ID INT = N'{0}'
SELECT		CAR.[CUS_ID]
			,[MEMBERID]
			,CAR.[DEALER]
			,CAR.[MODEL_ID]
			,MD.MODEL
			,CAR.[PLATE_NO]
			,CAR.[VIN]
			,CAR.[CREATE_DT]
			,CAR.[CREATE_USER]
			,CAR.[UPDATE_DT]
			,CAR.[UPDATE_USER]
			,CLR.BODYCLR_NAME
			,CLR.BODYCLR_CD
			,CASE CAR.[INACTIVE_FLAG] WHEN 'Y' THEN 'Yes' ELSE 'No' END AS [INACTIVE]
            ,ISNULL(CAR.[INACTIVE_FLAG], 'N') AS [INACTIVE_FLAG]
            ,ISNULL(RSN.[INACTIVE_REASON_ID], 0) AS [INACTIVE_REASON_ID]
			,RSN.[INACTIVE_REASON]
            ,CAR.[CUS_TYPE]
            ,CAR.[OWNERSHIP_TYPE]
            ,CAR.[RS_Date]
            ,CAR.[REMARK]
            ,ISNULL(CAR.SELLERID, '') AS [SELLERID]
            ,dm.branch_code
FROM		[T_CUSTOMER_CAR] CAR
LEFT JOIN	[T_CAR_MODEL] MD ON CAR.MODEL_ID = MD.MODEL_ID
LEFT JOIN	[T_CAR_MODEL_CLR] CLR ON CLR.BODYCLR_CD = CAR.BODYCLR_CD AND CLR.MODEL_ID = MD.MODEL_ID
LEFT JOIN	[T_INACTIVE_RSN] RSN ON RSN.[INACTIVE_REASON_ID] = CAR.[INACTIVE_REASON_ID]
LEFT JOIN   T_DEALER_MASTER dm ON CAR.DEALER = dm.id
WHERE		CAR.[CUS_ID] = @CUS_ID";

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(carId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public void DeleteCar(string id, string remark, string user)
        {
            try
            {
                //string cmd = @"DELETE FROM T_CUSTOMER_CAR WHERE CUS_ID = @CUS_ID";
                string cmd = @"
DECLARE @REMARK NVARCHAR(MAX) = N'{0}'
DECLARE @CUS_ID INT = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'

UPDATE T_CUSTOMER_CAR 
SET DEL_FLAG = 'Y', 
    REMARK = @REMARK,
    DEL_DT = DATEADD(HOUR, 7, GETDATE()),
    DEL_USER = @USER
WHERE CUS_ID = @CUS_ID

DECLARE @MEMBERID NVARCHAR(10) = (SELECT MEMBERID FROM T_CUSTOMER_CAR WHERE CUS_ID = @CUS_ID)
DECLARE @PRIVILEGE_CNT INT = (SELECT ISNULL(PRIVILEGE_CNT, 0) FROM T_CUSTOMER WHERE MEMBERID = @MEMBERID) - 0
IF @PRIVILEGE_CNT < 0
BEGIN
    SET @PRIVILEGE_CNT = 0
END

UPDATE	T_CUSTOMER
SET		PRIVILEGE_CNT = @PRIVILEGE_CNT
WHERE	MEMBERID = @MEMBERID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(remark),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsDuplicateActiveCar(string memberId)
        {
            bool isDup = true;
            try
            {
                string cmd = @"
DECLARE @MEMBERID NVARCHAR(50) = N'{0}'

SELECT COUNT(1) AS CNT FROM [T_CUSTOMER_CAR] WHERE MEMBERID = @MEMBERID AND ISNULL([INACTIVE_FLAG], 'N') = 'N'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(memberId));

                    if (db.ExecuteScalarFromCommandText<int>(cmd) > 0)
                    {
                        isDup = true;
                    }
                    else
                    {
                        isDup = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDup;
        }
        public bool IsDuplicateActiveVIN(string vin)
        {
            bool isDup = true;
            try
            {
                string cmd = @"
DECLARE @VIN NVARCHAR(255) = N'{0}'

SELECT COUNT(1) AS CNT FROM [T_CUSTOMER_CAR] WHERE VIN = @VIN AND ISNULL([INACTIVE_FLAG], 'N') = 'N'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(vin));

                    if (db.ExecuteScalarFromCommandText<int>(cmd) > 0)
                    {
                        isDup = true;
                    }
                    else
                    {
                        isDup = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDup;
        }
        public bool IsDuplicateActiveVIN2(string vin)
        {
            bool isDup = true;
            try
            {
                string cmd = @"
DECLARE @VIN NVARCHAR(255) = N'{0}'

SELECT COUNT(1) AS CNT FROM [T_CUSTOMER_CAR] WHERE VIN = @VIN AND ISNULL([DEL_FLAG], 'N') = 'N'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(vin));

                    if (db.ExecuteScalarFromCommandText<int>(cmd) > 0)
                    {
                        isDup = true;
                    }
                    else
                    {
                        isDup = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDup;
        }

        public DataTable GetSeller(string dealer)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";

                    if (string.IsNullOrEmpty(dealer))
                    {
                        cmd = @"
DECLARE @TABLE TABLE (SELLERID NVARCHAR(100))
INSERT INTO @TABLE VALUES ('')
INSERT INTO @TABLE
SELECT DISTINCT ISNULL(SELLERID, '') AS SELLERID FROM T_USER WHERE SELLERID IS NOT NULL

SELECT DISTINCT * FROM @TABLE";
                    }
                    else
                    {
                        cmd = @"
DECLARE @DEALER NVARCHAR(150) = N'{0}'

DECLARE @TABLE TABLE (SELLERID NVARCHAR(100))
INSERT INTO @TABLE VALUES ('')
INSERT INTO @TABLE
SELECT DISTINCT ISNULL(SELLERID, '') AS SELLERID FROM T_USER WHERE SELLERID IS NOT NULL
AND DEALER = @DEALER

SELECT * FROM @TABLE";
                        cmd = string.Format(cmd, WebUtility.GetSQLTextValue(dealer));
                    }

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetPrivilegesActive()
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
SELECT [ID] AS [PRIVILEGE_ID]
      ,[TITLE] AS [PRIVILEGE_NAME]
FROM  [T_PRIVILEDGES]
WHERE DATEADD(HOUR, 7, GETDATE()) BETWEEN [PERIOD_START] AND [PERIOD_END] AND DEL_FLAG IS NULL";

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
        public void ExtendCustomer(string id)
        {
            try
            {
                string cmd = @"
DECLARE @ID INT = N'{0}'

DECLARE @PRIVILEGE_CNT INT = ISNULL((SELECT PRIVILEGE_CNT FROM T_CUSTOMER WHERE ID = @ID), 0)
DECLARE @ACTIVE_CAR INT = (SELECT COUNT(1) FROM T_CUSTOMER_CAR WHERE INACTIVE_FLAG = 'N' AND MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER WHERE ID = @ID))

UPDATE T_CUSTOMER SET EXPIRY = DATEADD(YEAR, 1, EXPIRY) WHERE ID = @ID
UPDATE T_CUSTOMER SET PRIVILEGE_CNT = @PRIVILEGE_CNT + (@ACTIVE_CAR * 2) WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AdjustCustomer(string id, string point)
        {
            try
            {
                string cmd = @"
DECLARE @ID NVARCHAR(10) = N'{0}'
DECLARE @POINT INT = N'{1}'

UPDATE T_CUSTOMER SET PRIVILEGE_CNT = @POINT WHERE MEMBERID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(point));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetUserToken(string memberId)
        {
            string token = "";
            try
            {
                string cmd = @"
DECLARE @MEMBERID NVARCHAR(50) = N'{0}'
SELECT TOP 1 ISNULL([TOKEN_NO], '') AS TOKEN FROM T_CUSTOMER_TOKEN WHERE MEMBERID = @MEMBERID AND CONVERT(NVARCHAR(10), TOKEN_EXPIREY, 120) = '2199-01-01'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(memberId));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            token = dt.Rows[0]["TOKEN"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return token;
        }
        public string GetNotifyToken(string memberId)
        {
            string token = "";
            try
            {
                string cmd = @"
DECLARE @MEMBERID NVARCHAR(50) = N'{0}'
SELECT TOP 1 ISNULL(NOTIF_TOKEN, '') AS TOKEN FROM T_CUSTOMER_TOKEN WHERE MEMBERID = @MEMBERID AND CONVERT(NVARCHAR(10), TOKEN_EXPIREY, 120) = '2199-01-01'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(memberId));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            token = dt.Rows[0]["TOKEN"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return token;
        }

        public string AddCustomerApp(string firstName, string lastName, string nickName, string gender, 
            string age, string email, string ssn, string mobile, string memberId, string user, string title, 
            string birthdate, string addr1, string addr2, string subDistrict, string district, string province, 
            string postcode, string homeNumber, string sellerId)
        {
            //string receivedata = "firstName: " + firstName + " ,lastName: " + lastName + " ,nickName: " + nickName +
            //        " ,gender: " + gender + " ,age: " + age + " ,email: " + email + " ,ssn: " + ssn +
            //        " ,mobile: " + mobile + " ,memberId: " + memberId + " ,user: " + user + " ,title: " + title +
            //        " ,birthdate: " + birthdate + " ,addr1: " + addr1 + " ,addr2: " + addr2 + " ,subDistrict: " + subDistrict +
            //        " ,district: " + district + " ,province: " + province + " ,postcode: " + postcode +
            //        " ,homeNumber: " + homeNumber + " ,sellerId: " + sellerId;
            //LogService _log = new LogService();
            //_log.InsertLogReceiveData("AddCustomerApp", receivedata);

            string id = "";
            try
            {
                string cmd = @"
DECLARE @FNAME NVARCHAR(250) = N'{0}'
DECLARE @LNAME NVARCHAR(250) = N'{1}'
DECLARE @GENDER NVARCHAR(100) = N'{2}'
DECLARE @EMAIL NVARCHAR(100) = N'{3}'
DECLARE @SSN NVARCHAR(50) = N'{4}'
DECLARE @MOBILE NVARCHAR(50) = N'{5}'
DECLARE @MEMBERID NVARCHAR(50) = N'{6}'
DECLARE @USER NVARCHAR(50) = N'{7}'
DECLARE @TITLENAME NVARCHAR(50) = N'{8}'
DECLARE @BIRTHDATE NVARCHAR(20) = N'{9}'
DECLARE @ADDRESS1 NVARCHAR(300) = N'{10}'
DECLARE @ADDRESS2 NVARCHAR(300) = N'{11}'
DECLARE @SUBDISTRICT NVARCHAR(100) = N'{12}'
DECLARE @DISTRICT NVARCHAR(100) = N'{13}'
DECLARE @PROVINCE NVARCHAR(100) = N'{14}'
DECLARE @POSTALCODE NVARCHAR(10) = N'{15}'
DECLARE @HOME_NO NVARCHAR(30) = N'{16}'
DECLARE @SELLERID NVARCHAR(100) = N'{17}'

DECLARE @RS_Date NVARCHAR(100) = ''
DECLARE @Expiry_Date DATE
DECLARE @rs_year INT
DECLARE @rs_month INT
DECLARE @rs_day INT
DECLARE @regis_date DATE
SET @RS_Date = (SELECT rs_date FROM initial_data WHERE vin = @SSN)
SET @rs_year = (SELECT YEAR(@RS_Date))
SET @rs_month = (SELECT MONTH(@RS_Date))
SET @rs_day = (SELECT DAY(@RS_Date))
SET @regis_date = (select cast(cast(YEAR(GETDATE())*10000 + @rs_month*100 + @rs_day as varchar(255)) as date))
SET @Expiry_Date = (SELECT CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 4, @RS_Date) 
			         THEN DATEADD(YEAR, 4, @RS_Date) 
			         ELSE CASE WHEN CONVERT(DATE,GETDATE()) < DATEADD(YEAR, 10, @RS_Date) 
				            THEN  CASE WHEN CONVERT(DATE,GETDATE()) < CONVERT(DATE,@regis_date)
									        THEN @regis_date 
									        ELSE DATEADD(YEAR, 1, @regis_date) 
									        END 
						        ELSE DATEADD(YEAR, 10, @RS_Date) 
						        END 
			         END)

INSERT INTO T_CUSTOMER (MEMBERID, FNAME, LNAME, GENDER, AGE, EMAIL, SSN, MOBILE, CREATE_DT, CREATE_USER, TITLENAME, BIRTHDATE, ADDRESS1, ADDRESS2, 
SUBDISTRICT, DISTRICT, PROVINCE, POSTALCODE, HOME_NO, PRIVILEGE_CNT, EXPIRY, NICKNAME, SELLERID, citizen_id, register_type, is_update)
VALUES (
    CASE LEN(@MEMBERID) WHEN 0 THEN NULL ELSE @MEMBERID END,
	CASE LEN(@FNAME) WHEN 0 THEN NULL ELSE @FNAME END,
	CASE LEN(@LNAME) WHEN 0 THEN NULL ELSE @LNAME END,
	CASE LEN(@GENDER) WHEN 0 THEN NULL ELSE @GENDER END,
	CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE DATEDIFF(YEAR, CONVERT(DATE, @BIRTHDATE, 103), GETDATE()) END,
	CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
	CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END,
	CASE LEN(@MOBILE) WHEN 0 THEN NULL ELSE @MOBILE END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER,
	CASE LEN(@TITLENAME) WHEN 0 THEN NULL ELSE @TITLENAME END,
	CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 103) END,
	CASE LEN(@ADDRESS1) WHEN 0 THEN NULL ELSE @ADDRESS1 END,
	CASE LEN(@ADDRESS2) WHEN 0 THEN NULL ELSE @ADDRESS2 END,
	CASE LEN(@SUBDISTRICT) WHEN 0 THEN NULL ELSE @SUBDISTRICT END,
	CASE LEN(@DISTRICT) WHEN 0 THEN NULL ELSE @DISTRICT END,
	CASE LEN(@PROVINCE) WHEN 0 THEN NULL ELSE @PROVINCE END,
	CASE LEN(@POSTALCODE) WHEN 0 THEN NULL ELSE @POSTALCODE END,
	CASE LEN(@HOME_NO) WHEN 0 THEN NULL ELSE @HOME_NO END,
	0,
	@Expiry_Date,
	CASE LEN(@FNAME) + LEN(@LNAME) WHEN 0 THEN NULL ELSE @FNAME + ' ' + @LNAME END,
    CASE LEN(@SELLERID) WHEN 0 THEN NULL ELSE @SELLERID END,
    CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END,
    'APP_USER',
    1)

SELECT CONVERT(NVARCHAR, @@IDENTITY) AS [ID]";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(firstName),
                        WebUtility.GetSQLTextValue(lastName),
                        WebUtility.GetSQLTextValue(gender),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(ssn),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(memberId),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(birthdate),
                        WebUtility.GetSQLTextValue(addr1),
                        WebUtility.GetSQLTextValue(addr2),
                        WebUtility.GetSQLTextValue(subDistrict),
                        WebUtility.GetSQLTextValue(district),
                        WebUtility.GetSQLTextValue(province),
                        WebUtility.GetSQLTextValue(postcode),
                        WebUtility.GetSQLTextValue(homeNumber),
                        WebUtility.GetSQLTextValue(sellerId));

                    id = db.ExecuteScalarFromCommandText<string>(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }
        public void UpdateCustomerApp(string firstName, string lastName, string nickName, string gender, string age, 
            string email, string ssn, string mobile, string id, string memberId, string user, string title, 
            string birthdate, string addr1, string addr2, string subDistrict, string district, string province, 
            string postcode, string homeNumber, string remark, string sellerId)
        {
            try
            {
                string cmd = @"
DECLARE @FNAME NVARCHAR(250) = N'{0}'
DECLARE @LNAME NVARCHAR(250) = N'{1}'
DECLARE @GENDER NVARCHAR(100) = N'{2}'
DECLARE @EMAIL NVARCHAR(100) = N'{3}'
DECLARE @SSN NVARCHAR(50) = N'{4}'
DECLARE @MOBILE NVARCHAR(50) = N'{5}'
DECLARE @MEMBERID NVARCHAR(50) = N'{6}'
DECLARE @USER NVARCHAR(50) = N'{7}'
DECLARE @TITLENAME NVARCHAR(50) = N'{8}'
DECLARE @BIRTHDATE NVARCHAR(20) = N'{9}'
DECLARE @ADDRESS1 NVARCHAR(300) = N'{10}'
DECLARE @ADDRESS2 NVARCHAR(300) = N'{11}'
DECLARE @SUBDISTRICT NVARCHAR(100) = N'{12}'
DECLARE @DISTRICT NVARCHAR(100) = N'{13}'
DECLARE @PROVINCE NVARCHAR(100) = N'{14}'
DECLARE @POSTALCODE NVARCHAR(10) = N'{15}'
DECLARE @HOME_NO NVARCHAR(30) = N'{16}'
DECLARE @REMARK NVARCHAR(MAX) = N'{17}'
DECLARE @ID INT = N'{18}'
DECLARE @SELLERID NVARCHAR(100) = N'{19}'

UPDATE T_CUSTOMER_CAR SET MEMBERID = @MEMBERID WHERE MEMBERID = (SELECT MEMBERID FROM T_CUSTOMER WHERE ID = @ID)

UPDATE T_CUSTOMER
SET FNAME = CASE LEN(@FNAME) WHEN 0 THEN NULL ELSE @FNAME END,
	LNAME = CASE LEN(@LNAME) WHEN 0 THEN NULL ELSE @LNAME END,
	GENDER = CASE LEN(@GENDER) WHEN 0 THEN NULL ELSE @GENDER END,
	EMAIL = CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
	SSN = CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END,
	MOBILE = CASE LEN(@MOBILE) WHEN 0 THEN NULL ELSE @MOBILE END,
    MEMBERID = CASE LEN(@MEMBERID) WHEN 0 THEN NULL ELSE @MEMBERID END,
    UPDATE_DT = DATEADD(HOUR, 7, GETDATE()),
    UPDATE_USER = @USER,
	AGE = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE DATEDIFF(YEAR, CONVERT(DATE, @BIRTHDATE, 103), GETDATE()) END,
	TITLENAME = CASE LEN(@TITLENAME) WHEN 0 THEN NULL ELSE @TITLENAME END,
	BIRTHDATE = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 103) END,
	ADDRESS1 = CASE LEN(@ADDRESS1) WHEN 0 THEN NULL ELSE @ADDRESS1 END,
	ADDRESS2 = CASE LEN(@ADDRESS2) WHEN 0 THEN NULL ELSE @ADDRESS2 END,
	SUBDISTRICT = CASE LEN(@SUBDISTRICT) WHEN 0 THEN NULL ELSE @SUBDISTRICT END,
	DISTRICT = CASE LEN(@DISTRICT) WHEN 0 THEN NULL ELSE @DISTRICT END,
	PROVINCE = CASE LEN(@PROVINCE) WHEN 0 THEN NULL ELSE @PROVINCE END,
	POSTALCODE = CASE LEN(@POSTALCODE) WHEN 0 THEN NULL ELSE @POSTALCODE END,
	HOME_NO = CASE LEN(@HOME_NO) WHEN 0 THEN NULL ELSE @HOME_NO END, 
    REMARK = ISNULL(CONVERT(NVARCHAR(MAX), REMARK), '') + '|[' + CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) + '] ' + @REMARK,
    SELLERID = CASE LEN(@SELLERID) WHEN 0 THEN NULL ELSE @SELLERID END,
    citizen_id = CASE LEN(@SSN) WHEN 0 THEN NULL ELSE @SSN END
WHERE MEMBERID = @MEMBERID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(firstName),
                        WebUtility.GetSQLTextValue(lastName),
                        WebUtility.GetSQLTextValue(gender),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(ssn),
                        WebUtility.GetSQLTextValue(mobile),
                        WebUtility.GetSQLTextValue(memberId),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(title),
                        WebUtility.GetSQLTextValue(birthdate),
                        WebUtility.GetSQLTextValue(addr1),
                        WebUtility.GetSQLTextValue(addr2),
                        WebUtility.GetSQLTextValue(subDistrict),
                        WebUtility.GetSQLTextValue(district),
                        WebUtility.GetSQLTextValue(province),
                        WebUtility.GetSQLTextValue(postcode),
                        WebUtility.GetSQLTextValue(homeNumber),
                        WebUtility.GetSQLTextValue(remark),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(sellerId));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteCustomerApp(string id, string remark, string user)
        {
            try
            {
                //string cmd = @"DELETE FROM T_CUSTOMER WHERE ID = @ID";
                string cmd = @"
DECLARE @REMARK NVARCHAR(MAX) = N'{0}'
DECLARE @ID INT = N'{1}'
DECLARE @USER NVARCHAR(50) = N'{2}'

UPDATE T_CUSTOMER 
SET DEL_FLAG = 'Y', [EXPIRY] = DATEADD(HOUR, 7, GETDATE()), 
    REMARK = ISNULL(CONVERT(NVARCHAR(MAX), REMARK), '') + '|[' + CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) + '] ' + @REMARK,
    DEL_DT = DATEADD(HOUR, 7, GETDATE()),
    DEL_USER = @USER
WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(remark),
                        WebUtility.GetSQLTextValue(id),
                        WebUtility.GetSQLTextValue(user));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAppByMemberList(List<string> memberIdList)
        {
            DataTable dt = new DataTable();
            try
            {
                if (memberIdList.Count > 0)
                {
                    using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                    {
                        string cmd = @"
DECLARE @MEMBERIDLIST NVARCHAR(MAX) = N'{0}'

SELECT		DISTINCT CUS.[ID]
			,ISNULL(CUS.[MEMBERID], '') AS [MEMBERID]
			,CUS.[FNAME]
			,CUS.[LNAME]
			,CUS.[GENDER]
			,CUS.[AGE]
			,CUS.[EMAIL]
			,CUS.[SSN]
			,CUS.[MOBILE]
			,CUS.[PRIVILEGE_CNT]
			,CUS.[EXPIRY]
			,DATEADD(HOUR, 7, CUS.[CREATE_DT]) AS [CREATE_DT]
			,CUS.[CREATE_USER]
			,DATEADD(HOUR, 7, CUS.[UPDATE_DT]) AS [UPDATE_DT]
			,CUS.[UPDATE_USER]
			,CUS.[NICKNAME]
			,CUS.[PROFILE_IMAGE]
			,CUS.[BIRTHDATE]
			,CUS.[ADDRESS1]
			,CUS.[ADDRESS2]
			,CUS.[SUBDISTRICT]
			,CUS.[DISTRICT]
			,CUS.[PROVINCE]
			,CUS.[POSTALCODE]
			,CUS.[HOME_NO]
			,CUS.[TITLENAME]
            ,CUS.[DEL_FLAG]
            ,CASE CUS.[DEL_FLAG] WHEN 'Y' THEN 'Inactive' ELSE 'Active' END AS [DEL_FLAG_DISP]
            ,ISNULL(CUS.SELLERID, '') AS [SELLERID]
FROM		T_CUSTOMER CUS
LEFT JOIN	T_CUSTOMER_CAR CAR ON CAR.MEMBERID = CUS.MEMBERID
WHERE		[MEMBERID] IN (SELECT DATA FROM [dbo].[Split](@MEMBERIDLIST, ','))
ORDER BY CUS.ID";

                        string _memberIdList = "";
                        foreach (string s in memberIdList)
                        {
                            _memberIdList += "," + s;
                        }
                        _memberIdList = _memberIdList.Substring(1);

                        cmd = string.Format(cmd,
                            WebUtility.GetSQLTextValue(_memberIdList));

                        dt = db.GetDataTableFromCommandText(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public DataTable GetPrivilegesRedeemCode(string PrivilegeId)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmd = @"
DECLARE @PRIVILEGEID INT = N'{0}'

SELECT [NO] AS [PRIVILEGE_CODE_ID], REDEEM_CODE
FROM  [T_PRIVILEDGES_CODE]
WHERE PRIVILEGE_ID = @PRIVILEGEID AND [STATUS] = 'Y'";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(PrivilegeId));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public void RedeemPrivilegeCode(string privilege_code_id, string member_id, string privilege_id)
        {
            try
            {
                string cmd = @"
DECLARE @NO INT = N'{0}'
DECLARE @MEMBERID NVARCHAR(50) = N'{1}'
DECLARE @PRIVILEGE_ID INT = N'{2}'
DECLARE @REDEEM_CODE NVARCHAR(50)

DECLARE @PRIVILEGE_TYPE INT = (SELECT PRIVILEDGE_TYPE FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID)
DECLARE @EXPIRY_TYPE INT = (SELECT red_expiry_type FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID)
DECLARE @DISPLAY_TYPE INT = (SELECT display_type FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID)
DECLARE @EXPIRY_DATE NVARCHAR(50) = ''

IF @EXPIRY_TYPE = 1 BEGIN
  SET @EXPIRY_DATE = DATEADD(HOUR, 7, DATEADD(MINUTE, ISNULL((SELECT RED_EXPIRY FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID), 0), GETDATE()))
END
ELSE BEGIN
  SET @EXPIRY_DATE = (SELECT convert(datetime, CONVERT(VARCHAR(10), CONVERT(DATE, GETDATE(), 102), 120) + ' ' + (SELECT RED_EXPIRY_TIME FROM T_PRIVILEDGES WHERE ID = @PRIVILEGE_ID), 120))
  SET @EXPIRY_DATE = (CASE WHEN CONVERT(datetime, @EXPIRY_DATE, 103) < GETDATE() THEN DATEADD(DAY, 1, CONVERT(datetime, @EXPIRY_DATE, 103)) ELSE CONVERT(datetime, @EXPIRY_DATE, 103) END)
END

SET @REDEEM_CODE = (SELECT REDEEM_CODE FROM T_PRIVILEDGES_CODE WHERE PRIVILEGE_ID = @PRIVILEGE_ID AND [STATUS] = 'Y' AND [NO] = @NO)

UPDATE T_PRIVILEDGES_CODE SET [STATUS] = 'N' WHERE [STATUS] = 'Y' AND [NO] = @NO

IF @DISPLAY_TYPE = 2 BEGIN
    UPDATE T_CUSTOMER SET PRIVILEGE_CNT = PRIVILEGE_CNT - 1 WHERE MEMBERID = @MEMBERID
END

INSERT INTO T_CUSTOMER_REDEEM (CUS_ID, MEMBERID, PRIVILEGE_ID, [NO], REDEEM_CODE, REDEEM_DT, EXPIRY_DT, SHOP_NM)
    SELECT C.ID, C.MEMBERID, @PRIVILEGE_ID, @NO, @REDEEM_CODE, DATEADD(HOUR, 7, GETDATE()), CONVERT(DATETIME, @EXPIRY_DATE, 103), ''
    FROM T_CUSTOMER_TOKEN T
    INNER JOIN T_CUSTOMER C ON C.MEMBERID = T.MEMBERID
    WHERE T.MEMBERID = @MEMBERID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilege_code_id),
                        WebUtility.GetSQLTextValue(member_id),
                        WebUtility.GetSQLTextValue(privilege_id));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow CheckVinInitial(string vin)
        {
            DataRow row = null;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"

DECLARE @vin NVARCHAR(200) = N'{0}'

SELECT i.vin, i.plate_no, COALESCE(i.dealer,0) dealer, 
(SELECT branch_code FROM T_DEALER_MASTER WHERE id =i.dealer) branch, 
i.rs_date, COALESCE(i.model,0) model
FROM initial_data i
WHERE i.vin = @vin";

                    row = db.GetDataTableFromCommandText(string.Format(cmd, vin)).AsEnumerable().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public DataRow CheckVinInCar(string vin)
        {
            DataRow row = null;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"

DECLARE @vin NVARCHAR(200) = N'{0}'

SELECT *
FROM T_CUSTOMER_CAR 
WHERE VIN = @vin";

                    row = db.GetDataTableFromCommandText(string.Format(cmd, vin)).AsEnumerable().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public DataTable GetDealer()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT id, display_th 
FROM T_DEALER_MASTER
WHERE is_active = 1";

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetBranch(string dealer_code)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT branch_code, branch_th 
FROM T_DEALER_MASTER
WHERE is_active = 1 AND id = N'{0}'";

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(dealer_code));

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetModel()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT MODEL_ID, MODEL 
FROM T_CAR_MODEL
WHERE DEL_FLAG IS NULL";

                    dt = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public string GetPostcode(string sub_district_id)
        {
            string postcode = "";
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT POSTCODE
FROM T_SUBDISTRICT
WHERE SUBDISTRICT_ID = N'{0}'";

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(sub_district_id));

                    dt = db.GetDataTableFromCommandText(cmd);

                    DataRow dr = dt.Rows[0];
                    postcode = dr["POSTCODE"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return postcode;
        }

        public DataRow CheckDeliveryDate(string id)
        {
            DataRow row = null;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT a.RS_Date
FROM
(SELECT MAX(cc.RS_Date) AS RS_Date
FROM T_CUSTOMER c LEFT JOIN T_CUSTOMER_CAR cc ON c.MEMBERID = cc.MEMBERID
WHERE c.ID = N'{0}') a
WHERE	GETDATE() < DATEADD(YEAR, 10, a.RS_Date)";

                    row = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id))).AsEnumerable().FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public string GetDisplayType(string privilegeID)
        {
            string display_type = "";
            try
            {
                string cmd = @"
DECLARE @privilegeID INT = N'{0}'

SELECT display_type
FROM T_PRIVILEDGES 
WHERE ID = @privilegeID ";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(privilegeID));

                    using (DataTable dt = db.GetDataTableFromCommandText(cmd))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            display_type = dt.Rows[0]["display_type"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return display_type;
        }
    }
}