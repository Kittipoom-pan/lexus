using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web
{
    public class RptRegisterEventService
    {
        private string conn;
        public RptRegisterEventService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetReport(string events, string startDate, string endDate, string FullName, string Mobile, string MemberID)
        {
            DataTable table = new DataTable();
            try
            {
                string cmd = @"
DECLARE @START NVARCHAR(10) = N'{0}'
DECLARE @END NVARCHAR(10) = N'{1}'

SELECT ROW_NUMBER() OVER(ORDER BY [Date/Time] DESC) AS [No.], *
FROM (
    SELECT DISTINCT
    C.CREATE_DT AS [Date/Time],
    A.TITLENAME AS [Title],
    A.FNAME AS [Name],
    A.LNAME AS [Family Name],
    A.MEMBERID AS [Member ID],
    A.MOBILE AS [Tel.],
    A.EMAIL AS [E-mail],
	A.Birthdate,
    (CASE WHEN B.VIN IS NULL
    THEN (SELECT STRING_AGG(vin,', ')
          FROM T_CUSTOMER_CAR cr
          WHERE (A.MEMBERID = cr.MEMBERID) 
         )
    ELSE (B.VIN) 
    END) AS VIN,
    B.RS_DATE AS [R/S Date],
    G.MODEL AS [Rigister Model],
    D.display_en AS [Dealer],
    /*ROW_NUMBER() OVER(PARTITION BY A.MEMBERID ORDER BY A.MEMBERID) AS [Car No.],
    
    B.RS_DATE AS [R/S Date],*/
    C.follower_title AS [Follower Title],
    C.follower_name AS [Follower Name],
    C.follower_tel_no AS [Follower Tel No.],
    C.follower_email AS [Follower Email],
		C.follower_birthdate,
		E.content RelationShip,
		C.relationship_special,
		F.content FollowerCarBrand,
		C.car_brand_special,
		U.year_en follower_year_purchase
FROM [dbo].[T_EVENTS_REGISTER] C
    LEFT JOIN [dbo].[T_CUSTOMER] A
    ON A.MEMBERID = C.MEMBERID
    LEFT JOIN [dbo].[system_master_valuelist] E
    ON C.relationship_id = E.id AND E.group_data = 'relationship'
	LEFT JOIN [dbo].[system_master_valuelist] F
    ON C.car_brand_id = F.id AND F.group_data = 'car_brand'
    LEFT JOIN [dbo].[T_CUSTOMER_CAR] B
    ON A.MEMBERID = B.MEMBERID and c.vin = b.vin
	LEFT JOIN [dbo].[T_CAR_MODEL] G
    ON B.MODEL_ID = G.MODEL_ID
	LEFT JOIN [dbo].[T_DEALER_MASTER] D
    ON B.DEALER = D.ID
    LEFT JOIN [dbo].[utility_generate_year] U
    ON C.follower_year_purchase_id = U.id
WHERE (CONVERT(NVARCHAR(10), C.CREATE_DT, 120) >= CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), C.CREATE_DT, 120) END
	  AND CONVERT(NVARCHAR(10), C.CREATE_DT, 120) <= CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), C.CREATE_DT, 120) END)";

                if (events.Split(new char[] {','}).ToList().Where(p => p == "0").Count() == 0)
                {
                    cmd += string.Format(" AND C.EVENT_ID = '{0}'", events);
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

                cmd += " ) DATA";

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