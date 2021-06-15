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
    public partial class PageServiceAppointment : System.Web.UI.Page
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

                        BindPickupTime();
                        BindCriTypeOfService();
                        BindCriDealer();
                        BindDealer();

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

                ServiceAppointmentCriteria criteria = new ServiceAppointmentCriteria()
                {
                    DealerId = dlcriDealer.SelectedValue,
                    TypeOfServiceId = dlcriTypeOfService.SelectedValue,
                    AppointmentCode = tbcriAppointmentCode.Text,
                    Name = tbcriName.Text,
                    Surname = tbcriSurname.Text,
                    MobileNumber = tbcriMobileNumber.Text,
                    Status = dlcriStatus.SelectedValue,
                    RegisDateFrom = tbcriRegisterDateFrom.Text,
                    RegisDateTo = tbcriRegisterDateTo.Text,
                    AppointmentDateFrom = tbcriAppointmentDateFrom.Text,
                    AppointmentDateTo = tbcriAppointmentDateTo.Text,
                    IsPickupService = dlcriIsPickService.SelectedValue
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
                if (!string.IsNullOrWhiteSpace(criteria.AppointmentDateFrom) && !string.IsNullOrWhiteSpace(criteria.AppointmentDateTo))
                {
                    DateTime start = DateTime.ParseExact(criteria.AppointmentDateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(criteria.AppointmentDateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (end < start)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Appointment Date From > Appointment Date To");
                        return;
                    }

                }
                ServiceAppointmentService srv = new ServiceAppointmentService();
                gvCust.DataSource = srv.GetServiceAppointment(criteria);
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
                ServiceAppointmentService srv = new ServiceAppointmentService();
                dlcriDealer.DataSource = srv.GetDealerMaster(true);
                dlcriDealer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindCriTypeOfService()
        {
            try
            {
                ServiceAppointmentService srv = new ServiceAppointmentService();
                dlcriTypeOfService.DataSource = srv.GetTypeOfServiceMaster();
                dlcriTypeOfService.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindPickupTime()
        {
            try
            {
                ServiceAppointmentService srv = new ServiceAppointmentService();
                dlPickupTime.DataSource = srv.GetPickupTimes();
                dlPickupTime.DataBind();
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
                ServiceAppointmentService srv = new ServiceAppointmentService();
                #region CheckTypeOfService Required
                bool isCheckSomeRow = false;
                List<AppointmentTypeOfServiceDetailModel> appServiceDetails = new List<AppointmentTypeOfServiceDetailModel>();
                foreach (RepeaterItem i in RPTypeOfService.Items)
                {

                    AppointmentTypeOfServiceDetailModel detail = new AppointmentTypeOfServiceDetailModel();
                    Label lbTypeOfServiceId = i.FindControl("lbTypeOfServiceID") as Label;
                    Label lbName = i.FindControl("lbTypeOfServiceName") as Label;
                    CheckBox chk = i.FindControl("chkTypeOfService") as CheckBox;
                    DropDownList dlServiceDetail = i.FindControl("dlTypeOfServiceDetail") as DropDownList;
                    detail.AppointmentId = lbId.Text;
                    detail.TypeOfServiceId = lbTypeOfServiceId.Text;
                    if (chk.Checked)
                    {
                        isCheckSomeRow = true;
                        if (dlServiceDetail.Items.Count > 0)
                        {
                            if (dlServiceDetail.SelectedValue == "0")
                            {
                                if (dlStatus.SelectedValue == "3")
                                {
                                    detail.TypeOfServiceDetailId = "0";
                                }
                                else
                                {
                                    lbTypeOfserviceRequired.Visible = true;
                                    upRPTypeOfService.Update();
                                    return;
                                }
                            }
                            else
                            {
                                detail.TypeOfServiceDetailId = dlServiceDetail.SelectedValue;
                            }
                        }
                        appServiceDetails.Add(detail);
                    }

                }
                if (!isCheckSomeRow)
                {
                    lbTypeOfserviceRequired.Visible = true;
                    upModalAdd.Update();
                    return;
                }
                #endregion
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
                string confirmDate = tbConfirmDate.Text;
                string cancelDate = tbCancelDate.Text;
                string pickupDate = tbPickupDate.Text;
                if (!string.IsNullOrEmpty(confirmDate))
                {
                    DateTime date = DateTime.ParseExact(confirmDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(cancelDate))
                {
                    DateTime date = DateTime.ParseExact(cancelDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(pickupDate))
                {
                    DateTime date = DateTime.ParseExact(pickupDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                AppointmentModel appointment = new AppointmentModel()
                {
                    Id = lbId.Text,
                    Name = tbFirstname.Text,
                    Surname = tbSurname.Text,
                    MobileNumber = tbMobile.Text,
                    PlateNo = dlPlateNo.SelectedValue,
                    DealerId = dlDealer.SelectedValue,
                    ConfirmDate = confirmDate,
                    ConfirmTime = tbConfirmTime.Text,
                    IsCancel = chkCancel.Checked ? "1" : "0",
                    CancelDate = chkCancel.Checked ? cancelDate : "",
                    CancelReason = chkCancel.Checked ? tbCancelReason.Text : "",
                    StatusId = dlStatus.SelectedValue,
                    CallCenterRemark = tbCallCenterRemark.Text,
                    VehicleNo = hdVIN.Value,
                    PickupAddress = tbPickupAddress.Text,
                    PickupLocationDetail = tbPickupLocationDetail.Text,
                    PickupDate = tbPickupDate.Text,
                    PickupTimeId = (dlPickupTime.SelectedValue == "0")? "": dlPickupTime.SelectedValue,
                };

                if (tbPickupLocation.Text.Trim() != string.Empty)
                {
                    var location = tbPickupLocation.Text.Split(',').ToArray();
                    appointment.PickupLatitude = location[0];
                    appointment.PickupLongitude = location[1];
                }
                else
                {
                    appointment.PickupLatitude = "";
                    appointment.PickupLongitude = "";
                }

                if (lbType.Text == "Edit")
                {
                    srv.UpdateAppointment(appointment, appServiceDetails, user);

                    if (oldStatusID.Value != appointment.StatusId)
                    {
                        TreasureDataController treasureDataController = new TreasureDataController();
                        treasureDataController.AddEventAppointment(Convert.ToInt32(appointment.Id));
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
                lbAppontmentCode.Text = "";
                lbRegisterDate.Text = "";
                lbMemberID.Text = "";
                tbFirstname.Text = "";
                tbSurname.Text = "";
                tbMobile.Text = "";
                lbCarModel.Text = "";
                dlPlateNo.SelectedIndex = -1;
                dlDealer.SelectedIndex = -1;
                hdVIN.Value = "";
                lbRemark.Text = "";
                lbRegisterDate.Text = "";
                tbConfirmDate.Text = "";
                tbConfirmTime.Text = "";
                chkCancel.Checked = false;
                tbCancelDate.Text = "";
                tbCancelReason.Text = "";
                dlStatus.SelectedIndex = 0;
                tbCallCenterRemark.Text = "";
                lbTypeOfserviceRequired.Visible = false;
                lbRequiredCancel.Visible = false;
                lbIsPickUpService.Text = "";
                tbPickupLocation.Text = "";
                tbPickupAddress.Text = "";
                tbPickupAddress.Text = "";
                tbPickupDate.Text = "";
                dlPickupTime.SelectedIndex = -1;
                pnPickup.Visible = false;
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();

                    lbId.Text = id;
                    BindRPTypeOfServiceData();
                    ServiceAppointmentService srv = new ServiceAppointmentService();
                    DataRow row = srv.GetServiceAppointmentById(id);
                    if (row != null)
                    {
                        lbAppontmentCode.Text = row["CODE"].ToString();
                        lbRegisterDate.Text = (row["CREATED_DATE"] != DBNull.Value) ? ((DateTime)row["CREATED_DATE"]).ToString("dd MMM yyyy HH:mm") : "";
                        lbMemberID.Text = row["MEMBER_ID"].ToString();
                        tbFirstname.Text = row["NAME"].ToString();
                        tbSurname.Text = row["SURNAME"].ToString();
                        tbMobile.Text = row["MOBILE_NUMBER"].ToString();
                        lbCarModel.Text = row["MODEL_NAME"].ToString();
                        hdVIN.Value = row["VEHICLE_NO"].ToString();

                        BindPlateNo(lbMemberID.Text);
                        if (dlPlateNo.Items.FindByValue(row["PLATE_NUMBER"].ToString()) != null)
                        {
                            dlPlateNo.SelectedValue = row["PLATE_NUMBER"].ToString();
                        }
                        if (dlDealer.Items.FindByValue(row["DEALER_ID"].ToString()) != null)
                        {
                            dlDealer.SelectedValue = row["DEALER_ID"].ToString();
                        }

                        lbRemark.Text = row["REMARK"].ToString();

                        if (row["APPOINTMENTDATE"] != DBNull.Value)
                        {
                            lbAppointDate.Text = ((DateTime)row["APPOINTMENTDATE"]).ToString("dd MMM yyyy");
                        }
                        if (row["APPOINTMENTTIME"] != DBNull.Value)
                        {
                            TimeSpan time = ((TimeSpan)row["APPOINTMENTTIME"]);
                            lbAppointDate.Text += string.Format(" {0}:{1}", time.Hours.ToString("00"), time.Minutes.ToString("00"));
                        }
                        if (row["CONFIRM_DATE"] != DBNull.Value)
                        {
                            tbConfirmDate.Text = ((DateTime)row["CONFIRM_DATE"]).ToString("dd/MM/yyyy");
                        }
                        if (row["CONFIRM_TIME"] != DBNull.Value)
                        {
                            tbConfirmTime.Text = row["CONFIRM_TIME"].ToString().Substring(0, 5);
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
                        bool isPickupService = bool.Parse(row["IS_PICKUP_SERVICE"].ToString());
                        lbIsPickUpService.Text =isPickupService  ? "Yes" : "No";
                        pnPickup.Visible = isPickupService;

                        if (row["latitude"].ToString() != "" || row["longitude"].ToString() != "")
                        {
                            if (row["latitude"].ToString() == "0" && row["longitude"].ToString() == "0")
                            {
                                tbPickupLocation.Text = "";
                            }
                            else
                            {
                                tbPickupLocation.Text = row["latitude"].ToString() + "," + row["longitude"].ToString();
                            }
                        }
                        else
                        {
                            tbPickupLocation.Text = "";
                        }
                        tbPickupAddress.Text = row["PICKUP_ADDRESS"].ToString();
                        tbPickupLocationDetail.Text = row["LOCATION_DETAIL"].ToString();

                        if (row["PICKUP_DATE"] != DBNull.Value)
                        {
                            tbPickupDate.Text = ((DateTime)row["PICKUP_DATE"]).ToString("dd/MM/yyyy");
                        }
                        if (row["PICKUP_TIME_ID"] != DBNull.Value)
                        {
                            dlPickupTime.SelectedValue = row["PICKUP_TIME_ID"].ToString();
                        }


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
        private void BindRPTypeOfServiceData()
        {
            try
            {
                ServiceAppointmentService srv = new ServiceAppointmentService();
                dtTypeOfServiceDetail = srv.GetRPTypeOfServiceMaster();
                RPTypeOfService.DataSource = dtTypeOfServiceDetail;
                dtAppointmentTypeOfService = srv.GetServiceAppointmentTypeOfServiceByAppointmentId(lbId.Text);
                Session["AppointmentDetail"] = dtAppointmentTypeOfService;
                RPTypeOfService.DataBind();
                upRPTypeOfService.Update();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindDealer()
        {
            try
            {
                ServiceAppointmentService srv = new ServiceAppointmentService();
                dlDealer.DataSource = srv.GetDealerMaster(false);
                dlDealer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindPlateNo(string memberId)
        {
            try
            {
                ServiceAppointmentService srv = new ServiceAppointmentService();
                dlPlateNo.DataSource = srv.GetPlateNoByMemberId(memberId);
                dlPlateNo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void RPTypeOfService_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (lbType.Text == "Edit")
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView d = (DataRowView)e.Item.DataItem;
                    Label lbTypeOfServiceId = e.Item.FindControl("lbTypeOfServiceID") as Label;
                    Label lbName = e.Item.FindControl("lbTypeOfServiceName") as Label;
                    CheckBox chk = e.Item.FindControl("chkTypeOfService") as CheckBox;
                    DropDownList dlServiceDetail = e.Item.FindControl("dlTypeOfServiceDetail") as DropDownList;
                    Button btnCheck = e.Item.FindControl("btnCheck") as Button;

                    lbName.Text = d["NAME_EN"].ToString();
                    lbTypeOfServiceId.Text = d["ID"].ToString();
                    bool hasChild = Convert.ToInt32(d["HAS_CHILD"]) > 0;
                    if (hasChild)
                    {
                        // ((RequiredFieldValidator)e.Item.FindControl("rfTypeOfService")).ControlToValidate = ((DropDownList)e.Item.FindControl("dlTypeOfServiceDetail")).UniqueID;                 
                        DataTable serviceDetail = new ServiceAppointmentService().GetTypeOfServiceDetailMasterByTypeOfServiceId(lbTypeOfServiceId.Text);
                        dlServiceDetail.DataSource = serviceDetail;
                        dlServiceDetail.DataBind();
                    }
                    ServiceAppointmentService srv = new ServiceAppointmentService();

                    //var drAppointmentTypeOfService = srv.GetServiceAppointmentTypeOfServiceByAppointmentIdAndTypeId(lbId.Text, lbTypeOfServiceId.Text);
                    dlServiceDetail.Visible = false;
                    var data = ((DataTable)Session["AppointmentDetail"]).AsEnumerable().Where(c => c.Field<int>("TYPE_OF_SERVICE_ID").ToString() == lbTypeOfServiceId.Text).FirstOrDefault();
                    var drAppointmentTypeOfService = (data != null) ? (DataRow)data : null;
                    if (drAppointmentTypeOfService != null && hasChild)
                    {
                        chk.Checked = true;
                        //btnCheck.CssClass = "btn btn-success";
                        btnCheck.Text = "☑";
                        // btnCheck.ImageUrl = "../images/checkbox-check.jpg";

                        if (dlServiceDetail.Items.FindByValue(drAppointmentTypeOfService["TYPE_OF_SERVICE_DETAIL_ID"].ToString()) != null)
                        {
                            dlServiceDetail.SelectedValue = drAppointmentTypeOfService["TYPE_OF_SERVICE_DETAIL_ID"].ToString();
                        }
                        dlServiceDetail.Visible = true;
                    }
                    else if (drAppointmentTypeOfService != null && !hasChild)
                    {
                        chk.Checked = true;
                        //btnCheck.CssClass = "btn btn-success";
                        btnCheck.Text = "☑";
                    }
                    else
                    {
                        chk.Checked = false;
                        //btnCheck.CssClass = "btn btn-danger";
                        btnCheck.Text = "☐";
                        //  btnCheck.ImageUrl = "../images/checkbox-notcheck.jpg";  
                    }
                    upRPTypeOfService.Update();
                }

            }

        }

        protected void RPTypeOfService_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbTypeOfServiceId = e.Item.FindControl("lbTypeOfServiceID") as Label;
                Label lbName = e.Item.FindControl("lbTypeOfServiceName") as Label;
                CheckBox chk = e.Item.FindControl("chkTypeOfService") as CheckBox;
                DropDownList dlServiceDetail = e.Item.FindControl("dlTypeOfServiceDetail") as DropDownList;
                Button btnCheck = e.Item.FindControl("btnCheck") as Button;
                if (chk.Checked)
                {

                    // btnCheck.ImageUrl = "../images/checkbox-notcheck.jpg";
                    //btnCheck.CssClass = "btn btn-danger";
                    btnCheck.Text = "☐";
                    chk.Checked = false;
                    dlServiceDetail.Visible = false;
                }
                else
                {
                    // btnCheck.ImageUrl = "../images/checkbox-check.jpg";
                    //btnCheck.CssClass = "btn btn-success";
                    btnCheck.Text = "☑";
                    chk.Checked = true;
                    if (dlServiceDetail.Items.Count > 0)
                    {
                        dlServiceDetail.Visible = true;
                    }
                }
                upRPTypeOfService.Update();

            }
        }

        protected void chkCancel_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {

                string filePath = System.Web.Configuration.WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\ServiceAppointment" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ServiceAppointmentService srv = new ServiceAppointmentService();
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

                    ServiceAppointmentCriteria criteria = new ServiceAppointmentCriteria()
                    {
                        DealerId = dlcriDealer.SelectedValue,
                        TypeOfServiceId = dlcriTypeOfService.SelectedValue,
                        AppointmentCode = tbcriAppointmentCode.Text,
                        Name = tbcriName.Text,
                        Surname = tbcriSurname.Text,
                        MobileNumber = tbcriMobileNumber.Text,
                        Status = dlcriStatus.SelectedValue,
                        RegisDateFrom = tbcriRegisterDateFrom.Text,
                        RegisDateTo = tbcriRegisterDateTo.Text,
                        AppointmentDateFrom = tbcriAppointmentDateFrom.Text,
                        AppointmentDateTo = tbcriAppointmentDateTo.Text,
                        IsPickupService = dlcriIsPickService.SelectedValue
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
                    if (!string.IsNullOrWhiteSpace(criteria.AppointmentDateFrom) && !string.IsNullOrWhiteSpace(criteria.AppointmentDateTo))
                    {
                        DateTime start = DateTime.ParseExact(criteria.AppointmentDateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime end = DateTime.ParseExact(criteria.AppointmentDateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (end < start)
                        {
                            ClientPopup(ModalType.Warning, "Please Choose Appointment Date From > Appointment Date To");
                            return;
                        }

                    }
                    ws.Cells["A1"].LoadFromDataTable(srv.GetServiceAppointmentExport(criteria), true);
                    ws.Column(3).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(14).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(15).Style.Numberformat.Format = "HH:mm";
                    ws.Column(16).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(18).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(19).Style.Numberformat.Format = "HH:mm";
                    ws.Column(20).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(27).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Column(28).Style.Numberformat.Format = "HH:mm";
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
            upModalAdd.Update();
            oldStatusID.Value = null;
        }

        protected void dlPlateNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                ServiceAppointmentService srv = new ServiceAppointmentService();
                DataRow dr = srv.GetCarModelAndVinByPlateNo(dlPlateNo.SelectedValue);
                if (dr != null)
                {
                    lbCarModel.Text = dr["MODEL_NAME"] != DBNull.Value ? dr["MODEL_NAME"].ToString() : "";
                    hdVIN.Value = dr["VIN"] != DBNull.Value ? dr["VIN"].ToString() : "";
                }
                else
                {
                    lbCarModel.Text = "";
                    hdVIN.Value = "";
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbConfirmDate').datepicker({ changeMonth: true, changeYear: true, format: 'dd/mm/yyyy', language: 'en' }).on('changeDate', function (ev) { $(this).blur(); $(this).datepicker('hide'); });$('#ContentPlaceHolder1_tbCancelDate').datepicker({ changeMonth: true, changeYear: true, format: 'dd/mm/yyyy', language: 'en' }).on('changeDate', function(ev) { $(this).blur(); $(this).datepicker('hide'); }); ", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
    }
}