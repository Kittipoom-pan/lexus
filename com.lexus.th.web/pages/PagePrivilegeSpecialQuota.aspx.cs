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
    public partial class PagePrivilegeSpecialQuota : System.Web.UI.Page
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
                        BindGridData(dlPrivilege.SelectedValue, PageTitle.InnerText == "Privilege Block Quota");
                        dSpecialQuota.Visible = !(PageTitle.InnerText == "Privilege Block Quota");
                        dSpecialUsage.Visible = !(PageTitle.InnerText == "Privilege Block Quota");
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
                BindGridData(dlPrivilege.SelectedValue, PageTitle.InnerText == "Privilege Block Quota");
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
                BindGridData(dlPrivilege.SelectedValue, PageTitle.InnerText == "Privilege Block Quota");
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
                PrivilegeSpecialQuotaService srv = new PrivilegeSpecialQuotaService();
                dlPrivilege.DataSource = srv.GetPrivileges();
                dlPrivilege.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridData(string privilegeId, bool isBlockQuota)
        {
            try
            {
                PrivilegeSpecialQuotaService srv = new PrivilegeSpecialQuotaService();
                DataTable dt = srv.GetPrivilegeSpecialQuotaByPrivilege(privilegeId, isBlockQuota);
                //ClientPopup(ModalType.Success, dt.Rows.Count.ToString());
                gvCust.DataSource = dt;
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

                bool isBlockQuota = PageTitle.InnerText == "Privilege Block Quota";
                string user = Session["User"].ToString();
                string privilegeId = lbPrivilegeId.Text;
                string MemberID = tbMemberID.Text;
                string SpecialQuota = tbSpecialQuota.Text;
                string SpecialUsage = tbSpecialUsage.Text;
                if (isBlockQuota)
                {
                    SpecialQuota = "-1";
                    SpecialUsage = "0";
                }

                try
                {
                    if (int.Parse(SpecialQuota) <= 0 && !isBlockQuota)
                    {
                        ClientPopup(ModalType.Error, "Please fill SpecialQuota > 0");
                        return;
                    }
                }
                catch
                {
                    ClientPopup(ModalType.Error, "Please fill SpecialQuota > 0");
                    return;
                }

                try
                {
                    if (int.Parse(SpecialUsage) < 0 && !isBlockQuota)
                    {
                        ClientPopup(ModalType.Error, "Please fill SpecialUsage >= 0");
                        return;
                    }
                }
                catch
                {
                    ClientPopup(ModalType.Error, "Please fill SpecialUsage >= 0");
                    return;
                }

                PrivilegeSpecialQuotaService srv = new PrivilegeSpecialQuotaService();
                if (lbType.Text == "Add")
                {
                    srv.AddPrivilegeSpecialQuota(privilegeId, MemberID, SpecialQuota, user);
                    BindGridData(dlPrivilege.SelectedValue, isBlockQuota);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit" && !isBlockQuota) //BlockQuota cannot use this function
                {
                    srv.UpdatePrivilegeSpecialQuota(privilegeId, MemberID, SpecialQuota, SpecialUsage, user);
                    BindGridData(dlPrivilege.SelectedValue, isBlockQuota);
                    ClientPopup(ModalType.Success, "Completed");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot save. Please try again later.");
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
                upModelDel.Update();

                string privilege_id = lbDelPrivilegeId.Text;
                string member_id = lbDelMemberID.Text;
                string user = Session["User"].ToString();

                PrivilegeSpecialQuotaService srv = new PrivilegeSpecialQuotaService();
                srv.DeletePrivilegeSpecialQuota(privilege_id, member_id, user);

                BindGridData(dlPrivilege.SelectedValue, PageTitle.InnerText == "Privilege Block Quota");
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot delete. Please try again later.");
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

                tbPrivilegeName.Text = dlPrivilege.SelectedItem.Text;
                tbMemberID.Text = "";
                tbSpecialQuota.Text = "0";
                tbSpecialUsage.Text = "0";
                tbMemberID.Enabled = true;
                tbSpecialUsage.Enabled = false;

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
                    lbDelPrivilegeId.Text = "";
                    lbDelMemberID.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelPrivilegeId.Text = gvCust.DataKeys[index].Values["privilege_id"].ToString();
                        lbDelMemberID.Text = gvCust.DataKeys[index].Values["member_id"].ToString();
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

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["privilege_id"].ToString();
                    string member_id = gvCust.DataKeys[index].Values["member_id"].ToString();

                    lbType.Text = "Edit";
                    lbPrivilegeId.Text = id;
                    tbMemberID.Enabled = false;
                    tbSpecialUsage.Enabled = true;

                    PrivilegeSpecialQuotaService srv = new PrivilegeSpecialQuotaService();
                    DataRow row = srv.GetPrivilegeSpecialQuotaByID(id, member_id);
                    if (row != null)
                    {
                        tbPrivilegeName.Text = row["PRIVILEGE_NAME"].ToString();
                        tbMemberID.Text = row["member_id"].ToString();
                        tbSpecialQuota.Text = row["special_quota"].ToString();
                        tbSpecialUsage.Text = row["special_usage"].ToString();

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
                                PrivilegeSpecialQuotaService srv = new PrivilegeSpecialQuotaService();
                                srv.UploadPrivilegeCode(dlPrivilege.SelectedValue, redeemCode, Session["User"].ToString());
                                BindGridData(dlPrivilege.SelectedValue, PageTitle.InnerText == "Privilege Block Quota");
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
                                PrivilegeSpecialQuotaService srv = new PrivilegeSpecialQuotaService();
                                srv.UploadPrivilegeCode(dlPrivilege.SelectedValue, redeemCode, Session["User"].ToString());
                                BindGridData(dlPrivilege.SelectedValue, PageTitle.InnerText == "Privilege Block Quota");
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