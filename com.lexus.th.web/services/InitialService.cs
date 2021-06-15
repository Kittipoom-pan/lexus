using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class InitialService
    {
        private string conn;
        public InitialService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }
        public DataTable GetInitialSearch(string searchValue, bool isCitizenId, bool isFname, bool isEmail, bool isVin, bool isRSDate, string startDate, string endDate, string role)
        {

            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = "";

                    cmd = @"
DECLARE @Value NVARCHAR(255) = N'{0}'
DECLARE @S_CITIZENID BIT = N'{1}'
DECLARE @S_FNAME BIT = N'{2}'
DECLARE @S_EMAIL BIT = N'{3}'
DECLARE @S_VIN BIT = N'{4}'
DECLARE @S_RSDATE BIT = N'{5}'
DECLARE @START NVARCHAR(20) = N'{6}'
DECLARE @END NVARCHAR(20) = N'{7}'

SELECT		DISTINCT INI.[id]
			,INI.[firstname]
			,INI.[lastname]
			,INI.[gender]
			,INI.[birthday]
			,INI.[citizen_id]
			,INI.[email]
			,D.display_en AS dealer
			,INI.[vin]
			,INI.[plate_no]
            ,M.MODEL AS model
            ,INI.[color]
            ,INI.[rs_date]
            ,INI.[create_date]
			,INI.[create_by]
FROM		initial_data INI 
    LEFT JOIN T_DEALER_MASTER D ON INI.dealer = D.id
    LEFT JOIN T_CAR_MODEL M ON INI.model = M.MODEL_ID
WHERE		(
                ISNULL(INI.citizen_id, '') LIKE CASE @S_CITIZENID WHEN 1 THEN @Value + '%' END
                OR ISNULL(INI.email, '') LIKE CASE @S_EMAIL WHEN 1 THEN @Value + '%' END
                OR ISNULL(INI.vin, '') LIKE CASE @S_VIN WHEN 1 THEN @Value + '%' END
			    OR ISNULL(INI.firstname, '') LIKE CASE @S_FNAME WHEN 1 THEN @Value + '%' END
			    OR (CONVERT(NVARCHAR(10), CONVERT(DATETIME, INI.[rs_date], 120), 120) >= CASE @S_RSDATE WHEN 1 THEN CASE WHEN LEN(@START) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @START, 103), 120) ELSE CONVERT(NVARCHAR(10), CONVERT(DATETIME, INI.[rs_date], 120), 120) END END
	            AND CONVERT(NVARCHAR(10), CONVERT(DATETIME, INI.[rs_date], 120), 120) <= CASE @S_RSDATE WHEN 1 THEN CASE WHEN LEN(@END) > 0 THEN CONVERT(NVARCHAR(10), CONVERT(DATETIME, @END, 103), 120) ELSE CONVERT(NVARCHAR(10), CONVERT(DATETIME, INI.[rs_date], 120), 120) END END)
            )
