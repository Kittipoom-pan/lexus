using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web.pages
{
    public partial class PageUpcoming : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        private enum ImageType { Img1, Img2, Img3, Img4, Img5 }

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
                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathUpComing"];

                        BindGridData(dlListSearch.SelectedValue);
                    }
                }
                this.RegisterPostBackControl();
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
                BindGridData(dlListSearch.SelectedValue);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindGridData(string listSearch)
        {
            try
            {
                lbTypeList.Text = dlListSearch.SelectedValue;

                UpComingService srv = new UpComingService();
                gvCust.DataSource = srv.GetUpComing(listSearch);
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
                string list = dlList.SelectedValue;
                string order = tbOrder.Text;
                string type = dlType.SelectedValue;
                string action = dlAction.SelectedValue;

                string dispStart = tbDispStart.Text;
                string dispEnd = tbDispEnd.Text;
                string user = Session["User"].ToString();

                UpComingService srv = new UpComingService();
                if (lbType.Text == "Add")
                {
                    srv.AddUpcoming(type, action, order, dispStart, dispEnd, user, list);
                    BindGridData(dlListSearch.SelectedValue);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateUpcoming(type, action, order,  dispStart, dispEnd, user, lbId.Text, list);
                    BindGridData(dlListSearch.SelectedValue);
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
                string list = dlList.SelectedValue;

                UpComingService srv = new UpComingService();
                srv.DeleteUpcoming(id, user, list);

                BindGridData(dlListSearch.SelectedValue);
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
                dlList.SelectedIndex = -1;
                tbOrder.Text = "";
                dlType.SelectedIndex = -1;
                BindAction(dlType.SelectedValue);
                dlAction.SelectedIndex = -1;

                tbDispStart.Text = "";
                tbDispEnd.Text = "";
                lbType.Text = "Add";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", @"$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDispStart').datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDispEnd').datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
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
                lbId.Text = "";
                lbTypeList.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["id"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;
                    lbTypeList.Text = dlListSearch.SelectedValue;

                    UpComingService srv = new UpComingService();
                    DataRow row = srv.GetUpComingById(id, lbTypeList.Text);
                    if (row != null)
                    {
                        dlList.SelectedValue = row["list"].ToString();
                        tbOrder.Text = row["order"].ToString();
                        dlType.SelectedValue = row["type"].ToString();
                        BindActionEdit(dlType.SelectedValue, row["action_id"].ToString());
                        dlAction.SelectedValue = row["action_id"].ToString();

                        tbDispStart.Text = (row["start_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["start_date"]).ToString("dd/MM/yyyy HH:mm");
                        tbDispEnd.Text = (row["end_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["end_date"]).ToString("dd/MM/yyyy HH:mm");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", @"$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDispStart').datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});$('#ContentPlaceHolder1_tbDispEnd').datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true); upModalAdd.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void dlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dlType.SelectedValue == "Events")
            {
                BindAction(dlType.SelectedValue);
            }
            if (dlType.SelectedValue == "News")
            {
                BindAction(dlType.SelectedValue);
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDispStart').datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm',}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});" +
                "                                                                                   $('#ContentPlaceHolder1_tbDispEnd').datetimepicker({defaultDate: new Date(),format: 'DD/MM/YYYY HH:mm',}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }

        private void BindAction(string actionType)
        {
            try
            {
                UpComingService srv = new UpComingService();
                using (DataTable action = srv.GetAction(actionType)) // If dealer get seller from dealer else get all seller
                {
                    dlAction.Items.Clear();
                    dlAction.DataSource = action;
                    dlAction.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindActionEdit(string actionType, string id)
        {
            try
            {
                UpComingService srv = new UpComingService();
                using (DataTable action = srv.GetActionEdit(actionType, int.Parse(id))) // If dealer get seller from dealer else get all seller
                {
                    dlAction.Items.Clear();
                    dlAction.DataSource = action;
                    dlAction.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dlListSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridData(dlListSearch.SelectedValue);
        }
    }
}