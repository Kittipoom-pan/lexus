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
    public partial class PageTestDrive : System.Web.UI.Page
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
                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathConfig"];
                        ConfigService srv = new ConfigService();
                        DataRow row = srv.GetConfigTestDrive();
                        if (row != null)
                        {
                            tbImg1.Text = ImageService.Split1(row["test_drive_banner"].ToString());
                            tbURLAddress.Text = row["test_drive_url_address"].ToString();
                        }
                    }
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
        }

        private void UploadImage(ImageType type)
        {
            try
            {
                string imgDir = ViewState["ImagePath"] + "\\";

                FileUpload fileUpload = upImg1;
                TextBox tbImage = tbImg1;
                Label lbStatus = lbImg1;

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
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private void DeleteImage(ImageType type)
        {
            try
            {
                string imgDir = ViewState["ImagePath"] + "\\";

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbImg1.Text != "")
                {
                    DeleteImage(ImageType.Img1);
                }

                string test_drive_banner = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";
                string test_drive_url_address = tbURLAddress.Text;
                string user = Session["User"].ToString();

                ConfigService srv = new ConfigService();
                srv.UpdateConfigTestDrive(test_drive_banner, test_drive_url_address, user);
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //string image1 = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";
                //string image2 = (tbImg2.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg2.Text) : "";
                //string image3 = (tbImg3.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg3.Text) : "";
                string user = Session["User"].ToString();

                //NewsService srv = new NewsService();
                //if (lbType.Text == "Add")
                //{
                //    srv.AddNews(title, date, desc, image1, image2, image3, image4, image5, dispStart, dispEnd, user);
                //    BindGridData();
                //    ClientPopup(ModalType.Success, "Completed");
                //}
                //else if (lbType.Text == "Edit")
                //{
                //    srv.UpdateNews(title, date, desc, image1, image2, image3, image4, image5, dispStart, dispEnd, user, lbId.Text);
                //    BindGridData();
                //    ClientPopup(ModalType.Success, "Completed");
                //}
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
    }
}