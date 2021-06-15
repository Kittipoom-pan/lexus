using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Globalization;
using System.Data.OleDb;
using System.Web.Configuration;
using OfficeOpenXml;

namespace com.lexus.th.web.master
{
    public partial class PageSurvey : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        private enum ImageType { Img1 }
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
                        string uploadPath = WebConfigurationManager.AppSettings["UploadSurveyPath"];
                        ViewState["ImagePath"] = uploadPath;//UploadSurveyPath
                        ViewState["UrlPath"] = WebConfigurationManager.AppSettings["UploadImageSurveyUrl"];
                        ViewState["SurveyMemberPath"] = WebConfigurationManager.AppSettings["SurveyMemberPath"];
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();

                        gvQuestion.DataSource = new System.Data.DataTable();
                        gvQuestion.DataBind();

                        gvChoice.DataSource = new System.Data.DataTable();
                        gvChoice.DataBind();

                        ViewState["Depend"] = new List<QuestionRuleRPModel>();
                        ViewState["QuestionRule"] = new List<QuestionRuleModel>();

                        dlDestination.Items.Clear();
                        dlDestination.Items.Add(new ListItem { Text = "All", Value = "1" });
                        dlDestination.Items.Add(new ListItem { Text = "Member", Value = "2" });
                        dlDestination.Items.Add(new ListItem { Text = "Device", Value = "3" });
                        dlDestination.Items.Add(new ListItem { Text = "Mobile OS", Value = "4" });

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
                SurveyService srv = new SurveyService();
                gvCust.DataSource = srv.GetSurvey(txtSearch.Text);
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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                //DeleteImage(ImageType.Img1);
                string id = lbId.Text;
                string survey_th = tbSurveyTH.Text;
                string survey_en = tbSurveyEN.Text;
                string allow_postpone = dlAllowPostpone.SelectedValue;
                string start_date = tbStartDate.Text;
                string end_date = tbEndDate.Text;
                string interval = tbInterval.Text;
                string times = dlTimes.SelectedValue;
                string ordinal = tbSurveyOrdinal.Text;
                string user = Session["User"].ToString();
                string memberId = tbMemberID.Text;
                string deviceId = tbDeviceID.Text;
                string survey_type = dlDestination.SelectedValue;
                string survey_sub_type = "";
                string file_path = "";
                if (survey_type != "1")
                {
                    survey_sub_type = dlSubDestination.SelectedValue;
                }

