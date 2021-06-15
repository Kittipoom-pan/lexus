using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace com.lexus.th.web.pages
{
    public partial class PageBanner : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        private enum ImageType { Img1, Img2, Img3 }
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

                    dlType.Items.Add(new ListItem { Text = "None", Value = "None" });
                    dlType.Items.Add(new ListItem { Text = "Privilege", Value = "Privilege" });
                    dlType.Items.Add(new ListItem { Text = "Event", Value = "Event" });
                    dlType.Items.Add(new ListItem { Text = "News", Value = "News" });
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
        }

        protected void itemSelected(object sender, EventArgs e)
        {
            if (dlType.SelectedValue != "None")
            {
                dlName.Enabled = true;
                BindName(dlType.SelectedValue);
                if(dlName.Items.Count > 1)
                {
                    dlName.SelectedIndex = 1;
                }
                //dlName.SelectedIndex = 1;
            }
            if (dlType.SelectedValue == "None")
            {
                dlName.Enabled = false;
                dlName.SelectedIndex = 0;
            }
        }
        private void BindName(string type)
        {
            try
            {
                NameForTypeService srv = new NameForTypeService();
                DataTable seller = srv.GetName(type);
                if(seller.Rows.Count > 0)
                {
                    dlName.DataSource = seller;
                    dlName.DataBind();

                    //string url_image = 
                    //lbImg1
                }
                //else
                //{
                //    dlName.SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
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
                    DeleteImage(ImageType.Img1);
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
                    if (fileUpload.PostedFile.ContentType == "image/jpeg")
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
                        throw new Exception("Allow file type JPG only!");
                    }
                }
                else
                {
                    throw new Exception("No file selected!");
                }

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
                //upModalAdd.Update();
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
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }
    }
}