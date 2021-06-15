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
    public partial class PageCar : System.Web.UI.Page
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
                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathCar"];
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();

                        gvColor.DataSource = new System.Data.DataTable();
                        gvColor.DataBind();
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

                lbModelId.Text = "";
                lbColorId.Text = "";

                gvColor.DataSource = new System.Data.DataTable();
                gvColor.DataBind();
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
                CarModelService srv = new CarModelService();
                gvCust.DataSource = srv.GetModels(txtSearch.Text);
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
                if (lbImg1.Text != "")
                {
                    DeleteImage(ImageType.Img1);
                }

                string user = Session["User"].ToString();
                string model = tbModel.Text;
                string testDrive = chkTestDrive.Checked ? "1" : "0";
                string image = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";

                lbModelId.Text = ""; // Clear color

                CarModelService srv = new CarModelService();
                if (lbType.Text == "Add")
                {
                    srv.AddModel(model, image,testDrive, user);
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateModel(model, image, testDrive, user, lbId.Text);
                    BindGridData();
                    BindGridColorData(lbId.Text);
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

                lbModelId.Text = ""; // Clear color

                CarModelService srv = new CarModelService();
                srv.DeleteModel(id, user);

                BindGridData();
                BindGridColorData(id);
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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddColor", "$('#mdAddColor').modal('hide');", true);
            upModalAddColor.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmColor", "$('#mdDelConfirmColor').modal('hide');", true);
            upModalConfirmDeleteColor.Update();

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
                tbModel.Text = "";
                tbImg1.Text = "";
                lbType.Text = "Add";
                lbImg1.Text = "";
                chkTestDrive.Checked = false;
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
                        lbDelId.Text = gvCust.DataKeys[index].Values["MODEL_ID"].ToString();
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
                chkTestDrive.Checked = false;
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["MODEL_ID"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    CarModelService srv = new CarModelService();
                    DataRow row = srv.GetModelById(id);
                    if (row != null)
                    {
                        tbModel.Text = row["MODEL"].ToString();
                        chkTestDrive.Checked = (row["IS_TEST_DRIVE"] != DBNull.Value && row["IS_TEST_DRIVE"].ToString().ToLower().Equals("true")) ? true : false;
                        tbImg1.Text = ImageService.Split1(row["IMAGE"].ToString());
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
                lbColorId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["MODEL_ID"].ToString();
                    string modelName = gvCust.DataKeys[index].Values["MODEL"].ToString();

                    // Gridview Color
                    lbModelId.Text = id;
                    tbModelName.Text = modelName;

                    BindGridColorData(id);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        private void BindGridColorData(string modelId)
        {
            try
            {
                if (modelId != "")
                {
                    CarModelService srv = new CarModelService();
                    gvColor.DataSource = srv.GetColorByModelId(modelId);
                    gvColor.DataBind();
                }
                else
                {
                    gvColor.DataSource = new DataTable();
                    gvColor.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvColor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvColor.PageIndex = e.NewPageIndex;
                string id = lbModelId.Text;
                if (id != "")
                {
                    BindGridColorData(id);
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvColor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Clear")
                {
                    lbDelColorModelId.Text = "";
                    lbDelColorId.Text = "";

                    int index = Convert.ToInt32(e.CommandArgument);
                    if (gvColor.DataKeys != null)
                    {
                        lbDelColorModelId.Text = gvColor.DataKeys[index].Values["MODEL_ID"].ToString();
                        lbDelColorId.Text = gvColor.DataKeys[index].Values["BODYCLR_CD"].ToString();

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmColor", "$('#mdDelConfirmColor').modal();", true);
                        upModalConfirmDeleteColor.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnDeleteColor_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdDelConfirmColor", "$('#mdDelConfirmColor').modal('hide');", true);
                upModalConfirmDeleteColor.Update();

                CarModelService srv = new CarModelService();
                srv.DeleteColor(lbDelColorModelId.Text, lbDelColorId.Text);

                BindGridColorData(lbDelColorModelId.Text);
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void gvColor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void gvBtnEditColor_Click(object sender, EventArgs e)
        {
            try
            {
                lbTypeColor.Text = "";
                lbModelId.Text = "";
                lbColorId.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvColor.DataKeys != null)
                {
                    string modelId = gvColor.DataKeys[index].Values["MODEL_ID"].ToString();
                    string colorId = gvColor.DataKeys[index].Values["BODYCLR_CD"].ToString();
                    string modleName = gvColor.DataKeys[index].Values["MODEL"].ToString();

                    lbTypeColor.Text = "Edit";
                    lbModelId.Text = modelId;
                    lbColorId.Text = colorId;
                    tbModelName.Text = modleName;

                    CarModelService srv = new CarModelService();
                    DataRow row = srv.GetColorByModelIdAndCode(modelId, colorId);
                    if (row != null)
                    {
                        tbColorId.Text = row["BODYCLR_CD"].ToString();
                        tbColorName.Text = row["BODYCLR_NAME"].ToString();

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddColor", "$('#mdAddColor').modal();", true);
                        upModalAddColor.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnSaveColor_Click(object sender, EventArgs e)
        {
            try
            {
                string user = Session["User"].ToString();
                string modelId = lbModelId.Text;
                string colorIdOld = lbColorId.Text;
                string colorIdNew = tbColorId.Text;
                string colorName = tbColorName.Text;

                CarModelService srv = new CarModelService();
                if (lbTypeColor.Text == "Add")
                {
                    srv.AddColor(modelId, colorIdNew, colorName);
                    BindGridColorData(modelId);
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbTypeColor.Text == "Edit")
                {
                    srv.UpdateColor(modelId, colorIdOld, colorIdNew, colorName);
                    BindGridColorData(modelId);
                    ClientPopup(ModalType.Success, "Completed");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
        protected void btnAddColor_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbModelId.Text != "")
                {
                    lbColorId.Text = "";
                    tbColorName.Text = "";
                    tbColorId.Text = "";
                    lbTypeColor.Text = "Add";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAddColor", "$('#mdAddColor').modal();", true);
                    upModalAddColor.Update();
                }
                else
                {
                    ClientPopup(ModalType.Error, "Please select Car Model");
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
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
            upModalAdd.Update();
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

                tbImage.Text = "";
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
                            fileUpload.SaveAs(imgDir + fileUpload.FileName);
                            tbImage.Text = fileUpload.FileName;
                            //if (img.Height == 1003 && img.Width == 1500)
                            //{

                            //}
                            //else
                            //{
                            //    throw new Exception("Allow file size 1500 x 1003 only!");
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

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                //upModalAdd.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}