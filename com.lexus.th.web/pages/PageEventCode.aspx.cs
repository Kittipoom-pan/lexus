using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace com.lexus.th.web.pages
{
    public partial class PageEventCode : System.Web.UI.Page
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
                        BindGridData(dlEvent.SelectedValue);
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
                BindGridData(dlEvent.SelectedValue);
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
                BindGridData(dlEvent.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        // ddlEvent
        private void BindEvent()
        {
            try
            {
                EventCodeService srv = new EventCodeService();
                dlEvent.DataSource = srv.GetEvents();
                dlEvent.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridData(string eventId)
        {
            try
            {
                EventCodeService srv = new EventCodeService();
                gvCust.DataSource = srv.GetEventsCodeById(eventId);
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
                string eventId = lbEventId.Text;
                string redeemCode = tbRedeemCode.Text;
                string id = lbId.Text;
                string status = tbStatus.SelectedValue;


                EventCodeService srv = new EventCodeService();
                if (lbType.Text == "Add")
                {
                    srv.AddEventCode(eventId, redeemCode, user, status);
                    BindGridData(dlEvent.SelectedValue);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateEventCode(eventId, redeemCode, user, id, status);
                    BindGridData(dlEvent.SelectedValue);
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

                string eventId = lbDelId.Text;
                string id = lbDelNo.Text;
                string user = Session["User"].ToString();

                EventCodeService srv = new EventCodeService();
                srv.DeleteEventCode(eventId, id, user);

                BindGridData(dlEvent.SelectedValue);
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
                lbEventId.Text = dlEvent.SelectedValue;
                lbId.Text = "";

                tbEventType.Text = dlEvent.SelectedItem.Text;
                tbId.Text = "";
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
                        lbDelId.Text = gvCust.DataKeys[index].Values["EVENT_ID"].ToString();
                        lbDelNo.Text = gvCust.DataKeys[index].Values["ID"].ToString();
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
                lbId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string eventId = gvCust.DataKeys[index].Values["EVENT_ID"].ToString();
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();

                    lbType.Text = "Edit";
                    lbEventId.Text = eventId;
                    lbId.Text = id;

                    EventCodeService srv = new EventCodeService();
                    DataRow row = srv.GetEventCodeById(eventId, id);
                    if (row != null)
                    {
                        tbRedeemCode.Text = row["REDEEM_CODE"].ToString();
                        tbEventType.Text = row["TITLE"].ToString();
                        tbId.Text = row["ID"].ToString();
                        tbStatus.SelectedValue = row["STATUS"].ToString();

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
                EventCodeService srv = new EventCodeService();
                DataRow oldRow = srv.GetEventCodeById(lbEventId.Text, lbId.Text);
                if (oldRow != null)
                {
                    string oldRedeem = oldRow["REDEEM_CODE"].ToString();
                    string curRedeem = tbRedeemCode.Text.Trim();

                    if (oldRedeem == curRedeem)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        args.IsValid = !srv.IsDupplicateRedeem(lbEventId.Text, curRedeem);
                    }
                }
                else
                {
                    args.IsValid = !srv.IsDupplicateRedeem(lbEventId.Text, tbRedeemCode.Text);
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
                if (dlEvent.SelectedIndex < 0)
                {
                    throw new Exception("Please select Event!");
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
                                EventCodeService srv = new EventCodeService();
                                srv.UploadEventCode(dlEvent.SelectedValue, redeemCode, Session["User"].ToString());
                                BindGridData(dlEvent.SelectedValue);
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
                if (dlEvent.SelectedIndex < 0)
                {
                    throw new Exception("Please select Event!");
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
                                EventCodeService srv = new EventCodeService();
                                srv.UploadEventCode(dlEvent.SelectedValue, redeemCode, Session["User"].ToString());
                                BindGridData(dlEvent.SelectedValue);
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