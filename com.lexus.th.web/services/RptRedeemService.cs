using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class RptRedeemService
    {
        private string conn;
        public RptRedeemService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetReport(string privileges, string redStart, string redEnd, string verStart, string verEnd, string FullName, string Mobile, string MemberID)
        {
            DataTable table = new DataTable();
            try
            {
                string cmd = @"
DECLARE @PRIVILEGE NVARCHAR(MAX)
DECLARE @RED_START NVARCHAR(10) = N'{0}'
DECLARE @RED_END NVARCHAR(10) = N'{1}'
DECLARE @VER_START NVARCHAR(10) = N'{2}'
DECLARE @VER_END NVARCHAR(10) = N'{3}'

SELECT ROW_NUMBER() OVER(ORDER BY [Date Redeem]) AS [No.], *
FROM (
	SELECT DISTINCT
	    A.TITLENAME AS [Title],
	    A.FNAME AS [Name],
	    A.LNAME AS [Family Name],
	    A.MEMBERID AS [Member ID],
	    A.MOBILE AS [Tel.],
	    A.EMAIL AS [E-mail],
	    C.TITLE AS [Privilege Name],
	    B.REDEEM_DT AS [Date Redeem],
	    B.REDEEM_CODE AS [Redeem Code],
	    B.VERIFY_DT AS [Date Verify],
	    E.VIN, 
	    A.PRIVILEGE_CNT AS [Redeem Balance],
	    F.MODEL,
	    G.BODYCLR_NAME AS [Color],
        B.SHOP_NM
	FROM [dbo].[T_CUSTOMER] A
	    INNER JOIN [dbo].[T_CUSTOMER_REDEEM] B
	    ON A.MEMBERID = B.MEMBERID
	    LEFT JOIN [dbo].[T_PRIVILEDGES] C
	    ON B.PRIVILEGE_ID = C.ID
	    LEFT JOIN [dbo].[T_CUSTOMER_CAR] E
	    ON A.MEMBERID = E.MEMBERID
	    LEFT JOIN [dbo].[T_CAR_MODEL] F
	    ON E.MODEL_ID = F.MODEL_ID
	    LEFT JOIN [dbo].[T_CAR_MODEL_CLR] G
	    ON E.MODEL_ID = G.MODEL_ID AND E.BODYCLR_CD = G.BODYCLR_CD
	WHERE	
		1 = 1	    
		AND (ISNULL(CONVERT(NVARCHAR(10), B.REDEEM_DT, 120), '') >= CASE WHEN LEN(@RED_START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @RED_START, 103), 120) ELSE ISNULL(CONVERT(NVARCHAR(10), B.REDEEM_DT, 120), '') END
		AND ISNULL(CONVERT(NVARCHAR(10), B.REDEEM_DT, 120), '') <= CASE WHEN LEN(@RED_END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @RED_END, 103), 120) ELSE ISNULL(CONVERT(NVARCHAR(10), B.REDEEM_DT, 120), '') END)
		AND (ISNULL(CONVERT(NVARCHAR(10), B.VERIFY_DT, 120), '') >= CASE WHEN LEN(@VER_START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @VER_START, 103), 120) ELSE ISNULL(CONVERT(NVARCHAR(10), B.VERIFY_DT, 120), '') END
		AND ISNULL(CONVERT(NVARCHAR(10), B.VERIFY_DT, 120), '') <= CASE WHEN LEN(@VER_END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @VER_END, 103), 120) ELSE ISNULL(CONVERT(NVARCHAR(10), B.VERIFY_DT, 120), '') END)";

                if (privileges.Split(new char[] { ',' }).ToList().Where(p => p == "0").Count() == 0)
                {
                    cmd += string.Format(" AND C.ID IN (SELECT Data FROM dbo.Split('{0}', ','))", privileges);
                }
                if (FullName != "")
                {
                    FullName = "%" + FullName.Replace(" ", "%") + "%";
                    cmd += string.Format(" AND A.FNAME + A.LNAME like N'{0}'", WebUtility.GetSQLTextValue(FullName));
                }
                if (Mobile != "")
                {
                    cmd += string.Format(" AND A.MOBILE = '{0}'", WebUtility.GetSQLTextValue(Mobile));
                }
                if (MemberID != "")
                {
                    cmd += string.Format(" AND A.MEMBERID = '{0}'", WebUtility.GetSQLTextValue(MemberID));
                }

                cmd += " ) DATA ORDER BY [Date Redeem]";

                cmd = string.Format(cmd,
                    WebUtility.GetSQLTextValue(redStart),
                    WebUtility.GetSQLTextValue(redEnd),
                    WebUtility.GetSQLTextValue(verStart),
                    WebUtility.GetSQLTextValue(verEnd));

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
        public DataTable GetPrivileges()
        {
            DataTable table = new DataTable();
            try
            {
                string cmd = @"
DECLARE @TABLE TABLE (ID INT, TITLE NVARCHAR(250))

INSERT INTO @TABLE VALUES (0, 'All')

INSERT INTO @TABLE
SELECT ID, TITLE FROM T_PRIVILEDGES

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