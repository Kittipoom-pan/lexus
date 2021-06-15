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
    public class UploadFileKatashikiMapping : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                    HttpFileCollection files = context.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fname = context.Server.MapPath("~/Excel/Katashiki/" + file.FileName);
                        file.SaveAs(fname);

                        DataTable dt = ProcessExcel(fname);

                        KatashikiService srv = new KatashikiService();

                        foreach (DataRow dr in dt.Rows)
                        {
                            DataTable rowInsert = srv.AddKatashikiModel(dr["katashiki_code"].ToString(),
                                dr["model_id"].ToString(),
                                context.Session["User"].ToString(),
                                true);

                            if (dtResult == null && rowInsert != null)
                                dtResult = rowInsert;
                            else if (dtResult != null && rowInsert != null && rowInsert.Rows.Count > 0)
                            {
                                //dtResult.Rows.Add(new Object[]{ rowInsert.Rows[0]["vin"], rowInsert.Rows[0]["log"]});
                                dtResult.ImportRow(rowInsert.Rows[0]);
                            }
                        }

                    }

                    ExportResult(dtResult, context);
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
            //string strConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";

            OleDbConnection exconn = new OleDbConnection(strConnection);
            exconn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT katashiki_code, model_id FROM[Sheet1$] ", exconn);
            da.Fill(dt);
            exconn.Close();

            return dt;
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

            string FileName = "ImportKatakishiResult_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";
            string FilePath = context.Server.MapPath("~/Excel/Katashiki/" + FileName);
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