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
    public partial class PageConfig : System.Web.UI.Page
    {
        private enum ModalType { Success, Error, Warning }
        private enum ImageType { Img1, Img2, Img3, Img4, Img5, Img6, Img7, Img8, Img9, Img10, Img11, Img12 }
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
                        DataRow row = srv.GetAllConfig();
                        if (row != null)
                        {
                            //tbTiming.Text = row["otp_timing"].ToString();
                            //tbMaximumInput.Text = row["otp_maximum_input"].ToString();
                            tbCallCenterContact.Text = row["call_center_contact"].ToString();
                            tbCallCenterEmail.Text = row["call_center_email"].ToString();
                            TxtPushDay.Text = row["push_remaining_day"].ToString();
                            //tbImg1.Text = ImageService.Split1(row["picture_guest"].ToString());
                            //tbImg2.Text = ImageService.Split1(row["picture_blank_banner"].ToString());
                            tbImg3.Text = ImageService.Split1(row["picture_authorized_dealer"].ToString());
                            tbImg4.Text = ImageService.Split1(row["picture_authorized_service_dealer"].ToString());
                            //tbImg5.Text = ImageService.Split1(row["picture_blank_article"].ToString());
                            tbImg6.Text = ImageService.Split1(row["picture_upcoming"].ToString());
                            tbImg7.Text = ImageService.Split1(row["picture_news"].ToString());
                            //tbImg8.Text = ImageService.Split1(row["picture_blank_upcoming"].ToString());
                            //tbImg9.Text = ImageService.Split1(row["picture_blank_news"].ToString());
                            tbImg10.Text = ImageService.Split1(row["article_banner"].ToString());
                            tbImg11.Text = ImageService.Split1(row["service_appointment_banner"].ToString());
                            tbImg12.Text = ImageService.Split1(row["car_booking_banner"].ToString());
                            tbBannerSpeed.Text = ImageService.Split1(row["auto_slide_time"].ToString());
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
                //case "btnUpImg1":
                //    UploadImage(ImageType.Img1, btn);
                //    break;
                //case "btnUpImg2":
                //    UploadImage(ImageType.Img2, btn);
                //    break;
                case "btnUpImg3":
                    UploadImage(ImageType.Img3, btn);
                    break;
                case "btnUpImg4":
                    UploadImage(ImageType.Img4, btn);
                    break;
                //case "btnUpImg5":
                //    UploadImage(ImageType.Img5, btn);
                //    break;
                case "btnUpImg6":
                    UploadImage(ImageType.Img6, btn);
                    break;
                case "btnUpImg7":
                    UploadImage(ImageType.Img7, btn);
                    break;
                //case "btnUpImg8":
                //    UploadImage(ImageType.Img8, btn);
                //    break;
                //case "btnUpImg9":
                //    UploadImage(ImageType.Img9, btn);
                //    break;
                case "btnUpImg10":
                    UploadImage(ImageType.Img10, btn);
                    break;
                case "btnUpImg11":
                    UploadImage(ImageType.Img11, btn);
                    break;
                case "btnUpImg12":
                    UploadImage(ImageType.Img12, btn);
                    break;
            }
        }

        protected void DeleteImage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            switch (btn.ID)
            {
                //case "btnDelImg1":
                //    lbImg1.Text = ImageType.Img1.ToString();
                //    //DeleteImage(ImageType.Img1, btn);
                //    break;
                //case "btnDelImg2":
                //    lbImg2.Text = ImageType.Img2.ToString();
                //    //DeleteImage(ImageType.Img2, btn);
                //    break;
                case "btnDelImg3":
                    lbImg3.Text = ImageType.Img3.ToString();
                    //DeleteImage(ImageType.Img3, btn);
                    break;
                case "btnDelImg4":
                    lbImg4.Text = ImageType.Img4.ToString();
                    //DeleteImage(ImageType.Img4, btn);
                    break;
                //case "btnDelImg5":
                //    lbImg5.Text = ImageType.Img5.ToString();
                //    //DeleteImage(ImageType.Img5, btn);
                //    break;
                case "btnDelImg6":
                    lbImg6.Text = ImageType.Img6.ToString();
                    //DeleteImage(ImageType.Img6, btn);
                    break;
                case "btnDelImg7":
                    lbImg7.Text = ImageType.Img7.ToString();
                    //DeleteImage(ImageType.Img7, btn);
                    break;
                //case "btnDelImg8":
                //    lbImg8.Text = ImageType.Img8.ToString();
                //    //DeleteImage(ImageType.Img8, btn);
                //    break;
                //case "btnDelImg9":
                //    lbImg9.Text = ImageType.Img9.ToString();
                //    //DeleteImage(ImageType.Img9, btn);
                //    break;
                case "btnDelImg10":
                    lbImg10.Text = ImageType.Img10.ToString();
                    //DeleteImage(ImageType.Img10, btn);
                    break;
                case "btnDelImg11":
                    lbImg11.Text = ImageType.Img11.ToString();
                    break;
                case "btnDelImg12":
                    lbImg11.Text = ImageType.Img12.ToString();
                    break;
            }
        }

        private void UploadImage(ImageType type, LinkButton btn)
        {
            try
            {
                string imgDir = ViewState["ImagePath"] + "\\";

                FileUpload fileUpload = btn.Parent.FindControl("up" + type.ToString()) as FileUpload;
                TextBox tbImage = btn.Parent.FindControl("tb" + type.ToString()) as TextBox;
                Label lbStatus = btn.Parent.FindControl("lb" + type.ToString()) as Label;

                lbStatus.Text = "";

                if (fileUpload.HasFile)
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
                    throw new Exception("No file selected!");
                }
            }
            catch (Exception ex)
            {
                //ClientPopup(ModalType.Error, ex.Message);
            }
        }

        private void DeleteImage(ImageType type, LinkButton btn)
        {
            try
            {
                string imgDir = ViewState["ImagePath"] + "\\";

                TextBox tbImage = btn.Parent.FindControl("tb" + type.ToString()) as TextBox;

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
                LinkButton btn = sender as LinkButton;
                //if (lbImg1.Text != "")
                //{
                //    DeleteImage(ImageType.Img1, btn);
                //}
                //if (lbImg2.Text != "")
                //{
                //    DeleteImage(ImageType.Img2, btn);
                //}
                if (lbImg3.Text != "")
                {
                    DeleteImage(ImageType.Img3, btn);
                }
                if (lbImg4.Text != "")
                {
                    DeleteImage(ImageType.Img4, btn);
                }
                //if (lbImg5.Text != "")
                //{
                //    DeleteImage(ImageType.Img5, btn);
                //}
                if (lbImg6.Text != "")
                {
                    DeleteImage(ImageType.Img6, btn);
                }
                if (lbImg7.Text != "")
                {
                    DeleteImage(ImageType.Img7, btn);
                }
                //if (lbImg8.Text != "")
                //{
                //    DeleteImage(ImageType.Img8, btn);
                //}
                //if (lbImg9.Text != "")
                //{
                //    DeleteImage(ImageType.Img9, btn);
                //}
                if (lbImg10.Text != "")
                {
                    DeleteImage(ImageType.Img10, btn);
                }
                if (lbImg11.Text != "")
                {
                    DeleteImage(ImageType.Img11, btn);
                }
                if (lbImg12.Text != "")
                {
                    DeleteImage(ImageType.Img12, btn);
                }

                //string otp_timing = tbTiming.Text;
                //string otp_maximum_input = tbMaximumInput.Text;
                string call_center_contact = tbCallCenterContact.Text;
                string call_center_email = tbCallCenterEmail.Text;
                //string picture_guest = (tbImg1.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg1.Text) : "";
                //string picture_blank_banner = (tbImg2.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg2.Text) : "";
                string picture_authorized_dealer = (tbImg3.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg3.Text) : "";
                string picture_authorized_service_dealer = (tbImg4.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg4.Text) : "";
                //string picture_blank_article = (tbImg5.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg5.Text) : "";
                string picture_upcoming = (tbImg6.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg6.Text) : "";
                string picture_news = (tbImg7.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg7.Text) : "";
                //string picture_blank_upcoming = (tbImg8.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg8.Text) : "";
                //string picture_blank_news = (tbImg9.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg9.Text) : "";
                string article_banner = (tbImg10.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg10.Text) : "";
                string service_appointment_banner = (tbImg11.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg11.Text) : "";
                string car_booking_banner = (tbImg12.Text.Length > 0) ? ImageService.Split2(ViewState["ImagePath"].ToString() + "\\" + tbImg12.Text) : "";
                double auto_slide_time = Convert.ToDouble(tbBannerSpeed.Text);
                int remainingday =  ( TxtPushDay.Text =="") ?  0 : Convert.ToInt16(TxtPushDay.Text);
                
                string user = Session["User"].ToString();                

                ConfigService srv = new ConfigService();
                srv.UpdateAllConfig(picture_authorized_dealer, picture_authorized_service_dealer, picture_upcoming, picture_news, user, call_center_contact, call_center_email, article_banner, service_appointment_banner, car_booking_banner, auto_slide_time, remainingday);
                ClientPopup(ModalType.Success, "Completed");
            }
            catch (FormatException ex)
            {
                ClientPopup(ModalType.Error, "Banner Speed (Second) must be number.");
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