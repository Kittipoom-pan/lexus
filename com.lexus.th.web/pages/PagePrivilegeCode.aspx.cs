using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace com.lexus.th.web.master
{
    public partial class PagePrivilegeCode : System.Web.UI.Page
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
                PrivilegeCodeService srv = new PrivilegeCodeService();
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
                PrivilegeCodeService srv = new PrivilegeCodeService();
                gvCust.DataSource = srv.GetPrivilegeCodeById(privilegeId);
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
                string redeemCode = tbRedeemCode.Text;
                string no = lbNo.Text;
                string status = tbStatus.SelectedValue;

                PrivilegeCodeService srv = new PrivilegeCodeService();
                if (lbType.Text == "Add")
                {
                    srv.AddPrivilegeCode(privilegeId, redeemCode, user, status);
                    BindGridData(dlPrivilege.SelectedValue);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdatePrivilegeCode(privilegeId, redeemCode, user, no, status);
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
                string no = lbDelNo.Text;
                string user = Session["User"].ToString();

                PrivilegeCodeService srv = new PrivilegeCodeService();
                srv.DeletePrivilegeCode(id, no, user);

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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdUpload", "$('#mdUpload').modal('hide');", true);
            upMdUpload.Update();

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
                lbNo.Text = "";

                tbPrivilegeName.Text = dlPrivilege.SelectedItem.Text;
                tbNo.Text = "";
                tbRedeemCode.Text = "";

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
                    lbDelNo.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelId.Text = gvCust.DataKeys[index].Values["PRIVILEGE_ID"].ToString();
                        lbDelNo.Text = gvCust.DataKeys[index].Values["NO"].ToString();
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
                lbNo.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["PRIVILEGE_ID"].ToString();
                    string no = gvCust.DataKeys[index].Values["NO"].ToString();

                    lbType.Text = "Edit";
                    lbPrivilegeId.Text = id;
                    lbNo.Text = no;

                    PrivilegeCodeService srv = new PrivilegeCodeService();
                    DataRow row = srv.GetPrivilegeCodeById(id, no);
                    if (row != null)
                    {
                        tbRedeemCode.Text = row["REDEEM_CODE"].ToString();
                        tbPrivilegeName.Text = row["PRIVILEGE_NAME"].ToString();
                        tbNo.Text = row["NO"].ToString();
                        tbStatus.SelectedValue = row["Status"].ToString();

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
        protected void cvDupRedeem_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                PrivilegeCodeService srv = new PrivilegeCodeService();
                DataRow oldRow = srv.GetPrivilegeCodeById(lbPrivilegeId.Text, lbNo.Text);
                if (oldRow != null)
                {
                    string oldRedeem = oldRow["REDEEM_CODE"].ToString();
                    string curRedeem = tbRedeemCode.Text;

                    if (oldRedeem == curRedeem)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        args.IsValid = !srv.IsDupplicateRedeem(lbPrivilegeId.Text, curRedeem);
                    }
                }
                else
                {
                    args.IsValid = !srv.IsDupplicateRedeem(lbPrivilegeId.Text, tbRedeemCode.Text);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdUpload", "$('#mdUpload').modal();", true);
            upMdUpload.Update();
        }
        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlPrivilege.SelectedIndex < 0)
                {
                    throw new Exception("Please select Privilete!");
                }
                if (upload.HasFile)
                {
                    if (upload.PostedFile.ContentType == "text/plain")
                    {
                        string uploadFile = System.Web.Configuration.WebConfigurationManager.AppSettings["UploadPath"] + "\\" + upload.FileName;
                        if (!Directory.Exists(Path.GetDirectoryName(uploadFile)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(uploadFile));
                        }
                        upload.SaveAs(uploadFile);

                        if (File.Exists(uploadFile))
                        {
                            List<string> redeemCode = new List<string>();
                            using (StreamReader reader = new StreamReader(uploadFile, System.Text.Encoding.Default))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    redeemCode.Add(line);
                                }
                            }
                            if (redeemCode.Count > 0)
                            {
                                PrivilegeCodeService srv = new PrivilegeCodeService();
                                srv.UploadPrivilegeCode(dlPrivilege.SelectedValue, redeemCode, Session["User"].ToString());
                                BindGridData(dlPrivilege.SelectedValue);
                                ClientPopup(ModalType.Success, "Completed");
                            }
                            redeemCode = null;
                        }
                    }
                    else
                    {
                        throw new Exception("Allow file type Text only!");
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnUploadFile2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlPrivilege.SelectedIndex < 0)
                {
                    throw new Exception("Please select Privilete!");
                }
                if (upload.HasFile)
                {
                    if (upload.PostedFile.ContentType == "text/plain")
                    {
                        string uploadFile = System.Web.Configuration.WebConfigurationManager.AppSettings["UploadPath"] + "\\" + upload.FileName;
                        if (!Directory.Exists(Path.GetDirectoryName(uploadFile)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(uploadFile));
                        }
                        upload.SaveAs(uploadFile);

                        if (File.Exists(uploadFile))
                        {
                            List<string> redeemCode = new List<string>();
                            using (StreamReader reader = new StreamReader(uploadFile, System.Text.Encoding.Default))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    redeemCode.Add(line);
                                }
                            }
                            if (redeemCode.Count > 0)
                            {
                                PrivilegeCodeService srv = new PrivilegeCodeService();
                                srv.UploadPrivilegeCode(dlPrivilege.SelectedValue, redeemCode, Session["User"].ToString());
                                BindGridData(dlPrivilege.SelectedValue);
                                ClientPopup(ModalType.Success, "Completed");
                            }
                            redeemCode = null;
                        }
                    }
                    else
                    {
                        throw new Exception("Allow file type Text only!");
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