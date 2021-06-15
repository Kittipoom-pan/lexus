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
    public class UploadFileInitialCustomer : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            context.Response.ContentType = "text/plain";
            string result = "";

            try
            {
                DataTable dtResult = null;
                if (context.Request.Files.Count > 0)
                {
                    result += "Files.Count: " + context.Request.Files.Count.ToString() + "\n";
                    HttpFileCollection files = context.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fname = context.Server.MapPath("~/Excel/Initial/" + file.FileName);
                        file.SaveAs(fname);

                        result += "Open file index: " + i.ToString() + "\n";
                        DataTable dt = ProcessExcel(fname);

                        InitialService srv = new InitialService();

                        result += "Insert data\n";
                        foreach (DataRow dr in dt.Rows)
                        {
                            DataTable rowInsert = srv.AddInitial(dr["titleName"].ToString(),
                                dr["firstName"].ToString(),
                                dr["lastName"].ToString(),
                                dr["gender"].ToString(),
                                dr["birthday"].ToString(),
                                dr["citizen_id"].ToString(),
                                dr["email"].ToString(),
                                dr["dealer_code"].ToString(),
                                dr["branch_code"].ToString(),
                                dr["vin"].ToString(),
                                dr["plate_no"].ToString(),
                                dr["katashiki_code"].ToString(),
                                dr["rs_date"].ToString(),
                                context.Session["User"].ToString());

                            if (dtResult == null && rowInsert != null)
                                dtResult = rowInsert;
                            else if (dtResult != null && rowInsert != null && rowInsert.Rows.Count > 0)
                            {
                                //dtResult.Rows.Add(new Object[]{ rowInsert.Rows[0]["vin"], rowInsert.Rows[0]["log"]});
                                dtResult.ImportRow(rowInsert.Rows[0]);
                            }
                        }

                    }

                    result += "Export result \n";
                    string DownloadPath = ExportResult(dtResult, context);
                    row.Add("path", DownloadPath);
                    row.Add("message", result);
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

        private DataTable ProcessExcel(string filepath)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            string strConnection;

            if (Path.GetExtension(filepath.ToLower()) == ".xls")
                strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"";
            else
                strConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";

            OleDbConnection exconn = new OleDbConnection(strConnection);
            exconn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT titleName,firstName,lastName,gender,birthday,citizen_id,email,dealer_code,branch_code,vin,plate_no,katashiki_code,rs_date FROM[Sheet1$] ", exconn);
            da.Fill(dt);
            exconn.Close();

            return dt;
        }

        private string ExportResult(DataTable ExportResult, HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("VIN\tLog");

            if (ExportResult != null)
            {
                foreach (DataRow s in ExportResult.Rows)
                {
                    sb.Append("\r\n");
                    sb.Append((s["vin"] != null ? s["vin"].ToString() : "") + "\t");
                    sb.Append((s["log"] != null ? s["log"].ToString() : ""));
                }
            }

            //context.Response.Clear();
            //context.Response.Buffer = true;

            //context.Response.AddHeader("content-disposition", "attachment;filename=ImportResult.xls");
            //context.Response.Charset = "";
            //context.Response.ContentEncoding = Encoding.UTF8;

            //context.Response.ContentType = "application/ms-excel";
            //context.Response.ContentEncoding = Encoding.Unicode;
            //context.Response.BinaryWrite(Encoding.Unicode.GetPreamble());

            //context.Response.Output.Write(sb.ToString());
            string FileName = "ImportInitialResult_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";
            string FilePath = context.Server.MapPath("~/Excel/Initial/" + FileName);
            File.WriteAllText(FilePath, sb.ToString());
            return System.Web.Configuration.WebConfigurationManager.AppSettings["excel_initial"] + FileName;
            //context.Response.Write("http://dev.lexus-app.com/ExportExcel/" + FileName);
            //context.Response.Flush();
            //context.Response.End();
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