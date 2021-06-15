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
    public partial class PageEventBlockQuota : System.Web.UI.Page
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
                        BindEvent();
                        BindGridData(dlEvent.SelectedValue, PageTitle.InnerText == "Event Block Quota");
                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void dlEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindGridData(dlEvent.SelectedValue, PageTitle.InnerText == "Event Block Quota");
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
                BindGridData(dlEvent.SelectedValue, PageTitle.InnerText == "Event Block Quota");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindEvent()
        {
            try
            {
                EventSpecialQuotaService srv = new EventSpecialQuotaService();
                dlEvent.DataSource = srv.GetEvents();
                dlEvent.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridData(string EventId, bool isBlockQuota)
        {
            try
            {
                EventSpecialQuotaService srv = new EventSpecialQuotaService();
                DataTable dt = srv.GetEventSpecialQuotaByEvent(EventId);
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

                bool isBlockQuota = PageTitle.InnerText == "Event Block Quota";
                string user = Session["User"].ToString();
                string EventId = lbEventId.Text;
                string MemberID = tbMemberID.Text;
                string Mobile = tbMemberID.Text;
                //string SpecialQuota = tbSpecialQuota.Text;
                //string SpecialUsage = tbSpecialUsage.Text;
                //if (isBlockQuota)
                //{
                //    SpecialQuota = "-1";
                //    SpecialUsage = "0";
                //}



                EventSpecialQuotaService srv = new EventSpecialQuotaService();
                if (lbType.Text == "Add")
                {
                    srv.AddEventSpecialQuota(EventId, MemberID, Mobile, user);
                    BindGridData(dlEvent.SelectedValue, isBlockQuota);
                    ClientPopup(ModalType.Success, "Completed");
                }
                //else if (lbType.Text == "Edit" && !isBlockQuota) //BlockQuota cannot use this function
                //{
                //    srv.UpdateEventSpecialQuota(EventId, MemberID, SpecialQuota, SpecialUsage, user);
                //    BindGridData(dlEvent.SelectedValue, isBlockQuota);
                //    ClientPopup(ModalType.Success, "Completed");
                //}
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

                string Event_id = lbDelEventId.Text;
                string member_id = lbDelMemberID.Text;
                string user = Session["User"].ToString();

                EventSpecialQuotaService srv = new EventSpecialQuotaService();
                srv.DeleteEventSpecialQuota(Event_id, member_id, user);

                BindGridData(dlEvent.SelectedValue, PageTitle.InnerText == "Event Block Quota");
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
                lbEventId.Text = dlEvent.SelectedValue;

                tbEventName.Text = dlEvent.SelectedItem.Text;
                tbMemberID.Text = "";
                tbMemberID.Enabled = true;

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
                    lbDelEventId.Text = "";
                    lbDelMemberID.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelEventId.Text = gvCust.DataKeys[index].Values["Event_id"].ToString();
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
                lbEventId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                //if (gvCust.DataKeys != null)
                //{
                //    string id = gvCust.DataKeys[index].Values["Event_id"].ToString();
                //    string member_id = gvCust.DataKeys[index].Values["member_id"].ToString();

                //    lbType.Text = "Edit";
                //    lbEventId.Text = id;
                //    tbMemberID.Enabled = false;
                //    tbSpecialUsage.Enabled = true;

                //    EventSpecialQuotaService srv = new EventSpecialQuotaService();
                //    DataRow row = srv.GetEventSpecialQuotaByID(id, member_id);
                //    if (row != null)
                //    {
                //        tbEventName.Text = row["Event_NAME"].ToString();
                //        tbMemberID.Text = row["member_id"].ToString();
                //        tbSpecialQuota.Text = row["special_quota"].ToString();
                //        tbSpecialUsage.Text = row["special_usage"].ToString();

                //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                //        upModalAdd.Update();
                //    }
                //}
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

  

      
    }
}