ORDER BY INI.id";

                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(searchValue),
                        (isCitizenId) ? 1 : 0,
                        (isFname) ? 1 : 0,
                        (isEmail) ? 1 : 0,
                        (isVin) ? 1 : 0,
                        (isRSDate) ? 1 : 0,
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
        public DataRow GetInitialById(string id)
        {
            DataRow row = null;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
DECLARE @Value NVARCHAR(250) = N'{0}'

SELECT		DISTINCT INI.[id]
			,INI.[firstname]
			,INI.[lastname]
			,INI.[gender]
			,INI.[birthday]
			,INI.[citizen_id]
			,INI.[email]
			,INI.[dealer]
			,INI.[vin]
			,INI.[plate_no]
            ,INI.[model]
            ,INI.[color]
            ,INI.[rs_date]
			,DATEADD(HOUR, 7, INI.[create_date]) AS [create_date]
			,INI.[create_by]
FROM		initial_data INI
  WHERE	INI.id = {0}";

                    row = db.GetDataTableFromCommandText(string.Format(cmd, WebUtility.GetSQLTextValue(id))).AsEnumerable().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public DataTable AddInitial(string titleName, string firstName, string lastName, string gender, string birthday, string citizen_id, string email, string dealer_code, string branch_code, 
            string vin, string plate_no, string katashiki_code, string rs_date, string user)
        {
            DataTable row = null;
            DateTime datenow = DateTime.Now;
            try
            {
                string cmd = @"
DECLARE @DEALER_ID int 
DECLARE @MODEL_ID int
SET @DEALER_ID = (select top 1 id from T_DEALER_MASTER where DEALER_CODE = @dealer_code and BRANCH_CODE = @branch_code)
SET @MODEL_ID = (select top 1 model_id from katashiki_model where katashiki_code = @katashiki_code)

insert into initial_data
(titlename, firstname, lastname, gender, birthday, citizen_id, email, dealer, vin, plate_no, 
model, rs_date, create_date, create_by)
select a.* 
from 
(select @titlename titlename, @firstname firstname, @lastname lastname, @gender gender, @birthday birthday, @citizen_id citizen_id, @email email, 
@DEALER_ID DEALER_ID, @vin vin, @plate_no plate_no, @MODEL_ID MODEL_ID, @rs_date rs_date, @create_date create_date, @create_by create_by) a
join (select count(id) c_id from initial_data where vin = @vin) b
on b.c_id = 0 and @DEALER_ID is not null and @MODEL_ID is not null

IF @@ROWCOUNT = 0
BEGIN
	IF @DEALER_ID is null
	    BEGIN
		    select @vin vin, 'DEALER not found' log
	    END 
    ELSE IF @MODEL_ID is null
	    BEGIN
		    select @vin vin, 'MODEL not found' log
	    END 
        BEGIN
		    select @vin vin, 'VIN is duplicate' log
	    END
    END
ELSE
BEGIN
	select @vin vin, 'insert success' log
END";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    db.ParamterList.Add(new SqlParameter("@titleName", titleName));
                    db.ParamterList.Add(new SqlParameter("@firstName", firstName));
                    db.ParamterList.Add(new SqlParameter("@lastName", lastName));
                    db.ParamterList.Add(new SqlParameter("@gender", gender));
                    db.ParamterList.Add(new SqlParameter("@birthday", birthday));
                    db.ParamterList.Add(new SqlParameter("@citizen_id", citizen_id));
                    db.ParamterList.Add(new SqlParameter("@email", email));
                    db.ParamterList.Add(new SqlParameter("@dealer_code", dealer_code));
                    db.ParamterList.Add(new SqlParameter("@branch_code", branch_code));
                    db.ParamterList.Add(new SqlParameter("@vin", vin));
                    db.ParamterList.Add(new SqlParameter("@plate_no", plate_no));
                    db.ParamterList.Add(new SqlParameter("@katashiki_code", katashiki_code));
                    db.ParamterList.Add(new SqlParameter("@rs_date", rs_date));
                    db.ParamterList.Add(new SqlParameter("@create_date", datenow));
                    db.ParamterList.Add(new SqlParameter("@create_by", user));

                    row = db.GetDataTableFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        public string AddInitial(string titleName, string firstName, string lastName, string gender, string birthday, 
            string citizen_id, string email, string dealer, string vin, string plate_no, string model, string rs_date, string user)
        {
            string id = "";
            try
            {
                string cmd = @"
DECLARE @FNAME NVARCHAR(250) = N'{0}'
DECLARE @LNAME NVARCHAR(250) = N'{1}'
DECLARE @GENDER NVARCHAR(100) = N'{2}'
DECLARE @BIRTHDATE NVARCHAR(20) = N'{3}'
DECLARE @CITIZENID NVARCHAR(50) = N'{4}'
DECLARE @EMAIL NVARCHAR(100) = N'{5}'
DECLARE @DEALER NVARCHAR(50) = N'{6}'
DECLARE @VIN NVARCHAR(50) = N'{7}'
DECLARE @PLATENO NVARCHAR(50) = N'{8}'
DECLARE @MODEL NVARCHAR(300) = N'{9}'
DECLARE @RSDATE NVARCHAR(20) = N'{10}'
DECLARE @USER NVARCHAR(50) = N'{11}'
DECLARE @TITLE NVARCHAR(50) = N'{12}'

INSERT INTO initial_data (titlename, firstname, lastname, gender, birthday, citizen_id, email, dealer, vin, plate_no, model, rs_date,
create_date, create_by)
VALUES (
    CASE LEN(@TITLE) WHEN 0 THEN NULL ELSE @TITLE END,
    CASE LEN(@FNAME) WHEN 0 THEN NULL ELSE @FNAME END,
	CASE LEN(@LNAME) WHEN 0 THEN NULL ELSE @LNAME END,
	CASE LEN(@GENDER) WHEN 0 THEN NULL ELSE @GENDER END,
	CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @BIRTHDATE, 103) END,
	CASE LEN(@CITIZENID) WHEN 0 THEN NULL ELSE @CITIZENID END,
	CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
	CASE LEN(@DEALER) WHEN 0 THEN NULL ELSE @DEALER END,
    CASE LEN(@VIN) WHEN 0 THEN NULL ELSE @VIN END,
    CASE LEN(@PLATENO) WHEN 0 THEN NULL ELSE @PLATENO END,
    CASE LEN(@MODEL) WHEN 0 THEN NULL ELSE @MODEL END,
    CASE LEN(@RSDATE) WHEN 0 THEN NULL ELSE CONVERT(DATE, @RSDATE, 103) END,
    DATEADD(HOUR, 7, GETDATE()), 
    @USER)

SELECT CONVERT(NVARCHAR, @@IDENTITY) AS [ID]";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(firstName),
                        WebUtility.GetSQLTextValue(lastName),
                        WebUtility.GetSQLTextValue(gender),
                        WebUtility.GetSQLTextValue(birthday),
                        WebUtility.GetSQLTextValue(citizen_id),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(dealer),
                        WebUtility.GetSQLTextValue(vin),
                        WebUtility.GetSQLTextValue(plate_no),
                        WebUtility.GetSQLTextValue(model),
                        WebUtility.GetSQLTextValue(rs_date),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(titleName));

                    id = db.ExecuteScalarFromCommandText<string>(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }
        public void UpdateInitial(string id, string firstName, string lastName, string gender, string birthday, string citizen_id, string email, string dealer, string vin, string plate_no, string model, string color, string user)
        {
            try
            {
                string cmd = @"
DECLARE @FNAME NVARCHAR(250) = N'{0}'
DECLARE @LNAME NVARCHAR(250) = N'{1}'
DECLARE @GENDER NVARCHAR(100) = N'{2}'
DECLARE @BIRTHDATE NVARCHAR(20) = N'{3}'
DECLARE @CITIZENID NVARCHAR(50) = N'{4}'
DECLARE @EMAIL NVARCHAR(100) = N'{5}'
DECLARE @DEALER NVARCHAR(50) = N'{6}'
DECLARE @VIN NVARCHAR(50) = N'{7}'
DECLARE @PLATENO NVARCHAR(50) = N'{8}'
DECLARE @MODEL NVARCHAR(300) = N'{9}'
DECLARE @COLOR NVARCHAR(300) = N'{10}'
DECLARE @USER NVARCHAR(50) = N'{11}'
DECLARE @ID NVARCHAR(50) = N'{12}'

UPDATE initial_data
SET firstname = CASE LEN(@FNAME) WHEN 0 THEN NULL ELSE @FNAME END,
	lastname = CASE LEN(@LNAME) WHEN 0 THEN NULL ELSE @LNAME END,
	gender = CASE LEN(@GENDER) WHEN 0 THEN NULL ELSE @GENDER END,
	birthday = CASE LEN(@BIRTHDATE) WHEN 0 THEN NULL ELSE @BIRTHDATE END,
	citizen_id = CASE LEN(@CITIZENID) WHEN 0 THEN NULL ELSE @CITIZENID END,
	email = CASE LEN(@EMAIL) WHEN 0 THEN NULL ELSE @EMAIL END,
    dealer = CASE LEN(@DEALER) WHEN 0 THEN NULL ELSE @DEALER END,
    vin = CASE LEN(@VIN) WHEN 0 THEN NULL ELSE @VIN END,
    plate_no = CASE LEN(@PLATENO) WHEN 0 THEN NULL ELSE @PLATENO END,
    model = CASE LEN(@MODEL) WHEN 0 THEN NULL ELSE @MODEL END,
    color = CASE LEN(@COLOR) WHEN 0 THEN NULL ELSE @COLOR END,
    update_date = DATEADD(HOUR, 7, GETDATE()),
    update_by = @USER
WHERE ID = @ID";

                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    cmd = string.Format(cmd,
                        WebUtility.GetSQLTextValue(firstName),
                        WebUtility.GetSQLTextValue(lastName),
                        WebUtility.GetSQLTextValue(gender),
                        WebUtility.GetSQLTextValue(birthday),
                        WebUtility.GetSQLTextValue(citizen_id),
                        WebUtility.GetSQLTextValue(email),
                        WebUtility.GetSQLTextValue(dealer),
                        WebUtility.GetSQLTextValue(vin),
                        WebUtility.GetSQLTextValue(plate_no),
                        WebUtility.GetSQLTextValue(model),
                        WebUtility.GetSQLTextValue(color),
                        WebUtility.GetSQLTextValue(user),
                        WebUtility.GetSQLTextValue(id));

                    db.ExecuteNonQueryFromCommandText(cmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDealer()
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    string cmd = @"
SELECT id DEALER_ID, display_en DEALER_NAME
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

        public DataTable GetCarModels()
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

        public bool IsExistsVin(string vin)
        {

            bool isExists = true;
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    int cnt = db.ExecuteScalarFromCommandText<int>(string.Format("SELECT COUNT(1) AS CNT FROM initial_data WHERE vin = N'{0}'", WebUtility.GetSQLTextValue(vin)));
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
        //        public void DeleteCustomer(string id, string remark)
        //        {
        //            try
        //            {
        //                //string cmd = @"DELETE FROM T_CUSTOMER WHERE ID = @ID";
        //                string cmd = @"
        //DECLARE @REMARK NVARCHAR(MAX) = N'{0}'
        //DECLARE @ID INT = N'{1}'

        //UPDATE T_CUSTOMER_CAR_OWNER SET DEL_FLAG = 'Y', [EXPIRY] = DATEADD(HOUR, 7, GETDATE()), REMARK = ISNULL(CONVERT(NVARCHAR(MAX), REMARK), '') + '|[' + CONVERT(NVARCHAR(20), DATEADD(HOUR, 7, GETDATE()), 120) + '] ' + @REMARK WHERE ID = @ID";

        //                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
        //                {
        //                    cmd = string.Format(cmd,
        //                        WebUtility.GetSQLTextValue(remark),
        //                        WebUtility.GetSQLTextValue(id));

        //                    db.ExecuteNonQueryFromCommandText(cmd);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

    }
}