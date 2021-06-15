using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class RptRegisterService
    {
        private string conn;
        public RptRegisterService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetReport(string events, string startDate, string endDate)
        {
            DataTable table = new DataTable();
            try
            {
                string cmd = @"
DECLARE @START NVARCHAR(10) = N'{0}'
DECLARE @END NVARCHAR(10) = N'{1}'

SELECT ROW_NUMBER() OVER(ORDER BY [Member ID]) AS [No.], *
FROM (
    SELECT DISTINCT
    DATEADD(HOUR, 7, A.CREATE_DT) AS [Date/Time],
    A.TITLENAME AS [Title],
    A.FNAME AS [Name],
    A.LNAME AS [Family Name],
    A.MEMBERID AS [Member ID],
    A.MOBILE AS [Tel.],
    A.EMAIL AS [E-mail],
    A.PRIVILEGE_CNT AS [Redeem],
    ROW_NUMBER() OVER(PARTITION BY A.MEMBERID ORDER BY A.MEMBERID) AS [Car No.],
    B.VIN AS [Vin],
    B.PLATE_NO AS [Rigister No.],
	D.MODEL,
	E.BODYCLR_NAME AS [Color],
    B.DEALER AS [Dealer],
    B.RS_DATE AS [R/S Date],
	MAX(C.Login_DT) as LastLogin
    FROM [dbo].[T_CUSTOMER] A
    INNER JOIN [dbo].[T_CUSTOMER_CAR] B
    ON A.MEMBERID = B.MEMBERID
	LEFT join [dbo].[T_CUSTOMER_TOKEN] C 
	on A.memberid = C.memberid
	LEFT JOIN [dbo].[T_CAR_MODEL] D
	ON B.MODEL_ID = D.MODEL_ID
	LEFT JOIN [dbo].[T_CAR_MODEL_CLR] E
	ON B.MODEL_ID = E.MODEL_ID AND B.BODYCLR_CD = E.BODYCLR_CD
    WHERE
	    1 = 1
	    AND (CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, A.CREATE_DT), 120) >= CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, A.[CREATE_DT]), 120) END
	    AND CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, A.CREATE_DT), 120) <= CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), DATEADD(HOUR, 7, A.[CREATE_DT]), 120) END)";

                if (events.Split(new char[] {','}).ToList().Where(p => p == "0").Count() == 0)
                {
                    cmd += string.Format(" AND A.MEMBERID IN (SELECT MEMBERID FROM T_EVENTS_REGISTER WHERE EVENT_ID IN (SELECT Data FROM dbo.Split('{0}', ',')))", events);
                }

                cmd += " group by A.CREATE_DT, A.TITLENAME, A.FNAME, A.LNAME, A.MEMBERID, A.MOBILE, A.EMAIL, A.MEMBERID, B.VIN, B.PLATE_NO, B.DEALER, B.RS_DATE, D.MODEL, E.BODYCLR_NAME, A.PRIVILEGE_CNT ) DATA";

                cmd = string.Format(cmd,
                    WebUtility.GetSQLTextValue(startDate),
                    WebUtility.GetSQLTextValue(endDate));

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    table = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table;
        }
        public DataTable GetReport(string startDate, string endDate)
        {
            DataTable table = new DataTable();
            try
            {
                string cmd = @"
DECLARE @START NVARCHAR(10) = N'{0}'
DECLARE @END NVARCHAR(10) = N'{1}'

SELECT ROW_NUMBER() OVER(ORDER BY [Create Date] DESC) AS [No.], *
FROM (
    SELECT DISTINCT
    A.CREATE_DT AS [Create Date],
    A.TITLENAME AS [Title],
    A.FNAME AS [Name],
    A.LNAME AS [Family Name],
    A.MEMBERID AS [Member ID],
		iif(A.GENDER = 'M', 'Male', iif(A.GENDER = 'F', 'Female', iif(A.GENDER = 'N', 'Not specified', A.GENDER))) Gender,
    A.MOBILE AS [Tel.],
    A.EMAIL AS [E-mail],
    A.PRIVILEGE_CNT AS [Redeem],
		A.ADDRESS1 as [Address],
		A.SUBDISTRICT,
		A.DISTRICT,
    a.PROVINCE as [Province], 
    a.POSTALCODE as [Postcode],
	a.Birthdate,
    ROW_NUMBER() OVER(PARTITION BY A.MEMBERID ORDER BY A.MEMBERID) AS [Car No.],
    B.VIN AS [Vin],
    B.PLATE_NO AS [Rigister No.],
	D.MODEL,
	E.BODYCLR_NAME AS [Color],
    B.DEALER AS [Dealer],
    B.RS_DATE AS [R/S Date],
	MAX(C.Login_DT) as LastLogin,
	A.register_type
    FROM [dbo].[T_CUSTOMER_CAR_OWNER] A
    INNER JOIN [dbo].[T_CUSTOMER_CAR] B
    ON A.MEMBERID = B.MEMBERID
	LEFT join [dbo].[T_CUSTOMER_TOKEN] C 
	on A.memberid = C.memberid
	LEFT JOIN [dbo].[T_CAR_MODEL] D
	ON B.MODEL_ID = D.MODEL_ID
	LEFT JOIN [dbo].[T_CAR_MODEL_CLR] E
	ON B.MODEL_ID = E.MODEL_ID AND B.BODYCLR_CD = E.BODYCLR_CD
    LEFT JOIN [dbo].[T_CUSTOMER] customer
	ON customer.MEMBERID = A.MEMBERID
    where
	    A.DEL_FLAG IS NULL AND customer.DEL_FLAG IS NULL 
	    AND (CONVERT(NVARCHAR(10), A.CREATE_DT, 120) >= CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), A.[CREATE_DT], 120) END
	    AND CONVERT(NVARCHAR(10), A.CREATE_DT, 120) <= CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), A.[CREATE_DT], 120) END) 
group by A.CREATE_DT, A.TITLENAME, A.FNAME, A.LNAME, A.MEMBERID, A.MOBILE, A.EMAIL, A.MEMBERID, B.VIN, B.PLATE_NO, 
B.DEALER, B.RS_DATE, D.MODEL, E.BODYCLR_NAME, A.PRIVILEGE_CNT,A.address1,A.address2 ,A.SUBDISTRICT,a.DISTRICT,
a.PROVINCE,a.POSTALCODE,A.birthdate, A.GENDER, A.register_type) DATA";

                cmd = string.Format(cmd,
                    WebUtility.GetSQLTextValue(startDate),
                    WebUtility.GetSQLTextValue(endDate));

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    table = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table;
        }
        public DataTable GetEvents()
        {
            DataTable table = new DataTable();
            try
            {
                string cmd = @"
DECLARE @TABLE TABLE (ID INT, TITLE NVARCHAR(250))

INSERT INTO @TABLE VALUES (0, 'All')

INSERT INTO @TABLE
SELECT ID, TITLE FROM T_EVENTS

SELECT * FROM @TABLE ORDER BY ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    table = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table;
        }
    }
}