using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.lexus.th.web.pages
{
    public partial class PageArticle : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        private enum ImageType { Img1, Img2, Img3, Img4, Img5, Img7 }

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
                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathArticle"];
                        //ViewState["ContentArticlePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ContentArticlePathConfig"];
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
                ArticleService srv = new ArticleService();
                gvCust.DataSource = srv.GetArticle(txtSearch.Text);
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
                if (lbImg2.Text != "")
                {
                    DeleteImage(ImageType.Img2);
                }
                if (lbImg3.Text != "")
                {
                    DeleteImage(ImageType.Img3);
                }
                if (lbImg4.Text != "")
                {
                    DeleteImage(ImageType.Img4);
                }
                if (lbImg5.Text != "")
                {
                    DeleteImage(ImageType.Img5);
                }
                if (lbImg7.Text != "")
                {
                    DeleteContent(ImageType.Img7);
                }

                string title = tbTitle.Text;
                string date = tbDate.Text;
                string url = tbURL.Text;
                string active = dlActive.SelectedValue;
                string image1 = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";
                string image2 = (tbImg2.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg2.Text) : "";
                string image3 = (tbImg3.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg3.Text) : "";
                string image4 = (tbImg4.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg4.Text) : "";
                string image5 = (tbImg5.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg5.Text) : "";
                string dispStart = tbDispStart.Text;
                string dispEnd = tbDispEnd.Text;
                string order = tbOrder.Text;
                string user = Session["User"].ToString();

                

                ArticleService srv = new ArticleService();
                if (lbType.Text == "Add")
                {
                    srv.AddArticle(title, date, url, image1, image2, image3, image4, image5, dispStart, dispEnd, user, active, order);
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateArticle(title, date, url, image1, image2, image3, image4, image5, dispStart, dispEnd, user, lbId.Text, active, order);
                    BindGridData();
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

                ArticleService srv = new ArticleService();
                srv.DeleteArticle(id, user);

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
                tbTitle.Text = "";
                tbDate.Text = "";
                tbURL.Text = "";
                dlActive.SelectedValue = "1";
                tbImg1.Text = "";
                tbImg2.Text = "";
                tbImg3.Text = "";
                tbImg4.Text = "";
                tbImg5.Text = "";
                tbImg7.Text = "";
                tbDispStart.Text = "";
                tbDispEnd.Text = "";
                tbOrder.Text = "";
                lbType.Text = "Add";

                lbImg1.Text = "";
                lbImg2.Text = "";
                lbImg3.Text = "";
                lbImg4.Text = "";
                lbImg5.Text = "";
                lbImg7.Text = "";

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

                lbImg1.Text = "";
                lbImg2.Text = "";
                lbImg3.Text = "";
                lbImg4.Text = "";
                lbImg5.Text = "";
                lbImg7.Text = "";

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["id"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    ArticleService srv = new ArticleService();
                    DataRow row = srv.GetArticleById(id);
                    if (row != null)
                    {
                        tbTitle.Text = row["topic_th"].ToString();
                        tbDate.Text = (row["topic_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["topic_date"]).ToString("dd/MM/yyyy");
                        tbURL.Text = row["topic_url"].ToString();
                        dlActive.SelectedValue = row["is_active"].ToString();
                        tbImg1.Text = ImageService.Split1(row["images1"].ToString());
                        tbImg2.Text = ImageService.Split1(row["images2"].ToString());
                        tbImg3.Text = ImageService.Split1(row["images3"].ToString());
                        tbImg4.Text = ImageService.Split1(row["images4"].ToString());
                        tbImg5.Text = ImageService.Split1(row["images5"].ToString());
                        tbDispStart.Text = (row["display_start_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["display_start_date"]).ToString("dd/MM/yyyy");
                        tbDispEnd.Text = (row["display_end_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["display_end_date"]).ToString("dd/MM/yyyy");
                        tbOrder.Text = row["order"].ToString();

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

        protected void UploadImage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                case "btnUpImg1":
                    UploadImage(ImageType.Img1);
                    break;
                case "btnUpImg2":
                    UploadImage(ImageType.Img2);
                    break;
                case "btnUpImg3":
                    UploadImage(ImageType.Img3);
                    break;
                case "btnUpImg4":
                    UploadImage(ImageType.Img4);
                    break;
                case "btnUpImg5":
                    UploadImage(ImageType.Img5);
                    break;
                case "btnUpImg7":
                    UploadContent(ImageType.Img7);
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
                case "btnDelImg2":
                    lbImg2.Text = ImageType.Img2.ToString();
                    //DeleteImage(ImageType.Img2);
                    break;
                case "btnDelImg3":
                    lbImg3.Text = ImageType.Img3.ToString();
                    //DeleteImage(ImageType.Img3);
                    break;
                case "btnDelImg4":
                    lbImg4.Text = ImageType.Img4.ToString();
                    //DeleteImage(ImageType.Img4);
                    break;
                case "btnDelImg5":
                    lbImg5.Text = ImageType.Img5.ToString();
                    //DeleteImage(ImageType.Img5);
                    break;
                case "btnDelImg7":
                    lbImg7.Text = ImageType.Img7.ToString();
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

                lbStatus.Text = "";

                if (fileUpload.HasFile)
                {
                    if (type.ToString() == "Img7")
                    {
                        string imgDir2 = WebConfigurationManager.AppSettings["ContentArticlePathConfig"] + "\\";
                        if (!Directory.Exists(imgDir2))
                        {
                            Directory.CreateDirectory(imgDir2);
                        }
                        if (tbImage.Text != "")
                        {
                            if (File.Exists(imgDir2 + tbImage.Text))
                                File.Delete(imgDir2 + tbImage.Text);
                        }

                        fileUpload.SaveAs(imgDir2 + fileUpload.FileName);
                        tbImage.Text = fileUpload.FileName;

                        tbURL.Text = WebConfigurationManager.AppSettings["ContentArticleURLConfig"] + fileUpload.FileName;
                    }
                    else
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
                lbStatus.Text = "Upload image error. Please try again later.";//ex.Message;
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

                if (tbImage.Text != "")
                {
                    if (File.Exists(imgDir + tbImage.Text))
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

        private void UploadContent  (ImageType type)
        {
            try
            {
                string imgDir = WebConfigurationManager.AppSettings["ContentArticlePathConfig"] + "\\";

                FileUpload fileUpload = upModalAdd.FindControl("up" + type.ToString()) as FileUpload;
                TextBox tbImage = upModalAdd.FindControl("tb" + type.ToString()) as TextBox;
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;

                lbStatus.Text = "";

                if (fileUpload.HasFile)
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

                    tbURL.Text = WebConfigurationManager.AppSettings["ContentArticleURLConfig"] + fileUpload.FileName;
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
                //ClientPopup(ModalType.Error, ex.Message);
                Label lbStatus = upModalAdd.FindControl("lb" + type.ToString()) as Label;
                lbStatus.Text = "Upload content error. Please try again later.";//ex.Message;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                upModalAdd.Update();
            }
        }

        private void DeleteContent(ImageType type)
        {
            try
            {
                string imgDir = ViewState["ContentArticlePath"] + "\\";

                TextBox tbImage = tbImg1;

                if (tbImage.Text != "")
                {
                    if (File.Exists(imgDir + tbImage.Text))
                        File.Delete(imgDir + tbImage.Text);
                    tbImage.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}