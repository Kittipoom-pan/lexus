using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace com.lexus.th.web.pages
{
    public partial class PageDealer : System.Web.UI.Page
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
                        gvDealer.DataSource = new System.Data.DataTable();
                        gvDealer.DataBind();

                        BindGeo();

                        dlGeo.SelectedIndex = 1;
                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void RegisterPostBackControl()
        {
            foreach (GridViewRow row in gvDealer.Rows)
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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdWarning').modal('hide');", true);
            upModalWarning.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
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
        protected void gvDealer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
            if (lnkFull != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
            }
        }
        protected void gvDealer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvDealer.DataKeys != null)
                    {
                        lbDelId.Text = gvDealer.DataKeys[index].Values["ID"].ToString();
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
        protected void gvDealer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDealer.PageIndex = e.NewPageIndex;
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvDealer.SelectedIndex;
                if (gvDealer.DataKeys != null)
                {
                    string id = gvDealer.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {
                tbDealerName.Text = "";
                tbDealerCode.Text = "";
                tbBranchName.Text = "";
                tbBranchCode.Text = "";
                tbAddress.Text = "";
                tbMobile.Text = "";
                tbOffice_hours.Text = "";
                tbOffice_hours2.Text = "";
                dlGeo2.SelectedIndex = 1;
                dlActive.SelectedIndex = -1;

                lbType.Text = "Add";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                upModalAdd.Update();
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
                string dealer_name = tbDealerName.Text;
                string dealer_code = tbDealerCode.Text;
                string branch_name = tbBranchName.Text;
                string branch_code = tbBranchCode.Text;
                string address = tbAddress.Text;
                string mobile = tbMobile.Text;
                string office_hours = tbOffice_hours.Text;
                string office_hours2 = tbOffice_hours2.Text;
                string geo_id = dlGeo2.SelectedValue;
                string active = dlActive.SelectedValue;

                string user = Session["User"].ToString();

                DealerService srv = new DealerService();
                if (lbType.Text == "Add")
                {
                    srv.AddDealer(geo_id, dealer_name, dealer_code, branch_name, branch_code, address, office_hours, office_hours2, mobile, active, user, "", "","", "", "", "", "","", "", "0", "0", null,null,null,null, "");
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateDealer(geo_id, dealer_name, dealer_code, branch_name, branch_code, address, office_hours, office_hours2, mobile, active, user, lbId.Text, "","", "", "", "", "", "", "", "","0","0",null,null,null,null, "");
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
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
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
                upModelDel.Update();

                string id = lbDelId.Text;
                string user = Session["User"].ToString();

                DealerService srv = new DealerService();
                srv.DeleteDealer(id, user);

                BindGridData();
                ClientPopup(ModalType.Success, "Completed");
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
        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lbType.Text = "";
                lbId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvDealer.DataKeys != null)
                {
                    string id = gvDealer.DataKeys[index].Values["ID"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    DealerService srv = new DealerService();
                    DataRow row = srv.GetDealerById(id);
                    if (row != null)
                    {
                        tbDealerName.Text = row["DEALER_NAME"].ToString();
                        tbDealerCode.Text = row["DEALER_CODE"].ToString();
                        tbBranchName.Text = row["BRANCH_NAME"].ToString();
                        tbBranchCode.Text = row["BRANCH_CODE"].ToString();
                        tbAddress.Text = row["DEALER_ADDRESS"].ToString();
                        tbMobile.Text = row["DEALER_MOBILE"].ToString();
                        tbOffice_hours.Text = row["DEALER_OFFICE_HOURS"].ToString();
                        tbOffice_hours2.Text = row["DEALER_OFFICE_HOURS2"].ToString();

                        dlGeo2.SelectedValue = row["GEO_ID"].ToString();
                        dlActive.SelectedValue = row["ACTIVE"].ToString();

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

        private void BindGridData()
        {
            try
            {
                DealerService srv = new DealerService();
                
                gvDealer.DataSource = srv.GetDealers(dlGeo.SelectedValue,"-1");
                gvDealer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGeo()
        {
            try
            {
                DealerService srv = new DealerService();
                using (DataTable seller = srv.GetGeo()) // If dealer get seller from dealer else get all seller
                {
                    dlGeo.DataSource = seller;
                    dlGeo.DataBind();

                    dlGeo2.DataSource = seller;
                    dlGeo2.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}