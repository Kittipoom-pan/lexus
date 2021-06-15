using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AppLibrary.Database;

namespace com.lexus.th.web.master
{
    public partial class PagePrivilegeVerify : System.Web.UI.Page
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
                        BindPrivilege();
                        BindGridData(dlPrivilege.SelectedValue);
                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void dlPrivilege_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindGridData(dlPrivilege.SelectedValue);
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
                BindGridData(dlPrivilege.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindPrivilege()
        {
            try
            {
                PrivilegeVerifyService srv = new PrivilegeVerifyService();
                dlPrivilege.DataSource = srv.GetPrivileges();
                dlPrivilege.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridData(string privilegeId)
        {
            try
            {
                PrivilegeVerifyService srv = new PrivilegeVerifyService();
                gvCust.DataSource = srv.GetPrivilegeVerifyById(privilegeId);
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

                string user = Session["User"].ToString();
                string privilegeId = lbPrivilegeId.Text;
                string shopName = tbShopName.Text;
                string shopNameOld = lbShopNameOld.Text;
                string verifyCode = tbVerifyCode.Text;

                PrivilegeVerifyService srv = new PrivilegeVerifyService();
                if (lbType.Text == "Add")
                {
                    srv.AddPrivilegeVerify(privilegeId, shopName, verifyCode, user);
                    BindGridData(dlPrivilege.SelectedValue);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdatePrivilegeVerify(privilegeId, shopName, shopNameOld, verifyCode, user);
                    BindGridData(dlPrivilege.SelectedValue);
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
                string shopName = lbDelShop.Text;
                string user = Session["User"].ToString();

                PrivilegeVerifyService srv = new PrivilegeVerifyService();
                srv.DeletePrivilegeVerify(id, shopName, user);

                BindGridData(dlPrivilege.SelectedValue);
                ClientPopup(ModalType.Success, "Completed");
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
                lbType.Text = "Add";
                lbPrivilegeId.Text = dlPrivilege.SelectedValue;
                lbShopNameOld.Text = "";

                tbPrivilegeName.Text = dlPrivilege.SelectedItem.Text;
                tbShopName.Text = "";
                tbVerifyCode.Text = "";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                upModalAdd.Update();
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
                    lbDelShop.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelId.Text = gvCust.DataKeys[index].Values["PRIVILEGE_ID"].ToString();
                        lbDelShop.Text = gvCust.DataKeys[index].Values["SHOP_NM"].ToString();
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
        protected void gvCust_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
            if (lnkFull != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
            }
        }
        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lbType.Text = "";
                lbPrivilegeId.Text = "";
                lbShopNameOld.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["PRIVILEGE_ID"].ToString();
                    string shop = gvCust.DataKeys[index].Values["SHOP_NM"].ToString();

                    lbType.Text = "Edit";
                    lbPrivilegeId.Text = id;
                    lbShopNameOld.Text = shop;

                    PrivilegeVerifyService srv = new PrivilegeVerifyService();
                    DataRow row = srv.GetPrivilegeVerifyById(id, shop);
                    if (row != null)
                    {
                        tbVerifyCode.Text = row["VERIFY_CODE"].ToString();
                        tbPrivilegeName.Text = row["PRIVILEGE_NAME"].ToString();
                        tbShopName.Text = row["SHOP_NM"].ToString();
                        lbShopNameOld.Text = row["SHOP_NM"].ToString();

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

        protected void cvDupVerify_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                PrivilegeVerifyService srv = new PrivilegeVerifyService();
                DataRow oldRow = srv.GetPrivilegeVerifyById(lbPrivilegeId.Text, lbShopNameOld.Text);
                if (oldRow != null)
                {
                    string oldVerify = oldRow["VERIFY_CODE"].ToString();
                    string curVerify = tbVerifyCode.Text;

                    if (oldVerify == curVerify)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        args.IsValid = !srv.IsDupplicateVerify(lbPrivilegeId.Text, curVerify);
                    }
                }
                else
                {
                    args.IsValid = !srv.IsDupplicateVerify(lbPrivilegeId.Text, tbVerifyCode.Text);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void cvDupShopNM_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                PrivilegeVerifyService srv = new PrivilegeVerifyService();
                DataRow oldRow = srv.GetPrivilegeVerifyById(lbPrivilegeId.Text, lbShopNameOld.Text);
                if (oldRow != null)
                {
                    string oldVerify = oldRow["SHOP_NM"].ToString();
                    string curVerify = tbShopName.Text;

                    if (oldVerify == curVerify)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        args.IsValid = !srv.IsDupplicateShopNM(lbPrivilegeId.Text, curVerify);
                    }
                }
                else
                {
                    args.IsValid = !srv.IsDupplicateShopNM(lbPrivilegeId.Text, tbShopName.Text);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
    }
}