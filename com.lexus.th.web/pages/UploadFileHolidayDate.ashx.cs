using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace com.lexus.th.web.pages
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadFileHolidayDate : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            context.Response.ContentType = "text/plain";
            string result = "";
            string user = context.Request.QueryString["user"];
            try
            {
                
                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection files = context.Request.Files;

                    HttpPostedFile file = files[0];
                    string fname = context.Server.MapPath("~/Excel/Holiday/" + file.FileName);
                    file.SaveAs(fname);

                    var msg = ProcessExcel(fname, user);

                    //ExportResult(dtResult, context);
                    //string DownloadPath = ExportResult(dtResult, context);
                    //row.Add("path", DownloadPath);
                    row.Add("message", (msg!="")?msg:"Upload Success.");
                    row.Add("status", true);
                }
                else
                {
                    row.Add("message", "Pease select file");
                    row.Add("status", false);
                }
            }
            catch (Exception ex)
            {
                row.Add("message", result + ex.Message);
                row.Add("status", false);
            }

            context.Response.Write(JsonConvert.SerializeObject(row, new JavaScriptDateTimeConverter()));
        }

        private string ProcessExcel(string filepath, string user)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            string strConnection;

            if (Path.GetExtension(filepath.ToLower()) == ".xls")
                strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"";
            else
                strConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
            //string strConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";

            OleDbConnection exconn = new OleDbConnection(strConnection);
            exconn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT Holiday_Date, Dealer_ID FROM[Sheet1$] Where Dealer_ID IS NOT NULL AND Holiday_Date IS NOT NULL", exconn);
            da.Fill(dt);
            exconn.Close();

            DealerService srv = new DealerService();
            if (dt.AsEnumerable().Any())
            {
                List<HolidayDateModel> hollidays = dt.AsEnumerable().Select(c => new HolidayDateModel()
                {
                    Dealer_ID =  c["Dealer_ID"] != DBNull.Value? (int)(c.Field<double>("Dealer_ID")) : 0,
                    Holiday_Date = c["Holiday_Date"] != DBNull.Value? c.Field<DateTime>("Holiday_Date"): DateTime.Now

                }).Where(c=>c.Dealer_ID!=0).ToList();
                //bool isDuplicate = hollidays.GroupBy(x => new { x.Holiday_Date, x.Dealer_ID })
                //         .Where(x => x.Skip(1).Any()).Any();
                //if (isDuplicate)
                //{
                //    return "Duplicate Data!!";
                //}
                srv.RenewHollidayFromUpload(hollidays, user);
                return "";
            }
            else
            {
                return "File has no data!!";
            }
        }

        private string ExportResult(DataTable ExportResult, HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Katashiki Code\tLog");

            if (ExportResult != null)
            {
                foreach (DataRow s in ExportResult.Rows)
                {
                    sb.Append("\r\n");
                    sb.Append((s["katashiki_code"] != null ? s["katashiki_code"].ToString() : "") + "\t");
                    sb.Append((s["log"] != null ? s["log"].ToString() : ""));
                }
            }

            string FileName = "HolidayTemplate_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";
            string FilePath = context.Server.MapPath("~/Excel/Holiday/" + FileName);
            File.WriteAllText(FilePath, sb.ToString());
            return System.Web.Configuration.WebConfigurationManager.AppSettings["excel_katashiki"] + FileName;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}