using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Globalization;
using OfficeOpenXml;
using System.Web.Configuration;

namespace com.lexus.th.web.master
{
    public partial class PageServiceTestDrive : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        DataTable dtTypeOfServiceDetail;
        DataTable dtAppointmentTypeOfService;

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

                        BindCriPreferredModel();
                        BindCriDealer();
                        BindPurchasePlan();
                        BindDealer();
                        BindPreferredModel();

                        lbNotSet.Visible = false;

                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot open. Please try again later.");
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
                ClientPopup(ModalType.Error, "Cannot search. Please try again later.");
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
                ClientPopup(ModalType.Error, "Cannot change. Please try again later.");
            }
        }
        private void BindGridData()
        {
            try
            {

                ServiceTestDriveCriteria criteria = new ServiceTestDriveCriteria()
                {
                    DealerId = dlcriDealer.SelectedValue,
                    PreferredModel = dlcriPreferredModel.SelectedValue,
                    TestDriveCode = tbcriTestDriveCode.Text,
                    Name = tbcriName.Text,
                    Surname = tbcriSurname.Text,
                    MobileNumber = tbcriMobileNumber.Text,
                    Status = dlcriStatus.SelectedValue,
                    RegisDateFrom = tbcriRegisterDateFrom.Text,
                    RegisDateTo = tbcriRegisterDateTo.Text,
                };
                if (!string.IsNullOrWhiteSpace(criteria.RegisDateFrom) && !string.IsNullOrWhiteSpace(criteria.RegisDateTo))
                {
                    DateTime start = DateTime.ParseExact(criteria.RegisDateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(criteria.RegisDateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (end < start)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Register Date From > Register Date To");
                        return;
                    }

                }

                ServiceTestDriveService srv = new ServiceTestDriveService();
                gvCust.DataSource = srv.GetServiceTestDrive(criteria);
                gvCust.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindCriDealer()
        {
            try
            {
                ServiceTestDriveService srv = new ServiceTestDriveService();
                dlcriDealer.DataSource = srv.GetDealerMaster(true);
                dlcriDealer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindCriPreferredModel()
        {
            try
            {
                ServiceTestDriveService srv = new ServiceTestDriveService();
                dlcriPreferredModel.DataSource = srv.GetPreferredModel(true);
                dlcriPreferredModel.DataBind();
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
                ClientPopup(ModalType.Error, "Cannot change. Please try again later.");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                string user = Session["User"].ToString();
                lbRequiredCancel.Visible = false;
                upCancel.Update();
                lbRequiredCancel.Visible = false;
                upCancel.Update();
                ServiceTestDriveService srv = new ServiceTestDriveService();
                #region CheckCancel Required
                //TODO Validate Required
                if (chkCancel.Checked)
                {
                    if (tbCancelDate.Text == "" || tbCancelReason.Text == "")
                    {
                        lbRequiredCancel.Visible = true;
                        upCancel.Update();
                        return;
                    }
                }
                else
                {
                    lbRequiredCancel.Visible = false;
                    upCancel.Update();
                }
                #endregion             
                TestDriveModel testDrive = new TestDriveModel()
                {
                    Id = lbId.Text,
                    Name = tbFirstname.Text,
                    Surname = tbSurname.Text,
                    MobileNumber = tbMobile.Text,
                    Email = tbEmail.Text,
                    PreferredModelId = dlPreferredModel.SelectedValue,
                    DealerId = dlDealer.SelectedValue,
                    PurchasePlanId = dlPerchasePlan.SelectedValue,
                    ConfirmDate = tbConfirmDate.Text,
                    ConfirmTime = tbConfirmTime.Text,
                    IsCancel = chkCancel.Checked ? "1" : "0",
                    CancelDate = chkCancel.Checked ? tbCancelDate.Text : "",
                    CancelReason = chkCancel.Checked ? tbCancelReason.Text : "",
                    StatusId = dlStatus.SelectedValue,
                    CallCenterRemark = tbCallCenterRemark.Text
                };
                
                if (lbType.Text == "Edit")
                {
                    srv.UpdateTestDrive(testDrive, user);

                    if (oldStatusID.Value != testDrive.StatusId)
                    {
                        TreasureDataController treasureDataController = new TreasureDataController();
                        treasureDataController.AddEventTestDrive(Convert.ToInt32(testDrive.Id));
                    }

                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                oldStatusID.Value = null;
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot save. Please try again later.");
            }
        }
        private void ClientPopup(ModalType type, string message)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
            upModalAdd.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdWarning').modal('hide');", true);
            upModalWarning.Update();

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
        protected void gvCust_RowCommand(object sender, GridViewCommandEventArgs e)
        {

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
        protected void gvCust_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
            if (lnkFull != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
            }
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;");
            }
        }
        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lbType.Text = "Edit";
                lbTestDriveCode.Text = "";
                lbRegisterDate.Text = "";
                lbMemberID.Text = "";
                tbFirstname.Text = "";
                tbSurname.Text = "";
                tbMobile.Text = "";
                tbEmail.Text = "";
                dlPreferredModel.SelectedIndex = 0;
                dlDealer.SelectedIndex = 0;
                dlPerchasePlan.SelectedIndex = 0;
                tbConfirmDate.Text = "";
                tbConfirmTime.Text = "";
                chkCancel.Checked = false;
                tbCancelDate.Text = "";
                tbCancelReason.Text = "";
                dlStatus.SelectedIndex = 0;
                tbCallCenterRemark.Text = "";
                lbRequiredCancel.Visible = false;
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();

                    lbId.Text = id;                   
                    ServiceTestDriveService srv = new ServiceTestDriveService();
                    DataRow row = srv.GetTestDriveById(id);
                    if (row != null)
                    {
                        lbTestDriveCode.Text = row["CODE"].ToString();
                        lbRegisterDate.Text = (row["CREATED_DATE"] != DBNull.Value) ? ((DateTime)row["CREATED_DATE"]).ToString("dd MMM yyyy HH:mm") : "";
                        lbMemberID.Text = row["MEMBER_ID"].ToString();
                        tbFirstname.Text = row["FIRSTNAME"].ToString();
                        tbSurname.Text = row["SURNAME"].ToString();
                        tbMobile.Text = row["MOBILE_NUMBER"].ToString();
                        tbEmail.Text = row["EMAIL"].ToString();

                        if (dlPreferredModel.Items.FindByValue(row["PREFERRED_MODEL_ID"].ToString()) != null)
                        {
                            dlPreferredModel.SelectedValue = row["PREFERRED_MODEL_ID"].ToString();
                        }
                        if (dlDealer.Items.FindByValue(row["DEALER_ID"].ToString()) != null)
                        {
                            lbNotSet.Visible = false;
                            dlDealer.SelectedValue = row["DEALER_ID"].ToString();
                        }
                        else
                        {
                            lbNotSet.Visible = true;
                            BindDealer();
                        }
                        if (dlPerchasePlan.Items.FindByValue(row["PURCHASE_PLAN_ID"].ToString()) != null)
                        {
                            dlPerchasePlan.SelectedValue = row["PURCHASE_PLAN_ID"].ToString();
                        }
            
                        if (row["CONFIRMED_DATE"] != DBNull.Value)
                        {
                            tbConfirmDate.Text = ((DateTime)row["CONFIRMED_DATE"]).ToString("dd/MM/yyyy");
                        }
                        if (row["CONFIRMED_TIME"] != DBNull.Value)
                        {
                            tbConfirmTime.Text = row["CONFIRMED_TIME"].ToString().Substring(0, 5);
                        }
                        chkCancel.Checked = (row["IS_CANCEL"] != DBNull.Value && row["IS_CANCEL"].ToString().ToLower().Equals("true")) ? true : false;
                        if (row["CANCEL_DATE"] != DBNull.Value)
                        {
                            tbCancelDate.Text = ((DateTime)row["CANCEL_DATE"]).ToString("dd/MM/yyyy");
                        }
                        tbCancelReason.Text = row["CANCEL_REASON"].ToString();

                        oldStatusID.Value = row["STATUS_ID"].ToString();
                        if (dlStatus.Items.FindByValue(row["STATUS_ID"].ToString()) != null)
                        {
                            dlStatus.SelectedValue = row["STATUS_ID"].ToString();
                        }
                        tbCallCenterRemark.Text = row["CALL_CENTER_REMARK"].ToString();

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);


                        upModalAdd.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot edit. Please try again later.");
            }
        }

        
        private void BindDealer()
        {
            try
            {
                ServiceTestDriveService srv = new ServiceTestDriveService();
                dlDealer.DataSource = srv.GetDealerMaster(false);
                dlDealer.DataBind();
                //OnlineBookingService srv = new OnlineBookingService();
                //dlDealer.DataSource = srv.GetDealer();
                //dlDealer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindPreferredModel()
        {
            try
            {
                ServiceTestDriveService srv = new ServiceTestDriveService();
                dlPreferredModel.DataSource = srv.GetPreferredModel(false);
                dlPreferredModel.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindPurchasePlan()
        {
            try
            {
                ServiceTestDriveService srv = new ServiceTestDriveService();
                dlPerchasePlan.DataSource = srv.GetPurchasePlan();
                dlPerchasePlan.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        protected void chkCancel_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {

                string filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\TestDrive" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ServiceTestDriveService srv = new ServiceTestDriveService();
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                    ServiceTestDriveCriteria criteria = new ServiceTestDriveCriteria()
                    {
                        DealerId = dlcriDealer.SelectedValue,
                        PreferredModel = dlcriPreferredModel.SelectedValue,
                        TestDriveCode = tbcriTestDriveCode.Text,
                        Name = tbcriName.Text,
                        Surname = tbcriSurname.Text,
                        MobileNumber = tbcriMobileNumber.Text,
                        Status = dlcriStatus.SelectedValue,
                        RegisDateFrom = tbcriRegisterDateFrom.Text,
                        RegisDateTo = tbcriRegisterDateTo.Text,
                      
                    };
                    if (!string.IsNullOrWhiteSpace(criteria.RegisDateFrom) && !string.IsNullOrWhiteSpace(criteria.RegisDateTo))
                    {
                        DateTime start = DateTime.ParseExact(criteria.RegisDateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime end = DateTime.ParseExact(criteria.RegisDateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (end < start)
                        {
                            ClientPopup(ModalType.Warning, "Please Choose Register Date From > Register Date To");
                            return;
                        }

                    }
                    ws.Cells["A1"].LoadFromDataTable(srv.GetServiceTestDriveExport(criteria), true);
                    ws.Column(3).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(13).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(14).Style.Numberformat.Format = "HH:mm";
                    ws.Column(15).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(17).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";                   
                    pck.Save();
                }

                FileInfo file = new FileInfo(filePath);
                byte[] fileConent = File.ReadAllBytes(filePath);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
                HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.BinaryWrite(fileConent);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
            upModalAdd.Update();
            oldStatusID.Value = null;
        }
    }
}