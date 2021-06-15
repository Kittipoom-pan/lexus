using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using OfficeOpenXml;

namespace com.lexus.th.web.master
{
    public partial class PageReportCustomer : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Session["User"] == null)
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                    else if (Session["Role"].ToString() != "2")
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                    else
                    {
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();

                        //RptRegisterService srv = new RptRegisterService();
                        //dlEvent.DataSource = srv.GetEvents();
                        //dlEvent.DataBind();
                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvCust_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCust.PageIndex = e.NewPageIndex;
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindGridData()
        {
            try
            {
                RptRegisterService srv = new RptRegisterService();
                //gvCust.DataSource = srv.GetReport(dlEvent.SelectedValue, tbStartDate.Text, tbEndDate.Text);
                gvCust.DataSource = srv.GetReport(tbStartDate.Text, tbEndDate.Text);
                gvCust.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ClientPopup(ModalType type, string message)
        {
            if (type == ModalType.Error)
            {
                lbErrMsg.Text = message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdError", "$('#mdError').modal();", true);
                upModalError.Update();
            }
        }
        private void RegisterPostBackControl()
        {
            foreach (GridViewRow row in gvCust.Rows)
            {
                LinkButton lnkFull = row.FindControl("gvBtnEdit") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\RegisterReport.xlsx";
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                RptRegisterService srv = new RptRegisterService();
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                    //ws.Cells["A1"].LoadFromDataTable(srv.GetReport(dlEvent.SelectedValue, tbStartDate.Text, tbEndDate.Text), true);
                    ws.Cells["A1"].LoadFromDataTable(srv.GetReport(tbStartDate.Text, tbEndDate.Text), true);
                    ws.Column(2).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(13).Style.Numberformat.Format = "dd/mm/yyyy";
                    ws.Column(23).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(24).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    pck.Save();
                }

                Response.Clear();
                Response.ContentType = "Application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", string.Format("filename={0}", Path.GetFileName(filePath)));
                Response.TransmitFile(filePath);
                Response.End();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
    }
}