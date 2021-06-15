using Microsoft.Win32.TaskScheduler;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web.pages
{
    public partial class PageNotification2 : System.Web.UI.Page
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
                        ViewState["NotiPath"] = WebConfigurationManager.AppSettings["NotiPath"];
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();
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
                NotificationService srv = new NotificationService();
                gvCust.DataSource = srv.GetNotification(txtSearch.Text);
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
            if(!Validate())
            {
                return;
            }
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                string destination = dlDestination.SelectedValue;
                string sub_destination = dlSubDestination.SelectedValue == "" ? "none" : dlSubDestination.SelectedValue;
                string member_id = tbMemberID.Text;
                string device_id = tbDeviceID.Text;
                string title = tbTitle.Text;
                string message = tbMessage.Text;
                string is_link = cb_link.Checked == true ? "1" : "0";
                string link_type = cb_link.Checked == true ? dlLink.SelectedValue : "none";
                string link_url = dlLink.SelectedValue == "Ext_Link" ? tbLinkURL.Text : "";
                string is_schedule = cb_schedule.Checked == true ? "1" : "0";
                string schedule_date = cb_schedule.Checked == true ? tbDate.Text : "";
                string schedule_time = cb_schedule.Checked == true ? tbTime.Text : "";
                string reference_id = dlAction.SelectedValue == "" ? "0" : dlAction.SelectedValue;
                string user = Session["User"].ToString();
                string status = "in_queue";
                string Noti_Schedule_ID = "";
                int is_sended = 0;

                string NotiID = "";

                DateTime ScheduleDate = new DateTime(); 
                DateTime DateNow = DateTime.Now;

                if(schedule_time != "")
                {
                    DateTime myDate;
                    if (DateTime.TryParse(schedule_time, out myDate))
                    {
                        string time = myDate.ToString("HH:mm");
                        string hour = time.Substring(0, 2);
                        int hourInt = int.Parse(hour);
                        if (hourInt >= 24)
                        {
                            ClientPopup(ModalType.Warning, "กรุณากรอกรูปแบบเวลาให้ถูกต้อง");
                            return;
                        }
                    }
                    else
                    {
                        ClientPopup(ModalType.Warning, "กรุณากรอกรูปแบบเวลาให้ถูกต้อง");
                        return;
                    }
                }

                if (cb_schedule.Checked)
                {
                    ScheduleDate = DateTime.ParseExact(schedule_date + " " + schedule_time, "dd/MM/yyyy HH:mm", null);
                }
                else
                {
                    ScheduleDate = DateNow;
                }

                ScheduleDate = DateTime.SpecifyKind(ScheduleDate, DateTimeKind.Utc);
                //DateTimeOffset utcTime2 = ScheduleDate;

                APIService api = new APIService();
                ServiceModel firebase = new ServiceModel();
                //bool firebase = api.PostFirebase(APIService.PushType.Topic, to, tbMessage.Text, tbTitle.Text, ref_id, noti_type, link_url);

                NotificationService srv = new NotificationService();
                if (lbType.Text == "Add")
                {
                    #region All
                    if (dlDestination.SelectedValue == "All")
                    {
                        if (ScheduleDate == DateNow)
                        {
                            NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(),is_sended,Noti_Schedule_ID);

                            string to = "/topics/iOS";
                            var responseIOS = api.PostFirebase(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url, NotiID);

                            string toandroid = "/topics/Android";
                            var responseAndroid = api.PostFirebaseAndroid(APIService.PushType.Topic, toandroid, message, title, reference_id, link_type, link_url, NotiID);

                            var is_success = responseIOS.Success && responseAndroid.Success;
                            srv.UpdateNotificationSended(NotiID, responseIOS.Message + responseAndroid.Message, is_success, true);
                            firebase.Success = true;
                        }
                        else if (ScheduleDate > DateNow)
                        {
                            Noti_Schedule_ID = srv.AddNotificationSchedule(ScheduleDate, status, user);
                            NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);

                            firebase.Success = true;
                        }
                        else
                        {
                            firebase.Success = false;
                        }
                    }
                    #endregion

                    #region Member
                    else if (dlDestination.SelectedValue == "Member")
                    {
                        DataTable dt;
                        if (dlSubDestination.SelectedValue == "Group Member ID")
                        {
                            if (ScheduleDate == DateNow)
                            {
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);
                                dt = srv.AddMemberList(NotiID, (ViewState["noti_member_list"] as DataTable), "");

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string device_token = dr["device_token"].ToString();
                                        string device_Type = dr["os_type"].ToString();
                                        firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, NotiID);

                                        srv.UpdateNotificationSended(NotiID, firebase.Message, firebase.Success, true);
                                    }
                                }
                                else
                                {
                                    ClientPopup(ModalType.Success, "Message not deliver to member");
                                    return;
                                }
                            }
                            else if (ScheduleDate > DateNow)
                            {
                                Noti_Schedule_ID = srv.AddNotificationSchedule(ScheduleDate, status, user);
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);
                                dt = srv.AddMemberList(NotiID, (ViewState["noti_member_list"] as DataTable), "");

                                firebase.Success = true;
                            }
                            else
                            {
                                firebase.Success = false;
                            }
                        }
                        else
                        {
                            if (ScheduleDate == DateNow)
                            {
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);
                                dt = srv.AddMemberList(NotiID, null, member_id);

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string device_token = dr["device_token"].ToString();
                                        string device_Type = dr["os_type"].ToString();
                                        firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, NotiID);

                                        srv.UpdateNotificationSended(NotiID, firebase.Message, firebase.Success, true);
                                    }
                                }
                                else
                                {
                                    ClientPopup(ModalType.Success, "Message not deliver to member");
                                    return;
                                }
                            }
                            else if (ScheduleDate > DateNow)
                            {
                                Noti_Schedule_ID = srv.AddNotificationSchedule(ScheduleDate, status, user);
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);

                                firebase.Success = true;
                            }
                            else
                            {
                                firebase.Success = false;
                            }
                        }
                    }
                    #endregion

                    #region Device
                    else if (dlDestination.SelectedValue == "Device")
                    {
                        DataTable dt;
                        if (dlSubDestination.SelectedValue == "Group Device")
                        {
                            if (ScheduleDate == DateNow)
                            {
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);
                                dt = srv.AddDeviceList(NotiID, (ViewState["noti_member_list"] as DataTable), "");

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string device_token = dr["device_token"].ToString();
                                        string device_Type = dr["os_type"].ToString();
                                        firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, NotiID);

                                        srv.UpdateNotificationSended(NotiID, firebase.Message, firebase.Success, true);
                                    }
                                }
                                else
                                {
                                    ClientPopup(ModalType.Success, "Message not deliver to member");
                                    return;
                                }
                            }
                            else if (ScheduleDate > DateNow)
                            {
                                Noti_Schedule_ID =  srv.AddNotificationSchedule(ScheduleDate, status, user);
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);
                                srv.AddDeviceList(NotiID, (ViewState["noti_member_list"] as DataTable), "");

                                firebase.Success = true;
                            }
                            else
                            {
                                firebase.Success = false;
                            }
                        }
                        else
                        {
                            if (ScheduleDate == DateNow)
                            {
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);
                                dt = srv.AddDeviceList(NotiID, null, device_id);

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string device_token = dr["device_token"].ToString();
                                        string device_Type = dr["os_type"].ToString();
                                        firebase = api.SendPush(device_token, message, "SendPush", 1, device_Type, title, reference_id, link_type, link_url, NotiID);

                                        srv.UpdateNotificationSended(NotiID, firebase.Message, firebase.Success, true);
                                    }
                                }
                                else
                                {
                                    ClientPopup(ModalType.Success, "Message not deliver to member");
                                    return;
                                }
                            }
                            else if (ScheduleDate > DateNow)
                            {
                                Noti_Schedule_ID =  srv.AddNotificationSchedule(ScheduleDate, status, user);
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);

                                firebase.Success = true;
                            }
                            else
                            {
                                firebase.Success = false;
                            }
                        }
                    }
                    #endregion

                    #region Mobile OS
                    else if (dlDestination.SelectedValue == "Mobile OS")
                    {
                        string to = "";
                        if (dlSubDestination.SelectedValue == "Android")
                        {
                            if (ScheduleDate == DateNow)
                            {
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);

                                to = "/topics/Android";
                                firebase = api.PostFirebaseAndroid(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url, NotiID);
                            }
                            else if (ScheduleDate > DateNow)
                            {
                                Noti_Schedule_ID = srv.AddNotificationSchedule(ScheduleDate, status, user);
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);

                                firebase.Success = true;
                            }
                            else
                            {
                                firebase.Success = false;
                            }
                        }
                        else
                        {
                            if (ScheduleDate == DateNow)
                            {
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);

                                to = "/topics/iOS";
                                firebase = api.PostFirebase(APIService.PushType.Topic, to, message, title, reference_id, link_type, link_url, NotiID);
                            }
                            else if (ScheduleDate > DateNow)
                            {
                                Noti_Schedule_ID = srv.AddNotificationSchedule(ScheduleDate, status, user);
                                NotiID = srv.AddNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, "All", reference_id, ScheduleDate.ToString(), is_sended, Noti_Schedule_ID);

                                firebase.Success = true;
                            }
                            else
                            {
                                firebase.Success = false;
                            }
                        }
                    }
                    #endregion

                    else
                    {
                        firebase.Success = false;
                    }

                    if (firebase.Success)
                    {
                        BindGridData();
                        ClientPopup(ModalType.Success, "Completed");
                    }
                    else
                    {
                        ClientPopup(ModalType.Warning, "กรุณาเลือกวันที่มากกว่าหรือเท่ากับปัจจุบัน");
                        //throw new Exception("[Firebase failed] " + firebase.Message);
                    }
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateNotification(destination, sub_destination, member_id, device_id, title, message, is_link, link_type, link_url, is_schedule, schedule_date, schedule_time, user, lbId.Text, "", reference_id);
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
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

                NotificationService srv = new NotificationService();
                srv.DeleteNotification(id, user);

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
                dlDestination.Enabled = true;
                dlSubDestination.Enabled = true;
                tbMemberID.Enabled = true;
                tbDeviceID.Enabled = true;
                tbTitle.Enabled = true;
                tbMessage.Enabled = true;
                cb_link.Enabled = true;
                dlLink.Enabled = true;
                tbLinkURL.Enabled = true;
                cb_schedule.Enabled = true;
                tbDate.Enabled = true;
                tbTime.Enabled = true;
                dlAction.Enabled = true;
                btnSave.Visible = true;

                //btnSave.Enabled = true;
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsProduction"] == "0")
                {
                    dlDestination.Items.Clear();
                    dlDestination.Items.Add(new ListItem { Text = "Test", Value = "Test" });
                }
                else if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsProduction"] == "1")
                {
                    dlDestination.Items.Clear();
                    dlDestination.Items.Add(new ListItem { Text = "All", Value = "All" });
                    dlDestination.Items.Add(new ListItem { Text = "Member", Value = "Member" });
                    dlDestination.Items.Add(new ListItem { Text = "Device", Value = "Device" });
                    dlDestination.Items.Add(new ListItem { Text = "Mobile OS", Value = "Mobile OS" });
                }

                //dlDestination.SelectedValue = "1";
                //dlSubDestination.SelectedValue = "1";
                tbMemberID.Text = "";
                tbDeviceID.Text = "";
                tbTitle.Text = "";
                tbMessage.Text = "";
                cb_link.Checked = false;
                dlLink.SelectedIndex = -1;
                tbLinkURL.Text = "";
                cb_schedule.Checked = false;
                tbDate.Text = "";
                tbTime.Text = "";
                lbType.Text = "Add";
                lbFile1.Text = "";
                lbFile2.Text = "";
                dlDestination_SelectedIndexChanged(null, null);
                dlLink_SelectedIndexChanged(null, null);
                cb_link_CheckedChanged(null, null);
                cb_schedule_CheckedChanged(null, null);

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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                string delFlag = Convert.ToString(drv["DEL_FLAG"]);
                string statusFlag = Convert.ToString(drv["status"]);

                LinkButton _btnEdit = (LinkButton)e.Row.FindControl("gvBtnEdit");
                LinkButton _btnDelete = (LinkButton)e.Row.FindControl("gvBtnDelete");

                if (delFlag == "Y")
                {
                    _btnEdit.Visible = false;
                    _btnDelete.Visible = false;

                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.Font.Italic = true;
                        cell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#888");
                    }
                }
                if ((statusFlag == "Send") || (statusFlag == "Faild"))
                {
                    _btnDelete.Visible = false;
                }
            }
        }

        protected void gvBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                
                btnSave.Visible = false;

                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                lbType.Text = "";
                lbId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["id"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;
                    //btnSave.Visible = true;

                    NotificationService srv = new NotificationService();
                    DataRow row = srv.GetNotificationById(id);
                    if (row != null)
                    {
                        dlDestination.Items.Clear();
                        dlDestination.Items.Add(new ListItem { Text = "All", Value = "All" });
                        dlDestination.Items.Add(new ListItem { Text = "Member", Value = "Member" });
                        dlDestination.Items.Add(new ListItem { Text = "Device", Value = "Device" });
                        dlDestination.Items.Add(new ListItem { Text = "Mobile OS", Value = "Mobile OS" });

                        dlDestination.SelectedValue = row["destination"].ToString();
                        dlDestination_SelectedIndexChanged(null, null);
                        dlSubDestination.Items.Add(new ListItem { Text = "None", Value = "none" });
                        dlSubDestination.SelectedValue = row["sub_destination"].ToString();
                        tbMemberID.Text = row["member_id"].ToString();
                        tbDeviceID.Text = row["device_id"].ToString();
                        tbTitle.Text = row["title"].ToString();
                        tbMessage.Text = row["message"].ToString();
                        cb_link.Checked = (int.Parse(row["is_link"].ToString()) == 1 ? true : false);
                        dlLink.SelectedValue = row["link_type"].ToString();
                        dlLink_SelectedIndexChanged(null, null);
                        tbLinkURL.Text = row["link_url"].ToString();
                        cb_schedule.Checked = (int.Parse(row["is_schedule"].ToString()) == 1 ? true : false);
                        tbDate.Text = (row["schedule_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["schedule_date"]).ToString("dd/MM/yyyy");
                        tbTime.Text = (row["schedule_time"] == DBNull.Value) ? "" : Convert.ToDateTime(row["schedule_time"]).ToString("HH:mm");

                        dlDestination.Enabled = false;
                        dlSubDestination.Enabled = false;
                        tbMemberID.Enabled = false;
                        tbDeviceID.Enabled = false;
                        tbTitle.Enabled = false;
                        tbMessage.Enabled = false;
                        cb_link.Enabled = false;
                        dlLink.Enabled = false;
                        tbLinkURL.Enabled = false;
                        cb_schedule.Enabled = false;
                        tbDate.Enabled = false;
                        tbTime.Enabled = false;
                        dlAction.Enabled = false;



                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                        upModalAdd.Update();
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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                if (dlDestination.SelectedValue == "All")
                {
                    dlSubDestination.Visible = false;
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                    dlSubDestination.Items.Clear();
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

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModalAdd.Update();
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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
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
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void UploadFile_Click(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("~/Default.aspx", false);
            }
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnUpFile1":
                    UploadFile(FileType.File1);
                    break;
                case "btnUpFile2":
                    UploadFile(FileType.File2);
                    break;
            }
        }

        protected void DeleteFile_Click(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("~/Default.aspx", false);
            }
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnDelFile1":
                    DeleteFile(FileType.File1);
                    break;
                case "btnDelFile2":
                    DeleteFile(FileType.File2);
                    break;
            }
        }

        private void UploadFile(FileType type)
        {
            try
            {
                FileUpload fileUpload = upModalAdd.FindControl("up" + type.ToString()) as FileUpload;
                TextBox tbFile = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;

                lbStatus.Text = "";

                if (fileUpload.HasFile)
                {
                    //string filename = Server.MapPath("~/Excel//" + fileUpload.FileName);
                    string filename = ViewState["NotiPath"] + "\\" + fileUpload.FileName;
                    if (File.Exists(filename))
                        File.Delete(filename);
                    fileUpload.SaveAs(filename);
                    tbFile.Text = fileUpload.FileName;

                    DataTable dt = ProcessExcel(filename);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new Exception("This file cannot open or havenot data!");
                    }
                    else
                    {
                        ViewState["noti_member_list"] = dt;
                    }
                }
                else
                {
                    throw new Exception("No file selected!");
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;
                lbStatus.Text = "Upload file error. Please try again later.";// ex.Message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModalAdd.Update();
            }
        }

        private DataTable ProcessExcel(string filepath)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            string strConnection;

            if (Path.GetExtension(filepath.ToLower()) == ".xls")
                strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"";
            else
                strConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";

            OleDbConnection exconn = new OleDbConnection(strConnection);
            exconn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT member_id FROM[Sheet1$] ", exconn);
            da.Fill(dt);
            exconn.Close();

            return dt;
        }

        private void DeleteFile(FileType type)
        {
            try
            {
                string imgDir = ViewState["NotiPath"] + "\\";

                TextBox tbFile = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;

                if (tbFile.Text != "")
                {
                    if (File.Exists(imgDir + tbFile.Text))
                        File.Delete(imgDir + tbFile.Text);
                    tbFile.Text = "";
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dlLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Session["User"] == null)
            //{
            //    Response.Redirect("~/Default.aspx", false);
            //}
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

        protected void cb_schedule_CheckedChanged(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("~/Default.aspx", false);
            }
            if (cb_schedule.Checked == true)
            {
                tbDate.Enabled = true;
                tbTime.Enabled = true;
            }
            else
            {
                tbDate.Enabled = false;
                tbTime.Enabled = false;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
            upModalAdd.Update();
        }

        protected void btnDownloadMember_ServerClick(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(WebConfigurationManager.AppSettings["TemplatePath"] + "\\GroupMember_Template.xls");
            byte[] fileConent = File.ReadAllBytes(WebConfigurationManager.AppSettings["TemplatePath"] + "\\GroupMember_Template.xls");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(fileConent);
            HttpContext.Current.Response.End();
        }

        protected void btnDownloadDevice_ServerClick(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(WebConfigurationManager.AppSettings["TemplatePath"] + "\\GroupDevice_Template.xls");
            byte[] fileConent = File.ReadAllBytes(WebConfigurationManager.AppSettings["TemplatePath"] + "\\GroupDevice_Template.xls");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(fileConent);
            HttpContext.Current.Response.End();
        }

    }
}