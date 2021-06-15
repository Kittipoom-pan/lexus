using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;

namespace com.lexus.th.web.master
{
    public partial class PageEvent : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        private enum ImageType { Img1, ImgBanner1 }
        private string Banner = "BANNER";
        private string BannerDetail = "DETAIL";
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

                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathEvent"];
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();
                        ViewState[Banner] = new List<UploadImageModel>();
                        ViewState[BannerDetail] = new List<UploadImageModel>();
                        chkgAllocationOfRights.DataSource = new System.Data.DataTable();
                        chkgAllocationOfRights.DataBind();
           
                        BindEventGroup();

                        gvQuestion.DataSource = new System.Data.DataTable();
                        gvQuestion.DataBind();

                        BindPreferredModel();

                        gvAnswer.DataSource = new System.Data.DataTable();
                        gvAnswer.DataBind();

                        lbNotSet.Visible = false;
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
                EventService srv = new EventService();
                gvCust.DataSource = srv.GetEvents(txtSearch.Text);
                gvCust.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindPeriodCarAge()
        {
            try
            {
                dlCarAgeStart.Items.Clear();
                dlCarAgeEnd.Items.Clear();
                string PeriodCarAgeMax = System.Web.Configuration.WebConfigurationManager.AppSettings["PeriodCarAgeMax"];
                int max = Convert.ToInt32(PeriodCarAgeMax);

                for (int i = 0; i <= max; i++)
                {
                    dlCarAgeStart.Items.Add(i.ToString());
                    dlCarAgeStart.DataBind();
                }

                for (int i = 1; i <= max; i++)
                {
                    dlCarAgeEnd.Items.Add(i.ToString());
                    dlCarAgeEnd.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindEventGroup()
        {
            chkgAllocationOfRights.DataSource = new EventService().GetEventGroup();
            chkgAllocationOfRights.DataBind();
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
                DeleteImage(ImageType.Img1);

                string title = tbTitle.Text;
                string date = tbDate.Text;
                string desc = tbDesc.Text;
                string dispStart = tbDispStart.Text;
                string dispEnd = tbDispEnd.Text;
                string condition = tbCondition.Text;
                string regPeriod = tbRegPeriod.Text;
                string regPeriodStart = tbRegPeriondStart.Text;
                string regPeriodEnd = tbRegPeriondEnd.Text;
                string eventStart = tbEventStart.Text;
                string eventEnd = tbEventEnd.Text;
                string limitGuest = tbLimitGuest.Text;
                string user = Session["User"].ToString();
                string descTH = tbDescTH.Text;
                string conditionTH = tbConditionTH.Text;
                string regPeriodTH = tbRegPeriodTH.Text;
                string without_guest = dlWithoutGuest.SelectedValue;
                string car_owner_only = dlCarOwnerOnly.SelectedValue;
                string event_type = dlEventType.SelectedValue;
                string thank_you_message_th = tbThankYouMsgTH.Text;
                string thank_you_message_en = tbThankYouMsgEN.Text;
                string one_member_per_event = cbOneVinPerEvent.Checked ? "0" : "1";// ใน base ใช้ one member per event backoffice ใช้ 1 vin per event
                string car_age_start = dlCarAgeStart.SelectedValue;
                string car_age_end = dlCarAgeEnd.SelectedValue;

                if(Convert.ToInt32(car_age_start) > Convert.ToInt32(car_age_end))
                {
                    ClientPopup(ModalType.Warning, "Please Choose period car age start < period car age end");
                    return;
                }

                DateTime DisStart = DateTime.ParseExact(dispStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime DisEnd = DateTime.ParseExact(dispEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime PerStart = DateTime.ParseExact(regPeriodStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime PerEnd = DateTime.ParseExact(regPeriodEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime EventStart = DateTime.ParseExact(eventStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime EventEnd = DateTime.ParseExact(eventEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                List<UploadImageModel> banner = (List<UploadImageModel>)ViewState[Banner];
                List<UploadImageModel> bannerDetail = (List<UploadImageModel>)ViewState[BannerDetail];

                List<string> event_group = new List<string>();
                foreach (ListItem item in chkgAllocationOfRights.Items)
                {
                    if (item.Selected)
                    {
                        event_group.Add(item.Value);
                    }
                }
                string event_group_join = string.Join(",", event_group);

                //List<string> event_period_time = new List<string>();
                //foreach (ListItem item in chkPeriodTime.Items)
                //{
                //    if (item.Selected)
                //    {
                //        event_period_time.Add(item.Value);
                //    }
                //}
                //string event_period_join = string.Join(",", event_period_time);

                List<PreferredModel> Preferred = new List<PreferredModel>();
                if (chkPreferredModel.Items.Count > 0)
                {
                    foreach (ListItem item in chkPreferredModel.Items)
                    {
                        if (item.Selected)
                        {
                            Preferred.Add(new PreferredModel() { id = int.Parse(item.Value), name = item.Text });
                        }
                    }
                }
                string json = JsonConvert.SerializeObject(Preferred);
                //string PreferredModelIds = json;
                if (Preferred.Count == 0)
                {
                    lbRequiredPreferredModel.Visible = true;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                    upModalAdd.Update();
                }
                else
                {
                    lbRequiredPreferredModel.Visible = false;

                    if (DisEnd < DisStart)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Display End > Display Start");
                    }
                    else if (PerEnd < PerStart)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Period End > Period Start");
                    }
                    else if (EventEnd < EventStart)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose Event End > Event Start");
                    }
                    else
                    {
                        EventService srv = new EventService();
                        if ((descTH.Contains("'")) || (desc.Contains("'")))
                        {
                            ClientPopup(ModalType.Warning, "Don't used a single quote");
                        }
                        else
                        {
                            if (lbType.Text == "Add")
                            {
                                srv.AddEvent(title, date, desc, dispStart, dispEnd, condition, regPeriod, regPeriodStart, regPeriodEnd, eventStart, eventEnd, limitGuest, user, descTH, conditionTH, regPeriodTH, without_guest, car_owner_only, thank_you_message_en, thank_you_message_th, one_member_per_event, banner, bannerDetail, event_group_join, event_type, json, car_age_start, car_age_end);
                                BindGridData();
                                ClientPopup(ModalType.Success, "Completed");
                            }
                            else if (lbType.Text == "Edit")
                            {
                                srv.UpdateEvent(title, date, desc, dispStart, dispEnd, condition, regPeriod, regPeriodStart, regPeriodEnd, eventStart, eventEnd, limitGuest, user, lbId.Text, descTH, conditionTH, regPeriodTH, without_guest, car_owner_only, thank_you_message_en, thank_you_message_th, one_member_per_event, banner, bannerDetail, event_group_join, event_type, json, car_age_start, car_age_end);
                                BindGridData();
                                ClientPopup(ModalType.Success, "Completed");
                            }
                        }
                    }
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

                EventService srv = new EventService();
                srv.DeleteEvent(id, user);

                BindGridData();
                ClientPopup(ModalType.Success, "Completed");
                BindGridQuestionData("");
                btnAddAnswer.Visible = false;
                BindGridAnswerData("");
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
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddQuestion", "$('#mdAddQuestion').modal('hide');", true);
            upModalQuestion.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal('hide');", true);
            upModalAddAnswer.Update();
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdWarning').modal('hide');", true);
            //upModalWarning.Update();

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
                tbTitle.Text = "";
                tbDate.Text = "";
                tbDesc.Text = "";
                tbDispStart.Text = "";
                tbDispEnd.Text = "";
                tbCondition.Text = "";
                tbRegPeriod.Text = "";
                tbRegPeriondStart.Text = "";
                tbRegPeriondEnd.Text = "";
                tbEventStart.Text = "";
                tbEventEnd.Text = "";
                tbLimitGuest.Text = "";
                lbType.Text = "Add";
                tbDescTH.Text = "";
                tbConditionTH.Text = "";
                tbRegPeriodTH.Text = "";
                lbImg1.Text = "";
                lbImgBanner1.Text = "";
                tbThankYouMsgTH.Text = "";
                tbThankYouMsgEN.Text = "";
                lbImgBanner1.Text = "";
                dlWithoutGuest.SelectedIndex = -1;
                dlCarOwnerOnly.SelectedIndex = -1;
                dlCarAgeStart.SelectedIndex = -1;
                dlCarAgeEnd.SelectedIndex = -1;
                cbOneVinPerEvent.Checked = false;

                ViewState[Banner] = new List<UploadImageModel>();
                ViewState[BannerDetail] = new List<UploadImageModel>();
                gvBanner.DataSource = (List<UploadImageModel>)ViewState[Banner];
                gvBanner.DataBind();
                gvBannerDetail.DataSource = (List<UploadImageModel>)ViewState[BannerDetail];
                gvBannerDetail.DataBind();
                BindEventGroup();
                InitialPreferred();
                lbRequiredPreferredModel.Visible = false;
                pnPreferred.Visible = false;
                pnDetail.Visible = true;
                BindPeriodCarAge();

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

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvCust.DataKeys != null)
                    {
                        lbDelId.Text = gvCust.DataKeys[index].Values["ID"].ToString();
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
                lbImg1.Text = "";
                lbImgBanner1.Text = "";
                ViewState[Banner] = null;
                ViewState[BannerDetail] = null;
                BindEventGroup();
                lbRequiredPreferredModel.Visible = false;
                pnPreferred.Visible = false;
                pnDetail.Visible = true;
                InitialPreferred();
                BindPeriodCarAge();

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    EventService srv = new EventService();
                    DataRow row = srv.GetEventById(id);
                    if (row != null)
                    {
                        tbTitle.Text = row["TITLE"].ToString();
                        tbDate.Text = (row["DATE"] == DBNull.Value) ? "" : Convert.ToDateTime(row["DATE"]).ToString("dd/MM/yyyy");
                        tbDesc.Text = row["DESC"].ToString();
                        tbDispStart.Text = (row["DISPLAY_START"] == DBNull.Value) ? "" : Convert.ToDateTime(row["DISPLAY_START"]).ToString("dd/MM/yyyy HH:mm");
                        tbDispEnd.Text = (row["DISPLAY_END"] == DBNull.Value) ? "" : Convert.ToDateTime(row["DISPLAY_END"]).ToString("dd/MM/yyyy HH:mm");
                        tbCondition.Text = row["CONDITION"].ToString();
                        tbRegPeriod.Text = row["REG_PERIOD"].ToString();
                        tbRegPeriondStart.Text = (row["REG_PERIOD_START"] == DBNull.Value) ? "" : Convert.ToDateTime(row["REG_PERIOD_START"]).ToString("dd/MM/yyyy");
                        tbRegPeriondEnd.Text = (row["REG_PERIOD_END"] == DBNull.Value) ? "" : Convert.ToDateTime(row["REG_PERIOD_END"]).ToString("dd/MM/yyyy HH:mm");
                        tbEventStart.Text = (row["EVENT_START"] == DBNull.Value) ? "" : Convert.ToDateTime(row["EVENT_START"]).ToString("dd/MM/yyyy HH:mm");
                        tbEventEnd.Text = (row["EVENT_END"] == DBNull.Value) ? "" : Convert.ToDateTime(row["EVENT_END"]).ToString("dd/MM/yyyy HH:mm");
                        tbLimitGuest.Text = row["LIMIT_GUEST"].ToString();
                        tbDescTH.Text = row["desc_th"].ToString();
                        tbConditionTH.Text = row["condition_th"].ToString();
                        tbRegPeriodTH.Text = row["reg_period_th"].ToString();
                        tbThankYouMsgEN.Text = row["thankyou_message_en"].ToString();
                        tbThankYouMsgTH.Text = row["thankyou_message_th"].ToString();
                        dlWithoutGuest.SelectedValue = row["without_guest"].ToString();
                        dlCarOwnerOnly.SelectedValue = row["car_owner_only"].ToString();
                        dlEventType.SelectedValue = row["event_type"].ToString();
                        dlCarAgeStart.SelectedValue = (row["period_car_age_start"] == DBNull.Value) ? "0" : row["period_car_age_start"].ToString();
                        dlCarAgeEnd.SelectedValue = (row["period_car_age_end"] == DBNull.Value) ? "1" : row["period_car_age_end"].ToString();

                        if(row["one_member_per_event"] == DBNull.Value)
                        {
                            lbNotSet.Visible = true;
                            cbOneVinPerEvent.Checked = false;
                        }
                        else
                        {
                            lbNotSet.Visible = false;
                            cbOneVinPerEvent.Checked = Convert.ToInt16(row["one_member_per_event"]) == 1 ? false : true;
                        }


                        //cbOneVinPerEvent.Checked = Convert.ToBoolean(row["one_member_per_event"].ToString()) ? false : true;

                        if (row["event_group_ids"] != DBNull.Value)
                        {
                            List<string> groups = row["event_group_ids"].ToString().Split(',').ToList();
                            foreach (var group in groups)
                            {
                                foreach (ListItem chk in chkgAllocationOfRights.Items)
                                {
                                    if (chk.Value == group)
                                    {
                                        chk.Selected = true;
                                    }
                                }
                            }
                        }

                        List<PreferredModel> prefModel = JsonConvert.DeserializeObject<List<PreferredModel>>(row["preferred_model_ids"].ToString());
                        if (prefModel != null)
                        {
                            foreach (var model in prefModel)
                            {
                                foreach (ListItem chk in chkPreferredModel.Items)
                                {
                                    if (chk.Value == model.id.ToString())
                                    {
                                        chk.Selected = true;
                                    }
                                }
                            }
                            int count = 0;
                            foreach (ListItem item in chkPreferredModel.Items)
                            {
                                if (item.Selected)
                                {
                                    count++;
                                }
                            }
                            chkPreferredModelSelectAll.Checked = (count == chkPreferredModel.Items.Count);

                        }

                        BindGridBannerData();
                        BindGridBannerDetailData();

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                        upModalAdd.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot get data. Please try again later.");
            }
        }
        protected void gvBtnView_Click(object sender, EventArgs e)
        {
            try
            {
                lbQEventId.Text = "";
                lbShowEventId.Text = "";
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();

                    lbQEventId.Text = id;
                    lbShowEventId.Text = "Question For Event ID: " + id;
                    BindGridQuestionData(id);
                    btnAddAnswer.Visible = false;
                    BindGridAnswerData("");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindGridQuestionData(string event_id)
        {
            try
            {
                EventService srv = new EventService();
                gvQuestion.DataSource = srv.GetEventQuestionByEventId(event_id);
                gvQuestion.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridAnswerData(string question_id)
        {
            try
            {
                EventService srv = new EventService();
                gvAnswer.DataSource = srv.GetEventAnswerByQuestionId(question_id);
                gvAnswer.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvQuestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCust.PageIndex = e.NewPageIndex;
                BindGridData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvQuestion.SelectedIndex;
                if (gvQuestion.DataKeys != null)
                {
                    string id = gvQuestion.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvQuestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
            if (lnkFull != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
            }
        }
        protected void gvQuestion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelQuestionId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvQuestion.DataKeys != null)
                    {
                        lbDelQuestionId.Text = gvQuestion.DataKeys[index].Values["ID"].ToString();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmQuestion", "$('#mdDelConfirmQuestion').modal();", true);
                        upModalConfirmDeleteQuestion.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnAddModalQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbQEventId.Text == "")
                {
                    ClientPopup(ModalType.Warning, "Please select Event!!");
                    return;
                }
                lbId.Text = "";
                tbQuestionTH.Text = "";
                tbQuestionEN.Text = "";
                dlActiveQuestion.SelectedIndex = -1;
                dlAnswerType.SelectedIndex = -1;
                dlQuestionType.SelectedIndex = -1;
                tbOrderQuestion.Text = "";

                lbQuestionType.Text = "Add";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddQuestion", "$('#mdAddQuestion').modal();", true);
                upModalQuestion.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvQuestionBtnView_Click(object sender, EventArgs e)
        {
            try
            {
                lbAQuestionId.Text = "";
                lbAnswerId.Text = "";
                lbShowQuestionId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvQuestion.DataKeys != null)
                {
                    string id = gvQuestion.DataKeys[index].Values["ID"].ToString();

                    lbAQuestionId.Text = id;
                    lbShowQuestionId.Text = "Answer For Question ID: " + id;

                    DataRow qs = new EventService().GetQuestionById(id);
                    lbQAnswerType.Text = qs["answer_type"].ToString();
                    if (lbQAnswerType.Text == "2")
                    {
                        btnAddAnswer.Visible = false;
                    }
                    else
                    {
                        btnAddAnswer.Visible = true;
                    }

                    BindGridAnswerData(id);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvQuestionBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvQuestion.DataKeys != null)
                {
                    string id = gvQuestion.DataKeys[index].Values["ID"].ToString();
                    lbQuestionType.Text = "Edit";
                    lbQuestionId.Text = id;

                    EventService srv = new EventService();
                    DataRow row = srv.GetQuestionById(id);
                    if (row != null)
                    {
                        tbQuestionTH.Text = row["question_en"].ToString();
                        tbQuestionEN.Text = row["question_th"].ToString();
                        dlActiveQuestion.SelectedValue = row["is_active"].ToString();
                        dlQuestionType.SelectedValue = row["question_type"].ToString();
                        dlAnswerType.SelectedValue = row["answer_type"].ToString();
                        tbOrderQuestion.Text = row["order"].ToString();

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddQuestion", "$('#mdAddQuestion').modal();", true);
                        upModalQuestion.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnSaveQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                string event_id = lbQEventId.Text;
                string question_id = lbQuestionId.Text;
                string user = Session["User"].ToString();
                string name_th = tbQuestionTH.Text;
                string name_en = tbQuestionEN.Text;
                string active = dlActiveQuestion.SelectedValue;
                string order = tbOrderQuestion.Text;
                string questionType = dlQuestionType.SelectedValue;
                string answerType = dlAnswerType.SelectedValue;
                //string image = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";

                lbAQuestionId.Text = ""; // Clear Answer

                EventService srv = new EventService();
                if (lbQuestionType.Text == "Add")
                {
                    srv.AddQuestion(event_id, name_th, name_en, active, questionType, answerType, user, order);
                    BindGridQuestionData(event_id);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbQuestionType.Text == "Edit")
                {
                    srv.UpdateQuestion(question_id, name_th, name_en, active, questionType, answerType, user, order);
                    BindGridQuestionData(event_id);
                    //BindGridAnswerData(lbId.Text);
                    ClientPopup(ModalType.Success, "Completed");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnDeleteQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelQuestionConfirm", "$('#mdDelConfirmQuestion').modal('hide');", true);
                upModalConfirmDeleteQuestion.Update();

                string user = Session["User"].ToString();
                string id = lbDelQuestionId.Text;

                lbDelAnswerId.Text = ""; // Clear color

                EventService srv = new EventService();
                srv.DeleteEventQuestion(id, user);

                BindGridQuestionData(lbQEventId.Text);
                btnAddAnswer.Visible = false;
                BindGridAnswerData("");
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void gvAnswer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAnswer.PageIndex = e.NewPageIndex;
                string id = lbAQuestionId.Text;
                if (id != "")
                {
                    BindGridAnswerData(id);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvAnswer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelAnswerId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvAnswer.DataKeys != null)
                    {
                        //lbDelAnswerModelId.Text = gvAnswer.DataKeys[index].Values["question_id"].ToString();
                        lbDelAnswerId.Text = gvAnswer.DataKeys[index].Values["ID"].ToString();

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmAnswer", "$('#mdDelConfirmAnswer').modal();", true);
                        upModalConfirmDeleteAnswer.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvAnswer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvAnswer.SelectedIndex;
                if (gvAnswer.DataKeys != null)
                {
                    string id = gvAnswer.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvAnswer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void btnAddAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbQAnswerType.Text == "2")
                {
                    ClientPopup(ModalType.Warning, "Can't create answer!!");
                }

                if (lbAQuestionId.Text != "")
                {
                    lbAnswerType.Text = "Add";
                    lbAnswerId.Text = "";
                    tbAnswerTH.Text = "";
                    tbAnswerEN.Text = "";
                    dlActiveAnswer.SelectedIndex = -1;
                    tbOrderAnswer.Text = "";
                    dlOptional.SelectedIndex = -1;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal();", true);
                    upModalAddAnswer.Update();
                }
                else
                {
                    ClientPopup(ModalType.Error, "Please select Answer!!");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBtnEditAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                lbAnswerType.Text = "";
                lbAnswerId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvAnswer.DataKeys != null)
                {
                    string questionId = gvAnswer.DataKeys[index].Values["question_id"].ToString();
                    string answerId = gvAnswer.DataKeys[index].Values["ID"].ToString();

                    lbAnswerType.Text = "Edit";
                    lbAnswerId.Text = answerId;

                    EventService srv = new EventService();

                    DataRow row = srv.GetAnswerById(answerId);
                    if (row != null)
                    {
                        tbAnswerTH.Text = row["answer_th"].ToString();
                        tbAnswerEN.Text = row["answer_en"].ToString();
                        dlActiveAnswer.SelectedValue = row["is_active"].ToString();
                        tbOrderAnswer.Text = row["order"].ToString();
                        dlOptional.SelectedValue = row["is_optional"].ToString();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal();", true);
                        upModalAddAnswer.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnSaveAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                string user = Session["User"].ToString();
                string questionId = lbAQuestionId.Text;
                string answerId = lbAnswerId.Text;

                string answer_th = tbAnswerTH.Text;
                string answer_en = tbAnswerEN.Text;
                string active = dlActiveAnswer.SelectedValue;
                string order = tbOrderAnswer.Text;
                string is_optional = dlOptional.SelectedValue;

                EventService srv = new EventService();
                if (lbAnswerType.Text == "Add")
                {
                    srv.AddAnswer(questionId, answer_en, answer_th, active, user, order, is_optional);
                    BindGridAnswerData(questionId);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbAnswerType.Text == "Edit")
                {
                    srv.UpdateAnswer(answer_en, answer_th, active, user, answerId, order, is_optional);
                    BindGridAnswerData(questionId);
                    ClientPopup(ModalType.Success, "Completed");
                }

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal('hide');", true);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnDeleteAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmAnswer", "$('#mdDelConfirmAnswer').modal('hide');", true);
                upModalConfirmDeleteAnswer.Update();

                string user = Session["User"].ToString();

                EventService srv = new EventService();
                srv.DeleteAnswer(lbDelAnswerId.Text, user);

                BindGridAnswerData(lbAQuestionId.Text);
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void UploadImage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnUpImg1":
                    UploadImage(ImageType.Img1);
                    break;
                case "btnUpImgBanner1":
                    UploadImage(ImageType.ImgBanner1);
                    break;

            }
        }

        private void UploadImage(ImageType type)
        {
            try
            {
                string imgDir = ViewState["ImagePath"] + "\\";

                FileUpload fileUpload = upModalAdd.FindControl("up" + type.ToString()) as FileUpload;
                if (fileUpload.HasFile)
                {
                    foreach (var file in fileUpload.PostedFiles)
                    {
                        if ((fileUpload.PostedFile.ContentType == "image/jpeg") || (fileUpload.PostedFile.ContentType == "image/png"))
                        {
                        }
                        else
                        {
                            throw new Exception("Allow file type JPG and PNG only!");
                        }
                    }

                    foreach (var file in fileUpload.PostedFiles)
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream))
                        {
                            string originalFileName = file.FileName;
                            string name = originalFileName.Replace(new FileInfo(file.FileName).Extension, string.Empty);
                            string fileName = string.Format("{0}{1}{2}{3}", originalFileName.Replace(new FileInfo(file.FileName).Extension, string.Empty), type == ImageType.ImgBanner1 ? Banner : BannerDetail, DateTime.Now.ToString("yyyyMMddHHmmssfff"), new FileInfo(file.FileName).Extension);
                            string filePath = imgDir + fileName;
                            if (!Directory.Exists(imgDir))
                            {
                                Directory.CreateDirectory(imgDir);
                            }
                            if (file.FileName != "")
                            {
                                if (File.Exists(filePath))
                                    File.Delete(filePath);
                            }

                            file.SaveAs(filePath);
                            List<UploadImageModel> imgs = (List<UploadImageModel>)ViewState[(type == ImageType.ImgBanner1) ? Banner : BannerDetail];
                            imgs.Add(new UploadImageModel()
                            {
                                ID = "",
                                ParrentID = lbId.Text,
                                FileName = fileName,
                                OriginalFileName = originalFileName,
                                FilePath = ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + fileName),
                                Page = "NEWS",
                                Type = type == ImageType.ImgBanner1 ? Banner : BannerDetail,
                                Status = "NEW"
                            });
                            ViewState[(type == ImageType.ImgBanner1) ? Banner : BannerDetail] = imgs;

                            //Add Filename and FilePath to Grid
                            // tbImage.Text = fileUpload.FileName;
                        }
                    }
                }
                else
                {
                    throw new Exception("No file selected!");
                }

                if (type == ImageType.ImgBanner1)
                {
                    BindGridBannerData();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();bannerTab()", true);
                    upModalAdd.Update();

                }
                else
                {
                    BindGridBannerDetailData();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();bannerDetailTab()", true);
                    upModalAdd.Update();

                }

            }
            catch (Exception ex)
            {

                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;
                lbStatus.Text = "Upload image error. Please try again later.";// ex.Message;
                if (type == ImageType.ImgBanner1)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();bannerTab()", true);
                    upModalAdd.Update();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();bannerDetailTab()", true);
                    upModalAdd.Update();

                }
            }
        }
        private void DeleteImage(ImageType type)
        {
            try
            {
                string imgDir = ViewState["ImagePath"] + "\\";

                foreach (UploadImageModel img in ((List<UploadImageModel>)ViewState[Banner]).Where(c => c.Status == "DEL").ToList())
                {
                    if (File.Exists(imgDir + img.FileName))
                        File.Delete(imgDir + img.FileName);
                }
                foreach (UploadImageModel img in ((List<UploadImageModel>)ViewState[BannerDetail]).Where(c => c.Status == "DEL").ToList())
                {
                    if (File.Exists(imgDir + img.FileName))
                        File.Delete(imgDir + img.FileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGridBannerData()
        {
            try
            {
                if (ViewState[Banner] == null)
                {
                    EventService srv = new EventService();
                    DataTable dt = srv.GetUploadImage(lbId.Text, "BANNER");
                    if (dt != null)
                    {
                        ViewState[Banner] = dt.AsEnumerable().Select(c => new UploadImageModel()
                        {
                            ID = c.Field<int>("Id").ToString(),
                            ParrentID = c.Field<int>("Parent_Id").ToString(),
                            Type = c.Field<string>("Type"),
                            Page = c.Field<string>("Page"),
                            FileName = c.Field<string>("File_Name"),
                            FilePath = c.Field<string>("File_Path"),
                            OriginalFileName = c.Field<string>("Original_File_Name")
                        }).ToList();
                    }
                    else
                    {
                        ViewState[Banner] = new List<UploadImageModel>();
                    }
                }
                gvBanner.DataSource = ((List<UploadImageModel>)ViewState[Banner]).Where(c => c.Status != "DEL").ToList();
                gvBanner.DataBind();
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvBanner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBanner.PageIndex = e.NewPageIndex;
                BindGridBannerData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvBanner.SelectedIndex;
                if (gvBanner.DataKeys != null)
                {
                    string id = gvBanner.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBanner_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    //lbDelId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvBanner.DataKeys != null)
                    {

                        string key = gvBanner.DataKeys[index].Values["FileName"].ToString();
                        List<UploadImageModel> imgs = (List<UploadImageModel>)ViewState[Banner];
                        imgs.Where(c => c.FileName.Equals(key)).ToList().ForEach(n => n.Status = "DEL");
                        ViewState[Banner] = imgs;
                        BindGridBannerData();

                    }
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();bannerTab()", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBanner_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Image img = (Image)e.Row.FindControl("Image1");
            if (img == null)
            {
                return;
            }
            else
            {

                string imgDir = ViewState["ImagePath"] + "\\" + DataBinder.Eval(e.Row.DataItem, "FileName");
                img.ImageUrl = imgDir;
            }
        }
        private void BindGridBannerDetailData()
        {
            try
            {
                if (ViewState[BannerDetail] == null)
                {
                    EventService srv = new EventService();
                    DataTable dt = srv.GetUploadImage(lbId.Text, "DETAIL");
                    if (dt != null)
                    {
                        ViewState[BannerDetail] = dt.AsEnumerable().Select(c => new UploadImageModel()
                        {
                            ID = c.Field<int>("Id").ToString(),
                            ParrentID = c.Field<int>("Parent_Id").ToString(),
                            Type = c.Field<string>("Type"),
                            Page = c.Field<string>("Page"),
                            FileName = c.Field<string>("File_Name"),
                            FilePath = c.Field<string>("File_Path"),
                            OriginalFileName = c.Field<string>("Original_File_Name")
                        }).ToList();
                    }
                    else
                    {
                        ViewState[BannerDetail] = new List<UploadImageModel>();
                    }
                }
                gvBannerDetail.DataSource = ((List<UploadImageModel>)ViewState[BannerDetail]).Where(c => c.Status != "DEL").ToList();
                gvBannerDetail.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvBannerDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBannerDetail.PageIndex = e.NewPageIndex;
                BindGridBannerDetailData();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBannerDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvBannerDetail.SelectedIndex;
                if (gvBannerDetail.DataKeys != null)
                {
                    string id = gvBannerDetail.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBannerDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    //lbDelId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvBanner.DataKeys != null)
                    {

                        string key = gvBannerDetail.DataKeys[index].Values["FileName"].ToString();
                        List<UploadImageModel> imgs = (List<UploadImageModel>)ViewState[BannerDetail];
                        imgs.Where(c => c.FileName.Equals(key)).ToList().ForEach(n => n.Status = "DEL");
                        ViewState[BannerDetail] = imgs;
                        BindGridBannerDetailData();

                    }
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();bannerDetailTab()", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvBannerDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LinkButton lnkFull = e.Row.FindControl("gvBtnEdit") as LinkButton;
            if (lnkFull != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
            }
        }

        protected void chkgAllocationOfRights_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BindPreferredModel()
        {
            try
            {
                BookingService srv = new BookingService();
                chkPreferredModel.DataSource = srv.GetPreferredModel();
                chkPreferredModel.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Menu_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (btn.ID == "btnPreferred")
                {
                    pnPreferred.Visible = true;
                    pnDetail.Visible = false;
                }
                else if (btn.ID == "btnDetail")
                {
                    pnPreferred.Visible = false;
                    pnDetail.Visible = true;
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();initailDateTime();", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void chkPreferredModelSelectAll_CheckedChanged(object sender, EventArgs e)
        {

            foreach (ListItem item in chkPreferredModel.Items)
            {
                item.Selected = chkPreferredModelSelectAll.Checked;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();initailDateTime();", true);
            upModalAdd.Update();
        }

        protected void chkPreferredModel_SelectedIndexChanged(object sender, EventArgs e)
        {

            int count = 0;
            foreach (ListItem item in chkPreferredModel.Items)
            {
                if (item.Selected)
                {
                    count++;
                }
            }
            chkPreferredModelSelectAll.Checked = (count == chkPreferredModel.Items.Count);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();initailDateTime();", true);
            upModalAdd.Update();
        }
        private void InitialPreferred()
        {
            chkPreferredModelSelectAll.Checked = false;
            foreach (ListItem item in chkPreferredModel.Items)
            {
                item.Selected = false;
            }
        }
    }
}