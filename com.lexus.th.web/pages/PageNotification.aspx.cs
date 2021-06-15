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
    public partial class PageNotification : System.Web.UI.Page
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
                    
                    if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsProduction"] == "0")
                    {
                        dlDestination.Items.Add(new ListItem { Text = "Test", Value = "Test" });
                    }
                    else if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsProduction"] == "1")
                    {
                        dlDestination.Items.Add(new ListItem { Text = "All", Value = "All" });
                        dlDestination.Items.Add(new ListItem { Text = "Member", Value = "Member" });
                        dlDestination.Items.Add(new ListItem { Text = "Device", Value = "Device" });
                        dlDestination.Items.Add(new ListItem { Text = "Mobile OS", Value = "Mobile OS" });
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void dlDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dlDestination.SelectedValue == "All")
                {
                    dlSubDestination.Visible = false;
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                }
                if (dlDestination.SelectedValue == "Member")
                {
                    dlSubDestination.Visible = true;
                    pnMemberID.Visible = true;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                    dlSubDestination.Items.Clear();
                    dlSubDestination.Items.Add(new ListItem { Text = "Specific Member ID", Value = "Specific Member ID" });
                    dlSubDestination.Items.Add(new ListItem { Text = "Group Member ID", Value = "Group Member ID" });
                }
                if (dlDestination.SelectedValue == "Device")
                {
                    dlSubDestination.Visible = true;
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = true;
                    pnDeviceGroup.Visible = false;
                    dlSubDestination.Items.Clear();
                    dlSubDestination.Items.Add(new ListItem { Text = "Specific Device", Value = "Specific Device" });
                    dlSubDestination.Items.Add(new ListItem { Text = "Group Device", Value = "Group Device" });
                }
                if (dlDestination.SelectedValue == "Mobile OS")
                {
                    dlSubDestination.Visible = true;
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                    dlSubDestination.Items.Clear();
                    dlSubDestination.Items.Add(new ListItem { Text = "Android", Value = "Android" });
                    dlSubDestination.Items.Add(new ListItem { Text = "iOS", Value = "iOS" });
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void dlSubDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dlSubDestination.SelectedValue == "Specific Member ID")
                {
                    pnMemberID.Visible = true;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                }
                if (dlSubDestination.SelectedValue == "Group Member ID")
                {
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = true;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                }
                if (dlSubDestination.SelectedValue == "Specific Device")
                {
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = true;
                    pnDeviceGroup.Visible = false;
                }
                if (dlSubDestination.SelectedValue == "Group Device")
                {
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private void ClientPopup(ModalType type, string message)
        {
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
        protected void btnSend_Click(object sender, EventArgs e)
        {
            ServiceModel firebase = null;
            try
            {
                CustomerService srv = new CustomerService();
                APIService api = new APIService();

                string to = "";
                switch (dlDestination.SelectedValue)
                {
                    case "All":
                        to = "/topics/All";
                        break;
                    case "Android":
                        to = "/topics/Android";
                        break;
                    case "iOS":
                        to = "/topics/iOS";
                        break;
                    case "Member":
                        to = "/topics/Member";
                        break;
                    case "Test":
                        to = "/topics/TestPush";
                        break;
                }

                //string noti_type = "LINK";
                //string link_url = "https://www.google.co.th";
                //string ref_id = "29";
                //firebase = api.PostFirebase(APIService.PushType.Topic, to, tbMessage.Text, tbTitle.Text, ref_id, noti_type, link_url, "");
                
                if (firebase.Success) // Post Firebase
                {
                    //insert log notification
                    //api.AddNotification(tbTitle.Text, tbMessage.Text, dlDestination.SelectedValue, noti_type, ref_id, link_url);
                    //api.AddNotification(tbTitle.Text, tbMessage.Text, "ANDROID");
                    ClientPopup(ModalType.Success, "Completed");
                }
                else
                {
                    throw new Exception("[Firebase failed] " + firebase.Message);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
 
    }
}