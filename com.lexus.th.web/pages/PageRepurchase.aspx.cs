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
    public partial class PageRepurchase : System.Web.UI.Page
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

                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathRepurchase"];
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();
                        ViewState[Banner] = new List<UploadImageModel>();
                        ViewState[BannerDetail] = new List<UploadImageModel>();

                        BindPreferredModel();
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
                RepurchaseService srv = new RepurchaseService();
                gvCust.DataSource = srv.GetBooking(txtSearch.Text);
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

                DeleteImage(ImageType.Img1);
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
                BookingModel referral = new BookingModel()
                {
                    Id = lbId.Text,
                    TitleEN = TitleEN.Text,
                    TitleTH = TitleTH.Text,
                    DescEN = tbDesc.Text,
                    DescTH = tbDescTH.Text,
                    ConditionEN = tbCondition.Text,
                    ConditionTH = tbConditionTH.Text,
                    RegPeriodEN = tbRegPeriod.Text,
                    RegPeriodTH = tbRegPeriodTH.Text,
                    RegStart = tbRegPeriondStart.Text,
                    RegEnd = tbRegPeriondEnd.Text,
                    DisplayStart = tbDispStart.Text,
                    DisplayEnd = tbDispEnd.Text,
                    ThankyouMessageEN = tbThankYouMsgEN.Text,
                    ThankyouMessageTH = tbThankYouMsgTH.Text,
                    CodeMessageEN = tbCodemessageEN.Text,
                    CodeMessageTH = tbCodemessageTH.Text,
                    IsActive = dlActive.SelectedValue,
                    User = Session["User"].ToString(),
                    PreferredModelIds = json
                };

                DateTime DisStart = DateTime.ParseExact(referral.DisplayStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime DisEnd = DateTime.ParseExact(referral.DisplayEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime PerStart = DateTime.ParseExact(referral.RegStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime PerEnd = DateTime.ParseExact(referral.RegEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                List<UploadImageModel> banner = (List<UploadImageModel>)ViewState[Banner];
                List<UploadImageModel> bannerDetail = (List<UploadImageModel>)ViewState[BannerDetail];

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
                    else
                    {
                        RepurchaseService srv = new RepurchaseService();

                        if (lbType.Text == "Add")
                        {
                            srv.AddBooking(referral, banner, bannerDetail);
                            BindGridData();
                            ClientPopup(ModalType.Success, "Completed");
                        }
                        else if (lbType.Text == "Edit")
                        {
                            srv.UpdateBooking(referral, banner, bannerDetail);
                            BindGridData();
                            ClientPopup(ModalType.Success, "Completed");
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

                RepurchaseService srv = new RepurchaseService();
                srv.DeleteBooking(id, user);

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
                TitleTH.Text = "";
                TitleEN.Text = "";
                tbDesc.Text = "";
                tbDispStart.Text = "";
                tbDispEnd.Text = "";
                tbCondition.Text = "";
                tbRegPeriod.Text = "";
                tbRegPeriondStart.Text = "";
                tbRegPeriondEnd.Text = "";
                lbType.Text = "Add";
                tbDescTH.Text = "";
                tbConditionTH.Text = "";
                tbRegPeriodTH.Text = "";
                lbImg1.Text = "";
                lbImgBanner1.Text = "";
                tbThankYouMsgTH.Text = "";
                tbThankYouMsgEN.Text = "";
                tbCodemessageTH.Text = "";
                tbCodemessageEN.Text = "";
                dlActive.SelectedIndex = -1;
                pnPreferred.Visible = false;
                pnDetail.Visible = true;
                lbRequiredPreferredModel.Visible = false;

                ViewState[Banner] = new List<UploadImageModel>();
                ViewState[BannerDetail] = new List<UploadImageModel>();
                gvBanner.DataSource = (List<UploadImageModel>)ViewState[Banner];
                gvBanner.DataBind();
                gvBannerDetail.DataSource = (List<UploadImageModel>)ViewState[BannerDetail];
                gvBannerDetail.DataBind();
                InitialPreferred();
                // BindEventGroup();
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
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Attributes.Add("style", "white-space:nowrap;");
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
                dlActive.SelectedIndex = -1;
                pnPreferred.Visible = false;
                pnDetail.Visible = true;
                lbRequiredPreferredModel.Visible = false;
                InitialPreferred();
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    RepurchaseService srv = new RepurchaseService();
                    DataRow row = srv.GetBookingById(id);
                    if (row != null)
                    {
                        TitleEN.Text = row["title_en"].ToString();
                        TitleTH.Text = row["title_th"].ToString();
                        tbDispStart.Text = (row["display_start"] == DBNull.Value) ? "" : Convert.ToDateTime(row["display_start"]).ToString("dd/MM/yyyy HH:mm");
                        tbDispEnd.Text = (row["display_end"] == DBNull.Value) ? "" : Convert.ToDateTime(row["display_end"]).ToString("dd/MM/yyyy HH:mm");
                        tbCondition.Text = row["condition_en"].ToString();
                        tbConditionTH.Text = row["condition_th"].ToString();
                        tbRegPeriod.Text = row["reg_period_en"].ToString();
                        tbRegPeriodTH.Text = row["reg_period_th"].ToString();
                        tbRegPeriondStart.Text = (row["reg_start"] == DBNull.Value) ? "" : Convert.ToDateTime(row["reg_start"]).ToString("dd/MM/yyyy");
                        tbRegPeriondEnd.Text = (row["reg_end"] == DBNull.Value) ? "" : Convert.ToDateTime(row["reg_end"]).ToString("dd/MM/yyyy HH:mm");
                        tbDesc.Text = row["desc_en"].ToString();
                        tbDescTH.Text = row["desc_th"].ToString();
                        tbThankYouMsgEN.Text = row["thankyou_message_en"].ToString();
                        tbThankYouMsgTH.Text = row["thankyou_message_th"].ToString();
                        tbCodemessageEN.Text = row["code_message_en"].ToString();
                        tbCodemessageTH.Text = row["code_message_th"].ToString();
                        dlActive.SelectedValue = row["Is_active"].ToString().ToLower() == "true" ? "1" : "0";
                        List<PreferredModel> prefModel = JsonConvert.DeserializeObject<List<PreferredModel>>(row["Preferred_model_ids"].ToString());
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

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["ID"].ToString();

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
                                Page = "REPURCHASE",
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
                lbStatus.Text = ex.Message;// ex.Message;
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
                    RepurchaseService srv = new RepurchaseService();
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
                    RepurchaseService srv = new RepurchaseService();
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

        private void BindPreferredModel()
        {
            try
            {
                RepurchaseService srv = new RepurchaseService();
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

        protected void chkPrefferedModelSelectAll_CheckedChanged(object sender, EventArgs e)
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