                List<SurveyMemberGroupModel> member_group = null;
                if (survey_type == "2" || survey_type == "3")
                {
                    if (survey_type == "2" && survey_sub_type == "1")
                    {
                        if (!string.IsNullOrEmpty(memberId))
                        {
                            member_group = new List<SurveyMemberGroupModel>();
                            member_group.Add(new SurveyMemberGroupModel() { Id = id, MemberId = memberId });
                        }
                    }
                    else if (survey_type == "2" && survey_sub_type == "2")
                    {
                        member_group = (List<SurveyMemberGroupModel>)ViewState["survey_member_list"];
                        file_path = tbFile1.Text;
                    }
                    else if (survey_type == "3" && survey_sub_type == "3")
                    {
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            member_group = new List<SurveyMemberGroupModel>();
                            member_group.Add(new SurveyMemberGroupModel() { Id = id, MemberId = deviceId });
                        }
                    }
                    else if (survey_type == "3" && survey_sub_type == "4")
                    {
                        member_group = (List<SurveyMemberGroupModel>)ViewState["survey_member_list"];
                        file_path = tbFile2.Text;
                    }
                }

                if (!string.IsNullOrWhiteSpace(start_date) && !string.IsNullOrWhiteSpace(end_date))
                {
                    DateTime start = DateTime.ParseExact(start_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (end < start)
                    {
                        ClientPopup(ModalType.Warning, "Please Choose End Date > Start Date");
                        return;
                    }
              
                }
                SurveyService srv = new SurveyService();
                //if ((survey_th.Contains("'")) || (survey_en.Contains("'")))
                //{
                //    ClientPopup(ModalType.Warning, "Don't used a single quote");
                //}
                //else
                //{
                    if (lbType.Text == "Add")
                    {
                        srv.AddSurvey(survey_th, survey_en, allow_postpone, interval, times, start_date, end_date, ordinal, survey_type, survey_sub_type, user, file_path, member_group);
                        BindGridData();
                        ClientPopup(ModalType.Success, "Completed");
                    }
                    else if (lbType.Text == "Edit")
                    {
                        srv.UpdateSurvey(survey_th, survey_en, allow_postpone, interval, times, start_date, end_date, ordinal, survey_type, survey_sub_type, user, id, file_path, member_group);
                        BindGridData();
                        ClientPopup(ModalType.Success, "Completed");
                    }
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

                string id = lbDelId.Text;
                string user = Session["User"].ToString();

                SurveyService srv = new SurveyService();
                srv.DeleteSurvey(id, user);

                BindGridData();
                BindGridQuestionData("");
                BindGridChoiceData("");
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
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddQuestion", "$('#mdAddQuestion').modal('hide');", true);
            upModalQuestion.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddChoice", "$('#mdAddChoice').modal('hide');", true);
            upModalAddChoice.Update();
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

                lbType.Text = "Add";
                tbSurveyTH.Text = "";
                tbSurveyEN.Text = "";
                tbStartDate.Text = "";
                tbEndDate.Text = "";
                dlAllowPostpone.SelectedIndex = -1;
                tbInterval.Text = "1";
                dlTimes.SelectedIndex = -1;
                tbSurveyOrdinal.Text = "";
                lbFile1.Text = "";
                lbFile2.Text = "";
                tbFile1.Text = "";
                tbFile2.Text = "";
                tbMemberID.Text = "";
                tbDeviceID.Text = "";
                ViewState["survey_member_list"] = null;
                dlDestination.SelectedValue = "1";
                dlDestination_SelectedIndexChanged(null, null);
                //dlSubDestination.SelectedValue = "1";
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
                lbFile1.Text = "";
                lbFile2.Text = "";
                tbFile1.Text = "";
                tbFile2.Text = "";
                tbMemberID.Text = "";
                tbDeviceID.Text = "";
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    SurveyService srv = new SurveyService();
                    DataRow row = srv.GetSurveyById(id);
                    DataRow memberRow = srv.GetSurveyMemberGroupBySurveyId(id);
                    if (row != null)
                    {
                        tbSurveyTH.Text = row["survey_th"].ToString();
                        tbSurveyEN.Text = row["survey_en"].ToString();
                        tbStartDate.Text = (row["start_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["start_date"]).ToString("dd/MM/yyyy");
                        tbEndDate.Text = (row["end_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["end_date"]).ToString("dd/MM/yyyy");
                        dlAllowPostpone.SelectedValue = row["allow_postpone"].ToString().ToLower() == "true" ? "1" : "0";// row["allow_postpone"].ToString();
                        tbInterval.Text = row["interval"].ToString();
                        dlTimes.SelectedValue = (row["times"] == DBNull.Value) ? "0" : row["times"].ToString();
                        tbSurveyOrdinal.Text = row["ordinal"].ToString();
                        dlDestination.SelectedValue = (row["survey_type"] == DBNull.Value || (row["survey_type"] != DBNull.Value &&  row["survey_type"].ToString()=="0")) ? "1" : row["survey_type"].ToString();
                        dlDestination_SelectedIndexChanged(null, null);
                        if (dlDestination.SelectedValue != "1")
                        {
                            dlSubDestination.SelectedValue = (row["survey_sub_type"] == DBNull.Value) ? "1" : row["survey_sub_type"].ToString();

                        }
                        if (dlDestination.SelectedValue == "2" && dlSubDestination.SelectedValue == "1")
                        {
                            dlSubDestination.SelectedValue = (row["survey_sub_type"] == DBNull.Value) ? "1" : row["survey_sub_type"].ToString();
                            // memberId
                            tbMemberID.Text = memberRow["member_id"].ToString();
                        }
                        else if (dlDestination.SelectedValue == "2" && dlSubDestination.SelectedValue == "2")
                        {
                            dlSubDestination.SelectedValue = (row["survey_sub_type"] == DBNull.Value) ? "1" : row["survey_sub_type"].ToString();
                            tbFile1.Text = row["file_path"].ToString();
                        }
                        else if (dlDestination.SelectedValue == "3" && dlSubDestination.SelectedValue == "3")
                        {
                            dlSubDestination.SelectedValue = (row["survey_sub_type"] == DBNull.Value) ? "1" : row["survey_sub_type"].ToString();
                            // deviceId 
                            tbDeviceID.Text = memberRow["member_id"].ToString();
                        }
                        else if (dlDestination.SelectedValue == "3" && dlSubDestination.SelectedValue == "4")
                        {
                            dlSubDestination.SelectedValue = (row["survey_sub_type"] == DBNull.Value) ? "1" : row["survey_sub_type"].ToString();
                            tbFile2.Text = row["file_path"].ToString();
                        }
                        if (dlDestination.SelectedValue != "1")
                        {
                            dlSubDestination_SelectedIndexChanged(null, null);
                        }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                        upModalAdd.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot edit. Please try again later.");
            }
        }
        protected void gvBtnView_Click(object sender, EventArgs e)
        {
            try
            {
                lbQSurveyId.Text = "";
                lbShowEventId.Text = "";
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();

                    lbQSurveyId.Text = id;
                    lbShowEventId.Text = "Question For Survey ID: " + id;
                    BindGridQuestionData(id);
                    BindGridChoiceData("");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindGridQuestionData(string survey_id)
        {
            try
            {
                SurveyService srv = new SurveyService();
                gvQuestion.DataSource = srv.GetQuestionBySurveyId(survey_id);
                gvQuestion.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGridChoiceData(string question_id)
        {
            try
            {
                SurveyService srv = new SurveyService();
                gvChoice.DataSource = srv.GetChoiceByQuestionId(question_id);
                gvChoice.DataBind();
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
        private void BindPreviousQuestion()
        {
            SurveyService srv = new SurveyService();
            dlPrevQuestion.DataSource = srv.GetPreviousQuestionBySurveyIdAndQuestionId(lbQSurveyId.Text, lbQuestionId.Text);
            dlPrevQuestion.DataValueField = "id";
            dlPrevQuestion.DataTextField = "Question_TH";
            dlPrevQuestion.DataBind();
        }
        protected void btnAddModalQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbQSurveyId.Text == "")
                {
                    ClientPopup(ModalType.Warning, "Please select Survey!!");
                    return;
                }
                //lbId.Text = "";
                tbQuestionTH.Text = "";
                tbQuestionEN.Text = "";
                tbSuggesttionTH.Text = "";
                tbSuggesttionEN.Text = "";
                dlQuestionType.SelectedIndex = -1;
                dlAllowSuggestion.SelectedIndex = -1;
                dlAllowBack.SelectedIndex = -1;
                dlRequire.SelectedIndex = -1;
                dlRequiredAll.SelectedIndex = -1;
                tbOrderQuestion.Text = "";
                dlDependOn.SelectedIndex = -1;
                pnDepend.Visible = false;
                pnSuggestion.Visible = false;
                if (dlDependOn.SelectedValue == "1")
                {
                    pnDepend.Visible = true;
                }
                ViewState["Depend"] = new List<QuestionRuleRPModel>();
                ViewState["QuestionRule"] = new List<QuestionRuleModel>();
                lbQuestionType.Text = "Add";
                BindPreviousQuestion();
                BindRPDependData();
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
                lbChoiceId.Text = "";
                lbShowQuestionId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvQuestion.DataKeys != null)
                {
                    string id = gvQuestion.DataKeys[index].Values["ID"].ToString();

                    lbAQuestionId.Text = id;
                    lbShowQuestionId.Text = "Choice For Question ID: " + id;

                    DataRow qs = new SurveyService().GetQuestionById(id);
                    lbQChoiceType.Text = qs["question_type"].ToString();
                    List<string> questionTypeHasChoice = new List<string>() { "3", "4" };
                    if (questionTypeHasChoice.Contains(lbQChoiceType.Text))
                    {
                        btnAddAnswer.Visible = true;
                    }
                    else
                    {
                        btnAddAnswer.Visible = false;
                    }

                    BindGridChoiceData(id);
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
                    ViewState["Depend"] = null;
                    pnDepend.Visible = false;
                    pnSuggestion.Visible = false;
                    tbSuggesttionTH.Text= "";
                    tbSuggesttionEN.Text = "";
                    SurveyService srv = new SurveyService();
                    DataRow row = srv.GetQuestionById(id);
                    if (row != null)
                    {
                        tbQuestionTH.Text = row["question_th"].ToString();
                        tbQuestionEN.Text = row["question_en"].ToString();
                        dlQuestionType.SelectedValue = row["question_type"].ToString();
                        dlAllowSuggestion.SelectedValue = row["allow_suggestion"].ToString().ToLower() == "true" ? "1" : "0";
                        dlAllowBack.SelectedValue = row["allow_back"].ToString().ToLower() == "true" ? "1" : "0";
                        dlDependOn.SelectedValue = row["is_depend_on_other_questions"].ToString().ToLower() == "true" ? "1" : "0";
                        dlRequire.SelectedValue = row["is_required"].ToString().ToLower() == "true" ? "1" : "0";
                        dlRequiredAll.SelectedIndex = -1;
                        tbOrderQuestion.Text = row["ordinal"].ToString();
                        if (dlDependOn.SelectedValue == "1")
                        {
                            pnDepend.Visible = true;
                            dlRequiredAll.SelectedValue = row["is_require_all"].ToString().ToLower() == "true" ? "1" : "0";
                        }
                        if (dlAllowSuggestion.SelectedValue == "1")
                        {
                            pnSuggestion.Visible = true;
                              tbSuggesttionTH.Text= row["suggestion_th"].ToString();
                              tbSuggesttionEN.Text= row["suggestion_en"].ToString();
                        }
                        BindPreviousQuestion();

                        BindRPDependData();

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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                string survey_id = lbQSurveyId.Text;
                string id = lbQuestionId.Text;
                string user = Session["User"].ToString();
                string question_th = tbQuestionTH.Text;
                string question_en = tbQuestionEN.Text;
                string questionType = dlQuestionType.SelectedValue;
                string allow_suggestion = dlAllowSuggestion.SelectedValue;
                string allow_back = dlAllowBack.SelectedValue;
                string is_depend = dlDependOn.SelectedValue;
                string is_required = dlRequire.SelectedValue;
                string is_require_all = dlRequiredAll.SelectedValue;
                string ordinal = tbOrderQuestion.Text;
                string suggestion_th = "";
                string suggestion_en = "";
                if(allow_suggestion=="1")
                {
                    suggestion_th = tbSuggesttionTH.Text;
                    suggestion_en = tbSuggesttionEN.Text;
                }

                #region getValue from Depend Repreater   
                List<QuestionRuleModel> QuestionRules = (List<QuestionRuleModel>)ViewState["QuestionRule"];
                foreach (RepeaterItem i in RPDepend.Items)
                {
                    //Label lbQuestionRuleId = e.Item.FindControl("lbQuestionRuleID") as Label;
                    Label lbRPQuestionId = i.FindControl("lbRPQuestionID") as Label;
                    Label lbPrevQuestionId = i.FindControl("lbPreviousQuestionID") as Label;
                    Label lbQuestionTH = i.FindControl("lbQuestionTH") as Label;
                    Label lbQuestionEN = i.FindControl("lbQuestionEN") as Label;
                    Label lbQuestionType = i.FindControl("lbQuestionType") as Label;
                    Label lbQuestionTypeText = i.FindControl("lbQuestionTypeText") as Label;

                    CheckBoxList chkList = i.FindControl("chkQuestionRuleChoiceList") as CheckBoxList;
                    RadioButtonList rdoList = i.FindControl("rdQuestionRuleCoiceList") as RadioButtonList;
                    string prevId = lbPrevQuestionId.Text;
                    string questionId = lbRPQuestionId.Text;
                    if (lbQuestionType.Text == "3")
                    {
                        foreach (ListItem item in chkList.Items)
                        {
                            if (item.Selected)
                            {
                                QuestionRuleModel question = QuestionRules.Where(c => c.PreviousQuestionId == prevId && c.ChoiceId == item.Value).FirstOrDefault();
                                if (question == null)
                                {
                                    //Add
                                    QuestionRuleModel newRule = new QuestionRuleModel()
                                    {
                                        QuestionId = questionId,
                                        PreviousQuestionId = prevId,
                                        ChoiceId = item.Value,
                                        Status = "ADD"
                                    };
                                    QuestionRules.Add(newRule);
                                }

                            }
                            else
                            {
                                QuestionRuleModel question = QuestionRules.Where(c => c.PreviousQuestionId == prevId && c.ChoiceId == item.Value).FirstOrDefault();
                                if (question != null)
                                {
                                    question.Status = "DEL";
                                }
                            }
                        }
                    }
                    else if (lbQuestionType.Text == "4")
                    {
                        foreach (ListItem item in rdoList.Items)
                        {
                            if (item.Selected)
                            {
                                QuestionRuleModel question = QuestionRules.Where(c => c.PreviousQuestionId == prevId && c.ChoiceId == item.Value).FirstOrDefault();
                                if (question == null)
                                {
                                    //Add
                                    QuestionRuleModel newRule = new QuestionRuleModel()
                                    {
                                        QuestionId = questionId,
                                        PreviousQuestionId = prevId,
                                        ChoiceId = item.Value,
                                        Status = "ADD"
                                    };
                                    QuestionRules.Add(newRule);
                                }

                            }
                            else
                            {
                                QuestionRuleModel question = QuestionRules.Where(c => c.PreviousQuestionId == prevId && c.ChoiceId == item.Value).FirstOrDefault();
                                if (question != null)
                                {
                                    question.Status = "DEL";
                                }
                            }
                        }
                    }




                }


                #endregion


                var testQuestionRules = QuestionRules;
                lbAQuestionId.Text = ""; // Clear Answer

                SurveyService srv = new SurveyService();
                //if ((question_th.Contains("'")) || (question_en.Contains("'")))
                //{
                //    ClientPopup(ModalType.Warning, "Don't used a single quote");
                //}
                //else
                //{
                    if (lbQuestionType.Text == "Add")
                    {
                        srv.AddQuestion(survey_id, question_th, question_en, questionType, allow_suggestion, allow_back, is_depend, is_required, is_require_all, ordinal, user,suggestion_th,suggestion_en, QuestionRules);
                        BindGridQuestionData(survey_id);
                        ClientPopup(ModalType.Success, "Completed");
                    }
                    else if (lbQuestionType.Text == "Edit")
                    {
                        srv.UpdateQuestion(survey_id, question_th, question_en, questionType, allow_suggestion, allow_back, is_depend, is_required, is_require_all, ordinal, user,  id, suggestion_th, suggestion_en, QuestionRules);
                        BindGridQuestionData(survey_id);
                        //BindGridAnswerData(lbId.Text);
                        ClientPopup(ModalType.Success, "Completed");
                    }
                //}
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot save. Please try again later.");
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

                lbDelChoiceId.Text = ""; // Clear color

                SurveyService srv = new SurveyService();
                srv.DeleteQuestion(id, user);

                BindGridQuestionData(lbQSurveyId.Text);
                btnAddAnswer.Visible = false;
                BindGridChoiceData("");
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnAddPrevQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                var questionRuleRP = (List<QuestionRuleRPModel>)ViewState["Depend"];
                var questionRule = (List<QuestionRuleModel>)ViewState["QuestionRule"];

                #region Load Previous Repreater

                foreach (RepeaterItem i in RPDepend.Items)
                {

                    Label lbRPQuestionID = i.FindControl("lbRPQuestionID") as Label;
                    Label lbPrevQuestionId = i.FindControl("lbPreviousQuestionID") as Label;
                    Label lbQuestionTH = i.FindControl("lbQuestionTH") as Label;
                    Label lbQuestionEN = i.FindControl("lbQuestionEN") as Label;
                    Label lbQuestionType = i.FindControl("lbQuestionType") as Label;
                    Label lbQuestionTypeText = i.FindControl("lbQuestionTypeText") as Label;

                    CheckBoxList chkList = i.FindControl("chkQuestionRuleChoiceList") as CheckBoxList;
                    RadioButtonList rdoList = i.FindControl("rdQuestionRuleCoiceList") as RadioButtonList;
                    string prevId = lbPrevQuestionId.Text;
                    string questionId = lbRPQuestionID.Text;

                    if (lbQuestionType.Text == "3")
                    {
                        foreach (ListItem item in chkList.Items)
                        {
                            if (item.Selected)
                            {
                                var ruleChoices = questionRuleRP.Where(c => c.PreviousQuestionId == prevId).FirstOrDefault().QuestionRuleChoices.Where(c => c.ChoiceId == item.Value).FirstOrDefault();
                                if (ruleChoices == null)
                                {
                                    //Add
                                    ChoiceModel newChoice = new ChoiceModel()
                                    {
                                        ChoiceId = item.Value,
                                        Status = "ADD"
                                    };
                                    questionRuleRP.Where(c => c.PreviousQuestionId == prevId).FirstOrDefault().QuestionRuleChoices.Add(newChoice);
                                }

                            }
                            else
                            {
                                var ruleChoices = questionRuleRP.Where(c => c.PreviousQuestionId == prevId).FirstOrDefault().QuestionRuleChoices.Where(c => c.ChoiceId == item.Value).FirstOrDefault();
                                if (ruleChoices != null)
                                {
                                    questionRuleRP.Where(c => c.PreviousQuestionId == prevId).FirstOrDefault().QuestionRuleChoices.RemoveAll(c => c.ChoiceId == item.Value);
                                }
                            }
                        }
                    }
                    else if (lbQuestionType.Text == "4")
                    {
                        foreach (ListItem item in rdoList.Items)
                        {
                            if (item.Selected)
                            {
                                var ruleChoices = questionRuleRP.Where(c => c.PreviousQuestionId == prevId).FirstOrDefault().QuestionRuleChoices.Where(c => c.ChoiceId == item.Value).FirstOrDefault();
                                if (ruleChoices == null)
                                {
                                    //Add
                                    ChoiceModel newChoice = new ChoiceModel()
                                    {
                                        ChoiceId = item.Value,
                                        Status = "ADD"
                                    };
                                    questionRuleRP.Where(c => c.PreviousQuestionId == prevId).FirstOrDefault().QuestionRuleChoices.Add(newChoice);
                                }

                            }
                            else
                            {
                                var ruleChoices = questionRuleRP.Where(c => c.PreviousQuestionId == prevId).FirstOrDefault().QuestionRuleChoices.Where(c => c.ChoiceId == item.Value).FirstOrDefault();
                                if (ruleChoices != null)
                                {
                                    questionRuleRP.Where(c => c.PreviousQuestionId == prevId).FirstOrDefault().QuestionRuleChoices.RemoveAll(c => c.ChoiceId == item.Value);
                                }
                            }
                        }
                    }


                    ViewState["Depend"] = questionRuleRP;
                }

                #endregion

                #region Add New Previous Question

                SurveyService srv = new SurveyService();
                var question = srv.GetQuestionById(dlPrevQuestion.SelectedValue);
                QuestionRuleRPModel qr = new QuestionRuleRPModel()
                {
                    QuestionId = lbQuestionId.Text,
                    PreviousQuestionId = question.Field<int>("id").ToString(),
                    QuestionTH = question.Field<string>("question_th").ToString(),
                    QuestionEN = question.Field<string>("question_en").ToString(),
                    QuestionType = question.Field<int>("question_type"),
                    Choices = srv.GetChoiceByQuestionId(question.Field<int>("id").ToString()).AsEnumerable().Select(c => new ChoiceModel()
                    {
                        ChoiceId = c.Field<int>("id").ToString(),
                        ChoiceTH = c.Field<string>("choice_th").ToString(),
                        ChoiceEN = c.Field<string>("choice_en").ToString(),

                    }).ToList(),
                };
                if (questionRuleRP.Where(c => c.PreviousQuestionId == qr.PreviousQuestionId).Any())
                {

                }
                else
                {
                    questionRuleRP.Add(qr);
                    ViewState["Depend"] = questionRuleRP;
                    RPDepend.DataSource = questionRuleRP;
                    RPDepend.DataBind();
                    upModalQuestion.Update();
                }
                #endregion

            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindRPDependData()
        {
            try
            {
                if (ViewState["Depend"] == null)
                {
                    SurveyService srv = new SurveyService();
                    DataTable dt = srv.GetQuestionRuleByQuestionId(lbQuestionId.Text);
                    if (dt != null)
                    {
                        //Use for send service Update
                        var questionRules = dt.AsEnumerable().Select(c => new QuestionRuleModel()
                        {
                            Id = c.Field<int>("id").ToString(),
                            QuestionId = c.Field<int>("question_id").ToString(),
                            PreviousQuestionId = c.Field<int>("previous_question_id").ToString(),
                            QuestionType = c.Field<int>("question_type"),
                            ChoiceId = c.Field<int>("choice_id").ToString(),
                            QuestionTH = c.Field<string>("question_th").ToString(),
                            QuestionEN = c.Field<string>("question_en").ToString()
                        }).ToList();

                        //To do create class for binding to Repreater 
                        var questionRuleRP = questionRules.GroupBy(c => c.PreviousQuestionId).Select(t => new QuestionRuleRPModel()
                        {
                            PreviousQuestionId = t.Key
                        }).ToList();

                        foreach (var ruleRP in questionRuleRP)
                        {

                            var ruleChoices = questionRules.Where(c => c.PreviousQuestionId == ruleRP.PreviousQuestionId).ToList();
                            ruleRP.QuestionTH = ruleChoices.FirstOrDefault().QuestionTH;
                            ruleRP.QuestionEN = ruleChoices.FirstOrDefault().QuestionEN;
                            ruleRP.QuestionId = ruleChoices.FirstOrDefault().QuestionId;
                            ruleRP.PreviousQuestionId = ruleChoices.FirstOrDefault().PreviousQuestionId;
                            ruleRP.QuestionType = ruleChoices.FirstOrDefault().QuestionType;

                            ruleRP.QuestionRuleChoices = ruleChoices.GroupBy(c => c.ChoiceId).Select(t => new ChoiceModel()
                            {
                                ChoiceId = t.Key
                            }).ToList();
                            ruleRP.Choices = srv.GetChoiceRPByQuestionId(ruleRP.PreviousQuestionId).AsEnumerable().Select(c => new ChoiceModel()
                            {
                                ChoiceId = c.Field<int>("id").ToString(),
                                ChoiceTH = c.Field<string>("choice_th").ToString(),
                                ChoiceEN = c.Field<string>("choice_en").ToString(),

                            }).ToList();

                        }

                        questionRuleRP.Where(c => c.Status == "DEL").ToList();
                        ViewState["Depend"] = questionRuleRP.Where(c => c.Status != "DEL").ToList();
                        ViewState["QuestionRule"] = questionRules;
                    }
                    else
                    {
                        ViewState["Depend"] = new List<QuestionRuleRPModel>();
                        ViewState["QuestionRule"] = new List<QuestionRuleModel>();
                    }
                }
                else
                {
                    //RPDepend.DataSource = ViewState["Depend"];
                    //RPDepend.DataBind();
                }

                RPDepend.DataSource = ViewState["Depend"];
                RPDepend.DataBind();
                //upModalQuestion.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void RPDepend_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                QuestionRuleRPModel d = (QuestionRuleRPModel)e.Item.DataItem;
                Label lbQuestionRuleId = e.Item.FindControl("lbQuestionRuleID") as Label;
                Label lbRPQuestionID = e.Item.FindControl("lbRPQuestionID") as Label;
                Label lbPrevQuestionId = e.Item.FindControl("lbPreviousQuestionID") as Label;
                Label lbQuestionTH = e.Item.FindControl("lbQuestionTH") as Label;
                Label lbQuestionEN = e.Item.FindControl("lbQuestionEN") as Label;
                Label lbQuestionType = e.Item.FindControl("lbQuestionType") as Label;
                Label lbQuestionTypeLabel = e.Item.FindControl("lbQuestionTypeLabel") as Label;

                CheckBoxList chkList = e.Item.FindControl("chkQuestionRuleChoiceList") as CheckBoxList;
                RadioButtonList rdoList = e.Item.FindControl("rdQuestionRuleCoiceList") as RadioButtonList;
                chkList.Visible = false;
                rdoList.Visible = false;
                lbQuestionType.Text = d.QuestionType.ToString();
                if (d.QuestionType == 3)
                {
                    lbQuestionTypeLabel.Text = "Checkbox";
                    chkList.DataSource = d.Choices;
                    chkList.DataValueField = "ChoiceId";
                    chkList.DataTextField = "ChoiceTH";
                    chkList.DataBind();

                    if (d.QuestionRuleChoices != null && d.QuestionRuleChoices.Any())
                    {
                        foreach (ListItem item in chkList.Items)
                        {
                            if (d.QuestionRuleChoices.Where(c => c.ChoiceId == item.Value).FirstOrDefault() != null)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    chkList.Visible = true;

                }
                else if (d.QuestionType == 4)
                {
                    lbQuestionTypeLabel.Text = "Radio Button";
                    rdoList.DataSource = d.Choices;
                    rdoList.DataValueField = "ChoiceId";
                    rdoList.DataTextField = "ChoiceTH";
                    rdoList.DataBind();

                    if (d.QuestionRuleChoices != null && d.QuestionRuleChoices.Any())
                    {
                        foreach (ListItem item in rdoList.Items)
                        {
                            if (d.QuestionRuleChoices.Where(c => c.ChoiceId == item.Value).FirstOrDefault() != null)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    rdoList.Visible = true;
                }
                lbQuestionRuleId.Text = d.Id;
                lbRPQuestionID.Text = d.QuestionId;
                lbPrevQuestionId.Text = d.PreviousQuestionId;
                lbQuestionTH.Text = d.QuestionTH;
                lbQuestionEN.Text = d.QuestionEN;




                upModalQuestion.Update();
            }



        }
        protected void RPDepend_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName == "delete")
                {
                    QuestionRuleRPModel d = (QuestionRuleRPModel)e.Item.DataItem;
                    //Label lbQuestionRuleId = e.Item.FindControl("lbQuestionRuleID") as Label;
                    //Label lbQuestionId = e.Item.FindControl("lbQuestionID") as Label;
                    Label lbPrevQuestionId = e.Item.FindControl("lbPreviousQuestionID") as Label;
                    //Label lbQuestionTH = e.Item.FindControl("lbQuestionTH") as Label;
                    //Label lbQuestionEN = e.Item.FindControl("lbQuestionEN") as Label;
                    //Label lbQuestionType = e.Item.FindControl("lbQuestionType") as Label;
                    //Label lbQuestionTypeText = e.Item.FindControl("lbQuestionTypeText") as Label;
                    var questionRuleRP = (List<QuestionRuleRPModel>)ViewState["Depend"];
                    questionRuleRP.Where(c => c.PreviousQuestionId == lbPrevQuestionId.Text).ToList().ForEach(t => t.Status = "DEL");
                    ViewState["Depend"] = questionRuleRP;
                    var questionRule = (List<QuestionRuleModel>)ViewState["QuestionRule"];
                    questionRule.Where(c => c.PreviousQuestionId == lbPrevQuestionId.Text).ToList().ForEach(t => t.Status = "DEL");
                    ViewState["QuestionRule"] = questionRule;

                    ViewState["Depend"] = questionRuleRP.Where(c => c.Status != "DEL").ToList();
                    RPDepend.DataSource = ViewState["Depend"];
                    RPDepend.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddQuestion", "$('#mdAddQuestion').modal();", true);
                upModalQuestion.Update();

            }
        }
        protected void gvChoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvChoice.PageIndex = e.NewPageIndex;
                string id = lbAQuestionId.Text;
                if (id != "")
                {
                    BindGridChoiceData(id);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvChoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelChoiceId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvChoice.DataKeys != null)
                    {
                        //lbDelAnswerModelId.Text = gvChoice.DataKeys[index].Values["question_id"].ToString();
                        lbDelChoiceId.Text = gvChoice.DataKeys[index].Values["ID"].ToString();

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmChoice", "$('#mdDelConfirmChoice').modal();", true);
                        upModalConfirmDeleteChoice.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gvChoice.SelectedIndex;
                if (gvChoice.DataKeys != null)
                {
                    string id = gvChoice.DataKeys[rowIndex].Values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvChoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void btnAddAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbQChoiceType.Text == "2")
                {
                    ClientPopup(ModalType.Warning, "Can't create answer!!");
                }

                if (lbAQuestionId.Text != "")
                {
                    lbChoiceType.Text = "Add";
                    pnUpload.Visible = false;
                    lbChoiceId.Text = "";
                    tbChoiceTH.Text = "";
                    tbChoiceEN.Text = "";
                    dlIsImage.SelectedIndex = -1;
                    tbOrderChoice.Text = "";
                    tbImg1.Text = "";
                    lbImg1.Text = "";
                    lbImg1.Text = "";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddChoice", "$('#mdAddChoice').modal();", true);
                    upModalAddChoice.Update();
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
                lbChoiceType.Text = "";
                lbChoiceId.Text = "";
                lbImg1.Text = "";
                ViewState["survey_member_list"] = null;
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvChoice.DataKeys != null)
                {
                    string questionId = gvChoice.DataKeys[index].Values["question_id"].ToString();
                    string answerId = gvChoice.DataKeys[index].Values["ID"].ToString();

                    lbChoiceType.Text = "Edit";
                    lbChoiceId.Text = answerId;

                    SurveyService srv = new SurveyService();

                    DataRow row = srv.GetChoiceById(answerId);
                    if (row != null)
                    {
                        // ToDo Luzif3r
                        tbChoiceTH.Text = row["choice_th"].ToString();
                        tbChoiceEN.Text = row["choice_en"].ToString();
                        dlIsImage.SelectedValue = row["is_image"].ToString().ToLower() == "true" ? "1" : "0"; //row["is_image"].ToString();
                        if (dlIsImage.SelectedValue == "1")
                        {
                            pnUpload.Visible = true;
                        }
                        tbImg1.Text = row["image_path"].ToString();
                        tbOrderChoice.Text = row["ordinal"].ToString();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddChoice", "$('#mdAddChoice').modal();", true);
                        upModalAddChoice.Update();
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
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                string user = Session["User"].ToString();
                string questionId = lbAQuestionId.Text;
                string choiceId = lbChoiceId.Text;

                if (lbImg1.Text != "")
                {
                    DeleteImage(ImageType.Img1);
                }

                string choice_th = tbChoiceTH.Text;
                string choice_en = tbChoiceEN.Text;
                string is_image = dlIsImage.SelectedValue;

                string image_path = (tbImg1.Text.Length > 0) ? ImageService.Split4(ViewState["UrlPath"] + "\\" + tbImg1.Text) : "";
                if (is_image == "0")
                {
                    image_path = "";
                    DeleteImage(ImageType.Img1);
                }
                string ordinal = tbOrderChoice.Text;

                SurveyService srv = new SurveyService();
                //if ((choice_th.Contains("'")) || (choice_en.Contains("'")))
                //{
                //    ClientPopup(ModalType.Warning, "Don't used a single quote");
                //}
                //else
                //{
                    if (lbChoiceType.Text == "Add")
                    {
                        srv.AddChoice(questionId, choice_th, choice_en, is_image, image_path, "", ordinal, user);
                        BindGridChoiceData(questionId);
                        ClientPopup(ModalType.Success, "Completed");
                    }
                    else if (lbChoiceType.Text == "Edit")
                    {
                        srv.UpdateChoice(choiceId, choice_th, choice_en, is_image, image_path, "", ordinal, user);
                        BindGridChoiceData(questionId);
                        ClientPopup(ModalType.Success, "Completed");
                    }
                //}

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddChoice", "$('#mdAddChoice').modal('hide');", true);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
                ClientPopup(ModalType.Error, "Cannot save. Please try again later.");
            }
        }
        protected void btnDeleteAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmChoice", "$('#mdDelConfirmChoice').modal('hide');", true);
                upModalConfirmDeleteChoice.Update();

                string user = Session["User"].ToString();

                SurveyService srv = new SurveyService();
                srv.DeleteChoice(lbDelChoiceId.Text, user);

                BindGridChoiceData(lbAQuestionId.Text);
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void DeleteImage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnDelImg1":
                    lbImg1.Text = ImageType.Img1.ToString();
                    break;

            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddChoice", "$('#mdAddChoice').modal();", true);
            upModalAddChoice.Update();
        }
        protected void UploadImage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnUpImg1":
                    UploadImage(ImageType.Img1);
                    break;
            }
        }

        private void UploadImage(ImageType type)
        {
            try
            {

                string imgDir = ViewState["ImagePath"] + "\\";
                FileUpload fileUpload = new FileUpload();
                fileUpload = upModalAdd.FindControl("up" + type.ToString()) as FileUpload;
                TextBox tbImage = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;

                lbStatus.Text = "";

                if (fileUpload.HasFile)
                {
                    if ((fileUpload.PostedFile.ContentType == "image/jpeg") || (fileUpload.PostedFile.ContentType == "image/png"))
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(fileUpload.PostedFile.InputStream))
                        {
                            if (!Directory.Exists(imgDir))
                            {
                                Directory.CreateDirectory(imgDir);
                            }

                            if (tbImage.Text != "")
                            {
                                if (File.Exists(imgDir + tbImage.Text))
                                    File.Delete(imgDir + tbImage.Text);
                            }
                            string autoGen = Guid.NewGuid().ToString().Replace("-", string.Empty);
                            fileUpload.SaveAs(imgDir + autoGen + fileUpload.FileName);
                            tbImage.Text = autoGen + fileUpload.FileName;
                            //if (img.Height == 764 && img.Width == 1500)
                            //{

                            //}
                            //else
                            //{
                            //    throw new Exception("Allow file size 1500 x 764 only!");
                            //}
                        }
                    }
                    else
                    {
                        throw new Exception("Allow file type JPG and PNG only!");
                    }
                }
                else
                {
                    throw new Exception("No file selected!");
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddChoice", "$('#mdAddChoice').modal();", true);
                upModalAddChoice.Update();
            }
            catch (Exception ex)
            {
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;
                lbStatus.Text = ex.Message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddChoice", "$('#mdAddChoice').modal();", true);
                upModalAddChoice.Update();
            }
        }
        private void DeleteImage(ImageType type)
        {
            try
            {
                string imgDir = ViewState["ImagePath"] + "\\";

                TextBox tbImage = upModalAddChoice.FindControl("tb" + type.ToString()) as TextBox;

                if (tbImage.Text != "")
                {
                    if (File.Exists(imgDir + ImageService.Split1(tbImage.Text)))
                        File.Delete(imgDir + ImageService.Split1(tbImage.Text));
                    tbImage.Text = "";
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dlIsImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dlIsImage.SelectedValue == "0")
            {
                pnUpload.Visible = false;
            }
            else
            {
                pnUpload.Visible = true;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddChoice", "$('#mdAddChoice').modal()", true);
            upModalAddChoice.Update();
        }

        protected void dlDependOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dlDependOn.SelectedValue == "0")
            {
                pnDepend.Visible = false;
            }
            else
            {
                pnDepend.Visible = true;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddQuestion", "$('#mdAddQuestion').modal()", true);
            upModalQuestion.Update();
        }
        protected void dlAllowSuggestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dlAllowSuggestion.SelectedValue == "0")
            {
                pnSuggestion.Visible = false;
            }
            else
            {
                pnSuggestion.Visible = true;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddQuestion", "$('#mdAddQuestion').modal()", true);
            upModalQuestion.Update();
        }
        protected void dlDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }

                ViewState["survey_member_list"] = null;

                if (dlDestination.SelectedValue == "1")
                {
                    dlSubDestination.Visible = false;
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                }
                if (dlDestination.SelectedValue == "2")
                {
                    dlSubDestination.Visible = true;
                    pnMemberID.Visible = true;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                    dlSubDestination.Items.Clear();

                    dlSubDestination.Items.Add(new ListItem { Text = "Specific Member ID", Value = "1" });
                    dlSubDestination.Items.Add(new ListItem { Text = "Group Member ID", Value = "2" });
                }
                if (dlDestination.SelectedValue == "3")
                {
                    dlSubDestination.Visible = true;
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = true;
                    pnDeviceGroup.Visible = false;
                    dlSubDestination.Items.Clear();

                    dlSubDestination.Items.Add(new ListItem { Text = "Specific Device", Value = "3" });
                    dlSubDestination.Items.Add(new ListItem { Text = "Group Device", Value = "4" });
                }
                if (dlDestination.SelectedValue == "4")
                {
                    dlSubDestination.Visible = true;
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                    dlSubDestination.Items.Clear();
                    dlSubDestination.Items.Add(new ListItem { Text = "Android", Value = "5" });
                    dlSubDestination.Items.Add(new ListItem { Text = "iOS", Value = "6" });
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbStartDate').datepicker({ changeMonth: true, changeYear: true, format: 'dd/mm/yyyy', language: 'en' }).on('changeDate', function (ev) { $(this).blur(); $(this).datepicker('hide'); });$('#ContentPlaceHolder1_tbEndDate').datepicker({ changeMonth: true, changeYear: true, format: 'dd/mm/yyyy', language: 'en' }).on('changeDate', function(ev) { $(this).blur(); $(this).datepicker('hide'); }); ", true);
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbDate').datepicker({changeMonth: true,changeYear: true,format: 'dd/mm/yyyy',language: 'en'}).on('changeDate', function (ev) {$(this).blur();$(this).datepicker('hide');});", true);
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
                if (dlSubDestination.SelectedValue == "1")
                {
                    pnMemberID.Visible = true;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                }
                if (dlSubDestination.SelectedValue == "2")
                {
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = true;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = false;
                }
                if (dlSubDestination.SelectedValue == "3")
                {
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = true;
                    pnDeviceGroup.Visible = false;
                }
                if (dlSubDestination.SelectedValue == "4")
                {
                    pnMemberID.Visible = false;
                    pnMemberGroup.Visible = false;
                    pnDeviceID.Visible = false;
                    pnDeviceGroup.Visible = true;
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();$('#ContentPlaceHolder1_tbStartDate').datepicker({ changeMonth: true, changeYear: true, format: 'dd/mm/yyyy', language: 'en' }).on('changeDate', function (ev) { $(this).blur(); $(this).datepicker('hide'); });$('#ContentPlaceHolder1_tbEndDate').datepicker({ changeMonth: true, changeYear: true, format: 'dd/mm/yyyy', language: 'en' }).on('changeDate', function(ev) { $(this).blur(); $(this).datepicker('hide'); }); ", true);
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
                    string newFilename = Guid.NewGuid().ToString() + "_" + fileUpload.FileName;
                    string filename = ViewState["SurveyMemberPath"] + "\\" + newFilename;
                    if (File.Exists(filename))
                        File.Delete(filename);
                    fileUpload.SaveAs(filename);
                    tbFile.Text = newFilename;

                    DataTable dt = ProcessExcel(filename);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        throw new Exception("This file cannot open or havenot data!");
                    }
                    else
                    {
                        ViewState["survey_member_list"] = dt.AsEnumerable().Select(c => new SurveyMemberGroupModel()
                        {
                            Survey_Id = lbId.Text,
                            MemberId = c.Field<string>("member_id")
                        }).ToList();
                    }
                }
                else
                {
                    throw new Exception("No file selected!");
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;
                lbStatus.Text = ex.Message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
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
                string imgDir = ViewState["SurveyMemberPath"] + "\\";

                TextBox tbFile = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;

                if (tbFile.Text != "")
                {
                    if (File.Exists(imgDir + tbFile.Text))
                        File.Delete(imgDir + tbFile.Text);
                    tbFile.Text = "";
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnDownloadMember_ServerClick(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(WebConfigurationManager.AppSettings["TemplatePath"] + "\\SurveyGroupMember_Template.xls");
            byte[] fileConent = File.ReadAllBytes(WebConfigurationManager.AppSettings["TemplatePath"] + "\\SurveyGroupMember_Template.xls");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(fileConent);
            HttpContext.Current.Response.End();
        }

        protected void btnDownloadDevice_ServerClick(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(WebConfigurationManager.AppSettings["TemplatePath"] + "\\SurveyGroupDevice_Template.xls");
            byte[] fileConent = File.ReadAllBytes(WebConfigurationManager.AppSettings["TemplatePath"] + "\\SurveyGroupDevice_Template.xls");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(fileConent);
            HttpContext.Current.Response.End();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(lbId.Text))
                {
                    string filePath = WebConfigurationManager.AppSettings["ExportPathExcel"] + "\\SurveyGroup" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    SurveyService srv = new SurveyService();
                    using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                        DataTable dt = srv.GetSurveyMemberGroupListBySurveyId(lbId.Text);
                        if (dt.Rows.Count > 0)
                        {
                            ws.Cells["A1"].LoadFromDataTable(srv.GetSurveyMemberGroupListBySurveyId(lbId.Text), true);
                            pck.Save();

                            FileInfo file = new FileInfo(filePath);
                            byte[] fileConent = File.ReadAllBytes(filePath);
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", file.Name));
                            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.BinaryWrite(fileConent);
                            HttpContext.Current.Response.End();
                        }

                    }

                   

                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                upModalAdd.Update();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
           
        }

    }
}