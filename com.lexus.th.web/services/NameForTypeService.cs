using AppLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class NameForTypeService
    {
        private string conn;
        public NameForTypeService()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.conn = System.Web.Configuration.WebConfigurationManager.AppSettings["LexusDBConn"];
        }

        public DataTable GetName(string type)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DBAccess db = new DBAccess(conn, DBAccess.ConnectionType.SQLDB))
                {
                    if (type == "Privilege")
                    {
                        string cmd = @"
DECLARE	@TABLE TABLE ([ID] INT,[TITLE] NVARCHAR(150))

INSERT INTO @TABLE VALUES (0, '')
INSERT INTO @TABLE
SELECT ID, TITLE, IMAGE FROM T_PRIVILEDGES WHERE GETDATE() BETWEEN DISPLAY_START AND DISPLAY_END

SELECT * FROM @TABLE ORDER BY [ID]";

                        dt = db.GetDataTableFromCommandText(cmd);
                    }
                    if (type == "Event")
                    {
                        string cmd = @"
DECLARE	@TABLE TABLE ([ID] INT,[TITLE] NVARCHAR(150))

INSERT INTO @TABLE VALUES (0, '')
INSERT INTO @TABLE
SELECT ID, TITLE, IMAGES1 AS IMAGE FROM T_EVENTS WHERE GETDATE() BETWEEN DISPLAY_START AND DISPLAY_END

SELECT * FROM @TABLE ORDER BY [ID]";

                        dt = db.GetDataTableFromCommandText(cmd);
                    }
                    if (type == "News")
                    {
                        string cmd = @"
DECLARE	@TABLE TABLE ([ID] INT,[TITLE] NVARCHAR(150))

INSERT INTO @TABLE VALUES (0, '')
INSERT INTO @TABLE
SELECT ID, TITLE, IMAGES1 AS IMAGE FROM T_NEWS WHERE GETDATE() BETWEEN DISPLAY_START AND DISPLAY_END

SELECT * FROM @TABLE ORDER BY [ID]";

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
    }
}