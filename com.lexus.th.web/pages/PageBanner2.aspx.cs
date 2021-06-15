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
    public partial class PageBanner2 : System.Web.UI.Page
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
                        ViewState["ImagePath"] = System.Web.Configuration.WebConfigurationManager.AppSettings["ImagePathBanner"];
                        
                        gvCust.DataSource = new System.Data.DataTable();
                        gvCust.DataBind();
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
                BannerService srv = new BannerService();
                gvCust.DataSource = srv.GetBanner();
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

                string order = tbOrder.Text;
                string type = dlType.SelectedValue;
                string action = dlAction.SelectedValue;
                string image1 = (tbImg1.Text.Length > 0) ? ImageService.Split3(ViewState["ImagePath"].ToString() + "\\" + type + "\\" + tbImg1.Text) : "";
                string dispStart = tbDispStart.Text;
                string dispEnd = tbDispEnd.Text;
                string user = Session["User"].ToString();

                BannerService srv = new BannerService();
                if (lbType.Text == "Add")
                {
                    srv.AddBanner(type, action, order, image1, dispStart, dispEnd, user);
                    BindGridData();
                    ClientPopup(ModalType.Success, "Completed");
                }
                else if (lbType.Text == "Edit")
                {
                    srv.UpdateBanner(type, action, order, image1, dispStart, dispEnd, user, lbId.Text);
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

                BannerService srv = new BannerService();
                srv.DeleteBanner(id, user);

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
                tbOrder.Text = "";
                dlType.SelectedIndex = -1;
                BindAction(dlType.SelectedValue);
                //dlAction.SelectedIndex = -1;
                tbImg1.Text = "";

                tbDispStart.Text = "";
                tbDispEnd.Text = "";
                lbType.Text = "Add";

                lbImg1.Text = "";

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
               

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                if (gvCust.DataKeys != null)
                {
                    string id = gvCust.DataKeys[index].Values["id"].ToString();
                    lbType.Text = "Edit";
                    lbId.Text = id;

                    BannerService srv = new BannerService();
                    DataRow row = srv.GetBannerById(id);
                    if (row != null)
                    {
                        //tbTitle.Text = row["title"].ToString();
                        tbOrder.Text = row["order"].ToString();
                        dlType.SelectedValue = row["type"].ToString();
                        BindActionEdit(dlType.SelectedValue);
                        dlAction.SelectedValue = row["action_id"].ToString();
                        tbImg1.Text = ImageService.Split1(row["image_url"].ToString());
                        tbDispStart.Text = (row["start_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["start_date"]).ToString("dd/MM/yyyy HH:mm");
                        tbDispEnd.Text = (row["end_date"] == DBNull.Value) ? "" : Convert.ToDateTime(row["end_date"]).ToString("dd/MM/yyyy HH:mm");

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
                string type2 = dlType.SelectedValue;
                string imgDir = ViewState["ImagePath"] + "\\" + type2 + "\\";

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
                    //if (fileUpload.PostedFile.ContentType == "image/jpeg")
                    //{
                    //    using (System.Drawing.Image img = System.Drawing.Image.FromStream(fileUpload.PostedFile.InputStream))
                    //    {
                    //        if (img.Height == 764 && img.Width == 1500)
                    //        {

                    //        }
                    //        else
                    //        {
                    //            throw new Exception("Allow file size 1500 x 764 only!");
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    throw new Exception("Allow file type JPG only!");
                    //}
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
                string type2 = dlType.SelectedValue;
                string imgDir = ViewState["ImagePath"] + "\\" + type2 + "\\";

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

        protected void dlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dlType.SelectedValue == "Events")
            {
                dlAction.Enabled = true;
                BindAction(dlType.SelectedValue);
            }
            if (dlType.SelectedValue == "News")
            {
                dlAction.Enabled = true;
                BindAction(dlType.SelectedValue);
            }
            if (dlType.SelectedValue == "Privilege")
            {
                dlAction.Enabled = true;
                BindAction(dlType.SelectedValue);
            }
            if (dlType.SelectedValue == "Main_Online_Booking")
            {
                dlAction.Items.Clear();
                dlAction.Enabled = false;
            }
            if (dlType.SelectedValue == "Online_Booking_Repurchase")
            {
                dlAction.Enabled = true;
                BindAction(dlType.SelectedValue);
            }
            if (dlType.SelectedValue == "Online_Booking_Referral")
            {
                dlAction.Enabled = true;
                BindAction(dlType.SelectedValue);
            }
            if (dlType.SelectedValue == "Online_Booking_Guest")
            {
                dlAction.Enabled = true;
                BindAction(dlType.SelectedValue);
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mdAdd", "$('#mdAdd').modal();", true);
            upModalAdd.Update();
        }

        private void BindAction(string actionType)
        {
            try
            {
                BannerService srv = new BannerService();
                using (DataTable action = srv.GetAction(actionType)) // If dealer get seller from dealer else get all seller
                {
                    dlAction.Items.Clear();
                    dlAction.DataSource = action;
                    dlAction.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindActionEdit(string actionType)
        {
            try
            {
                BannerService srv = new BannerService();
                using (DataTable action = srv.GetActionEdit(actionType)) // If dealer get seller from dealer else get all seller
                {
                    dlAction.Items.Clear();
                    dlAction.DataSource = action;
                    dlAction.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}