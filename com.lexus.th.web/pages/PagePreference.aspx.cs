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
    public partial class PagePreference : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        private enum ImageType { Img1 }

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
                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathPreference"];
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();

                        gvAnswer.DataSource = new System.Data.DataTable();
                        gvAnswer.DataBind();

                        lbModelId.Text = "";
                        lbAnswerId.Text = "";
                        BindGridData();
                    }
                }
                this.RegisterPostBackControl();
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BindGridData();

        //        lbModelId.Text = "";
        //        lbColorId.Text = "";

        //        gvColor.DataSource = new System.Data.DataTable();
        //        gvColor.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        //ClientPopup(ModalType.Error, ex.Message);
        //    }
        //}
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
                PreferenceService srv = new PreferenceService();
                gvCust.DataSource = srv.GetPreference();
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
                string user = Session["User"].ToString();
                string name_th = tbDescQTH.Text;
                string name_en = tbDescQ.Text;
                string active = dlActiveQ.SelectedValue;
                //string start_date = tbStartQdate.Text;
                //string end_date = tbEndQdate.Text;
                string max_selected = tbMaxSelected.Text;
                string order = tbOrderQ.Text;
                string require_answer = dlRequireAnswer.SelectedValue;
                //string image = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";

                lbModelId.Text = ""; // Clear Answer

                PreferenceService srv = new PreferenceService();
                if (lbType.Text == "Add")
                {
                    srv.AddPreference(name_th, name_en, active, require_answer, user, order, max_selected);
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdatePreference(name_th, name_en, active, require_answer, user, lbId.Text, order, max_selected);
                    BindGridData();
                    BindGridAnswerData(lbId.Text);
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

                string user = Session["User"].ToString();
                string id = lbDelId.Text;

                lbModelId.Text = ""; // Clear color

                PreferenceService srv = new PreferenceService();
                srv.DeletePreference(id, user);

                BindGridData();
                BindGridAnswerData(id);
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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdWarning", "$('#mdWarning').modal('hide');", true);
            upModalWarning.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirm", "$('#mdDelConfirm').modal('hide');", true);
            upModelDel.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal('hide');", true);
            upModalAddAnswer.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmAnswer", "$('#mdDelConfirmAnswer').modal('hide');", true);
            upModalConfirmDeleteAnswer.Update();

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
                lbId.Text = "";
                tbDescQTH.Text = "";
                tbDescQ.Text = "";
                dlActiveQ.SelectedIndex = -1;
                dlActive.SelectedIndex = -1;
                tbOrderQ.Text = "";
                tbMaxSelected.Text = "";

                lbType.Text = "Add";

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
                        lbDelId.Text = gvCust.DataKeys[index].Values["question_id"].ToString();
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

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["question_id"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    PreferenceService srv = new PreferenceService();
                    DataRow row = srv.GetPreferenceById(id);
                    if (row != null)
                    {
                        tbDescQTH.Text = row["name_th"].ToString();
                        tbDescQ.Text = row["name_en"].ToString();
                        dlActiveQ.SelectedValue = row["is_active"].ToString();
                        //tbStartQdate.Text = (row["start_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["start_date"]).ToString("dd/MM/yyyy");
                        //tbEndQdate.Text = (row["end_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["end_date"]).ToString("dd/MM/yyyy");
                        dlRequireAnswer.SelectedValue = row["is_require_answer"].ToString();
                        tbOrderQ.Text = row["order"].ToString();
                        tbMaxSelected.Text = row["max_selected"].ToString();

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

        protected void gvBtnView_Click(object sender, EventArgs e)
        {
            try
            {
                lbModelId.Text = "";
                lbAnswerId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["question_id"].ToString();

                    lbModelId.Text = id;
                    BindGridAnswerData(id);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindGridAnswerData(string questionId)
        {
            try
            {
                if (questionId != "")
                {
                    PreferenceService srv = new PreferenceService();
                    gvAnswer.DataSource = srv.GetAnswerByModelId(questionId);
                    gvAnswer.DataBind();
                }
                else
                {
                    gvAnswer.DataSource = new DataTable();
                    gvAnswer.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvAnswer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAnswer.PageIndex = e.NewPageIndex;
                string id = lbModelId.Text;
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
                    lbDelAnswerModelId.Text = "";
                    lbDelAnswerId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvAnswer.DataKeys != null)
                    {
                        lbDelAnswerModelId.Text = gvAnswer.DataKeys[index].Values["question_id"].ToString();
                        lbDelAnswerId.Text = gvAnswer.DataKeys[index].Values["answer_id"].ToString();

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
        protected void btnDeleteAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmAnswer", "$('#mdDelConfirmAnswer').modal('hide');", true);
                upModalConfirmDeleteAnswer.Update();

                string user = Session["User"].ToString();

                PreferenceService srv = new PreferenceService();
                srv.DeleteAnswer(lbDelAnswerId.Text, user);

                BindGridAnswerData(lbDelAnswerModelId.Text);
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvAnswer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void gvBtnEditAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                lbTypeAnswer.Text = "";
                lbModelId.Text = "";
                lbAnswerId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvAnswer.DataKeys != null)
                {
                    string modelId = gvAnswer.DataKeys[index].Values["question_id"].ToString();
                    string answerId = gvAnswer.DataKeys[index].Values["answer_id"].ToString();

                    lbTypeAnswer.Text = "Edit";
                    lbModelId.Text = modelId;
                    lbAnswerId.Text = answerId;

                    PreferenceService srv = new PreferenceService();
                    dlQuestion.Items.Clear();
                    dlQuestion.DataSource = srv.GetDlPreference();
                    dlQuestion.DataBind();
                    DataRow row = srv.GetAnswerByModelIdAndChoice(answerId);
                    if (row != null)
                    {
                        tbDescTH.Text = row["name_th"].ToString();
                        tbDesc.Text = row["name_en"].ToString();
                        dlQuestion.SelectedValue = row["question_id"].ToString();
                        dlActive.SelectedValue = row["is_active"].ToString();
                        tbImg1.Text = ImageService.Split1(row["icon_image_url"].ToString());
                        //tbStartdate.Text = (row["start_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["start_date"]).ToString("dd/MM/yyyy");
                        //tbEnddate.Text = (row["end_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["end_date"]).ToString("dd/MM/yyyy");
                        dlOptional.SelectedValue = row["is_optional"].ToString();
                        tbOptionalHeader.Text = row["optional_header"].ToString();
                        tbOptionalText.Text = row["optional_text"].ToString();
                        tbOrder.Text = row["order"].ToString();

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
                if (lbImg1.Text != "")
                {
                    DeleteImage(ImageType.Img1);
                }

                string user = Session["User"].ToString();
                string preferenceId = dlQuestion.SelectedValue;
                string answerId = lbAnswerId.Text;

                string name_th = tbDescTH.Text;
                string name_en = tbDesc.Text;
                string icon_image_url = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";
                string active = dlActive.SelectedValue;
                //string start_date = tbStartdate.Text;
                //string end_date = tbEnddate.Text;
                string optional = dlOptional.SelectedValue;
                string optional_header = tbOptionalHeader.Text;
                string optional_text = tbOptionalText.Text;
                string order = tbOrder.Text;

                

                PreferenceService srv = new PreferenceService();
                if (lbTypeAnswer.Text == "Add")
                {
                    srv.AddAnswer(preferenceId, name_en, name_th, icon_image_url, active, optional, optional_header, optional_text, user, order);
                    BindGridAnswerData(preferenceId);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbTypeAnswer.Text == "Edit")
                {
                    srv.UpdateAnswer(preferenceId, name_en, name_th, icon_image_url, active, optional, optional_header, optional_text, user, answerId, order);
                    BindGridAnswerData(preferenceId);
                    ClientPopup(ModalType.Success, "Completed");
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal('hide');", true);
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnAddAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                PreferenceService srv = new PreferenceService();
                dlQuestion.Items.Clear();
                dlQuestion.DataSource = srv.GetDlPreference();
                dlQuestion.DataBind();

                if (lbModelId.Text != "")
                {
                    lbAnswerId.Text = "";
                    tbDescTH.Text = "";
                    tbDesc.Text = "";
                    dlActive.SelectedIndex = -1;
                    tbImg1.Text = "";
                    lbImg1.Text = "";

                    if(lbModelId.Text == "")
                    {
                        dlQuestion.SelectedIndex = -1;
                    }
                    else
                    {
                        dlQuestion.SelectedValue = lbModelId.Text;
                    }
                    
                    dlOptional.SelectedIndex = -1;
                    tbOptionalHeader.Text = "";
                    tbOptionalText.Text = "";
                    lbTypeAnswer.Text = "Add";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal();", true);
                    upModalAddAnswer.Update();
                }
                else
                {
                    ClientPopup(ModalType.Error, "Please select Answer");
                }
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
            }
        }
        protected void DeleteImage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnDelImg1":
                    lbImg1.Text = ImageType.Img1.ToString();
                    //DeleteImage(ImageType.Img1);
                    break;
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal();", true);
            upModalAddAnswer.Update();
        }
        private void UploadImage(ImageType type)
        {
            try
            {
                //string root = Server.MapPath("~");
                //string parent = Path.GetDirectoryName(root);
                //string grandParent = Path.GetDirectoryName(parent);
                //string imgDir = grandParent + "\\" + ViewState["ImagePath"].ToString().Replace("/", "\\") + "\\";
                //string imgDir = root + "\\" + ViewState["ImagePath"].ToString().Replace("/", "\\") + "\\";
                string imgDir = ViewState["ImagePath"] + "\\";

                FileUpload fileUpload = upModalAdd.FindControl("up" + type.ToString()) as FileUpload;
                TextBox tbImage = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;

                //tbImage.Text = "";
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

                            fileUpload.SaveAs(imgDir + fileUpload.FileName);
                            tbImage.Text = fileUpload.FileName;
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

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal();", true);
                upModalAddAnswer.Update();
            }
            catch (Exception ex)
            {
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;
                lbStatus.Text = "Upload image error. Please try again later.";// ex.Message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal();", true);
                upModalAddAnswer.Update();
            }
        }
        private void DeleteImage(ImageType type)
        {
            try
            {
                //string root = Server.MapPath("~");
                ////string parent = Path.GetDirectoryName(root);
                ////string grandParent = Path.GetDirectoryName(parent);
                ////string imgDir = grandParent + "\\" + ViewState["ImagePath"].ToString().Replace("/", "\\") + "\\";
                //string imgDir = root + "\\" + ViewState["ImagePath"].ToString().Replace("/", "\\") + "\\";
                string imgDir = ViewState["ImagePath"] + "\\";

                TextBox tbImage = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;

                if (File.Exists(imgDir + tbImage.Text))
                {
                    File.Delete(imgDir + tbImage.Text);
                    tbImage.Text = "";
                }

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddAnswer", "$('#mdAddAnswer').modal();", true);
                //upModalAddAnswer.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}