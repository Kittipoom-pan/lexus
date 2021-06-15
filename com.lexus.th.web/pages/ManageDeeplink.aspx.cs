using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web.pages
{
    public partial class ManageDeeplink : System.Web.UI.Page
    {
        public bool is_loading { get; set; }
        private enum ModalType { Success, Error, Warning }
        private enum FileType { File1, File2 }

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
                        //ViewState["NotiPath"] = WebConfigurationManager.AppSettings["NotiPath"];
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();
                        BindGridData();
                    }
                }
                //this.RegisterPostBackControl();
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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
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
                ManageDeeplinkService srv = new ManageDeeplinkService();
                gvCust.DataSource = srv.GetManageDeeplink();
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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
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

        protected bool Validate()
        {
            if (cb_link.Checked)
            {
                string link_url = dlLink.SelectedValue;
                if (link_url == "Privilege" || link_url == "Event" || link_url == "Event" ||
                    link_url == "News" || link_url == "Article")
                {
                    if (String.IsNullOrEmpty((dlAction.SelectedValue)))
                    {
                        ClientPopup(ModalType.Error, "Required Link detail.");
                        return false;
                    }
                }
                if (link_url == "Ext_Link")
                {
                    if (String.IsNullOrEmpty((tbLinkURL.Text)))
                    {
                        ClientPopup(ModalType.Error, "Required Link detail.");
                        return false;
                    }
                }
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validate())
            {
                return;
            }
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                string member_id = tbMemberID.Text;
                string name = tbName.Text;
                string url = tbUrl.Text;
                string is_link = cb_link.Checked == true ? "1" : "0";
                string page = cb_link.Checked == true ? dlLink.SelectedValue : "none";
                string reference_id = dlAction.SelectedValue;
                string user = Session["User"].ToString();

                string ext_link = tbLinkURL.Text;

                DateTime created_date = DateTime.Now;

                APIService api = new APIService();

                string link = url;
                if (cb_link.Checked == true)
                {
                    link = link + "?page=" + page;
                }
                if (reference_id != "")
                {
                    link = link + "&reference_id=" + reference_id;
                }
                if(page == "Ext_Link")
                {
                    link = link + "&link_url=" + ext_link;
                }
                DeeplinkRequest request = new DeeplinkRequest(link);

                ManageDeeplinkService service = new ManageDeeplinkService();
                service.CreateDeeplink(request, response =>
                {
                    JObject json = JObject.Parse(response);
                    string shortlink = json["shortLink"].ToString();
                    var addDeeplink = service.AddManageDeepLink(name, shortlink, page, reference_id, link, created_date.ToString(), user);
                    ClientPopup(ModalType.Success, "Completed");
                    BindGridData();
                }, error =>
                {
                    LogManager.ServiceLog.WriteCustomLog("AddEvent TreasureData", error);
                });
            }
            catch (Exception ex)
            {
                LogManager.ServiceLog.WriteCustomLog("AddEvent TreasureData", ex.Message);
                ClientPopup(ModalType.Error, "Cannot save. Please try again later.");
            }
        }
    
        protected void gvBtnDeleteApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                lbDelId.Text = "";

                if (gvCust.DataKeys != null)
                {
                    lbDelId.Text = gvCust.DataKeys[index].Values["id"].ToString();

                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal();", true);
                upModelDel.Update();
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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdDelConfirm').modal('hide');", true);
                upModelDel.Update();

                string id = lbDelId.Text;
                string user = Session["User"].ToString();

                ManageDeeplinkService srv = new ManageDeeplinkService();
                srv.DeleteDeeplink(id,user);

                BindGridData();
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

        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {
                tbMemberID.Enabled = true;
                tbName.Enabled = true;
                cb_link.Enabled = true;
                dlLink.Enabled = true;
                tbLinkURL.Enabled = true;
            
                dlAction.Enabled = true;
                btnSave.Visible = true;

                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
  
                tbMemberID.Text = "";
                tbName.Text = "";
                cb_link.Checked = false;
                dlLink.SelectedIndex = -1;
                tbLinkURL.Text = "";
    
                lbType.Text = "Add";
                dlLink_SelectedIndexChanged(null, null);
                cb_link_CheckedChanged(null, null);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
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

        protected void dlLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("~/Default.aspx", false);
            }
            if (dlLink.SelectedValue == "Ext_Link")
            {
                pnLinkURL.Visible = true;
                pnLinkAction.Visible = false;
                dlAction.Items.Clear();
            }
            else
            {
                pnLinkURL.Visible = false;
                pnLinkAction.Visible = true;
                BindAction(dlLink.SelectedValue);
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }

        private void BindAction(string actionType)
        {
            try
            {
                dlAction.Items.Clear();
                if (actionType == "Privilege" || actionType == "Event" || actionType == "News" || actionType == "Article")
                {
                    NotificationService srv = new NotificationService();
                    using (DataTable action = srv.GetAction(actionType)) // If dealer get seller from dealer else get all seller
                    {
                        dlAction.DataSource = action;
                        dlAction.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cb_link_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("~/Default.aspx", false);
            }
            if (cb_link.Checked == true)
            {
                dlLink.Enabled = true;
                tbLinkURL.Enabled = true;
                dlAction.Enabled = true;
            }
            else
            {
                dlLink.Enabled = false;
                tbLinkURL.Enabled = false;
                dlAction.Enabled = false;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }
    }
}