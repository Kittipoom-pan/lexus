using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web.pages
{
    public partial class PageInitialData : System.Web.UI.Page
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
                    else
                    {
                        //ViewState["User"] = "ADMIN";
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();

                        

                        tbSearchFrom.Enabled = chkSearchRSDate.Checked;
                        tbSearchTo.Enabled = chkSearchRSDate.Checked;
                        //dlReason.Enabled = chkInActive.Checked;
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
                tbSearchFrom.Enabled = chkSearchRSDate.Checked;
                tbSearchTo.Enabled = chkSearchRSDate.Checked;
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
                InitialService srv = new InitialService();
                using (DataTable dt = srv.GetInitialSearch(txtSearch.Text,
                        chkSearchCitizenId.Checked,
                        chkSearchFname.Checked,
                        chkSearchEmail.Checked,
                        chkSearchVin.Checked,
                        chkSearchRSDate.Checked,
                        tbSearchFrom.Text,
                        tbSearchTo.Text,
                        Session["Role"].ToString()))
                {
                    gvCust.DataSource = dt;
                    gvCust.DataBind();
                }
                //if (Session["Role"].ToString() == "3" || Session["Role"].ToString() == "4")
                //{
                //    DataTable seller = null;

                //    if (Session["Role"].ToString() == "3")
                //    {
                //        seller = srv.GetSeller(Session["Dealer"].ToString());
                //    }
                //    else if (Session["Role"].ToString() == "4")
                //    {
                //        seller = srv.GetSeller(null);
                //        var row = seller.AsEnumerable().Where(p => p["SELLERID"].ToString() == Session["Seller"].ToString());
                //        if (row.Any())
                //        {
                //            seller = row.CopyToDataTable();
                //        }
                //        else
                //        {
                //            seller.Rows.Clear();
                //        }
                //        row = null;
                //    }

                //    DataTable dt = srv.GetCustomerSearch(txtSearch.Text,
                //        chkSearchMemberId.Checked,
                //        chkSearchFname.Checked,
                //        chkSearchMobile.Checked,
                //        chkSearchVin.Checked,
                //        chkSearchCreateDate.Checked,
                //        tbSearchFrom.Text,
                //        tbSearchTo.Text,
                //        Session["Role"].ToString());

                //    DataTable dt2 = srv.GetAppSearch(txtSearch.Text,
                //        chkSearchMemberId.Checked,
                //        chkSearchFname.Checked,
                //        chkSearchMobile.Checked,
                //        chkSearchVin.Checked,
                //        chkSearchCreateDate.Checked,
                //        tbSearchFrom.Text,
                //        tbSearchTo.Text,
                //        Session["Role"].ToString());

                //    bool isExistCustomer = false;
                //    FillterExistingCustomer(ref dt, ref isExistCustomer, seller);

                //    gvCust.DataSource = dt;
                //    gvCust.DataBind();
                //    BindGridDataCar(dt.AsEnumerable().Select(p => p["MEMBERID"].ToString()).Distinct().ToList(), seller);
                //    gvAppUser.DataSource = dt2;
                //    gvAppUser.DataBind();


                //    if (isExistCustomer)
                //    {
                //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdExistCust", "$('#mdExistCust').modal();", true);
                //        upMdExistCust.Update();
                //    }

                //    dt = null;
                //    seller = null;
                //}
                //else
                //{

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {
                InitialService srv = new InitialService();
                dlModel.Items.Clear();
                dlModel.DataSource = srv.GetCarModels();
                dlModel.DataBind();

                dlDealer.Items.Clear();
                dlDealer.DataSource = srv.GetDealer();
                dlDealer.DataBind();

                dlTitle.SelectedIndex = -1;
                tbFirstname.Text = "";
                tbLastname.Text = "";
                dlGener.SelectedIndex = -1;
                dlModel.SelectedIndex = -1;
                dlDealer.SelectedIndex = -1;
                tbBirthdate.Text = "";
                tbSSN.Text = "";
                tbEmail.Text = "";
                tbVin.Text = "";
                tbPlateNo1.Text = "";
                tbPlateNo2.Text = "";
                tbRSDate.Text = "";
                lbType.Text = "Add";
                //if (Session["Role"].ToString() != "4")
                //{
                //    dlSellerID.SelectedIndex = -1;
                //}            
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                upModalAdd.Update();
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
        protected void gvCust_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelId.Text = "";

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
        protected void gvCust_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
                if (lnkFull != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton _btnEditCust = (LinkButton)e.Row.FindControl("gvBtnEdit");
                    LinkButton _btnDelete = (LinkButton)e.Row.FindControl("gvBtnDelete");

                    if (Session["Role"].ToString() == "4")
                    {
                        _btnEditCust.Visible = false;
                        _btnDelete.Visible = false;
                    }
                }
            }
            catch
            {
            }
        }
        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbId.Text = "";

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["id"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    InitialService srv = new InitialService();
                    DataRow row = srv.GetInitialById(id);
                    if (row != null)
                    {
                        tbFirstname.Text = row["firstname"].ToString();
                        tbLastname.Text = row["lastname"].ToString();
                        dlGener.SelectedValue = row["gender"].ToString();
                        tbBirthdate.Text = (row["birthday"] == DBNull.Value) ? "" : Convert.ToDateTime(row["birthday"]).ToString("dd/MM/yyyy");
                        tbSSN.Text = row["citizen_id"].ToString();
                        tbEmail.Text = row["email"].ToString();
                        //tbDealer.Text = row["dealer"].ToString();
                        tbVin.Text = row["vin"].ToString();

                        string plate_no = row["plate_no"].ToString();
                        string[] data_auth = plate_no.Split('-');
                        if (data_auth.Length > 1)
                        {
                            tbPlateNo1.Text = data_auth[0];
                            tbPlateNo2.Text = data_auth[1];
                        }
                        //tbModel.Text = row["model"].ToString();
                        //tbColor.Text = row["color"].ToString();

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
        protected void gvBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbDelId.Text = "";
                tbReasonDelCust.Text = "";

                if (gvCust.DataKeys != null)
                {
                    lbDelId.Text = gvCust.DataKeys[index].Values["id"].ToString();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal();", true);
                    upModelDel.Update();
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
                string id = lbId.Text;
                string titleName = dlTitle.SelectedValue;
                string firstName = tbFirstname.Text;
                string lastName = tbLastname.Text;
                string gender = dlGener.SelectedValue;
                string birthdate = tbBirthdate.Text;
                string citizen_id = tbSSN.Text;
                string email = tbEmail.Text;
                string dealer = dlDealer.SelectedValue;
                string vin = tbVin.Text.ToUpper();
                string plate_no = tbPlateNo1.Text + "-" + tbPlateNo2.Text;
                string model = dlModel.SelectedValue;
                string rs_date = tbRSDate.Text;

                DateTime RSDate = DateTime.ParseExact(rs_date, "dd/MM/yyyy", null);
                DateTime BirthDate = DateTime.ParseExact(birthdate, "dd/MM/yyyy", null);
                DateTime LowDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                DateTime DateNow = DateTime.Now;
                if (RSDate > DateNow)
                {
                    ClientPopup(ModalType.Warning, "Please Choose Delivery Date < Today");
                }
                else if (BirthDate > DateNow)
                {
                    ClientPopup(ModalType.Warning, "Please Choose Birthday Date < Today");
                }
                else if (BirthDate < LowDate)
                {
                    ClientPopup(ModalType.Warning, "Please Choose Birthday Date > 01/01/1900");
                }
                else
                {
                    InitialService srv = new InitialService();
                    if (lbType.Text == "Add")
                    {
                        string idInsert = srv.AddInitial(titleName, firstName, lastName, gender, birthdate, citizen_id, email, dealer, vin, plate_no, model, rs_date, Session["User"].ToString());
                        BindGridData();
                        ClientPopup(ModalType.Success, "Completed");
                    }
                    else if (lbType.Text == "Edit")
                    {
                        srv.UpdateInitial(id, firstName, lastName, gender, birthdate, citizen_id, email, dealer, vin, plate_no, model, rs_date, Session["User"].ToString());
                        BindGridData();
                        ClientPopup(ModalType.Success, "Completed");
                    }
                }
                
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbDelId.Text != "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
                    upModelDel.Update();

                    string id = lbDelId.Text;

                    //CustomerService srv = new CustomerService();
                    //srv.DeleteCustomer(id, tbReasonDelCust.Text);

                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else
                {
                    BindGridData();
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
        private void ClientPopup(ModalType type, string message)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal('hide');", true);
            upModalAdd.Update();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdWarning", "$('#mdWarning').modal('hide');", true);
            //upModalWarning.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal('hide');", true);
            upModelDel.Update();

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

        protected void cvVin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (tbVin.Text.Length > 0)
                {
                    InitialService srv = new InitialService();
                    if (lbType.Text == "Add")
                    {
                        args.IsValid = !srv.IsExistsVin(tbVin.Text);
                    }
                    //else
                    //{
                    //    if (srv.GetLastMobile(lbId.Text) != tbVin.Text)
                    //    {
                    //        args.IsValid = !srv.IsExistsMobile(tbVin.Text);
                    //    }
                    //    else
                    //    {
                    //        args.IsValid = true;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnDownload_ServerClick(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(WebConfigurationManager.AppSettings["TemplatePath"] + "\\InitialData_Template.xls");
            byte[] fileConent = File.ReadAllBytes(WebConfigurationManager.AppSettings["TemplatePath"] + "\\InitialData_Template.xls");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(fileConent);
            HttpContext.Current.Response.End();
        }
    }
}