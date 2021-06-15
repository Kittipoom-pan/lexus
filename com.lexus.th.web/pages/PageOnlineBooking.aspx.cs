using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web.pages
{
    public partial class PageOnlineBooking : System.Web.UI.Page
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
                        BindGridData(dlBooking.SelectedValue, dlBookingType.SelectedValue);
                        BindDealer();
                        lbNotSet.Visible = false;
                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void dlBookingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindBooking(dlBookingType.SelectedValue);
                BindGridData(dlBooking.SelectedValue, dlBookingType.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(dlBookingType.SelectedValue) || dlBookingType.SelectedValue == "0")
            {
                ClientPopup(ModalType.Warning, "Please select online booking type.");
                return;
            }
            try
            {
                string filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\OnlineBookingReport.xlsx";
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                OnlineBookingService srv = new OnlineBookingService();
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells["A1"].LoadFromDataTable(srv.GetReportRegister(dlBookingType.SelectedValue, dlBooking.SelectedValue), true);
                    //ws.Column(13).Style.Numberformat.Format = "dd/mm/yyyy"; 
                    pck.Save();
                }

                Response.Clear();
                Response.ContentType = "Application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", string.Format("filename={0}", Path.GetFileName(filePath)));
                Response.TransmitFile(filePath);
                Response.End();
                ClientPopup(ModalType.Success, "Success");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private void BindDealer()
        {
            try
            {
                OnlineBookingService srv = new OnlineBookingService();
                dlDealer.DataSource = srv.GetDealer();
                dlDealer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dlBooking_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindGridData(dlBooking.SelectedValue, dlBookingType.SelectedValue);
                
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
                BindGridData(dlBooking.SelectedValue, dlBookingType.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private void BindBooking(string type)
        {
            try
            {
                OnlineBookingService srv = new OnlineBookingService();
                dlBooking.DataSource = srv.GetBookingByType(type);
                dlBooking.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGridData(string bookingId, string bookingType)
        {
            try
            {
                OnlineBookingService srv = new OnlineBookingService();
                gvCust.DataSource = srv.GetBookingCodeByBookingId(bookingId, bookingType);
                gvCust.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGridDataByType(string bookingType)
        {
            try
            {
                OnlineBookingService srv = new OnlineBookingService();
                gvCust.DataSource = srv.GetBookingBySaveType(bookingType);
                gvCust.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvCust.SelectedIndex;
                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    upModalAdd.Update();
                    return;
                }
                string name = tbName.Text;
                string surname = tbSurname.Text;
                string contact_number = tbContactNumber.Text;
                string email = tbEmail.Text;
                string plate_number = tbPlateNumber.Text;
                string remark = tbRemark.Text;
                string referral_name = tbReferralName.Text;
                string referral_surname = tbReferralSurname.Text;
                string referral_contact_number = tbReferralContactNumber.Text;
                string referral_email = tbEmail.Text;
                string dealer = dlDealer.SelectedValue;
                string test_drive = dlTestDrive.SelectedValue;

                OnlineBookingService srv = new OnlineBookingService();
                srv.UpdateBooking(lbId.Text,name,surname, contact_number,email,plate_number,remark, referral_name, referral_surname, referral_contact_number, referral_email, dealer, test_drive);
                ClientPopup(ModalType.Success, "Completed");
                BindGridDataByType(dlBookingType.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private void ClientPopup(ModalType type, string message)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
            upModalAdd.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdWarning').modal('hide');", true);
            upModalWarning.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
            upModelDel.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdUpload", "$('#mdUpload').modal('hide');", true);

            if (type == ModalType.Success)
            {
                lbSuccess.Text = message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdSucess", "$('#mdSuccess').modal();", true);
                upModalSuccess.Update();
            }
            if (type == ModalType.Error)
            {
                lbErrMsg.Text = message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdError", "$('#mdError').modal();", true);
                upModalError.Update();
            }
            if (type == ModalType.Warning)
            {
                lbWarning.Text = message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdWarning", "$('#mdWarning').modal();", true);
                upModalWarning.Update();
            }
        }
        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvCust_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelId.Text = "";
                    lbDelNo.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelId.Text = gvCust.DataKeys[index].Values["id"].ToString();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal();", true);
                        upModelDel.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
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

        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lbId.Text = "";
                tbName.Text = "";
                tbSurname.Text = "";
                tbContactNumber.Text = "";
                tbEmail.Text = "";
                tbPlateNumber.Text = "";
                tbRemark.Text = "";
                tbReferralName.Text = "";
                tbReferralSurname.Text = "";
                tbReferralContactNumber.Text = "";
                tbReferralEmail.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();
                    lbId.Text = id;

                    OnlineBookingService srv = new OnlineBookingService();
                    DataRow row = srv.GetBookingById(id);

                    //DataRow row = srv.GetBookingCodeById(lbBookingCodeId.Text, lbBookingId.Text);

                    if (row != null)
                    {
                        tbName.Text = row["name"].ToString();
                        tbSurname.Text = row["surname"].ToString();
                        tbContactNumber.Text = row["contact_number"].ToString();
                        tbEmail.Text = row["email"].ToString();
                        tbPlateNumber.Text = row["plate_number"].ToString();
                        tbRemark.Text = row["remark"].ToString();
                        tbReferralName.Text = row["referral_name"].ToString();
                        tbReferralSurname.Text = row["referral_surname"].ToString();
                        tbReferralContactNumber.Text = row["referral_contact_number"].ToString();
                        tbReferralEmail.Text = row["referral_email"].ToString();
                        string dealer_id = row["dealer_id"].ToString();

                        var vlDealer = dlDealer.Items.FindByValue(dealer_id);
                        if (vlDealer != null)
                        {
                            lbNotSet.Visible = false;
                            dlDealer.SelectedValue = row["dealer_id"].ToString();
                        }
                        else
                        {
                            lbNotSet.Visible = true;
                            BindDealer();
                        }
                        dlTestDrive.SelectedValue = row["need_to_test_drive"].ToString().ToLower() == "true" ? "1" : "0";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                        upModalAdd.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

    }
}